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
using Abp.Runtime.Caching;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Application.Services;
using Abp.Json;
using Abp.Threading;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow.Entity;
using Microsoft.AspNetCore.Hosting;
using Abp.File;
using Abp.Domain.Uow;
using ZCYX.FRMSCore.Model;
using Abp.Authorization.Users;
using ZCYX.FRMSCore.Authorization.Roles;

namespace Abp.WorkFlow
{
    public class WorkFlowAppService : ApplicationService, IWorkFlowAppService
    {
        private readonly IRepository<User, long> _useRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<WorkFlow, Guid> _workFlowRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<WorkFlowVersionNum, Guid> _workFlowVersionNumRepository;
        private readonly IRepository<WorkFlowModel, Guid> _workFlowModel;
        private readonly IRepository<WorkFlowModelColumn, Guid> _workFlowModelColumn;
        private readonly IRepository<WorkFlowTemplate, Guid> _workFlowTemplate;
        private readonly IRepository<AbpFile, Guid> _abpFilerepository;
        private readonly IRepository<RoleRelation, Guid> _roleRelationRepository;
        private IHostingEnvironment hostingEnv;

        private readonly IRepository<UserRole, long> _userRoleRepository;

        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public WorkFlowAppService(IRepository<User, long> useRepository, IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IRepository<WorkFlow, Guid> workFlowRepository
            , WorkFlowCacheManager workFlowCacheManager, WorkFlowTaskManager workFlowTaskManager, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , IWorkFlowTaskRepository workFlowTaskRepository, IRepository<WorkFlowVersionNum, Guid> workFlowVersionNumRepository, IRepository<WorkFlowModel, Guid> workFlowModel, IRepository<WorkFlowModelColumn, Guid> workFlowModelColumn
            , IRepository<WorkFlowTemplate, Guid> workFlowTemplate, IHostingEnvironment env, IRepository<AbpFile, Guid> abpFilerepository, IUnitOfWorkManager unitOfWorkManager, IRepository<RoleRelation, Guid> roleRelationRepository, IRepository<UserRole, long> userRoleRepository)
        {
            this._useRepository = useRepository;
            _organizeRepository = organizeRepository;
            _workFlowRepository = workFlowRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowVersionNumRepository = workFlowVersionNumRepository;
            _workFlowModel = workFlowModel;
            _workFlowModelColumn = workFlowModelColumn;
            _workFlowTemplate = workFlowTemplate;
            _abpFilerepository = abpFilerepository;
            this.hostingEnv = env;
            _unitOfWorkManager = unitOfWorkManager;
            _roleRelationRepository = roleRelationRepository;
            _userRoleRepository =userRoleRepository;
        }

        public async Task<Guid> CreateWorkFlow(CreateWorkFlowInput input)
        {
            var model = new WorkFlow();
            model.Id = Guid.NewGuid();
            input.ID = model.Id;
            model.InstallDate = DateTime.Now;
            model.InstallUserID = AbpSession.UserId.Value;
            model.Status = 1;
            model.Name = input.Name;
            model.Type = input.Type.ToGuid();
            model.IsChange = input.IsChange;
            model.IsFiles = input.IsFiles;
            model.Manager = AbpSession.UserId.Value;
            model.InstanceManager = AbpSession.UserId.Value;
            model.VersionNum = 1;
            if (input.Steps != null && input.Steps.Count() > 0)
            {
                var repeat = input.Steps.GroupBy(ite => ite.StepToStatus).Where(g => g.Count() > 1).ToList();
                if (repeat != null && repeat.Count > 0)
                {
                    var stepname = new List<string>();
                    foreach (var r in repeat)
                    {
                        stepname.AddRange(r.Select(ite => ite.Name));
                    }
                    var msg = string.Join("、", stepname);
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"流程数据错误，步骤{msg}的状态值存在重复，请修正。");
                }
            }
            foreach (var step in input.Steps)
            {
                if (!step.Behavior.DefaultHandler.IsNullOrWhiteSpace())
                {
                    step.Behavior.DefaultHandlerName = _workFlowOrganizationUnitsManager.GetNames(step.Behavior.DefaultHandler);
                }

                if (!step.Behavior.SelectRange.IsNullOrWhiteSpace())
                {
                    step.Behavior.SelectRangeName = _workFlowOrganizationUnitsManager.GetNames(step.Behavior.SelectRange);
                }

                if (!step.Behavior.SelelctOrgIds.IsNullOrWhiteSpace())
                {
                    step.Behavior.SelelctOrgIdNames = _workFlowOrganizationUnitsManager.GetNames(step.Behavior.SelelctOrgIds);
                }

                if (!step.Behavior.SelectPostIds.IsNullOrWhiteSpace())
                {
                    var selectPostIdarry = step.Behavior.SelectPostIds.Split(",");
                    var postInfoService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();
                    var postNames = new List<string>();
                    foreach (var item in selectPostIdarry)
                    {
                        var entity_name = postInfoService.GetNameWithOrgByOrgPostId(item.ToGuid());
                        postNames.Add(entity_name);
                    }
                    step.Behavior.SelectPostIdNamse = string.Join(",", postNames);
                }


                if (!step.Behavior.CopyForSelelctOrgIds.IsNullOrWhiteSpace())
                {
                    step.Behavior.CopyForSelelctOrgIdNames = _workFlowOrganizationUnitsManager.GetNames(step.Behavior.CopyForSelelctOrgIds);
                }

                if (!step.Behavior.CopyForSelectPostIds.IsNullOrWhiteSpace())
                {
                    var selectPostIdarry = step.Behavior.CopyForSelectPostIds.Split(",");
                    var postInfoService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();
                    var postNames = new List<string>();
                    foreach (var item in selectPostIdarry)
                    {
                        var entity_name = postInfoService.GetNameWithOrgByOrgPostId(item.ToGuid());
                        postNames.Add(entity_name);
                    }
                    step.Behavior.CopyForSelectPostIdNamse = string.Join(",", postNames);
                }

                if (!step.Behavior.CopyForSendSelelctOrgIds.IsNullOrWhiteSpace())
                {
                    step.Behavior.CopyForSendSelelctOrgIdNames = _workFlowOrganizationUnitsManager.GetNames(step.Behavior.CopyForSendSelelctOrgIds);
                }

                if (!step.Behavior.CopyForSendSelectPostIds.IsNullOrWhiteSpace())
                {
                    var selectPostIdarry = step.Behavior.CopyForSendSelectPostIds.Split(",");
                    var postInfoService = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IPostInfoAppService>();
                    var postNames = new List<string>();
                    foreach (var item in selectPostIdarry)
                    {
                        var entity_name = postInfoService.GetNameWithOrgByOrgPostId(item.ToGuid());
                        postNames.Add(entity_name);
                    }
                    step.Behavior.CopyForSendSelectPostIdNamse = string.Join(",", postNames);
                }

                if (!step.Behavior.CopyFor.IsNullOrWhiteSpace())
                {
                    step.Behavior.CopyForName = _workFlowOrganizationUnitsManager.GetNames(step.Behavior.CopyFor);
                }
                if (!step.Behavior.copyForSend.IsNullOrWhiteSpace())
                {
                    step.Behavior.copyForSendName = _workFlowOrganizationUnitsManager.GetNames(step.Behavior.copyForSend);
                }

            }

