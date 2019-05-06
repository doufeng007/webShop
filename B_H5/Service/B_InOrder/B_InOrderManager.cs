//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Abp.Application.Services.Dto;
//using Abp.AutoMapper;
//using Abp.Linq.Extensions;
//using System.Linq.Dynamic;
//using System.Diagnostics;
//using Abp.Domain.Repositories;
//using System.Web;
//using Castle.Core.Internal;
//using Abp.UI;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Dynamic.Core;
//using ZCYX.FRMSCore;
//using Abp.File;
//using Abp.WorkFlow;
//using ZCYX.FRMSCore.Application;
//using ZCYX.FRMSCore.Extensions;
//using ZCYX.FRMSCore.Model;
//using Abp.WorkFlowDictionary;
//using Abp.Domain.Uow;
//using Abp.Application.Services;

//namespace B_H5
//{
//    public class B_InOrderManager : ApplicationService
//    {
//        private readonly IRepository<B_Agency, Guid> _repository;
//        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryrepository;
//        private readonly IUnitOfWorkManager _unitOfWorkManager;
//        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        


//        public B_InOrderManager(IRepository<B_Agency, Guid> repository, IRepository<AbpDictionary, Guid> abpDictionaryrepository
//            , IUnitOfWorkManager unitOfWorkManager, IAbpFileRelationAppService abpFileRelationAppService

//        )
//        {
//            this._repository = repository;
//            _abpDictionaryrepository = abpDictionaryrepository;
//            _unitOfWorkManager = unitOfWorkManager;
//            _abpFileRelationAppService = abpFileRelationAppService;

//        }


//        public void OrderInForCurrentUser()
//        {
//            SendWeChatMessage(orderInmodel.Id.ToString(), TemplateMessageBusinessTypeEnum.当前用户进货订单完成, AbpSession.UserId.Value, $"进货订单{orderInmodel.OrderNo}"
//                        , categroyModel.Name, "货物已转入云仓", totalAmout, InOrderStatusEnum.已完成);

//            await _b_CWDetailAppService.Create(new CreateB_CWDetailInput()
//            {
//                BusinessType = CWDetailBusinessTypeEnum.自身进货入仓,
//                CategroyId = input.CategroyId,
//                Number = input.Number,
//                Type = CWDetailTypeEnum.入仓,
//                UserId = bModel.UserId
//            });
//        }






//        public void SendWeChatMessage(string bid, TemplateMessageBusinessTypeEnum bType, long userId, string title, string categroyName, string remark, decimal amout, InOrderStatusEnum status)
//        {
//            var bModel = _b_AgencyRepository.FirstOrDefault(r => r.UserId == userId);
//            if (bModel == null)
//                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在！");
//            var dic = new Dictionary<string, string>();
//            dic.Add("keyword1", categroyName);
//            dic.Add("keyword2", amout.ToString());
//            dic.Add("keyword3", status.ToString());
//            _wxTemplateMessageManager.SendWeChatMsg(bid, bType, bModel.OpenId, title, dic, remark, "");

//        }


//    }

    
//}