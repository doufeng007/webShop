using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore;
using Abp.Domain.Repositories;
using Abp.Authorization.Users;
using Abp.Organizations;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;
using Dapper;
using Abp.Extensions;
using ZCYX.FRMSCore.Authorization.Roles;
using Abp.Domain.Uow;

using Abp.Authorization;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Roles;

namespace ZCYX.FRMSCore.Application
{
    public class WorkFlowOrganizationUnitsAppService : ApplicationService, IWorkFlowOrganizationUnitsAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostrepository;
        private readonly IRepository<PostInfo, Guid> _postInforepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IRepository<UserPosts, Guid> _userPostsrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<OrganizationUnitPostsRole, Guid> _organizationUnitPostsRoleRepository;

        public WorkFlowOrganizationUnitsAppService(OrganizationUnitManager organizationUnitManager,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository,
            IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<User, long> userRepository, UserManager userManager,
            IRepository<OrganizationUnitPostsRole, Guid> organizationUnitPostsRoleRepository,
            RoleManager roleManager,
            WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager,
        IDynamicRepository dynamicRepository, IRepository<OrganizationUnitPosts, Guid> organizationUnitPostrepository, IRepository<PostInfo, Guid> postInforepository
            , IRepository<UserPosts, Guid> userPostsrepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _dynamicRepository = dynamicRepository;
            _organizationUnitPostrepository = organizationUnitPostrepository;
            _postInforepository = postInforepository;
            _userManager = userManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _roleManager = roleManager;
            _userPostsrepository = userPostsrepository;
            _unitOfWorkManager = unitOfWorkManager;
            _organizationUnitPostsRoleRepository = organizationUnitPostsRoleRepository;
        }
        public async Task AddUserToOrganizationUnit(UserToWorkFlowOrganizationUnitInput input)
        {
            var abpOrganizationUnitsManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<AbpOrganizationUnitsManager>();
            await abpOrganizationUnitsManager.AddToOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        public async Task<WorkFlowOrganizationUnitDto> CreateOrganizationUnit(CreateWorkFlowOrganizationUnitInput input)
        {
            var organizationUnit = new WorkFlowOrganizationUnits();
            organizationUnit.TenantId = AbpSession.TenantId;
            organizationUnit.ParentId = input.ParentId;
            organizationUnit.DisplayName = input.DisplayName;
            organizationUnit.Type = input.Type;
            //organizationUnit.ChargeLeader = input.ChargeLeader;
            organizationUnit.Note = input.Note;
            organizationUnit.Code = await _organizationUnitManager.GetNextChildCodeAsync(organizationUnit.ParentId);
            await ValidateOrganizationUnitAsync(organizationUnit);
            if (input.Posts.Select(r => r.PostName).Distinct().Count() != input.Posts.Count())
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "部门的岗位名称不能重复");

            var orgId = await _organizationUnitRepository.InsertAndGetIdAsync(organizationUnit);
            
            foreach (var item in input.Posts)
            {
                var orgpostid = Guid.NewGuid();
                if (item.PostId.HasValue)
                {
                    var entity = new OrganizationUnitPosts() { Id = orgpostid, OrganizationUnitId = orgId, PostId = item.PostId.Value, PrepareNumber = item.PrepareNumber };
                    await _organizationUnitPostrepository.InsertAsync(entity);
                }
                else
                {
                    var postService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();

                    var exit_ModelByName = await postService.GetByNameAsync(item.PostName);
                    if (exit_ModelByName != null)
                    {
                        var entity = new OrganizationUnitPosts() { Id = orgpostid, OrganizationUnitId = orgId, PostId = exit_ModelByName.Id, PrepareNumber = item.PrepareNumber, Level = item.Level };
                        await _organizationUnitPostrepository.InsertAsync(entity);
                    }
                    else
                    {
                        var new_post = new PostInfo() { Id = Guid.NewGuid(), Name = item.PostName };
                        await _postInforepository.InsertAsync(new_post);
                        var entity = new OrganizationUnitPosts() { Id = Guid.NewGuid(), OrganizationUnitId = orgId, PostId = new_post.Id, PrepareNumber = item.PrepareNumber,Level=item.Level };
                        await _organizationUnitPostrepository.InsertAsync(entity);
                    }
                }

                // 创建岗位角色关联
                if (item.RoleNames == null || item.RoleNames.Count == 0)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "岗位至少需要关联一个角色。");
                }
                foreach (var r in item.RoleNames)
                {
                    _organizationUnitPostsRoleRepository.Insert(new OrganizationUnitPostsRole()
                    {
                        Id = Guid.NewGuid(),
                        OrgPostId = orgpostid,
                        RoleName = r
                    });
                }

            }

            await CurrentUnitOfWork.SaveChangesAsync();
            //创建部门默认岗位
            var posts = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IOrganizationUnitPostsAppService>();
            await posts.CreateDefaultPost(orgId);
            return organizationUnit.MapTo<WorkFlowOrganizationUnitDto>();
        }

        public async Task<WorkFlowOrganizationUnitDto> GetAsync(EntityDto<long> input)
        {
            var model = await _organizationUnitRepository.GetAsync(input.Id);
            var ret = model.MapTo<WorkFlowOrganizationUnitDto>();
            //var posts_query = from a in _postInforepository.GetAll()
            //                  join b in _organizationUnitPostrepository.GetAll() on a.Id equals b.PostId
            //                  join c in _organizationUnitPostsRoleRepository.GetAll() on b.Id equals c.OrgPostId into d
            //                  where b.OrganizationUnitId == input.Id
            //                  select new { PostId = a.Id, PostName = a.Name, OrgPostId = b.Id, b.PrepareNumber,b.Level,d };

            var posts_query = from a in _organizationUnitPostrepository.GetAll()
                              join b in _postInforepository.GetAll() on a.PostId equals b.Id
                              //join c in _organizationUnitPostsRoleRepository.GetAll() on a.Id equals c.OrgPostId into g
                              where a.OrganizationUnitId == input.Id
                              select new 
                              { PostId = a.PostId, PostName = b.Name, OrgPostId = a.Id, a.PrepareNumber, a.Level,// g
                              };

            var posts = await posts_query.ToListAsync();
            foreach (var item in posts)
            {
                var rolenames = from a in _organizationUnitPostsRoleRepository.GetAll()
                                where a.OrgPostId==item.OrgPostId
                                select a.RoleName;
                var entity = new OrgPostInfoDto()
                {
                    Id = item.OrgPostId,
                    PrepareNumber = item.PrepareNumber,
                    PostId = item.PostId,
                    PostName = item.PostName,
                    Level = (int)item.Level,
                    RoleNames = rolenames.ToList()
                };
                ret.Posts.Add(entity);
            }

            return ret;
        }

        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            var hasuser = _workFlowOrganizationUnitsManager.GetAllUsersById(input.Id);
            if (hasuser != null && hasuser.Count > 0)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请先将部门下面的所有人员移动到别的部门后再执行该操作。");
            }
            await _organizationUnitManager.DeleteAsync(input.Id);
        }

        public async Task<ListResultDto<WorkFlowOrganizationUnitDto>> GetOrganizationUnits()
        {
            var query =
               from ou in _organizationUnitRepository.GetAll()
               join uou in _userOrganizationUnitRepository.GetAll() on ou.Id equals uou.OrganizationUnitId into g
               orderby ou.Sort
               select new { ou, memberCount = g.Count() };

            var items = await query.ToListAsync();

            return new ListResultDto<WorkFlowOrganizationUnitDto>(
                items.Select(item =>
                {
                    var dto = item.ou.MapTo<WorkFlowOrganizationUnitDto>();
                    dto.MemberCount = item.memberCount;
                    return dto;
                }).ToList());
        }


        public async Task<PagedResultDto<WorkFlowOrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetWorkFlowOrganizationUnitUsersInput input)
        {
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManager>();
            var query = from uou in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                        join user in userManager.Users on uou.UserId equals user.Id
                        where uou.OrganizationUnitId == input.Id && user.IsActive
                        select new { uou, user };

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<WorkFlowOrganizationUnitUserListDto>(
                totalCount,
                items.Select(item =>
                {
                    var dto = item.user.MapTo<WorkFlowOrganizationUnitUserListDto>();
                    dto.Organization = new List<SimpleOrganizationDto>();
                    dto.AddedTime = item.uou.CreationTime;
                    return dto;
                }).ToList());
        }
        ///// <summary>
        ///// 获取当前部门下的所有人员（包括子部门）
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<PagedResultDto<WorkFlowOrganizationUnitUserListDto>> GetOrganizationUnitUsersAndUnder(GetWorkFlowOrganizationUnitUsersInput input)
        //{
        //    //递归当前部门下的所有部门
        //    var underorg = _organizationUnitManager.FindChildren(input.Id, true).Select(ite => ite.Id).ToList();
        //    underorg.Add(input.Id);
        //    var usersm = (from a in _userOrganizationUnitRepository.GetAll()
        //                  join b in _userRepository.GetAll() on a.UserId equals b.Id
        //                  join c in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals c.Id
        //                  where underorg.Contains(a.OrganizationUnitId)
        //                  select new
        //                  {
        //                      AddedTime = b.CreationTime,
        //                      EmailAddress = b.EmailAddress,
        //                      Id = b.Id,
        //                      Name = b.Name,
        //                      IsActive = b.IsActive,
        //                      OId = c.Id,
        //                      Code = c.Code,
        //                      DisplayName = c.DisplayName,
        //                      Surname = b.Surname,
        //                      UserName = b.UserName,
        //                      IsMain = a.IsMain,
        //                      LastLoginTime = b.LastLoginTime,
        //                  });
        //    if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
        //    {
        //        usersm = usersm.Where(ite => ite.DisplayName.Contains(input.SearchKey) || ite.Name.Contains(input.SearchKey) || ite.UserName.Contains(input.SearchKey));
        //    }
        //    var users = await usersm.ToListAsync();
        //    var ret = new List<WorkFlowOrganizationUnitUserListDto>();
        //    if (users.Count > 0)
        //    {
        //        foreach (var u in users)
        //        {
        //            var t = ret.FirstOrDefault(ite => ite.Id == u.Id);
        //            if (t == null)
        //            {
        //                t = new WorkFlowOrganizationUnitUserListDto();
        //                t.AddedTime = u.AddedTime;
        //                t.EmailAddress = u.EmailAddress;
        //                t.Id = u.Id;
        //                t.Name = u.Name;
        //                t.IsActive = u.IsActive;
        //                var o = new SimpleOrganizationDto() { Id = u.OId, Code = u.Code, Title = u.DisplayName, IsMain = u.IsMain };
        //                t.Organization = new List<SimpleOrganizationDto>();
        //                t.Organization.Add(o);
        //                t.Surname = u.Surname;
        //                t.UserName = u.UserName;
        //                t.LastLoginTime = u.LastLoginTime;
        //                ret.Add(t);
        //            }
        //            else
        //            {
        //                var o = new SimpleOrganizationDto() { Id = u.OId, Code = u.Code, Title = u.DisplayName, IsMain = u.IsMain };
        //                t.Organization.Add(o);
        //            }
        //        }
        //    }

        //    var pagelist = ret.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
        //    return new PagedResultDto<WorkFlowOrganizationUnitUserListDto>(ret.Count, pagelist);

        //}

        /// <summary>
        /// 获取当前部门下的所有人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WorkFlowOrganizationUnitUserListDto>> GetOrganizationUnitUsersAndUnder(GetWorkFlowOrganizationUnitUsersInput input) {
            //var underorg = _organizationUnitManager.FindChildren(input.Id, true).Select(ite => ite.Id).ToList();
            //underorg.Add(input.Id);
            var underusers = await _userManager.GetUsersInOrganizationUnit(_organizationUnitRepository.Get(input.Id), true);
            underusers = underusers.Distinct().ToList();
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                underusers = underusers.Where(ite => ite.Name.Contains(input.SearchKey) || ite.UserName.Contains(input.SearchKey)).ToList();
            }
            var total = underusers.Count();
            var users = underusers.OrderByDescending(ite => ite.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            //var usersm = (from a in _userOrganizationUnitRepository.GetAll()
            //              join b in _userRepository.GetAll() on a.UserId equals b.Id
            //              where underorg.Contains(a.OrganizationUnitId)
            //              select new
            //              {
            //                  AddedTime = b.CreationTime,
            //                  EmailAddress = b.EmailAddress,
            //                  Id = b.Id,
            //                  Name = b.Name,
            //                  IsActive = b.IsActive,
            //                  Surname = b.Surname,
            //                  UserName = b.UserName,
            //                  IsMain = a.IsMain,
            //                  LastLoginTime = b.LastLoginTime,
            //              });
            //if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            //{
            //    usersm = usersm.Where(ite => ite.Name.Contains(input.SearchKey) || ite.UserName.Contains(input.SearchKey));
            //}
            //var total = usersm.Count();
            //var users = await usersm.OrderByDescending(ite=>ite.AddedTime).PageBy(input).ToListAsync();
            var ret = new List<WorkFlowOrganizationUnitUserListDto>();
            if (users.Count > 0)
            {
                foreach (var u in users)
                {
                    var t = ret.FirstOrDefault(ite => ite.Id == u.Id);
                    if (t == null)
                    {
                        var orgs = from a in _userOrganizationUnitRepository.GetAll()
                                   join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                                   where a.UserId == t.Id
                                   select new SimpleOrganizationDto()
                                   {
                                       Code = b.Code,
                                       Id = b.Id,
                                       IsMain = a.IsMain,
                                       Title = b.DisplayName
                                   };
                        var posts= from a in _userPostsrepository.GetAll()
                                   join b in _postInforepository.GetAll() on a.PostId equals b.Id
                                   join c in _organizationUnitRepository.GetAll() on a.OrgId equals c.Id
                                   where a.UserId == u.Id
                                   select new UserPostDto { Id = a.Id, IsMain=a.IsMain, OrgName=c.DisplayName, OrgPostId=a.OrgPostId, PostId=a.PostId, PostName=b.Name };
                        t = new WorkFlowOrganizationUnitUserListDto();
                        t.AddedTime = u.CreationTime;
                        t.EmailAddress = u.EmailAddress;
                        t.Id = u.Id;
                        t.Name = u.Name;
                        t.IsActive = u.IsActive;
                        t.Organization = orgs.ToList();
                        t.Posts = posts.ToList();
                        t.Surname = u.Surname;
                        t.UserName = u.UserName;
                        t.LastLoginTime = u.LastLoginTime;
                        ret.Add(t);
                    }
                }
            }
            return new PagedResultDto<WorkFlowOrganizationUnitUserListDto>(total,ret);
        }
        /// <summary>
        /// 获取岗位下的所有人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<WorkFlowOrganizationUnitUserListDto>> GetUserUnderPost(GetUserUnderPostSearch input) {
            var postusers = from a in _userPostsrepository.GetAll()
                            join b in _userRepository.GetAll() on a.UserId equals b.Id
                            where a.OrgPostId ==input.orgPostId
                            
                            select new WorkFlowOrganizationUnitUserListDto()
                            {
                                AddedTime = a.CreationTime,
                                EmailAddress = b.EmailAddress,
                                Id = a.UserId,
                                IsActive = b.IsActive,
                                LastLoginTime = b.LastLoginTime,
                                Name = b.Name,
                                ProfilePictureId = b.ProfilePictureId,
                                Surname = b.Surname,
                                UserName = b.UserName,
                                
                            };

            var total = postusers.Count();
            var pagelist = postusers.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            foreach (var item in pagelist) {
                var o = from b in _userOrganizationUnitRepository.GetAll()
                        join c in _organizationUnitRepository.GetAll() on b.OrganizationUnitId equals c.Id
                        where b.UserId == item.Id
                        select new SimpleOrganizationDto()
                        {
                            Code = c.Code,
                            Id = b.Id,
                            IsMain = b.IsMain,
                            Title = c.DisplayName
                        };
                var posts = from a in _userPostsrepository.GetAll()
                            join b in _postInforepository.GetAll() on a.PostId equals b.Id
                            join c in _organizationUnitRepository.GetAll() on a.OrgId equals c.Id
                            where a.UserId == item.Id
                            select new UserPostDto { Id = a.Id, IsMain = a.IsMain, OrgName = c.DisplayName, OrgPostId = a.OrgPostId, PostId = a.PostId, PostName = b.Name };
                item.Organization = o.ToList();
                item.Posts = posts.ToList();
            }
            return new PagedResultDto<WorkFlowOrganizationUnitUserListDto>(total, pagelist);

        }
        /// <summary>
        /// 设置用户的部门信息
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public async Task SetUserToOrganizationUnit(OrganizationUnitUserInput input)
        {
            //if (input.Organization.Count(r => r.IsMain) != 1)
            //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "用户有且只能有一个主部门");//这里不设置主部门 改为通过设置主岗位来更新主部门
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var abpOrganizationUnitsManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<AbpOrganizationUnitsManager>();
                await abpOrganizationUnitsManager.SetOrganizationUnitsAsync(input.Id, input.Organization.Select(ite => ite.Id).ToArray());
                unitOfWork.Complete();
            }

            //这里不设置主部门 改为通过设置主岗位来更新主部门
            //using (var unitOfWork = _unitOfWorkManager.Begin())
            //{
            //    var relations = await _userOrganizationUnitRepository.GetAll().Where(r => r.UserId == input.Id).ToListAsync();
            //    var mainOrgId = input.Organization.SingleOrDefault(r => r.IsMain);
            //    foreach (var item in relations)
            //    {
            //        if (item.OrganizationUnitId == mainOrgId.Id)
            //            item.IsMain = true;
            //        else
            //            item.IsMain = false;
            //        await _userOrganizationUnitRepository.UpdateAsync(item);

            //    }
            //    unitOfWork.Complete();
            //}

        }

        public async Task<bool> IsInOrganizationUnit(UserToWorkFlowOrganizationUnitInput input)
        {
            var abpOrganizationUnitsManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<AbpOrganizationUnitsManager>();
            return await abpOrganizationUnitsManager.IsInOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }

        //public async Task<WorkFlowOrganizationUnitDto> MoveOrganizationUnit(MoveWorkFlowOrganizationUnitInput input)
        //{
        //    await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

        //    return await CreateOrganizationUnitDto(
        //        await _organizationUnitRepository.GetAsync(input.Id)
        //        );
        //}


        public async Task RemoveUserFromOrganizationUnit(UserToWorkFlowOrganizationUnitInput input)
        {
            var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManager>();
            await userManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        }
        /// <summary>
        /// 编辑部门信息

        //public async Task RemoveUserFromOrganizationUnit(UserToWorkFlowOrganizationUnitInput input)
        //{
        //    var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManager>();
        //    await userManager.RemoveFromOrganizationUnitAsync(input.UserId, input.OrganizationUnitId);
        //}

        /// <summary>
        /// 更新部门信息 对部门的岗位信息的编辑单独处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkFlowOrganizationUnitDto> UpdateOrganizationUnit(UpdateWorkFlowOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            organizationUnit.DisplayName = input.DisplayName;
            organizationUnit.Type = input.Type;
            organizationUnit.Note = input.Note;
            if (input.Posts.Select(r => r.PostName).Distinct().Count() != input.Posts.Count())
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "部门的岗位名称不能重复");
            await _organizationUnitManager.UpdateAsync(organizationUnit);

            #region  删除岗位
            var organizationUnitPostses = await _organizationUnitPostrepository.GetAll().Where(x =>
                    x.OrganizationUnitId == input.Id &&
                    !input.Posts.Where(y => y.Id != null).Select(y => y.Id).Contains(x.Id))
                .ToListAsync();
            foreach (var item in organizationUnitPostses)
            {
                var model = await _organizationUnitPostrepository.GetAsync(item.Id);
                if (await _userPostsrepository.GetAll().AnyAsync(r => r.OrgPostId == model.Id))
                {
                    var postInfo = await _postInforepository.GetAsync(item.PostId);
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, postInfo.Name + "岗位下存在用户，无法删除");
                }
                await _organizationUnitPostrepository.DeleteAsync(model);

                //删除岗位和角色的关联
                await _organizationUnitPostsRoleRepository.DeleteAsync(ite => ite.OrgPostId == item.Id);
            }
            #endregion

            foreach (var item in input.Posts)
            {
                if (item.Id.HasValue)
                {
                    var exit_OrgPost = await _organizationUnitPostrepository.GetAsync(item.Id.Value);
                    if (item.PostId.HasValue)
                    {
                        if (item.PostId.Value != exit_OrgPost.PostId)
                        {
                            exit_OrgPost.PostId = item.PostId.Value;
                        }

                    }
                    else
                    {
                        var postService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();
                        var exit_PostByName = await postService.GetByNameAsync(item.PostName);
                        if (exit_PostByName != null)
                        {
                            exit_OrgPost.PostId = exit_PostByName.Id;
                        }
                        else
                        {
                            var new_post = new PostInfo() { Id = Guid.NewGuid(), Name = item.PostName };
                            await _postInforepository.InsertAsync(new_post);
                            exit_OrgPost.PostId = new_post.Id;
                        }
                    }
                    exit_OrgPost.PrepareNumber = item.PrepareNumber;
                    exit_OrgPost.Level = item.Level;
                }
                else
                {
                    var new_Org = new OrganizationUnitPosts() { Id = Guid.NewGuid(), PrepareNumber = item.PrepareNumber, OrganizationUnitId = input.Id, Level = item.Level };
                    item.Id = new_Org.Id;
                    if (item.PostId.HasValue)
                    {
                        if (await _postInforepository.GetAll().AnyAsync(r => r.Id == item.PostId.Value))
                        {
                            new_Org.PostId = item.PostId.Value;
                        }
                        else
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "参数异常");
                    }
                    else
                    {
                        var postService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();
                        var exit_PostByName = await postService.GetByNameAsync(item.PostName);
                        if (exit_PostByName != null)
                        {
                            new_Org.PostId = exit_PostByName.Id;
                        }
                        else
                        {
                            var new_post = new PostInfo() { Id = Guid.NewGuid(), Name = item.PostName };
                            await _postInforepository.InsertAsync(new_post);
                            new_Org.PostId = new_post.Id;
                        }
                    }
                    await _organizationUnitPostrepository.InsertAsync(new_Org);
                }

                // 创建岗位角色关联
                if (item.RoleNames == null || item.RoleNames.Count == 0)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "岗位至少需要关联一个角色。");
                }
                // 如果岗位关联的角色调整则该岗位所有用户的角色都需要更新

                var oldroles = _organizationUnitPostsRoleRepository.GetAll().Where(ite => ite.OrgPostId == item.Id).Select(ite => ite.RoleName);//原来的角色
                var addroles = item.RoleNames.Except(oldroles).ToList();
                var delroles = oldroles.Except(item.RoleNames).ToList();

                await _organizationUnitPostsRoleRepository.DeleteAsync(ite => ite.OrgPostId == item.Id);

                foreach (var r in item.RoleNames)
                {
                    _organizationUnitPostsRoleRepository.Insert(new OrganizationUnitPostsRole()
                    {
                        Id = Guid.NewGuid(),
                        OrgPostId = item.Id.Value,
                        RoleName = r
                    });
                }
                //获取当前岗位所有用户
                var postusers = _userPostsrepository.GetAll().Where(ite => ite.OrgPostId == item.Id).ToList();
                foreach (var u in postusers) {
                    //当前岗位角色
                    
                    var u2 = _userRepository.GetAll().FirstOrDefault(ite=>ite.Id==u.UserId);
                    if (u2 == null) {
                        continue;
                    }
                    if (delroles != null && delroles.Count > 0)
                    {
                        await _userManager.RemoveFromRolesAsync(u2, delroles.ToArray());
                    }
                    if (addroles != null && addroles.Count > 0)
                    {
                        await _userManager.AddToRolesAsync(u2, addroles.ToArray());
                    }

                }
            }

            return await CreateOrganizationUnitDto(organizationUnit);
        }

        /// <summary>
        /// 删除部门的岗位
        /// </summary>
        /// <param name="input">部门的岗位id</param>
        /// <returns></returns>
        public async Task DeleteOrgPost(EntityDto<Guid> input)
        {
            var model = await _organizationUnitPostrepository.GetAsync(input.Id);
            if (await _userPostsrepository.GetAll().AnyAsync(r => r.OrgPostId == model.Id))
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "岗位下存在用户，无法删除");
            await _organizationUnitPostrepository.DeleteAsync(model);
        }


        public async Task<GetOrganizationUnitTreeOutput> GetOrganizationUnitTree(GetOrganizationUnitTreeInput input)
        {
            var query = from o in _organizationUnitRepository.GetAll()
                        join childo in _organizationUnitRepository.GetAll() on o.Id equals childo.ParentId into g
                        where o.Id == input.ParentId
                        select new
                        {
                            o = o,
                            c = g
                        };
            var ret = new GetOrganizationUnitTreeOutput();
            var data = await query.FirstOrDefaultAsync();
            if (data != null)
            {
                ret.Id = data.o.Id.ToString();
                ret.Title = data.o.DisplayName;
                ret.NodeType = data.o.Type;
                foreach (var item_c in data.c)
                {
                    var entity_c = new GetOrganizationUnitTreeOutput();
                    entity_c.Id = item_c.Id.ToString();
                    entity_c.Title = item_c.DisplayName;
                    entity_c.NodeType = item_c.Type;
                    entity_c.OrgId = item_c.Id;
                    ret.Children.Add(entity_c);
                }
            }
            if (input.SelectType != 1)
            {
                var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = input.ParentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });

                if (!data.o.ChargeLeader.IsNullOrEmpty())
                {
                    var entity_GodLeader = new GetOrganizationUnitTreeOutput();
                    entity_GodLeader.Id = $"l_{input.ParentId}";
                    entity_GodLeader.Title = $"{data.o.DisplayName}[领导]";
                    entity_GodLeader.NodeType = 3;
                    entity_GodLeader.OrgId = data.o.Id;
                    ret.Children.Add(entity_GodLeader);
                }

                if (users.TotalCount > 0)
                {
                    var entity_allmember = new GetOrganizationUnitTreeOutput();
                    entity_allmember.Id = $"d_{input.ParentId}";
                    entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                    entity_allmember.NodeType = 4;
                    entity_allmember.OrgId = data.o.Id;
                    ret.Children.Add(entity_allmember);
                }



                foreach (var item in users.Items)
                {
                    var entity = new GetOrganizationUnitTreeOutput();
                    entity.Id = $"u_{item.Id}";
                    entity.Title = item.Name;
                    entity.OrgId = data.o.Id;
                    entity.NodeType = 2;
                    ret.Children.Add(entity);
                }
            }

            return ret;
        }


        public async Task<GetOrganizationUnitTreeOutput> GetOrganizationUnitTreeNew(GetOrganizationUnitTreeNewInput input)
        {
            #region 查询
            var parentId = MemberPerfix.RemovePrefix(input.ParentId).ToLong();

            var query = from o in _organizationUnitRepository.GetAll()
                        join childo in _organizationUnitRepository.GetAll() on o.Id equals childo.ParentId into g
                        where o.Id == parentId
                        select new
                        {
                            o = o,
                            c = g
                        };
            var ret = new GetOrganizationUnitTreeOutput();
            var data = await query.FirstOrDefaultAsync();
            if (data != null)
            {
                ret.Id = data.o.Id.ToString();
                ret.Title = data.o.DisplayName;
                ret.Code = data.o.Code;
                ret.NodeType = data.o.Type;
                foreach (var item_c in data.c)
                {
                    var entity_c = new GetOrganizationUnitTreeOutput();
                    entity_c.Id = item_c.Id.ToString();
                    entity_c.Title = item_c.DisplayName;
                    entity_c.Code = item_c.Code;
                    //entity_c.NodeType = item_c.Type;
                    entity_c.NodeType = 1;
                    entity_c.OrgId = data.o.Id;
                    ret.Children.Add(entity_c);
                }
            }

            #region  选择人员
            if (input.SelectType == 0)
            {



                if (input.TreeType == 1)
                {
                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }
                }
                else if (input.TreeType == 2)
                {
                    ret.Id = $"{MemberPerfix.DepartmentPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[领导]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.DepartmentPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[领导]";

                    }
                }
                else if (input.TreeType == 3)
                {
                    ret.Id = $"{MemberPerfix.DepartmentPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[领导]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.DepartmentPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[领导]";

                    }


                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }
                }
                else if (input.TreeType == 4)
                {
                    ret.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[直属成员]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[直属成员]";

                    }
                }
                else if (input.TreeType == 5)
                {
                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    if (users.TotalCount > 0)
                    {
                        var entity_allmember = new GetOrganizationUnitTreeOutput();
                        entity_allmember.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{input.ParentId}";
                        entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                        entity_allmember.NodeType = 4;
                        entity_allmember.OrgId = data.o.Id;
                        entity_allmember.Code = data.o.Code;
                        ret.Children.Add(entity_allmember);
                    }
                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }

                }


                else if (input.TreeType == 6)
                {
                    if (!data.o.ChargeLeader.IsNullOrEmpty())
                    {
                        var entity_GodLeader = new GetOrganizationUnitTreeOutput();
                        entity_GodLeader.Id = $"{MemberPerfix.DepartmentPREFIX}{input.ParentId}";
                        entity_GodLeader.Title = $"{data.o.DisplayName}[领导]";
                        entity_GodLeader.NodeType = 3;
                        entity_GodLeader.OrgId = data.o.Id;
                        entity_GodLeader.Code = data.o.Code;
                        ret.Children.Add(entity_GodLeader);
                    }

                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    if (users.TotalCount > 0)
                    {
                        var entity_allmember = new GetOrganizationUnitTreeOutput();
                        entity_allmember.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{input.ParentId}";
                        entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                        entity_allmember.NodeType = 4;
                        entity_allmember.OrgId = data.o.Id;
                        entity_allmember.Code = data.o.Code;
                        ret.Children.Add(entity_allmember);
                    }
                }

                else if (input.TreeType == 7)
                {
                    if (!data.o.ChargeLeader.IsNullOrEmpty())
                    {
                        var entity_GodLeader = new GetOrganizationUnitTreeOutput();
                        entity_GodLeader.Id = $"{MemberPerfix.DepartmentPREFIX}{input.ParentId}";
                        entity_GodLeader.Title = $"{data.o.DisplayName}[领导]";
                        entity_GodLeader.NodeType = 3;
                        entity_GodLeader.OrgId = data.o.Id;
                        entity_GodLeader.Code = data.o.Code;
                        ret.Children.Add(entity_GodLeader);
                    }

                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    if (users.TotalCount > 0)
                    {
                        var entity_allmember = new GetOrganizationUnitTreeOutput();
                        entity_allmember.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{input.ParentId}";
                        entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                        entity_allmember.NodeType = 4;
                        entity_allmember.OrgId = data.o.Id;
                        entity_allmember.Code = data.o.Code;
                        ret.Children.Add(entity_allmember);
                    }

                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }
                }

                else if (input.TreeType == 8)
                {
                    ret.Id = $"{MemberPerfix.AllUserPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[所有成员]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.AllUserPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[所有成员]";

                    }
                }

                else if (input.TreeType == 9)
                {
                    ret.Id = $"{MemberPerfix.AllUserPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[所有成员]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.AllUserPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[所有成员]";

                    }

                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }
                }


                else if (input.TreeType == 10)
                {
                    if (!data.o.ChargeLeader.IsNullOrEmpty())
                    {
                        var entity_GodLeader = new GetOrganizationUnitTreeOutput();
                        entity_GodLeader.Id = $"{MemberPerfix.DepartmentPREFIX}{input.ParentId}";
                        entity_GodLeader.Title = $"{data.o.DisplayName}[领导]";
                        entity_GodLeader.NodeType = 3;
                        entity_GodLeader.OrgId = data.o.Id;
                        entity_GodLeader.Code = data.o.Code;
                        ret.Children.Add(entity_GodLeader);
                    }

                    var entity_all = new GetOrganizationUnitTreeOutput();
                    entity_all.Id = $"{MemberPerfix.AllUserPREFIX}{input.ParentId}";
                    entity_all.Title = $"{data.o.DisplayName}[所有人]";
                    entity_all.NodeType = 5;
                    entity_all.OrgId = data.o.Id;
                    entity_all.Code = data.o.Code;
                    ret.Children.Add(entity_all);
                }

                else if (input.TreeType == 11)
                {
                    ret.Id = $"{MemberPerfix.AllUserPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[所有成员]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.AllUserPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[所有成员]";

                    }
                    if (!data.o.ChargeLeader.IsNullOrEmpty())
                    {
                        var entity_GodLeader = new GetOrganizationUnitTreeOutput();
                        entity_GodLeader.Id = $"{MemberPerfix.DepartmentPREFIX}{input.ParentId}";
                        entity_GodLeader.Title = $"{data.o.DisplayName}[领导]";
                        entity_GodLeader.NodeType = 3;
                        entity_GodLeader.OrgId = data.o.Id;
                        entity_GodLeader.Code = data.o.Code;
                        ret.Children.Add(entity_GodLeader);
                    }

                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }
                }



                else if (input.TreeType == 12)
                {

                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    if (users.TotalCount > 0)
                    {
                        var entity_allmember = new GetOrganizationUnitTreeOutput();
                        entity_allmember.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{input.ParentId}";
                        entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                        entity_allmember.NodeType = 4;
                        entity_allmember.OrgId = data.o.Id;
                        entity_allmember.Code = data.o.Code;
                        ret.Children.Add(entity_allmember);
                    }

                    var entity_all = new GetOrganizationUnitTreeOutput();
                    entity_all.Id = $"{MemberPerfix.AllUserPREFIX}{input.ParentId}";
                    entity_all.Title = $"{data.o.DisplayName}[所有人]";
                    entity_all.NodeType = 5;
                    entity_all.OrgId = data.o.Id;
                    entity_all.Code = data.o.Code;
                    ret.Children.Add(entity_all);

                }

                else if (input.TreeType == 13)
                {
                    ret.Id = $"{MemberPerfix.AllUserPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[所有成员]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.AllUserPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[所有成员]";

                    }


                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    if (users.TotalCount > 0)
                    {
                        var entity_allmember = new GetOrganizationUnitTreeOutput();
                        entity_allmember.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{input.ParentId}";
                        entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                        entity_allmember.NodeType = 4;
                        entity_allmember.OrgId = data.o.Id;
                        entity_allmember.Code = data.o.Code;
                        ret.Children.Add(entity_allmember);
                    }

                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }

                }

                else if (input.TreeType == 14)
                {


                    if (!data.o.ChargeLeader.IsNullOrEmpty())
                    {
                        var entity_GodLeader = new GetOrganizationUnitTreeOutput();
                        entity_GodLeader.Id = $"{MemberPerfix.DepartmentPREFIX}{input.ParentId}";
                        entity_GodLeader.Title = $"{data.o.DisplayName}[领导]";
                        entity_GodLeader.NodeType = 3;
                        entity_GodLeader.OrgId = data.o.Id;
                        entity_GodLeader.Code = data.o.Code;
                        ret.Children.Add(entity_GodLeader);
                    }


                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    if (users.TotalCount > 0)
                    {
                        var entity_allmember = new GetOrganizationUnitTreeOutput();
                        entity_allmember.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{input.ParentId}";
                        entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                        entity_allmember.NodeType = 4;
                        entity_allmember.OrgId = data.o.Id;
                        entity_allmember.Code = data.o.Code;
                        ret.Children.Add(entity_allmember);
                    }
                    var entity_all = new GetOrganizationUnitTreeOutput();
                    entity_all.Id = $"{MemberPerfix.AllUserPREFIX}{input.ParentId}";
                    entity_all.Title = $"{data.o.DisplayName}[所有人]";
                    entity_all.NodeType = 5;
                    entity_all.OrgId = data.o.Id;
                    entity_all.Code = data.o.Code;
                    ret.Children.Add(entity_all);



                }


                else if (input.TreeType == 15)
                {
                    ret.Id = $"{MemberPerfix.AllUserPREFIX}{ret.Id}";
                    ret.Title = $"{ ret.Title }[所有成员]";
                    foreach (var item in ret.Children)
                    {
                        item.Id = $"{MemberPerfix.AllUserPREFIX}{item.Id}";
                        item.Title = $"{ item.Title }[所有成员]";

                    }


                    if (!data.o.ChargeLeader.IsNullOrEmpty())
                    {
                        var entity_GodLeader = new GetOrganizationUnitTreeOutput();
                        entity_GodLeader.Id = $"{MemberPerfix.DepartmentPREFIX}{input.ParentId}";
                        entity_GodLeader.Title = $"{data.o.DisplayName}[领导]";
                        entity_GodLeader.NodeType = 3;
                        entity_GodLeader.OrgId = data.o.Id;
                        entity_GodLeader.Code = data.o.Code;
                        ret.Children.Add(entity_GodLeader);
                    }


                    var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });
                    if (users.TotalCount > 0)
                    {
                        var entity_allmember = new GetOrganizationUnitTreeOutput();
                        entity_allmember.Id = $"{MemberPerfix.DepartmentMemberPREFIX}{input.ParentId}";
                        entity_allmember.Title = $"{data.o.DisplayName}[直属成员]";
                        entity_allmember.NodeType = 4;
                        entity_allmember.OrgId = data.o.Id;
                        entity_allmember.Code = data.o.Code;
                        ret.Children.Add(entity_allmember);
                    }


                    foreach (var item in users.Items)
                    {
                        var entity = new GetOrganizationUnitTreeOutput();
                        entity.Id = $"{MemberPerfix.UserPREFIX}{item.Id}";
                        entity.Title = item.Name;
                        entity.OrgId = data.o.Id;
                        entity.Code = data.o.Code;
                        entity.NodeType = 2;
                        ret.Children.Add(entity);
                    }

                }
            }
            #endregion

            else if (input.SelectType == 1)
            {

            }
            else if (input.SelectType == 2)
            {
                var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });

                foreach (var item in users.Items)
                {
                    var entity = new GetOrganizationUnitTreeOutput();
                    entity.Id = item.Id.ToString();
                    entity.Title = item.Name;
                    entity.OrgId = data.o.Id;
                    entity.Code = data.o.Code;
                    entity.NodeType = 2;
                    ret.Children.Add(entity);
                }
            }
            else if (input.SelectType == 3)
            {
                ret.Id = $"{MemberPerfix.DepartmentIdPREFIX}{ret.Id}";
                foreach (var item in ret.Children)
                {
                    item.Id = $"{MemberPerfix.DepartmentIdPREFIX}{item.Id}";
                }

                var users = await GetOrganizationUnitUsers(new GetWorkFlowOrganizationUnitUsersInput() { Id = parentId, MaxResultCount = 10000, SkipCount = 0, Sorting = " user.Name " });

                foreach (var item in users.Items)
                {
                    var entity = new GetOrganizationUnitTreeOutput();
                    entity.Id = $"{MemberPerfix.UserIdPREFIX}{item.Id}";
                    entity.Title = item.Name;
                    entity.OrgId = data.o.Id;
                    entity.Code = data.o.Code;
                    entity.NodeType = 2;
                    ret.Children.Add(entity);
                }
            }
            #endregion

            #region 过滤

            if (input.Roles.Count() > 0)
            {
                var cwUsers = _workFlowOrganizationUnitsManager.GetAbpUsersByRoleCodes(input.Roles);
               
                for (int i = ret.Children.Count()-1; i >=0 ; i--)
                {
                    var info = ret.Children[i];
                    var isContains = false;
                    switch (info.NodeType)
                    {
                        case 1:
                            foreach (var item in cwUsers)
                            {
                                var org = _workFlowOrganizationUnitsManager.GetDeptByUserID(item.Id);
                                if (org.Code.Contains(info.Code))
                                {
                                    isContains = true;
                                    continue;
                                }
                            }
                            break;
                        case 2:
                            if (cwUsers.Select(x => x.Id.ToString()).Contains(info.Id))
                                isContains = true;
                            break;
                    }
                    if (!isContains)
                        ret.Children.Remove(info);
                }
            }
            if (input.Permissions.Count() > 0)
            {
                var cwUsers = _workFlowOrganizationUnitsManager.GetAbpUsersByPermissions(input.Permissions);
                for (int i = ret.Children.Count() - 1; i >= 0; i--)
                {
                    var info = ret.Children[i];
                    var isContains = false;
                    switch (info.NodeType)
                    {
                        case 1:
                            foreach (var item in cwUsers)
                            {
                                var org = _workFlowOrganizationUnitsManager.GetDeptByUserID(item.Id);
                                if (org.Code.Contains(info.Code))
                                {
                                    isContains = true;
                                    continue;
                                }
                            }
                            break;
                        case 2:
                            if (cwUsers.Select(x => x.Id.ToString()).Contains(info.Id))
                                isContains = true;
                            break;
                    }
                    if (!isContains)
                        ret.Children.Remove(info);
                }
            }
            #endregion
            return ret;
        }



        /// <summary>
        /// 获取一个用户的所有部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<List<OrganizationUnitUserOutput>> GetUserAllOrgs(NullableIdDto<long> input)
        {
            var current_userId = 0L;
            if (!input.Id.HasValue)
                current_userId = AbpSession.UserId.Value;
            else
                current_userId = input.Id.Value;
            var query = from a in _userOrganizationUnitRepository.GetAll()
                        join b in _organizationUnitRepository.GetAll() on a.OrganizationUnitId equals b.Id
                        where a.UserId == current_userId
                        select new OrganizationUnitUserOutput
                        {
                            Id = a.OrganizationUnitId,
                            Title = b.DisplayName,
                            Code = b.Code,
                            IsMain = a.IsMain
                        };
            var relations = await _userOrganizationUnitRepository.GetAll().Where(r => r.UserId == input.Id).ToListAsync();
            var ret = await query.ToListAsync();
            return ret;
        }




        /// <summary>
        /// 获取一个节点下的所有子部门 递归
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrganizationUnitTreeOutput> GetOrganizationUnitChildren(NullableIdDto<long> input)
        {
            var organs = await _organizationUnitManager.FindChildrenAsync(input.Id, true);
            var firstorg = new OrganizationUnitTreeOutput();
            var firstModel = new WorkFlowOrganizationUnits();
            if (input.Id.HasValue)
            {
                firstModel = await _organizationUnitRepository.GetAsync(input.Id.Value);

            }
            else
            {
                firstModel = await _organizationUnitRepository.GetAll().FirstOrDefaultAsync(r => !r.ParentId.HasValue);

            }
            firstorg.Id = firstModel.Id;
            firstorg.Value = firstModel.Id;
            firstorg.Title = firstModel.DisplayName;
            firstorg.Label = firstModel.DisplayName;
            MakeOrganizationUnitTree(firstorg, organs);
            return firstorg;
        }


        public async Task<List<UserUnderOrgProssceStaticOutput>> GetUserUnderOrgProssceStatic(UserUnderOrgProssceStaticInput input)
        {
            var parameters = new DynamicParameters();
            if (input.OrgId.HasValue)
            {
                parameters.Add("@orgId", input.OrgId ?? (object)DBNull.Value);
                var ret = await _dynamicRepository.QueryAsync<UserUnderOrgProssceStaticOutput>($"exec [dbo].[spGetUsersWithCurrentAndUnderOrgStatic] @orgId", parameters);
                return ret.ToList();
            }
            else
            {
                var ret = await _dynamicRepository.QueryAsync<UserUnderOrgProssceStaticOutput>($"exec [dbo].[spGetUsersWithCurrentAndUnderOrgStatic] ");
                return ret.ToList();
            }


        }

        public async Task<List<UserUnderOrgOutput>> GetUserWithCurrentAndUnderOrg(UserUnderOrgProssceStaticInput input)
        {
            var parameters = new DynamicParameters();
            if (input.OrgId.HasValue)
            {
                parameters.Add("@orgId", input.OrgId ?? (object)DBNull.Value);
                var ret = await _dynamicRepository.QueryAsync<UserUnderOrgOutput>($"exec [dbo].[spGetUsersWithCurrentAndUnderOrg] @orgId", parameters);
                return ret.ToList();
            }
            else
            {
                var ret = await _dynamicRepository.QueryAsync<UserUnderOrgOutput>($"exec [dbo].[spGetUsersWithCurrentAndUnderOrg] ");
                return ret.ToList();
            }


        }





        protected virtual async Task ValidateOrganizationUnitAsync(OrganizationUnit organizationUnit)
        {
            var siblings = (await _organizationUnitManager.FindChildrenAsync(organizationUnit.ParentId))
                .Where(ou => ou.Id != organizationUnit.Id)
                .ToList();

            if (siblings.Any(ou => ou.DisplayName == organizationUnit.DisplayName))
            {
                throw new UserFriendlyException(L("OrganizationUnitDuplicateDisplayNameWarning", organizationUnit.DisplayName));
            }
        }
        private async Task<WorkFlowOrganizationUnitDto> CreateOrganizationUnitDto(WorkFlowOrganizationUnits organizationUnit)
        {
            var dto = organizationUnit.MapTo<WorkFlowOrganizationUnitDto>();
            dto.MemberCount = await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }

        private void MakeOrganizationUnitTree(OrganizationUnitTreeOutput model, List<OrganizationUnit> source)
        {
            if (source.Any(r => r.ParentId.HasValue && r.ParentId.Value == model.Id))
            {
                var childs = source.Where(r => r.ParentId.HasValue && r.ParentId.Value == model.Id);
                foreach (var item in childs)
                {
                    var entity = new OrganizationUnitTreeOutput();
                    entity.Id = item.Id;
                    entity.Value = item.Id;
                    entity.Title = item.DisplayName;
                    entity.Label = item.DisplayName;
                    model.Children.Add(entity);
                    MakeOrganizationUnitTree(entity, source);
                }
            }
        }

        public List<UserText> GetUserNameForShow(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return null;
            }
            var id = Array.ConvertAll<string, long>(ids.Split(",", StringSplitOptions.RemoveEmptyEntries), new Converter<string, long>(ite => long.Parse(ite)));
            var users = _userRepository.GetAll().Where(ite => id.Contains(ite.Id)).Select(ite => new UserText() { Id = ite.Id, Name = ite.Name }).ToList();
            return users;
        }

        public List<UserOrgShow> GetUserNameForWorkFlow(UserOrgShowInput input)
        {
            var ret = _workFlowOrganizationUnitsManager.GetNamesArr(input);

            return ret.ToList();
        }




        /// <summary>
        /// 获取一个用户的部门、岗位信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<UserWorkFlowOrganizationUnitDto> GetUserPostInfo(NullableIdDto<long> input, NullableIdDto<long> orgInput)
        {
            var current_userId = 0L;
            if (!input.Id.HasValue)
                current_userId = AbpSession.UserId.Value;
            else
                current_userId = input.Id.Value;
            var orgId = 0L;
            var orgModel = new WorkFlowOrganizationUnits();
            if (orgInput.Id.HasValue)
            {
                orgId = orgInput.Id.Value;
                orgModel = await _organizationUnitRepository.GetAsync(orgId);
            }
            else
            {
                orgModel = _workFlowOrganizationUnitsManager.GetDeptByUserID(current_userId);
                if (orgModel != null)
                    orgId = orgModel.Id;
                else
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到用户的主部门");
            }

            var query = from a in _postInforepository.GetAll()
                        join b in _userPostsrepository.GetAll() on a.Id equals b.PostId
                        join c in _organizationUnitPostrepository.GetAll() on b.OrgPostId equals c.Id
                        where b.UserId == current_userId && c.OrganizationUnitId == orgId
                        select new UserPostInfo { OrgPostId = c.Id, PostId = a.Id, PostName = a.Name, UserOrgPostId = b.Id };
            var ret = new UserWorkFlowOrganizationUnitDto()
            {
                OrgId = orgModel.Id,
                OrgId_Name = orgModel.DisplayName,
                UserPosts = await query.ToListAsync()
            };
            return ret;
        }


        [AbpAuthorize]
        public UserWorkFlowOrganizationUnitDto GetUserPostInfoV2(NullableIdDto<long> input, NullableIdDto<long> orgInput)
        {
            var current_userId = 0L;
            if (!input.Id.HasValue)
                current_userId = AbpSession.UserId.Value;
            else
                current_userId = input.Id.Value;
            var orgId = 0L;
            var orgModel = new WorkFlowOrganizationUnits();
            if (orgInput.Id.HasValue)
            {
                orgId = orgInput.Id.Value;
                orgModel = _organizationUnitRepository.Get(orgId);
            }
            else
            {
                orgModel = _workFlowOrganizationUnitsManager.GetDeptByUserID(current_userId);
                if (orgModel != null)
                    orgId = orgModel.Id;
                else
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到用户的主部门");
            }

            var query = from a in _postInforepository.GetAll()
                        join b in _userPostsrepository.GetAll() on a.Id equals b.PostId
                        join c in _organizationUnitPostrepository.GetAll() on b.OrgPostId equals c.Id
                        where b.UserId == current_userId && c.OrganizationUnitId == orgId
                        select new UserPostInfo { OrgPostId = c.Id, PostId = a.Id, PostName = a.Name, UserOrgPostId = b.Id };
            var ret = new UserWorkFlowOrganizationUnitDto()
            {
                OrgId = orgModel.Id,
                OrgId_Name = orgModel.DisplayName,
                UserPosts = query.ToList()
            };
            return ret;
        }
        /// <summary>
        /// 更新机构名称
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool UpdateOrgName(UpdateOrganizationName input)
        {
            var org = _organizationUnitRepository.Get(input.Id);
            org.DisplayName = input.DisplayName;
            _organizationUnitRepository.Update(org);
            return true;
        }
    }
}
