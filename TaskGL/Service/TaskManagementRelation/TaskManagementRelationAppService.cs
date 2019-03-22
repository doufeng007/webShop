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
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Application
{
    public class TaskManagementRelationAppService : FRMSCoreAppServiceBase, ITaskManagementRelationAppService
    {
        private readonly IRepository<TaskManagementRelation, Guid> _repository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IDynamicRepository _dynamicRepository;
        public TaskManagementRelationAppService(IRepository<TaskManagementRelation, Guid> repository, WorkFlowCacheManager workFlowCacheManager, IDynamicRepository dynamicRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager

        )
        {
            this._repository = repository;
            _workFlowCacheManager = workFlowCacheManager;
            _dynamicRepository = dynamicRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<TaskManagementRelationListOutputDto>> GetList(GetTaskManagementRelationListInput input)
        {
            var query = from a in _repository.GetAll()

                        select new TaskManagementRelationListOutputDto()
                        {
                            Id = a.Id,
                            FlowId = a.FlowId,
                            InStanceId = a.InStanceId,
                            TaskManagementId = a.TaskManagementId

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.PageBy(input).ToListAsync();

            return new PagedResultDto<TaskManagementRelationListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<TaskManagementRelationOutputDto> Get(GetTaskManagementRelationInput input)
        {
            var models = _repository.GetAll().Where(x => x.FlowId == input.FlowId && x.TaskManagementId == input.RelationTaskId).Select(x => x.InStanceId).ToList();
            if (models == null)
                return null;

            var currentwfInstallModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
       
            if (currentwfInstallModel.TitleField != null && !currentwfInstallModel.TitleField.Table.IsNullOrEmpty()
                && !currentwfInstallModel.TitleField.Field.IsNullOrEmpty() && currentwfInstallModel.DataBases.Any())
            {
                var firstDB = currentwfInstallModel.DataBases.First();
                try
                {
                    var indata = "";
                    foreach (var item in models)
                    {
                        indata += "'" + item + "',";
                    }

                    var query_Sql = $"select top 1 Id,Status  from {currentwfInstallModel.TitleField.Table}  where {firstDB.PrimaryKey} in ({indata.TrimEnd(',')}) order by CreationTime desc";
                    var dynamicModel = _dynamicRepository.QueryFirst(query_Sql);
                    if (dynamicModel != null)
                    {
                        var model = new TaskManagementRelationOutputDto();
                        model.InstanceId = Convert.ToString(dynamicModel.Id);
                        model.Status = dynamicModel.Status;
                        _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, model);
                        return model;
                    }
                }
                catch (Exception e)
                {
                }
            }

            return null;
        }

    }
}