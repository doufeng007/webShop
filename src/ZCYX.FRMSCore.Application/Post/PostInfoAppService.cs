using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Application.Services;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.Organizations;
using ZCYX.FRMSCore.Extensions;
using System.Collections;
using ZCYX.FRMSCore.Authorization.Users;


namespace ZCYX.FRMSCore.Application
{
    public class PostInfoAppService : AsyncCrudAppService<PostInfo, PostInfoDto, Guid, PagedResultRequestDto, CreatePostInfoDto, PostInfoDto>, IPostInfoAppService
    {
        private readonly IRepository<PostInfo, Guid> _repository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _orgRepository;

        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitsRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<User, long> _useRepository;
        private readonly IRepository<UserPosts, Guid> _userPostrepository;
       
        public PostInfoAppService(IRepository<PostInfo, Guid> repository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitsRepository,
            IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository, IRepository<WorkFlowOrganizationUnits, long> orgRepository
            , OrganizationUnitManager organizationUnitManager, IRepository<User, long> useRepository, IRepository<UserPosts, Guid> userPostrepository)
            : base(repository)
        {
            _repository = repository;
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _orgRepository = orgRepository;
            _organizationUnitManager = organizationUnitManager;
            _userOrganizationUnitsRepository = userOrganizationUnitsRepository;
            _useRepository = useRepository;
            _userPostrepository = userPostrepository;
        }

        public override async Task<PostInfoDto> Update(PostInfoDto input)
        {
            var model = await _repository.GetAsync(input.Id);
            model.Name = input.Name;
            model.Summary = input.Summary;
            await _repository.UpdateAsync(model);
            return model.MapTo<PostInfoDto>();
        }

        public async Task<PostInfoDto> GetByNameAsync(string name)
        {
            if (name.IsNullOrWhiteSpace()) return null;
            var model = await _repository.FirstOrDefaultAsync(r => r.Name == name);
            if (model == null)
                return null;
            else
                return model.MapTo<PostInfoDto>();
        }


        /// <summary>
        /// 获取部门下的岗位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationUnitPostsDto>> GetOrgPosts(GetOrganizationUnitPostsListInput input)
        {
            var query = from a in _organizationUnitPostsRepository.GetAll()
                        join b in _repository.GetAll() on a.PostId equals b.Id
                        join c in _orgRepository.GetAll() on a.OrganizationUnitId equals c.Id
                        //join d in _userPostrepository.GetAll() on a.Id equals d.OrgPostId into e
                        //where a.OrganizationUnitId == input.OrgId
                        select new OrganizationUnitPostsDto { Id = a.Id, PostId = b.Id, PostName = b.Name, CreationTime = a.CreationTime,
                            OrganizationUnitId = a.OrganizationUnitId, OrganizationName = c.DisplayName,  PrepareNumber=a.PrepareNumber, Level= (int)a.Level,
                             Number=0,
                            TenantId = a.TenantId };
            if (input.OrgIds != null && input.OrgIds.Count > 0)
            {
                query = query.Where(ite => input.OrgIds.Contains(ite.OrganizationUnitId));
            }
            else
            {
                query = query.Where(ite => ite.OrganizationUnitId == input.OrgId);
            }
            var totalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var d in data) {
                var userp = (from a in _userPostrepository.GetAll()
                            join b in _useRepository.GetAll() on a.UserId equals b.Id
                            where a.OrgPostId == d.Id                           
                            select a.UserId).Distinct().ToList();
                d.Number = userp.Count();
            }
            return new PagedResultDto<OrganizationUnitPostsDto>(totalCount, data);
        }

