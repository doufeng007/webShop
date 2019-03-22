using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Train
{
    public interface ITrainStatisticsAppService : IApplicationService
    {

        Task<PagedResultDto<TrainScoreOutputDto>> GetList(TrainScoreInput input);
    }
}
