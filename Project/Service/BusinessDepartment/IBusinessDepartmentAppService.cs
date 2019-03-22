using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project
{
    public interface IBusinessDepartmentAppService : IApplicationService
    {
        Task CreateOrUpdate(BusinessDepartmentDto input);

        Task<GetBusinessDepartmentForEditOutput> GetForEdit(NullableIdDto<int> input);

        Task<PagedResultDto<BusinessDepartmentList>> GetPage(GetBusinessDepartmentListInput input);

        Task Delete(EntityDto<int> input);

        List<BusinessDepartmentList> GetAll();

        string GetName(EntityDto id);
    }

}
