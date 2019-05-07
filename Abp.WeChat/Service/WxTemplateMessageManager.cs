using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.WeChat.Entity;
using Abp.WeChat.Enum;

namespace Abp.WeChat
{
    public class WxTemplateMessageManager : ApplicationService
    {
        private static string[] fifterkey = { "ID", "TemplateID", "TemplateName" };



        [UnitOfWork(IsDisabled = true)]
        public void SendWeChatMsg(string businessId, TemplateMessageBusinessTypeEnum bType, string wxOpenId,
            string title, Dictionary<string, string> keyword, string remark)
        {
            return;
            var entity = new AbpWxMessage();
            entity.Id = Guid.NewGuid();
            entity.BusinessId = businessId;
            entity.BType = bType;
            entity.Status = (int)Enum_ProjectCheckMsgStatus.NoDo;
            entity.SendStatus = (int)Enum_WxMsgStatus.WaitReceive;
            entity.Title = title;
            //entity.LinkUrl = "http://cp.bubaocloud.com/WXindex/ProjectInfo?projectId=" + project.Id;
            entity.Content = $"title:{title}, data:{Json.JsonSerializationHelper.SerializeWithType(keyword)}";
            entity.Sort = 0;
            entity.ReceiveTime = null;
            entity.ReceiveOpenId = wxOpenId;

            var _messageRepository =
                      Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                          .IocManager.IocContainer.Resolve<IRepository<AbpWxMessage, Guid>>();
            _messageRepository.Insert(entity);
            CurrentUnitOfWork.SaveChanges();


            TemplateMessageResult resultInfo;
            var data = MessageTemplate.NoticeDealer(title, keyword, remark);
            var weChatService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                          .IocManager.IocContainer.Resolve<IWeChatAppService>();
            var templateId = weChatService.GetTemplateIdByType(bType);
            var result = SendTemplateMessageManager.SendTemplateMessage(templateId, wxOpenId, "", data, out resultInfo);
            entity.MsgID = resultInfo.msgid;
            entity.ReceiveTime = DateTime.Now;
            entity.ReceiveOpenId = wxOpenId;
            if (result)
            {
                if (resultInfo.errcode == ReturnCode.请求成功)
                {
                    //修改数据
                    entity.SendStatus = (int)Enum_WxMsgStatus.Success;
                }
                else
                {
                    entity.SendStatus = (int)Enum_WxMsgStatus.SystemFailed;

                }

            }
            else
            {
                entity.SendStatus = (int)Enum_WxMsgStatus.SystemFailed;
            }
            entity.SendError = $"{resultInfo.errcode}：{resultInfo.errmsg}";
            try
            {

                var old_model = _messageRepository.Get(entity.Id);
                old_model.MsgID = entity.MsgID;
                old_model.ReceiveTime = DateTime.Now;
                old_model.ReceiveOpenId = entity.ReceiveOpenId;
                old_model.SendStatus = entity.SendStatus;
                old_model.SendError = entity.SendError;
                _messageRepository.Update(old_model);
            }
            catch (Exception e)
            {

                throw;
            }



        }
    }
}
