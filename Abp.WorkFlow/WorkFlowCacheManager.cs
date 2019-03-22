using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace Abp.WorkFlow
{
    [RemoteService(IsEnabled = false)]
    public class WorkFlowCacheManager : ApplicationService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<WorkFlow, Guid> _workFlowRepository;
        private readonly IRepository<WorkFlowVersionNum, Guid> _workFlowVersionNumRepository;

        public WorkFlowCacheManager(ICacheManager cacheManager, IRepository<WorkFlow, Guid> workFlowRepository, IRepository<WorkFlowVersionNum, Guid> workFlowVersionNumRepository)
        {
            _cacheManager = cacheManager;
            _workFlowRepository = workFlowRepository;
            _workFlowVersionNumRepository = workFlowVersionNumRepository;
        }

        public WorkFlowInstalled GetWorkFlowModelFromCache(Guid flowId, int? versionNumber = null)
        {
            var cacheName = "InstalledWorkFlow";
            return _cacheManager
               .GetCache(cacheName)
               .Get<string, WorkFlowInstalled>(versionNumber.HasValue ? $"{flowId}-{versionNumber}" : flowId.ToString(), f => GetWorkFlowModel(flowId, versionNumber));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flowVersionNumStr">flowId-versionNumber</param>
        /// <param name="data"></param>
        public void SetWorkFlowModelCache(string flowVersionNumStr, WorkFlowInstalled data)
        {
            var cacheName = "InstalledWorkFlow";
            _cacheManager.GetCache(cacheName).SetAsync(flowVersionNumStr, data);
        }

        private WorkFlowInstalled GetWorkFlowModel(Guid flowId, int? versionNumber = null)
        {
            var query = _workFlowRepository.GetAll().FirstOrDefault(r => r.Id == flowId);
            if (query == null) return null;
            var model = new WorkFlowInstalled();
            if (!versionNumber.HasValue || query.VersionNum == versionNumber)
            {
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkFlowInstalled>(query.RunJSON);
                model.VersionNum = query.VersionNum;
                model.IsChange = query.IsChange;
                model.IsFiles = query.IsFiles;
            }
            else
            {
                var old_VersionModel = _workFlowVersionNumRepository.FirstOrDefault(r => r.VersionNum == versionNumber && r.FlowId == flowId);
                if (old_VersionModel != null)
                {
                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkFlowInstalled>(old_VersionModel.RunJSON);
                    model.VersionNum = versionNumber.Value;
                    model.IsChange = query.IsChange;
                    model.IsFiles = query.IsFiles;
                }
                else
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取工作流历史版本数据异常");
                }
            }

            if (model.Steps.Count() > 0)
            {
                model.FirstStepID = model.Steps.Select(r => r.ID).Except(model.Lines.Select(r => r.ToID)).FirstOrDefault();
            }
            return model;
        }






    }
}
