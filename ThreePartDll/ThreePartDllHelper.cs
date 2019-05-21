using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Text;


namespace ThreePartDll
{
    public class ThreePartDllHelper
    {
        public static bool CreatePDF(string soursePath, string savePath)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(soursePath);
                //保存PDF文件
                doc.Save(savePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static byte[] Get(string[] a, object[] b, string path,string content)
        {
            var doc = new Aspose.Words.Document(path);
            doc.MailMerge.Execute(a, b);
            DocumentBuilder builder = new DocumentBuilder(doc);
            builder.MoveToBookmark("content");
            builder.InsertHtml(content,true);
            var docStream = new System.IO.MemoryStream();
            doc.Save(docStream, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Docx));
            return docStream.ToArray();
        }


        //public void CreatGoods(WmsProductsSyncRequest.Item input)
        //{
        //    try
        //    {
        //        DefaultFwClient client = new DefaultFwClient(CloudServiceUrl, CloudServiceAppKey, CloudServiceAppSecret, CloudServicePartnerId);
        //        WmsProductsSyncRequest request = new WmsProductsSyncRequest();
        //        WmsProductsSyncResponse response = new WmsProductsSyncResponse();
        //        List<WmsProductsSyncRequest.Item> products = new List<WmsProductsSyncRequest.Item>();
        //        products.Add(input);
        //        request.Items = products;
        //        response = client.Execute(request);
        //        if (!response.IsSuccess)
        //        {
        //            Abp.Logging.LogHelper.Logger.Error($"调用云仓创建产品失败：返回code：{response.ErrCode}, Msg:{response.ErrMsg}");
        //            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建产品失败！");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Abp.Logging.LogHelper.Logger.Error($"调用云仓创建产品失败：异常{ex.Message}");
        //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "调用云仓创建产品失败！");
        //    }

        //}

    }





}
