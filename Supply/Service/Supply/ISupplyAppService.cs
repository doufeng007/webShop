using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    public interface ISupplyAppService : IApplicationService
    {

        
        Task<SupplyDto> Get(GetWorkFlowTaskCommentInput input);

        Task<PagedResultDto<SupplyListDto>> GetAll(GetSupplyListInput input);


        Task<AbpFileUploadResultModel> ExportExcle();


        Task<InitWorkFlowOutput> Create(SupplyCreateInput input);

        Task CreateList(List<SupplyCreateInput> input);

        Task Update(SupplyUpdateInput input);

        Task Send(SupplySendDto input);

        decimal Worth();


        /// <summary>
        /// 是否需要分管领导审核 若行政人员发起的流程 则需要分管领导审核
        /// </summary>
        /// <param name="flowID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        bool IsNeedFGLDAudit(Guid flowID, Guid groupID);
    }

}
