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
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp.Domain.Uow;
using Project;

namespace GWGL
{
    public class NoticeDocumentAppService : FRMSCoreAppServiceBase, INoticeDocumentAppService
    {
        private readonly IRepository<NoticeDocument, Guid> _noticeDocumentRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<ProjectRegistration, Guid> _projectRegistrationRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<GW_DocumentType, Guid> _gW_DocumentTypeRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrgRepository;

        public NoticeDocumentAppService(IRepository<NoticeDocument, Guid> noticeDocumentRepository, IRepository<WorkFlowOrganizationUnits, long> organizeRepository,
            IWorkFlowTaskRepository roadFlowWorkFlowTaskRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IRepository<ProjectRegistration, Guid> projectRegistrationRepository
            , ProjectAuditManager projectAuditManager
            , WorkFlowCacheManager workFlowCacheManager, IRepository<GW_DocumentType, Guid> gW_DocumentTypeRepository
            , IRepository<WorkFlowUserOrganizationUnits, long> userOrgRepository, WorkFlowTaskManager workFlowTaskManager)
        {
            this._noticeDocumentRepository = noticeDocumentRepository;
            _organizeRepository = organizeRepository;
            this._workFlowTaskRepository = roadFlowWorkFlowTaskRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _projectRegistrationRepository = projectRegistrationRepository;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _gW_DocumentTypeRepository = gW_DocumentTypeRepository;
            _userOrgRepository = userOrgRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }

        //public async Task CreateOrUpdate(CreateNoticeDocumentInput input)
        //{
        //    if (input.Id.HasValue)
        //    {
        //        await Update(input);
        //    }
        //    else
        //    {
        //        // await Create(input);
        //    }
        //}

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        [AbpAuthorize]
        public async Task<GetNoticeDocumentForEditOutput> GetEdit(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);

            var model = await _noticeDocumentRepository.GetAsync(id);
            var ret = new GetNoticeDocumentForEditOutput();
            ret.Id = model.Id;
            //ret.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(model.FileInfo);
            ret.NoticeType = model.NoticeType;
            ret.DispatchUnit = model.DispatchUnit;
            ret.DispatchTime = model.DispatchTime;
            ret.PrintNum = model.PrintNum;
            ret.DispatchCode = model.DispatchCode;
            ret.Urgency = model.Urgency;
            ret.Urgency_Name = model.Urgency?.ToString() ?? "";
            ret.SecretLevel = model.SecretLevel;
            ret.SecretLevel_Name = model.SecretLevel?.ToString() ?? "";
            ret.ReceiveId = model.ReceiveId;
            ret.ReceiveName = model.ReceiveName;
            ret.Reason = model.Reason;
            ret.Content = model.Content;
            ret.IsNeedRes = model.IsNeedRes;
            ret.ProjectId = model.ProjectId;
            ret.Title = model.Title;
            ret.PubilishUserName = model.PubilishUserName;
            ret.MainReceiveName = model.MainReceiveName;
            ret.DocumentTyep = model.DocumentTyep;
            ret.DocumentTyepName = model.DocumentTyep == null ? "" : ((ReceiptDocProperty)model.DocumentTyep).ToString();
            ret.DispatchUnitName = model.DispatchUnitName;
            ret.ReplyContent = model.ReplyContent;
            ret.IsNeedAddWrite = model.IsNeedAddWrite;
            ret.AddType = model.AddType;
            ret.WriteType = model.WriteType;
            ret.AddWriteUsers = model.AddWriteUsers;
            ret.AddWriteOrgIds = model.AddWriteOrgIds;

            if (!model.AddWriteOrgIds.IsNullOrWhiteSpace())
            {
                var orgIdArrys = model.AddWriteOrgIds.Split(",");
                var query = from a in _organizeRepository.GetAll()
                            where orgIdArrys.Contains(a.Id.ToString())
                            select new { a.Id, a.DisplayName };
                var retData = query.ToList();
                var nameList = new List<string>();
                foreach (var item in orgIdArrys)
                {
                    nameList.Add(retData.FirstOrDefault(r => r.Id.ToString() == item).DisplayName);
                }
                ret.AddWriteOrgIdName = string.Join(",", nameList);
            }

            ret.GW_DocumentTypeId = model.GW_DocumentTypeId;
            ret.GW_DocumentTypeName = model.GW_DocumentTypeName;

            ret.CheckUser = model.CheckUser;
            if (model.CheckUser.HasValue)
                ret.CheckUser_Name = (await UserManager.GetUserByIdAsync(model.CheckUser.Value)).Name;

