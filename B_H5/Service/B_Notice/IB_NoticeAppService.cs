using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    /// <summary>
    /// 公司消息接口
    /// </summary>
    public interface IB_NoticeAppService : IApplicationService
    {	
	    /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		Task<PagedResultDto<B_NoticeListOutputDto>> GetList(GetB_NoticeListInput input);


        Task<PagedResultDto<B_NoticeListOutputDto>> GetListForWx(GetB_NoticeListInput input);


        Task ReadNotice(EntityDto<Guid> input);


        Task<int> GetListForWxNotRead();




        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<B_NoticeOutputDto> Get(EntityDto<Guid> input);

		/// <summary>
        /// 添加一个B_Notice
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateB_NoticeInput input);

		/// <summary>
        /// 修改一个B_Notice
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateB_NoticeInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);


        Task SendOrCancle(EntityDto<Guid> input);
    }
}