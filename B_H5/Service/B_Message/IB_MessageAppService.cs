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
    /// 代理消息接口
    /// </summary>
    public interface IB_MessageAppService : IApplicationService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<B_MessageListOutputDto>> GetList(GetB_MessageListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<B_MessageOutputDto> Get(EntityDto<Guid> input);

        /// <summary>
        /// 添加一个B_Message
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task CreateAsync(CreateB_MessageInput input);


        void Create(CreateB_MessageInput input);

        /// <summary>
        /// 修改一个B_Message
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateB_MessageInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);
    }
}