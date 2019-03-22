using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project
{
    public interface IChargeOrganizationsAppService : IApplicationService
    {
        Task CreateOrUpdate(ChargeOrganizationsDto input);

        Task<GetChargeOrganizationsForEditOutput> GetForEdit(NullableIdDto<int> input);

        Task<PagedResultDto<ChargeOrganizationsList>> GetPage(GetChargeOrganizationsListInput input);

        Task Delete(EntityDto<int> input);

        List<ChargeOrganizationsList> GetAll();

        string GetName(EntityDto input);


        string COGetPy(string name);
    }

}
