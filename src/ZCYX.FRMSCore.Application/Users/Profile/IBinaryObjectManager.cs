using System;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Core;

namespace ZCYX.FRMSCore.Application
{
    public interface IBinaryObjectManager
    {
        Task<BinaryObject> GetOrNullAsync(Guid id);
        
        Task SaveAsync(BinaryObject file);
        
        Task DeleteAsync(Guid id);
    }
}