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
    }



}
