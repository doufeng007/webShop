﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    public interface IB_PaymentPrepayAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<B_PaymentPrepayListOutputDto>> GetList(GetB_PaymentPrepayListInput input);


        Task<PagedResultDto<B_PaymentPrepayListForWxOutputDto>> GetListForWx(GetB_PaymentPrepayListInput input);



        Task<B_AgencyApplyCount> GetCount();

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<B_PaymentPrepayOutputDto> Get(EntityDto<Guid> input);

        /// <summary>
        /// 添加一个B_PaymentPrepay
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Create(CreateB_PaymentPrepayInput input);

        /// <summary>
        /// 修改一个B_PaymentPrepay
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateB_PaymentPrepayInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);



        Task Audit(AuditB_PrePayInput input);
    }
}