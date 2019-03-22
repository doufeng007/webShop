using System;
using Abp.UI;
using Abp.Net.Mail.Smtp;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Model;
using System.Net.Mail;
using System.Text;

namespace EmailServer
{
    public class EmailAppService : FRMSCoreAppServiceBase, IEmailAppService
    {
        private readonly ISmtpEmailSenderConfiguration _smtpEmialSenderConfig;

        public EmailAppService(ISmtpEmailSenderConfiguration smtpEmialSenderConfigtion)
        {
            _smtpEmialSenderConfig = smtpEmialSenderConfigtion;
        }

        /// <summary>
        /// 发送信件
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public bool SendEMail(SendEmailInput input)
        {
            try
            {
                SmtpEmailSender emailSender = new SmtpEmailSender(_smtpEmialSenderConfig);
                emailSender.Send(input.From,input.To, input.Subject, input.Body,input.IsBodyHtml);
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException((int)ErrorCode.HttpPortErr, "发信失败：" + e.Message);
            }
        }


        /// <summary>
        /// 发送信件
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public void SendEMailMessage(SendEmailInput input)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress(input.From, "真有意思", Encoding.UTF8);
                //抄送
                //mail.CC.Add(new MailAddress(input.From, "真有意思", System.Text.Encoding.UTF8));
                mail.To.Add(new MailAddress(input.To, "接收者g", Encoding.UTF8)); ;
                mail.Subject = input.Subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;
                mail.Body = input.Body;
                mail.IsBodyHtml = true;
                mail.Attachments.Add(new Attachment(@"F:\im.pdm"));

                 SmtpEmailSender emailSender = new SmtpEmailSender(_smtpEmialSenderConfig);
                emailSender.Send(mail);  
            }
            catch (Exception e)
            {
                throw new UserFriendlyException((int)ErrorCode.HttpPortErr, "发信失败：" + e.Message);
            }
        }
        /// <summary>
        /// 发送信件
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public void SendMailMessage(MailMessage input)
        {
            try
            {
                SmtpEmailSender emailSender = new SmtpEmailSender(_smtpEmialSenderConfig);
                emailSender.Send(input);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException((int)ErrorCode.HttpPortErr, "发信失败：" + e.Message);
            }
        }
    }
}