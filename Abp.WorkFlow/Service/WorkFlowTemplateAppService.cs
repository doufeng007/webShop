using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.WorkFlow.Service.Dto;
using Abp.Dapper.Repositories;
using Abp.WorkFlow.Entity;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using System.Linq;
using Abp.UI;
using Dapper;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using Abp.Extensions;
using Abp.Application.Services.Dto;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using ZCYX.FRMSCore.Model;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Authorization;
using Microsoft.AspNetCore.Http;

namespace Abp.WorkFlow.Service
{
    public class WorkFlowTemplateAppService : ApplicationService, IWorkFlowTemplateAppService
    {
        private readonly IRepository<WorkFlowTemplate, Guid> _workFlowTemplateRepository;
        private readonly IRepository<WorkFlowTemplateLog, Guid> _workFlowTemplateLogRepository;
        private readonly IRepository<WorkFlowModel,Guid> _workFlowModelRepository;
        private readonly IRepository<WorkFlowModelColumn, Guid> _workFlowModelColumnRepository;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<User, long> _userRepository;
        private IHttpContextAccessor _accessor;

        private IHostingEnvironment _hostingEnv;

        public WorkFlowTemplateAppService(IRepository<WorkFlowTemplate, Guid> workFlowTemplateRepository, IRepository<User, long> userRepository,
            IRepository<WorkFlowModel, Guid> workFlowModelRepository, IHostingEnvironment hostingEnv, IHttpContextAccessor accessor,
            IRepository<WorkFlowModelColumn, Guid> workFlowModelColumnRepository, IRepository<WorkFlowTemplateLog, Guid> workFlowTemplateLogRepository,
            IDynamicRepository dynamicRepository, WorkFlowCacheManager workFlowCacheManager
            ) {
            _workFlowTemplateRepository = workFlowTemplateRepository;
            _workFlowModelRepository = workFlowModelRepository;
            _workFlowModelColumnRepository = workFlowModelColumnRepository;
            _dynamicRepository = dynamicRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _hostingEnv = hostingEnv;
            _userRepository = userRepository;
            _accessor = accessor;
            _workFlowTemplateLogRepository = workFlowTemplateLogRepository;
        }
        public Guid CreateOrUpdateModel(WorkFlowModelDto model)
        {
            if (model.Id.HasValue == false)
            {
                var m = model.MapTo<WorkFlowModel>();
                var mid = _workFlowModelRepository.InsertAndGetId(m);
                foreach (var c in model.Columnes)
                {
                    var cm = c.MapTo<WorkFlowModelColumn>();
                    cm.WorkFlowModelId = mid;
                    _workFlowModelColumnRepository.Insert(cm);
                }
                return mid;
            }
            else
            {
                var m = _workFlowModelRepository.Get(model.Id.Value);
                m= model.MapTo(m);
                _workFlowModelRepository.Update(m);
                _workFlowModelColumnRepository.Delete(ite => ite.WorkFlowModelId == m.Id);
                foreach (var c in model.Columnes) {
                    var cm = c.MapTo<WorkFlowModelColumn>();
                    cm.WorkFlowModelId = m.Id;
                    cm.Id = Guid.NewGuid();
                    _workFlowModelColumnRepository.Insert(cm);
                }
                return m.Id;
            }
        }

