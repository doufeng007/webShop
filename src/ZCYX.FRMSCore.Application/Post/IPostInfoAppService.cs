using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IPostInfoAppService : IAsyncCrudAppService<PostInfoDto, Guid, PagedResultRequestDto, CreatePostInfoDto, PostInfoDto>
    {

        Task<PostInfoDto> GetByNameAsync(string name);


        Task<PagedResultDto<OrganizationUnitPostsDto>> GetOrgPosts(GetOrganizationUnitPostsListInput input);



        string GetNameWithOrgByOrgPostId(Guid orgPostId);


        string GetUserNameByOrgPostId(Guid orgPostId);


        Task<List<OrgPostAllNameDto>> GetPostInfoWithOrgName();

        Task<PagedResultDto<OrganizationUnitPostsDto>> GetMyOrgPosts(long userId);
    }
}
