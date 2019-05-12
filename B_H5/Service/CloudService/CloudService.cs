using Abp.Application.Services;
using Abp.UI;
using B_H5.Service.CloudService.Dto;
using ServiceReference;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Model;

namespace B_H5
{
    [RemoteService(IsEnabled = false)]
    public class CloudService : ApplicationService
    {
        public APIWebServiceSoapClient SoapClient { get; set; }


        public string AppId = "80010";

        public string AppKey = "fe6db94a-68d4-47fa-838b-42955529807380010";

        public CloudService()
        {
            SoapClient = new ServiceReference.APIWebServiceSoapClient(ServiceReference.APIWebServiceSoapClient.EndpointConfiguration.APIWebServiceSoap);
        }


        /// <summary>
        /// 创建产品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateGoods(CreateGoodsInput input)
        {
            var parameter = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            var ret = await SoapClient.Create_GFF_GoodsAsync(parameter, AppId, AppKey);
            var retObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ReasponseBaseDto>(ret.Body.Create_GFF_GoodsResult);
            if (retObj.flag != "success")
            {
                Abp.Logging.LogHelper.Logger.Error($"调用云仓创建产品失败：参数{parameter},返回结果:{retObj}");
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建产品失败！");
            }
        }



        public async Task InsertOrUpdateOrder()
        {
            //string strorderinfo = "Style:2;GFF_CustomerID:80000;GFF_ReceiveSendAddressID:;ConsigneeName:Ameerahmed;Country:84;Base_ChannelInfoID:CNGZGH;State:Paris-;City:Paris;OrderStatus:1;Address1:144 rue de rennes, 5eme etage – CODE 6335A – NOM –COTTIN AMEER;Address2:;CsRefNo:D4N3YZMJ69LI;Zipcode:75006;Contact:+33643052323;CusRemark:;TrackingNo:;";
            //string strorderproduct = "MaterialRefNo:VB40021,MaterialQuantity:1,Price:27.5,Weight:0.2,EnName:Handheld Massager,WarehouseID:302,ProducingArea:112,CnName:,;MaterialRefNo:VB40021,MaterialQuantity:1,Price:27.5,Weight:0.2,EnName:Handheld Massager,WarehouseID:302,ProducingArea:112,CnName:,;";
            //string stradd = "";

            //var result = await SoapClient.InsertUpdateOrderAsync(strorderinfo, strorderproduct, stradd, AppKey);
            //var retObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ReasponseBaseDto>(result.Body.InsertUpdateOrderResult);
            //if (retObj.flag != "success")
            //{
            //    Abp.Logging.LogHelper.Logger.Error($"调用云仓创建产品失败：参数{parameter},返回结果:{retObj}");
            //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建产品失败！");
            //}

        }
    }
}
