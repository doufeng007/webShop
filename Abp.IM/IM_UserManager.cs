using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Runtime.Caching;
using WebClientServer;
using Abp.UI;
using Abp.Configuration;
using Newtonsoft.Json;
using System.Linq;
using ZCYX.FRMSCore.Configuration;
using Abp.Domain.Repositories;

namespace Abp.IM
{
    [RemoteService(IsEnabled = false)]
    public class IM_UserManager : ApplicationService
    {
        private IHostingEnvironment hostingEnv;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<Setting, long> _settingRepository;

        public string IM_access_token { get; set; }

        public string IM_expires_in { get; set; }

        public string IM_applicationId { get; set; }

        public string IM_org_name { get; set; }


        public string IM_app_name { get; set; }


        public string IM_client_id { get; set; }


        public string IM_client_secret { get; set; }


        public string IM_ApiUrl { get; set; }


        public IM_UserManager(IHostingEnvironment env, ICacheManager cacheManager, ISettingManager settingManager, IRepository<Setting, long> settingRepository)
        {
            hostingEnv = env;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
            _cacheManager = cacheManager;
            _settingManager = settingManager;
            IM_org_name = _appConfiguration["IM:OrgName"];
            IM_app_name = _appConfiguration["IM:AppName"];
            IM_client_id = _appConfiguration["IM:ClientID"];
            IM_client_secret = _appConfiguration["IM:ClientSecret"];
            IM_ApiUrl = _appConfiguration["IM:apiURL"];
            _settingRepository = settingRepository;

        }




        public IM_Access_TokenReturnDto GetAccess_TokenModel()
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/token";
            var parameter = new { grant_type = "client_credentials", client_id = IM_client_id, client_secret = IM_client_secret };
            //var parameter = new { grant_type = "password", username = IM_client_id, client_secret = IM_client_secret };
            var retdata = HttpClientHelper.PostResponse<IM_Access_TokenReturnDto>(url, parameter);
            return retdata;
        }

        public IM_Access_TokenReturnDto GetIM_Access_TokenFromCache()
        {
            var cacheName = "IM_access_token";
            var ret = _cacheManager.GetCache(cacheName);
            var access_TokenModel = ret.GetOrDefault(cacheName);
            if (access_TokenModel == null)
            {
                var model = GetAccess_TokenModel();
                ret.SetAsync(cacheName, model, new TimeSpan(0, 0, model.expires_in));
                return model;
            }
            else
                return access_TokenModel as IM_Access_TokenReturnDto;
            //return _cacheManager
            //   .GetCache(cacheName)
            //   .Get<string, IM_Access_TokenReturnDto>(cacheName, f => GetAccess_Token());

        }


        public string GetAccess_Token()
        {
            var model = GetIM_Access_TokenFromCache();
            return model.access_token;
        }

        public void CreateIMUsers(List<CreateIMUserInput> input, bool isAddFirstUser = false, string companyName = "")
        {
            if (input.Count == 0)
                return;
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/users";
            var userList = new List<object>();
            foreach (var item in input)
            {
                userList.Add(new { username = item.username, password = item.password, nickname = item.nickname });
            }

            var flag = false;
            var retdata = HttpClientHelper.PostResponseForIM(url, userList, out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "创建IM用户api接口访问失败");
            else
            {
                if (isAddFirstUser)
                {
                    var members = new List<string>();
                    for (int i = 1; i < input.Count; i++)
                    {
                        members.Add(input[i].username);
                    }
                    CreateApplicationGroup(companyName, input.First().username, companyName, members);
                }
                else
                {
                    //var groupId = _appConfiguration.GetValue<string>("IM_CompanyGroupId");
                    var set = _settingRepository.FirstOrDefault(ite => ite.Name == "IM_CompanyGroupId");
                    if (set != null)
                        AddGroupMembers(set.Value, input.Select(r => r.username).ToList());
                }
            }
        }


        public void GetIMUser(GetIMUserInput input)
        {
            throw new NotImplementedException();
        }

