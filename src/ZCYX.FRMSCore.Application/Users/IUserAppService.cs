using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore.Roles.Dto;
using ZCYX.FRMSCore.Users.Dto;

namespace ZCYX.FRMSCore.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);

        Task ResetUserSpecificPermissions(EntityDto<long> input);

        Task UpdateUserPermissions(UpdateUserPermissionsInput input);
        Task<List<UserDto>> GetUserByRoleCode(string code);

        Task UpdateUserAsyn(UpdateUserInput input);

        List<SpecialPermissionDto> GetSpecialPermiss(long userId);
        bool RemoveSpecialPermiss(long id);

        Task<PagedResultDto<UserUnderRoleDto>> GetAbpUsersByRoleId(GetAbpUsersByRoleIdInput input);


        Task UpdateUsersUnderRole(UpdateUsersUnderRoleInput input);

        Task UpdateUsersUnderRoleOneWay(UpdateUsersUnderRoleOneWayInput input);



        /// <summary>
        ///通过userid字符串获取对应的username
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<IMUserInfoDto>> GetUserNameByIds(EntityDto<string> input);

        Task Enable(EntityDto<long> input);
        Task CreateImUser(CreateImUserDto input);
    }
}
