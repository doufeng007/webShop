using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SearchAll
{
   public class OfficeHelper
    {
        /// <summary>
        /// 读取Word内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadWordText(string fileName)
        {
            string fileText = string.Empty;
            StringBuilder sbFileText = new StringBuilder();

            #region 打开文档
            XWPFDocument document = null;
            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    document = new XWPFDocument(file);
                }
            }
            catch (Exception e)
            {
                //LogHandler.LogWrite(string.Format("文件{0}打开失败，错误：{1}", new string[] { fileName, e.ToString() }));
            }
            #endregion
            if (document == null)
                return "";

            foreach (XWPFHeader xwpfHeader in document.HeaderList)
            {
                sbFileText.AppendLine(string.Format("{0}", new string[] { xwpfHeader.Text }));
            }
            foreach (XWPFFooter xwpfFooter in document.FooterList)
            {
                sbFileText.AppendLine(string.Format("{0}", new string[] { xwpfFooter.Text }));
            }

            #region 表格
            foreach (XWPFTable table in document.Tables)
            {
                foreach (XWPFTableRow row in table.Rows)
                {
                    foreach (XWPFTableCell cell in row.GetTableCells())
                    {
                        sbFileText.Append(cell.GetText());
                    }
                }
            }
            #endregion
            foreach (XWPFParagraph paragraph in document.Paragraphs)
            {
                sbFileText.AppendLine(paragraph.ParagraphText);

            }
            fileText = sbFileText.ToString();
            return fileText;
        }
        public static string ReadExcelText(string fileName)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                        workbook = new XSSFWorkbook(file);
                    else if (fileName.IndexOf(".xls") > 0) // 2003版本
                        workbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception e)
            {
                //LogHandler.LogWrite(string.Format("文件{0}打开失败，错误：{1}", new string[] { fileName, e.ToString() }));
            }
            StringBuilder sbFileText = new StringBuilder();
            try
            {
                sheet = workbook.GetSheetAt(0);
                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum;
                int rowCount = sheet.LastRowNum;
                for (int i = 0; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    for (int j = 0; j < cellCount; ++j)
                    {
                        //sbFileText.Append(row.GetCell(j).ToString().Trim());
                        NPOI.SS.UserModel.ICell cell = row.GetCell(j);
                        var txt = "";
                        if (cell != null)
                        {
                            if (cell.CellType == CellType.Numeric)
                            {
                                //判断是否日期类型
                                if (DateUtil.IsCellDateFormatted(cell))
                                {
                                    txt = row.GetCell(j).DateCellValue.ToString().Trim();
                                }
                                else
                                {
                                    txt = row.GetCell(j).ToString().Trim();
                                }
                            }
                            else
                            {
                                txt = row.GetCell(j).ToString().Trim();
                            }
                        }
                        sbFileText.Append(txt);
                    }
                    sbFileText.Append(" ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);

            }
            return sbFileText.ToString();
        }
    }
}