            model.RunJSON = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            model.DesignJSON = model.RunJSON;
            await _workFlowRepository.InsertAsync(model);
            var wfCacheModel = (input as WorkFlowInstalledBase).MapTo<WorkFlowInstalled>();
            wfCacheModel.VersionNum = model.VersionNum;
            _workFlowCacheManager.SetWorkFlowModelCache(model.Id.ToString(), wfCacheModel);
            _workFlowCacheManager.SetWorkFlowModelCache($"{model.Id}-{model.VersionNum}", wfCacheModel);
            var entity = new WorkFlowVersionNum() { Id = Guid.NewGuid(), FlowId = model.Id, RunJSON = model.RunJSON, VersionNum = model.VersionNum };
            await _workFlowVersionNumRepository.InsertAsync(entity);

            return model.Id;
        }


        public async Task UpdateWorkFlow(CreateWorkFlowInput input)
        {
            var model = await _workFlowRepository.GetAsync(input.ID);
            var old_runJsonHash = model.RunJSON.GetHashCode();
            model.Name = input.Name;
            model.Type = input.Type.ToGuid();
            model.IsChange = input.IsChange;
            model.IsFiles = input.IsFiles;
            if (input.Steps != null && input.Steps.Count() > 0) {
                var repeat = input.Steps.GroupBy(ite => ite.StepToStatus).Where(g => g.Count() > 1).ToList();
                if (repeat != null && repeat.Count > 0) {
                    var stepname = new List<string>();
                    foreach (var r in repeat) {
                        stepname.AddRange(r.Select(ite => ite.Name));
                    }
                    var msg = string.Join("、", stepname);
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"流程数据错误，步骤{msg}的状态值存在重复，请修正。");
                }
            }
            model.RunJSON = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            var new_runJsonHash = model.RunJSON.GetHashCode();
            model.DesignJSON = Newtonsoft.Json.JsonConvert.SerializeObject(input);

