using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.Extensions;
using Abp.UI;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Roles;

namespace ZCYX.FRMSCore.Application
{
    public class OrganizationUnitPostsAppService : FRMSCoreAppServiceBase, IOrganizationUnitPostsAppService
    {
        private readonly IRepository<OrganizationUnitPosts, Guid> _repository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<PostInfo, Guid> _postInforepository;
        private readonly IRepository<UserPosts, Guid> _userPostsrepository;
        private readonly IRepository<OrganizationUnitPostsRole, Guid> _organizationUnitPostsRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<User, long> _useRepository;
        private readonly RoleManager _roleManager;
        public OrganizationUnitPostsAppService(IRepository<OrganizationUnitPosts, Guid> repository, IRepository<OrganizationUnitPostsRole, Guid> organizationUnitPostsRoleRepository
            , IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<PostInfo, Guid> postInforepository, RoleManager roleManager
            , IRepository<UserPosts, Guid> userPostsrepository, IRepository<Role> roleRepository, IRepository<User, long> useRepository)
        {
            this._repository = repository;
            _organizationUnitRepository = organizationUnitRepository;
            _postInforepository = postInforepository;
            _userPostsrepository = userPostsrepository;
            _organizationUnitPostsRoleRepository = organizationUnitPostsRoleRepository;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _useRepository = useRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationUnitPostsListOutputDto>> GetList(GetOrganizationUnitPostsBianzhiListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                        join c in _postInforepository.GetAll() on a.PostId equals c.Id

                        //where a.Status == -1
                        select new OrganizationUnitPostsListOutputDto()
                        {
                            Id = a.Id,
                            OrganizationUnitId = a.OrganizationUnitId,
                            OrganizationUnitName = b.DisplayName,
                            PostId = a.PostId,
                            PostName = c.Name,
                            PrepareNumber = a.PrepareNumber,
                            //NowNumber = userCount.Count(),
                            CreationTime = a.CreationTime
                        };
            query = query.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), r => r.OrganizationUnitName.Contains(input.SearchKey) || r.PostName.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var x in ret)
            {
                var userCount = (from e in _userPostsrepository.GetAll()
                                 join f in _useRepository.GetAll() on e.UserId equals f.Id
                                 where e.OrgPostId == x.Id
                                 select e.UserId).Distinct().ToList();
                x.NowNumber = userCount.Count();
            }
            return new PagedResultDto<OrganizationUnitPostsListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取岗位实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<OrganizationUnitPostsOutputDto> Get(NullableIdDto<Guid> input)
        {
            var model = (from a in _repository.GetAll()
                         join b in _postInforepository.GetAll() on a.PostId equals b.Id
                         where a.Id == input.Id.Value
                         select new OrganizationUnitPostsOutputDto()
                         {
                             CreationTime = a.CreationTime,
                             Id = a.Id,
                             Level = (int)a.Level,
                             OrganizationUnitId = a.OrganizationUnitId,
                             PostId = a.PostId,
                             PostName = b.Name,
                             PrepareNumber = a.PrepareNumber,
                         }).FirstOrDefault();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<OrganizationUnitPostsOutputDto>();
            var rolenames = (from a in _organizationUnitPostsRoleRepository.GetAll()
                             join b in _roleRepository.GetAll() on a.RoleName equals b.Name
                             where a.OrgPostId == model.Id
                             select new { a.RoleName, b.IsStatic, b.Id }).ToList();

            ret.RoleNames = rolenames.Where(ite => ite.IsStatic).Select(ite => ite.RoleName).ToList();
            var r = rolenames.FirstOrDefault(ite => ite.IsStatic == false);
            if (r != null)
            {
                ret.RoleId = r.Id;
            }
            return ret;
        }
        /// <summary>
        /// 添加一个岗位信息
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Create(CreateOrganizationUnitPostsInput input)
        {

            var postid = _postInforepository.InsertAndGetId(new PostInfo()
            {
                Name = input.Name,
            });
            var newmodel = new OrganizationUnitPosts()
            {
                Id = Guid.NewGuid(),
                OrganizationUnitId = input.OrganizationUnitId,
                PostId = postid,
                PrepareNumber = input.PrepareNumber,
                Level = (Level)input.Level
            };
            if (input.RoleNames != null && input.RoleNames.Count > 0)
            {
                foreach (var r in input.RoleNames)
                {
                    var role2 = _roleRepository.GetAll().FirstOrDefault(ite => ite.Name == r);
                    if (role2 != null)
                    {
                        var tmp = new OrganizationUnitPostsRole()
                        {
                            OrgPostId = newmodel.Id,
                            RoleName = r
                        };
                        _organizationUnitPostsRoleRepository.Insert(tmp);
                    }
                }
            }

            var role = _roleRepository.GetAll().FirstOrDefault(ite => ite.Id == input.RoleId);
            if (role != null)
            {
                var tmp = new OrganizationUnitPostsRole()
                {
                    OrgPostId = newmodel.Id,
                    RoleName = role.Name
                };
                _organizationUnitPostsRoleRepository.Insert(tmp);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请先设置岗位权限。");
            }

            await _repository.InsertAsync(newmodel);
        }

