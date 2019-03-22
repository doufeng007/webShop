using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ZCYX.FRMSCore;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Application;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.WorkFlow;

namespace HR
{
    public class EmployeeSignAppService : FRMSCoreAppServiceBase, IEmployeeSignAppService
    {
        private readonly IRepository<EmployeeSign, Guid> _repository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;




        public EmployeeSignAppService(IRepository<EmployeeSign, Guid> repository, IRepository<Employee, Guid> employeeRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , WorkFlowTaskManager workFlowTaskManager, IWorkFlowTaskRepository workFlowTaskRepository)
        {
            _repository = repository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowTaskRepository = workFlowTaskRepository;
        }

        [AbpAuthorize]
        public async Task CreateAsync(CreateEmployeeSignInput input)
        {
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var model = await _repository.GetAll().FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value && r.MakeDate == today);
            if (model == null)
            {
                var entity = new EmployeeSign() { Id = Guid.NewGuid(), GoToWorkTime = DateTime.Now, MakeDate = today, UserId = AbpSession.UserId.Value };
                await _repository.InsertAsync(entity);
            }
            else
            {
                if (input.EmployeeSignType == EmployeeSignType.上班打卡)
                {
                    model.GoToWorkTime = DateTime.Now;
                }
                else
                    model.GoOfWork = DateTime.Now;
                await _repository.UpdateAsync(model);
            }

        }


        [AbpAuthorize]
        public async Task<GetEmployeeSignOutput> GetAsync()
        {
            var ret = new GetEmployeeSignOutput();
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var model = await _repository.GetAll().FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value && r.MakeDate == today);
            if (model == null)
            {
                ret.EmployeeSignType = EmployeeSignType.上班打卡;
            }
            else
            {
                ret.GoOfWork = model.GoOfWork;
                ret.GoToWorkTime = model.GoToWorkTime;
                if (!ret.GoToWorkTime.HasValue)
                {
                    ret.EmployeeSignType = EmployeeSignType.上班打卡;
                }
                else
                {
                    if (!ret.GoOfWork.HasValue)
                    {
                        ret.EmployeeSignType = EmployeeSignType.下班打卡;
                    }
                    else
                    {
                        ret.EmployeeSignType = EmployeeSignType.完成打卡;
                    }
                }
            }

            return ret;
        }

    }
}
