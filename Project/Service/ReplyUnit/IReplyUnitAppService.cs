using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project
{
    public interface IReplyUnitAppService : IApplicationService
    {
        Task CreateOrUpdate(ReplyUnitDto input);

        Task<GetReplyUnitForEditOutput> GetForEdit(NullableIdDto<int> input);

        Task<PagedResultDto<ReplyUnitList>> GetPage(GetReplyUnitListInput input);

        Task Delete(EntityDto<int> input);

        List<ReplyUnitList> GetAll();

        string GetName(EntityDto id);
    }

}