        /// <summary>
        /// 修改一个岗位信息
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateOrganizationUnitPostsInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var post = _postInforepository.Get(dbmodel.PostId);
                if (post.Name != input.Name)
                {
                    post.Name = input.Name;
                    _postInforepository.Update(post);
                }
                dbmodel.PrepareNumber = input.PrepareNumber;
                dbmodel.Level = (Level)input.Level;


                var delroles = (from a in _organizationUnitPostsRoleRepository.GetAll()
                                join b in _roleRepository.GetAll() on a.RoleName equals b.Name
                                where a.OrgPostId == input.Id && b.IsStatic
                                select new
                                {
                                    b.IsStatic,
                                    a.RoleName,
                                    a.Id
                                }).ToList();
                foreach (var r in delroles)
                {
                    _organizationUnitPostsRoleRepository.Delete(ite => ite.Id == r.Id);
                }
                foreach (var r in input.RoleNames)
                {
                    _organizationUnitPostsRoleRepository.Insert(new OrganizationUnitPostsRole()
                    {
                        OrgPostId = input.Id,
                        RoleName = r,
                    });
                }
                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            var rolerel = _organizationUnitPostsRoleRepository.GetAll().Where(ite => ite.OrgPostId == input.Id).Select(ite => ite.RoleName);
            var role = _roleRepository.GetAll().Where(ite => ite.IsStatic == false && rolerel.Contains(ite.Name)).ToList();
            if (role != null && role.Count > 0)
            {
                foreach (var a in role)
                {
                    await _roleManager.DeleteAsync(a);
                }
            }
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }
        /// <summary>
        /// 修复数据（对每个部门默认添加“分管领导”和“部门领导”岗位）
        /// </summary>
        /// <returns></returns>
        public async Task CreateDefaultPost(long? orgid)
        {
            var query = _organizationUnitRepository.GetAll();
            if (orgid.HasValue)
            {
                query = query.Where(ite => ite.Id == orgid.Value);
            }
            var orgs = query.ToList();
            var role = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRoleAppService>();
            foreach (var org in orgs)
            {
                var orgpost = _repository.GetAll().Where(ite => ite.OrganizationUnitId == org.Id).ToList();
                if (orgpost.Exists(ite => ite.Level == Level.分管领导) == false)
                {
                    var roleid = await role.Create(new Roles.Dto.CreateRoleDto()
                    {
                        DisplayName = org.DisplayName + "分管领导角色",
                        Permissions = new List<string>()
                    });
                    CurrentUnitOfWork.SaveChanges();
                    await Create(new CreateOrganizationUnitPostsInput()
                    {
                        Level = (int)Level.分管领导,
                        Name = org.DisplayName + "分管领导",
                        OrganizationUnitId = org.Id,
                        PrepareNumber = 2,
                        RoleNames = new List<string>() { "OrgFGLD" },
                        RoleId = roleid.Id
                    });
                }
                else
                {
                    var chargeOrgPost = orgpost.FirstOrDefault(ite => ite.Level == Level.分管领导);
                    if (!_organizationUnitPostsRoleRepository.GetAll().Any(r => r.OrgPostId == chargeOrgPost.Id && r.RoleName == "OrgFGLD"))
                    {
                        var tmp = new OrganizationUnitPostsRole()
                        {
                            OrgPostId = chargeOrgPost.Id,
                            RoleName = "OrgFGLD"
                        };
                        _organizationUnitPostsRoleRepository.Insert(tmp);
                    }
                }
                if (orgpost.Exists(ite => ite.Level == Level.部门领导) == false)
                {
                    var roleid = await role.Create(new Roles.Dto.CreateRoleDto()
                    {
                        DisplayName = org.DisplayName + "部门领导角色",
                        Permissions = new List<string>()
                    });
                    CurrentUnitOfWork.SaveChanges();
                    await Create(new CreateOrganizationUnitPostsInput()
                    {
                        Level = (int)Level.部门领导,
                        Name = org.DisplayName + "部门领导",
                        OrganizationUnitId = org.Id,
                        PrepareNumber = 2,
                        RoleNames = new List<string>() { "DLEADER" },
                        RoleId = roleid.Id
                    });
                }
            }
        }
    }
}