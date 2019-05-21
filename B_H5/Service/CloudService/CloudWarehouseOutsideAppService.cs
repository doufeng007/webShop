using Abp.Application.Services;
using Abp.Reflection.Extensions;
using Abp.UI;
using B_H5.Service.CloudService.Dto;
using Fw.Api;
using Fw.Api.Request;
using Fw.Api.Response;
using Microsoft.Extensions.Configuration;
using ServiceReference;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Model;

namespace B_H5
{
    public class CloudWarehouseOutsideAppService : FRMSCoreAppServiceBase, ICloudWarehouseOutsideAppService
    {
        public APIWebServiceSoapClient SoapClient { get; set; }

        private readonly IConfigurationRoot _appConfiguration;

        private string CloudServiceUrl { get; set; }

        private string CloudServiceAppKey { get; set; }

        private string CloudServiceAppSecret { get; set; }

        private string CloudServicePartnerId { get; set; }



        public string AppId = "80010";

        public string AppKey = "fe6db94a-68d4-47fa-838b-42955529807380010";

        public CloudWarehouseOutsideAppService()
        {
            SoapClient = new ServiceReference.APIWebServiceSoapClient(ServiceReference.APIWebServiceSoapClient.EndpointConfiguration.APIWebServiceSoap);

            var coreAssemblyDirectoryPath = typeof(CloudService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);

            CloudServiceUrl = _appConfiguration["App:CloudServiceUrl"].ToString();
            CloudServiceAppKey = _appConfiguration["App:CloudServiceAppKey"].ToString();
            CloudServiceAppSecret = _appConfiguration["App:CloudServiceAppSecret"].ToString();
            CloudServicePartnerId = _appConfiguration["App:CloudServicePartnerId"].ToString();
        }





        public async Task ProductsSync(WmsProductsSyncRequest.Item input)
        {
            try
            {
                DefaultFwClient client = new DefaultFwClient(CloudServiceUrl, CloudServiceAppKey, CloudServiceAppSecret, CloudServicePartnerId);
                WmsProductsSyncRequest request = new WmsProductsSyncRequest();
                WmsProductsSyncResponse response = new WmsProductsSyncResponse();
                List<WmsProductsSyncRequest.Item> products = new List<WmsProductsSyncRequest.Item>();
                products.Add(input);
                request.Items = products;
                response = client.Execute(request);
                if (!response.IsSuccess)
                {
                    Abp.Logging.LogHelper.Logger.Error($"调用云仓创建产品失败：返回code：{response.ErrCode}, Msg:{response.ErrMsg}");
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建产品失败！");
                }
            }
            catch (Exception ex)
            {

                Abp.Logging.LogHelper.Logger.Error($"调用云仓创建产品失败：异常{ex.Message}");
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建产品失败！");
            }

        }


        /// <summary>
        /// 创建出入库单
        /// </summary>
        /// <returns></returns>
        public async Task PurchaseOutinorderAdd(PurchaseOutinorderAddInput input)
        {
            try
            {
                DefaultFwClient client = new DefaultFwClient(CloudServiceUrl, CloudServiceAppKey, CloudServiceAppSecret, CloudServicePartnerId);
                WmsPurchaseOutinorderAddRequest request = new WmsPurchaseOutinorderAddRequest();
                WmsPurchaseOutinorderAddResponse response = new WmsPurchaseOutinorderAddResponse();
                request.WareHouseCode = input.wareHouseCode;
                request.SyncId = $"RK{DateTime.Now.ToString("yyyyMMddHHmm")}";
                request.ActionType = input.actionType;
                request.Remark = "测试";
                List<WmsPurchaseOutinorderAddRequest.Item> products = new List<WmsPurchaseOutinorderAddRequest.Item>();
                products.Add(new WmsPurchaseOutinorderAddRequest.Item()
                {
                    BarCode = input.barCode,
                    InventoryType = "NORMAL",
                    Quantity = input.quantity,
                    ProductBatch = "",
                    ExpireDate = ""
                });
                request.Items = products;
                response = client.Execute(request);

                if (!response.IsSuccess)
                {
                    Abp.Logging.LogHelper.Logger.Error($"调用云仓创建入库单失败：返回code：{response.ErrCode}, Msg:{response.ErrMsg}");
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建入库单失败！");
                }
            }
            catch (Exception ex)
            {

                Abp.Logging.LogHelper.Logger.Error($"调用云仓创建入库单失败：异常{ex.Message}");
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建入库单失败！");
            }

        }



        /// <summary>
        /// 出入库单确认接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PurchaseOutinorderConfirm(PurchaseOutinorderConfirmInput input)
        {
            var model = input;
            Abp.Logging.LogHelper.Logger.Error($"收到参数：{Newtonsoft.Json.JsonConvert.SerializeObject(input)}");
        }





    }
}
