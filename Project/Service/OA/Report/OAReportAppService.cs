using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.WorkFlow;
using Project.Service.OA.Report.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;

namespace Project.Service.OA.Report
{
    public class OAReportAppService : ApplicationService, IOAReportAppService
    {
        private readonly IRepository<OAReport, Guid> _oaReportrepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAReportAppService(IRepository<OAReport, Guid> oaReportrepository, IRepository<User, long> userRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _oaReportrepository = oaReportrepository;
            _userRepository = userRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAReportInputDto input)
        {
            OAReport model = input.MapTo<OAReport>();
            model.Status = 0;
            _oaReportrepository.InsertOrUpdate(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }

        public void Update(OAReportInputDto input) {
            var ret = _oaReportrepository.Get(input.Id.Value);
            ret= input.MapTo(ret);
            _oaReportrepository.Update(ret);
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OAReportDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaReportrepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OAReportDto>();
            //model.UserName = _userRepository.Get(model.CreatorUserId.Value).Surname;

            return model;
        }
        [AbpAuthorize]
        public async Task<PagedResultDto<OAReportListDto>> GetAll(SearchReportInput input)
        {
            var ret = _oaReportrepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            //if (input.ReportWay.HasValue) {
            //    switch (input.ReportWay) {
            //        case ReportWay.我写的:
            //            ret = ret.Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            //            break;
            //        case ReportWay.收到的:
            //            ret = ret.Where(ite => ite.ReportAudits.Contains(userguid));
            //            break;

            //    }
            //}
            if (input.ReportType.HasValue)
            {
                ret = ret.Where(ite => ite.ReportType == input.ReportType.Value);
            }
            if (string.IsNullOrWhiteSpace(input.Key) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.Key));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OAReportListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAReportListDto>(total, model);
        }

        public string UpdateState(Guid input)
        {
            var model = _oaReportrepository.Get(input);
            if (model != null)
            {
                model.Status = 2;//处理中
                return "成功";
            }
            else
            {
                return "失败";
            }
        }
    }
}
