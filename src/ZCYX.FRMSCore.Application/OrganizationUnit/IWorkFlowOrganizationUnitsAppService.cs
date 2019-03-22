using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IWorkFlowOrganizationUnitsAppService : IApplicationService
    {
        Task<ListResultDto<WorkFlowOrganizationUnitDto>> GetOrganizationUnits();

        //Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input);
        Task<PagedResultDto<WorkFlowOrganizationUnitUserListDto>> GetOrganizationUnitUsersAndUnder(GetWorkFlowOrganizationUnitUsersInput input);

        Task SetUserToOrganizationUnit(OrganizationUnitUserInput input);

        Task<WorkFlowOrganizationUnitDto> CreateOrganizationUnit(CreateWorkFlowOrganizationUnitInput input);

        Task<WorkFlowOrganizationUnitDto> GetAsync(EntityDto<long> input);

        Task<WorkFlowOrganizationUnitDto> UpdateOrganizationUnit(UpdateWorkFlowOrganizationUnitInput input);

        //Task<WorkFlowOrganizationUnitDto> MoveOrganizationUnit(MoveWorkFlowOrganizationUnitInput input);

        Task DeleteOrganizationUnit(EntityDto<long> input);

        Task AddUserToOrganizationUnit(UserToWorkFlowOrganizationUnitInput input);

        //Task RemoveUserFromOrganizationUnit(UserToWorkFlowOrganizationUnitInput input);

        Task<bool> IsInOrganizationUnit(UserToWorkFlowOrganizationUnitInput input);
        Task<PagedResultDto<WorkFlowOrganizationUnitUserListDto>> GetUserUnderPost(GetUserUnderPostSearch input);
        Task<GetOrganizationUnitTreeOutput> GetOrganizationUnitTree(GetOrganizationUnitTreeInput input);

        Task<GetOrganizationUnitTreeOutput> GetOrganizationUnitTreeNew(GetOrganizationUnitTreeNewInput input);

        Task<OrganizationUnitTreeOutput> GetOrganizationUnitChildren(NullableIdDto<long> input);

        Task<List<UserUnderOrgProssceStaticOutput>> GetUserUnderOrgProssceStatic(UserUnderOrgProssceStaticInput input);
        Task<List<OrganizationUnitUserOutput>> GetUserAllOrgs(NullableIdDto<long> input);
        Task<List<UserUnderOrgOutput>> GetUserWithCurrentAndUnderOrg(UserUnderOrgProssceStaticInput input);
        /// <summary>
        /// 将用户id转换为用户名（只限全部是人员）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<UserText> GetUserNameForShow(string ids);

        /// <summary>
        /// 将用户id转换为用户名（只限工作流上用）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<UserOrgShow> GetUserNameForWorkFlow(UserOrgShowInput input);


        /// <summary>
        /// 删除部门的岗位
        /// </summary>
        /// <param name="input">部门的岗位id</param>
        /// <returns></returns>
        Task DeleteOrgPost(EntityDto<Guid> input);


        Task<UserWorkFlowOrganizationUnitDto> GetUserPostInfo(NullableIdDto<long> input, NullableIdDto<long> orgInput);



        UserWorkFlowOrganizationUnitDto GetUserPostInfoV2(NullableIdDto<long> input, NullableIdDto<long> orgInput);


        /// <summary>
        /// 更新部门名字
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        bool UpdateOrgName(UpdateOrganizationName input);

    }
}
