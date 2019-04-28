//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace CloudApiService
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CloudApiService.APIWebServiceSoap")]
    public interface APIWebServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLablesUrl", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetLablesUrlResponse> GetLablesUrlAsync(CloudApiService.GetLablesUrlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetUSPSLabel", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetUSPSLabelResponse> GetUSPSLabelAsync(CloudApiService.GetUSPSLabelRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLablesMerge", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetLablesMergeResponse> GetLablesMergeAsync(CloudApiService.GetLablesMergeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BindTrackingDetail", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.BindTrackingDetailResponse> BindTrackingDetailAsync(CloudApiService.BindTrackingDetailRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PintPDF", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.PintPDFResponse> PintPDFAsync(CloudApiService.PintPDFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/get_pinttotal", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.get_pinttotalResponse> get_pinttotalAsync(CloudApiService.get_pinttotalRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLineDate", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetLineDateResponse> GetLineDateAsync(CloudApiService.GetLineDateRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.HelloWorldResponse> HelloWorldAsync(CloudApiService.HelloWorldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Get_Pda_Sql", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.Get_Pda_SqlResponse> Get_Pda_SqlAsync(CloudApiService.Get_Pda_SqlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getCountry", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getCountryResponse> getCountryAsync(CloudApiService.getCountryRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getChanelFeeType", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getChanelFeeTypeResponse> getChanelFeeTypeAsync(CloudApiService.getChanelFeeTypeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getWarehouse", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getWarehouseResponse> getWarehouseAsync(CloudApiService.getWarehouseRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getChannel", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getChannelResponse> getChannelAsync(CloudApiService.getChannelRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getStockChannel", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getStockChannelResponse> getStockChannelAsync(CloudApiService.getStockChannelRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/webapp", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.webappResponse> webappAsync(CloudApiService.webappRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getFeeByCWV_2", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getFeeByCWV_2Response> getFeeByCWV_2Async(CloudApiService.getFeeByCWV_2Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getFeeByCWV", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getFeeByCWVResponse> getFeeByCWVAsync(CloudApiService.getFeeByCWVRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebaySessionId", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getebaySessionIdResponse> getebaySessionIdAsync(CloudApiService.getebaySessionIdRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebayToken", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getebayTokenResponse> getebayTokenAsync(CloudApiService.getebayTokenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebayOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getebayOrderResponse> getebayOrderAsync(CloudApiService.getebayOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebaymessage", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getebaymessageResponse> getebaymessageAsync(CloudApiService.getebaymessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getFeeByCWVSN", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getFeeByCWVSNResponse> getFeeByCWVSNAsync(CloudApiService.getFeeByCWVSNRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getPackage", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getPackageResponse> getPackageAsync(CloudApiService.getPackageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getOrder_Track", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getOrder_TrackResponse> getOrder_TrackAsync(CloudApiService.getOrder_TrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetOrderTrack", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetOrderTrackResponse> GetOrderTrackAsync(CloudApiService.GetOrderTrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/get_Track", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.get_TrackResponse> get_TrackAsync(CloudApiService.get_TrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ComMon_Tracking", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.ComMon_TrackingResponse> ComMon_TrackingAsync(CloudApiService.ComMon_TrackingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getStock", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getStockResponse> getStockAsync(CloudApiService.getStockRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GoodsListInfo", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GoodsListInfoResponse> GoodsListInfoAsync(CloudApiService.GoodsListInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SetEbayTrack", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.SetEbayTrackResponse> SetEbayTrackAsync(CloudApiService.SetEbayTrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ChannelInfo", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.ChannelInfoResponse> ChannelInfoAsync(CloudApiService.ChannelInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ChannelInfo_sub", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.ChannelInfo_subResponse> ChannelInfo_subAsync(CloudApiService.ChannelInfo_subRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ChannelInfo_id", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.ChannelInfo_idResponse> ChannelInfo_idAsync(CloudApiService.ChannelInfo_idRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetWarehouse", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetWarehouseResponse1> GetWarehouse1Async(CloudApiService.GetWarehouseRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetASN_NO", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetASN_NOResponse> GetASN_NOAsync(CloudApiService.GetASN_NORequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetOrderID", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetOrderIDResponse> GetOrderIDAsync(CloudApiService.GetOrderIDRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UpdateISPrint", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.UpdateISPrintResponse> UpdateISPrintAsync(CloudApiService.UpdateISPrintRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Select_SKU_Count", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.Select_SKU_CountResponse> Select_SKU_CountAsync(CloudApiService.Select_SKU_CountRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InsertUpdateOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.InsertUpdateOrderResponse> InsertUpdateOrderAsync(CloudApiService.InsertUpdateOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CancelOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.CancelOrderResponse> CancelOrderAsync(CloudApiService.CancelOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DeleteOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.DeleteOrderResponse> DeleteOrderAsync(CloudApiService.DeleteOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Create_GFF_Goods", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.Create_GFF_GoodsResponse> Create_GFF_GoodsAsync(CloudApiService.Create_GFF_GoodsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Create_ASNMain", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.Create_ASNMainResponse> Create_ASNMainAsync(CloudApiService.Create_ASNMainRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Create_RejectOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.Create_RejectOrderResponse> Create_RejectOrderAsync(CloudApiService.Create_RejectOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetASNMainList", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetASNMainListResponse> GetASNMainListAsync(CloudApiService.GetASNMainListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PrintPDF", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.PrintPDFResponse> PrintPDFAsync(CloudApiService.PrintPDFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetService", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.GetServiceResponse> GetServiceAsync(CloudApiService.GetServiceRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getdata", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getdataResponse> getdataAsync(CloudApiService.getdataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getMagentoOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getMagentoOrderResponse> getMagentoOrderAsync(CloudApiService.getMagentoOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SetMagentoTrackingNo", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.SetMagentoTrackingNoResponse> SetMagentoTrackingNoAsync(CloudApiService.SetMagentoTrackingNoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getBing", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getBingResponse> getBingAsync(CloudApiService.getBingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getOrderResponse> getOrderAsync(CloudApiService.getOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getOrder1", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getOrder1Response> getOrder1Async(CloudApiService.getOrder1Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Get_ALLData", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.Get_ALLDataResponse> Get_ALLDataAsync(CloudApiService.Get_ALLDataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Get_ALLBing", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.Get_ALLBingResponse> Get_ALLBingAsync(CloudApiService.Get_ALLBingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PintEUB", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.PintEUBResponse> PintEUBAsync(CloudApiService.PintEUBRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getday", ReplyAction="*")]
        System.Threading.Tasks.Task<CloudApiService.getdayResponse> getdayAsync(CloudApiService.getdayRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLablesUrlRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLablesUrl", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetLablesUrlRequestBody Body;
        
        public GetLablesUrlRequest()
        {
        }
        
        public GetLablesUrlRequest(CloudApiService.GetLablesUrlRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetLablesUrlRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string OrderNo;
        
        public GetLablesUrlRequestBody()
        {
        }
        
        public GetLablesUrlRequestBody(string OrderNo)
        {
            this.OrderNo = OrderNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLablesUrlResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLablesUrlResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetLablesUrlResponseBody Body;
        
        public GetLablesUrlResponse()
        {
        }
        
        public GetLablesUrlResponse(CloudApiService.GetLablesUrlResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetLablesUrlResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetLablesUrlResult;
        
        public GetLablesUrlResponseBody()
        {
        }
        
        public GetLablesUrlResponseBody(string GetLablesUrlResult)
        {
            this.GetLablesUrlResult = GetLablesUrlResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUSPSLabelRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUSPSLabel", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetUSPSLabelRequestBody Body;
        
        public GetUSPSLabelRequest()
        {
        }
        
        public GetUSPSLabelRequest(CloudApiService.GetUSPSLabelRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetUSPSLabelRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string TrackingNo;
        
        public GetUSPSLabelRequestBody()
        {
        }
        
        public GetUSPSLabelRequestBody(string TrackingNo)
        {
            this.TrackingNo = TrackingNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetUSPSLabelResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetUSPSLabelResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetUSPSLabelResponseBody Body;
        
        public GetUSPSLabelResponse()
        {
        }
        
        public GetUSPSLabelResponse(CloudApiService.GetUSPSLabelResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetUSPSLabelResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetUSPSLabelResult;
        
        public GetUSPSLabelResponseBody()
        {
        }
        
        public GetUSPSLabelResponseBody(string GetUSPSLabelResult)
        {
            this.GetUSPSLabelResult = GetUSPSLabelResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLablesMergeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLablesMerge", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetLablesMergeRequestBody Body;
        
        public GetLablesMergeRequest()
        {
        }
        
        public GetLablesMergeRequest(CloudApiService.GetLablesMergeRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetLablesMergeRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string OrderNo;
        
        public GetLablesMergeRequestBody()
        {
        }
        
        public GetLablesMergeRequestBody(string OrderNo)
        {
            this.OrderNo = OrderNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLablesMergeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLablesMergeResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetLablesMergeResponseBody Body;
        
        public GetLablesMergeResponse()
        {
        }
        
        public GetLablesMergeResponse(CloudApiService.GetLablesMergeResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetLablesMergeResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetLablesMergeResult;
        
        public GetLablesMergeResponseBody()
        {
        }
        
        public GetLablesMergeResponseBody(string GetLablesMergeResult)
        {
            this.GetLablesMergeResult = GetLablesMergeResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class BindTrackingDetailRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="BindTrackingDetail", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.BindTrackingDetailRequestBody Body;
        
        public BindTrackingDetailRequest()
        {
        }
        
        public BindTrackingDetailRequest(CloudApiService.BindTrackingDetailRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class BindTrackingDetailRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string OrderNo;
        
        public BindTrackingDetailRequestBody()
        {
        }
        
        public BindTrackingDetailRequestBody(string OrderNo)
        {
            this.OrderNo = OrderNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class BindTrackingDetailResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="BindTrackingDetailResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.BindTrackingDetailResponseBody Body;
        
        public BindTrackingDetailResponse()
        {
        }
        
        public BindTrackingDetailResponse(CloudApiService.BindTrackingDetailResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class BindTrackingDetailResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string BindTrackingDetailResult;
        
        public BindTrackingDetailResponseBody()
        {
        }
        
        public BindTrackingDetailResponseBody(string BindTrackingDetailResult)
        {
            this.BindTrackingDetailResult = BindTrackingDetailResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PintPDFRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PintPDF", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.PintPDFRequestBody Body;
        
        public PintPDFRequest()
        {
        }
        
        public PintPDFRequest(CloudApiService.PintPDFRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PintPDFRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string order;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string printtype;
        
        public PintPDFRequestBody()
        {
        }
        
        public PintPDFRequestBody(string order, string printtype)
        {
            this.order = order;
            this.printtype = printtype;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PintPDFResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PintPDFResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.PintPDFResponseBody Body;
        
        public PintPDFResponse()
        {
        }
        
        public PintPDFResponse(CloudApiService.PintPDFResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PintPDFResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PintPDFResult;
        
        public PintPDFResponseBody()
        {
        }
        
        public PintPDFResponseBody(string PintPDFResult)
        {
            this.PintPDFResult = PintPDFResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class get_pinttotalRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="get_pinttotal", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.get_pinttotalRequestBody Body;
        
        public get_pinttotalRequest()
        {
        }
        
        public get_pinttotalRequest(CloudApiService.get_pinttotalRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class get_pinttotalRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Start;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string End;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Company;
        
        public get_pinttotalRequestBody()
        {
        }
        
        public get_pinttotalRequestBody(string Start, string End, string Company)
        {
            this.Start = Start;
            this.End = End;
            this.Company = Company;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class get_pinttotalResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="get_pinttotalResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.get_pinttotalResponseBody Body;
        
        public get_pinttotalResponse()
        {
        }
        
        public get_pinttotalResponse(CloudApiService.get_pinttotalResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class get_pinttotalResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string get_pinttotalResult;
        
        public get_pinttotalResponseBody()
        {
        }
        
        public get_pinttotalResponseBody(string get_pinttotalResult)
        {
            this.get_pinttotalResult = get_pinttotalResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLineDateRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLineDate", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetLineDateRequestBody Body;
        
        public GetLineDateRequest()
        {
        }
        
        public GetLineDateRequest(CloudApiService.GetLineDateRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetLineDateRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string startTime;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string endTime;
        
        public GetLineDateRequestBody()
        {
        }
        
        public GetLineDateRequestBody(string startTime, string endTime)
        {
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLineDateResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLineDateResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetLineDateResponseBody Body;
        
        public GetLineDateResponse()
        {
        }
        
        public GetLineDateResponse(CloudApiService.GetLineDateResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetLineDateResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetLineDateResult;
        
        public GetLineDateResponseBody()
        {
        }
        
        public GetLineDateResponseBody(string GetLineDateResult)
        {
            this.GetLineDateResult = GetLineDateResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.HelloWorldRequestBody Body;
        
        public HelloWorldRequest()
        {
        }
        
        public HelloWorldRequest(CloudApiService.HelloWorldRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody
    {
        
        public HelloWorldRequestBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.HelloWorldResponseBody Body;
        
        public HelloWorldResponse()
        {
        }
        
        public HelloWorldResponse(CloudApiService.HelloWorldResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloWorldResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody()
        {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult)
        {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_Pda_SqlRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_Pda_Sql", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Get_Pda_SqlRequestBody Body;
        
        public Get_Pda_SqlRequest()
        {
        }
        
        public Get_Pda_SqlRequest(CloudApiService.Get_Pda_SqlRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Get_Pda_SqlRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ip;
        
        public Get_Pda_SqlRequestBody()
        {
        }
        
        public Get_Pda_SqlRequestBody(string ip)
        {
            this.ip = ip;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_Pda_SqlResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_Pda_SqlResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Get_Pda_SqlResponseBody Body;
        
        public Get_Pda_SqlResponse()
        {
        }
        
        public Get_Pda_SqlResponse(CloudApiService.Get_Pda_SqlResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Get_Pda_SqlResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Get_Pda_SqlResult;
        
        public Get_Pda_SqlResponseBody()
        {
        }
        
        public Get_Pda_SqlResponseBody(string Get_Pda_SqlResult)
        {
            this.Get_Pda_SqlResult = Get_Pda_SqlResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getCountryRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getCountry", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getCountryRequestBody Body;
        
        public getCountryRequest()
        {
        }
        
        public getCountryRequest(CloudApiService.getCountryRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getCountryRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string secretkey;
        
        public getCountryRequestBody()
        {
        }
        
        public getCountryRequestBody(string secretkey)
        {
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getCountryResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getCountryResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getCountryResponseBody Body;
        
        public getCountryResponse()
        {
        }
        
        public getCountryResponse(CloudApiService.getCountryResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getCountryResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getCountryResult;
        
        public getCountryResponseBody()
        {
        }
        
        public getCountryResponseBody(string getCountryResult)
        {
            this.getCountryResult = getCountryResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getChanelFeeTypeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getChanelFeeType", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getChanelFeeTypeRequestBody Body;
        
        public getChanelFeeTypeRequest()
        {
        }
        
        public getChanelFeeTypeRequest(CloudApiService.getChanelFeeTypeRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getChanelFeeTypeRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string chanelID;
        
        public getChanelFeeTypeRequestBody()
        {
        }
        
        public getChanelFeeTypeRequestBody(string chanelID)
        {
            this.chanelID = chanelID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getChanelFeeTypeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getChanelFeeTypeResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getChanelFeeTypeResponseBody Body;
        
        public getChanelFeeTypeResponse()
        {
        }
        
        public getChanelFeeTypeResponse(CloudApiService.getChanelFeeTypeResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getChanelFeeTypeResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getChanelFeeTypeResult;
        
        public getChanelFeeTypeResponseBody()
        {
        }
        
        public getChanelFeeTypeResponseBody(string getChanelFeeTypeResult)
        {
            this.getChanelFeeTypeResult = getChanelFeeTypeResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getWarehouseRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getWarehouse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getWarehouseRequestBody Body;
        
        public getWarehouseRequest()
        {
        }
        
        public getWarehouseRequest(CloudApiService.getWarehouseRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getWarehouseRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string secretkey;
        
        public getWarehouseRequestBody()
        {
        }
        
        public getWarehouseRequestBody(string secretkey)
        {
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getWarehouseResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getWarehouseResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getWarehouseResponseBody Body;
        
        public getWarehouseResponse()
        {
        }
        
        public getWarehouseResponse(CloudApiService.getWarehouseResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getWarehouseResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getWarehouseResult;
        
        public getWarehouseResponseBody()
        {
        }
        
        public getWarehouseResponseBody(string getWarehouseResult)
        {
            this.getWarehouseResult = getWarehouseResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getChannelRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getChannel", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getChannelRequestBody Body;
        
        public getChannelRequest()
        {
        }
        
        public getChannelRequest(CloudApiService.getChannelRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getChannelRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string secretkey;
        
        public getChannelRequestBody()
        {
        }
        
        public getChannelRequestBody(string secretkey)
        {
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getChannelResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getChannelResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getChannelResponseBody Body;
        
        public getChannelResponse()
        {
        }
        
        public getChannelResponse(CloudApiService.getChannelResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getChannelResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getChannelResult;
        
        public getChannelResponseBody()
        {
        }
        
        public getChannelResponseBody(string getChannelResult)
        {
            this.getChannelResult = getChannelResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getStockChannelRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getStockChannel", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getStockChannelRequestBody Body;
        
        public getStockChannelRequest()
        {
        }
        
        public getStockChannelRequest(CloudApiService.getStockChannelRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getStockChannelRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string secretkey;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string StockID;
        
        public getStockChannelRequestBody()
        {
        }
        
        public getStockChannelRequestBody(string secretkey, string StockID)
        {
            this.secretkey = secretkey;
            this.StockID = StockID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getStockChannelResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getStockChannelResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getStockChannelResponseBody Body;
        
        public getStockChannelResponse()
        {
        }
        
        public getStockChannelResponse(CloudApiService.getStockChannelResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getStockChannelResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getStockChannelResult;
        
        public getStockChannelResponseBody()
        {
        }
        
        public getStockChannelResponseBody(string getStockChannelResult)
        {
            this.getStockChannelResult = getStockChannelResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class webappRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="webapp", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.webappRequestBody Body;
        
        public webappRequest()
        {
        }
        
        public webappRequest(CloudApiService.webappRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class webappRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string country;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string weight;
        
        public webappRequestBody()
        {
        }
        
        public webappRequestBody(string country, string weight)
        {
            this.country = country;
            this.weight = weight;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class webappResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="webappResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.webappResponseBody Body;
        
        public webappResponse()
        {
        }
        
        public webappResponse(CloudApiService.webappResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class webappResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string webappResult;
        
        public webappResponseBody()
        {
        }
        
        public webappResponseBody(string webappResult)
        {
            this.webappResult = webappResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getFeeByCWV_2Request
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getFeeByCWV_2", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getFeeByCWV_2RequestBody Body;
        
        public getFeeByCWV_2Request()
        {
        }
        
        public getFeeByCWV_2Request(CloudApiService.getFeeByCWV_2RequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getFeeByCWV_2RequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string FromCountry;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ToCountry;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string weight;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string volume;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string customerid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string secretkey;
        
        public getFeeByCWV_2RequestBody()
        {
        }
        
        public getFeeByCWV_2RequestBody(string FromCountry, string ToCountry, string weight, string volume, string customerid, string secretkey)
        {
            this.FromCountry = FromCountry;
            this.ToCountry = ToCountry;
            this.weight = weight;
            this.volume = volume;
            this.customerid = customerid;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getFeeByCWV_2Response
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getFeeByCWV_2Response", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getFeeByCWV_2ResponseBody Body;
        
        public getFeeByCWV_2Response()
        {
        }
        
        public getFeeByCWV_2Response(CloudApiService.getFeeByCWV_2ResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getFeeByCWV_2ResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getFeeByCWV_2Result;
        
        public getFeeByCWV_2ResponseBody()
        {
        }
        
        public getFeeByCWV_2ResponseBody(string getFeeByCWV_2Result)
        {
            this.getFeeByCWV_2Result = getFeeByCWV_2Result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getFeeByCWVRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getFeeByCWV", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getFeeByCWVRequestBody Body;
        
        public getFeeByCWVRequest()
        {
        }
        
        public getFeeByCWVRequest(CloudApiService.getFeeByCWVRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getFeeByCWVRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string country;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string weight;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string volume;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string customerid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string secretkey;
        
        public getFeeByCWVRequestBody()
        {
        }
        
        public getFeeByCWVRequestBody(string country, string weight, string volume, string customerid, string secretkey)
        {
            this.country = country;
            this.weight = weight;
            this.volume = volume;
            this.customerid = customerid;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getFeeByCWVResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getFeeByCWVResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getFeeByCWVResponseBody Body;
        
        public getFeeByCWVResponse()
        {
        }
        
        public getFeeByCWVResponse(CloudApiService.getFeeByCWVResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getFeeByCWVResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getFeeByCWVResult;
        
        public getFeeByCWVResponseBody()
        {
        }
        
        public getFeeByCWVResponseBody(string getFeeByCWVResult)
        {
            this.getFeeByCWVResult = getFeeByCWVResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebaySessionIdRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebaySessionId", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebaySessionIdRequestBody Body;
        
        public getebaySessionIdRequest()
        {
        }
        
        public getebaySessionIdRequest(CloudApiService.getebaySessionIdRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class getebaySessionIdRequestBody
    {
        
        public getebaySessionIdRequestBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebaySessionIdResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebaySessionIdResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebaySessionIdResponseBody Body;
        
        public getebaySessionIdResponse()
        {
        }
        
        public getebaySessionIdResponse(CloudApiService.getebaySessionIdResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getebaySessionIdResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getebaySessionIdResult;
        
        public getebaySessionIdResponseBody()
        {
        }
        
        public getebaySessionIdResponseBody(string getebaySessionIdResult)
        {
            this.getebaySessionIdResult = getebaySessionIdResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebayTokenRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebayToken", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebayTokenRequestBody Body;
        
        public getebayTokenRequest()
        {
        }
        
        public getebayTokenRequest(CloudApiService.getebayTokenRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class getebayTokenRequestBody
    {
        
        public getebayTokenRequestBody()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebayTokenResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebayTokenResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebayTokenResponseBody Body;
        
        public getebayTokenResponse()
        {
        }
        
        public getebayTokenResponse(CloudApiService.getebayTokenResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getebayTokenResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getebayTokenResult;
        
        public getebayTokenResponseBody()
        {
        }
        
        public getebayTokenResponseBody(string getebayTokenResult)
        {
            this.getebayTokenResult = getebayTokenResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebayOrderRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebayOrder", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebayOrderRequestBody Body;
        
        public getebayOrderRequest()
        {
        }
        
        public getebayOrderRequest(CloudApiService.getebayOrderRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getebayOrderRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Base_APIAccountID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string GFF_CustomerID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string StrDate;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string EndDate;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string Status;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string isGoodsStore;
        
        public getebayOrderRequestBody()
        {
        }
        
        public getebayOrderRequestBody(string Base_APIAccountID, string GFF_CustomerID, string StrDate, string EndDate, string Status, string isGoodsStore)
        {
            this.Base_APIAccountID = Base_APIAccountID;
            this.GFF_CustomerID = GFF_CustomerID;
            this.StrDate = StrDate;
            this.EndDate = EndDate;
            this.Status = Status;
            this.isGoodsStore = isGoodsStore;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebayOrderResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebayOrderResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebayOrderResponseBody Body;
        
        public getebayOrderResponse()
        {
        }
        
        public getebayOrderResponse(CloudApiService.getebayOrderResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getebayOrderResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getebayOrderResult;
        
        public getebayOrderResponseBody()
        {
        }
        
        public getebayOrderResponseBody(string getebayOrderResult)
        {
            this.getebayOrderResult = getebayOrderResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebaymessageRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebaymessage", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebaymessageRequestBody Body;
        
        public getebaymessageRequest()
        {
        }
        
        public getebaymessageRequest(CloudApiService.getebaymessageRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getebaymessageRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string maessage;
        
        public getebaymessageRequestBody()
        {
        }
        
        public getebaymessageRequestBody(string maessage)
        {
            this.maessage = maessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getebaymessageResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getebaymessageResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getebaymessageResponseBody Body;
        
        public getebaymessageResponse()
        {
        }
        
        public getebaymessageResponse(CloudApiService.getebaymessageResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getebaymessageResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getebaymessageResult;
        
        public getebaymessageResponseBody()
        {
        }
        
        public getebaymessageResponseBody(string getebaymessageResult)
        {
            this.getebaymessageResult = getebaymessageResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getFeeByCWVSNRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getFeeByCWVSN", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getFeeByCWVSNRequestBody Body;
        
        public getFeeByCWVSNRequest()
        {
        }
        
        public getFeeByCWVSNRequest(CloudApiService.getFeeByCWVSNRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getFeeByCWVSNRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string shortname;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string weight;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string volume;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string customerid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string secretkey;
        
        public getFeeByCWVSNRequestBody()
        {
        }
        
        public getFeeByCWVSNRequestBody(string shortname, string weight, string volume, string customerid, string secretkey)
        {
            this.shortname = shortname;
            this.weight = weight;
            this.volume = volume;
            this.customerid = customerid;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getFeeByCWVSNResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getFeeByCWVSNResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getFeeByCWVSNResponseBody Body;
        
        public getFeeByCWVSNResponse()
        {
        }
        
        public getFeeByCWVSNResponse(CloudApiService.getFeeByCWVSNResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getFeeByCWVSNResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getFeeByCWVSNResult;
        
        public getFeeByCWVSNResponseBody()
        {
        }
        
        public getFeeByCWVSNResponseBody(string getFeeByCWVSNResult)
        {
            this.getFeeByCWVSNResult = getFeeByCWVSNResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getPackageRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getPackage", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getPackageRequestBody Body;
        
        public getPackageRequest()
        {
        }
        
        public getPackageRequest(CloudApiService.getPackageRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getPackageRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string orderNO;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string customerid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string secretkey;
        
        public getPackageRequestBody()
        {
        }
        
        public getPackageRequestBody(string orderNO, string customerid, string secretkey)
        {
            this.orderNO = orderNO;
            this.customerid = customerid;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getPackageResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getPackageResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getPackageResponseBody Body;
        
        public getPackageResponse()
        {
        }
        
        public getPackageResponse(CloudApiService.getPackageResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getPackageResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getPackageResult;
        
        public getPackageResponseBody()
        {
        }
        
        public getPackageResponseBody(string getPackageResult)
        {
            this.getPackageResult = getPackageResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOrder_TrackRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getOrder_Track", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getOrder_TrackRequestBody Body;
        
        public getOrder_TrackRequest()
        {
        }
        
        public getOrder_TrackRequest(CloudApiService.getOrder_TrackRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getOrder_TrackRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Orderid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string secretkey;
        
        public getOrder_TrackRequestBody()
        {
        }
        
        public getOrder_TrackRequestBody(string Orderid, string secretkey)
        {
            this.Orderid = Orderid;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOrder_TrackResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getOrder_TrackResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getOrder_TrackResponseBody Body;
        
        public getOrder_TrackResponse()
        {
        }
        
        public getOrder_TrackResponse(CloudApiService.getOrder_TrackResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getOrder_TrackResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getOrder_TrackResult;
        
        public getOrder_TrackResponseBody()
        {
        }
        
        public getOrder_TrackResponseBody(string getOrder_TrackResult)
        {
            this.getOrder_TrackResult = getOrder_TrackResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOrderTrackRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOrderTrack", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetOrderTrackRequestBody Body;
        
        public GetOrderTrackRequest()
        {
        }
        
        public GetOrderTrackRequest(CloudApiService.GetOrderTrackRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetOrderTrackRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string OrderNo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string secretkey;
        
        public GetOrderTrackRequestBody()
        {
        }
        
        public GetOrderTrackRequestBody(string OrderNo, string secretkey)
        {
            this.OrderNo = OrderNo;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOrderTrackResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOrderTrackResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetOrderTrackResponseBody Body;
        
        public GetOrderTrackResponse()
        {
        }
        
        public GetOrderTrackResponse(CloudApiService.GetOrderTrackResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetOrderTrackResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetOrderTrackResult;
        
        public GetOrderTrackResponseBody()
        {
        }
        
        public GetOrderTrackResponseBody(string GetOrderTrackResult)
        {
            this.GetOrderTrackResult = GetOrderTrackResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class get_TrackRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="get_Track", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.get_TrackRequestBody Body;
        
        public get_TrackRequest()
        {
        }
        
        public get_TrackRequest(CloudApiService.get_TrackRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class get_TrackRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string OrderNo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Field1;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string TrackingNo;
        
        public get_TrackRequestBody()
        {
        }
        
        public get_TrackRequestBody(string OrderNo, string Field1, string TrackingNo)
        {
            this.OrderNo = OrderNo;
            this.Field1 = Field1;
            this.TrackingNo = TrackingNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class get_TrackResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="get_TrackResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.get_TrackResponseBody Body;
        
        public get_TrackResponse()
        {
        }
        
        public get_TrackResponse(CloudApiService.get_TrackResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class get_TrackResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string get_TrackResult;
        
        public get_TrackResponseBody()
        {
        }
        
        public get_TrackResponseBody(string get_TrackResult)
        {
            this.get_TrackResult = get_TrackResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ComMon_TrackingRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ComMon_Tracking", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ComMon_TrackingRequestBody Body;
        
        public ComMon_TrackingRequest()
        {
        }
        
        public ComMon_TrackingRequest(CloudApiService.ComMon_TrackingRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ComMon_TrackingRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string TrackingNo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ChannelInfoCode;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string key;
        
        public ComMon_TrackingRequestBody()
        {
        }
        
        public ComMon_TrackingRequestBody(string TrackingNo, string ChannelInfoCode, string key)
        {
            this.TrackingNo = TrackingNo;
            this.ChannelInfoCode = ChannelInfoCode;
            this.key = key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ComMon_TrackingResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ComMon_TrackingResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ComMon_TrackingResponseBody Body;
        
        public ComMon_TrackingResponse()
        {
        }
        
        public ComMon_TrackingResponse(CloudApiService.ComMon_TrackingResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ComMon_TrackingResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ComMon_TrackingResult;
        
        public ComMon_TrackingResponseBody()
        {
        }
        
        public ComMon_TrackingResponseBody(string ComMon_TrackingResult)
        {
            this.ComMon_TrackingResult = ComMon_TrackingResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getStockRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getStock", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getStockRequestBody Body;
        
        public getStockRequest()
        {
        }
        
        public getStockRequest(CloudApiService.getStockRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getStockRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string skuorcode;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string customerid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string secretkey;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string WarehouseName;
        
        public getStockRequestBody()
        {
        }
        
        public getStockRequestBody(string skuorcode, string customerid, string secretkey, string WarehouseName)
        {
            this.skuorcode = skuorcode;
            this.customerid = customerid;
            this.secretkey = secretkey;
            this.WarehouseName = WarehouseName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getStockResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getStockResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getStockResponseBody Body;
        
        public getStockResponse()
        {
        }
        
        public getStockResponse(CloudApiService.getStockResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getStockResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getStockResult;
        
        public getStockResponseBody()
        {
        }
        
        public getStockResponseBody(string getStockResult)
        {
            this.getStockResult = getStockResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GoodsListInfoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GoodsListInfo", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GoodsListInfoRequestBody Body;
        
        public GoodsListInfoRequest()
        {
        }
        
        public GoodsListInfoRequest(CloudApiService.GoodsListInfoRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GoodsListInfoRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string customerid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string secretkey;
        
        public GoodsListInfoRequestBody()
        {
        }
        
        public GoodsListInfoRequestBody(string customerid, string secretkey)
        {
            this.customerid = customerid;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GoodsListInfoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GoodsListInfoResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GoodsListInfoResponseBody Body;
        
        public GoodsListInfoResponse()
        {
        }
        
        public GoodsListInfoResponse(CloudApiService.GoodsListInfoResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GoodsListInfoResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GoodsListInfoResult;
        
        public GoodsListInfoResponseBody()
        {
        }
        
        public GoodsListInfoResponseBody(string GoodsListInfoResult)
        {
            this.GoodsListInfoResult = GoodsListInfoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SetEbayTrackRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SetEbayTrack", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.SetEbayTrackRequestBody Body;
        
        public SetEbayTrackRequest()
        {
        }
        
        public SetEbayTrackRequest(CloudApiService.SetEbayTrackRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SetEbayTrackRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string orderid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Base_APIAccountID;
        
        public SetEbayTrackRequestBody()
        {
        }
        
        public SetEbayTrackRequestBody(string orderid, string Base_APIAccountID)
        {
            this.orderid = orderid;
            this.Base_APIAccountID = Base_APIAccountID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SetEbayTrackResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SetEbayTrackResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.SetEbayTrackResponseBody Body;
        
        public SetEbayTrackResponse()
        {
        }
        
        public SetEbayTrackResponse(CloudApiService.SetEbayTrackResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SetEbayTrackResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SetEbayTrackResult;
        
        public SetEbayTrackResponseBody()
        {
        }
        
        public SetEbayTrackResponseBody(string SetEbayTrackResult)
        {
            this.SetEbayTrackResult = SetEbayTrackResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChannelInfoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChannelInfo", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ChannelInfoRequestBody Body;
        
        public ChannelInfoRequest()
        {
        }
        
        public ChannelInfoRequest(CloudApiService.ChannelInfoRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ChannelInfoRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ChannelInfoID;
        
        public ChannelInfoRequestBody()
        {
        }
        
        public ChannelInfoRequestBody(string ChannelInfoID)
        {
            this.ChannelInfoID = ChannelInfoID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChannelInfoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChannelInfoResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ChannelInfoResponseBody Body;
        
        public ChannelInfoResponse()
        {
        }
        
        public ChannelInfoResponse(CloudApiService.ChannelInfoResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ChannelInfoResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ChannelInfoResult;
        
        public ChannelInfoResponseBody()
        {
        }
        
        public ChannelInfoResponseBody(string ChannelInfoResult)
        {
            this.ChannelInfoResult = ChannelInfoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChannelInfo_subRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChannelInfo_sub", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ChannelInfo_subRequestBody Body;
        
        public ChannelInfo_subRequest()
        {
        }
        
        public ChannelInfo_subRequest(CloudApiService.ChannelInfo_subRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ChannelInfo_subRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string subChannelInfoID;
        
        public ChannelInfo_subRequestBody()
        {
        }
        
        public ChannelInfo_subRequestBody(string subChannelInfoID)
        {
            this.subChannelInfoID = subChannelInfoID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChannelInfo_subResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChannelInfo_subResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ChannelInfo_subResponseBody Body;
        
        public ChannelInfo_subResponse()
        {
        }
        
        public ChannelInfo_subResponse(CloudApiService.ChannelInfo_subResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ChannelInfo_subResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ChannelInfo_subResult;
        
        public ChannelInfo_subResponseBody()
        {
        }
        
        public ChannelInfo_subResponseBody(string ChannelInfo_subResult)
        {
            this.ChannelInfo_subResult = ChannelInfo_subResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChannelInfo_idRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChannelInfo_id", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ChannelInfo_idRequestBody Body;
        
        public ChannelInfo_idRequest()
        {
        }
        
        public ChannelInfo_idRequest(CloudApiService.ChannelInfo_idRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ChannelInfo_idRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string subChannelInfoID;
        
        public ChannelInfo_idRequestBody()
        {
        }
        
        public ChannelInfo_idRequestBody(string subChannelInfoID)
        {
            this.subChannelInfoID = subChannelInfoID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ChannelInfo_idResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ChannelInfo_idResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.ChannelInfo_idResponseBody Body;
        
        public ChannelInfo_idResponse()
        {
        }
        
        public ChannelInfo_idResponse(CloudApiService.ChannelInfo_idResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ChannelInfo_idResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ChannelInfo_idResult;
        
        public ChannelInfo_idResponseBody()
        {
        }
        
        public ChannelInfo_idResponseBody(string ChannelInfo_idResult)
        {
            this.ChannelInfo_idResult = ChannelInfo_idResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetWarehouse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetWarehouseRequest1
    {
        
        public GetWarehouseRequest1()
        {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetWarehouseResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetWarehouseResponse1
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string GetWarehouseResult;
        
        public GetWarehouseResponse1()
        {
        }
        
        public GetWarehouseResponse1(string GetWarehouseResult)
        {
            this.GetWarehouseResult = GetWarehouseResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetASN_NORequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetASN_NO", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetASN_NORequestBody Body;
        
        public GetASN_NORequest()
        {
        }
        
        public GetASN_NORequest(CloudApiService.GetASN_NORequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetASN_NORequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Turn;
        
        public GetASN_NORequestBody()
        {
        }
        
        public GetASN_NORequestBody(string Turn)
        {
            this.Turn = Turn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetASN_NOResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetASN_NOResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetASN_NOResponseBody Body;
        
        public GetASN_NOResponse()
        {
        }
        
        public GetASN_NOResponse(CloudApiService.GetASN_NOResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetASN_NOResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetASN_NOResult;
        
        public GetASN_NOResponseBody()
        {
        }
        
        public GetASN_NOResponseBody(string GetASN_NOResult)
        {
            this.GetASN_NOResult = GetASN_NOResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOrderIDRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOrderID", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetOrderIDRequestBody Body;
        
        public GetOrderIDRequest()
        {
        }
        
        public GetOrderIDRequest(CloudApiService.GetOrderIDRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetOrderIDRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GoodsCode;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Field3;
        
        public GetOrderIDRequestBody()
        {
        }
        
        public GetOrderIDRequestBody(string GoodsCode, string Field3)
        {
            this.GoodsCode = GoodsCode;
            this.Field3 = Field3;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetOrderIDResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetOrderIDResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetOrderIDResponseBody Body;
        
        public GetOrderIDResponse()
        {
        }
        
        public GetOrderIDResponse(CloudApiService.GetOrderIDResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetOrderIDResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetOrderIDResult;
        
        public GetOrderIDResponseBody()
        {
        }
        
        public GetOrderIDResponseBody(string GetOrderIDResult)
        {
            this.GetOrderIDResult = GetOrderIDResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UpdateISPrintRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="UpdateISPrint", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.UpdateISPrintRequestBody Body;
        
        public UpdateISPrintRequest()
        {
        }
        
        public UpdateISPrintRequest(CloudApiService.UpdateISPrintRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class UpdateISPrintRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ODR_OrderMainID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string OrderNo;
        
        public UpdateISPrintRequestBody()
        {
        }
        
        public UpdateISPrintRequestBody(string ODR_OrderMainID, string OrderNo)
        {
            this.ODR_OrderMainID = ODR_OrderMainID;
            this.OrderNo = OrderNo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UpdateISPrintResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="UpdateISPrintResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.UpdateISPrintResponseBody Body;
        
        public UpdateISPrintResponse()
        {
        }
        
        public UpdateISPrintResponse(CloudApiService.UpdateISPrintResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class UpdateISPrintResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string UpdateISPrintResult;
        
        public UpdateISPrintResponseBody()
        {
        }
        
        public UpdateISPrintResponseBody(string UpdateISPrintResult)
        {
            this.UpdateISPrintResult = UpdateISPrintResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Select_SKU_CountRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Select_SKU_Count", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Select_SKU_CountRequestBody Body;
        
        public Select_SKU_CountRequest()
        {
        }
        
        public Select_SKU_CountRequest(CloudApiService.Select_SKU_CountRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Select_SKU_CountRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Field3;
        
        public Select_SKU_CountRequestBody()
        {
        }
        
        public Select_SKU_CountRequestBody(string Field3)
        {
            this.Field3 = Field3;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Select_SKU_CountResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Select_SKU_CountResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Select_SKU_CountResponseBody Body;
        
        public Select_SKU_CountResponse()
        {
        }
        
        public Select_SKU_CountResponse(CloudApiService.Select_SKU_CountResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Select_SKU_CountResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Select_SKU_CountResult;
        
        public Select_SKU_CountResponseBody()
        {
        }
        
        public Select_SKU_CountResponseBody(string Select_SKU_CountResult)
        {
            this.Select_SKU_CountResult = Select_SKU_CountResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class InsertUpdateOrderRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="InsertUpdateOrder", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.InsertUpdateOrderRequestBody Body;
        
        public InsertUpdateOrderRequest()
        {
        }
        
        public InsertUpdateOrderRequest(CloudApiService.InsertUpdateOrderRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class InsertUpdateOrderRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string strorderinfo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string strorderproduct;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string stradd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string secretkey;
        
        public InsertUpdateOrderRequestBody()
        {
        }
        
        public InsertUpdateOrderRequestBody(string strorderinfo, string strorderproduct, string stradd, string secretkey)
        {
            this.strorderinfo = strorderinfo;
            this.strorderproduct = strorderproduct;
            this.stradd = stradd;
            this.secretkey = secretkey;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class InsertUpdateOrderResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="InsertUpdateOrderResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.InsertUpdateOrderResponseBody Body;
        
        public InsertUpdateOrderResponse()
        {
        }
        
        public InsertUpdateOrderResponse(CloudApiService.InsertUpdateOrderResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class InsertUpdateOrderResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string InsertUpdateOrderResult;
        
        public InsertUpdateOrderResponseBody()
        {
        }
        
        public InsertUpdateOrderResponseBody(string InsertUpdateOrderResult)
        {
            this.InsertUpdateOrderResult = InsertUpdateOrderResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CancelOrderRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CancelOrder", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.CancelOrderRequestBody Body;
        
        public CancelOrderRequest()
        {
        }
        
        public CancelOrderRequest(CloudApiService.CancelOrderRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class CancelOrderRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string OrderNo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Key;
        
        public CancelOrderRequestBody()
        {
        }
        
        public CancelOrderRequestBody(string OrderNo, string Key)
        {
            this.OrderNo = OrderNo;
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CancelOrderResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CancelOrderResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.CancelOrderResponseBody Body;
        
        public CancelOrderResponse()
        {
        }
        
        public CancelOrderResponse(CloudApiService.CancelOrderResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class CancelOrderResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string CancelOrderResult;
        
        public CancelOrderResponseBody()
        {
        }
        
        public CancelOrderResponseBody(string CancelOrderResult)
        {
            this.CancelOrderResult = CancelOrderResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteOrderRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteOrder", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.DeleteOrderRequestBody Body;
        
        public DeleteOrderRequest()
        {
        }
        
        public DeleteOrderRequest(CloudApiService.DeleteOrderRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class DeleteOrderRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string OrderNo;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Key;
        
        public DeleteOrderRequestBody()
        {
        }
        
        public DeleteOrderRequestBody(string OrderNo, string Key)
        {
            this.OrderNo = OrderNo;
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class DeleteOrderResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="DeleteOrderResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.DeleteOrderResponseBody Body;
        
        public DeleteOrderResponse()
        {
        }
        
        public DeleteOrderResponse(CloudApiService.DeleteOrderResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class DeleteOrderResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DeleteOrderResult;
        
        public DeleteOrderResponseBody()
        {
        }
        
        public DeleteOrderResponseBody(string DeleteOrderResult)
        {
            this.DeleteOrderResult = DeleteOrderResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Create_GFF_GoodsRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Create_GFF_Goods", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Create_GFF_GoodsRequestBody Body;
        
        public Create_GFF_GoodsRequest()
        {
        }
        
        public Create_GFF_GoodsRequest(CloudApiService.Create_GFF_GoodsRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Create_GFF_GoodsRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GFF_Goods;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Userid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Key;
        
        public Create_GFF_GoodsRequestBody()
        {
        }
        
        public Create_GFF_GoodsRequestBody(string GFF_Goods, string Userid, string Key)
        {
            this.GFF_Goods = GFF_Goods;
            this.Userid = Userid;
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Create_GFF_GoodsResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Create_GFF_GoodsResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Create_GFF_GoodsResponseBody Body;
        
        public Create_GFF_GoodsResponse()
        {
        }
        
        public Create_GFF_GoodsResponse(CloudApiService.Create_GFF_GoodsResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Create_GFF_GoodsResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Create_GFF_GoodsResult;
        
        public Create_GFF_GoodsResponseBody()
        {
        }
        
        public Create_GFF_GoodsResponseBody(string Create_GFF_GoodsResult)
        {
            this.Create_GFF_GoodsResult = Create_GFF_GoodsResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Create_ASNMainRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Create_ASNMain", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Create_ASNMainRequestBody Body;
        
        public Create_ASNMainRequest()
        {
        }
        
        public Create_ASNMainRequest(CloudApiService.Create_ASNMainRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Create_ASNMainRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ASNMain;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Userid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Key;
        
        public Create_ASNMainRequestBody()
        {
        }
        
        public Create_ASNMainRequestBody(string ASNMain, string Userid, string Key)
        {
            this.ASNMain = ASNMain;
            this.Userid = Userid;
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Create_ASNMainResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Create_ASNMainResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Create_ASNMainResponseBody Body;
        
        public Create_ASNMainResponse()
        {
        }
        
        public Create_ASNMainResponse(CloudApiService.Create_ASNMainResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Create_ASNMainResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Create_ASNMainResult;
        
        public Create_ASNMainResponseBody()
        {
        }
        
        public Create_ASNMainResponseBody(string Create_ASNMainResult)
        {
            this.Create_ASNMainResult = Create_ASNMainResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Create_RejectOrderRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Create_RejectOrder", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Create_RejectOrderRequestBody Body;
        
        public Create_RejectOrderRequest()
        {
        }
        
        public Create_RejectOrderRequest(CloudApiService.Create_RejectOrderRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Create_RejectOrderRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string RejectOrder;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Userid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string Key;
        
        public Create_RejectOrderRequestBody()
        {
        }
        
        public Create_RejectOrderRequestBody(string RejectOrder, string Userid, string Key)
        {
            this.RejectOrder = RejectOrder;
            this.Userid = Userid;
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Create_RejectOrderResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Create_RejectOrderResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Create_RejectOrderResponseBody Body;
        
        public Create_RejectOrderResponse()
        {
        }
        
        public Create_RejectOrderResponse(CloudApiService.Create_RejectOrderResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Create_RejectOrderResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Create_RejectOrderResult;
        
        public Create_RejectOrderResponseBody()
        {
        }
        
        public Create_RejectOrderResponseBody(string Create_RejectOrderResult)
        {
            this.Create_RejectOrderResult = Create_RejectOrderResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetASNMainListRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetASNMainList", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetASNMainListRequestBody Body;
        
        public GetASNMainListRequest()
        {
        }
        
        public GetASNMainListRequest(CloudApiService.GetASNMainListRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetASNMainListRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ASNNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Key;
        
        public GetASNMainListRequestBody()
        {
        }
        
        public GetASNMainListRequestBody(string ASNNumber, string Key)
        {
            this.ASNNumber = ASNNumber;
            this.Key = Key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetASNMainListResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetASNMainListResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetASNMainListResponseBody Body;
        
        public GetASNMainListResponse()
        {
        }
        
        public GetASNMainListResponse(CloudApiService.GetASNMainListResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetASNMainListResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetASNMainListResult;
        
        public GetASNMainListResponseBody()
        {
        }
        
        public GetASNMainListResponseBody(string GetASNMainListResult)
        {
            this.GetASNMainListResult = GetASNMainListResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PrintPDFRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PrintPDF", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.PrintPDFRequestBody Body;
        
        public PrintPDFRequest()
        {
        }
        
        public PrintPDFRequest(CloudApiService.PrintPDFRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PrintPDFRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string orderno;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string type;
        
        public PrintPDFRequestBody()
        {
        }
        
        public PrintPDFRequestBody(string orderno, string type)
        {
            this.orderno = orderno;
            this.type = type;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PrintPDFResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PrintPDFResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.PrintPDFResponseBody Body;
        
        public PrintPDFResponse()
        {
        }
        
        public PrintPDFResponse(CloudApiService.PrintPDFResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PrintPDFResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PrintPDFResult;
        
        public PrintPDFResponseBody()
        {
        }
        
        public PrintPDFResponseBody(string PrintPDFResult)
        {
            this.PrintPDFResult = PrintPDFResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetServiceRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetService", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetServiceRequestBody Body;
        
        public GetServiceRequest()
        {
        }
        
        public GetServiceRequest(CloudApiService.GetServiceRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetServiceRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string S_store;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string GoodsID;
        
        public GetServiceRequestBody()
        {
        }
        
        public GetServiceRequestBody(string S_store, string GoodsID)
        {
            this.S_store = S_store;
            this.GoodsID = GoodsID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetServiceResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetServiceResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.GetServiceResponseBody Body;
        
        public GetServiceResponse()
        {
        }
        
        public GetServiceResponse(CloudApiService.GetServiceResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetServiceResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetServiceResult;
        
        public GetServiceResponseBody()
        {
        }
        
        public GetServiceResponseBody(string GetServiceResult)
        {
            this.GetServiceResult = GetServiceResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getdataRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getdata", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getdataRequestBody Body;
        
        public getdataRequest()
        {
        }
        
        public getdataRequest(CloudApiService.getdataRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getdataRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DYear;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string DMonth;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string User;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string SKU;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string Dept;
        
        public getdataRequestBody()
        {
        }
        
        public getdataRequestBody(string DYear, string DMonth, string User, string SKU, string Dept)
        {
            this.DYear = DYear;
            this.DMonth = DMonth;
            this.User = User;
            this.SKU = SKU;
            this.Dept = Dept;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getdataResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getdataResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getdataResponseBody Body;
        
        public getdataResponse()
        {
        }
        
        public getdataResponse(CloudApiService.getdataResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getdataResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getdataResult;
        
        public getdataResponseBody()
        {
        }
        
        public getdataResponseBody(string getdataResult)
        {
            this.getdataResult = getdataResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getMagentoOrderRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getMagentoOrder", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getMagentoOrderRequestBody Body;
        
        public getMagentoOrderRequest()
        {
        }
        
        public getMagentoOrderRequest(CloudApiService.getMagentoOrderRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getMagentoOrderRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Base_APIAccountID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string GFF_CustomerID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string status;
        
        public getMagentoOrderRequestBody()
        {
        }
        
        public getMagentoOrderRequestBody(string Base_APIAccountID, string GFF_CustomerID, string status)
        {
            this.Base_APIAccountID = Base_APIAccountID;
            this.GFF_CustomerID = GFF_CustomerID;
            this.status = status;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getMagentoOrderResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getMagentoOrderResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getMagentoOrderResponseBody Body;
        
        public getMagentoOrderResponse()
        {
        }
        
        public getMagentoOrderResponse(CloudApiService.getMagentoOrderResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getMagentoOrderResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getMagentoOrderResult;
        
        public getMagentoOrderResponseBody()
        {
        }
        
        public getMagentoOrderResponseBody(string getMagentoOrderResult)
        {
            this.getMagentoOrderResult = getMagentoOrderResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SetMagentoTrackingNoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SetMagentoTrackingNo", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.SetMagentoTrackingNoRequestBody Body;
        
        public SetMagentoTrackingNoRequest()
        {
        }
        
        public SetMagentoTrackingNoRequest(CloudApiService.SetMagentoTrackingNoRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SetMagentoTrackingNoRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string orderid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Base_APIAccountID;
        
        public SetMagentoTrackingNoRequestBody()
        {
        }
        
        public SetMagentoTrackingNoRequestBody(string orderid, string Base_APIAccountID)
        {
            this.orderid = orderid;
            this.Base_APIAccountID = Base_APIAccountID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SetMagentoTrackingNoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SetMagentoTrackingNoResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.SetMagentoTrackingNoResponseBody Body;
        
        public SetMagentoTrackingNoResponse()
        {
        }
        
        public SetMagentoTrackingNoResponse(CloudApiService.SetMagentoTrackingNoResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SetMagentoTrackingNoResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string SetMagentoTrackingNoResult;
        
        public SetMagentoTrackingNoResponseBody()
        {
        }
        
        public SetMagentoTrackingNoResponseBody(string SetMagentoTrackingNoResult)
        {
            this.SetMagentoTrackingNoResult = SetMagentoTrackingNoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getBingRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getBing", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getBingRequestBody Body;
        
        public getBingRequest()
        {
        }
        
        public getBingRequest(CloudApiService.getBingRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getBingRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DYear;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string DMonth;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string User;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Dept;
        
        public getBingRequestBody()
        {
        }
        
        public getBingRequestBody(string DYear, string DMonth, string User, string Dept)
        {
            this.DYear = DYear;
            this.DMonth = DMonth;
            this.User = User;
            this.Dept = Dept;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getBingResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getBingResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getBingResponseBody Body;
        
        public getBingResponse()
        {
        }
        
        public getBingResponse(CloudApiService.getBingResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getBingResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getBingResult;
        
        public getBingResponseBody()
        {
        }
        
        public getBingResponseBody(string getBingResult)
        {
            this.getBingResult = getBingResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOrderRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getOrder", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getOrderRequestBody Body;
        
        public getOrderRequest()
        {
        }
        
        public getOrderRequest(CloudApiService.getOrderRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getOrderRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DYear;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string DMonth;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string User;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Dept;
        
        public getOrderRequestBody()
        {
        }
        
        public getOrderRequestBody(string DYear, string DMonth, string User, string Dept)
        {
            this.DYear = DYear;
            this.DMonth = DMonth;
            this.User = User;
            this.Dept = Dept;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOrderResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getOrderResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getOrderResponseBody Body;
        
        public getOrderResponse()
        {
        }
        
        public getOrderResponse(CloudApiService.getOrderResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getOrderResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getOrderResult;
        
        public getOrderResponseBody()
        {
        }
        
        public getOrderResponseBody(string getOrderResult)
        {
            this.getOrderResult = getOrderResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOrder1Request
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getOrder1", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getOrder1RequestBody Body;
        
        public getOrder1Request()
        {
        }
        
        public getOrder1Request(CloudApiService.getOrder1RequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getOrder1RequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DYear;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string DMonth;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string User;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Dept;
        
        public getOrder1RequestBody()
        {
        }
        
        public getOrder1RequestBody(string DYear, string DMonth, string User, string Dept)
        {
            this.DYear = DYear;
            this.DMonth = DMonth;
            this.User = User;
            this.Dept = Dept;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getOrder1Response
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getOrder1Response", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getOrder1ResponseBody Body;
        
        public getOrder1Response()
        {
        }
        
        public getOrder1Response(CloudApiService.getOrder1ResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getOrder1ResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string getOrder1Result;
        
        public getOrder1ResponseBody()
        {
        }
        
        public getOrder1ResponseBody(string getOrder1Result)
        {
            this.getOrder1Result = getOrder1Result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_ALLDataRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_ALLData", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Get_ALLDataRequestBody Body;
        
        public Get_ALLDataRequest()
        {
        }
        
        public Get_ALLDataRequest(CloudApiService.Get_ALLDataRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Get_ALLDataRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DYear;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string DMonth;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string User;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string SKU;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string Dept;
        
        public Get_ALLDataRequestBody()
        {
        }
        
        public Get_ALLDataRequestBody(string DYear, string DMonth, string User, string SKU, string Dept)
        {
            this.DYear = DYear;
            this.DMonth = DMonth;
            this.User = User;
            this.SKU = SKU;
            this.Dept = Dept;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_ALLDataResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_ALLDataResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Get_ALLDataResponseBody Body;
        
        public Get_ALLDataResponse()
        {
        }
        
        public Get_ALLDataResponse(CloudApiService.Get_ALLDataResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Get_ALLDataResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Get_ALLDataResult;
        
        public Get_ALLDataResponseBody()
        {
        }
        
        public Get_ALLDataResponseBody(string Get_ALLDataResult)
        {
            this.Get_ALLDataResult = Get_ALLDataResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_ALLBingRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_ALLBing", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Get_ALLBingRequestBody Body;
        
        public Get_ALLBingRequest()
        {
        }
        
        public Get_ALLBingRequest(CloudApiService.Get_ALLBingRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Get_ALLBingRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DYear;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string DMonth;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string User;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Dept;
        
        public Get_ALLBingRequestBody()
        {
        }
        
        public Get_ALLBingRequestBody(string DYear, string DMonth, string User, string Dept)
        {
            this.DYear = DYear;
            this.DMonth = DMonth;
            this.User = User;
            this.Dept = Dept;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Get_ALLBingResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Get_ALLBingResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.Get_ALLBingResponseBody Body;
        
        public Get_ALLBingResponse()
        {
        }
        
        public Get_ALLBingResponse(CloudApiService.Get_ALLBingResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Get_ALLBingResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Get_ALLBingResult;
        
        public Get_ALLBingResponseBody()
        {
        }
        
        public Get_ALLBingResponseBody(string Get_ALLBingResult)
        {
            this.Get_ALLBingResult = Get_ALLBingResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PintEUBRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PintEUB", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.PintEUBRequestBody Body;
        
        public PintEUBRequest()
        {
        }
        
        public PintEUBRequest(CloudApiService.PintEUBRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PintEUBRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string oid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string printcode;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string filetype;
        
        public PintEUBRequestBody()
        {
        }
        
        public PintEUBRequestBody(string oid, string printcode, string filetype)
        {
            this.oid = oid;
            this.printcode = printcode;
            this.filetype = filetype;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PintEUBResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PintEUBResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.PintEUBResponseBody Body;
        
        public PintEUBResponse()
        {
        }
        
        public PintEUBResponse(CloudApiService.PintEUBResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PintEUBResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PintEUBResult;
        
        public PintEUBResponseBody()
        {
        }
        
        public PintEUBResponseBody(string PintEUBResult)
        {
            this.PintEUBResult = PintEUBResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getdayRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getday", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getdayRequestBody Body;
        
        public getdayRequest()
        {
        }
        
        public getdayRequest(CloudApiService.getdayRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getdayRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string DYear;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string DMonth;
        
        public getdayRequestBody()
        {
        }
        
        public getdayRequestBody(string DYear, string DMonth)
        {
            this.DYear = DYear;
            this.DMonth = DMonth;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getdayResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="getdayResponse", Namespace="http://tempuri.org/", Order=0)]
        public CloudApiService.getdayResponseBody Body;
        
        public getdayResponse()
        {
        }
        
        public getdayResponse(CloudApiService.getdayResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class getdayResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public int getdayResult;
        
        public getdayResponseBody()
        {
        }
        
        public getdayResponseBody(int getdayResult)
        {
            this.getdayResult = getdayResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface APIWebServiceSoapChannel : CloudApiService.APIWebServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class APIWebServiceSoapClient : System.ServiceModel.ClientBase<CloudApiService.APIWebServiceSoap>, CloudApiService.APIWebServiceSoap
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public APIWebServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(APIWebServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), APIWebServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public APIWebServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(APIWebServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public APIWebServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(APIWebServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public APIWebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetLablesUrlResponse> CloudApiService.APIWebServiceSoap.GetLablesUrlAsync(CloudApiService.GetLablesUrlRequest request)
        {
            return base.Channel.GetLablesUrlAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetLablesUrlResponse> GetLablesUrlAsync(string OrderNo)
        {
            CloudApiService.GetLablesUrlRequest inValue = new CloudApiService.GetLablesUrlRequest();
            inValue.Body = new CloudApiService.GetLablesUrlRequestBody();
            inValue.Body.OrderNo = OrderNo;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetLablesUrlAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetUSPSLabelResponse> CloudApiService.APIWebServiceSoap.GetUSPSLabelAsync(CloudApiService.GetUSPSLabelRequest request)
        {
            return base.Channel.GetUSPSLabelAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetUSPSLabelResponse> GetUSPSLabelAsync(string TrackingNo)
        {
            CloudApiService.GetUSPSLabelRequest inValue = new CloudApiService.GetUSPSLabelRequest();
            inValue.Body = new CloudApiService.GetUSPSLabelRequestBody();
            inValue.Body.TrackingNo = TrackingNo;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetUSPSLabelAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetLablesMergeResponse> CloudApiService.APIWebServiceSoap.GetLablesMergeAsync(CloudApiService.GetLablesMergeRequest request)
        {
            return base.Channel.GetLablesMergeAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetLablesMergeResponse> GetLablesMergeAsync(string OrderNo)
        {
            CloudApiService.GetLablesMergeRequest inValue = new CloudApiService.GetLablesMergeRequest();
            inValue.Body = new CloudApiService.GetLablesMergeRequestBody();
            inValue.Body.OrderNo = OrderNo;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetLablesMergeAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.BindTrackingDetailResponse> CloudApiService.APIWebServiceSoap.BindTrackingDetailAsync(CloudApiService.BindTrackingDetailRequest request)
        {
            return base.Channel.BindTrackingDetailAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.BindTrackingDetailResponse> BindTrackingDetailAsync(string OrderNo)
        {
            CloudApiService.BindTrackingDetailRequest inValue = new CloudApiService.BindTrackingDetailRequest();
            inValue.Body = new CloudApiService.BindTrackingDetailRequestBody();
            inValue.Body.OrderNo = OrderNo;
            return ((CloudApiService.APIWebServiceSoap)(this)).BindTrackingDetailAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.PintPDFResponse> CloudApiService.APIWebServiceSoap.PintPDFAsync(CloudApiService.PintPDFRequest request)
        {
            return base.Channel.PintPDFAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.PintPDFResponse> PintPDFAsync(string order, string printtype)
        {
            CloudApiService.PintPDFRequest inValue = new CloudApiService.PintPDFRequest();
            inValue.Body = new CloudApiService.PintPDFRequestBody();
            inValue.Body.order = order;
            inValue.Body.printtype = printtype;
            return ((CloudApiService.APIWebServiceSoap)(this)).PintPDFAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.get_pinttotalResponse> CloudApiService.APIWebServiceSoap.get_pinttotalAsync(CloudApiService.get_pinttotalRequest request)
        {
            return base.Channel.get_pinttotalAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.get_pinttotalResponse> get_pinttotalAsync(string Start, string End, string Company)
        {
            CloudApiService.get_pinttotalRequest inValue = new CloudApiService.get_pinttotalRequest();
            inValue.Body = new CloudApiService.get_pinttotalRequestBody();
            inValue.Body.Start = Start;
            inValue.Body.End = End;
            inValue.Body.Company = Company;
            return ((CloudApiService.APIWebServiceSoap)(this)).get_pinttotalAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetLineDateResponse> CloudApiService.APIWebServiceSoap.GetLineDateAsync(CloudApiService.GetLineDateRequest request)
        {
            return base.Channel.GetLineDateAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetLineDateResponse> GetLineDateAsync(string startTime, string endTime)
        {
            CloudApiService.GetLineDateRequest inValue = new CloudApiService.GetLineDateRequest();
            inValue.Body = new CloudApiService.GetLineDateRequestBody();
            inValue.Body.startTime = startTime;
            inValue.Body.endTime = endTime;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetLineDateAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.HelloWorldResponse> CloudApiService.APIWebServiceSoap.HelloWorldAsync(CloudApiService.HelloWorldRequest request)
        {
            return base.Channel.HelloWorldAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.HelloWorldResponse> HelloWorldAsync()
        {
            CloudApiService.HelloWorldRequest inValue = new CloudApiService.HelloWorldRequest();
            inValue.Body = new CloudApiService.HelloWorldRequestBody();
            return ((CloudApiService.APIWebServiceSoap)(this)).HelloWorldAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.Get_Pda_SqlResponse> CloudApiService.APIWebServiceSoap.Get_Pda_SqlAsync(CloudApiService.Get_Pda_SqlRequest request)
        {
            return base.Channel.Get_Pda_SqlAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.Get_Pda_SqlResponse> Get_Pda_SqlAsync(string ip)
        {
            CloudApiService.Get_Pda_SqlRequest inValue = new CloudApiService.Get_Pda_SqlRequest();
            inValue.Body = new CloudApiService.Get_Pda_SqlRequestBody();
            inValue.Body.ip = ip;
            return ((CloudApiService.APIWebServiceSoap)(this)).Get_Pda_SqlAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getCountryResponse> CloudApiService.APIWebServiceSoap.getCountryAsync(CloudApiService.getCountryRequest request)
        {
            return base.Channel.getCountryAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getCountryResponse> getCountryAsync(string secretkey)
        {
            CloudApiService.getCountryRequest inValue = new CloudApiService.getCountryRequest();
            inValue.Body = new CloudApiService.getCountryRequestBody();
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getCountryAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getChanelFeeTypeResponse> CloudApiService.APIWebServiceSoap.getChanelFeeTypeAsync(CloudApiService.getChanelFeeTypeRequest request)
        {
            return base.Channel.getChanelFeeTypeAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getChanelFeeTypeResponse> getChanelFeeTypeAsync(string chanelID)
        {
            CloudApiService.getChanelFeeTypeRequest inValue = new CloudApiService.getChanelFeeTypeRequest();
            inValue.Body = new CloudApiService.getChanelFeeTypeRequestBody();
            inValue.Body.chanelID = chanelID;
            return ((CloudApiService.APIWebServiceSoap)(this)).getChanelFeeTypeAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getWarehouseResponse> CloudApiService.APIWebServiceSoap.getWarehouseAsync(CloudApiService.getWarehouseRequest request)
        {
            return base.Channel.getWarehouseAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getWarehouseResponse> getWarehouseAsync(string secretkey)
        {
            CloudApiService.getWarehouseRequest inValue = new CloudApiService.getWarehouseRequest();
            inValue.Body = new CloudApiService.getWarehouseRequestBody();
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getWarehouseAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getChannelResponse> CloudApiService.APIWebServiceSoap.getChannelAsync(CloudApiService.getChannelRequest request)
        {
            return base.Channel.getChannelAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getChannelResponse> getChannelAsync(string secretkey)
        {
            CloudApiService.getChannelRequest inValue = new CloudApiService.getChannelRequest();
            inValue.Body = new CloudApiService.getChannelRequestBody();
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getChannelAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getStockChannelResponse> CloudApiService.APIWebServiceSoap.getStockChannelAsync(CloudApiService.getStockChannelRequest request)
        {
            return base.Channel.getStockChannelAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getStockChannelResponse> getStockChannelAsync(string secretkey, string StockID)
        {
            CloudApiService.getStockChannelRequest inValue = new CloudApiService.getStockChannelRequest();
            inValue.Body = new CloudApiService.getStockChannelRequestBody();
            inValue.Body.secretkey = secretkey;
            inValue.Body.StockID = StockID;
            return ((CloudApiService.APIWebServiceSoap)(this)).getStockChannelAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.webappResponse> CloudApiService.APIWebServiceSoap.webappAsync(CloudApiService.webappRequest request)
        {
            return base.Channel.webappAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.webappResponse> webappAsync(string country, string weight)
        {
            CloudApiService.webappRequest inValue = new CloudApiService.webappRequest();
            inValue.Body = new CloudApiService.webappRequestBody();
            inValue.Body.country = country;
            inValue.Body.weight = weight;
            return ((CloudApiService.APIWebServiceSoap)(this)).webappAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getFeeByCWV_2Response> CloudApiService.APIWebServiceSoap.getFeeByCWV_2Async(CloudApiService.getFeeByCWV_2Request request)
        {
            return base.Channel.getFeeByCWV_2Async(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getFeeByCWV_2Response> getFeeByCWV_2Async(string FromCountry, string ToCountry, string weight, string volume, string customerid, string secretkey)
        {
            CloudApiService.getFeeByCWV_2Request inValue = new CloudApiService.getFeeByCWV_2Request();
            inValue.Body = new CloudApiService.getFeeByCWV_2RequestBody();
            inValue.Body.FromCountry = FromCountry;
            inValue.Body.ToCountry = ToCountry;
            inValue.Body.weight = weight;
            inValue.Body.volume = volume;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getFeeByCWV_2Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getFeeByCWVResponse> CloudApiService.APIWebServiceSoap.getFeeByCWVAsync(CloudApiService.getFeeByCWVRequest request)
        {
            return base.Channel.getFeeByCWVAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getFeeByCWVResponse> getFeeByCWVAsync(string country, string weight, string volume, string customerid, string secretkey)
        {
            CloudApiService.getFeeByCWVRequest inValue = new CloudApiService.getFeeByCWVRequest();
            inValue.Body = new CloudApiService.getFeeByCWVRequestBody();
            inValue.Body.country = country;
            inValue.Body.weight = weight;
            inValue.Body.volume = volume;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getFeeByCWVAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getebaySessionIdResponse> CloudApiService.APIWebServiceSoap.getebaySessionIdAsync(CloudApiService.getebaySessionIdRequest request)
        {
            return base.Channel.getebaySessionIdAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getebaySessionIdResponse> getebaySessionIdAsync()
        {
            CloudApiService.getebaySessionIdRequest inValue = new CloudApiService.getebaySessionIdRequest();
            inValue.Body = new CloudApiService.getebaySessionIdRequestBody();
            return ((CloudApiService.APIWebServiceSoap)(this)).getebaySessionIdAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getebayTokenResponse> CloudApiService.APIWebServiceSoap.getebayTokenAsync(CloudApiService.getebayTokenRequest request)
        {
            return base.Channel.getebayTokenAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getebayTokenResponse> getebayTokenAsync()
        {
            CloudApiService.getebayTokenRequest inValue = new CloudApiService.getebayTokenRequest();
            inValue.Body = new CloudApiService.getebayTokenRequestBody();
            return ((CloudApiService.APIWebServiceSoap)(this)).getebayTokenAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getebayOrderResponse> CloudApiService.APIWebServiceSoap.getebayOrderAsync(CloudApiService.getebayOrderRequest request)
        {
            return base.Channel.getebayOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getebayOrderResponse> getebayOrderAsync(string Base_APIAccountID, string GFF_CustomerID, string StrDate, string EndDate, string Status, string isGoodsStore)
        {
            CloudApiService.getebayOrderRequest inValue = new CloudApiService.getebayOrderRequest();
            inValue.Body = new CloudApiService.getebayOrderRequestBody();
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            inValue.Body.GFF_CustomerID = GFF_CustomerID;
            inValue.Body.StrDate = StrDate;
            inValue.Body.EndDate = EndDate;
            inValue.Body.Status = Status;
            inValue.Body.isGoodsStore = isGoodsStore;
            return ((CloudApiService.APIWebServiceSoap)(this)).getebayOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getebaymessageResponse> CloudApiService.APIWebServiceSoap.getebaymessageAsync(CloudApiService.getebaymessageRequest request)
        {
            return base.Channel.getebaymessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getebaymessageResponse> getebaymessageAsync(string maessage)
        {
            CloudApiService.getebaymessageRequest inValue = new CloudApiService.getebaymessageRequest();
            inValue.Body = new CloudApiService.getebaymessageRequestBody();
            inValue.Body.maessage = maessage;
            return ((CloudApiService.APIWebServiceSoap)(this)).getebaymessageAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getFeeByCWVSNResponse> CloudApiService.APIWebServiceSoap.getFeeByCWVSNAsync(CloudApiService.getFeeByCWVSNRequest request)
        {
            return base.Channel.getFeeByCWVSNAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getFeeByCWVSNResponse> getFeeByCWVSNAsync(string shortname, string weight, string volume, string customerid, string secretkey)
        {
            CloudApiService.getFeeByCWVSNRequest inValue = new CloudApiService.getFeeByCWVSNRequest();
            inValue.Body = new CloudApiService.getFeeByCWVSNRequestBody();
            inValue.Body.shortname = shortname;
            inValue.Body.weight = weight;
            inValue.Body.volume = volume;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getFeeByCWVSNAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getPackageResponse> CloudApiService.APIWebServiceSoap.getPackageAsync(CloudApiService.getPackageRequest request)
        {
            return base.Channel.getPackageAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getPackageResponse> getPackageAsync(string orderNO, string customerid, string secretkey)
        {
            CloudApiService.getPackageRequest inValue = new CloudApiService.getPackageRequest();
            inValue.Body = new CloudApiService.getPackageRequestBody();
            inValue.Body.orderNO = orderNO;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getPackageAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getOrder_TrackResponse> CloudApiService.APIWebServiceSoap.getOrder_TrackAsync(CloudApiService.getOrder_TrackRequest request)
        {
            return base.Channel.getOrder_TrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getOrder_TrackResponse> getOrder_TrackAsync(string Orderid, string secretkey)
        {
            CloudApiService.getOrder_TrackRequest inValue = new CloudApiService.getOrder_TrackRequest();
            inValue.Body = new CloudApiService.getOrder_TrackRequestBody();
            inValue.Body.Orderid = Orderid;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).getOrder_TrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetOrderTrackResponse> CloudApiService.APIWebServiceSoap.GetOrderTrackAsync(CloudApiService.GetOrderTrackRequest request)
        {
            return base.Channel.GetOrderTrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetOrderTrackResponse> GetOrderTrackAsync(string OrderNo, string secretkey)
        {
            CloudApiService.GetOrderTrackRequest inValue = new CloudApiService.GetOrderTrackRequest();
            inValue.Body = new CloudApiService.GetOrderTrackRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetOrderTrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.get_TrackResponse> CloudApiService.APIWebServiceSoap.get_TrackAsync(CloudApiService.get_TrackRequest request)
        {
            return base.Channel.get_TrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.get_TrackResponse> get_TrackAsync(string OrderNo, string Field1, string TrackingNo)
        {
            CloudApiService.get_TrackRequest inValue = new CloudApiService.get_TrackRequest();
            inValue.Body = new CloudApiService.get_TrackRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.Field1 = Field1;
            inValue.Body.TrackingNo = TrackingNo;
            return ((CloudApiService.APIWebServiceSoap)(this)).get_TrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.ComMon_TrackingResponse> CloudApiService.APIWebServiceSoap.ComMon_TrackingAsync(CloudApiService.ComMon_TrackingRequest request)
        {
            return base.Channel.ComMon_TrackingAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.ComMon_TrackingResponse> ComMon_TrackingAsync(string TrackingNo, string ChannelInfoCode, string key)
        {
            CloudApiService.ComMon_TrackingRequest inValue = new CloudApiService.ComMon_TrackingRequest();
            inValue.Body = new CloudApiService.ComMon_TrackingRequestBody();
            inValue.Body.TrackingNo = TrackingNo;
            inValue.Body.ChannelInfoCode = ChannelInfoCode;
            inValue.Body.key = key;
            return ((CloudApiService.APIWebServiceSoap)(this)).ComMon_TrackingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getStockResponse> CloudApiService.APIWebServiceSoap.getStockAsync(CloudApiService.getStockRequest request)
        {
            return base.Channel.getStockAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getStockResponse> getStockAsync(string skuorcode, string customerid, string secretkey, string WarehouseName)
        {
            CloudApiService.getStockRequest inValue = new CloudApiService.getStockRequest();
            inValue.Body = new CloudApiService.getStockRequestBody();
            inValue.Body.skuorcode = skuorcode;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            inValue.Body.WarehouseName = WarehouseName;
            return ((CloudApiService.APIWebServiceSoap)(this)).getStockAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GoodsListInfoResponse> CloudApiService.APIWebServiceSoap.GoodsListInfoAsync(CloudApiService.GoodsListInfoRequest request)
        {
            return base.Channel.GoodsListInfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GoodsListInfoResponse> GoodsListInfoAsync(string customerid, string secretkey)
        {
            CloudApiService.GoodsListInfoRequest inValue = new CloudApiService.GoodsListInfoRequest();
            inValue.Body = new CloudApiService.GoodsListInfoRequestBody();
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).GoodsListInfoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.SetEbayTrackResponse> CloudApiService.APIWebServiceSoap.SetEbayTrackAsync(CloudApiService.SetEbayTrackRequest request)
        {
            return base.Channel.SetEbayTrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.SetEbayTrackResponse> SetEbayTrackAsync(string orderid, string Base_APIAccountID)
        {
            CloudApiService.SetEbayTrackRequest inValue = new CloudApiService.SetEbayTrackRequest();
            inValue.Body = new CloudApiService.SetEbayTrackRequestBody();
            inValue.Body.orderid = orderid;
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            return ((CloudApiService.APIWebServiceSoap)(this)).SetEbayTrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.ChannelInfoResponse> CloudApiService.APIWebServiceSoap.ChannelInfoAsync(CloudApiService.ChannelInfoRequest request)
        {
            return base.Channel.ChannelInfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.ChannelInfoResponse> ChannelInfoAsync(string ChannelInfoID)
        {
            CloudApiService.ChannelInfoRequest inValue = new CloudApiService.ChannelInfoRequest();
            inValue.Body = new CloudApiService.ChannelInfoRequestBody();
            inValue.Body.ChannelInfoID = ChannelInfoID;
            return ((CloudApiService.APIWebServiceSoap)(this)).ChannelInfoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.ChannelInfo_subResponse> CloudApiService.APIWebServiceSoap.ChannelInfo_subAsync(CloudApiService.ChannelInfo_subRequest request)
        {
            return base.Channel.ChannelInfo_subAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.ChannelInfo_subResponse> ChannelInfo_subAsync(string subChannelInfoID)
        {
            CloudApiService.ChannelInfo_subRequest inValue = new CloudApiService.ChannelInfo_subRequest();
            inValue.Body = new CloudApiService.ChannelInfo_subRequestBody();
            inValue.Body.subChannelInfoID = subChannelInfoID;
            return ((CloudApiService.APIWebServiceSoap)(this)).ChannelInfo_subAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.ChannelInfo_idResponse> CloudApiService.APIWebServiceSoap.ChannelInfo_idAsync(CloudApiService.ChannelInfo_idRequest request)
        {
            return base.Channel.ChannelInfo_idAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.ChannelInfo_idResponse> ChannelInfo_idAsync(string subChannelInfoID)
        {
            CloudApiService.ChannelInfo_idRequest inValue = new CloudApiService.ChannelInfo_idRequest();
            inValue.Body = new CloudApiService.ChannelInfo_idRequestBody();
            inValue.Body.subChannelInfoID = subChannelInfoID;
            return ((CloudApiService.APIWebServiceSoap)(this)).ChannelInfo_idAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetWarehouseResponse1> CloudApiService.APIWebServiceSoap.GetWarehouse1Async(CloudApiService.GetWarehouseRequest1 request)
        {
            return base.Channel.GetWarehouse1Async(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetWarehouseResponse1> GetWarehouse1Async()
        {
            CloudApiService.GetWarehouseRequest1 inValue = new CloudApiService.GetWarehouseRequest1();
            return ((CloudApiService.APIWebServiceSoap)(this)).GetWarehouse1Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetASN_NOResponse> CloudApiService.APIWebServiceSoap.GetASN_NOAsync(CloudApiService.GetASN_NORequest request)
        {
            return base.Channel.GetASN_NOAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetASN_NOResponse> GetASN_NOAsync(string Turn)
        {
            CloudApiService.GetASN_NORequest inValue = new CloudApiService.GetASN_NORequest();
            inValue.Body = new CloudApiService.GetASN_NORequestBody();
            inValue.Body.Turn = Turn;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetASN_NOAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetOrderIDResponse> CloudApiService.APIWebServiceSoap.GetOrderIDAsync(CloudApiService.GetOrderIDRequest request)
        {
            return base.Channel.GetOrderIDAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetOrderIDResponse> GetOrderIDAsync(string GoodsCode, string Field3)
        {
            CloudApiService.GetOrderIDRequest inValue = new CloudApiService.GetOrderIDRequest();
            inValue.Body = new CloudApiService.GetOrderIDRequestBody();
            inValue.Body.GoodsCode = GoodsCode;
            inValue.Body.Field3 = Field3;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetOrderIDAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.UpdateISPrintResponse> CloudApiService.APIWebServiceSoap.UpdateISPrintAsync(CloudApiService.UpdateISPrintRequest request)
        {
            return base.Channel.UpdateISPrintAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.UpdateISPrintResponse> UpdateISPrintAsync(string ODR_OrderMainID, string OrderNo)
        {
            CloudApiService.UpdateISPrintRequest inValue = new CloudApiService.UpdateISPrintRequest();
            inValue.Body = new CloudApiService.UpdateISPrintRequestBody();
            inValue.Body.ODR_OrderMainID = ODR_OrderMainID;
            inValue.Body.OrderNo = OrderNo;
            return ((CloudApiService.APIWebServiceSoap)(this)).UpdateISPrintAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.Select_SKU_CountResponse> CloudApiService.APIWebServiceSoap.Select_SKU_CountAsync(CloudApiService.Select_SKU_CountRequest request)
        {
            return base.Channel.Select_SKU_CountAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.Select_SKU_CountResponse> Select_SKU_CountAsync(string Field3)
        {
            CloudApiService.Select_SKU_CountRequest inValue = new CloudApiService.Select_SKU_CountRequest();
            inValue.Body = new CloudApiService.Select_SKU_CountRequestBody();
            inValue.Body.Field3 = Field3;
            return ((CloudApiService.APIWebServiceSoap)(this)).Select_SKU_CountAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.InsertUpdateOrderResponse> CloudApiService.APIWebServiceSoap.InsertUpdateOrderAsync(CloudApiService.InsertUpdateOrderRequest request)
        {
            return base.Channel.InsertUpdateOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.InsertUpdateOrderResponse> InsertUpdateOrderAsync(string strorderinfo, string strorderproduct, string stradd, string secretkey)
        {
            CloudApiService.InsertUpdateOrderRequest inValue = new CloudApiService.InsertUpdateOrderRequest();
            inValue.Body = new CloudApiService.InsertUpdateOrderRequestBody();
            inValue.Body.strorderinfo = strorderinfo;
            inValue.Body.strorderproduct = strorderproduct;
            inValue.Body.stradd = stradd;
            inValue.Body.secretkey = secretkey;
            return ((CloudApiService.APIWebServiceSoap)(this)).InsertUpdateOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.CancelOrderResponse> CloudApiService.APIWebServiceSoap.CancelOrderAsync(CloudApiService.CancelOrderRequest request)
        {
            return base.Channel.CancelOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.CancelOrderResponse> CancelOrderAsync(string OrderNo, string Key)
        {
            CloudApiService.CancelOrderRequest inValue = new CloudApiService.CancelOrderRequest();
            inValue.Body = new CloudApiService.CancelOrderRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.Key = Key;
            return ((CloudApiService.APIWebServiceSoap)(this)).CancelOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.DeleteOrderResponse> CloudApiService.APIWebServiceSoap.DeleteOrderAsync(CloudApiService.DeleteOrderRequest request)
        {
            return base.Channel.DeleteOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.DeleteOrderResponse> DeleteOrderAsync(string OrderNo, string Key)
        {
            CloudApiService.DeleteOrderRequest inValue = new CloudApiService.DeleteOrderRequest();
            inValue.Body = new CloudApiService.DeleteOrderRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.Key = Key;
            return ((CloudApiService.APIWebServiceSoap)(this)).DeleteOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.Create_GFF_GoodsResponse> CloudApiService.APIWebServiceSoap.Create_GFF_GoodsAsync(CloudApiService.Create_GFF_GoodsRequest request)
        {
            return base.Channel.Create_GFF_GoodsAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.Create_GFF_GoodsResponse> Create_GFF_GoodsAsync(string GFF_Goods, string Userid, string Key)
        {
            CloudApiService.Create_GFF_GoodsRequest inValue = new CloudApiService.Create_GFF_GoodsRequest();
            inValue.Body = new CloudApiService.Create_GFF_GoodsRequestBody();
            inValue.Body.GFF_Goods = GFF_Goods;
            inValue.Body.Userid = Userid;
            inValue.Body.Key = Key;
            return ((CloudApiService.APIWebServiceSoap)(this)).Create_GFF_GoodsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.Create_ASNMainResponse> CloudApiService.APIWebServiceSoap.Create_ASNMainAsync(CloudApiService.Create_ASNMainRequest request)
        {
            return base.Channel.Create_ASNMainAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.Create_ASNMainResponse> Create_ASNMainAsync(string ASNMain, string Userid, string Key)
        {
            CloudApiService.Create_ASNMainRequest inValue = new CloudApiService.Create_ASNMainRequest();
            inValue.Body = new CloudApiService.Create_ASNMainRequestBody();
            inValue.Body.ASNMain = ASNMain;
            inValue.Body.Userid = Userid;
            inValue.Body.Key = Key;
            return ((CloudApiService.APIWebServiceSoap)(this)).Create_ASNMainAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.Create_RejectOrderResponse> CloudApiService.APIWebServiceSoap.Create_RejectOrderAsync(CloudApiService.Create_RejectOrderRequest request)
        {
            return base.Channel.Create_RejectOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.Create_RejectOrderResponse> Create_RejectOrderAsync(string RejectOrder, string Userid, string Key)
        {
            CloudApiService.Create_RejectOrderRequest inValue = new CloudApiService.Create_RejectOrderRequest();
            inValue.Body = new CloudApiService.Create_RejectOrderRequestBody();
            inValue.Body.RejectOrder = RejectOrder;
            inValue.Body.Userid = Userid;
            inValue.Body.Key = Key;
            return ((CloudApiService.APIWebServiceSoap)(this)).Create_RejectOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetASNMainListResponse> CloudApiService.APIWebServiceSoap.GetASNMainListAsync(CloudApiService.GetASNMainListRequest request)
        {
            return base.Channel.GetASNMainListAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetASNMainListResponse> GetASNMainListAsync(string ASNNumber, string Key)
        {
            CloudApiService.GetASNMainListRequest inValue = new CloudApiService.GetASNMainListRequest();
            inValue.Body = new CloudApiService.GetASNMainListRequestBody();
            inValue.Body.ASNNumber = ASNNumber;
            inValue.Body.Key = Key;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetASNMainListAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.PrintPDFResponse> CloudApiService.APIWebServiceSoap.PrintPDFAsync(CloudApiService.PrintPDFRequest request)
        {
            return base.Channel.PrintPDFAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.PrintPDFResponse> PrintPDFAsync(string orderno, string type)
        {
            CloudApiService.PrintPDFRequest inValue = new CloudApiService.PrintPDFRequest();
            inValue.Body = new CloudApiService.PrintPDFRequestBody();
            inValue.Body.orderno = orderno;
            inValue.Body.type = type;
            return ((CloudApiService.APIWebServiceSoap)(this)).PrintPDFAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.GetServiceResponse> CloudApiService.APIWebServiceSoap.GetServiceAsync(CloudApiService.GetServiceRequest request)
        {
            return base.Channel.GetServiceAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.GetServiceResponse> GetServiceAsync(string S_store, string GoodsID)
        {
            CloudApiService.GetServiceRequest inValue = new CloudApiService.GetServiceRequest();
            inValue.Body = new CloudApiService.GetServiceRequestBody();
            inValue.Body.S_store = S_store;
            inValue.Body.GoodsID = GoodsID;
            return ((CloudApiService.APIWebServiceSoap)(this)).GetServiceAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getdataResponse> CloudApiService.APIWebServiceSoap.getdataAsync(CloudApiService.getdataRequest request)
        {
            return base.Channel.getdataAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getdataResponse> getdataAsync(string DYear, string DMonth, string User, string SKU, string Dept)
        {
            CloudApiService.getdataRequest inValue = new CloudApiService.getdataRequest();
            inValue.Body = new CloudApiService.getdataRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.SKU = SKU;
            inValue.Body.Dept = Dept;
            return ((CloudApiService.APIWebServiceSoap)(this)).getdataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getMagentoOrderResponse> CloudApiService.APIWebServiceSoap.getMagentoOrderAsync(CloudApiService.getMagentoOrderRequest request)
        {
            return base.Channel.getMagentoOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getMagentoOrderResponse> getMagentoOrderAsync(string Base_APIAccountID, string GFF_CustomerID, string status)
        {
            CloudApiService.getMagentoOrderRequest inValue = new CloudApiService.getMagentoOrderRequest();
            inValue.Body = new CloudApiService.getMagentoOrderRequestBody();
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            inValue.Body.GFF_CustomerID = GFF_CustomerID;
            inValue.Body.status = status;
            return ((CloudApiService.APIWebServiceSoap)(this)).getMagentoOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.SetMagentoTrackingNoResponse> CloudApiService.APIWebServiceSoap.SetMagentoTrackingNoAsync(CloudApiService.SetMagentoTrackingNoRequest request)
        {
            return base.Channel.SetMagentoTrackingNoAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.SetMagentoTrackingNoResponse> SetMagentoTrackingNoAsync(string orderid, string Base_APIAccountID)
        {
            CloudApiService.SetMagentoTrackingNoRequest inValue = new CloudApiService.SetMagentoTrackingNoRequest();
            inValue.Body = new CloudApiService.SetMagentoTrackingNoRequestBody();
            inValue.Body.orderid = orderid;
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            return ((CloudApiService.APIWebServiceSoap)(this)).SetMagentoTrackingNoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getBingResponse> CloudApiService.APIWebServiceSoap.getBingAsync(CloudApiService.getBingRequest request)
        {
            return base.Channel.getBingAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getBingResponse> getBingAsync(string DYear, string DMonth, string User, string Dept)
        {
            CloudApiService.getBingRequest inValue = new CloudApiService.getBingRequest();
            inValue.Body = new CloudApiService.getBingRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((CloudApiService.APIWebServiceSoap)(this)).getBingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getOrderResponse> CloudApiService.APIWebServiceSoap.getOrderAsync(CloudApiService.getOrderRequest request)
        {
            return base.Channel.getOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getOrderResponse> getOrderAsync(string DYear, string DMonth, string User, string Dept)
        {
            CloudApiService.getOrderRequest inValue = new CloudApiService.getOrderRequest();
            inValue.Body = new CloudApiService.getOrderRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((CloudApiService.APIWebServiceSoap)(this)).getOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getOrder1Response> CloudApiService.APIWebServiceSoap.getOrder1Async(CloudApiService.getOrder1Request request)
        {
            return base.Channel.getOrder1Async(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getOrder1Response> getOrder1Async(string DYear, string DMonth, string User, string Dept)
        {
            CloudApiService.getOrder1Request inValue = new CloudApiService.getOrder1Request();
            inValue.Body = new CloudApiService.getOrder1RequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((CloudApiService.APIWebServiceSoap)(this)).getOrder1Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.Get_ALLDataResponse> CloudApiService.APIWebServiceSoap.Get_ALLDataAsync(CloudApiService.Get_ALLDataRequest request)
        {
            return base.Channel.Get_ALLDataAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.Get_ALLDataResponse> Get_ALLDataAsync(string DYear, string DMonth, string User, string SKU, string Dept)
        {
            CloudApiService.Get_ALLDataRequest inValue = new CloudApiService.Get_ALLDataRequest();
            inValue.Body = new CloudApiService.Get_ALLDataRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.SKU = SKU;
            inValue.Body.Dept = Dept;
            return ((CloudApiService.APIWebServiceSoap)(this)).Get_ALLDataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.Get_ALLBingResponse> CloudApiService.APIWebServiceSoap.Get_ALLBingAsync(CloudApiService.Get_ALLBingRequest request)
        {
            return base.Channel.Get_ALLBingAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.Get_ALLBingResponse> Get_ALLBingAsync(string DYear, string DMonth, string User, string Dept)
        {
            CloudApiService.Get_ALLBingRequest inValue = new CloudApiService.Get_ALLBingRequest();
            inValue.Body = new CloudApiService.Get_ALLBingRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((CloudApiService.APIWebServiceSoap)(this)).Get_ALLBingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.PintEUBResponse> CloudApiService.APIWebServiceSoap.PintEUBAsync(CloudApiService.PintEUBRequest request)
        {
            return base.Channel.PintEUBAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.PintEUBResponse> PintEUBAsync(string oid, string printcode, string filetype)
        {
            CloudApiService.PintEUBRequest inValue = new CloudApiService.PintEUBRequest();
            inValue.Body = new CloudApiService.PintEUBRequestBody();
            inValue.Body.oid = oid;
            inValue.Body.printcode = printcode;
            inValue.Body.filetype = filetype;
            return ((CloudApiService.APIWebServiceSoap)(this)).PintEUBAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<CloudApiService.getdayResponse> CloudApiService.APIWebServiceSoap.getdayAsync(CloudApiService.getdayRequest request)
        {
            return base.Channel.getdayAsync(request);
        }
        
        public System.Threading.Tasks.Task<CloudApiService.getdayResponse> getdayAsync(string DYear, string DMonth)
        {
            CloudApiService.getdayRequest inValue = new CloudApiService.getdayRequest();
            inValue.Body = new CloudApiService.getdayRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            return ((CloudApiService.APIWebServiceSoap)(this)).getdayAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.APIWebServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.APIWebServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.APIWebServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://www.tonch.com.cn/webservice/APIWebService.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.APIWebServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://www.tonch.com.cn/webservice/APIWebService.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            APIWebServiceSoap,
            
            APIWebServiceSoap12,
        }
    }
}