            ret.NoticeDocumentBusinessType = (NoticeDocumentBusinessType)model.NoticeDocumentBusinessType;
            if (model.NoticeDocumentBusinessType == (int)NoticeDocumentBusinessType.项目评审发文)
            {
                ret.DispatchMessage = new DispatchPublishOutput()
                {
                    Additional = model.Additional,
                    AppraisalTypeId = 0,
                    AuditAmount = model.AuditAmount,
                    DispatchCode = model.DispatchCode,
                    EndDate = model.EndDate,
                    ProjectId = model.ProjectId.Value,
                    ProjectLeader = model.ProjectLeader,
                    ProjectReviewer = model.ProjectReviewer,
                    ProjectUndertakeCode = model.ProjectUndertakeCode,
                    Reason = model.Reason,
                    SendUnitName = model.SendUnitName,
                    StartDate = model.StartDate,
                };
            }


            return ret;


        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        [AbpAuthorize]
        public async Task<GetNoticeDocumentForEditOutput> GetByRegistrationIdEdit(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _noticeDocumentRepository.GetAll().FirstOrDefaultAsync(x => x.ProjectRegistrationId == id);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            var ret = new GetNoticeDocumentForEditOutput();
            ret.Id = model.Id;
            //ret.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(model.FileInfo);
            ret.NoticeType = model.NoticeType;
            ret.DispatchUnit = model.DispatchUnit;
            ret.DispatchTime = model.DispatchTime;
            ret.PrintNum = model.PrintNum;
            ret.DispatchCode = model.DispatchCode;
            ret.Urgency = model.Urgency;
            ret.Urgency_Name = model.Urgency?.ToString() ?? "";
            ret.SecretLevel = model.SecretLevel;
            ret.SecretLevel_Name = model.SecretLevel?.ToString() ?? "";
            ret.ReceiveId = model.ReceiveId;
            ret.ReceiveName = model.ReceiveName;
            ret.Reason = model.Reason;
            ret.Content = model.Content;
            ret.IsNeedRes = model.IsNeedRes;
            ret.ProjectId = model.ProjectId;
            ret.Title = model.Title;
            ret.PubilishUserName = model.PubilishUserName;
            ret.MainReceiveName = model.MainReceiveName;
            ret.DocumentTyep = model.DocumentTyep;
            ret.DocumentTyepName = model.DocumentTyep == null ? "" : ((ReceiptDocProperty)model.DocumentTyep.Value).ToString();
            ret.DispatchUnitName = model.DispatchUnitName;
            ret.ReplyContent = model.ReplyContent;
            ret.IsNeedAddWrite = model.IsNeedAddWrite;
            ret.AddType = model.AddType;
            ret.WriteType = model.WriteType;
            ret.AddWriteUsers = model.AddWriteUsers;
            ret.AddWriteOrgIds = model.AddWriteOrgIds;
            if (!model.AddWriteOrgIds.IsNullOrWhiteSpace())
            {
                var orgIdArrys = model.AddWriteOrgIds.Split(",");
                var query = from a in _organizeRepository.GetAll()
                            where orgIdArrys.Contains(a.Id.ToString())
                            select a.DisplayName;
                ret.AddWriteOrgIdName = string.Join(",", query.ToList());
            }
            ret.GW_DocumentTypeId = model.GW_DocumentTypeId;
            ret.GW_DocumentTypeName = model.GW_DocumentTypeName;

            ret.CheckUser = model.CheckUser;
            if (model.CheckUser.HasValue)
                ret.CheckUser_Name = (await UserManager.GetUserByIdAsync(model.CheckUser.Value)).Name;

            ret.NoticeDocumentBusinessType = (NoticeDocumentBusinessType)model.NoticeDocumentBusinessType;
            if (model.NoticeDocumentBusinessType == (int)NoticeDocumentBusinessType.项目评审发文)
            {
                ret.DispatchMessage = new DispatchPublishOutput()
                {
                    Additional = model.Additional,
                    AppraisalTypeId = 0,
                    AuditAmount = model.AuditAmount,
                    DispatchCode = model.DispatchCode,
                    EndDate = model.EndDate,
                    ProjectId = model.ProjectId.Value,
                    ProjectLeader = model.ProjectLeader,
                    ProjectReviewer = model.ProjectReviewer,
                    ProjectUndertakeCode = model.ProjectUndertakeCode,
                    Reason = model.Reason,
                    SendUnitName = model.SendUnitName,
                    StartDate = model.StartDate,
                };
            }
            return ret;


        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        [AbpAuthorize]
        public async Task<InitWorkFlowOutput> Create(CreateNoticeDocumentInput input)
        {
            var ret = new InitWorkFlowOutput();

            var model = new NoticeDocument();
            model.Id = Guid.NewGuid();
            //model.FileInfo = Newtonsoft.Json.JsonConvert.SerializeObject(input.FileInfo);
            model.NoticeType = input.NoticeType;
            //var userorgModel = await _userOrgRepository.FirstOrDefaultAsync(r => r.UserId == AbpSession.UserId.Value && r.IsMain == true);
            //var orgModel = await _organizeRepository.GetAsync(userorgModel.OrganizationUnitId);
            model.DispatchUnitName =  input.DispatchUnitName;
            //model.DispatchUnit = orgModel.Id;
            model.DispatchTime = DateTime.Now;
            model.PrintNum = input.PrintNum;
            model.DispatchCode = input.DispatchCode;
            model.Urgency = input.Urgency;
            model.SecretLevel = input.SecretLevel;
            model.ReceiveId = input.ReceiveId;
            model.ReceiveName = input.ReceiveName;
            model.Reason = input.Reason;
            model.Content = input.Content;
            model.IsNeedRes = input.IsNeedRes;
            model.ProjectId = input.ProjectId;
            model.Title = input.Title;
            model.Status = 0;
            model.ProjectRegistrationId = input.ProjectRegistrationId;
            model.PubilishUserName = input.PubilishUserName;
            model.MainReceiveName = input.MainReceiveName;
            if (input.DocumentTyep.HasValue)
                model.DocumentTyep = (int)input.DocumentTyep.Value;

            model.NoticeDocumentBusinessType = (int)input.NoticeDocumentBusinessType;
            model.IsNeedAddWrite = input.IsNeedAddWrite;
            model.GW_DocumentTypeId = input.GW_DocumentTypeId;
            var gw_Model = await _gW_DocumentTypeRepository.FirstOrDefaultAsync(input.GW_DocumentTypeId);
            if (gw_Model != null)
                model.GW_DocumentTypeName = gw_Model.Name;

            model.AddType = input.AddType;
            model.WriteType = input.WriteType;
            model.AddWriteUsers = GetWriteUser(input);
            model.AddWriteOrgIds = input.AddWriteOrgIds;
            if (input.NoticeDocumentBusinessType == NoticeDocumentBusinessType.项目评审发文)
            {

                model.Additional = input.Additional;
                model.AuditAmount = input.AuditAmount;
                model.EndDate = input.EndDate;
                model.ProjectId = input.ProjectId;
                model.ProjectLeader = input.ProjectLeader;
                model.ProjectReviewer = input.ProjectReviewer;
                model.ProjectUndertakeCode = input.ProjectUndertakeCode;
                model.Reason = input.Reason;
                model.SendUnitName = input.SendUnitName;
                model.StartDate = input.StartDate;
            }


            await _noticeDocumentRepository.InsertAsync(model);
            await CurrentUnitOfWork.SaveChangesAsync();
            ret.InStanceId = model.Id.ToString();
            return ret;
        }



        public async Task Update(CreateNoticeDocumentInput input)
        {

            var model = await _noticeDocumentRepository.GetAsync(input.Id.Value);
            var old_Model = model.DeepClone();
            //model.FileInfo = Newtonsoft.Json.JsonConvert.SerializeObject(input.FileInfo);
            model.NoticeType = input.NoticeType;
            model.DispatchUnit = input.DispatchUnit;
            model.DispatchTime = DateTime.Now;
            model.PrintNum = input.PrintNum;
            model.DispatchCode = input.DispatchCode;
            model.Urgency = input.Urgency;
            model.SecretLevel = input.SecretLevel;
            model.ReceiveId = input.ReceiveId;
            model.ReceiveName = input.ReceiveName;
            model.Reason = input.Reason;
            model.Content = input.Content;
            model.IsNeedRes = input.IsNeedRes;
            model.ProjectId = input.ProjectId;
            model.Title = input.Title;
            model.PubilishUserName = input.PubilishUserName;
            model.MainReceiveName = input.MainReceiveName;
            if (input.DocumentTyep.HasValue)
                model.DocumentTyep = (int)input.DocumentTyep.Value;
            model.DispatchUnitName = input.DispatchUnitName;
            model.ReplyContent = input.ReplyContent;
            model.IsNeedAddWrite = input.IsNeedAddWrite;
            model.AddType = input.AddType;
            model.WriteType = input.WriteType;
            model.AddWriteUsers = GetWriteUser(input);
            model.AddWriteOrgIds = input.AddWriteOrgIds;
            model.GW_DocumentTypeId = input.GW_DocumentTypeId;
            var gw_Model = await _gW_DocumentTypeRepository.GetAsync(input.GW_DocumentTypeId);
            model.GW_DocumentTypeName = gw_Model.Name;

            //model.NoticeDocumentBusinessType = (int)input.NoticeDocumentBusinessType;

            if (model.NoticeDocumentBusinessType == (int)NoticeDocumentBusinessType.项目评审发文)
            {
                model.Additional = input.Additional;
                model.AuditAmount = input.AuditAmount;
                model.EndDate = input.EndDate;
                model.ProjectId = input.ProjectId;
                model.ProjectLeader = input.ProjectLeader;
                model.ProjectReviewer = input.ProjectReviewer;
                model.ProjectUndertakeCode = input.ProjectUndertakeCode;
                model.Reason = input.Reason;
                model.SendUnitName = input.SendUnitName;
                model.StartDate = input.StartDate;
            }
            await _noticeDocumentRepository.UpdateAsync(model);
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(model));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
                _workFlowTaskManager.CreateNoticeForTask(input.FlowId,input.Id.ToString(), "发文变更", model.Title);
            }
        }






