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
using Abp.RealTime;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.SignalR.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Project
{
    public class ProjectSupplementAppService : ApplicationService, IProjectSupplementAppService
    {
        private readonly IRepository<ProjectSupplement, Guid> _projectSupplementRepository;
        private readonly IBackgroudWorkJobWithHangFire _backgroudWorkJobWithHangFire;
        //private readonly INoticeCommunicator _noticeCommunicator;
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        public ProjectSupplementAppService(IRepository<ProjectSupplement, Guid> projectSupplementRepository, IBackgroudWorkJobWithHangFire backgroudWorkJobWithHangFire,
            //INoticeCommunicator noticeCommunicator,
            IOnlineClientManager onlineClientManager, IProjectBaseRepository projectBaseRepository, IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository)
        {
            _projectSupplementRepository = projectSupplementRepository;
            _backgroudWorkJobWithHangFire = backgroudWorkJobWithHangFire;
            //_noticeCommunicator = noticeCommunicator;
            _onlineClientManager = onlineClientManager;
            _projectBaseRepository = projectBaseRepository;
            _singleProjectInfoRepository = singleProjectInfoRepository;
        }



        public async Task<PagedResultDto<ProjectSupplementListDto>> GetProjectSupplements(GetProjectSupplementListInput input)
        {
            try
            {
                var query = _projectSupplementRepository.GetAll().Where(r => r.ProjectBaseId == input.ProjectId);
                var count = await query.CountAsync();
                var projectSupplements = await query
                 .OrderBy(input.Sorting)
                 .PageBy(input)
                 .ToListAsync();
                var periodDtos = projectSupplements.MapTo<List<ProjectSupplementListDto>>();
                return new PagedResultDto<ProjectSupplementListDto>(count, periodDtos);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public ListResultDto<ProjectSupplementListDto> GetAllProjectSupplements(GetProjectSupplementListInput input)
        {
            var query = _projectSupplementRepository.GetAll().Where(r => r.ProjectBaseId == input.ProjectId);
            var projectSupplements = query
             .OrderBy(input.Sorting)
             .ToList();
            return new ListResultDto<ProjectSupplementListDto>
            {
                Items = projectSupplements.MapTo<List<ProjectSupplementListDto>>()
            };
        }


        public async Task<bool> ProjectExitSupplements(GetProjectSupplementListInput input)
        {
            var query = _projectSupplementRepository.GetAll().Where(r => r.ProjectBaseId == input.ProjectId && r.HasSupplement == false);
            var exitcount = await query.CountAsync();
            if (exitcount > 0)
                return true;
            else
                return false;

        }


        public void JobForProjectForceSubmit(Guid projectId)
        {
            var singleprojectmodel = _singleProjectInfoRepository.FirstOrDefault(r => r.Id == projectId);
            if (singleprojectmodel == null) return;
            var query = _projectSupplementRepository.GetAll().Where(r => r.ProjectBaseId == projectId && r.HasSupplement == false);
            var count = query.Count();
            if (count > 0)
            {
                Debug.Assert(singleprojectmodel.CreatorUserId != null, "CreatorUserId != null");
                var projectModel = _projectBaseRepository.Get(singleprojectmodel.ProjectId);
                var link = $"/DataAdd?id={projectId}&appraisalTypeId={projectModel.AppraisalTypeId}";
                //var link = $"/Mpa/Project/DataChange?instanceId={projectId}&appTypeId={projectmodel.AppraisalTypeId}&notInAudit=1";

                var onlineclients =
                    _onlineClientManager.GetAllClients().Where(r => r.UserId == singleprojectmodel.CreatorUserId).ToList();
                var signalrNoticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ISignalrNoticeAppService>();

                signalrNoticeService.SendNoticeToClient(onlineclients, projectId.ToString(), "补充资料提示", $"项目{singleprojectmodel.SingleProjectName}缺少{count}处资料，请及时补充", link);
            }
            else
            {
                _backgroudWorkJobWithHangFire.RemoveIfExistsJobForProjectForceSubmit(projectId);
            }

        }



        public async Task CertainSubmite(Guid projectId)
        {
            var projectmodel = _projectBaseRepository.Get(projectId);
            if (projectmodel == null) return;
            var query = _projectSupplementRepository.GetAll().Where(r => r.ProjectBaseId == projectId && r.HasSupplement == false);
            var count = query.Count();
            if (count > 0)
                _backgroudWorkJobWithHangFire.CreateJobForProjectForceSubmit(projectId);

        }


    }
}
