using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace B_H5
{
    /// <summary>
    /// 云仓
    /// </summary>
    public class B_CloudWarehouseAppService : FRMSCoreAppServiceBase, IB_CloudWarehouseAppService
    {
        /// <summary>
        ///  获取云仓进出明显列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<B_CWInOutDetailListOutputDto>> GetCWInOutDetailListAsync(GetB_CWInOutDetailListInput input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取云仓库存列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<B_CWInventoryListOutputDto>> GetCWInventoryListAsync(GetB_CWInventoryListInput input)
        {
            throw new NotImplementedException();
        }
    }
}
