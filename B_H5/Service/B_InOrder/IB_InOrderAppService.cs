﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace B_H5
{
    public interface IB_InOrderAppService : IApplicationService
    {
        /// <summary>
        /// 获取进货单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<B_InOrderListOutputDto>> GetB_InOrderListAsync(GetB_InOrderListInput input);



        Task<List<B_InOrderListOutputDto>> GetList(GetB_InOrderListInput input);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OrderInDto> Get(EntityDto<Guid> input);


        /// <summary>
        /// 我的云仓-进货
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InOrderStatusEnum> OrderIn(OrderInInput input);


    }
}
