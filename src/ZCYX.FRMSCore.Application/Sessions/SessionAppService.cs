using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Domain.Repositories;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Sessions.Dto;
using ZCYX.FRMSCore.SignalR;
using System.Linq;

namespace ZCYX.FRMSCore.Sessions
{
    public class SessionAppService : FRMSCoreAppServiceBase, ISessionAppService
    {
        private readonly IRepository<OrganizationUnitPosts, Guid> _organizationUnitPostsRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitsRepository;
        private readonly IRepository<PostInfo, Guid> _postRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _orgRepository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        public SessionAppService(IRepository<OrganizationUnitPosts, Guid> organizationUnitPostsRepository
            , IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitsRepository, IRepository<PostInfo, Guid> postRepository,
            IRepository<WorkFlowOrganizationUnits, long> orgRepository, IRepository<UserPosts, Guid> userPostRepository
            ) {
            _organizationUnitPostsRepository = organizationUnitPostsRepository;
            _userOrganizationUnitsRepository = userOrganizationUnitsRepository;
            _postRepository = postRepository;
            _orgRepository = orgRepository;
            _userPostRepository = userPostRepository;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>
                    {
                        { "SignalR", SignalRFeature.IsAvailable }
                    }
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                //var query = from a in _organizationUnitPostsRepository.GetAll()
                //            join b in _userOrganizationUnitsRepository.GetAll() on a.OrganizationUnitId equals b.OrganizationUnitId
                //            join c in _postRepository.GetAll() on a.PostId equals c.Id
                //            join d in _orgRepository.GetAll() on a.OrganizationUnitId equals d.Id
                //            where b.UserId == AbpSession.UserId.Value
                //            select new OrganizationUnitPostsDto { Id = a.Id, PostId = a.PostId, PostName = c.Name, CreationTime = a.CreationTime, OrganizationUnitId = a.OrganizationUnitId, OrganizationName = d.DisplayName, TenantId = a.TenantId };

                var query = from a in _userPostRepository.GetAll()
                            join b in _postRepository.GetAll() on a.PostId equals b.Id
                            join c in _orgRepository.GetAll() on a.OrgId equals c.Id
                            where a.UserId == AbpSession.UserId.Value
                            select new OrganizationUnitPostsDto()
                            {
                                CreationTime = a.CreationTime,
                                Id = a.Id,
                                OrganizationName = c.DisplayName,
                                OrganizationUnitId = a.OrgId,
                                PostId = a.PostId,
                                PostName = b.Name,
                                TenantId = a.TenantId,
                                IsMain=a.IsMain,
                            };
                output.User.Posts = query.ToList();
            }

            return output;
        }
    }
}