        public void DeleteIMUser(string username)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/users/{username}";
            var flag = false;
            var retdata = HttpClientHelper.DeleteResponseForIM(url, out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "删除IM用户api接口访问失败");
        }

        public void UpdateIMUserPassword(string username, string passsword)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/users/{username}/password";
            var parameter = new { newpassword = passsword };
            var flag = false;
            var retdata = HttpClientHelper.PutResponseForIM(url, parameter, out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "更新IM用户密码api接口访问失败");
        }


        public void UpdateIMUserNickname(string username, string nickname)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/users/{username}";
            var parameter = new { nickname = nickname };
            var flag = false;
            var retdata = HttpClientHelper.PutResponseForIM(url, parameter, out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "更新IM用户昵称api接口访问失败");
        }


        public void DisableIMUser(string username)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/users/{username}/deactivate";
            var flag = false;
            var retdata = HttpClientHelper.PostResponseForIM(url, new object(), out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "禁用IM用户api接口访问失败");
        }

        public void EnableIMUser(string username)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/users/{username}/activate";
            var flag = false;
            var retdata = HttpClientHelper.PostResponseForIM(url, new object(), out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "解禁IM用户api接口访问失败");
        }


        /// <summary>
        /// 聊天记录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="time"></param>
        public void GetHistoryMessage(string username, string time)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/chatmessages/{time}";
            var flag = false;
            var retdata = HttpClientHelper.GetResponseForIM(url, out flag);
            if (!flag)
                throw new UserFriendlyException(114, "解禁IM用户api接口访问失败");
        }


        /// <summary>
        /// 获取用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <param name="time"></param>
        public bool GetImIsExistenceUser(string username)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/users/{username}";
            var flag = false;
            var retdata = HttpClientHelper.GetResponseForIM(url, out flag, GetAccess_Token());
            return flag;
        }


        public void CreateApplicationGroup(string groupname, string owner, string desc, List<string> members = null, bool ispublic = true, int maxusers = 200, bool isNeedApproval = false)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/chatgroups";
            var dic_parameter = new Dictionary<string, object>();
            dic_parameter.Add("groupname", groupname);
            dic_parameter.Add("desc", desc);
            dic_parameter.Add("public", ispublic);
            dic_parameter.Add("maxusers", maxusers);
            dic_parameter.Add("members_only", isNeedApproval);
            dic_parameter.Add("allowinvites", false);
            dic_parameter.Add("owner", owner);
            if (members != null && members.Count > 0)
                dic_parameter.Add("members", members.ToArray());
            var flag = false;

            var retdata = HttpClientHelper.PostResponseForIM(url, dic_parameter, out flag, GetAccess_Token());
            var retdataObj = JsonConvert.DeserializeObject<dynamic>(retdata);
            if (!flag)
                throw new UserFriendlyException(114, "创建群组api接口访问失败");
            else
            {
                var newGroupId = (string)retdataObj.data.groupid;
                var set = _settingRepository.FirstOrDefault(ite => ite.Name == "IM_CompanyGroupId");
                if (set != null)
                {
                    set.Value = newGroupId;
                }
                //_settingManager.ChangeSettingForApplication("IM_CompanyGroupId", newGroupId);
            }
        }

        public void AddGroupMember(string groupid, string username)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/chatgroups/{groupid}/users/{username}";
            var flag = false;
            var retdata = HttpClientHelper.PostResponseForIM(url, new object(), out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "添加群组成员api接口访问失败");

        }


        public void AddGroupMembers(string groupid, List<string> usernames)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/chatgroups/{groupid}/users";
            var parameterobj = new { usernames = usernames.ToArray() };
            var flag = false;
            var retdata = HttpClientHelper.PostResponseForIM(url, parameterobj, out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "批量添加群组成员api接口访问失败");

        }

        public void RemoveGroupMember(string groupId, string username)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/chatgroups/{groupId}/users/{username}";
            var flag = false;
            var retdata = HttpClientHelper.DeleteResponseForIM(url, out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "移除群主成员api接口访问失败");
        }

        public void RemoveGroupMembers(string groupId, List<string> usernames)
        {
            var url = $"{IM_ApiUrl}/{IM_org_name}/{IM_app_name}/chatgroups/{groupId}/users/{string.Join(',', usernames)}";
            var flag = false;
            var retdata = HttpClientHelper.DeleteResponseForIM(url, out flag, GetAccess_Token());
            if (!flag)
                throw new UserFriendlyException(114, "移除群主成员api接口访问失败");
        }

    }
}
