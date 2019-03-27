using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace B_H5
{
    /// <summary>
    /// 进货
    /// </summary>
    public class B_InOrderAppService : FRMSCoreAppServiceBase, IB_InOrderAppService
    {
        /// <summary>
        /// 获取进货单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<B_InOrderListOutputDto>> GetB_InOrderListAsync(GetB_InOrderListInput input)
        {
            throw new NotImplementedException();
        }
    }
}
