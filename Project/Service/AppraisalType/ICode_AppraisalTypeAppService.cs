using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface ICode_AppraisalTypeAppService : IApplicationService
    {

        //ListResultDto<Code_AppraisalTypeListDto> GetCode_AppraisalTypes();
        Task<PagedResultDto<Code_AppraisalTypeListDto>> GetCode_AppraisalTypes(GetCode_AppraisalTypeListInput input);

        ListResultDto<Code_AppraisalTypeListDto> GetAllCode_AppraisalTypes(GetCode_AppraisalTypeListInput input);


        Task<GetCode_AppraisalTypeForEditOutput> GetCode_AppraisalTypeForEdit(NullableIdDto<int> input);


        Task CreateOrUpdateCode_AppraisalType(CreateOrUpdateCode_AppraisalTypeInput input);


        Task DeleteCode_AppraisalType(EntityDto<int> input);


        Task<int> CreateOrUpdate(CreateOrUpdateCode_AppraisalTypeInput input);

    }

}
