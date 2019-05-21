using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using B_H5.Service.CloudService.Dto;
using Fw.Api.Request;

namespace B_H5
{
    public interface ICloudWarehouseOutsideAppService : IApplicationService
    {



        Task ProductsSync(WmsProductsSyncRequest.Item input);



        Task PurchaseOutinorderAdd(PurchaseOutinorderAddInput input);



        Task PurchaseOutinorderConfirm(PurchaseOutinorderConfirmInput input);
    }
}