            if (old_runJsonHash != new_runJsonHash)
            {
                model.VersionNum = (model.VersionNum + 1);
                var entity = new WorkFlowVersionNum() { Id = Guid.NewGuid(), FlowId = model.Id, RunJSON = model.RunJSON, VersionNum = model.VersionNum };
                await _workFlowVersionNumRepository.InsertAsync(entity);
                var wfCacheModel = (input as WorkFlowInstalledBase).MapTo<WorkFlowInstalled>();
                wfCacheModel.VersionNum = model.VersionNum;
                _workFlowCacheManager.SetWorkFlowModelCache(model.Id.ToString(), wfCacheModel);
                _workFlowCacheManager.SetWorkFlowModelCache($"{model.Id}-{model.VersionNum}", wfCacheModel);
            }
            await _workFlowRepository.UpdateAsync(model);

        }
        /// <summary>
        /// 将流程步骤全部允许变更
        /// </summary>
        /// <returns></returns>
        public async Task UpdateWorkFlows()
        {
            var models = await (from w in _workFlowRepository.GetAll() join wv in _workFlowVersionNumRepository.GetAll() on w.Id equals wv.FlowId where w.IsChange && w.RunJSON != null select w).ToListAsync();
            foreach (var model in models)
            {
                try
                {
                    var s = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateWorkFlowInput>(model.RunJSON);
                    foreach (var item in s.Steps)
                    {
                        item.IsChange = true;
                    }
                    model.RunJSON = Newtonsoft.Json.JsonConvert.SerializeObject(s);
                    model.DesignJSON = Newtonsoft.Json.JsonConvert.SerializeObject(s);
                    await _workFlowRepository.UpdateAsync(model);
                }
                catch (Exception ex)
                {
                }
            }
        }

        [AbpAuthorize]
        public async Task DeleteWorkFlow(EntityDto<Guid> input)
        {
            var model = await _workFlowRepository.GetAsync(input.Id);
            await _workFlowRepository.DeleteAsync(model);
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<GetWorkFlowListDto>> GetList(GetWorkFlowListInput input)
        {
            var query = from w in _workFlowRepository.GetAll()
                        orderby w.CreationTime descending
                        select new { WorkFlow = w};

            query = query.Where(r => r.WorkFlow.Manager == AbpSession.UserId.Value).WhereIf(input.TypeId.HasValue, r => r.WorkFlow.Type == input.TypeId.Value);
            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.WorkFlow.Name.Contains(input.SearchKey));
            }
            if (!input.Status.IsNullOrWhiteSpace())
            {
                query = query.Where(r => input.Status.Contains(r.WorkFlow.Status.ToString()));
            }

            var totalCount = await query.CountAsync();
            var datas = await query.PageBy(input).ToListAsync();
            var list = new List<GetWorkFlowListDto>();
            foreach (var item in datas)
            {
                var entity = new GetWorkFlowListDto();
                entity.Id = item.WorkFlow.Id;
                entity.InstallDate = item.WorkFlow.InstallDate;
                entity.LastModificationTime = item.WorkFlow.LastModificationTime;
                entity.Name = item.WorkFlow.Name;
                entity.Status = item.WorkFlow.Status;
                entity.VersionNumber = item.WorkFlow.VersionNum;
                list.Add(entity);


            }
            return new PagedResultDto<GetWorkFlowListDto>(totalCount, list);
        }


        public async Task<EidtWorkFlowOutput> GetForEdit(EntityDto<Guid> input)
        {
            var model = _workFlowCacheManager.GetWorkFlowModelFromCache(input.Id);
            var ret = model.MapTo<EidtWorkFlowOutput>();
            return ret;
        }


        public async Task<EidtWorkFlowOutput> GetForEditByVersionNum(GetForEditByVersionNumInput input)
        {
            var model = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, input.VersionNum);
            var ret = model.MapTo<EidtWorkFlowOutput>();
            return ret;
        }


        public async Task<GetWorkFlowUrlParameterOutput> GetWorkFlowUrlParameterAsync(GetWorkFlowUrlParameterInput input)
        {
            var ret = new GetWorkFlowUrlParameterOutput();
            var currentStep = new WorkFlowStep();
            ret.IsDelete = false;
            if (input.TaskId.HasValue)
            {
                var currentTaskModel = _workFlowTaskRepository.Get(input.TaskId.Value);
                ret.TaskType = currentTaskModel.Type;
                ret.Status = currentTaskModel.Status;
                var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(currentTaskModel.FlowID, currentTaskModel.VersionNum);
                ret.FlowerType = workflowModel.Type;
                ret.IsFiles = workflowModel.IsFiles;

                currentStep = workflowModel.Steps.FirstOrDefault(r => r.ID == currentTaskModel.StepID);
                if (currentStep == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程数据错误");
                if (currentTaskModel.StepID == workflowModel.FirstStepID)
                {
                    ret.ChangeButtonShow = false;
                    ret.ChangeLogShow = false;
                    ret.IsFirstStepID = true;
                    ret.IsDelete = true;
                    if (_workFlowTaskRepository.GetAll().Any(x => x.SubFlowGroupID.Contains(currentTaskModel.GroupID.ToString())))
                        ret.IsDelete = false;
                }

                else
                {
                    if (workflowModel.IsChange)
                    {
                        ret.ChangeLogShow = true;
                        ret.ChangeButtonShow = currentStep.IsChange;
                    }
                    else
                    {
                        ret.ChangeLogShow = false;
                        ret.ChangeButtonShow = false;
                    }
                }
            }
            else
            {
                if (input.StepId.HasValue)
                {
                    var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    var currentStepModel = _workFlowTaskManager.GetStepWithStepId(input.FlowId, input.StepId.Value);
                    if (currentStepModel == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程数据错误");
                    currentStep = currentStepModel;
                    ret.IsFiles = workflowModel.IsFiles;
                    if (workflowModel.FirstStepID == currentStep.ID)
                    {
                        ret.ChangeButtonShow = false;
                        ret.ChangeLogShow = false;
                        ret.IsFirstStepID = true;
                        ret.IsDelete = true;
                    }
                    else
                    {
                        if (workflowModel.IsChange)
                        {
                            ret.ChangeLogShow = true;
                            ret.ChangeButtonShow = currentStep.IsChange;
                        }
                        else
                        {
                            ret.ChangeLogShow = false;
                            ret.ChangeButtonShow = false;
                        }
                    }
                }
                else
                {
                    var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (workflowModel.Steps.Count() == 0)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程数据错误");
                    currentStep = workflowModel.Steps.FirstOrDefault();
                    ret.IsFiles = workflowModel.IsFiles;
                    ret.FlowerType = workflowModel.Type;
                    ret.IsFirstStepID = true;
                    ret.ChangeLogShow = false;
                    ret.ChangeButtonShow = false;
                }
            }


            ret.FlowId = input.FlowId;
            ret.StepId = currentStep.ID;

            ret.StepName = currentStep.Name;
            if (currentStep.WorkFlowModelId.HasValue == false)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前步骤未设置视图，请修改流程设置。");
            }
            ret.ModelId = currentStep.WorkFlowModelId.Value;
            ret.OpinionDisplay = currentStep.OpinionDisplay;
            ret.SignatureType = currentStep.SignatureType;
            ret.SugguestionTitle = currentStep.SugguestionTitle;
            ret.TemplateType = currentStep.TemplateType;
            ret.Buttons = currentStep.Buttons?.ToList() ?? null;
            ret.FieldStatus = currentStep.FieldStatus?.ToList() ?? null;
            ret.ModelData = new Service.Dto.WorkFlowModelDto();
            return ret;
        }


        public async Task<GetNextStepForRunOutput> GetNextStepForRun(GetNextStepForRunInput input)
        {

            var currentTaskModel = await _workFlowTaskRepository.GetAsync(input.TaskId);
            var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(currentTaskModel.FlowID, currentTaskModel.VersionNum);
            var currentStep = new WorkFlowStep();
            var entity = workflowModel.Steps.FirstOrDefault(r => r.ID == currentTaskModel.StepID);
            currentStep = entity ?? throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程数据错误，未找到对应的步骤。");
            var ret = new GetNextStepForRunOutput();
            ret.FlowType = currentStep.Behavior.FlowType;
            ret.SignatureType = currentStep.SignatureType;
            ret.SugguestionTitle = currentStep.SugguestionTitle;
            var nextSteps = _workFlowTaskManager.GetNextSteps(currentTaskModel.FlowID, currentTaskModel.StepID, currentTaskModel.VersionNum);


            //判断流转条件
            if ((currentStep.Behavior.FlowType == 0 || currentStep.Behavior.FlowType == 3 )&& nextSteps.Count() > 0)
            {
                List<Guid> removeIDList = new List<Guid>();
                var eventParams = new WorkFlowCustomEventParams();
                eventParams.FlowID = currentTaskModel.FlowID;
                eventParams.GroupID = currentTaskModel.GroupID;
                eventParams.StepID = currentTaskModel.StepID;
                eventParams.TaskID = input.TaskId;
                eventParams.InstanceID = currentTaskModel.InstanceID;

                System.Text.StringBuilder nosubmitMsg = new System.Text.StringBuilder();
                foreach (var step in nextSteps)
                {
                    var lines = workflowModel.Lines.Where(p => p.ToID == step.ID && p.FromID == currentStep.ID);
                    if (lines.Count() > 0)
                    {
                        var line = lines.First();
                        if (!line.SqlWhere.IsNullOrEmpty())
                        {
                            if (workflowModel.DataBases.Count() == 0)
                            {
                                removeIDList.Add(step.ID);
                                //nosubmitMsg.Append("流程未设置数据连接");
                                //nosubmitMsg.Append("\\n");
                            }
                            else
                            {
                                if (!_workFlowTaskManager.TestLineSql(workflowModel.DataBases.First().LinkID, workflowModel.DataBases.First().Table,
                                     workflowModel.DataBases.First().PrimaryKey, currentTaskModel.InstanceID, line.SqlWhere))
                                {
                                    removeIDList.Add(step.ID);
                                    //nosubmitMsg.Append(string.Concat("提交条件未满足"));
                                    //nosubmitMsg.Append("\\n");
                                }
                            }
                        }
                        if (!line.CustomMethod.IsNullOrEmpty())
                        {
                            //object obj = _workFlowTaskManager.ExecuteFlowCustomEvent(line.CustomMethod.Trim(), eventParams);
                            ///策略---流转条件----自定义方法判断 返回bool 或者1
                            dynamic obj = _workFlowTaskManager.ExecuteFlowCustomEvent(line.CustomMethod.Trim(), eventParams);
                            var objType = obj.GetType();
                            var boolType = typeof(Boolean);
                            if (objType != boolType && "1" != obj.ToString())
                            {
                                removeIDList.Add(step.ID);
                                nosubmitMsg.Append(obj.ToString());
                                nosubmitMsg.Append("\\n");
                            }
                            else if (objType == boolType && !(bool)obj)
                            {
                                removeIDList.Add(step.ID);
                                nosubmitMsg.Append(obj.ToString());
                                nosubmitMsg.Append("\\n");
                            }
                        }
                        #region 组织机构关系判断  暂时屏蔽掉
                        //var SenderID = AbpSession.UserId.Value;
                        //long sponserID = 0;//发起者ID
                        //if (currentStep.ID == workflowModel.FirstStepID)//如果是第一步则发起者就是发送者
                        //{
                        //    sponserID = SenderID;
                        //}
                        //else
                        //{
                        //    sponserID = _workFlowTaskManager.GetFirstSnderID(eventParams.FlowID, eventParams.GroupID);
                        //}
                        //System.Text.StringBuilder orgWheres = new System.Text.StringBuilder();
                        //if (!line.Organize.IsNullOrEmpty())
                        //{
                        //    LitJson.JsonData orgJson = LitJson.JsonMapper.ToObject(line.Organize);
                        //    foreach (LitJson.JsonData json in orgJson)
                        //    {
                        //        if (orgJson.Count == 0)
                        //        {
                        //            continue;
                        //        }
                        //        string usertype = json["usertype"].ToString();
                        //        string in1 = json.ContainsKey("in1") ? json["in1"].ToString() : "";
                        //        string users = json["users"].ToString();
                        //        string selectorganize = json["selectorganize"].ToString();
                        //        string tjand = json["tjand"].ToString();
                        //        string khleft = json["khleft"].ToString();
                        //        string khright = json["khright"].ToString();
                        //        long userid = "0" == usertype ? SenderID : sponserID;
                        //        string memberid = "";
                        //        bool isin = false;
                        //        if ("0" == users)
                        //        {
                        //            memberid = selectorganize;
                        //        }
                        //        else if ("1" == users)
                        //        {
                        //            memberid = busers.GetLeader(userid);
                        //        }
                        //        else if ("2" == users)
                        //        {
                        //            memberid = busers.GetChargeLeader(userid);
                        //        }
                        //        if ("0" == in1)
                        //        {
                        //            isin = busers.IsContains(userid, memberid);
                        //        }
                        //        else if ("1" == in1)
                        //        {
                        //            isin = !busers.IsContains(userid, memberid);
                        //        }
                        //        if (!khleft.IsNullOrEmpty())
                        //        {
                        //            orgWheres.Append(khleft);
                        //        }
                        //        orgWheres.Append(isin ? " true " : " false ");
                        //        if (!khright.IsNullOrEmpty())
                        //        {
                        //            orgWheres.Append(khright);
                        //        }
                        //        orgWheres.Append(tjand);
                        //    }
                        //    string orgCode = string.Concat("bool testbool=", orgWheres.ToString(), ";return testbool;");
                        //    object rogCodeResult = RoadFlow.Utility.Tools.ExecuteCsharpCode(orgCode);
                        //    if (rogCodeResult != null && !(bool)rogCodeResult)
                        //    {
                        //        removeIDList.Add(step.ID);
                        //    }
                        //}
                        #endregion
                    }
                }
                foreach (Guid rid in removeIDList)
                {
                    nextSteps.RemoveAll(p => p.ID == rid);
                }
                if (nextSteps.Count == 0)
                {
                    string alertMsg = nosubmitMsg.ToString();
                    alertMsg = alertMsg.IsNullOrEmpty() ? "后续步骤条件均不符合,任务不能提交!" : alertMsg;
                }
            }
            var retSteps = new List<GetNextStepOutput>();
            var users = new List<RelationUser>();
            foreach (var step in nextSteps.OrderBy(p => p.Position_y).ThenBy(p => p.Position_x))
            {
                var entityStep = new GetNextStepOutput();
                var selectType = "";
                var selectRang = "";
                var defaultMember = _workFlowTaskManager.GetDefultMember(currentTaskModel.FlowID, step.ID, currentTaskModel.GroupID, currentStep.ID, currentTaskModel.InstanceID, out selectType, out selectRang,
                    input.TaskId, currentTaskModel.VersionNum);

                string[] array = defaultMember.Split(',');
                var list = array.Where(x => x.StartsWith(MemberPerfix.UserPREFIX)).Select(x => Convert.ToInt64(x.Replace(MemberPerfix.UserPREFIX, ""))).ToList();
                foreach (var item in list)
                {
                    var dt = DateTime.Now;
                    var model = _roleRelationRepository.GetAll().FirstOrDefault(x => x.UserId == item && x.StartTime < dt && x.EndTime > dt);
                    if (model != null && !list.Contains(model.RelationUserId))
                    {
                        defaultMember = defaultMember.Replace(MemberPerfix.UserPREFIX + model.UserId, MemberPerfix.UserPREFIX + model.RelationUserId);
                        users.Add(new RelationUser() { NextStepId = step.ID, UserId = model.UserId, RelationUserId = model.RelationUserId, RelationId = model.Id });
                    }
                }
                entityStep.DefaultUserId = defaultMember;
                entityStep.DefaultUserName = _workFlowOrganizationUnitsManager.GetNames(defaultMember);
                entityStep.NextStepId = step.ID;
                entityStep.NextStepName = step.Name;
                entityStep.IsAllowChoose = step.Behavior.RunSelect != 0;
                entityStep.SelectRangeRootId = step.Behavior.SelectRange?.Trim() ?? "";
                retSteps.Add(entityStep);

            }
            ret.FlowType = currentStep.Behavior.FlowType;
            ret.Steps = retSteps;
            ret.Users = users;
            return ret;
        }

        public  GetNextStepForRunOutput GetNextStepForRunSync(GetNextStepForRunInput input)
        {

            var currentTaskModel =  _workFlowTaskRepository.Get(input.TaskId);
            var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(currentTaskModel.FlowID, currentTaskModel.VersionNum);
            var currentStep = new WorkFlowStep();
            var entity = workflowModel.Steps.FirstOrDefault(r => r.ID == currentTaskModel.StepID);
            currentStep = entity ?? throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程数据错误，未找到对应的步骤。");
            var ret = new GetNextStepForRunOutput();
            ret.FlowType = currentStep.Behavior.FlowType;
            ret.SignatureType = currentStep.SignatureType;
            ret.SugguestionTitle = currentStep.SugguestionTitle;
            var nextSteps = _workFlowTaskManager.GetNextSteps(currentTaskModel.FlowID, currentTaskModel.StepID, currentTaskModel.VersionNum);


            //判断流转条件
            if ((currentStep.Behavior.FlowType == 0 || currentStep.Behavior.FlowType == 3) && nextSteps.Count() > 0)
            {
                List<Guid> removeIDList = new List<Guid>();
                var eventParams = new WorkFlowCustomEventParams();
                eventParams.FlowID = currentTaskModel.FlowID;
                eventParams.GroupID = currentTaskModel.GroupID;
                eventParams.StepID = currentTaskModel.StepID;
                eventParams.TaskID = input.TaskId;
                eventParams.InstanceID = currentTaskModel.InstanceID;

                System.Text.StringBuilder nosubmitMsg = new System.Text.StringBuilder();
                foreach (var step in nextSteps)
                {
                    var lines = workflowModel.Lines.Where(p => p.ToID == step.ID && p.FromID == currentStep.ID);
                    if (lines.Count() > 0)
                    {
                        var line = lines.First();
                        if (!line.SqlWhere.IsNullOrEmpty())
                        {
                            if (workflowModel.DataBases.Count() == 0)
                            {
                                removeIDList.Add(step.ID);
                                //nosubmitMsg.Append("流程未设置数据连接");
                                //nosubmitMsg.Append("\\n");
                            }
                            else
                            {
                                if (!_workFlowTaskManager.TestLineSql(workflowModel.DataBases.First().LinkID, workflowModel.DataBases.First().Table,
                                     workflowModel.DataBases.First().PrimaryKey, currentTaskModel.InstanceID, line.SqlWhere))
                                {
                                    removeIDList.Add(step.ID);
                                    //nosubmitMsg.Append(string.Concat("提交条件未满足"));
                                    //nosubmitMsg.Append("\\n");
                                }
                            }
                        }
                        if (!line.CustomMethod.IsNullOrEmpty())
                        {
                            //object obj = _workFlowTaskManager.ExecuteFlowCustomEvent(line.CustomMethod.Trim(), eventParams);
                            ///策略---流转条件----自定义方法判断 返回bool 或者1
                            dynamic obj = _workFlowTaskManager.ExecuteFlowCustomEvent(line.CustomMethod.Trim(), eventParams);
                            var objType = obj.GetType();
                            var boolType = typeof(Boolean);
                            if (objType != boolType && "1" != obj.ToString())
                            {
                                removeIDList.Add(step.ID);
                                nosubmitMsg.Append(obj.ToString());
                                nosubmitMsg.Append("\\n");
                            }
                            else if (objType == boolType && !(bool)obj)
                            {
                                removeIDList.Add(step.ID);
                                nosubmitMsg.Append(obj.ToString());
                                nosubmitMsg.Append("\\n");
                            }
                        }
                        #region 组织机构关系判断  暂时屏蔽掉
                        //var SenderID = AbpSession.UserId.Value;
                        //long sponserID = 0;//发起者ID
                        //if (currentStep.ID == workflowModel.FirstStepID)//如果是第一步则发起者就是发送者
                        //{
                        //    sponserID = SenderID;
                        //}
                        //else
                        //{
                        //    sponserID = _workFlowTaskManager.GetFirstSnderID(eventParams.FlowID, eventParams.GroupID);
                        //}
                        //System.Text.StringBuilder orgWheres = new System.Text.StringBuilder();
                        //if (!line.Organize.IsNullOrEmpty())
                        //{
                        //    LitJson.JsonData orgJson = LitJson.JsonMapper.ToObject(line.Organize);
                        //    foreach (LitJson.JsonData json in orgJson)
                        //    {
                        //        if (orgJson.Count == 0)
                        //        {
                        //            continue;
                        //        }
                        //        string usertype = json["usertype"].ToString();
                        //        string in1 = json.ContainsKey("in1") ? json["in1"].ToString() : "";
                        //        string users = json["users"].ToString();
                        //        string selectorganize = json["selectorganize"].ToString();
                        //        string tjand = json["tjand"].ToString();
                        //        string khleft = json["khleft"].ToString();
                        //        string khright = json["khright"].ToString();
                        //        long userid = "0" == usertype ? SenderID : sponserID;
                        //        string memberid = "";
                        //        bool isin = false;
                        //        if ("0" == users)
                        //        {
                        //            memberid = selectorganize;
                        //        }
                        //        else if ("1" == users)
                        //        {
                        //            memberid = busers.GetLeader(userid);
                        //        }
                        //        else if ("2" == users)
                        //        {
                        //            memberid = busers.GetChargeLeader(userid);
                        //        }
                        //        if ("0" == in1)
                        //        {
                        //            isin = busers.IsContains(userid, memberid);
                        //        }
                        //        else if ("1" == in1)
                        //        {
                        //            isin = !busers.IsContains(userid, memberid);
                        //        }
                        //        if (!khleft.IsNullOrEmpty())
                        //        {
                        //            orgWheres.Append(khleft);
                        //        }
                        //        orgWheres.Append(isin ? " true " : " false ");
                        //        if (!khright.IsNullOrEmpty())
                        //        {
                        //            orgWheres.Append(khright);
                        //        }
                        //        orgWheres.Append(tjand);
                        //    }
                        //    string orgCode = string.Concat("bool testbool=", orgWheres.ToString(), ";return testbool;");
                        //    object rogCodeResult = RoadFlow.Utility.Tools.ExecuteCsharpCode(orgCode);
                        //    if (rogCodeResult != null && !(bool)rogCodeResult)
                        //    {
                        //        removeIDList.Add(step.ID);
                        //    }
                        //}
                        #endregion
                    }
                }
                foreach (Guid rid in removeIDList)
                {
                    nextSteps.RemoveAll(p => p.ID == rid);
                }
                if (nextSteps.Count == 0)
                {
                    string alertMsg = nosubmitMsg.ToString();
                    alertMsg = alertMsg.IsNullOrEmpty() ? "后续步骤条件均不符合,任务不能提交!" : alertMsg;
                }
            }
            var retSteps = new List<GetNextStepOutput>();
            foreach (var step in nextSteps.OrderBy(p => p.Position_y).ThenBy(p => p.Position_x))
            {
                var entityStep = new GetNextStepOutput();
                var selectType = "";
                var selectRang = "";
                var defaultMember = _workFlowTaskManager.GetDefultMember(currentTaskModel.FlowID, step.ID, currentTaskModel.GroupID, currentStep.ID, currentTaskModel.InstanceID, out selectType, out selectRang,
                    input.TaskId, currentTaskModel.VersionNum);
                entityStep.DefaultUserId = defaultMember;
                entityStep.DefaultUserName = _workFlowOrganizationUnitsManager.GetNames(defaultMember);
                entityStep.NextStepId = step.ID;
                entityStep.NextStepName = step.Name;
                entityStep.IsAllowChoose = step.Behavior.RunSelect != 0;
                entityStep.SelectRangeRootId = step.Behavior.SelectRange?.Trim() ?? "";
                retSteps.Add(entityStep);
            }
            ret.FlowType = currentStep.Behavior.FlowType;
            ret.Steps = retSteps;
            return ret;
        }

        public async Task<GetBackStepsForRunOutput> GetBackStepsForRun(GetBackStepsForRunInput input)
        {
            var currentTaskModel = await _workFlowTaskRepository.GetAsync(input.TaskId);
            var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId, currentTaskModel.VersionNum);
            var currentStep = new WorkFlowStep();
            var entity = workflowModel.Steps.FirstOrDefault(r => r.ID == input.StepId);
            currentStep = entity ?? throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程数据错误");
            var ret = new GetBackStepsForRunOutput();
            ret.BackModel = currentStep.Behavior.BackModel;
            ret.BackType = currentStep.Behavior.BackType;
            ret.SignatureType = currentStep.SignatureType;
            ret.SugguestionTitle = currentStep.SugguestionTitle;
            var currentTask = _workFlowTaskRepository.Get(input.TaskId);
            if (ret.BackModel == 0)
            {
                ret.Result = BackStepResult.不能退回;
                return ret;
            }
            else if (ret.BackModel == 3)
            {
                if (_workFlowTaskManager.GetTaskList2(input.TaskId).Any(p => p.Status > 1))
                {
                    ret.Result = BackStepResult.当前任务不能退回;
                    return ret;
                }

            }
            var preSteps = _workFlowTaskManager.GetBackSteps(input.TaskId, ret.BackType, currentStep.ID, workflowModel);
            foreach (var item in preSteps)
            {
                var entityStep = new GetBackStepsOutput() { BackStepId = item.Key, BackStepName = item.Value };
                var domembers = _workFlowTaskManager.GetStepHasPassMember(input.FlowId, input.GroupId, item.Key, currentTask.Sort - 1);
                foreach (var memeber in domembers)
                {
                    entityStep.BackUsers.Add(new GetBackStepsUserOutput() { UserIdWithPerfix = $"u_{memeber.Key}", UserName = memeber.Value });
                }
                ret.Steps.Add(entityStep);
            }

            return ret;
        }

        public async Task<List<GetWorkFlowInstanceStatusOutput>> GetStatussAsync(WorkFlowBaseInput input)
        {
            var flow = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            var ret = new List<GetWorkFlowInstanceStatusOutput>();
            foreach (var item in flow.Steps)
            {
                var entity = new GetWorkFlowInstanceStatusOutput()
                {
                    Status = item.StepToStatus,
                    StatusSummary = item.StepToStatusTitle
                };
                ret.Add(entity);
            }

            return ret;

        }

        /// <summary>
        /// 得到一个实例的任务
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<WorkFlowTask> GetTaskList(string instanceId, Guid flowID, Guid? groupID)
        {
            return _workFlowTaskManager.GetTaskList4(instanceId, flowID, groupID);
        }

        public void GetWorkFlowIn(Guid fileId, bool isNew, string extName)
        {
            try
            {
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var workFlowModelNews = new List<WorkFlowModelNew>();
                    var fileModel = _abpFilerepository.Get(fileId);
                    var json = System.IO.File.ReadAllText(fileModel.FilePath, Encoding.UTF8);
                    var content = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkFlowListOutIn>(json);
                    WorkFlow workFlow = new WorkFlow();
                    var isNewWorkFlow = true;
                    if (_workFlowRepository.GetAll().Any(x => x.Id == content.workFlow.Id))
                    {
                        workFlow = _workFlowRepository.GetAll().First(x => x.Id == content.workFlow.Id);
                        isNewWorkFlow = false;
                    }
                    content.workFlow.MapTo(workFlow);

                    if (isNew)
                    {
                        workFlow.Id = Guid.NewGuid();
                        workFlow.CreationTime = DateTime.Now;
                        workFlow.Name = workFlow.Name + extName;
                    }
                    else
                    {
                        if (isNewWorkFlow)
                        {
                            _workFlowRepository.Insert(workFlow);
                            InserWorkFlowVersionNum(workFlow);
                        }
                        else
                        {
                            var model = _workFlowRepository.Get(workFlow.Id);
                            workFlow.VersionNum = (model.VersionNum + 1);
                            InserWorkFlowVersionNum(workFlow);
                            _workFlowRepository.Update(workFlow);
                        }
                    }

                    foreach (var item in content.workFlowModel)
                    {

                        isNewWorkFlow = true;
                        WorkFlowModel workFlowModel = new WorkFlowModel();
                        if (_workFlowModel.GetAll().Any(x => x.Id == item.Id))
                        {
                            workFlowModel = _workFlowModel.GetAll().First(x => x.Id == item.Id);
                            isNewWorkFlow = false;
                        }
                        item.MapTo(workFlowModel);
                        if (isNew)
                        {
                            workFlowModel.Id = Guid.NewGuid();
                            workFlowModel.Name = workFlowModel.Name + extName;
                            _workFlowModel.Insert(workFlowModel);
                            workFlowModelNews.Add(new WorkFlowModelNew { OldGuid = item.Id, NewGuid = workFlowModel.Id });
                        }
                        else
                        {
                            if (isNewWorkFlow)
                                _workFlowModel.Insert(workFlowModel);
                            else
                                _workFlowModel.Update(workFlowModel);
                        }
                    }
                    foreach (var item in content.workFlowModelColumn)
                    {
                        isNewWorkFlow = true;
                        WorkFlowModelColumn workFlowModelColumn = new WorkFlowModelColumn();
                        if (_workFlowModelColumn.GetAll().Any(x => x.Id == item.Id))
                        {
                            workFlowModelColumn = _workFlowModelColumn.GetAll().First(x => x.Id == item.Id);
                            isNewWorkFlow = false;
                        }
                        item.MapTo(workFlowModelColumn);
                        if (isNew)
                        {
                            workFlowModelColumn.Id = Guid.NewGuid();
                            var workFlowModelNew = workFlowModelNews.FirstOrDefault(x => x.OldGuid == workFlowModelColumn.WorkFlowModelId);
                            workFlowModelColumn.WorkFlowModelId = workFlowModelNew.NewGuid;
                            _workFlowModelColumn.Insert(workFlowModelColumn);
                        }
                        else
                        {
                            if (isNewWorkFlow)
                                _workFlowModelColumn.Insert(workFlowModelColumn);
                            else
                                _workFlowModelColumn.Update(workFlowModelColumn);
                        }
                    }

                    foreach (var item in content.workFlowTemplate)
                    {
                        isNewWorkFlow = true;
                        WorkFlowTemplate workFlowTemplate = new WorkFlowTemplate();
                        if (_workFlowTemplate.GetAll().Any(x => x.Id == item.Id))
                        {
                            workFlowTemplate = _workFlowTemplate.GetAll().First(x => x.Id == item.Id);
                            isNewWorkFlow = false;
                        }
                        item.MapTo(workFlowTemplate);
                        if (isNew)
                        {
                            workFlowTemplate.Id = Guid.NewGuid();
                            var workFlowModelNew = workFlowModelNews.FirstOrDefault(x => x.OldGuid == workFlowTemplate.WorkFlowModelId);
                            workFlowTemplate.WorkFlowModelId = workFlowModelNew.NewGuid;
                            _workFlowTemplate.Insert(workFlowTemplate);
                        }
                        else
                        {
                            if (isNewWorkFlow)
                                _workFlowTemplate.Insert(workFlowTemplate);
                            else
                                _workFlowTemplate.Update(workFlowTemplate);
                        }
                    }
                    if (isNew)
                    {
                        foreach (var item in workFlowModelNews)
                        {
                            workFlow.DesignJSON = workFlow.DesignJSON.Replace(item.OldGuid.ToString(), item.NewGuid.ToString());
                            workFlow.RunJSON = workFlow.RunJSON.Replace(item.OldGuid.ToString(), item.NewGuid.ToString());
                        }
                        _workFlowRepository.Insert(workFlow);
                        InserWorkFlowVersionNum(workFlow);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "错误：" + ex.Message);
            }
        }

        public void InserWorkFlowVersionNum(WorkFlow workFlow)
        {
            var wfCacheModel = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkFlowInstalled>(workFlow.RunJSON);
            wfCacheModel.VersionNum = workFlow.VersionNum;
            _workFlowCacheManager.SetWorkFlowModelCache(workFlow.Id.ToString(), wfCacheModel);
            _workFlowCacheManager.SetWorkFlowModelCache($"{workFlow.Id}-{workFlow.VersionNum}", wfCacheModel);

            var entity = new WorkFlowVersionNum() { Id = Guid.NewGuid(), FlowId = workFlow.Id, RunJSON = workFlow.RunJSON, VersionNum = workFlow.VersionNum };
            _workFlowVersionNumRepository.Insert(entity);
        }
        public string GetWorkFlowOut(Guid flowID)
        {
            var workFlow = _workFlowRepository.GetAll().First(x => x.Id == flowID);
            var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(workFlow.Id);
            List<WorkFlowModel> workFlowModels = new List<WorkFlowModel>();
            List<WorkFlowModelColumn> workFlowModelColumns = new List<WorkFlowModelColumn>();
            List<WorkFlowTemplate> workFlowTemplates = new List<WorkFlowTemplate>();
            foreach (var item in workflowModel.Steps.Where(x => x.WorkFlowModelId != null))
            {
                var guid = Guid.Parse(item.WorkFlowModelId.ToString());
                var model = _workFlowModel.GetAll().First(x => x.Id == guid);
                if(!workFlowModels.Any(x=>x.Id==model.Id))
                    workFlowModels.Add(model);
                var columnModels = _workFlowModelColumn.GetAll().Where(x => x.WorkFlowModelId == model.Id).ToList();
                foreach (var column in columnModels)
                {
                    if (!workFlowModelColumns.Any(x => x.Id == column.Id))
                        workFlowModelColumns.Add(column);
                }

                var templates = _workFlowTemplate.GetAll().Where(x => x.WorkFlowModelId == model.Id).ToList();
                foreach (var template in templates)
                {
                    if (!workFlowTemplates.Any(x => x.Id == template.Id))
                        workFlowTemplates.Add(template);
                }
            }

            var json =
                new WorkFlowListOutIn
                {
                    workFlow = workFlow,
                    workFlowModel = workFlowModels.ToList(),
                    workFlowModelColumn = workFlowModelColumns.ToList(),
                    workFlowTemplate = workFlowTemplates.ToList()
                };

            var content = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            string filePath = hostingEnv.WebRootPath + $@"\Files\upload\" + flowID + ".json";
            System.IO.File.WriteAllText(filePath, content, Encoding.UTF8);
            var isNewFile = true;
            var entity = new AbpFile();
            if (_abpFilerepository.GetAll().Any(x => x.Id == flowID))
            {
                entity = _abpFilerepository.GetAll().First(x => x.Id == flowID);
                isNewFile = false;
            }
            entity.FilePath = filePath;
            entity.FileName = workFlow.Name + ".json";
            entity.FileExtend = "json";
            entity.FileSize = 0;
            if (isNewFile)
            {
                entity.Id = flowID;
                _abpFilerepository.Insert(entity);
            }
            else
                _abpFilerepository.Update(entity);
            return entity.Id.ToString();
        }


        public void GetWorkFlowCopy(Guid flowID)
        {
            var workFlow = _workFlowRepository.GetAll().First(x => x.Id == flowID);
            var newWorkFlow = new WorkFlow();
            workFlow.MapTo(newWorkFlow);
            newWorkFlow.Id = Guid.NewGuid();
            newWorkFlow.CreationTime = DateTime.Now;
            newWorkFlow.Name = workFlow.Name + " (复制)";
            var workFlowModelNews = new List<WorkFlowModelNew>();

            var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowID);
            if (workflowModel != null)
            {
                foreach (var item in workflowModel.Steps.Where(x => x.WorkFlowModelId != null))
                {
                    var guid = Guid.Parse(item.WorkFlowModelId.ToString());
                    var model = _workFlowModel.GetAll().First(x => x.Id == guid);
                    var newModel = new WorkFlowModel();
                    model.MapTo(newModel);
                    newModel.Id = Guid.NewGuid();
                    newModel.Name = model.Name + " (复制)";
                    _workFlowModel.Insert(newModel);
                    workFlowModelNews.Add(new WorkFlowModelNew { OldGuid = model.Id, NewGuid = newModel.Id });
                }
                foreach (var item in workflowModel.Steps.Where(x => x.WorkFlowModelId != null))
                {
                    var guid = Guid.Parse(item.WorkFlowModelId.ToString());
                    var model = _workFlowModel.GetAll().First(x => x.Id == guid);
                    var columnModels = _workFlowModelColumn.GetAll().Where(x => x.WorkFlowModelId == model.Id).ToList();
                    foreach (var column in columnModels)
                    {
                        var newModel = new WorkFlowModelColumn();
                        column.MapTo(newModel);

                        newModel.Id = Guid.NewGuid();
                        var workFlowModelNew = workFlowModelNews.FirstOrDefault(x => x.OldGuid == column.WorkFlowModelId);
                        newModel.WorkFlowModelId = workFlowModelNew.NewGuid;
                        _workFlowModelColumn.Insert(newModel);
                    }
                    var templates = _workFlowTemplate.GetAll().Where(x => x.WorkFlowModelId == model.Id).ToList();
                    foreach (var template in templates)
                    {
                        var newModel = new WorkFlowTemplate();
                        template.MapTo(newModel);
                        newModel.Id = Guid.NewGuid();
                        var workFlowModelNew = workFlowModelNews.FirstOrDefault(x => x.OldGuid == template.WorkFlowModelId);
                        newModel.WorkFlowModelId = workFlowModelNew.NewGuid;
                        _workFlowTemplate.Insert(newModel);
                    }
                }
            }
            foreach (var item in workFlowModelNews)
            {
                newWorkFlow.DesignJSON = newWorkFlow.DesignJSON
                    .Replace(item.OldGuid.ToString(), item.NewGuid.ToString())
                    .Replace(workFlow.Id.ToString(), newWorkFlow.Id.ToString());
                newWorkFlow.RunJSON = newWorkFlow.RunJSON
                    .Replace(item.OldGuid.ToString(), item.NewGuid.ToString())
                    .Replace(workFlow.Id.ToString(), newWorkFlow.Id.ToString());
            }
            _workFlowRepository.Insert(newWorkFlow);
        }

        /// <summary>
        /// 工作流删除第一步数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteFlowFirstData(DeleteFlowFirstStepDataIn input)
        {
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var wfInstalled = workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            if (wfInstalled.FirstStepID != input.StepId)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "非第一步不能删除。");
            var model = _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == input.InstanceID && x.ReceiveID == AbpSession.UserId.Value && x.FlowID == input.FlowId && x.StepID == input.StepId).FirstOrDefault();
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            if (model.Status == 0 || model.Status == 1)
            {
                if (_workFlowTaskRepository.GetAll().Any(x => x.SubFlowGroupID.Contains(model.GroupID.ToString())))
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前为子流程步骤，不能删除。");
                var tasks = _workFlowTaskRepository.GetAll().Where(x => x.InstanceID == input.InstanceID && x.FlowID == input.FlowId).ToList();
                foreach (var item in tasks)
                {
                    item.Status = 11;
                    await _workFlowTaskRepository.UpdateAsync(item);
                }
                if (wfInstalled.TitleField != null && wfInstalled.TitleField.LinkID != Guid.Empty && !wfInstalled.TitleField.Table.IsNullOrEmpty()
                    && !wfInstalled.TitleField.Field.IsNullOrEmpty() && wfInstalled.DataBases.Count() > 0)
                {
                    var firstDB = wfInstalled.DataBases.First();
                    string sql = string.Format("UPDATE {0} SET IsDeleted=1,DeleterUserId={1} WHERE {2} and CreatorUserId={1}", wfInstalled.TitleField.Table, AbpSession.UserId.Value, string.Format("{0}='{1}'", firstDB.PrimaryKey, input.InstanceID));
                    var i = _workFlowTaskRepository.CompletaWorkFlowInstanceExecuteSql(sql);
                    if (i < 1)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "非创建者不能删除。");
                }
            }
            else
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "待办不在处理范围。");
        }


        
    }
}
