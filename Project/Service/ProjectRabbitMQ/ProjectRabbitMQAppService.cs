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
using Abp.UI;
using Newtonsoft.Json.Linq;
using Abp.Extensions;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using Abp;
using System.Net.Http;
using System.Net;
using ZCYX.FRMSCore.Web;
using Abp.Reflection.Extensions;
using Abp.Authorization;
using Abp.Authorization.Users;
using ZCYX.FRMSCore.Application;

namespace Project
{
    [RemoteService(IsEnabled = false)]
    public class ProjectRabbitMQAppService : ApplicationService, IProjectRabbitMQAppService
    {
        private readonly IRepository<User, long> _useRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<ContractWithSystem, Guid> _contractWithSystemRepository;
        private readonly IRepository<RealationSystem, Guid> _realationSystemRepository;



        public ProjectRabbitMQAppService(IRepository<User, long> useRepository, IProjectBaseRepository projectBaseRepository,
            IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<ContractWithSystem, Guid> contractWithSystemRepository,
            IRepository<RealationSystem, Guid> realationSystemRepository)
        {
            this._useRepository = useRepository;
            var coreAssemblyDirectoryPath = typeof(RabbitMQRecevieCallBackAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            _projectBaseRepository = projectBaseRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _contractWithSystemRepository = contractWithSystemRepository;
            _realationSystemRepository = realationSystemRepository;

        }

        public void PushProjectToXieZou(Guid projectId)
        {
            var xietongdanweiOrgId = _appConfiguration["App:XietongdanweiOrgId"];
            var xietongdanweiOrgModel = _organizationUnitRepository.Get(xietongdanweiOrgId.ToLong());
            var projectAuditMember = _projectAuditMemberRepository.GetAll().Where(r => r.ProjectBaseId == projectId && r.UserAuditRole == 2).ToList();
            var projectAuditMemberUserIds = projectAuditMember.Select(r => r.UserId).ToList();

            var query = from user in _useRepository.GetAll()
                        join ur in _userOrganizationUnitRepository.GetAll() on user.Id equals ur.UserId
                        join org in _organizationUnitRepository.GetAll() on ur.OrganizationUnitId equals org.Id
                        join c in _contractWithSystemRepository.GetAll() on user.Id equals c.UserId into g
                        from c in g.DefaultIfEmpty()
                        join s in _realationSystemRepository.GetAll() on c.SystemId equals s.Id into f
                        from s in f.DefaultIfEmpty()
                        where projectAuditMemberUserIds.Contains(user.Id)
                        //&& org.Type == 2
                        && org.Code.Contains(xietongdanweiOrgModel.Code)
                        select new { user, system = s };
            if (query.Count() > 0)
            {
                foreach (var item in query.ToList())
                {
                    if (item.system != null)
                    {
                        var queueName = item.system.Code;
                        var obj = new { MessageType = "1", Parameter = projectId.ToString() };
                        var message = JToken.FromObject(obj);
                        //RabbitMqPublish.PushInfo("FRMS", message.ToString());
                        RabbitMqPublish.PushInfo(queueName, message.ToString());
                    }


                }

            }


        }




















    }
}
