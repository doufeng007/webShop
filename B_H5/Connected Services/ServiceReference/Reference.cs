//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace ServiceReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.APIWebServiceSoap")]
    public interface APIWebServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLablesUrl", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetLablesUrlResponse> GetLablesUrlAsync(ServiceReference.GetLablesUrlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetUSPSLabel", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetUSPSLabelResponse> GetUSPSLabelAsync(ServiceReference.GetUSPSLabelRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLablesMerge", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetLablesMergeResponse> GetLablesMergeAsync(ServiceReference.GetLablesMergeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BindTrackingDetail", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.BindTrackingDetailResponse> BindTrackingDetailAsync(ServiceReference.BindTrackingDetailRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PintPDF", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.PintPDFResponse> PintPDFAsync(ServiceReference.PintPDFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/get_pinttotal", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.get_pinttotalResponse> get_pinttotalAsync(ServiceReference.get_pinttotalRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLineDate", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetLineDateResponse> GetLineDateAsync(ServiceReference.GetLineDateRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.HelloWorldResponse> HelloWorldAsync(ServiceReference.HelloWorldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Get_Pda_Sql", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.Get_Pda_SqlResponse> Get_Pda_SqlAsync(ServiceReference.Get_Pda_SqlRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getCountry", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getCountryResponse> getCountryAsync(ServiceReference.getCountryRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getChanelFeeType", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getChanelFeeTypeResponse> getChanelFeeTypeAsync(ServiceReference.getChanelFeeTypeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getWarehouse", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getWarehouseResponse> getWarehouseAsync(ServiceReference.getWarehouseRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getChannel", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getChannelResponse> getChannelAsync(ServiceReference.getChannelRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getStockChannel", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getStockChannelResponse> getStockChannelAsync(ServiceReference.getStockChannelRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/webapp", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.webappResponse> webappAsync(ServiceReference.webappRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getFeeByCWV_2", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getFeeByCWV_2Response> getFeeByCWV_2Async(ServiceReference.getFeeByCWV_2Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getFeeByCWV", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getFeeByCWVResponse> getFeeByCWVAsync(ServiceReference.getFeeByCWVRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebaySessionId", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getebaySessionIdResponse> getebaySessionIdAsync(ServiceReference.getebaySessionIdRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebayToken", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getebayTokenResponse> getebayTokenAsync(ServiceReference.getebayTokenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebayOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getebayOrderResponse> getebayOrderAsync(ServiceReference.getebayOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getebaymessage", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getebaymessageResponse> getebaymessageAsync(ServiceReference.getebaymessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getFeeByCWVSN", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getFeeByCWVSNResponse> getFeeByCWVSNAsync(ServiceReference.getFeeByCWVSNRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getPackage", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getPackageResponse> getPackageAsync(ServiceReference.getPackageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getOrder_Track", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getOrder_TrackResponse> getOrder_TrackAsync(ServiceReference.getOrder_TrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetOrderTrack", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetOrderTrackResponse> GetOrderTrackAsync(ServiceReference.GetOrderTrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/get_Track", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.get_TrackResponse> get_TrackAsync(ServiceReference.get_TrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ComMon_Tracking", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.ComMon_TrackingResponse> ComMon_TrackingAsync(ServiceReference.ComMon_TrackingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getStock", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getStockResponse> getStockAsync(ServiceReference.getStockRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GoodsListInfo", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GoodsListInfoResponse> GoodsListInfoAsync(ServiceReference.GoodsListInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SetEbayTrack", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.SetEbayTrackResponse> SetEbayTrackAsync(ServiceReference.SetEbayTrackRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ChannelInfo", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.ChannelInfoResponse> ChannelInfoAsync(ServiceReference.ChannelInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ChannelInfo_sub", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.ChannelInfo_subResponse> ChannelInfo_subAsync(ServiceReference.ChannelInfo_subRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ChannelInfo_id", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.ChannelInfo_idResponse> ChannelInfo_idAsync(ServiceReference.ChannelInfo_idRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetWarehouse", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetWarehouseResponse1> GetWarehouse1Async(ServiceReference.GetWarehouseRequest1 request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetASN_NO", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetASN_NOResponse> GetASN_NOAsync(ServiceReference.GetASN_NORequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetOrderID", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetOrderIDResponse> GetOrderIDAsync(ServiceReference.GetOrderIDRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UpdateISPrint", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.UpdateISPrintResponse> UpdateISPrintAsync(ServiceReference.UpdateISPrintRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Select_SKU_Count", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.Select_SKU_CountResponse> Select_SKU_CountAsync(ServiceReference.Select_SKU_CountRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/InsertUpdateOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.InsertUpdateOrderResponse> InsertUpdateOrderAsync(ServiceReference.InsertUpdateOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CancelOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.CancelOrderResponse> CancelOrderAsync(ServiceReference.CancelOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DeleteOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.DeleteOrderResponse> DeleteOrderAsync(ServiceReference.DeleteOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Create_GFF_Goods", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.Create_GFF_GoodsResponse> Create_GFF_GoodsAsync(ServiceReference.Create_GFF_GoodsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Create_ASNMain", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.Create_ASNMainResponse> Create_ASNMainAsync(ServiceReference.Create_ASNMainRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Create_RejectOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.Create_RejectOrderResponse> Create_RejectOrderAsync(ServiceReference.Create_RejectOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetASNMainList", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetASNMainListResponse> GetASNMainListAsync(ServiceReference.GetASNMainListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PrintPDF", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.PrintPDFResponse> PrintPDFAsync(ServiceReference.PrintPDFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetService", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.GetServiceResponse> GetServiceAsync(ServiceReference.GetServiceRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getdata", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getdataResponse> getdataAsync(ServiceReference.getdataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getMagentoOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getMagentoOrderResponse> getMagentoOrderAsync(ServiceReference.getMagentoOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SetMagentoTrackingNo", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.SetMagentoTrackingNoResponse> SetMagentoTrackingNoAsync(ServiceReference.SetMagentoTrackingNoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getBing", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getBingResponse> getBingAsync(ServiceReference.getBingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getOrder", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getOrderResponse> getOrderAsync(ServiceReference.getOrderRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getOrder1", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getOrder1Response> getOrder1Async(ServiceReference.getOrder1Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Get_ALLData", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.Get_ALLDataResponse> Get_ALLDataAsync(ServiceReference.Get_ALLDataRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Get_ALLBing", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.Get_ALLBingResponse> Get_ALLBingAsync(ServiceReference.Get_ALLBingRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/PintEUB", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.PintEUBResponse> PintEUBAsync(ServiceReference.PintEUBRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/getday", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference.getdayResponse> getdayAsync(ServiceReference.getdayRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetLablesUrlRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetLablesUrl", Namespace="http://tempuri.org/", Order=0)]
        public ServiceReference.GetLablesUrlRequestBody Body;
        
        public GetLablesUrlRequest()
        {
        }
        
        public GetLablesUrlRequest(ServiceReference.GetLablesUrlRequestBody Body)
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
        public ServiceReference.GetLablesUrlResponseBody Body;
        
        public GetLablesUrlResponse()
        {
        }
        
        public GetLablesUrlResponse(ServiceReference.GetLablesUrlResponseBody Body)
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
        public ServiceReference.GetUSPSLabelRequestBody Body;
        
        public GetUSPSLabelRequest()
        {
        }
        
        public GetUSPSLabelRequest(ServiceReference.GetUSPSLabelRequestBody Body)
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
        public ServiceReference.GetUSPSLabelResponseBody Body;
        
        public GetUSPSLabelResponse()
        {
        }
        
        public GetUSPSLabelResponse(ServiceReference.GetUSPSLabelResponseBody Body)
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
        public ServiceReference.GetLablesMergeRequestBody Body;
        
        public GetLablesMergeRequest()
        {
        }
        
        public GetLablesMergeRequest(ServiceReference.GetLablesMergeRequestBody Body)
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
        public ServiceReference.GetLablesMergeResponseBody Body;
        
        public GetLablesMergeResponse()
        {
        }
        
        public GetLablesMergeResponse(ServiceReference.GetLablesMergeResponseBody Body)
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
        public ServiceReference.BindTrackingDetailRequestBody Body;
        
        public BindTrackingDetailRequest()
        {
        }
        
        public BindTrackingDetailRequest(ServiceReference.BindTrackingDetailRequestBody Body)
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
        public ServiceReference.BindTrackingDetailResponseBody Body;
        
        public BindTrackingDetailResponse()
        {
        }
        
        public BindTrackingDetailResponse(ServiceReference.BindTrackingDetailResponseBody Body)
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
        public ServiceReference.PintPDFRequestBody Body;
        
        public PintPDFRequest()
        {
        }
        
        public PintPDFRequest(ServiceReference.PintPDFRequestBody Body)
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
        public ServiceReference.PintPDFResponseBody Body;
        
        public PintPDFResponse()
        {
        }
        
        public PintPDFResponse(ServiceReference.PintPDFResponseBody Body)
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
        public ServiceReference.get_pinttotalRequestBody Body;
        
        public get_pinttotalRequest()
        {
        }
        
        public get_pinttotalRequest(ServiceReference.get_pinttotalRequestBody Body)
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
        public ServiceReference.get_pinttotalResponseBody Body;
        
        public get_pinttotalResponse()
        {
        }
        
        public get_pinttotalResponse(ServiceReference.get_pinttotalResponseBody Body)
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
        public ServiceReference.GetLineDateRequestBody Body;
        
        public GetLineDateRequest()
        {
        }
        
        public GetLineDateRequest(ServiceReference.GetLineDateRequestBody Body)
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
        public ServiceReference.GetLineDateResponseBody Body;
        
        public GetLineDateResponse()
        {
        }
        
        public GetLineDateResponse(ServiceReference.GetLineDateResponseBody Body)
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
        public ServiceReference.HelloWorldRequestBody Body;
        
        public HelloWorldRequest()
        {
        }
        
        public HelloWorldRequest(ServiceReference.HelloWorldRequestBody Body)
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
        public ServiceReference.HelloWorldResponseBody Body;
        
        public HelloWorldResponse()
        {
        }
        
        public HelloWorldResponse(ServiceReference.HelloWorldResponseBody Body)
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
        public ServiceReference.Get_Pda_SqlRequestBody Body;
        
        public Get_Pda_SqlRequest()
        {
        }
        
        public Get_Pda_SqlRequest(ServiceReference.Get_Pda_SqlRequestBody Body)
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
        public ServiceReference.Get_Pda_SqlResponseBody Body;
        
        public Get_Pda_SqlResponse()
        {
        }
        
        public Get_Pda_SqlResponse(ServiceReference.Get_Pda_SqlResponseBody Body)
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
        public ServiceReference.getCountryRequestBody Body;
        
        public getCountryRequest()
        {
        }
        
        public getCountryRequest(ServiceReference.getCountryRequestBody Body)
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
        public ServiceReference.getCountryResponseBody Body;
        
        public getCountryResponse()
        {
        }
        
        public getCountryResponse(ServiceReference.getCountryResponseBody Body)
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
        public ServiceReference.getChanelFeeTypeRequestBody Body;
        
        public getChanelFeeTypeRequest()
        {
        }
        
        public getChanelFeeTypeRequest(ServiceReference.getChanelFeeTypeRequestBody Body)
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
        public ServiceReference.getChanelFeeTypeResponseBody Body;
        
        public getChanelFeeTypeResponse()
        {
        }
        
        public getChanelFeeTypeResponse(ServiceReference.getChanelFeeTypeResponseBody Body)
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
        public ServiceReference.getWarehouseRequestBody Body;
        
        public getWarehouseRequest()
        {
        }
        
        public getWarehouseRequest(ServiceReference.getWarehouseRequestBody Body)
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
        public ServiceReference.getWarehouseResponseBody Body;
        
        public getWarehouseResponse()
        {
        }
        
        public getWarehouseResponse(ServiceReference.getWarehouseResponseBody Body)
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
        public ServiceReference.getChannelRequestBody Body;
        
        public getChannelRequest()
        {
        }
        
        public getChannelRequest(ServiceReference.getChannelRequestBody Body)
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
        public ServiceReference.getChannelResponseBody Body;
        
        public getChannelResponse()
        {
        }
        
        public getChannelResponse(ServiceReference.getChannelResponseBody Body)
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
        public ServiceReference.getStockChannelRequestBody Body;
        
        public getStockChannelRequest()
        {
        }
        
        public getStockChannelRequest(ServiceReference.getStockChannelRequestBody Body)
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
        public ServiceReference.getStockChannelResponseBody Body;
        
        public getStockChannelResponse()
        {
        }
        
        public getStockChannelResponse(ServiceReference.getStockChannelResponseBody Body)
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
        public ServiceReference.webappRequestBody Body;
        
        public webappRequest()
        {
        }
        
        public webappRequest(ServiceReference.webappRequestBody Body)
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
        public ServiceReference.webappResponseBody Body;
        
        public webappResponse()
        {
        }
        
        public webappResponse(ServiceReference.webappResponseBody Body)
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
        public ServiceReference.getFeeByCWV_2RequestBody Body;
        
        public getFeeByCWV_2Request()
        {
        }
        
        public getFeeByCWV_2Request(ServiceReference.getFeeByCWV_2RequestBody Body)
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
        public ServiceReference.getFeeByCWV_2ResponseBody Body;
        
        public getFeeByCWV_2Response()
        {
        }
        
        public getFeeByCWV_2Response(ServiceReference.getFeeByCWV_2ResponseBody Body)
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
        public ServiceReference.getFeeByCWVRequestBody Body;
        
        public getFeeByCWVRequest()
        {
        }
        
        public getFeeByCWVRequest(ServiceReference.getFeeByCWVRequestBody Body)
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
        public ServiceReference.getFeeByCWVResponseBody Body;
        
        public getFeeByCWVResponse()
        {
        }
        
        public getFeeByCWVResponse(ServiceReference.getFeeByCWVResponseBody Body)
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
        public ServiceReference.getebaySessionIdRequestBody Body;
        
        public getebaySessionIdRequest()
        {
        }
        
        public getebaySessionIdRequest(ServiceReference.getebaySessionIdRequestBody Body)
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
        public ServiceReference.getebaySessionIdResponseBody Body;
        
        public getebaySessionIdResponse()
        {
        }
        
        public getebaySessionIdResponse(ServiceReference.getebaySessionIdResponseBody Body)
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
        public ServiceReference.getebayTokenRequestBody Body;
        
        public getebayTokenRequest()
        {
        }
        
        public getebayTokenRequest(ServiceReference.getebayTokenRequestBody Body)
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
        public ServiceReference.getebayTokenResponseBody Body;
        
        public getebayTokenResponse()
        {
        }
        
        public getebayTokenResponse(ServiceReference.getebayTokenResponseBody Body)
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
        public ServiceReference.getebayOrderRequestBody Body;
        
        public getebayOrderRequest()
        {
        }
        
        public getebayOrderRequest(ServiceReference.getebayOrderRequestBody Body)
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
        public ServiceReference.getebayOrderResponseBody Body;
        
        public getebayOrderResponse()
        {
        }
        
        public getebayOrderResponse(ServiceReference.getebayOrderResponseBody Body)
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
        public ServiceReference.getebaymessageRequestBody Body;
        
        public getebaymessageRequest()
        {
        }
        
        public getebaymessageRequest(ServiceReference.getebaymessageRequestBody Body)
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
        public ServiceReference.getebaymessageResponseBody Body;
        
        public getebaymessageResponse()
        {
        }
        
        public getebaymessageResponse(ServiceReference.getebaymessageResponseBody Body)
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
        public ServiceReference.getFeeByCWVSNRequestBody Body;
        
        public getFeeByCWVSNRequest()
        {
        }
        
        public getFeeByCWVSNRequest(ServiceReference.getFeeByCWVSNRequestBody Body)
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
        public ServiceReference.getFeeByCWVSNResponseBody Body;
        
        public getFeeByCWVSNResponse()
        {
        }
        
        public getFeeByCWVSNResponse(ServiceReference.getFeeByCWVSNResponseBody Body)
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
        public ServiceReference.getPackageRequestBody Body;
        
        public getPackageRequest()
        {
        }
        
        public getPackageRequest(ServiceReference.getPackageRequestBody Body)
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
        public ServiceReference.getPackageResponseBody Body;
        
        public getPackageResponse()
        {
        }
        
        public getPackageResponse(ServiceReference.getPackageResponseBody Body)
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
        public ServiceReference.getOrder_TrackRequestBody Body;
        
        public getOrder_TrackRequest()
        {
        }
        
        public getOrder_TrackRequest(ServiceReference.getOrder_TrackRequestBody Body)
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
        public ServiceReference.getOrder_TrackResponseBody Body;
        
        public getOrder_TrackResponse()
        {
        }
        
        public getOrder_TrackResponse(ServiceReference.getOrder_TrackResponseBody Body)
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
        public ServiceReference.GetOrderTrackRequestBody Body;
        
        public GetOrderTrackRequest()
        {
        }
        
        public GetOrderTrackRequest(ServiceReference.GetOrderTrackRequestBody Body)
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
        public ServiceReference.GetOrderTrackResponseBody Body;
        
        public GetOrderTrackResponse()
        {
        }
        
        public GetOrderTrackResponse(ServiceReference.GetOrderTrackResponseBody Body)
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
        public ServiceReference.get_TrackRequestBody Body;
        
        public get_TrackRequest()
        {
        }
        
        public get_TrackRequest(ServiceReference.get_TrackRequestBody Body)
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
        public ServiceReference.get_TrackResponseBody Body;
        
        public get_TrackResponse()
        {
        }
        
        public get_TrackResponse(ServiceReference.get_TrackResponseBody Body)
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
        public ServiceReference.ComMon_TrackingRequestBody Body;
        
        public ComMon_TrackingRequest()
        {
        }
        
        public ComMon_TrackingRequest(ServiceReference.ComMon_TrackingRequestBody Body)
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
        public ServiceReference.ComMon_TrackingResponseBody Body;
        
        public ComMon_TrackingResponse()
        {
        }
        
        public ComMon_TrackingResponse(ServiceReference.ComMon_TrackingResponseBody Body)
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
        public ServiceReference.getStockRequestBody Body;
        
        public getStockRequest()
        {
        }
        
        public getStockRequest(ServiceReference.getStockRequestBody Body)
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
        public ServiceReference.getStockResponseBody Body;
        
        public getStockResponse()
        {
        }
        
        public getStockResponse(ServiceReference.getStockResponseBody Body)
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
        public ServiceReference.GoodsListInfoRequestBody Body;
        
        public GoodsListInfoRequest()
        {
        }
        
        public GoodsListInfoRequest(ServiceReference.GoodsListInfoRequestBody Body)
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
        public ServiceReference.GoodsListInfoResponseBody Body;
        
        public GoodsListInfoResponse()
        {
        }
        
        public GoodsListInfoResponse(ServiceReference.GoodsListInfoResponseBody Body)
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
        public ServiceReference.SetEbayTrackRequestBody Body;
        
        public SetEbayTrackRequest()
        {
        }
        
        public SetEbayTrackRequest(ServiceReference.SetEbayTrackRequestBody Body)
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
        public ServiceReference.SetEbayTrackResponseBody Body;
        
        public SetEbayTrackResponse()
        {
        }
        
        public SetEbayTrackResponse(ServiceReference.SetEbayTrackResponseBody Body)
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
        public ServiceReference.ChannelInfoRequestBody Body;
        
        public ChannelInfoRequest()
        {
        }
        
        public ChannelInfoRequest(ServiceReference.ChannelInfoRequestBody Body)
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
        public ServiceReference.ChannelInfoResponseBody Body;
        
        public ChannelInfoResponse()
        {
        }
        
        public ChannelInfoResponse(ServiceReference.ChannelInfoResponseBody Body)
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
        public ServiceReference.ChannelInfo_subRequestBody Body;
        
        public ChannelInfo_subRequest()
        {
        }
        
        public ChannelInfo_subRequest(ServiceReference.ChannelInfo_subRequestBody Body)
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
        public ServiceReference.ChannelInfo_subResponseBody Body;
        
        public ChannelInfo_subResponse()
        {
        }
        
        public ChannelInfo_subResponse(ServiceReference.ChannelInfo_subResponseBody Body)
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
        public ServiceReference.ChannelInfo_idRequestBody Body;
        
        public ChannelInfo_idRequest()
        {
        }
        
        public ChannelInfo_idRequest(ServiceReference.ChannelInfo_idRequestBody Body)
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
        public ServiceReference.ChannelInfo_idResponseBody Body;
        
        public ChannelInfo_idResponse()
        {
        }
        
        public ChannelInfo_idResponse(ServiceReference.ChannelInfo_idResponseBody Body)
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
        public ServiceReference.GetASN_NORequestBody Body;
        
        public GetASN_NORequest()
        {
        }
        
        public GetASN_NORequest(ServiceReference.GetASN_NORequestBody Body)
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
        public ServiceReference.GetASN_NOResponseBody Body;
        
        public GetASN_NOResponse()
        {
        }
        
        public GetASN_NOResponse(ServiceReference.GetASN_NOResponseBody Body)
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
        public ServiceReference.GetOrderIDRequestBody Body;
        
        public GetOrderIDRequest()
        {
        }
        
        public GetOrderIDRequest(ServiceReference.GetOrderIDRequestBody Body)
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
        public ServiceReference.GetOrderIDResponseBody Body;
        
        public GetOrderIDResponse()
        {
        }
        
        public GetOrderIDResponse(ServiceReference.GetOrderIDResponseBody Body)
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
        public ServiceReference.UpdateISPrintRequestBody Body;
        
        public UpdateISPrintRequest()
        {
        }
        
        public UpdateISPrintRequest(ServiceReference.UpdateISPrintRequestBody Body)
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
        public ServiceReference.UpdateISPrintResponseBody Body;
        
        public UpdateISPrintResponse()
        {
        }
        
        public UpdateISPrintResponse(ServiceReference.UpdateISPrintResponseBody Body)
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
        public ServiceReference.Select_SKU_CountRequestBody Body;
        
        public Select_SKU_CountRequest()
        {
        }
        
        public Select_SKU_CountRequest(ServiceReference.Select_SKU_CountRequestBody Body)
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
        public ServiceReference.Select_SKU_CountResponseBody Body;
        
        public Select_SKU_CountResponse()
        {
        }
        
        public Select_SKU_CountResponse(ServiceReference.Select_SKU_CountResponseBody Body)
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
        public ServiceReference.InsertUpdateOrderRequestBody Body;
        
        public InsertUpdateOrderRequest()
        {
        }
        
        public InsertUpdateOrderRequest(ServiceReference.InsertUpdateOrderRequestBody Body)
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
        public ServiceReference.InsertUpdateOrderResponseBody Body;
        
        public InsertUpdateOrderResponse()
        {
        }
        
        public InsertUpdateOrderResponse(ServiceReference.InsertUpdateOrderResponseBody Body)
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
        public ServiceReference.CancelOrderRequestBody Body;
        
        public CancelOrderRequest()
        {
        }
        
        public CancelOrderRequest(ServiceReference.CancelOrderRequestBody Body)
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
        public ServiceReference.CancelOrderResponseBody Body;
        
        public CancelOrderResponse()
        {
        }
        
        public CancelOrderResponse(ServiceReference.CancelOrderResponseBody Body)
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
        public ServiceReference.DeleteOrderRequestBody Body;
        
        public DeleteOrderRequest()
        {
        }
        
        public DeleteOrderRequest(ServiceReference.DeleteOrderRequestBody Body)
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
        public ServiceReference.DeleteOrderResponseBody Body;
        
        public DeleteOrderResponse()
        {
        }
        
        public DeleteOrderResponse(ServiceReference.DeleteOrderResponseBody Body)
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
        public ServiceReference.Create_GFF_GoodsRequestBody Body;
        
        public Create_GFF_GoodsRequest()
        {
        }
        
        public Create_GFF_GoodsRequest(ServiceReference.Create_GFF_GoodsRequestBody Body)
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
        public ServiceReference.Create_GFF_GoodsResponseBody Body;
        
        public Create_GFF_GoodsResponse()
        {
        }
        
        public Create_GFF_GoodsResponse(ServiceReference.Create_GFF_GoodsResponseBody Body)
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
        public ServiceReference.Create_ASNMainRequestBody Body;
        
        public Create_ASNMainRequest()
        {
        }
        
        public Create_ASNMainRequest(ServiceReference.Create_ASNMainRequestBody Body)
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
        public ServiceReference.Create_ASNMainResponseBody Body;
        
        public Create_ASNMainResponse()
        {
        }
        
        public Create_ASNMainResponse(ServiceReference.Create_ASNMainResponseBody Body)
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
        public ServiceReference.Create_RejectOrderRequestBody Body;
        
        public Create_RejectOrderRequest()
        {
        }
        
        public Create_RejectOrderRequest(ServiceReference.Create_RejectOrderRequestBody Body)
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
        public ServiceReference.Create_RejectOrderResponseBody Body;
        
        public Create_RejectOrderResponse()
        {
        }
        
        public Create_RejectOrderResponse(ServiceReference.Create_RejectOrderResponseBody Body)
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
        public ServiceReference.GetASNMainListRequestBody Body;
        
        public GetASNMainListRequest()
        {
        }
        
        public GetASNMainListRequest(ServiceReference.GetASNMainListRequestBody Body)
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
        public ServiceReference.GetASNMainListResponseBody Body;
        
        public GetASNMainListResponse()
        {
        }
        
        public GetASNMainListResponse(ServiceReference.GetASNMainListResponseBody Body)
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
        public ServiceReference.PrintPDFRequestBody Body;
        
        public PrintPDFRequest()
        {
        }
        
        public PrintPDFRequest(ServiceReference.PrintPDFRequestBody Body)
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
        public ServiceReference.PrintPDFResponseBody Body;
        
        public PrintPDFResponse()
        {
        }
        
        public PrintPDFResponse(ServiceReference.PrintPDFResponseBody Body)
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
        public ServiceReference.GetServiceRequestBody Body;
        
        public GetServiceRequest()
        {
        }
        
        public GetServiceRequest(ServiceReference.GetServiceRequestBody Body)
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
        public ServiceReference.GetServiceResponseBody Body;
        
        public GetServiceResponse()
        {
        }
        
        public GetServiceResponse(ServiceReference.GetServiceResponseBody Body)
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
        public ServiceReference.getdataRequestBody Body;
        
        public getdataRequest()
        {
        }
        
        public getdataRequest(ServiceReference.getdataRequestBody Body)
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
        public ServiceReference.getdataResponseBody Body;
        
        public getdataResponse()
        {
        }
        
        public getdataResponse(ServiceReference.getdataResponseBody Body)
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
        public ServiceReference.getMagentoOrderRequestBody Body;
        
        public getMagentoOrderRequest()
        {
        }
        
        public getMagentoOrderRequest(ServiceReference.getMagentoOrderRequestBody Body)
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
        public ServiceReference.getMagentoOrderResponseBody Body;
        
        public getMagentoOrderResponse()
        {
        }
        
        public getMagentoOrderResponse(ServiceReference.getMagentoOrderResponseBody Body)
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
        public ServiceReference.SetMagentoTrackingNoRequestBody Body;
        
        public SetMagentoTrackingNoRequest()
        {
        }
        
        public SetMagentoTrackingNoRequest(ServiceReference.SetMagentoTrackingNoRequestBody Body)
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
        public ServiceReference.SetMagentoTrackingNoResponseBody Body;
        
        public SetMagentoTrackingNoResponse()
        {
        }
        
        public SetMagentoTrackingNoResponse(ServiceReference.SetMagentoTrackingNoResponseBody Body)
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
        public ServiceReference.getBingRequestBody Body;
        
        public getBingRequest()
        {
        }
        
        public getBingRequest(ServiceReference.getBingRequestBody Body)
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
        public ServiceReference.getBingResponseBody Body;
        
        public getBingResponse()
        {
        }
        
        public getBingResponse(ServiceReference.getBingResponseBody Body)
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
        public ServiceReference.getOrderRequestBody Body;
        
        public getOrderRequest()
        {
        }
        
        public getOrderRequest(ServiceReference.getOrderRequestBody Body)
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
        public ServiceReference.getOrderResponseBody Body;
        
        public getOrderResponse()
        {
        }
        
        public getOrderResponse(ServiceReference.getOrderResponseBody Body)
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
        public ServiceReference.getOrder1RequestBody Body;
        
        public getOrder1Request()
        {
        }
        
        public getOrder1Request(ServiceReference.getOrder1RequestBody Body)
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
        public ServiceReference.getOrder1ResponseBody Body;
        
        public getOrder1Response()
        {
        }
        
        public getOrder1Response(ServiceReference.getOrder1ResponseBody Body)
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
        public ServiceReference.Get_ALLDataRequestBody Body;
        
        public Get_ALLDataRequest()
        {
        }
        
        public Get_ALLDataRequest(ServiceReference.Get_ALLDataRequestBody Body)
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
        public ServiceReference.Get_ALLDataResponseBody Body;
        
        public Get_ALLDataResponse()
        {
        }
        
        public Get_ALLDataResponse(ServiceReference.Get_ALLDataResponseBody Body)
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
        public ServiceReference.Get_ALLBingRequestBody Body;
        
        public Get_ALLBingRequest()
        {
        }
        
        public Get_ALLBingRequest(ServiceReference.Get_ALLBingRequestBody Body)
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
        public ServiceReference.Get_ALLBingResponseBody Body;
        
        public Get_ALLBingResponse()
        {
        }
        
        public Get_ALLBingResponse(ServiceReference.Get_ALLBingResponseBody Body)
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
        public ServiceReference.PintEUBRequestBody Body;
        
        public PintEUBRequest()
        {
        }
        
        public PintEUBRequest(ServiceReference.PintEUBRequestBody Body)
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
        public ServiceReference.PintEUBResponseBody Body;
        
        public PintEUBResponse()
        {
        }
        
        public PintEUBResponse(ServiceReference.PintEUBResponseBody Body)
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
        public ServiceReference.getdayRequestBody Body;
        
        public getdayRequest()
        {
        }
        
        public getdayRequest(ServiceReference.getdayRequestBody Body)
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
        public ServiceReference.getdayResponseBody Body;
        
        public getdayResponse()
        {
        }
        
        public getdayResponse(ServiceReference.getdayResponseBody Body)
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
    public interface APIWebServiceSoapChannel : ServiceReference.APIWebServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class APIWebServiceSoapClient : System.ServiceModel.ClientBase<ServiceReference.APIWebServiceSoap>, ServiceReference.APIWebServiceSoap
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
        System.Threading.Tasks.Task<ServiceReference.GetLablesUrlResponse> ServiceReference.APIWebServiceSoap.GetLablesUrlAsync(ServiceReference.GetLablesUrlRequest request)
        {
            return base.Channel.GetLablesUrlAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetLablesUrlResponse> GetLablesUrlAsync(string OrderNo)
        {
            ServiceReference.GetLablesUrlRequest inValue = new ServiceReference.GetLablesUrlRequest();
            inValue.Body = new ServiceReference.GetLablesUrlRequestBody();
            inValue.Body.OrderNo = OrderNo;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetLablesUrlAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetUSPSLabelResponse> ServiceReference.APIWebServiceSoap.GetUSPSLabelAsync(ServiceReference.GetUSPSLabelRequest request)
        {
            return base.Channel.GetUSPSLabelAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetUSPSLabelResponse> GetUSPSLabelAsync(string TrackingNo)
        {
            ServiceReference.GetUSPSLabelRequest inValue = new ServiceReference.GetUSPSLabelRequest();
            inValue.Body = new ServiceReference.GetUSPSLabelRequestBody();
            inValue.Body.TrackingNo = TrackingNo;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetUSPSLabelAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetLablesMergeResponse> ServiceReference.APIWebServiceSoap.GetLablesMergeAsync(ServiceReference.GetLablesMergeRequest request)
        {
            return base.Channel.GetLablesMergeAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetLablesMergeResponse> GetLablesMergeAsync(string OrderNo)
        {
            ServiceReference.GetLablesMergeRequest inValue = new ServiceReference.GetLablesMergeRequest();
            inValue.Body = new ServiceReference.GetLablesMergeRequestBody();
            inValue.Body.OrderNo = OrderNo;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetLablesMergeAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.BindTrackingDetailResponse> ServiceReference.APIWebServiceSoap.BindTrackingDetailAsync(ServiceReference.BindTrackingDetailRequest request)
        {
            return base.Channel.BindTrackingDetailAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.BindTrackingDetailResponse> BindTrackingDetailAsync(string OrderNo)
        {
            ServiceReference.BindTrackingDetailRequest inValue = new ServiceReference.BindTrackingDetailRequest();
            inValue.Body = new ServiceReference.BindTrackingDetailRequestBody();
            inValue.Body.OrderNo = OrderNo;
            return ((ServiceReference.APIWebServiceSoap)(this)).BindTrackingDetailAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.PintPDFResponse> ServiceReference.APIWebServiceSoap.PintPDFAsync(ServiceReference.PintPDFRequest request)
        {
            return base.Channel.PintPDFAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.PintPDFResponse> PintPDFAsync(string order, string printtype)
        {
            ServiceReference.PintPDFRequest inValue = new ServiceReference.PintPDFRequest();
            inValue.Body = new ServiceReference.PintPDFRequestBody();
            inValue.Body.order = order;
            inValue.Body.printtype = printtype;
            return ((ServiceReference.APIWebServiceSoap)(this)).PintPDFAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.get_pinttotalResponse> ServiceReference.APIWebServiceSoap.get_pinttotalAsync(ServiceReference.get_pinttotalRequest request)
        {
            return base.Channel.get_pinttotalAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.get_pinttotalResponse> get_pinttotalAsync(string Start, string End, string Company)
        {
            ServiceReference.get_pinttotalRequest inValue = new ServiceReference.get_pinttotalRequest();
            inValue.Body = new ServiceReference.get_pinttotalRequestBody();
            inValue.Body.Start = Start;
            inValue.Body.End = End;
            inValue.Body.Company = Company;
            return ((ServiceReference.APIWebServiceSoap)(this)).get_pinttotalAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetLineDateResponse> ServiceReference.APIWebServiceSoap.GetLineDateAsync(ServiceReference.GetLineDateRequest request)
        {
            return base.Channel.GetLineDateAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetLineDateResponse> GetLineDateAsync(string startTime, string endTime)
        {
            ServiceReference.GetLineDateRequest inValue = new ServiceReference.GetLineDateRequest();
            inValue.Body = new ServiceReference.GetLineDateRequestBody();
            inValue.Body.startTime = startTime;
            inValue.Body.endTime = endTime;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetLineDateAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.HelloWorldResponse> ServiceReference.APIWebServiceSoap.HelloWorldAsync(ServiceReference.HelloWorldRequest request)
        {
            return base.Channel.HelloWorldAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.HelloWorldResponse> HelloWorldAsync()
        {
            ServiceReference.HelloWorldRequest inValue = new ServiceReference.HelloWorldRequest();
            inValue.Body = new ServiceReference.HelloWorldRequestBody();
            return ((ServiceReference.APIWebServiceSoap)(this)).HelloWorldAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.Get_Pda_SqlResponse> ServiceReference.APIWebServiceSoap.Get_Pda_SqlAsync(ServiceReference.Get_Pda_SqlRequest request)
        {
            return base.Channel.Get_Pda_SqlAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.Get_Pda_SqlResponse> Get_Pda_SqlAsync(string ip)
        {
            ServiceReference.Get_Pda_SqlRequest inValue = new ServiceReference.Get_Pda_SqlRequest();
            inValue.Body = new ServiceReference.Get_Pda_SqlRequestBody();
            inValue.Body.ip = ip;
            return ((ServiceReference.APIWebServiceSoap)(this)).Get_Pda_SqlAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getCountryResponse> ServiceReference.APIWebServiceSoap.getCountryAsync(ServiceReference.getCountryRequest request)
        {
            return base.Channel.getCountryAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getCountryResponse> getCountryAsync(string secretkey)
        {
            ServiceReference.getCountryRequest inValue = new ServiceReference.getCountryRequest();
            inValue.Body = new ServiceReference.getCountryRequestBody();
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getCountryAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getChanelFeeTypeResponse> ServiceReference.APIWebServiceSoap.getChanelFeeTypeAsync(ServiceReference.getChanelFeeTypeRequest request)
        {
            return base.Channel.getChanelFeeTypeAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getChanelFeeTypeResponse> getChanelFeeTypeAsync(string chanelID)
        {
            ServiceReference.getChanelFeeTypeRequest inValue = new ServiceReference.getChanelFeeTypeRequest();
            inValue.Body = new ServiceReference.getChanelFeeTypeRequestBody();
            inValue.Body.chanelID = chanelID;
            return ((ServiceReference.APIWebServiceSoap)(this)).getChanelFeeTypeAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getWarehouseResponse> ServiceReference.APIWebServiceSoap.getWarehouseAsync(ServiceReference.getWarehouseRequest request)
        {
            return base.Channel.getWarehouseAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getWarehouseResponse> getWarehouseAsync(string secretkey)
        {
            ServiceReference.getWarehouseRequest inValue = new ServiceReference.getWarehouseRequest();
            inValue.Body = new ServiceReference.getWarehouseRequestBody();
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getWarehouseAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getChannelResponse> ServiceReference.APIWebServiceSoap.getChannelAsync(ServiceReference.getChannelRequest request)
        {
            return base.Channel.getChannelAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getChannelResponse> getChannelAsync(string secretkey)
        {
            ServiceReference.getChannelRequest inValue = new ServiceReference.getChannelRequest();
            inValue.Body = new ServiceReference.getChannelRequestBody();
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getChannelAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getStockChannelResponse> ServiceReference.APIWebServiceSoap.getStockChannelAsync(ServiceReference.getStockChannelRequest request)
        {
            return base.Channel.getStockChannelAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getStockChannelResponse> getStockChannelAsync(string secretkey, string StockID)
        {
            ServiceReference.getStockChannelRequest inValue = new ServiceReference.getStockChannelRequest();
            inValue.Body = new ServiceReference.getStockChannelRequestBody();
            inValue.Body.secretkey = secretkey;
            inValue.Body.StockID = StockID;
            return ((ServiceReference.APIWebServiceSoap)(this)).getStockChannelAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.webappResponse> ServiceReference.APIWebServiceSoap.webappAsync(ServiceReference.webappRequest request)
        {
            return base.Channel.webappAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.webappResponse> webappAsync(string country, string weight)
        {
            ServiceReference.webappRequest inValue = new ServiceReference.webappRequest();
            inValue.Body = new ServiceReference.webappRequestBody();
            inValue.Body.country = country;
            inValue.Body.weight = weight;
            return ((ServiceReference.APIWebServiceSoap)(this)).webappAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getFeeByCWV_2Response> ServiceReference.APIWebServiceSoap.getFeeByCWV_2Async(ServiceReference.getFeeByCWV_2Request request)
        {
            return base.Channel.getFeeByCWV_2Async(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getFeeByCWV_2Response> getFeeByCWV_2Async(string FromCountry, string ToCountry, string weight, string volume, string customerid, string secretkey)
        {
            ServiceReference.getFeeByCWV_2Request inValue = new ServiceReference.getFeeByCWV_2Request();
            inValue.Body = new ServiceReference.getFeeByCWV_2RequestBody();
            inValue.Body.FromCountry = FromCountry;
            inValue.Body.ToCountry = ToCountry;
            inValue.Body.weight = weight;
            inValue.Body.volume = volume;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getFeeByCWV_2Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getFeeByCWVResponse> ServiceReference.APIWebServiceSoap.getFeeByCWVAsync(ServiceReference.getFeeByCWVRequest request)
        {
            return base.Channel.getFeeByCWVAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getFeeByCWVResponse> getFeeByCWVAsync(string country, string weight, string volume, string customerid, string secretkey)
        {
            ServiceReference.getFeeByCWVRequest inValue = new ServiceReference.getFeeByCWVRequest();
            inValue.Body = new ServiceReference.getFeeByCWVRequestBody();
            inValue.Body.country = country;
            inValue.Body.weight = weight;
            inValue.Body.volume = volume;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getFeeByCWVAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getebaySessionIdResponse> ServiceReference.APIWebServiceSoap.getebaySessionIdAsync(ServiceReference.getebaySessionIdRequest request)
        {
            return base.Channel.getebaySessionIdAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getebaySessionIdResponse> getebaySessionIdAsync()
        {
            ServiceReference.getebaySessionIdRequest inValue = new ServiceReference.getebaySessionIdRequest();
            inValue.Body = new ServiceReference.getebaySessionIdRequestBody();
            return ((ServiceReference.APIWebServiceSoap)(this)).getebaySessionIdAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getebayTokenResponse> ServiceReference.APIWebServiceSoap.getebayTokenAsync(ServiceReference.getebayTokenRequest request)
        {
            return base.Channel.getebayTokenAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getebayTokenResponse> getebayTokenAsync()
        {
            ServiceReference.getebayTokenRequest inValue = new ServiceReference.getebayTokenRequest();
            inValue.Body = new ServiceReference.getebayTokenRequestBody();
            return ((ServiceReference.APIWebServiceSoap)(this)).getebayTokenAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getebayOrderResponse> ServiceReference.APIWebServiceSoap.getebayOrderAsync(ServiceReference.getebayOrderRequest request)
        {
            return base.Channel.getebayOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getebayOrderResponse> getebayOrderAsync(string Base_APIAccountID, string GFF_CustomerID, string StrDate, string EndDate, string Status, string isGoodsStore)
        {
            ServiceReference.getebayOrderRequest inValue = new ServiceReference.getebayOrderRequest();
            inValue.Body = new ServiceReference.getebayOrderRequestBody();
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            inValue.Body.GFF_CustomerID = GFF_CustomerID;
            inValue.Body.StrDate = StrDate;
            inValue.Body.EndDate = EndDate;
            inValue.Body.Status = Status;
            inValue.Body.isGoodsStore = isGoodsStore;
            return ((ServiceReference.APIWebServiceSoap)(this)).getebayOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getebaymessageResponse> ServiceReference.APIWebServiceSoap.getebaymessageAsync(ServiceReference.getebaymessageRequest request)
        {
            return base.Channel.getebaymessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getebaymessageResponse> getebaymessageAsync(string maessage)
        {
            ServiceReference.getebaymessageRequest inValue = new ServiceReference.getebaymessageRequest();
            inValue.Body = new ServiceReference.getebaymessageRequestBody();
            inValue.Body.maessage = maessage;
            return ((ServiceReference.APIWebServiceSoap)(this)).getebaymessageAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getFeeByCWVSNResponse> ServiceReference.APIWebServiceSoap.getFeeByCWVSNAsync(ServiceReference.getFeeByCWVSNRequest request)
        {
            return base.Channel.getFeeByCWVSNAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getFeeByCWVSNResponse> getFeeByCWVSNAsync(string shortname, string weight, string volume, string customerid, string secretkey)
        {
            ServiceReference.getFeeByCWVSNRequest inValue = new ServiceReference.getFeeByCWVSNRequest();
            inValue.Body = new ServiceReference.getFeeByCWVSNRequestBody();
            inValue.Body.shortname = shortname;
            inValue.Body.weight = weight;
            inValue.Body.volume = volume;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getFeeByCWVSNAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getPackageResponse> ServiceReference.APIWebServiceSoap.getPackageAsync(ServiceReference.getPackageRequest request)
        {
            return base.Channel.getPackageAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getPackageResponse> getPackageAsync(string orderNO, string customerid, string secretkey)
        {
            ServiceReference.getPackageRequest inValue = new ServiceReference.getPackageRequest();
            inValue.Body = new ServiceReference.getPackageRequestBody();
            inValue.Body.orderNO = orderNO;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getPackageAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getOrder_TrackResponse> ServiceReference.APIWebServiceSoap.getOrder_TrackAsync(ServiceReference.getOrder_TrackRequest request)
        {
            return base.Channel.getOrder_TrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getOrder_TrackResponse> getOrder_TrackAsync(string Orderid, string secretkey)
        {
            ServiceReference.getOrder_TrackRequest inValue = new ServiceReference.getOrder_TrackRequest();
            inValue.Body = new ServiceReference.getOrder_TrackRequestBody();
            inValue.Body.Orderid = Orderid;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).getOrder_TrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetOrderTrackResponse> ServiceReference.APIWebServiceSoap.GetOrderTrackAsync(ServiceReference.GetOrderTrackRequest request)
        {
            return base.Channel.GetOrderTrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetOrderTrackResponse> GetOrderTrackAsync(string OrderNo, string secretkey)
        {
            ServiceReference.GetOrderTrackRequest inValue = new ServiceReference.GetOrderTrackRequest();
            inValue.Body = new ServiceReference.GetOrderTrackRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetOrderTrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.get_TrackResponse> ServiceReference.APIWebServiceSoap.get_TrackAsync(ServiceReference.get_TrackRequest request)
        {
            return base.Channel.get_TrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.get_TrackResponse> get_TrackAsync(string OrderNo, string Field1, string TrackingNo)
        {
            ServiceReference.get_TrackRequest inValue = new ServiceReference.get_TrackRequest();
            inValue.Body = new ServiceReference.get_TrackRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.Field1 = Field1;
            inValue.Body.TrackingNo = TrackingNo;
            return ((ServiceReference.APIWebServiceSoap)(this)).get_TrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.ComMon_TrackingResponse> ServiceReference.APIWebServiceSoap.ComMon_TrackingAsync(ServiceReference.ComMon_TrackingRequest request)
        {
            return base.Channel.ComMon_TrackingAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.ComMon_TrackingResponse> ComMon_TrackingAsync(string TrackingNo, string ChannelInfoCode, string key)
        {
            ServiceReference.ComMon_TrackingRequest inValue = new ServiceReference.ComMon_TrackingRequest();
            inValue.Body = new ServiceReference.ComMon_TrackingRequestBody();
            inValue.Body.TrackingNo = TrackingNo;
            inValue.Body.ChannelInfoCode = ChannelInfoCode;
            inValue.Body.key = key;
            return ((ServiceReference.APIWebServiceSoap)(this)).ComMon_TrackingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getStockResponse> ServiceReference.APIWebServiceSoap.getStockAsync(ServiceReference.getStockRequest request)
        {
            return base.Channel.getStockAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getStockResponse> getStockAsync(string skuorcode, string customerid, string secretkey, string WarehouseName)
        {
            ServiceReference.getStockRequest inValue = new ServiceReference.getStockRequest();
            inValue.Body = new ServiceReference.getStockRequestBody();
            inValue.Body.skuorcode = skuorcode;
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            inValue.Body.WarehouseName = WarehouseName;
            return ((ServiceReference.APIWebServiceSoap)(this)).getStockAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GoodsListInfoResponse> ServiceReference.APIWebServiceSoap.GoodsListInfoAsync(ServiceReference.GoodsListInfoRequest request)
        {
            return base.Channel.GoodsListInfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GoodsListInfoResponse> GoodsListInfoAsync(string customerid, string secretkey)
        {
            ServiceReference.GoodsListInfoRequest inValue = new ServiceReference.GoodsListInfoRequest();
            inValue.Body = new ServiceReference.GoodsListInfoRequestBody();
            inValue.Body.customerid = customerid;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).GoodsListInfoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.SetEbayTrackResponse> ServiceReference.APIWebServiceSoap.SetEbayTrackAsync(ServiceReference.SetEbayTrackRequest request)
        {
            return base.Channel.SetEbayTrackAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.SetEbayTrackResponse> SetEbayTrackAsync(string orderid, string Base_APIAccountID)
        {
            ServiceReference.SetEbayTrackRequest inValue = new ServiceReference.SetEbayTrackRequest();
            inValue.Body = new ServiceReference.SetEbayTrackRequestBody();
            inValue.Body.orderid = orderid;
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            return ((ServiceReference.APIWebServiceSoap)(this)).SetEbayTrackAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.ChannelInfoResponse> ServiceReference.APIWebServiceSoap.ChannelInfoAsync(ServiceReference.ChannelInfoRequest request)
        {
            return base.Channel.ChannelInfoAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.ChannelInfoResponse> ChannelInfoAsync(string ChannelInfoID)
        {
            ServiceReference.ChannelInfoRequest inValue = new ServiceReference.ChannelInfoRequest();
            inValue.Body = new ServiceReference.ChannelInfoRequestBody();
            inValue.Body.ChannelInfoID = ChannelInfoID;
            return ((ServiceReference.APIWebServiceSoap)(this)).ChannelInfoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.ChannelInfo_subResponse> ServiceReference.APIWebServiceSoap.ChannelInfo_subAsync(ServiceReference.ChannelInfo_subRequest request)
        {
            return base.Channel.ChannelInfo_subAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.ChannelInfo_subResponse> ChannelInfo_subAsync(string subChannelInfoID)
        {
            ServiceReference.ChannelInfo_subRequest inValue = new ServiceReference.ChannelInfo_subRequest();
            inValue.Body = new ServiceReference.ChannelInfo_subRequestBody();
            inValue.Body.subChannelInfoID = subChannelInfoID;
            return ((ServiceReference.APIWebServiceSoap)(this)).ChannelInfo_subAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.ChannelInfo_idResponse> ServiceReference.APIWebServiceSoap.ChannelInfo_idAsync(ServiceReference.ChannelInfo_idRequest request)
        {
            return base.Channel.ChannelInfo_idAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.ChannelInfo_idResponse> ChannelInfo_idAsync(string subChannelInfoID)
        {
            ServiceReference.ChannelInfo_idRequest inValue = new ServiceReference.ChannelInfo_idRequest();
            inValue.Body = new ServiceReference.ChannelInfo_idRequestBody();
            inValue.Body.subChannelInfoID = subChannelInfoID;
            return ((ServiceReference.APIWebServiceSoap)(this)).ChannelInfo_idAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetWarehouseResponse1> ServiceReference.APIWebServiceSoap.GetWarehouse1Async(ServiceReference.GetWarehouseRequest1 request)
        {
            return base.Channel.GetWarehouse1Async(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetWarehouseResponse1> GetWarehouse1Async()
        {
            ServiceReference.GetWarehouseRequest1 inValue = new ServiceReference.GetWarehouseRequest1();
            return ((ServiceReference.APIWebServiceSoap)(this)).GetWarehouse1Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetASN_NOResponse> ServiceReference.APIWebServiceSoap.GetASN_NOAsync(ServiceReference.GetASN_NORequest request)
        {
            return base.Channel.GetASN_NOAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetASN_NOResponse> GetASN_NOAsync(string Turn)
        {
            ServiceReference.GetASN_NORequest inValue = new ServiceReference.GetASN_NORequest();
            inValue.Body = new ServiceReference.GetASN_NORequestBody();
            inValue.Body.Turn = Turn;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetASN_NOAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetOrderIDResponse> ServiceReference.APIWebServiceSoap.GetOrderIDAsync(ServiceReference.GetOrderIDRequest request)
        {
            return base.Channel.GetOrderIDAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetOrderIDResponse> GetOrderIDAsync(string GoodsCode, string Field3)
        {
            ServiceReference.GetOrderIDRequest inValue = new ServiceReference.GetOrderIDRequest();
            inValue.Body = new ServiceReference.GetOrderIDRequestBody();
            inValue.Body.GoodsCode = GoodsCode;
            inValue.Body.Field3 = Field3;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetOrderIDAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.UpdateISPrintResponse> ServiceReference.APIWebServiceSoap.UpdateISPrintAsync(ServiceReference.UpdateISPrintRequest request)
        {
            return base.Channel.UpdateISPrintAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.UpdateISPrintResponse> UpdateISPrintAsync(string ODR_OrderMainID, string OrderNo)
        {
            ServiceReference.UpdateISPrintRequest inValue = new ServiceReference.UpdateISPrintRequest();
            inValue.Body = new ServiceReference.UpdateISPrintRequestBody();
            inValue.Body.ODR_OrderMainID = ODR_OrderMainID;
            inValue.Body.OrderNo = OrderNo;
            return ((ServiceReference.APIWebServiceSoap)(this)).UpdateISPrintAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.Select_SKU_CountResponse> ServiceReference.APIWebServiceSoap.Select_SKU_CountAsync(ServiceReference.Select_SKU_CountRequest request)
        {
            return base.Channel.Select_SKU_CountAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.Select_SKU_CountResponse> Select_SKU_CountAsync(string Field3)
        {
            ServiceReference.Select_SKU_CountRequest inValue = new ServiceReference.Select_SKU_CountRequest();
            inValue.Body = new ServiceReference.Select_SKU_CountRequestBody();
            inValue.Body.Field3 = Field3;
            return ((ServiceReference.APIWebServiceSoap)(this)).Select_SKU_CountAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.InsertUpdateOrderResponse> ServiceReference.APIWebServiceSoap.InsertUpdateOrderAsync(ServiceReference.InsertUpdateOrderRequest request)
        {
            return base.Channel.InsertUpdateOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.InsertUpdateOrderResponse> InsertUpdateOrderAsync(string strorderinfo, string strorderproduct, string stradd, string secretkey)
        {
            ServiceReference.InsertUpdateOrderRequest inValue = new ServiceReference.InsertUpdateOrderRequest();
            inValue.Body = new ServiceReference.InsertUpdateOrderRequestBody();
            inValue.Body.strorderinfo = strorderinfo;
            inValue.Body.strorderproduct = strorderproduct;
            inValue.Body.stradd = stradd;
            inValue.Body.secretkey = secretkey;
            return ((ServiceReference.APIWebServiceSoap)(this)).InsertUpdateOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.CancelOrderResponse> ServiceReference.APIWebServiceSoap.CancelOrderAsync(ServiceReference.CancelOrderRequest request)
        {
            return base.Channel.CancelOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.CancelOrderResponse> CancelOrderAsync(string OrderNo, string Key)
        {
            ServiceReference.CancelOrderRequest inValue = new ServiceReference.CancelOrderRequest();
            inValue.Body = new ServiceReference.CancelOrderRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.Key = Key;
            return ((ServiceReference.APIWebServiceSoap)(this)).CancelOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.DeleteOrderResponse> ServiceReference.APIWebServiceSoap.DeleteOrderAsync(ServiceReference.DeleteOrderRequest request)
        {
            return base.Channel.DeleteOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.DeleteOrderResponse> DeleteOrderAsync(string OrderNo, string Key)
        {
            ServiceReference.DeleteOrderRequest inValue = new ServiceReference.DeleteOrderRequest();
            inValue.Body = new ServiceReference.DeleteOrderRequestBody();
            inValue.Body.OrderNo = OrderNo;
            inValue.Body.Key = Key;
            return ((ServiceReference.APIWebServiceSoap)(this)).DeleteOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.Create_GFF_GoodsResponse> ServiceReference.APIWebServiceSoap.Create_GFF_GoodsAsync(ServiceReference.Create_GFF_GoodsRequest request)
        {
            return base.Channel.Create_GFF_GoodsAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.Create_GFF_GoodsResponse> Create_GFF_GoodsAsync(string GFF_Goods, string Userid, string Key)
        {
            ServiceReference.Create_GFF_GoodsRequest inValue = new ServiceReference.Create_GFF_GoodsRequest();
            inValue.Body = new ServiceReference.Create_GFF_GoodsRequestBody();
            inValue.Body.GFF_Goods = GFF_Goods;
            inValue.Body.Userid = Userid;
            inValue.Body.Key = Key;
            return ((ServiceReference.APIWebServiceSoap)(this)).Create_GFF_GoodsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.Create_ASNMainResponse> ServiceReference.APIWebServiceSoap.Create_ASNMainAsync(ServiceReference.Create_ASNMainRequest request)
        {
            return base.Channel.Create_ASNMainAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.Create_ASNMainResponse> Create_ASNMainAsync(string ASNMain, string Userid, string Key)
        {
            ServiceReference.Create_ASNMainRequest inValue = new ServiceReference.Create_ASNMainRequest();
            inValue.Body = new ServiceReference.Create_ASNMainRequestBody();
            inValue.Body.ASNMain = ASNMain;
            inValue.Body.Userid = Userid;
            inValue.Body.Key = Key;
            return ((ServiceReference.APIWebServiceSoap)(this)).Create_ASNMainAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.Create_RejectOrderResponse> ServiceReference.APIWebServiceSoap.Create_RejectOrderAsync(ServiceReference.Create_RejectOrderRequest request)
        {
            return base.Channel.Create_RejectOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.Create_RejectOrderResponse> Create_RejectOrderAsync(string RejectOrder, string Userid, string Key)
        {
            ServiceReference.Create_RejectOrderRequest inValue = new ServiceReference.Create_RejectOrderRequest();
            inValue.Body = new ServiceReference.Create_RejectOrderRequestBody();
            inValue.Body.RejectOrder = RejectOrder;
            inValue.Body.Userid = Userid;
            inValue.Body.Key = Key;
            return ((ServiceReference.APIWebServiceSoap)(this)).Create_RejectOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetASNMainListResponse> ServiceReference.APIWebServiceSoap.GetASNMainListAsync(ServiceReference.GetASNMainListRequest request)
        {
            return base.Channel.GetASNMainListAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetASNMainListResponse> GetASNMainListAsync(string ASNNumber, string Key)
        {
            ServiceReference.GetASNMainListRequest inValue = new ServiceReference.GetASNMainListRequest();
            inValue.Body = new ServiceReference.GetASNMainListRequestBody();
            inValue.Body.ASNNumber = ASNNumber;
            inValue.Body.Key = Key;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetASNMainListAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.PrintPDFResponse> ServiceReference.APIWebServiceSoap.PrintPDFAsync(ServiceReference.PrintPDFRequest request)
        {
            return base.Channel.PrintPDFAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.PrintPDFResponse> PrintPDFAsync(string orderno, string type)
        {
            ServiceReference.PrintPDFRequest inValue = new ServiceReference.PrintPDFRequest();
            inValue.Body = new ServiceReference.PrintPDFRequestBody();
            inValue.Body.orderno = orderno;
            inValue.Body.type = type;
            return ((ServiceReference.APIWebServiceSoap)(this)).PrintPDFAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.GetServiceResponse> ServiceReference.APIWebServiceSoap.GetServiceAsync(ServiceReference.GetServiceRequest request)
        {
            return base.Channel.GetServiceAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.GetServiceResponse> GetServiceAsync(string S_store, string GoodsID)
        {
            ServiceReference.GetServiceRequest inValue = new ServiceReference.GetServiceRequest();
            inValue.Body = new ServiceReference.GetServiceRequestBody();
            inValue.Body.S_store = S_store;
            inValue.Body.GoodsID = GoodsID;
            return ((ServiceReference.APIWebServiceSoap)(this)).GetServiceAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getdataResponse> ServiceReference.APIWebServiceSoap.getdataAsync(ServiceReference.getdataRequest request)
        {
            return base.Channel.getdataAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getdataResponse> getdataAsync(string DYear, string DMonth, string User, string SKU, string Dept)
        {
            ServiceReference.getdataRequest inValue = new ServiceReference.getdataRequest();
            inValue.Body = new ServiceReference.getdataRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.SKU = SKU;
            inValue.Body.Dept = Dept;
            return ((ServiceReference.APIWebServiceSoap)(this)).getdataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getMagentoOrderResponse> ServiceReference.APIWebServiceSoap.getMagentoOrderAsync(ServiceReference.getMagentoOrderRequest request)
        {
            return base.Channel.getMagentoOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getMagentoOrderResponse> getMagentoOrderAsync(string Base_APIAccountID, string GFF_CustomerID, string status)
        {
            ServiceReference.getMagentoOrderRequest inValue = new ServiceReference.getMagentoOrderRequest();
            inValue.Body = new ServiceReference.getMagentoOrderRequestBody();
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            inValue.Body.GFF_CustomerID = GFF_CustomerID;
            inValue.Body.status = status;
            return ((ServiceReference.APIWebServiceSoap)(this)).getMagentoOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.SetMagentoTrackingNoResponse> ServiceReference.APIWebServiceSoap.SetMagentoTrackingNoAsync(ServiceReference.SetMagentoTrackingNoRequest request)
        {
            return base.Channel.SetMagentoTrackingNoAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.SetMagentoTrackingNoResponse> SetMagentoTrackingNoAsync(string orderid, string Base_APIAccountID)
        {
            ServiceReference.SetMagentoTrackingNoRequest inValue = new ServiceReference.SetMagentoTrackingNoRequest();
            inValue.Body = new ServiceReference.SetMagentoTrackingNoRequestBody();
            inValue.Body.orderid = orderid;
            inValue.Body.Base_APIAccountID = Base_APIAccountID;
            return ((ServiceReference.APIWebServiceSoap)(this)).SetMagentoTrackingNoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getBingResponse> ServiceReference.APIWebServiceSoap.getBingAsync(ServiceReference.getBingRequest request)
        {
            return base.Channel.getBingAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getBingResponse> getBingAsync(string DYear, string DMonth, string User, string Dept)
        {
            ServiceReference.getBingRequest inValue = new ServiceReference.getBingRequest();
            inValue.Body = new ServiceReference.getBingRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((ServiceReference.APIWebServiceSoap)(this)).getBingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getOrderResponse> ServiceReference.APIWebServiceSoap.getOrderAsync(ServiceReference.getOrderRequest request)
        {
            return base.Channel.getOrderAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getOrderResponse> getOrderAsync(string DYear, string DMonth, string User, string Dept)
        {
            ServiceReference.getOrderRequest inValue = new ServiceReference.getOrderRequest();
            inValue.Body = new ServiceReference.getOrderRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((ServiceReference.APIWebServiceSoap)(this)).getOrderAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getOrder1Response> ServiceReference.APIWebServiceSoap.getOrder1Async(ServiceReference.getOrder1Request request)
        {
            return base.Channel.getOrder1Async(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getOrder1Response> getOrder1Async(string DYear, string DMonth, string User, string Dept)
        {
            ServiceReference.getOrder1Request inValue = new ServiceReference.getOrder1Request();
            inValue.Body = new ServiceReference.getOrder1RequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((ServiceReference.APIWebServiceSoap)(this)).getOrder1Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.Get_ALLDataResponse> ServiceReference.APIWebServiceSoap.Get_ALLDataAsync(ServiceReference.Get_ALLDataRequest request)
        {
            return base.Channel.Get_ALLDataAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.Get_ALLDataResponse> Get_ALLDataAsync(string DYear, string DMonth, string User, string SKU, string Dept)
        {
            ServiceReference.Get_ALLDataRequest inValue = new ServiceReference.Get_ALLDataRequest();
            inValue.Body = new ServiceReference.Get_ALLDataRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.SKU = SKU;
            inValue.Body.Dept = Dept;
            return ((ServiceReference.APIWebServiceSoap)(this)).Get_ALLDataAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.Get_ALLBingResponse> ServiceReference.APIWebServiceSoap.Get_ALLBingAsync(ServiceReference.Get_ALLBingRequest request)
        {
            return base.Channel.Get_ALLBingAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.Get_ALLBingResponse> Get_ALLBingAsync(string DYear, string DMonth, string User, string Dept)
        {
            ServiceReference.Get_ALLBingRequest inValue = new ServiceReference.Get_ALLBingRequest();
            inValue.Body = new ServiceReference.Get_ALLBingRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            inValue.Body.User = User;
            inValue.Body.Dept = Dept;
            return ((ServiceReference.APIWebServiceSoap)(this)).Get_ALLBingAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.PintEUBResponse> ServiceReference.APIWebServiceSoap.PintEUBAsync(ServiceReference.PintEUBRequest request)
        {
            return base.Channel.PintEUBAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.PintEUBResponse> PintEUBAsync(string oid, string printcode, string filetype)
        {
            ServiceReference.PintEUBRequest inValue = new ServiceReference.PintEUBRequest();
            inValue.Body = new ServiceReference.PintEUBRequestBody();
            inValue.Body.oid = oid;
            inValue.Body.printcode = printcode;
            inValue.Body.filetype = filetype;
            return ((ServiceReference.APIWebServiceSoap)(this)).PintEUBAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference.getdayResponse> ServiceReference.APIWebServiceSoap.getdayAsync(ServiceReference.getdayRequest request)
        {
            return base.Channel.getdayAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference.getdayResponse> getdayAsync(string DYear, string DMonth)
        {
            ServiceReference.getdayRequest inValue = new ServiceReference.getdayRequest();
            inValue.Body = new ServiceReference.getdayRequestBody();
            inValue.Body.DYear = DYear;
            inValue.Body.DMonth = DMonth;
            return ((ServiceReference.APIWebServiceSoap)(this)).getdayAsync(inValue);
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
