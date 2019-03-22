using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace IMLib
{
    public interface IIM_InquiryAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<List<InquiryOutput>> GetList(GetIM_InquiryListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<InquiryOutput> Get(EntityDto<Guid> input);

        /// <summary>
        /// 添加一个IM_Inquiry
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Create(CreateIM_InquiryInput input);

		
    }
}