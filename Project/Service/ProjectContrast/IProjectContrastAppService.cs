using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectContrastAppService : IApplicationService
    {
        //   Task<GetProjectAuditStopForEidtOutput> GetProjectStop(NullableIdDto<Guid> input);


        GetProjectContrastOutput GetProjectContrastResult();


        List<ProjectContrastResultData> GetProjectContrastResultData(EntityDto<string> intput);

        Task<ProjectCjzCompareData> GetProjectContrastResultDataV2(GetProjectContrastInput intput);


        Task<ProjectCjzData> GetProjectContrastResultDataFromDB(GetProjectContrastInput input);


        Task UpdateCompareResultRemark(UpdateCompareResultRemarkInput input);

        Task InsertIntoMongoDB(InsertIntoMongoDBInput input);

        Task<List<FileUploadFiles>> GetProjectFileForCompare(GetProjectFileForCompareInput intput);

    }

}