        /// <summary>
        /// 获取人员所在所有部门的所有岗位
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationUnitPostsDto>> GetMyOrgPosts(long userId)
        {
            var query = from a in _organizationUnitPostsRepository.GetAll()
                        join b in _userOrganizationUnitsRepository.GetAll() on a.OrganizationUnitId equals b.OrganizationUnitId
                        join c in _repository.GetAll() on a.PostId equals c.Id
                        join d in _orgRepository.GetAll() on a.OrganizationUnitId equals d.Id
                        where b.UserId == userId
                        select new OrganizationUnitPostsDto { Id = a.Id, PostId = a.PostId, PostName = c.Name, CreationTime = a.CreationTime, OrganizationUnitId = a.OrganizationUnitId, OrganizationName = d.DisplayName, TenantId = a.TenantId };

            var totalCount = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).ToListAsync();
            return new PagedResultDto<OrganizationUnitPostsDto>(totalCount, data);
        }
        /// <summary>
        /// 获取部门下面职位的全称
        /// </summary>
        /// <param name="orgPostId"></param>
        /// <returns></returns>
        public string GetNameWithOrgByOrgPostId(Guid orgPostId)
        {
            var orgPostModel = _organizationUnitPostsRepository.Get(orgPostId);
            var postModel = _repository.Get(orgPostModel.PostId);
            var orgModel = _orgRepository.Get(orgPostModel.OrganizationUnitId);
            var parentOrgIds = orgModel.Code.Split(',').ToList();
            var p_orgs = from o in _orgRepository.GetAll()
                         where parentOrgIds.Contains(o.Id.ToString())
                         select o;
            var p_orgModels = p_orgs.OrderBy(r => r.Code.Length).ToList();
            var result = $"{ string.Join("/", p_orgModels.Select(r => r.DisplayName))}/{postModel.Name}";
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgPostId"></param>
        /// <returns></returns>
        public string GetUserNameByOrgPostId(Guid orgPostId)
        {
            var query = from u in _useRepository.GetAll()
                        join up in _userPostrepository.GetAll() on u.Id equals up.UserId
                        where u.IsDeleted == false && u.IsActive == true && up.IsDeleted == false && up.OrgPostId == orgPostId
                        select u;
            var userNames = "";
            query = query.Distinct();
            if (query.Count() > 0)
                userNames = string.Join(",", query.Select(r => r.Name).ToList());
            var orgPostModel = _organizationUnitPostsRepository.Get(orgPostId);
            var postModel = _repository.Get(orgPostModel.PostId);
            var orgModel = _orgRepository.Get(orgPostModel.OrganizationUnitId);
            var parentOrgIds = orgModel.Code.Split(',').ToList();
            var p_orgs = from o in _orgRepository.GetAll()
                         where parentOrgIds.Contains(o.Id.ToString())
                         select o;
            var p_orgModels = p_orgs.OrderBy(r => r.Code.Length).ToList();
            var result = $"{ string.Join("/", p_orgModels.Select(r => r.DisplayName))}/{postModel.Name}";
            result = $"{result}[{userNames}]";
            return result;
        }

        public async Task<List<OrgPostAllNameDto>> GetPostInfoWithOrgName()
        {
            var ret = new List<OrgPostAllNameDto>();
            var query = from orgPost in _organizationUnitPostsRepository.GetAll()
                        join org in _orgRepository.GetAll() on orgPost.OrganizationUnitId equals org.Id
                        join post in _repository.GetAll() on orgPost.PostId equals post.Id
                        let parent_orgs = (from o in _orgRepository.GetAll()
                                           where org.Code.GetStrContainsArrayWithChar(org.Code, ".")
                                           orderby o.Code.Length
                                           select o)
                        select new
                        {
                            orgPost,
                            org,
                            post,
                            parent_orgs
                        };
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                var postEntity = new OrgPostAllNameDto()
                {
                    IsPost = true,
                    Name = item.post.Name,
                    PostId = item.post.Id,
                    OrgPostId = item.orgPost.Id,
                    Pid = item.org.Id.ToString(),
                    Id = item.orgPost.Id.ToString(),
                     Level=item.orgPost.Level,
                };
                ret.Add(postEntity);
                if (ret.Any(r => r.Id == item.org.Id.ToString()))
                    continue;
                var orgEntity = new OrgPostAllNameDto()
                {
                    IsPost = false,
                    Name = item.org.DisplayName,
                    OrgId = item.org.Id,
                    Pid = item.org.ParentId.ToString(),
                    Id = item.org.Id.ToString(),
                    Level = item.orgPost.Level,
                };
                ret.Add(orgEntity);
                foreach (var item_ParentOrg in item.parent_orgs)
                {
                    if (ret.Any(r => r.Id == item_ParentOrg.Id.ToString()))
                        continue;
                    var orgEntity_p = new OrgPostAllNameDto()
                    {
                        IsPost = false,
                        Name = item_ParentOrg.DisplayName,
                        OrgId = item_ParentOrg.Id,
                        Pid = item_ParentOrg.ParentId.ToString(),
                        Id = item_ParentOrg.Id.ToString(),
                        Level = item.orgPost.Level,
                    };
                    ret.Add(orgEntity_p);
                }
            }
            return ret;
        }
    }


    public class OrgPostAllNameDtoCompare : IEqualityComparer<OrgPostAllNameDto>
    {
        public bool Equals(OrgPostAllNameDto x, OrgPostAllNameDto y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(OrgPostAllNameDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
