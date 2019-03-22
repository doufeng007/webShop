using Abp.Application.Services;
using System.Net.Mail;

namespace EmailServer
{
    public interface IEmailAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件发送信件
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        bool SendEMail(SendEmailInput input);
        void SendMailMessage(MailMessage input);
    }
}