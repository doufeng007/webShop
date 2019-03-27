using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace B_H5
{
    public interface IB_CloudWarehouseAppService : IApplicationService
    {
        /// <summary>
        /// 获取云仓库存列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<B_CWInventoryListOutputDto>> GetCWInventoryListAsync(GetB_CWInventoryListInput input);

        /// <summary>
        /// 获取云仓进出明显列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<B_CWInOutDetailListOutputDto>> GetCWInOutDetailListAsync(GetB_CWInOutDetailListInput input);





    }
}
