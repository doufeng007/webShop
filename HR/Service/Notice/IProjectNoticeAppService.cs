using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace HR
{
    public interface IProjectNoticeAppService : IApplicationService
    {
        //Task CreateNotice(NoticePublishInput input);

        Task SendAsync(NoticeMessageInput input);

        void Send(NoticeMessageInput input);

        Task<PagedResultDto<NoticeList>> GetPage(GetNoticeListInput input);

        Task<ListResultDto<NoticeList>> GetNotReadList(int? type);

        Task<NoticeView> GetForView(NoticeViewInput input);


        Task<PagedResultDto<NoticeList>> GetNoticeTextsAsync(GetNoticeListInput input);

        Task<NoticeView> GetNoticeForEditAsync(Guid? id);

        Task CreateOrUpdateNotice(NoticePublishInput input);


        Task DeleteNoticeAsync(EntityDto<Guid> input);
        Task<NoticeList> GetNewNotice();
        Task<NoticeList> Get(EntityDto<Guid> intput);

    }

}