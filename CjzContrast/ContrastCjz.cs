using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CjzDataBase;
using CjzFormat;

namespace CjzContrast
{
    public interface ContrastItf
    {

        CtaData ContrastCjz(string File, string CtaFile, int Option);
    }

    public class ContrastSvr : ContrastItf
    {
        public CtaData ContrastCjz(string File, string CtaFile, int Option)
        {
            CtaData ret = null;

            FormatItf formatSvr = new FormatSvr();
            CjzData CjzData = formatSvr.FormatCjz(File, 0);
            //CjzData.WriteToXmlFile(@"C:\Users\Administrator\Desktop\项目\cjz.xml");
            CjzData CtaCjzData = formatSvr.FormatCjz(CtaFile, 0);
            //CtaCjzData.WriteToXmlFile(@"C:\Users\Administrator\Desktop\项目\cjz1.xml");

            ret = ContrastCjz(CjzData, CtaCjzData, Option);

            return ret;
        }

        public CtaData ContrastCjz(CjzData CjzData, CjzData CtaCjzData, int Option)
        {
            CtaData ret = new CtaData();
            ret.fileName = CjzData.fileName;
            ret.ctaFileName = CtaCjzData.fileName;
            ret.cjzData = CjzData;
            ret.ctaCjzData = CtaCjzData;

            foreach (DocData doc in CjzData.docList)
            {
                CtaDocData ctaDoc = new CtaDocData();
                ret.docList.Add(ctaDoc);
                ctaDoc.docData = doc;
                ctaDoc.docName = doc.docName;
                ctaDoc.docLevel = doc.docLevel;
                ctaDoc.docType = doc.docType;
                ctaDoc.docDataId = doc.UniqId;
                ctaDoc.parentUid = doc.parentUid;
                DocData ddoc = GetContrastDoc(doc, CjzData.docList.IndexOf(doc), CtaCjzData);
                
                if (ddoc != null)
                {
                    ctaDoc.ctaDocData = ddoc;
                    ctaDoc.ctaDocDataId = ddoc.UniqId;
                    ctaDoc.errorCount = 0;

                    foreach (TableData table in doc.tableList)
                    {
                        CtaDzTableData ctaDzTable = new CtaDzTableData();
                        ctaDoc.tableList.Add(ctaDzTable);
                        ctaDzTable.tableName = table.tableName;
                        ctaDzTable.souTable = table;
                        ctaDzTable.ReFormFields();
                        ctaDzTable.RefreshRecs();
                        ctaDzTable.ReSetRecStatus();
                        
                        TableData dtable = GetContrastTable(table, ddoc);
                        if (dtable != null)
                        {
                            CtaTableData ctaTable = ctaDzTable.dzTable;
                            ctaTable.tableName = dtable.tableName;
                            ctaTable.souTable = dtable;
                            ctaTable.ReFormFields();
                            ctaTable.RefreshRecs();
                            ctaTable.ReSetRecStatus();

                            ctaDzTable.FormCtaRelates();
                            ctaDoc.errorCount = ctaDoc.errorCount + ctaDzTable.errorCount;
                        }
                        else
                        {
                            ctaDzTable.ctaStatus = (int)_ctaStatusEnum.csAdd;
                            ctaDoc.errorCount = ctaDoc.errorCount + 1;
                        }
                    }
                }
                else
                {
                    ctaDoc.ctaStatus = (int)_ctaStatusEnum.csAdd;
                    ctaDoc.errorCount = 1;
                }
            }

            foreach (DocData doc in CtaCjzData.docList)
            {
                DocData ddoc = GetContrastDoc(doc, CtaCjzData.docList.IndexOf(doc), CjzData);
                if (ddoc == null)
                {
                    CtaDocData ctaDoc = new CtaDocData();
                    ret.docList.Add(ctaDoc);
                    ctaDoc.ctaDocData = doc;
                    ctaDoc.docName = doc.docName;
                    ctaDoc.docLevel = doc.docLevel;
                    ctaDoc.docType = doc.docType;
                    ctaDoc.ctaDocDataId = doc.UniqId;
                    ctaDoc.ctaStatus = (int)_ctaStatusEnum.csDecrease;
                    ctaDoc.errorCount = 1;
                }
            }

            return ret;
        }

        private DocData GetContrastDoc(DocData doc, int idx, CjzData ctaCjzData)
        {
            DocData ret = null;
            if (idx < ctaCjzData.docList.Count)
            {
                DocData ddoc = ctaCjzData.docList[idx] as DocData;
                if ((doc.docName==ddoc.docName) && (doc.docLevel==ddoc.docLevel))
                {
                    ret = ddoc;
                }
                else
                {
                    ddoc = ctaCjzData.docList.Cast<DocData>().FirstOrDefault(a => a.docName==doc.docName);
                }
            }

            return ret;
        }

        private TableData GetContrastTable(TableData table, DocData ctadoc)
        {
            return ctadoc.tableList.Cast<TableData>().FirstOrDefault(a => a.tableName == table.tableName);
        }
    }

}
