using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Abp.Application.Services;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.Authorization;
using ZCYX.FRMSCore.Authorization.Roles;
using Abp.Domain.Uow;
using Abp;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Users;
using Abp.Authorization.Roles;

namespace ZCYX.FRMSCore.Application
{
    [RemoteService(IsEnabled = false)]
    public class WorkFlowOrganizationUnitsManager : ApplicationService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<UserRoleRelation, long> _userRoleRelationRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionSettingRepository;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionSettingRepository;
        private readonly IRepository<UserPermissionRelation, long> _userPermissionRelationRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _orgPostRepository;
        private readonly IRepository<PostInfo, Guid> _postRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<OrganizationUnitPostsRole, Guid> _organizationUnitPostsRoleRepository;
        private readonly AbpOrganizationUnitsManager _abpOrganizationUnitsManager;

        public UserManager UserManager { get; set; }

        public WorkFlowOrganizationUnitsManager(IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            AbpOrganizationUnitsManager abpOrganizationUnitsManager,
            IRepository<User, long> userRepository, IRepository<OrganizationUnitPostsRole, Guid> organizationUnitPostsRoleRepository,
            OrganizationUnitManager organizationUnitManager, IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository,
            IRepository<UserRole, long> userRoleRepository, IRepository<Role> roleRepository, IRepository<UserPosts, Guid> userPostRepository
            , IRepository<OrganizationUnitPosts, Guid> orgPostRepository, IRepository<PostInfo, Guid> postRepository, IUnitOfWorkManager unitOfWorkManager, IRepository<UserRoleRelation, long> userRoleRelationRepository, IRepository<UserPermissionRelation, long> userPermissionRelationRepository, IRepository<UserPermissionSetting, long> userPermissionSettingRepository, IRepository<RolePermissionSetting, long> rolePermissionSettingRepository)
        {
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRepository = userRepository;
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userPostRepository = userPostRepository;
            _orgPostRepository = orgPostRepository;
            _postRepository = postRepository;
            _organizationUnitPostsRoleRepository = organizationUnitPostsRoleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _abpOrganizationUnitsManager = abpOrganizationUnitsManager;
            _userRoleRelationRepository = userRoleRelationRepository;
            _userPermissionRelationRepository = userPermissionRelationRepository;
            _userPermissionSettingRepository = userPermissionSettingRepository;
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
        }
        public List<User> GetAllUsers(string idString)
        {
            if (idString.IsNullOrEmpty())
            {
                return new List<User>();
            }
            string[] idArray = idString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var userList = new List<User>();
            foreach (string id in idArray)
            {
                if (id.StartsWith(MemberPerfix.UserPREFIX))//人员
                {
                    var user = _userRepository.Get(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.Add(user);
                }
                else if (id.IsLong())//机构
                {
                    userList.AddRange(GetAllUsersById(id.ToLong()));
                }
                else if (id.StartsWith(MemberPerfix.DepartmentPREFIX)) //部门领导
                {
                    var leader = GetLeaderById(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.AddRange(leader);
                }
                else if (id.StartsWith(MemberPerfix.DepartmentFGLeaderPREFIX)) //部门分管领导
                {
                    var chargleader = GetChargeLeaderById(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.AddRange(chargleader);
                }
                else if (id.StartsWith(MemberPerfix.DepartmentMemberPREFIX)) //部门直属成员
                {
                    var users = GetAllByOrganizeIDArray(new long[] { MemberPerfix.RemovePrefix(id).ToLong() });
                    userList.AddRange(users);
                }
                else if (id.StartsWith(MemberPerfix.AllUserPREFIX)) //部门直属成员
                {
                    var users = GetAllUsersById(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.AddRange(users.Distinct());
                }
                else if (id.StartsWith(MemberPerfix.WorkGroupPREFIX))//角色组
                {
                    //角色组暂未实现
                }
                else if (id.StartsWith(MemberPerfix.PostPREFIX))
                {
                    var users = GetAbpUsersByOrgPostIds(id);
                    userList.AddRange(users.Distinct());
                }
            }
            userList.RemoveAll(p => p == null);
            return userList.Distinct(new UsersEqualityComparer()).ToList();
        }
        /// <summary>
        /// 获取角色下用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public StringBuilder GetAbpUserIdByRoleId(int roleId)
        {
            var query = (from a in _userRoleRepository.GetAll()
                         join b in _userRepository.GetAll() on a.UserId equals b.Id
                         where a.RoleId == roleId
                         select b.Id
                         ).ToList();
            StringBuilder sb = new StringBuilder();
            if (query != null)
            {
                foreach (var u in query)
                {

                    sb.Append(MemberPerfix.UserPREFIX);
                    sb.Append(u);
                    sb.Append(",");
                }
            }
            return sb;
        }
        public void UpdateRelationRoleAndPermission(Guid relationId, long userId, long relationUserId)
        {
            AddRelationRole(relationId, userId, relationUserId);
            AddRelationPermission(relationId, userId, relationUserId);
        }

        public void DeleteRelationRoleAndPermission(Guid relationId)
        {
            var relationRoles = _userRoleRelationRepository.GetAll().Where(x => x.RelationId == relationId).ToList();
            foreach (var item in relationRoles)
            {
                _userRoleRelationRepository.Delete(item);
            }
            var relationPermissions = _userPermissionRelationRepository.GetAll().Where(x => x.RelationId == relationId).ToList();
            foreach (var item in relationPermissions)
            {
                _userPermissionRelationRepository.Delete(item);
            }
        }
        public void AddRelationRole(Guid relationId, long userId, long relationUserId)
        {

            var relationRoles = _userRoleRelationRepository.GetAll().Where(x => x.RelationId == relationId).ToList();
            foreach (var item in relationRoles)
            {
                _userRoleRelationRepository.Delete(item);
            }
            var roles = _userRoleRepository.GetAll().Where(x => x.UserId == userId).ToList();
            foreach (var item in roles)
            {
                var info = new UserRoleRelation()
                {
                    RoleId = item.RoleId,
                    UserId = relationUserId,
                    RelationId = relationId
                };
                _userRoleRelationRepository.Insert(info);
            }
        }
        public void AddRelationPermission(Guid relationId, long userId, long relationUserId)
        {

            var relationPermissions = _userPermissionRelationRepository.GetAll().Where(x => x.RelationId == relationId).ToList();
            foreach (var item in relationPermissions)
            {
                _userPermissionRelationRepository.Delete(item);
            }
            var permissions = _userPermissionSettingRepository.GetAll().Where(x => x.UserId == userId).ToList();
            foreach (var item in permissions)
            {
                var info = new UserPermissionRelation()
                {
                    IsGranted = item.IsGranted,
                    Name = item.Name,
                    UserId = relationUserId,
                    RelationId = relationId
                };
                _userPermissionRelationRepository.Insert(info);
            }

        }

        /// <summary>
        /// 获取角色下用户id
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<User> GetAbpUsersByRoleCode(string code)
        {
            var query = from user in _userRepository.GetAll()
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId
                        join r in _roleRepository.GetAll() on ur.RoleId equals r.Id
                        where r.Name.Contains(code)
                        select user;

            return query.ToList();
        }
        /// <summary>
        /// 获取角色下用户ids
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public List<User> GetAbpUsersByRoleCodes(List<string> codes)
        {
            var query = from user in _userRepository.GetAll()
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId
                        join r in _roleRepository.GetAll() on ur.RoleId equals r.Id
                        where codes.Contains(r.Name)
                        select user;

            return query.ToList();
        }
        /// <summary>
        /// 获取角色下用户ids
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public List<User> GetAbpUsersByPermissions(List<string> permissions)
        {
            var query = from user in _userRepository.GetAll()
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId
                        join r in _roleRepository.GetAll() on ur.RoleId equals r.Id
                        join p in _rolePermissionSettingRepository.GetAll() on r.Id equals p.RoleId
                        where permissions.Contains(p.Name) && p.IsGranted
                        select user;
            var users= query.ToList();
            var queryUser = from user in _userRepository.GetAll()
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId
                        join r in _roleRepository.GetAll() on ur.RoleId equals r.Id
                        join p in _userPermissionSettingRepository.GetAll() on user.Id equals p.UserId
                        where permissions.Contains(p.Name) && p.IsGranted
                        select user;
            users.AddRange(queryUser.ToList());
            return users.Distinct().ToList();
        }
        /// <summary>
        /// 获取角色Id下用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<User> GetAbpUsersByRoleId(int roleId)
        {
            var roles = _userRoleRepository.GetAll().Where(x => x.RoleId == roleId).Select(x => x.UserId).ToList();
            var users = _userRepository.GetAll().Where(x => roles.Contains(x.Id)).ToList();
            return users.ToList();
        }


        public List<User> GetAbpUsersByOrgPostIds(string orgPostIds)
        {
            if (orgPostIds.IsNullOrWhiteSpace()) return new List<User>();
            var orgPostIdArry = orgPostIds.Split(',').ToList();
            for (int i = 0; i < orgPostIdArry.Count(); i++)
            {
                orgPostIdArry[i] = MemberPerfix.RemovePrefix(orgPostIdArry[i]);
            }
            var query = from userpost in _userPostRepository.GetAll()
                        join u in _userRepository.GetAll() on userpost.UserId equals u.Id
                        where orgPostIdArry.Contains(userpost.OrgPostId.ToString())
                        select u;
            return query.ToList();
        }


        //public List<UserText> GetAllUsersOrg(string idString)
        //{
        //    if (idString.IsNullOrEmpty())
        //    {
        //        return new List<UserText>();
        //    }
        //    string[] idArray = idString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    var userList = new List<UserText>();
        //    foreach (string id in idArray)
        //    {
        //        if (id.StartsWith(MemberPerfix.UserPREFIX))//人员
        //        {
        //            var user = _userRepository.Get(MemberPerfix.RemovePrefix(id).ToLong());
        //            var tmp = new UserText() {  Id=id,Name=user.Name};
        //            userList.Add(tmp);
        //        }
        //        else if (id.IsLong())//机构
        //        {
        //            userList.AddRange(GetAllUsersById(id.ToLong()));
        //        }
        //        else if (id.StartsWith(MemberPerfix.DepartmentPREFIX)) //部门领导
        //        {
        //            var chargleader = GetChargeLeaderById(MemberPerfix.RemovePrefix(id).ToLong());
        //            var tmp = new UserText() { Id = id, Name = chargleader.Name };
        //            userList.Add(tmp);
        //        }
        //        else if (id.StartsWith(MemberPerfix.DepartmentMemberPREFIX)) //部门直属成员
        //        {
        //            //var users = GetAllByOrganizeIDArray(new long[] { MemberPerfix.RemovePrefix(id).ToLong() });
        //            //var tmp = users.Select(ite => new UserText() {
        //            //     Id=
        //            //}); new UserText() { Id = id, Name = users. };
        //            //userList.Add(tmp);
        //            //暂未实现
        //        }
        //        else if (id.StartsWith(MemberPerfix.WorkGroupPREFIX))//角色组
        //        {
        //            //角色组暂未实现
        //        }
        //    }
        //    userList.RemoveAll(p => p == null);
        //    return userList.Distinct(new UserTextEqualityComparer()).ToList();
        //}

        public async Task<List<User>> GetAllUsersAsync(string idString)
        {
            if (idString.IsNullOrEmpty())
            {
                return new List<User>();
            }
            string[] idArray = idString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var userList = new List<User>();
            foreach (string id in idArray)
            {
                if (id.StartsWith(MemberPerfix.UserPREFIX))//人员
                {
                    var user = _userRepository.Get(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.Add(user);
                }
                else if (id.IsLong())//机构
                {
                    userList.AddRange(GetAllUsersById(id.ToLong()));
                }
                else if (id.StartsWith(MemberPerfix.DepartmentPREFIX)) //部门领导
                {
                    var leader = GetLeaderById(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.AddRange(leader);
                }
                else if (id.StartsWith(MemberPerfix.DepartmentFGLeaderPREFIX)) //部门分管领导
                {
                    var chargleader = GetChargeLeaderById(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.AddRange(chargleader);
                }
                else if (id.StartsWith(MemberPerfix.DepartmentMemberPREFIX)) //部门直属成员
                {
                    var users = GetAllByOrganizeIDArray(new long[] { MemberPerfix.RemovePrefix(id).ToLong() });
                    userList.AddRange(users);
                }
                else if (id.StartsWith(MemberPerfix.AllUserPREFIX)) //部门直属成员
                {
                    var users = GetAllUsersById(MemberPerfix.RemovePrefix(id).ToLong());
                    userList.AddRange(users.Distinct());
                }
                else if (id.StartsWith(MemberPerfix.WorkGroupPREFIX))//角色组
                {
                    //角色组暂未实现
                }
                else if (id.StartsWith(MemberPerfix.PostPREFIX))
                {
                    var users = GetAbpUsersByOrgPostIds(id);
                    userList.AddRange(users.Distinct());
                }
            }
            userList.RemoveAll(p => p == null);
            return userList.Distinct(new UsersEqualityComparer()).ToList();
        }


        public List<User> GetAllUsersById(long id)
        {
            var childs = _organizationUnitManager.FindChildren(id, true);
            var ids = new List<long>();
            ids.Add(id);
            foreach (var child in childs)
            {
                ids.Add(child.Id);
            }
            var organizationUnitsServer = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
            var retusers = GetAllByOrganizeIDArray(ids.ToArray());
            return retusers;
        }

        public List<User> GetAllByOrganizeIDArray(long[] organizeIDArray)
        {
            if (organizeIDArray == null || organizeIDArray.Length == 0)
            {
                return new List<User>();
            }
            var query = from u in _userRepository.GetAll()
                        join ur in _userOrganizationUnitRepository.GetAll() on u.Id equals ur.UserId
                        where organizeIDArray.Contains(ur.OrganizationUnitId)
                        select u;
            var result = query.ToList();
            return result;
        }


        /// <summary>
        /// 得到一个人员的部门领导
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetLeader(long userId)
        {
            //var mainStation = GetMainStation(userID);
            //if (mainStation == null)
            //{
            //    return "";
            //}
            var station = _userOrganizationUnitRepository.FirstOrDefault(r => r.UserId == userId && r.IsMain);

            if (station == null)
            {
                return "";
            }
            var org = _organizationUnitRepository.Get(station.OrganizationUnitId);
            if (!org.Leader.IsNullOrEmpty())
            {
                return org.Leader;
            }
            var parents = GetAllParent(org.Code);
            foreach (var parent in parents)
            {
                if (!parent.Leader.IsNullOrEmpty())
                {
                    return parent.Leader;
                }
            }
            return "";
        }

        /// <summary>
        /// 得到一个人员的部门领导
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<User> GetLeaderUsers(long userId)
        {
            var leaderStr = GetLeader(userId);
            return GetAllUsers(leaderStr);
        }

        /// <summary>
        /// 得到一个人员的分管领导
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetChargeLeader(long userID)
        {
            //var mainStation = GetMainStation(userID);
            //if (mainStation == null)
            //{
            //    return "";
            //}


            var station = _userOrganizationUnitRepository.FirstOrDefault(r => r.UserId == userID && r.IsMain);
            if (station == null)
            {
                return "";
            }
            var org = _organizationUnitRepository.Get(station.OrganizationUnitId);
            if (!org.ChargeLeader.IsNullOrEmpty())
            {
                return org.ChargeLeader;
            }
            var parents = GetAllParent(org.Code);
            foreach (var parent in parents)
            {
                if (!parent.ChargeLeader.IsNullOrEmpty())
                {
                    return parent.ChargeLeader;
                }
            }
            return "";
        }


        /// <summary>
        /// 得到一个人员的分管领导
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<User> GetChargeLeaderUsers(long userId)
        {
            var chargeLeaderStr = GetChargeLeader(userId);
            return GetAllUsers(chargeLeaderStr);
        }

        /// <summary>
        /// 获取一个部门的分管领导
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<User> GetChargeLeaderById(long orgId)
        {
            var ret = new List<User>();
            var org = _organizationUnitRepository.Get(orgId);
            if (!org.ChargeLeader.IsNullOrWhiteSpace())
            {
                var chargeLeaderAarry = org.ChargeLeader.Split(",");
                foreach (var item in chargeLeaderAarry)
                {
                    var user = _userRepository.Get(MemberPerfix.RemovePrefix(item).ToLong());
                    ret.Add(user);
                }
            }
            else
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该部门未设置分管领导。");
            return ret;
        }
        /// <summary>
        /// 获取一个部门的部门领导
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<User> GetLeaderById(long orgId)
        {
            var ret = new List<User>();
            var org = _organizationUnitRepository.Get(orgId);
            if (!org.Leader.IsNullOrWhiteSpace())
            {
                var leaderAarry = org.Leader.Split(",");
                foreach (var item in leaderAarry)
                {
                    var user = _userRepository.Get(MemberPerfix.RemovePrefix(item).ToLong());
                    ret.Add(user);
                }

            }
            else
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该部门未设置部门领导。");
            return ret;
        }
        /// <summary>
        /// 根据部门id获取部门下部门领导岗位的用户
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public string GetChargeLeaderByIds(long orgId)
        {
            //var leaderpost = _orgPostRepository.GetAll().FirstOrDefault(ite => ite.OrganizationUnitId == orgId && ite.Level == 0);
            //if (leaderpost == null)
            //{
            //    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该部门未设置领导岗位。");
            //}

            //var leaderuser = _userPostRepository.GetAll().FirstOrDefault(ite => ite.OrgPostId == leaderpost.Id);//
            //if (leaderuser == null)
            //{
            //    return null;
            //}
            //else
            //{
            //    return "u_" + leaderuser.UserId;
            //}
            var org = _organizationUnitRepository.Get(orgId);
            if (org == null)
            {
                return "";
            }
            if (string.IsNullOrWhiteSpace(org.ChargeLeader))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"该部门【{org.DisplayName}】未设置领导。");
            }
            return org.ChargeLeader;
        }
        /// <summary>
        /// 得到一个人员的上级部门主管
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetParentDeptLeader(long userID)
        {
            //var mainStation = GetMainStation(userID);
            //if (mainStation == null)
            //{
            //    return "";
            //}
            var station = _userOrganizationUnitRepository.FirstOrDefault(r => r.UserId == userID && r.IsMain);
            if (station == null)
            {
                return "";
            }
            var org = _organizationUnitRepository.Get(station.OrganizationUnitId);
            var parents = GetAllParent(org.Code);
            foreach (var parent in parents)
            {
                if (!parent.Leader.IsNullOrEmpty())
                {
                    return parent.Leader;
                }
            }
            return "";
        }


        /// <summary>
        /// 得到一个用户所在部门
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public WorkFlowOrganizationUnits GetDeptByUserID(long userID)
        {
            var station = _userOrganizationUnitRepository.FirstOrDefault(r => r.UserId == userID && r.IsMain);
            if (station == null)
            {
                return null;
            }
            var org = _organizationUnitRepository.Get(station.OrganizationUnitId);
            return org;
            //var parents = GetAllParent(org.Code);
            //parents.Reverse();
            //foreach (var parent in parents)
            //{
            //    if (parent.Type == 2)
            //    {
            //        return parent;
            //    }
            //}
            //return null;
        }



        /// <summary>
        /// 查询一个组织的所有上级
        /// </summary>
        private List<WorkFlowOrganizationUnits> GetAllParent(string number)
        {
            if (number.IsNullOrEmpty()) return new List<WorkFlowOrganizationUnits>();
            var query = _organizationUnitRepository.GetAll().Where(r => number.Contains(r.Code.ToString())).OrderBy(r => r.Depth);
            return query.ToList();
        }


        /// <summary>
        /// 得到一组机构的名称(逗号分隔，有前缀)
        /// </summary>
        /// <param name="idString"></param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public string GetNames(string idString, string split = ",")
        {
            if (idString.IsNullOrEmpty())
            {
                return "";
            }
            string[] array = idString.Split(',');
            StringBuilder sb = new StringBuilder(array.Length * 50);
            int i = 0;
            foreach (var arr in array)
            {
                if (arr.IsNullOrEmpty())
                {
                    continue;
                }
                string code = "";
                string orgid = "";
                sb.Append(GetName(arr, out code, out orgid));
                if (i++ < array.Length - 1)
                {
                    sb.Append(split);
                }
            }
            return sb.ToString();
        }

        public List<UserOrgShow> GetNamesArr(UserOrgShowInput input, string split = ",")
        {
            if (input.ids.IsNullOrEmpty())
            {
                return null;
            }
            var ret = new List<UserOrgShow>();
            string[] array = input.ids.Split(',');
            //var id = 0;//如果idstring是个整数，则当成userid 这里是当组织控件类型为选择用户的时候
            switch (input.selectTypes)
            {
                case 0://人员
                    int i = 0;
                    foreach (var arr in array)
                    {
                        if (arr.IsNullOrEmpty())
                        {
                            continue;
                        }
                        var code = "";
                        var orgid = "";
                        var t = new UserOrgShow() { Id = arr, Title = GetName(arr, out code, out orgid), Code = code, OrgId = orgid };
                        ret.Add(t);
                    }
                    return ret;
                    break;
                case 1://部门
                    var ids = Array.ConvertAll<string, long>(array, new Converter<string, long>(ite => long.Parse(ite))).ToList();
                    var o = _organizationUnitRepository.GetAll().Where(ite => ids.Contains(ite.Id)).ToList();
                    ret = o.Select(ite => new UserOrgShow()
                    {
                        Id = ite.Id.ToString(),
                        Code = ite.Code,
                        OrgId = ite.Id.ToString(),
                        Title = ite.DisplayName
                    }).ToList();

                    return ret;
                case 2://用户
                    var ids2 = Array.ConvertAll<string, long>(array, new Converter<string, long>(ite => long.Parse(ite))).ToList();
                    var u = _userRepository.GetAll().Where(ite => ids2.Contains(ite.Id)).ToList();
                    foreach (var ite in u)
                    {
                        var orgs = UserManager.GetOrganizationUnitsAsync(ite).Result;
                        var t = new UserOrgShow();
                        t.Id = ite.Id.ToString();
                        t.Title = ite.Name;
                        if (orgs != null)
                        {
                            t.Code = string.Join(",", orgs.Select(ie => ie.Code));
                            t.OrgId = string.Join(",", orgs.Select(ie => ie.Id));
                        }
                        ret.Add(t);
                    }

                    return ret;
                default:
                    return null;
            }


        }

        /// <summary>
        /// 根据ID得到名称(有前缀的情况)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetName(string id, out string code, out string orgid)
        {
            code = "";
            orgid = "";
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                string name = string.Empty;
                if (id.IsLong())//机构
                {
                    var org = _organizationUnitRepository.Get(id.ToLong());
                    code = org.Code;
                    orgid = org.Id.ToString();
                    return org.DisplayName;
                }
                else if (id.StartsWith(MemberPerfix.UserPREFIX))//用户
                {
                    var user = _userRepository.Get(MemberPerfix.RemovePrefix(id).ToLong());
                    var orgs = UserManager.GetOrganizationUnitsAsync(user).Result.Where(ite => ite.IsDeleted == false);
                    if (orgs != null)
                    {
                        code = string.Join(",", orgs.Select(ite => ite.Code));
                        orgid = string.Join(",", orgs.Select(ite => ite.Id));
                    }
                    return user.Name;
                }
                else if (id.StartsWith(MemberPerfix.DepartmentPREFIX))//部门-领导
                {
                    string uid = id.Replace(MemberPerfix.DepartmentPREFIX, "");
                    long deptID;
                    if (!uid.IsLong(out deptID))
                    {
                        return "";
                    }
                    else
                    {
                        var org = _organizationUnitRepository.Get(deptID);
                        var users = GetLeaderById(deptID);
                        return org.DisplayName + $"[领导:{string.Join(",", users.Select(r => r.Name))}]";

                    }
                }
                else if (id.StartsWith(MemberPerfix.DepartmentFGLeaderPREFIX))//部门-分管领导
                {
                    string uid = id.Replace(MemberPerfix.DepartmentFGLeaderPREFIX, "");
                    long deptID;
                    if (!uid.IsLong(out deptID))
                    {
                        return "";
                    }
                    else
                    {
                        var org = _organizationUnitRepository.Get(deptID);
                        var users = GetChargeLeaderById(deptID);
                        return org.DisplayName + $"[分管领导:{string.Join(",", users.Select(r => r.Name))}]";
                    }
                }

                else if (id.StartsWith(MemberPerfix.DepartmentMemberPREFIX))//部门直属人
                {
                    string uid = id.Replace(MemberPerfix.DepartmentMemberPREFIX, "");
                    long deptID;
                    if (!uid.IsLong(out deptID))
                    {
                        return "";
                    }
                    else
                    {
                        var org = _organizationUnitRepository.Get(deptID);
                        code = org.Code;
                        orgid = org.Id.ToString();
                        return org.DisplayName + "[直属成员]";
                    }
                }

                else if (id.StartsWith(MemberPerfix.DepartmentIdPREFIX))//部门直属人
                {
                    string uid = id.Replace(MemberPerfix.DepartmentIdPREFIX, "");
                    long deptID;
                    if (!uid.IsLong(out deptID))
                    {
                        return "";
                    }
                    else
                    {
                        var org = _organizationUnitRepository.Get(deptID);
                        code = org.Code;
                        orgid = org.Id.ToString();
                        return org.DisplayName;

                    }
                }
                else if (id.StartsWith(MemberPerfix.UserIdPREFIX))//用户
                {
                    var user = _userRepository.Get(MemberPerfix.RemovePrefix(id).ToLong());
                    var orgs = UserManager.GetOrganizationUnitsAsync(user).Result.Where(ite => ite.IsDeleted == false);
                    if (orgs != null)
                    {
                        code = string.Join(",", orgs.Select(ite => ite.Code));
                        orgid = string.Join(",", orgs.Select(ite => ite.Id));
                    }
                    return user.Name;
                }
                else if (id.StartsWith(MemberPerfix.AllUserPREFIX))
                {
                    var torgid = long.Parse(id.Replace(MemberPerfix.AllUserPREFIX, ""));
                    var org = _organizationUnitRepository.FirstOrDefault(torgid);
                    if (org == null)
                    {
                        return "[该部门已被删除]";
                    }
                    code = org.Code;
                    orgid = org.Id.ToString();
                    return org.DisplayName + "[所有人员]";
                }
                else if (id.StartsWith(MemberPerfix.PostPREFIX))
                {
                    var orgpostid = Guid.Parse(id.Replace(MemberPerfix.PostPREFIX, ""));
                    var postInfoService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();
                    return postInfoService.GetUserNameByOrgPostId(orgpostid);
                }

                //else if (id.StartsWith(WorkGroup.PREFIX))//工作组
                //{
                //    string uid = WorkGroup.RemovePrefix(id);
                //    Guid wid;
                //    if (!uid.IsGuid(out wid))
                //    {
                //        return "";
                //    }
                //    else
                //    {
                //        return new WorkGroup().GetName(wid);
                //    }
                //}

                return "";
            }
        }



        /// <summary>
        /// 用户是否是部门领导
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool IsChargerLeader(long userId, long orgId)
        {
            try
            {
                var orgModel = _organizationUnitRepository.Get(orgId);
                if (MemberPerfix.RemovePrefix(orgModel.ChargeLeader).ToLong() == userId)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 用户是否是分管领导
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool IsChargerLeader(long userId)
        {
            try
            {
                var orgModel = GetDeptByUserID(userId);
                if (!string.IsNullOrEmpty(orgModel.ChargeLeader))
                {
                    var leaders = orgModel.ChargeLeader.Split(",");
                    foreach (var item in leaders)
                    {
                        if (!string.IsNullOrEmpty(item) && MemberPerfix.RemovePrefix(item).ToLong() == userId)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        /// <summary>
        /// 用户是否是部门领导
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool IsLeader(long userId)
        {
            try
            {
                var orgModel = GetDeptByUserID(userId);
                if (!string.IsNullOrEmpty(orgModel.ChargeLeader))
                {
                    var leaders = orgModel.Leader.Split(",");
                    foreach (var item in leaders)
                    {
                        if (!string.IsNullOrEmpty(item) && MemberPerfix.RemovePrefix(item).ToLong() == userId)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        /// <summary>
        /// 用户是否是部门领导或分管领导
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool IsChargerLeaderOrDivision(long userId)
        {
            try
            {
                var userManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<UserManagerNotRemote>();
                var userRoles = userManager.GetRoles(userId);
                if (IsChargerLeader(userId) || userRoles.Any(x => string.Compare(x, "FGLD", true) == 0))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //[AbpAuthorize]
        //public List<User> GetAllUsersDown()
        //{
        //    var orgs = _organizationUnitRepository.GetAll().Where(r => r.ChargeLeader.Contains(AbpSession.UserId.Value.ToString()));

        //}

        /// <summary>
        /// 设置用户岗位，并更新权限。
        /// </summary>
        /// <param name="orgPostIds"></param>
        /// <returns></returns>
        public async Task<bool> SetUserPost(long userId, List<Guid> orgPostIds, Guid mainPost)
        {
            var user = await UserManager.GetUserByIdAsync(userId);
            //1.原来的岗位
            var oldpost = _userPostRepository.GetAll().Where(ite => ite.UserId == userId).ToList();

            var oldpostids = oldpost.Select(ite => ite.OrgPostId).ToList();
            //2.需要新增的岗位
            var addpost = orgPostIds.Except(oldpostids).ToList();

            //3.需要删除的岗位
            var delpost = oldpostids.Except(orgPostIds).ToList();

            var addorg = new List<long>();
            var delorg = new List<long>();
            long mainorg = 0;
            var addroles = new List<string>();
            var roles = await UserManager.GetRolesAsync(user);
            //4.角色重置并更新岗位
            if (addpost != null && addpost.Count > 0)
            {
                foreach (var a in addpost)
                {
                    var postinfo = _orgPostRepository.GetAll().FirstOrDefault(ite => ite.Id == a);
                    var orgid = postinfo.OrganizationUnitId;
                    var ismainorg = false;
                    if (mainPost == a)
                    {
                        ismainorg = true;
                        mainorg = orgid;
                    }
                    if (addorg.Exists(ite => ite == orgid) == false)
                    {
                        addorg.Add(orgid);
                    }
                    if (postinfo == null)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"岗位信息【{a}】不存在");
                    }
                    _userPostRepository.Insert(new UserPosts()
                    {
                        OrgPostId = a,
                        IsMain = ismainorg,
                        OrgId = postinfo.OrganizationUnitId,
                        PostId = postinfo.PostId,
                        UserId = userId
                    });

                }

                var addrs = _organizationUnitPostsRoleRepository.GetAll().Where(ite => addpost.Contains(ite.OrgPostId)).Select(ite => ite.RoleName).ToList();
                foreach (var a in addrs)
                {
                    if (roles.FirstOrDefault(ite => ite == a) == null)
                    {
                        roles.Add(a);
                    }
                }

            }
            if (delpost != null && delpost.Count > 0)
            {
                foreach (var d in delpost)
                {
                    var postinfo = _orgPostRepository.GetAll().FirstOrDefault(ite => ite.Id == d);
                    var orgid = postinfo.OrganizationUnitId;
                    if (delorg.Exists(ite => ite == orgid) == false)
                    {
                        delorg.Add(orgid);
                    }
                    _userPostRepository.Delete(ite => ite.OrgPostId == d && ite.UserId == userId);
                }
                var delrs = _organizationUnitPostsRoleRepository.GetAll().Where(ite => delpost.Contains(ite.OrgPostId)).Select(ite => ite.RoleName).ToList();
                foreach (var a in delrs)
                {
                    if (roles.FirstOrDefault(ite => ite == a) != null)
                    {
                        roles.Remove(a);
                    }
                }
            }
            await UserManager.SetRoles(user, roles.ToArray());
            //5.更新组织架构
            foreach (var o in addorg)
            {
                _userOrganizationUnitRepository.Insert(new WorkFlowUserOrganizationUnits()
                {
                    UserId = userId,
                    IsMain = o == mainorg,
                    OrganizationUnitId = o,
                });
            }
            foreach (var o in delorg)
            {
                _userOrganizationUnitRepository.Delete(ite => ite.OrganizationUnitId == o && ite.UserId == userId);
            }
            return true;
        }

    }


    public class UsersEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User user1, User user2)
        {
            return user1 == null || user2 == null || user1.Id == user2.Id;
        }
        public int GetHashCode(User user)
        {
            return user.ToString().GetHashCode();
        }
    }

    public class UserTextEqualityComparer : IEqualityComparer<UserText>
    {
        public bool Equals(UserText x, UserText y)
        {
            return x == null || y == null || x.Id == y.Id;
        }

        public int GetHashCode(UserText obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
