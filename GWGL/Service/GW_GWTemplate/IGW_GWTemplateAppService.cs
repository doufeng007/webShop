﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace GWGL
{
    public interface IGW_GWTemplateAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<GW_GWTemplateListOutputDto>> GetList(GetGW_GWTemplateListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<GW_GWTemplateOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个GW_GWTemplate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateGW_GWTemplateInput input);

		/// <summary>
        /// 修改一个GW_GWTemplate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateGW_GWTemplateInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}