        public PagedResultDto<WorkFlowModelListDto> GetModels(PagedAndSortedInputDto input)
        {

            var query = _workFlowModelRepository.GetAll();
            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Name.Contains(input.SearchKey));
            }
            var total = query.Count();
            var model = query.OrderByDescending(ite => ite.Name).PageBy(input).ToList().MapTo<List<WorkFlowModelListDto>>();
            return new PagedResultDto<WorkFlowModelListDto>(total,model);
        }
        public WorkFlowModelDto GetModel(Guid id)
        {
            var m = _workFlowModelRepository.Get(id).MapTo<WorkFlowModelDto>();
            var mc = _workFlowModelColumnRepository.GetAll().Where(it => it.WorkFlowModelId == m.Id).OrderBy(ite=>ite.Sort).MapTo<List<WorkFlowModelColumnDto>>();
            m.Columnes = mc;
            return m;
        }

        public IEnumerable<dynamic> GetTableColumnes(string tableName)
        {
            DynamicParameters Parameters = new DynamicParameters();
            Parameters.Add("TableName",tableName);
            var sql= "SELECT SYSCOLUMNS.name as code,SYSCOLUMNS.xtype as dataType,SYSCOLUMNS.length as maxLength,SYSCOLUMNS.isnullable as isRequired FROM SYSCOLUMNS   WHERE ID=OBJECT_ID(@TableName)";
            //var sql = "SELECT SYSCOLUMNS.name as code,sysTypes.name as dataType,SYSCOLUMNS.length as maxLength,SYSCOLUMNS.isnullable as isRequired FROM SYSCOLUMNS left join sysTypes on SYSCOLUMNS.xtype=sysTypes.xtype   WHERE ID=OBJECT_ID(@TableName)";
            var ret = _dynamicRepository.Query(sql,Parameters);
            return ret;
        }

        public IEnumerable<dynamic> GetTables()
        {
            var sql = "SELECT NAME as tableName FROM SYSOBJECTS WHERE TYPE='U' and name !='__EFMigrationsHistory' order by name";
            var ret = _dynamicRepository.Query(sql);
            return ret;
        }

        public WorkFlowTemplateDto GetTemplate(GetTemplateInput input)
        {
            throw new NotImplementedException();
        }
        public WorkFlowTemplateDto GetTemplateByModelId(Guid modelId, TemplateType type= TemplateType.编辑模版)
        {
            var m = _workFlowTemplateRepository.GetAll().FirstOrDefault(ite => ite.TemplateType == type && ite.WorkFlowModelId == modelId);
            if (m == null)
            {
                //throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前模版不存在。");
                return null;
            }
            var ret= m.MapTo<WorkFlowTemplateDto>(); 
            var temproot = _hostingEnv.ContentRootPath + @"\wwwroot\templates\";//模板文件夹
            if (!Directory.Exists(temproot))
            {
                Directory.CreateDirectory(temproot);
            }
            var t = "";
            switch (type)
            {
                case TemplateType.打印模版:
                    t = "detail";
                    break;
                case TemplateType.编辑模版:
                    t = "edit";
                    break;
            }
            var temfilename = $"{modelId}-{t}.vue";//模板文件名
            if (System.IO.File.Exists(temproot + $@"\{temfilename}"))//如果存在文件模板 则从文件中读取，没有则默认取数据库的值
            {
                var content = System.IO.File.ReadAllText(temproot + $@"\{temfilename}");
                ret.VueTemplate = content;
            }
            return ret;
        }
        /// <summary>
        /// 根据工作流id获取工作流第一步模型对应的模版
        /// </summary>
        /// <param name="flowId">工作流id</param>
        /// <param name="type">模版类型 默认0编辑模版 1详情视图</param>
        /// <returns></returns>
        public WorkFlowTemplateDto GetTemplateByFlowId(Guid flowId, TemplateType type = TemplateType.编辑模版) {
            var workflowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(flowId);
            if (workflowModel == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到流程定义。");
            }
            var currentStep = workflowModel.Steps.FirstOrDefault();
            var mid = currentStep.WorkFlowModelId;
            if (mid.HasValue == false) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程第一步未定义关联模型。");
            }
            var m = _workFlowTemplateRepository.GetAll().FirstOrDefault(ite => ite.TemplateType == type && ite.WorkFlowModelId == mid);
            if (m != null)
            {
                return m.MapTo<WorkFlowTemplateDto>();
            }
            throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "当前模版不存在。");
        }
        [AbpAuthorize]
        public void SaveTemplate(WorkFlowTemplateDto template)
        {
            var m = template.MapTo<WorkFlowTemplate>();
            var has = _workFlowTemplateRepository.GetAll().FirstOrDefault(ite=>ite.WorkFlowModelId==m.WorkFlowModelId&&ite.TemplateType==m.TemplateType);

            if (has != null)
            {
                if (has.IsLocked == false) {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "请先锁定后再编辑文件。");
                }
                var ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                if (has.LastLockIP != ip)
                {
                    var username = "";
                    if (has.LastLockUserId.HasValue)
                    {
                        var user = _userRepository.FirstOrDefault(has.LastLockUserId.Value);
                        if (user != null)
                        {
                            username = user.Name;
                        }
                    }
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"当前模板已经被【{username}-{has.LastLockIP}-{has.LastLockTime}】锁定编辑，请先解锁后再编辑。");
                }
                has.IsLocked = false;
                var vernum = 0;
                var ver = _workFlowTemplateLogRepository.GetAll().OrderByDescending(ite => ite.VersionNum).FirstOrDefault(ite => ite.TemplateId == has.Id);
                if (ver != null) {
                    vernum = ver.VersionNum + 1;
                }
                _workFlowTemplateLogRepository.Insert(new WorkFlowTemplateLog()
                {
                    EditTime = DateTime.Now,
                    EditUserId = AbpSession.UserId.Value,
                    TemplateId = has.Id,
                    VersionNum = vernum,
                    VueTemplate = has.VueTemplate,
                     LastLockIP=ip
                });
                has.VueTemplate = m.VueTemplate;
                _workFlowTemplateRepository.Update(has);
            }
            else
            {
                _workFlowTemplateRepository.Insert(m);
            }
            //var temproot = _hostingEnv.ContentRootPath + @"\wwwroot\templates\";//模板文件夹
            //if (!Directory.Exists(temproot))
            //{
            //    Directory.CreateDirectory(temproot);
            //}
            //var t = "";
            //switch (template.TemplateType)
            //{
            //    case TemplateType.打印模版:
            //        t = "detail";
            //        break;
            //    case TemplateType.编辑模版:
            //        t = "edit";
            //        break;
            //}
            //var temfilename = $"{m.WorkFlowModelId}-{t}.vue";//模板文件名
            //System.IO.File.WriteAllText(temproot + $@"\{temfilename}", m.VueTemplate);

        }

        
        public void SaveTemplates(List<WorkFlowTemplateDto> templates)
        {
            throw new NotImplementedException();
        }

        public dynamic TestSave(dynamic model)
        {
            throw new NotImplementedException();
        }

        public List<WorkFlowModelDto> GetModelRelations(Guid id)
        {
            var ret = new List<WorkFlowModelDto>();
            var m = _workFlowModelRepository.Get(id).MapTo<WorkFlowModelDto>();
            var mc = _workFlowModelColumnRepository.GetAll().OrderBy(ite=>ite.Sort).Where(it => it.WorkFlowModelId == m.Id).MapTo<List<WorkFlowModelColumnDto>>();
            m.Columnes = mc;
            ret.Add(m);
            var rid = m.Columnes.Where(ite => string.IsNullOrEmpty(ite.Relation) == false&&ite.FieldType!= FieldType.多对一).Select(ite => ite.Relation).ToList();
            if (rid != null && rid.Count > 0)
            {
                foreach (var r in rid)
                {
                    
                    var t = _workFlowModelRepository.Get(new Guid(r)).MapTo<WorkFlowModelDto>();
                    var tc = _workFlowModelColumnRepository.GetAll().OrderBy(ite => ite.Sort).Where(it => it.WorkFlowModelId == t.Id).MapTo<List<WorkFlowModelColumnDto>>();
                    t.Columnes = tc;
                    ret.Add(t);
                }
            }
            return ret;
        }

        public IEnumerable<dynamic> GetModelSelect(GetModelSelectInput input) {
            var model = _workFlowModelRepository.Get(input.ModelId);
            if (model == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到关联的模型定义。");
            }
            var titleFiled = model.TitleCode;
            if (string.IsNullOrWhiteSpace(titleFiled)) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"模型【{model.Code}】未定义标题字段。");
            }
            var sql = $"select id,{titleFiled} as name from {model.Code} where IsDeleted=0 or IsDeleted is null";
            var ret= _dynamicRepository.Query(sql).ToList();
            return ret;
        }
        /// <summary>
        /// 锁定编辑
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public bool LockEdit(Guid templateId)
        {
            var t = _workFlowTemplateRepository.Get(templateId);
            var username = "";
            if (t.IsLocked)
            {
                if (t.LastLockUserId.HasValue)
                {
                    var user = _userRepository.FirstOrDefault(t.LastLockUserId.Value);
                    if (user != null)
                    {
                        username = user.Name;
                    }
                }
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"当前模板已经被【{username}-{t.LastLockIP}-{t.LastLockTime}】锁定编辑，请先解锁后再编辑。");
            }
            else {
                t.IsLocked = true;
                t.LastLockUserId = AbpSession.UserId.Value;
                t.LastLockTime = DateTime.Now;
                t.LastLockIP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return false;
        }
        /// <summary>
        /// 强制解锁
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public bool UnLock(Guid templateId) {
            var t = _workFlowTemplateRepository.Get(templateId);
            t.IsLocked = false;
            return true;
        }
    }
}
