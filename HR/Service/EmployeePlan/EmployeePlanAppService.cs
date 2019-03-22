using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.File;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using Abp.WorkFlow;
using HR.Service;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class EmployeePlanAppService : FRMSCoreAppServiceBase, IEmployeePlanAppService
    {
        private readonly IRepository<EmployeePlan, Guid> _repository;
        private readonly IRepository<EmployeeResult, Guid> _resultrepository;
        private readonly IRepository<EmployeeResume, Guid> _resumerepository;
        private readonly IRepository<PostInfo, Guid> _postrepository;
        private readonly IRepository<OrganizationUnitPosts, Guid> _orgpostrepository;
        private readonly IRepository<OrganizationUnit, long> _organizarepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        public EmployeePlanAppService(IRepository<EmployeePlan, Guid> repository, IRepository<EmployeeResult, Guid> resultrepository, IRepository<PostInfo, Guid> postrepository
            , WorkFlowCacheManager workFlowCacheManager, ProjectAuditManager projectAuditManager, IRepository<EmployeeResume, Guid> resumerepository
            , IRepository<OrganizationUnit, long> organizarepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , IAbpFileRelationAppService abpFileRelationAppService, IWorkFlowTaskRepository workFlowTaskRepository, IRepository<OrganizationUnitPosts, Guid> orgpostrepository) {
            _repository = repository;
            _resultrepository = resultrepository;
            _postrepository = postrepository;
            _organizarepository = organizarepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskRepository = workFlowTaskRepository;
            _orgpostrepository = orgpostrepository;
            _workFlowCacheManager = workFlowCacheManager;
            _projectAuditManager = projectAuditManager;
            _resumerepository = resumerepository;
        }
        /// <summary>
        /// 面试人员创建预约登记
        /// </summary>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreatePlanInput input)
        {
            EmployeePlan model = null;
            model = input.MapTo<EmployeePlan>();
            var has = _repository.GetAll().FirstOrDefault(ite=>ite.Phone==input.Phone.Trim() && ite.Status >= 0);
            if (has != null) {
                throw new Abp.UI.UserFriendlyException((int)ErrorCode.CodeValErr, "当前人员已存在面试计划，电话号码重复。");
            }
            model.ApplyNo = "ZP" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (model.ApplyPostId.HasValue == false) {
                throw new Abp.UI.UserFriendlyException((int)ErrorCode.CodeValErr, "应聘职位不能为空。");
            }
            await _repository.InsertAsync(model);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.人力资源面试者简历,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EmployeePlanDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            //var model =await _repository.GetAsync(id);
            var ret = (from a in _repository.GetAll()
                         //join b in _orgpostrepository.GetAll() on a.ApplyPostId equals b.Id
                         join c in _postrepository.GetAll() on a.ApplyPostId equals c.Id
                         where a.Id == id
                         select new EmployeePlanDto()
                         {
                             AdminUserId = a.AdminUserId,
                             AdminVerifyDiscuss = a.AdminVerifyDiscuss,
                             ApplyCount = a.ApplyCount,
                              ApplyPostId=a.ApplyPostId,
                             ApplyJob = c.Name,
                             ApplyNo = a.ApplyNo,
                             ApplyTime = a.ApplyTime,
                             ApplyUser = a.ApplyUser,
                             Comment = a.Comment,
                             CreationTime = a.CreationTime,
                             Discuss = a.Discuss,
                             EmployeePlanId = a.Id,
                              EmployeeUserIds=a.EmployeeUserIds,
                             Id = a.Id,
                             IsJoin = a.IsJoin,
                             JoinDes = a.JoinDes,
                             MergeUserId = a.MergeUserId,
                             NeedAdmin = a.NeedAdmin,
                             Phone = a.Phone,
                             RecordUserId = a.RecordUserId,
                             Result = a.Result,
                             VerifyDiscuss = a.VerifyDiscuss,
                             VerifyUserId = a.VerifyUserId,
                         }).FirstOrDefault();
            var logs = _resultrepository.GetAll().Where(ite => ite.EmployeePlanId == id).OrderByDescending(ite=>ite.ApplyCount).ToList();
            if (logs != null && logs.Count > 0) {
                ret.Log = logs.MapTo<List<EmployeePlanDto>>();
                foreach (var l in ret.Log) {

                    if (string.IsNullOrWhiteSpace(l.AdminUserId) == false)
                    {
                        l.AdminUserId_Name = _workFlowOrganizationUnitsManager.GetNames(l.AdminUserId);
                    }
                    if (string.IsNullOrWhiteSpace(l.EmployeeUserIds) == false)
                    {
                        l.EmployeeUserIds_Name = _workFlowOrganizationUnitsManager.GetNames(l.EmployeeUserIds);
                    }
                    if (string.IsNullOrWhiteSpace(l.MergeUserId) == false)
                    {
                        l.MergeUserId_Name = _workFlowOrganizationUnitsManager.GetNames(l.MergeUserId);
                    }
                    if (string.IsNullOrWhiteSpace(l.RecordUserId) == false)
                    {
                        l.RecordUserId_Name = _workFlowOrganizationUnitsManager.GetNames(l.RecordUserId);
                    }
                    if (string.IsNullOrWhiteSpace(l.VerifyUserId) == false)
                    {
                        l.VerifyUserId_Name = _workFlowOrganizationUnitsManager.GetNames(l.VerifyUserId);
                    }
                    l.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = l.EmployeePlanId.ToString(), BusinessType = (int)AbpFileBusinessType.人力资源面试者简历 });
                    l.ResultFileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = l.EmployeePlanId.ToString(), BusinessType = (int)AbpFileBusinessType.人力资源面试者结果 });
                }
                logs[0].AdminVerifyDiscuss = ret.AdminVerifyDiscuss;
            }
            
            if (string.IsNullOrWhiteSpace(ret.AdminUserId) == false){
                ret.AdminUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.AdminUserId);
            }
            if (string.IsNullOrWhiteSpace(ret.EmployeeUserIds) == false)
            {
                ret.EmployeeUserIds_Name = _workFlowOrganizationUnitsManager.GetNames(ret.EmployeeUserIds);
            }
            if (string.IsNullOrWhiteSpace(ret.MergeUserId) == false)
            {
                ret.MergeUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.MergeUserId);
            }
            if (string.IsNullOrWhiteSpace(ret.RecordUserId) == false)
            {
                ret.RecordUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.RecordUserId);
            }
            if (string.IsNullOrWhiteSpace(ret.VerifyUserId) == false)
            {
                ret.VerifyUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.VerifyUserId);
            }
            ret.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = ret.Id.ToString(), BusinessType = (int)AbpFileBusinessType.人力资源面试者简历 });
            ret.ResultFileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = ret.Id.ToString(), BusinessType = (int)AbpFileBusinessType.人力资源面试者结果 });
            return ret;
        }

        
        /// <summary>
        /// 获取预约面试列表
        /// </summary>
        /// <param name="input"></param>
        public async  Task<PagedResultDto<EmployeePlanListDto>> GetList(EmployeePlanSearchInput input)
        {
            var ret = (from a in _repository.GetAll()
                       join b in _postrepository.GetAll() on a.ApplyPostId equals b.Id
                       
                       let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                        select c)
                       select new EmployeePlanListDto()
                       {
                           ApplyJob = b.Name,
                           ApplyNo = a.ApplyNo,
                           ApplyUser = a.ApplyUser,
                           CreationTime = a.CreationTime,
                           Id = a.Id,
                           Phone = a.Phone,
                           ApplyPostId = a.ApplyPostId,
                           Status = a.Status,
                           OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                       });
            if (input.Status == 0)
            {
                ret = ret.Where(ite => ite.Status == 0);
            }
            else {
                ret = ret.Where(ite => ite.Status > 0);
            }
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false) {
                ret = ret.Where(ite=>ite.ApplyUser.Contains(input.SearchKey)||ite.ApplyJob.Contains(input.SearchKey)||ite.Phone.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model =(await ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToListAsync()).MapTo<List<EmployeePlanListDto>>();
            foreach (var item in model) {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);
                item.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = item.InstanceId, BusinessType = (int)AbpFileBusinessType.人力资源面试者简历 });
            }
            return new PagedResultDto<EmployeePlanListDto>(total, model);
        }

        public async Task Update(EmployeePlanEditInput input)
        {
            var has = _repository.GetAll().FirstOrDefault(ite => ite.Phone == input.Phone.Trim()&&ite.Id!=input.Id&&ite.Status>=0);
            if (has != null)
            {
                throw new Abp.UI.UserFriendlyException((int)ErrorCode.CodeValErr, "当前人员已存在面试计划，电话号码重复。");
            }
            var model = _repository.Get(input.Id);
            var old_Model = model.DeepClone();
            input.MapTo(model);
            await _repository.UpdateAsync(model);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.人力资源面试者简历,
                    Files = fileList
                });
            }
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                if (old_Model.MergeUserId != model.MergeUserId) {
                    //如果变更了汇总人则将原来汇总人的待办也换到新汇总人那里
                    var task = _workFlowTaskRepository.GetAll().FirstOrDefault(ite => ite.InstanceID == old_Model.Id.ToString()&&ite.Status<2);
                    if (task != null) {
                        task.ReceiveID = model.MergeUserId.Replace("u_","").ToLong();
                        task.ReceiveName =(await UserManager.GetUserByIdAsync(task.ReceiveID)).Name;
                        _workFlowTaskRepository.Update(task);
                    }
                }
                if (old_Model.RecordUserId != model.RecordUserId)
                {
                    //如果变更了记录人则将原来记录人的待办也换到新记录人那里
                    var task = _workFlowTaskRepository.GetAll().FirstOrDefault(ite => ite.InstanceID == old_Model.Id.ToString() && ite.Status < 2);
                    if (task != null)
                    {
                        task.ReceiveID = model.RecordUserId.Replace("u_", "").ToLong();
                        task.ReceiveName = (await UserManager.GetUserByIdAsync(task.ReceiveID)).Name;
                        _workFlowTaskRepository.Update(task);
                    }
                }
                
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(model));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }

        }
        private EmployeePlanEditInput GetChangeModel(EmployeePlan model)
        {
            // 如果有外键数据 在这里转换
            var ret = model.MapTo<EmployeePlanEditInput>();
            var doc = _repository.Get(ret.Id);
            if (string.IsNullOrWhiteSpace(ret.AdminUserId) == false)
            {
                ret.AdminUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.AdminUserId);
            }
            if (string.IsNullOrWhiteSpace(ret.EmployeeUserIds) == false)
            {
                ret.EmployeeUserIds_Name = _workFlowOrganizationUnitsManager.GetNames(ret.EmployeeUserIds);
            }
            if (string.IsNullOrWhiteSpace(ret.MergeUserId) == false)
            {
                ret.MergeUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.MergeUserId);
            }
            if (string.IsNullOrWhiteSpace(ret.RecordUserId) == false)
            {
                ret.RecordUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.RecordUserId);
            }
            if (string.IsNullOrWhiteSpace(ret.VerifyUserId) == false)
            {
                ret.VerifyUserId_Name = _workFlowOrganizationUnitsManager.GetNames(ret.VerifyUserId);
            }
            return ret;
        }
        /// <summary>
        /// 领导登记面试结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAdminVerify(EmployeePlanAdminResultInput input)
        {
            var ret = _repository.Get(input.Id);
            input.MapTo(ret);
            await _repository.UpdateAsync(ret);
        }
        /// <summary>
        /// 登记员、汇总员登记面试结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateComment(EmployeePlanCommentInput input)
        {
            var ret = _repository.Get(input.Id);
            input.MapTo(ret);
            await _repository.UpdateAsync(ret);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = ret.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.人力资源面试者结果,
                    Files = fileList
                });
            }
        }
        /// <summary>
        /// 人事确认员工是否入职
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateJoin(EmployeeJoinInput input)
        {
            var ret = _repository.Get(input.Id);
            ret.IsJoin = input.IsJoin;
            ret.JoinDes = input.JoinDes;
           await _repository.UpdateAsync(ret);
        }

        /// <summary>
        /// 人力资源安排面试时间和面试官
        /// </summary>
        /// <param name="input"></param>
        public async Task UpdatePlan(EmployeePlanInput input)
        {
            var ret = _repository.Get(input.Id);
            input.MapTo(ret);
            await _repository.UpdateAsync(ret);
        }
        /// <summary>
        /// 分管领导登记面试结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateVerify(EmployeePlanResultInput input)
        {
            var ret = _repository.Get(input.Id);
            input.MapTo(ret);
            await _repository.UpdateAsync(ret);
        }
        /// <summary>
        /// 流程管理员作废申请
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flowId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task StopPlan(Guid id, Guid flowId, string reason)
        {
            var record = _repository.Get(id);
            if (flowId == Guid.Empty)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "工作流id不能为空。");
            }
            record.Status = (int)WorkFlowBusinessStatus.已终止;
            var task = _workFlowTaskRepository.GetAll().Where(ite => ite.FlowID == flowId && ite.InstanceID == id.ToString()).ToList();
            foreach (var t in task)
            {
                t.Status = 10;//流程管理员作废申请
                t.Comment = reason;
                await _workFlowTaskRepository.UpdateAsync(t);
            }
            await _repository.UpdateAsync(record);

            //发送事务通知
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
            var noticeInput = new ZCYX.FRMSCore.Application.NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = record.Id;
            noticeInput.Content = $"人员【{record.ApplyUser}】面试已被终止。";
            noticeInput.Title = $"人员【{record.ApplyUser}】面试已被终止。";
            var us = new List<string>();
            if (string.IsNullOrWhiteSpace(record.AdminUserId) == false) {
                us.Add(record.AdminUserId.Replace("u_", ""));
            }
            if (string.IsNullOrWhiteSpace(record.EmployeeUserIds) == false)
            {
                var t = record.EmployeeUserIds.Split(",");
                if (t != null) {
                    foreach (var x in t) {
                        us.Add(x.Replace("u_", ""));
                    }
                }
                
            }
            if (string.IsNullOrWhiteSpace(record.MergeUserId) == false)
            {
                us.Add(record.MergeUserId.Replace("u_", ""));
            }
            if (string.IsNullOrWhiteSpace(record.RecordUserId) == false)
            {
                us.Add(record.RecordUserId.Replace("u_", ""));
            }
            if (string.IsNullOrWhiteSpace(record.VerifyUserId) == false)
            {
                us.Add(record.VerifyUserId.Replace("u_", ""));
            }
            us.Add(record.AdminUserId);
            noticeInput.NoticeUserIds = string.Join(",", us);
            noticeInput.NoticeType = 1;
            await noticeService.CreateOrUpdateNoticeAsync(noticeInput);
            //
        }
        /// <summary>
        /// 面试后更新人才库状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="statue"></param>
        public void ChangeResumeStatus(ChangeStatusInput input) {
            var plan = _repository.Get(input.Id);
            if (string.IsNullOrWhiteSpace(plan.Phone) == false) {
                var resume = _resumerepository.GetAll().FirstOrDefault(ite => ite.Phone == plan.Phone);
                if (resume != null) {
                    resume.Status = input.Status;
                    _resumerepository.Update(resume);
                }
            }
        }
    }
}
