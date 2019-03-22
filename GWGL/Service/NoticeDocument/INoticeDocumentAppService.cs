using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWGL
{
    public interface INoticeDocumentAppService : IApplicationService
    {
        Task<GetNoticeDocumentForEditOutput> GetEdit(GetWorkFlowTaskCommentInput input);

        Task<InitWorkFlowOutput> Create(CreateNoticeDocumentInput input);

        Task Update(CreateNoticeDocumentInput input);


        Task<GetNoticeDocumentForEditOutput> GetByRegistrationIdEdit(GetWorkFlowTaskCommentInput input);


        Task<PagedResultDto<NoticeDocumentListOutput>> GetNoticeDocuments(GetNoticeDocumentListInput input);


        bool IsNeedAddWrite(Guid id);


        Task UpdateSupplierPrint(UpdateSupplierPrintInput input);


        void MarkCheckUser(string instanceId);
        //void AudtAddWrite(string instanceId, Guid taskId);


    }

}