        [AbpAuthorize]
        public async Task<PagedResultDto<NoticeDocumentListOutput>> GetNoticeDocuments(GetNoticeDocumentListInput input)
        {
            var query = from n in _noticeDocumentRepository.GetAll()
                        join w in _workFlowTaskRepository.GetAll() on new { InstanceId = n.Id.ToString(), UserId = AbpSession.UserId.Value } equals new
                        {
                            InstanceId = w.InstanceID,
                            UserId = w.ReceiveID
                        } into g
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                    x.FlowID == input.FlowId && x.InstanceID == n.Id.ToString() &&
                                    x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        where (n.CreatorUserId == AbpSession.UserId.Value || n.DealWithUsers.GetStrContainsArray(AbpSession.UserId.Value.ToString()))
                        join b in _gW_DocumentTypeRepository.GetAll() on n.GW_DocumentTypeId equals b.Id into m
                        from b in m.DefaultIfEmpty()
                        select new
                        {
                            NoticeDocumentInfo = n,
                            GW_DocTypeId = b == null ? Guid.Empty : b.Id,
                            GW_DocTypeName = b.Name,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2,
                            NeedDoCount = g.Count(q => q.Status < 2),
                        };




            query = query.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), r => r.NoticeDocumentInfo.Title.Contains(input.SearchKey) || r.NoticeDocumentInfo.DispatchCode.Contains(input.SearchKey)
            || r.NoticeDocumentInfo.GW_DocumentTypeName.Contains(input.SearchKey))
                         .WhereIf(input.IsNeedRes.HasValue, r => r.NoticeDocumentInfo.IsNeedRes == input.IsNeedRes.Value);


            if (!string.IsNullOrWhiteSpace(input.GW_DocumentTypeIds))
            {
                var gw_Ids = input.GW_DocumentTypeIds.Split(",");
                query = query.Where(r => gw_Ids.Contains(r.GW_DocTypeId.ToString()));
            }

            var datas = new List<NoticeDocumentListOutput>();
            var count = await query.CountAsync();
            var nds = await query.OrderBy(x => x.OpenModel).ThenByDescending(r => r.NoticeDocumentInfo.CreationTime)
            .PageBy(input)
            .ToListAsync();

            foreach (var document in nds)
            {
                var entity = new NoticeDocumentListOutput()
                {
                    Id = document.NoticeDocumentInfo.Id,
                    Title = document.NoticeDocumentInfo.Title,
                    IsNeedRes = document.NoticeDocumentInfo.IsNeedRes,
                    Status = document.NoticeDocumentInfo.Status,
                    DispatchUnitName = document.NoticeDocumentInfo.DispatchUnitName,
                    DispatchCode = document.NoticeDocumentInfo.DispatchCode,
                    DocumentTyepName = document.NoticeDocumentInfo.DocumentTyep.HasValue ? ((ReceiptDocProperty)document.NoticeDocumentInfo.DocumentTyep.Value).ToString() : "",
                    PubilishUserName = document.NoticeDocumentInfo.PubilishUserName,
                    CreationTime = document.NoticeDocumentInfo.CreationTime,
                    NoticeDocumentBusinessType = (NoticeDocumentBusinessType)document.NoticeDocumentInfo.NoticeDocumentBusinessType,
                    OpenModel = document.OpenModel,
                    GW_DocumentTypeId = document.GW_DocTypeId,
                    GW_DocumentTypeName = document.GW_DocTypeName,
                    SecretLevel = document.NoticeDocumentInfo.SecretLevel,
                    SecretLevel_Name = document.NoticeDocumentInfo.SecretLevel?.ToString() ?? "",
                    Urgency = document.NoticeDocumentInfo.Urgency,
                    Urgency_Name = document.NoticeDocumentInfo.Urgency?.ToString() ?? "",

                };

                entity.InstanceId = document.NoticeDocumentInfo.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, entity as BusinessWorkFlowListOutput);
                //if (document.WorkFlowInfo != null)
                //{
                //    entity.Query = $"flowid={document.WorkFlowInfo.FlowID}&stepid={document.WorkFlowInfo.StepID}&instanceid={document.WorkFlowInfo.InstanceID}&taskid={document.WorkFlowInfo.Id}&groupid={document.WorkFlowInfo.GroupID}&appid=";
                //    entity.StepName = document.WorkFlowInfo.StepName;

                //}
                datas.Add(entity);
            }
            return new PagedResultDto<NoticeDocumentListOutput>(count, datas);
        }


        /// <summary>
        /// 获取是否需要会签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsNeedAddWrite(Guid id)
        {
            var model = _noticeDocumentRepository.Get(id);
            return model.IsNeedAddWrite;
        }



        public async Task UpdateSupplierPrint(UpdateSupplierPrintInput input)
        {
            var model = await _noticeDocumentRepository.GetAsync(input.Id);
            model.DeliveryUser = input.DeliveryUser;
            model.PrintNum = model.PrintNum+input.PrintNum;
            model.SupplySupplierId = input.SupplySupplierId;
            model.SupplySupplierRemark = input.SupplySupplierRemark;
        }

        [AbpAuthorize]
        public void MarkCheckUser(string instanceId)
        {
            var id = instanceId.ToGuid();
            var model = _noticeDocumentRepository.Get(id);
            model.CheckUser = AbpSession.UserId.Value;
        }




        private NoticeDocumentChangeDto GetChangeModel(NoticeDocument model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<NoticeDocumentChangeDto>();
            ret.DocumentTyepName = model.DocumentTyep.HasValue ? ((ReceiptDocProperty)model.DocumentTyep.Value).ToString() : "";
            if (!model.AddWriteOrgIds.IsNullOrWhiteSpace())
            {
                var orgIdArrys = model.AddWriteOrgIds.Split(",");
                var query = from a in _organizeRepository.GetAll()
                            where orgIdArrys.Contains(a.Id.ToString())
                            select a.DisplayName;
                ret.AddWriteOrgIdName = string.Join(",", query.ToList());
            }
            return ret;
        }



        private string GetWriteUser(CreateNoticeDocumentInput input)
        {
            if (!input.IsNeedAddWrite)
                return "";
            if (input.IsNeedAddWrite && input.AddWriteOrgIds.IsNullOrWhiteSpace())
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "为设置会签部门");
            var addOrgArry = input.AddWriteOrgIds.Split(",");
            var userList = new List<string>();
            var orgs = from o in _organizeRepository.GetAll()
                       where addOrgArry.Contains(o.Id.ToString())
                       select o;
            foreach (var item in orgs)
            {
                userList.Add(item.Leader);
                userList.Add(item.ChargeLeader);
            }

            for (int i = 0; i < (userList.Count / 2) - 1; i++)
            {
                if (userList[i * 2 + 1] == userList[(i + 1) * 2 + 1])
                    userList[i * 2 + 1] = "";

            }
            var newUserList = new List<string>();
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i] != "")
                    newUserList.Add(userList[i]);
            }

            var userStr = string.Join(",", newUserList);
            userStr = "," + userStr;
            foreach (var item in newUserList)
            {
                do
                {
                    userStr = userStr.Replace1($",{item},{item}", $",{item}");
                } while (userStr.IndexOf($",{item},{item}") > 0);
            }
            userStr = userStr.Remove(0, 1);
            return userStr;

        }











    }
}
