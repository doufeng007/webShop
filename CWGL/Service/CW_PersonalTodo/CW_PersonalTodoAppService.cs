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
using Abp.Authorization;

namespace CWGL
{
    public class CW_PersonalTodoAppService : FRMSCoreAppServiceBase, ICW_PersonalTodoAppService
    {
        private readonly IRepository<CW_PersonalTodo, Guid> _repository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;

        public CW_PersonalTodoAppService(IRepository<CW_PersonalTodo, Guid> repository, WorkFlowCacheManager workFlowCacheManager

        )
        {
            this._repository = repository;
            _workFlowCacheManager = workFlowCacheManager;


        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<CW_PersonalTodoListOutputDto>> GetList(GetCW_PersonalTodoListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        where a.UserId == AbpSession.UserId.Value
                        select new CW_PersonalTodoListOutputDto()
                        {
                            Id = a.Id,
                            BusinessId = a.BusinessId,
                            BusinessType = a.BusinessType,
                            CWType = a.CWType,
                            Amout_Pay = a.Amout_Pay,
                            Amout_Gather = a.Amout_Gather,
                            Status = a.Status,
                            Remark = a.Remark,
                            FlowId = a.FlowId,
                            CreationTime = a.CreationTime,
                            HasSubmitGather = a.HasSubmitGather,
                            HasSubmitPay = a.HasSubmitPay,
                            Title = a.Title,
                            BusinessType_Name = a.BusinessType.ToString(),
                            CWType_Name = a.CWType.ToString(),
                            StatusTitle = a.Status.ToString(),


                        };
            if (input.ActionType.HasValue)
            {
                if (input.ActionType == Enums.RefundActionType.付款)
                {
                    query = query.Where(r => (r.CWType == Enums.RefundResultType.财务应付款 || r.CWType == Enums.RefundResultType.收付款) && !r.HasSubmitPay);
                }
                else if (input.ActionType == Enums.RefundActionType.收款)
                {
                    query = query.Where(r => (r.CWType == Enums.RefundResultType.财务应收款 || r.CWType == Enums.RefundResultType.收付款) && !r.HasSubmitGather);
                }
            }


            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                if (item.FlowId.HasValue)
                {
                    var wfInstalled = _workFlowCacheManager.GetWorkFlowModelFromCache(item.FlowId.Value);
                    var firstStep = wfInstalled.Steps.FirstOrDefault();
                    item.WorkFlowModelId = firstStep.WorkFlowModelId;
                }
            }

            return new PagedResultDto<CW_PersonalTodoListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<CW_PersonalTodoOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<CW_PersonalTodoOutputDto>();
            ret.BusinessType_Name = ret.BusinessType.ToString();
            ret.CWType_Name = ret.CWType.ToString();
            ret.StatusTitle = ret.Status.ToString();
            return ret;
        }
        /// <summary>
        /// 添加一个CW_PersonalTodo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateCW_PersonalTodoInput input)
        {
            var newmodel = new CW_PersonalTodo()
            {
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType,
                CWType = input.CWType,
                Amout_Pay = input.Amout_Pay,
                Amout_Gather = input.Amout_Gather,
                Status = input.Status,
                Remark = input.Remark,
                FlowId = input.FlowId,
                Title = input.Title,
                UserId = input.UserId,
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个CW_PersonalTodo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateCW_PersonalTodoInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.BusinessId = input.BusinessId;
                dbmodel.BusinessType = input.BusinessType;
                dbmodel.CWType = input.CWType;
                dbmodel.Amout_Pay = input.Amout_Pay;
                dbmodel.Amout_Gather = input.Amout_Gather;
                dbmodel.Status = input.Status;
                dbmodel.Remark = input.Remark;
                dbmodel.FlowId = input.FlowId;
                dbmodel.Title = input.Title;
                dbmodel.UserId = input.UserId;

                await _repository.UpdateAsync(dbmodel);

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }




        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }
    }
}