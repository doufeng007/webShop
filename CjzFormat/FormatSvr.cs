using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CjzDataBase;
using System.IO;
using System.Xml;

namespace CjzFormat
{
    public interface FormatItf
    {

        CjzData FormatCjz(string AFile, int Option);
    }

    public class FormatSvr : FormatItf
    {
        string tmppath = null;

        public CjzData FormatCjz(string AFile, int Option)
        {
            CjzData ret = null;

            tmppath = Path.GetTempPath() + Path.GetFileNameWithoutExtension(AFile) + "\\";
            if (File.Exists(AFile))
            {
                CommUtils.UnZip(AFile, tmppath, null, true);
                if (true)
                {
                    try
                    {
                        string prjFile = "Project";
                        if (File.Exists(tmppath + prjFile))
                        {
                            XmlDocument mainXml = new XmlDocument();
                            byte[] fileData = System.IO.File.ReadAllBytes(tmppath + prjFile);
                            string fileStr = Encoding.UTF8.GetString(fileData);
                            mainXml.LoadXml(fileStr);
                            XmlNode xmNode = mainXml.SelectSingleNode("//建设项目");
                            if (xmNode != null)
                            {
                                ret = new CjzData();
                                ret.fileName = AFile;

                                // 格式化xml节点
                                FormatMainXml((XmlElement)xmNode, ret, Option);
                            }
                        }
                    }
                    finally
                    {
                        CommUtils.DeleteDirectory(tmppath);
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// 序列化项目主体
        /// </summary>
        /// <param name="xmNode"></param>
        /// <param name="cjzData"></param>
        private void FormatMainXml(XmlElement xmNode, CjzData cjzData, int option)
        {
            // 增加总项目Doc
            DocData doc = new DocData();
            doc.docName = xmNode.GetAttribute("项目名称");
            doc.docLevel = 1;
            doc.docType = "ZGC";
            doc.parentUid = "#";
            cjzData.docList.Add(doc);

            FormatSjDocData(xmNode, cjzData, doc);

            // 工程总信息
            XmlNode gczxxNode = xmNode.SelectSingleNode("工程总信息");
            if (gczxxNode != null)
                FormatGczxxXml((XmlElement)gczxxNode, doc);

            // 系统信息
            XmlNode xtxxNode = xmNode.SelectSingleNode("系统信息");
            if (xtxxNode != null)
                FormatXtxxXml((XmlElement)xtxxNode, doc);

            // 编制说明
            XmlNode bzsmNode = xmNode.SelectSingleNode("编制说明");
            if (bzsmNode != null)
            {
                cjzData.description = bzsmNode.Attributes.GetNamedItem("内容").Value;
            }

            //定额综合单价计算程序
            XmlNode zhdjNode = xmNode.SelectSingleNode("定额综合单价计算程序");
            if (zhdjNode != null)
                FormatZhdjjsbXml((XmlElement)zhdjNode, doc);

            //工程造价汇总
            XmlNode gchzNode = xmNode.SelectSingleNode("工程造价汇总");
            if (gchzNode != null)
                FormatGczjhzXml((XmlElement)gchzNode, doc);

            // 需评审的材料及设备汇总
            XmlNode psclNode = xmNode.SelectSingleNode("需评审的材料及设备汇总");
            if (psclNode != null)
                FormatPsclXml((XmlElement)psclNode, doc);


            // 工程数据结构
            XmlNode gcjgNode = xmNode.SelectSingleNode("工程数据结构");
            if (gcjgNode != null)
            {
                foreach (XmlNode cNode in gcjgNode.ChildNodes)
                {
                    if (cNode is XmlElement)
                    {
                        switch (cNode.Name)
                        {
                            case "单项工程":
                                ForamtDxgcXml((XmlElement)cNode, cjzData, doc, option);
                                break;
                            case "单位工程":
                                FormatDwgcXml((XmlElement)cNode, cjzData, doc, option);
                                break;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 序列化总项目信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="cjzData"></param>
        /// <param name="doc"></param>
        private void FormatSjDocData(XmlElement node, CjzData cjzData, DocData doc)
        {
            cjzData.projectName = node.GetAttribute("项目名称");      // 项目名称
            cjzData.projectCode = node.GetAttribute("项目编号");      // 项目编号
            cjzData.bidTitle = node.GetAttribute("标段名称");      // 标段名称
            cjzData.constructionUnit = node.GetAttribute("建设单位");      // 建设单位
            cjzData.projectAddress = node.GetAttribute("工程地点");      // 工程地点
            cjzData.projectScale = node.GetAttribute("工程规模");      // 工程规模
            cjzData.standardName = node.GetAttribute("标准名称");      // 标准名称
            cjzData.standardVersion = node.GetAttribute("版本号");      // 版本号
            cjzData.calcMethod = node.GetAttribute("计价方式");      // 计价标准
            cjzData.dataType = node.GetAttribute("数据类型");      // 数据类型
            cjzData.listPriceRule = node.GetAttribute("清单计价规则");      // 清单计价规则
            cjzData.fixedPriceRul = node.GetAttribute("定额计价规则");      // 定额计价规则
            cjzData.writeDate = node.GetAttribute("编制日期");      // 定额计价规则


            if (new[] { "3", "4", "5" }.Contains(cjzData.dataType))  // 招标清单，招标控制价，招标控制价不含组价
            {
                XmlNode fynode = node.SelectSingleNode("//建设项目/工程总信息/招标信息");
                if (fynode != null)
                    cjzData.projectPrice = CommUtils.GetAttributeDouble((XmlElement)fynode, "招标控制价");
            }
            else
            {
                XmlNode fynode = node.SelectSingleNode("//建设项目/工程总信息/投标信息");
                if (fynode != null)
                    cjzData.projectPrice = CommUtils.GetAttributeDouble((XmlElement)fynode, "投标报价");
            }
        }
        /// <summary>
        /// 序列化工程总信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatGczxxXml(XmlElement node, DocData doc)
        {
            XmxxTable tbl = new XmxxTable();
            tbl.tableName = "工程总信息";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "内容", "内容", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            AppendGcxxRecByNodeAttribS(node, tbl, null);

            XmlNode zbxx = node.SelectSingleNode("招标信息");
            if (zbxx != null)
            {
                XmxxRec zbxxRec = AppendGcxxRecByXpath(node, "招标信息", tbl, null);
                AppendGcxxRecByNodeAttribS((XmlElement)zbxx, tbl, zbxxRec);

                XmlNode zbcs = zbxx.SelectSingleNode("招标参数");
                if (zbcs != null)
                {
                    XmxxRec zbcsRec = AppendGcxxRecByXpath((XmlElement)zbxx, "招标参数", tbl, zbxxRec);
                    AppendGcxxRecByNodeAttribS((XmlElement)zbcs, tbl, zbcsRec);

                    XmlNodeList qtcslist = zbcs.SelectNodes("其他参数");
                    foreach (XmlNode qtcs in qtcslist)
                    {
                        XmxxRec qtcsRec = tbl.NewRecord() as XmxxRec;
                        qtcsRec.recDeep = zbcsRec.recDeep + 1;
                        qtcsRec.SetValueByColName("名称", "其他参数");
                        qtcsRec.SetValueByColName("内容", qtcs.Attributes.GetNamedItem("内容").Value);
                        tbl.recList.Add(qtcsRec);
                    }

                    XmlNode gffl = zbcs.SelectSingleNode("规费费率");
                    if (gffl != null)
                    {
                        XmxxRec gfflRec = AppendGcxxRecByXpath((XmlElement)zbcs, "规费费率", tbl, zbcsRec);
                        AppendGcxxRecByNodeAttribS((XmlElement)gffl, tbl, gfflRec);
                    }
                }
            }

            XmlNode tbxx = node.SelectSingleNode("投标信息");
            if (tbxx != null)
            {
                XmxxRec tbxxRec = AppendGcxxRecByXpath(node, "投标信息", tbl, null);
                AppendGcxxRecByNodeAttribS((XmlElement)tbxx, tbl, tbxxRec);
            }

            XmlNode jsxx = node.SelectSingleNode("投标信息/结算信息");
            if (jsxx != null)
            {
                XmxxRec jsxxRec = AppendGcxxRecByXpath(node, "投标信息/结算信息", tbl, null);
                AppendGcxxRecByNodeAttribS((XmlElement)jsxx, tbl, jsxxRec);
            }

            XmlNode qtxx = node.SelectSingleNode("其他信息");
            if (qtxx != null)
            {
                XmxxRec qtxxRec = AppendGcxxRecByXpath(node, "其他信息", tbl, null);

                XmlNodeList qtxxlist = qtxx.SelectNodes("其他信息项");
                foreach (XmlNode qtxxnode in qtxxlist)
                {
                    XmxxRec qtxxRec1 = tbl.NewRecord() as XmxxRec;
                    qtxxRec1.recDeep = qtxxRec.recDeep + 1;
                    qtxxRec1.SetValueByColName("名称", qtxxnode.Attributes.GetNamedItem("名称").Value);
                    qtxxRec1.SetValueByColName("内容", qtxxnode.Attributes.GetNamedItem("内容").Value);
                    tbl.recList.Add(qtxxRec1);
                }
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 根据xpath增加工程信息数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="xpath"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        /// <returns></returns>
        public XmxxRec AppendGcxxRecByXpath(XmlElement node, string xpath, XmxxTable tbl, XmxxRec pRec)
        {
            XmxxRec ret = null;
            XmlNode node1 = node.SelectSingleNode(xpath);
            if (node1 != null)
            {
                ret = tbl.NewRecord() as XmxxRec;
                if (pRec != null)
                    ret.recDeep = pRec.recDeep + 1;
                else
                    ret.recDeep = 1;

                ret.SetValueByColName("名称", node1.Name);
                if (node1 is XmlAttribute)
                    ret.SetValueByColName("内容", node1.Value);
                tbl.recList.Add(ret);
            }
            return ret;
        }
        /// <summary>
        /// 增加节点所有属性到工程信息表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        public void AppendGcxxRecByNodeAttribS(XmlElement node, XmxxTable tbl, XmxxRec pRec)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                XmxxRec rec = tbl.NewRecord() as XmxxRec;
                if (pRec != null)
                    rec.recDeep = pRec.recDeep + 1;
                else
                    rec.recDeep = 1;

                rec.SetValueByColName("名称", attr.Name);
                rec.SetValueByColName("内容", attr.Value);
                tbl.recList.Add(rec);

            }
        }
        /// <summary>
        /// 序列化系统信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatXtxxXml(XmlElement node, DocData doc)
        {
            XmxxTable tbl = new XmxxTable();
            tbl.tableName = "系统信息";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "内容", "内容", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNode yjxx = node.SelectSingleNode("硬件信息");
            if (yjxx != null)
            {
                XmxxRec yjxxRec = AppendGcxxRecByXpath(node, "硬件信息", tbl, null);
                AppendGcxxRecByNodeAttribS((XmlElement)yjxx, tbl, yjxxRec);
            }

            XmlNode rjxx = node.SelectSingleNode("软件信息");
            if (rjxx != null)
            {
                XmxxRec rjxxRec = AppendGcxxRecByXpath(node, "硬件信息", tbl, null);
                AppendGcxxRecByNodeAttribS((XmlElement)rjxx, tbl, rjxxRec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化定额综合单价计算程序
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatZhdjjsbXml(XmlElement node, DocData doc)
        {
            DjjsbTable tbl = new DjjsbTable();
            tbl.tableName = "定额综合单价计算程序";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "Id", "Id", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "名称", "名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.AddNewDField(1, "项目名称", "项目名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewDField(2, "计算公式", "计算公式", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewDField(3, "计算公式说明", "计算公式说明", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewDField(4, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewDField(5, "备注", "备注", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewDField(6, "行变量", "行变量", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewDField(7, "费用类别", "费用类别", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList djjsbList = node.SelectNodes("综合单价计算程序文件");
            foreach (XmlNode djjsbNode in djjsbList)
            {
                XmlElement djjsbElement = (XmlElement)djjsbNode;
                DjjsbRec djjsbRec = tbl.NewRecord() as DjjsbRec;
                djjsbRec.recDeep = 1;
                djjsbRec.SetValueByColName("Id", djjsbElement.GetAttribute("ID"));
                djjsbRec.SetValueByColName("名称", djjsbElement.GetAttribute("名称"));
                djjsbRec.SetValueByColName("备注", djjsbElement.GetAttribute("备注"));
                tbl.recList.Add(djjsbRec);

                XmlNodeList fyDataList = djjsbElement.SelectNodes("计算程序表");
                foreach (XmlNode fyDataNode in fyDataList)
                {
                    XmlElement fyDataElement = (XmlElement)fyDataNode;
                    DjjsMxRec fyDataRec = tbl.NewDRecord() as DjjsMxRec;
                    fyDataRec.recDeep = 1;
                    fyDataRec.SetValueByColName("序号", fyDataElement.GetAttribute("序号"));
                    fyDataRec.SetValueByColName("项目名称", fyDataElement.GetAttribute("项目名称"));
                    fyDataRec.SetValueByColName("计算公式", fyDataElement.GetAttribute("计算公式"));
                    fyDataRec.SetValueByColName("计算公式说明", fyDataElement.GetAttribute("计算公式说明"));
                    fyDataRec.SetValueByColName("费率", fyDataElement.GetAttribute("费率"));
                    fyDataRec.SetValueByColName("备注", fyDataElement.GetAttribute("备注"));
                    fyDataRec.SetValueByColName("行变量", fyDataElement.GetAttribute("行变量"));
                    fyDataRec.SetValueByColName("费用类别", fyDataElement.GetAttribute("费用类别"));
                    djjsbRec.childList.Add(fyDataRec);
                }
            };

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化工程造价汇总表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatGczjhzXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "工程造价汇总";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);

            foreach (XmlAttribute attr in node.Attributes)
            {
                FyxmRec rec = tbl.NewRecord() as FyxmRec;

                rec.recDeep = 1;

                rec.SetValueByColName("名称", attr.Name);
                rec.SetValueByColName("金额", attr.Value);
                tbl.recList.Add(rec);

            }

            XmlNodeList qtfyList = node.SelectNodes("其他费用");
            foreach (XmlNode qtfyNode in qtfyList)
            {
                XmlElement qtfyElement = (XmlElement)qtfyNode;
                FyxmRec qtfyRec = tbl.NewRecord() as FyxmRec;
                qtfyRec.recDeep = 1;
                qtfyRec.SetValueByColName("名称", qtfyElement.GetAttribute("费用名称"));
                qtfyRec.SetValueByColName("金额", qtfyElement.GetAttribute("金额"));
                tbl.recList.Add(qtfyRec);
            };

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化需评审的材料及设备汇总
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatPsclXml(XmlElement node, DocData doc)
        {
            ClxmTable tbl = new ClxmTable();
            tbl.tableName = "需评审的材料及设备汇总";
            doc.tableList.Add(tbl);


            tbl.AddNewField(1, "招标编码", "招标编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "关联材料代码", "关联材料代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "材料名称", "材料名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "规格型号", "规格型号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(5, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(6, "材料单价", "材料单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "产地", "产地", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(8, "厂家", "厂家", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(9, "品种", "品种", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(10, "质量档次", "质量档次", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(11, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("需评审的材料及设备明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                ClxmRec clRec = tbl.NewRecord() as ClxmRec;
                clRec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                clRec.SetValueByColName("招标编码", cElement.GetAttribute("招标编码"));
                clRec.SetValueByColName("关联材料代码", cElement.GetAttribute("关联材料代码"));
                clRec.SetValueByColName("材料名称", cElement.GetAttribute("材料名称"));
                clRec.SetValueByColName("规格型号", cElement.GetAttribute("规格型号"));
                clRec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                clRec.SetValueByColName("材料单价", cElement.GetAttribute("材料单价"));
                clRec.SetValueByColName("产地", cElement.GetAttribute("产地"));
                clRec.SetValueByColName("厂家", cElement.GetAttribute("厂家"));
                clRec.SetValueByColName("品种", cElement.GetAttribute("品种"));
                clRec.SetValueByColName("质量档次", cElement.GetAttribute("质量档次"));
                clRec.SetValueByColName("备注", cElement.GetAttribute("备注"));

                tbl.recList.Add(clRec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化单项工程
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sjDoc"></param>
        /// <param name="pDoc"></param>
        /// <param name="stg"></param>
        private void ForamtDxgcXml(XmlElement node, CjzData cjzData, DocData pDoc, int option)
        {
            DocData doc = new DocData();
            doc.docLevel = pDoc.docLevel + 1;
            doc.docName = node.GetAttribute("名称");      // 工程树名称
            doc.docType = "DXGC";      // 工程标识
            doc.parentUid = pDoc.UniqId;
            cjzData.docList.Add(doc);

            string relFile = node.GetAttribute("文件名称");
            if (File.Exists(tmppath + relFile))
            {
                XmlDocument mainXml = new XmlDocument();
                byte[] fileData = System.IO.File.ReadAllBytes(tmppath + relFile);
                string fileStr = Encoding.UTF8.GetString(fileData);
                mainXml.LoadXml(fileStr);
                FormatDxgcDataXml(mainXml.DocumentElement, doc);
            }

            // 处理子工程项目
            foreach (XmlNode cNode in node.ChildNodes)
            {
                if (cNode is XmlElement)
                {
                    switch (cNode.Name)
                    {
                        case "单项工程":
                            ForamtDxgcXml((XmlElement)cNode, cjzData, doc, option);
                            break;
                        case "单位工程":
                            FormatDwgcXml((XmlElement)cNode, cjzData, doc, option);
                            break;
                    }
                }
            };
        }
        /// <summary>
        /// 序列化单项工程信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDxgcDataXml(XmlElement node, DocData doc)
        {
            //doc.docName = node.GetAttribute("工程名称");      // 工程名称
            doc.docScale = node.GetAttribute("工程规模");      // 工程规模
            doc.docCategory = node.GetAttribute("工程类别");      // 工程类别

            // 单项工程造价汇总
            XmlNode gchzNode = node.SelectSingleNode("单项工程造价汇总");
            if (gchzNode != null)
                FormatDxGchzXml((XmlElement)gchzNode, doc);

            // 工程概况及特征
            //工程概况
            XmlNode gcgkNode = node.SelectSingleNode("工程概况及特征/工程概况");
            if (gcgkNode != null)
                FormatDwGcgkXml((XmlElement)gcgkNode, doc);

            //单位工程特征
            XmlNode gctzNode = node.SelectSingleNode("工程概况及特征/单位工程特征");
            if (gctzNode != null)
                FormatDxGctzXml((XmlElement)gctzNode, doc);

            //其他信息
            XmlNode qtNode = node.SelectSingleNode("其他信息");
            if (qtNode != null)
                FormatDxQtxxXml((XmlElement)qtNode, doc);
        }
        /// <summary>
        /// 序列化单项工程造价汇总
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDxGchzXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "单项工程造价汇总";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);

            foreach (XmlAttribute attr in node.Attributes)
            {
                FyxmRec rec = tbl.NewRecord() as FyxmRec;

                rec.recDeep = 1;

                rec.SetValueByColName("名称", attr.Name);
                rec.SetValueByColName("金额", attr.Value);
                tbl.recList.Add(rec);
            }

            XmlNodeList qtfyList = node.SelectNodes("其他费用");
            foreach (XmlNode qtfyNode in qtfyList)
            {
                XmlElement qtfyElement = (XmlElement)qtfyNode;
                FyxmRec qtfyRec = tbl.NewRecord() as FyxmRec;
                qtfyRec.recDeep = 1;
                qtfyRec.SetValueByColName("名称", qtfyElement.GetAttribute("费用名称"));
                qtfyRec.SetValueByColName("金额", qtfyElement.GetAttribute("金额"));
                tbl.recList.Add(qtfyRec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化单项工程工程概况
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDwGcgkXml(XmlElement node, DocData doc)
        {
            XmxxTable tbl = new XmxxTable();
            tbl.tableName = "工程概况";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "编码", "编码", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "内容", "内容", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("概况明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                XmxxRec rec = tbl.NewRecord() as XmxxRec;
                rec.recDeep = 1;
                rec.SetValueByColName("编码", cElement.GetAttribute("编码"));
                rec.SetValueByColName("名称", cElement.GetAttribute("名称"));
                rec.SetValueByColName("内容", cElement.GetAttribute("内容"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化单位工程特征
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDxGctzXml(XmlElement node, DocData doc)
        {
            XmxxTable tbl = new XmxxTable();
            tbl.tableName = "单位工程特征";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "编码", "编码", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "内容", "内容", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("单位工程特征明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                XmxxRec rec = tbl.NewRecord() as XmxxRec;
                rec.recDeep = 1;
                rec.SetValueByColName("编码", cElement.GetAttribute("编码"));
                rec.SetValueByColName("名称", cElement.GetAttribute("名称"));
                rec.SetValueByColName("内容", cElement.GetAttribute("内容"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化其他信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDxQtxxXml(XmlElement node, DocData doc)
        {
            XmxxTable tbl = new XmxxTable();
            tbl.tableName = "其他信息";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "内容", "内容", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("其他信息项");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                XmxxRec rec = tbl.NewRecord() as XmxxRec;
                rec.recDeep = 1;
                rec.SetValueByColName("名称", cElement.GetAttribute("名称"));
                rec.SetValueByColName("内容", cElement.GetAttribute("内容"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化单位工程
        /// </summary>
        /// <param name="node"></param>
        /// <param name="sjDoc"></param>
        /// <param name="pDoc"></param>
        /// <param name="stg"></param>
        private void FormatDwgcXml(XmlElement node, CjzData cjzData, DocData pDoc, int option)
        {
            DocData doc = new DocData();
            doc.docLevel = pDoc.docLevel + 1;
            doc.docName = node.GetAttribute("名称");      // 工程树名称
            doc.docType = "DWGC";      // 工程标识
            doc.parentUid = pDoc.UniqId;
            cjzData.docList.Add(doc);

            string relFile = node.GetAttribute("文件名称");
            if (File.Exists(tmppath + relFile))
            {
                XmlDocument mainXml = new XmlDocument();
                byte[] fileData = System.IO.File.ReadAllBytes(tmppath + relFile);
                string fileStr = Encoding.UTF8.GetString(fileData);
                mainXml.LoadXml(fileStr);
                FormatDwgcDataXml(mainXml.DocumentElement, doc, option);
            }
        }
        /// <summary>
        /// 序列化单位工程数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDwgcDataXml(XmlElement node, DocData doc, int option)
        {
            //doc.docName = node.GetAttribute("工程名称");      // 工程名称
            doc.docScale = node.GetAttribute("工程规模");      // 工程规模
            doc.docCategory = node.GetAttribute("工程类别");      // 工程类别
            if (option == 2)
            {
                //分部分项清单
                XmlNode fbfxqdNode = node.SelectSingleNode("分部分项清单");
                if (fbfxqdNode != null)
                    FormatFbfxqdXml((XmlElement)fbfxqdNode, doc);

                //工料机汇总
                XmlNode rcjNode = node.SelectSingleNode("工料机汇总");
                if (rcjNode != null)
                    FormatrcjhzXml((XmlElement)rcjNode, doc);
            }
            else
            {
                //单位工程信息
                XmlNode dwgcxxNode = node.SelectSingleNode("单位工程信息");
                if (dwgcxxNode != null)
                    FormatDwgcxxXml((XmlElement)dwgcxxNode, doc);

                //单位工程造价汇总
                XmlNode dwgchzNode = node.SelectSingleNode("单位工程造价汇总");
                if (dwgchzNode != null)
                    FormatDwgchzXml((XmlElement)dwgchzNode, doc);

                //分部分项清单
                XmlNode fbfxqdNode = node.SelectSingleNode("分部分项清单");
                if (fbfxqdNode != null)
                    FormatFbfxqdXml((XmlElement)fbfxqdNode, doc);

                //措施项目清单
                //总价措施清单表
                XmlNode zjcsqdNode = node.SelectSingleNode("措施项目清单/总价措施清单表");
                if (zjcsqdNode != null)
                    FormatZjcsqdXml((XmlElement)zjcsqdNode, doc);

                //单价措施清单表
                XmlNode djcsqdNode = node.SelectSingleNode("措施项目清单/单价措施清单表");
                if (djcsqdNode != null)
                    FormatDjcsqdXml((XmlElement)djcsqdNode, doc);

                //需评审材料及设备的清单消耗量
                XmlNode psclqdgclNode = node.SelectSingleNode("需评审材料及设备的清单消耗量");
                if (psclqdgclNode != null)
                    FormatPsclqdgclXml((XmlElement)psclqdgclNode, doc);

                //其他项目清单
                XmlNode qtxmNode = node.SelectSingleNode("其他项目清单");
                if (qtxmNode != null)
                    FormateQtxmqdXml((XmlElement)qtxmNode, doc);

                //暂列金额
                XmlNode zljeNode = node.SelectSingleNode("其他项目清单/暂列金额");
                if (zljeNode != null)
                    FormatzljeXml((XmlElement)zljeNode, doc);

                //材料及设备暂估价
                XmlNode clzgNode = node.SelectSingleNode("其他项目清单/材料及设备暂估价");
                if (clzgNode != null)
                    FormatClzgXml((XmlElement)clzgNode, doc);

                //专业工程暂估价
                XmlNode zyzgNode = node.SelectSingleNode("其他项目清单/专业工程暂估价");
                if (zyzgNode != null)
                    FormatZyzgXml((XmlElement)zyzgNode, doc);

                //计日工
                XmlNode lxxmNode = node.SelectSingleNode("其他项目清单/计日工");
                if (lxxmNode != null)
                    FormatLxxmXml((XmlElement)lxxmNode, doc);

                //总承包服务费
                XmlNode zcbfwfNode = node.SelectSingleNode("其他项目清单/总承包服务费");
                if (zcbfwfNode != null)
                    FormatZcbfwfXml((XmlElement)zcbfwfNode, doc);

                //索赔与现场签证
                //索赔计价表
                XmlNode spbNode = node.SelectSingleNode("其他项目清单/索赔与现场签证/索赔计价表");
                if (spbNode != null)
                    FormatSpbXml((XmlElement)spbNode, doc);

                //现场签证计价表
                XmlNode qzbNode = node.SelectSingleNode("其他项目清单/索赔与现场签证/现场签证计价表");
                if (qzbNode != null)
                    FormatQzbXml((XmlElement)qzbNode, doc);

                //其他
                XmlNode qtNode = node.SelectSingleNode("其他项目清单/其他");
                if (qtNode != null)
                    FormatQtXml((XmlElement)qtNode, doc);

                //规费和税金清单
                XmlNode gfNode = node.SelectSingleNode("规费和税金清单");
                if (gfNode != null)
                    FormatGfsjXml((XmlElement)gfNode, doc);

                //工料机汇总
                XmlNode rcjNode = node.SelectSingleNode("工料机汇总");
                if (rcjNode != null)
                    FormatrcjhzXml((XmlElement)rcjNode, doc);

                //发包人提供材料及设备
                XmlNode fbrclNode = node.SelectSingleNode("发包人提供材料及设备");
                if (fbrclNode != null)
                    FormatFbrclXml((XmlElement)fbrclNode, doc);

                //承包人采购主要材料和设备
                //造价信息差额法
                XmlNode zjxxcefNode = node.SelectSingleNode("承包人采购主要材料和设备/造价信息差额法");
                if (zjxxcefNode != null)
                    FormatzjxxcefXml((XmlElement)zjxxcefNode, doc);

                //价格指数差额法
                XmlNode jezscefNode = node.SelectSingleNode("承包人采购主要材料和设备/价格指数差额法");
                if (jezscefNode != null)
                    FormatJgzscefXml((XmlElement)jezscefNode, doc);
            }


        }
        /// <summary>
        /// 序列化单位工程信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDwgcxxXml(XmlElement node, DocData doc)
        {
            XmxxTable tbl = new XmxxTable();
            tbl.tableName = "单位工程信息";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "内容", "内容", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("信息项");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                XmxxRec rec = tbl.NewRecord() as XmxxRec;
                rec.recDeep = 1;
                rec.SetValueByColName("名称", cElement.GetAttribute("名称"));
                rec.SetValueByColName("内容", cElement.GetAttribute("内容"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化单位工程造价汇总
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDwgchzXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "单位工程造价汇总";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "排序号", "排序号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "项目名称", "项目名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "计算公式说明", "计算公式说明", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "计算公式", "计算公式", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(5, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(8, "行变量", "行变量", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(9, "费用类别", "费用类别", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("单位工程造价行");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));      // 序号
                rec.SetValueByColName("排序号", cElement.GetAttribute("排序号"));
                rec.SetValueByColName("项目名称", cElement.GetAttribute("项目名称"));
                rec.SetValueByColName("计算公式说明", cElement.GetAttribute("计算公式说明"));
                rec.SetValueByColName("计算公式", cElement.GetAttribute("计算公式"));
                rec.SetValueByColName("费率", cElement.GetAttribute("费率"));
                rec.SetValueByColName("金额", cElement.GetAttribute("金额"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                rec.SetValueByColName("行变量", cElement.GetAttribute("行变量"));
                rec.SetValueByColName("费用类别", cElement.GetAttribute("费用类别"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化分部分项清单
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatFbfxqdXml(XmlElement node, DocData doc)
        {
            QdxmTable tbl = new QdxmTable();
            tbl.tableName = "分部分项清单";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "项目编码", "项目编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "项目名称", "项目名称", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "计量单位", "计量单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "工程量", "工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            tbl.AddNewField(5, "工程量计算式", "工程量计算式", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            tbl.AddNewField(6, "综合单价", "综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "综合合价", "综合合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "人工费", "人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(9, "人工费合价", "人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(10, "材料费", "材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(11, "材料费合价", "材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(12, "施工机具使用费", "施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(13, "施工机具使用费合价", "施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(14, "材料暂估单价", "材料暂估单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(15, "材料暂估合价", "材料暂估合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(16, "人工费调整费率", "人工费调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(17, "人工费调整单价", "人工费调整单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(18, "人工费调整合价", "人工费调整合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(19, "综合费率", "综合费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(20, "综合费单价", "综合费单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(21, "综合费合价", "综合费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(22, "定额单价", "定额单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(23, "定额合价", "定额合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(24, "定额人工费", "定额人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(25, "定额人工费合价", "定额人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(26, "定额材料费", "定额材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(27, "定额材料费合价", "定额材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(28, "定额施工机具使用费", "定额施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(29, "定额施工机具使用费合价", "定额施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(30, "单价构成文件ID", "单价构成文件ID", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(31, "主要清单标志", "主要清单标志", 6, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(32, "清单类别", "清单类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(33, "定额专业类别", "定额专业类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(34, "项目特征", "项目特征", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(35, "工作内容", "工作内容", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(36, "换算描述", "换算描述", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(37, "费用类别", "费用类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(38, "备注", "备注", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(39, "已标价工程量", "已标价工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(40, "已标价综合单价", "已标价综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(41, "已标价人工费", "已标价人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(42, "已标价材料费", "已标价材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(43, "已标价施工机具使用费", "已标价施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(44, "已标价综合费", "已标价综合费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(45, "其他信息", "其他信息", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);


            tbl.AddNewDField(1, "关联材料代码", "关联材料代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewDField(2, "消耗量", "消耗量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewDField(3, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewDField(4, "不计价", "不计价", 15, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewDField(5, "数量计算方式", "数量计算方式", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");
            tbl.zgj = CommUtils.GetAttributeDouble(node, "材料及设备暂估价合计");

            foreach (XmlNode cNode in node.ChildNodes)
            {
                if (cNode is XmlElement)
                {
                    switch (cNode.Name)
                    {
                        case "清单分部":
                            AppendQdfbRec((XmlElement)cNode, tbl, null);
                            break;
                        case "清单项目":
                            AppendQdxmRec((XmlElement)cNode, tbl, null);
                            break;
                    }
                }
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化清单分部数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        private void AppendQdfbRec(XmlElement node, QdxmTable tbl, QdxmRec pRec)
        {
            QdxmRec rec = tbl.NewRecord() as QdxmRec;
            if (pRec != null)
                rec.recDeep = pRec.recDeep + 1;
            else
                rec.recDeep = 1;
            rec.SetValueByColName("项目编码", node.GetAttribute("项目编码"));
            rec.SetValueByColName("项目名称", node.GetAttribute("项目名称"));
            rec.SetValueByColName("备注", node.GetAttribute("备注"));
            rec.qdtype = (int)_qdType.qtFbxm;
            tbl.recList.Add(rec);

            XmlElement fbfyNode = (XmlElement)node.SelectSingleNode("分部费用");
            if (fbfyNode != null)
                AppendFbfyToRec(fbfyNode, rec);


            // 处理子分部
            foreach (XmlNode cNode in node.ChildNodes)
            {
                if (cNode is XmlElement)
                {
                    switch (cNode.Name)
                    {
                        case "清单分部":
                            AppendQdfbRec((XmlElement)cNode, tbl, rec);
                            break;
                        case "清单项目":
                            AppendQdxmRec((XmlElement)cNode, tbl, rec);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 序列化分部费用
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        private void AppendFbfyToRec(XmlElement node, QdxmRec rec)
        {
            rec.SetValueByColName("合价", node.GetAttribute("综合合价"));
            rec.SetValueByColName("人工费合价", node.GetAttribute("人工费合价"));
            rec.SetValueByColName("施工机具使用费合价", node.GetAttribute("施工机具使用费合价"));
            rec.SetValueByColName("材料费合价", node.GetAttribute("材料费合价"));
            rec.SetValueByColName("人工费调整合价", node.GetAttribute("人工费调整合价"));
            rec.SetValueByColName("综合费合价", node.GetAttribute("综合费合价"));
            rec.SetValueByColName("定额人工费合价", node.GetAttribute("定额人工费合价"));
            rec.SetValueByColName("定额材料费合价", node.GetAttribute("定额材料费合价"));
            rec.SetValueByColName("定额施工机具使用费合价", node.GetAttribute("定额施工机具使用费合价"));
            rec.SetValueByColName("材料暂估单价", node.GetAttribute("材料暂估合价"));
        }
        /// <summary>
        /// 序列化清单项目
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        private void AppendQdxmRec(XmlElement node, QdxmTable tbl, QdxmRec pRec)
        {
            QdxmRec rec = tbl.NewRecord() as QdxmRec;
            if (pRec != null)
                rec.recDeep = pRec.recDeep + 1;
            else
                rec.recDeep = 1;

            rec.SetValueByColName("项目编码", node.GetAttribute("项目编码"));
            rec.SetValueByColName("项目名称", node.GetAttribute("项目名称"));
            rec.SetValueByColName("计量单位", node.GetAttribute("计量单位"));
            rec.SetValueByColName("综合单价", node.GetAttribute("综合单价"));
            rec.SetValueByColName("工程量", node.GetAttribute("工程量"));
            rec.SetValueByColName("工程量计算式", node.GetAttribute("工程量计算式"));
            rec.SetValueByColName("人工费调整费率", node.GetAttribute("人工费调整费率"));
            rec.SetValueByColName("备注", node.GetAttribute("备注"));
            rec.SetValueByColName("主要清单标志", node.GetAttribute("主要清单标志"));
            rec.SetValueByColName("清单类别", node.GetAttribute("清单类别"));
            rec.SetValueByColName("费用类别", node.GetAttribute("费用类别"));
            rec.SetValueByColName("已标价工程量", node.GetAttribute("已标价工程量"));
            rec.SetValueByColName("已标价综合单价", node.GetAttribute("已标价综合单价"));
            rec.SetValueByColName("已标价人工费", node.GetAttribute("已标价人工费"));
            rec.SetValueByColName("已标价材料费", node.GetAttribute("已标价材料费"));
            rec.SetValueByColName("已标价施工机具使用费", node.GetAttribute("已标价施工机具使用费"));
            rec.SetValueByColName("已标价综合费", node.GetAttribute("已标价综合费"));
            rec.SetValueByColName("其他信息", node.GetAttribute("其他信息"));
            rec.qdtype = (int)_qdType.qtQdxm;
            tbl.recList.Add(rec);

            XmlElement fyzcNode = (XmlElement)node.SelectSingleNode("费用组成");
            if (fyzcNode != null)
                AppendFyzcToRec(fyzcNode, rec);

            XmlElement xmtzNode = (XmlElement)node.SelectSingleNode("项目特征");
            if (xmtzNode != null)
            {
                rec.SetValueByColName("项目特征", "");
                XmlNodeList nodeList = xmtzNode.SelectNodes("特征明细");
                foreach (XmlNode cNode in nodeList)
                {
                    XmlElement cElement = (XmlElement)cNode;
                    string ztvlue = cElement.GetAttribute("内容");
                    if (!string.IsNullOrEmpty(ztvlue))
                    {
                        if (string.IsNullOrEmpty(rec.GetValueByColName("项目特征")))
                            rec.SetValueByColName("项目特征", ztvlue);
                        else
                            rec.SetValueByColName("项目特征", rec.GetValueByColName("项目特征") + "\r\n" + ztvlue);
                    }
                }
            }
            XmlElement gznrNode = (XmlElement)node.SelectSingleNode("工作内容");
            if (gznrNode != null)
            {
                rec.SetValueByColName("工作内容", "");
                XmlNodeList nodeList = gznrNode.SelectNodes("内容明细");
                foreach (XmlNode cNode in nodeList)
                {
                    XmlElement cElement = (XmlElement)cNode;
                    string nrvalue = cElement.GetAttribute("内容");
                    if (!string.IsNullOrEmpty(nrvalue))
                    {
                        if (string.IsNullOrEmpty(rec.GetValueByColName("工作内容")))
                            rec.SetValueByColName("工作内容", nrvalue);
                        else
                            rec.SetValueByColName("工作内容", rec.GetValueByColName("工作内容") + "\r\n" + nrvalue);
                    }
                }
            }

            XmlElement zjnrNode = (XmlElement)node.SelectSingleNode("组价内容");
            if (zjnrNode != null)
            {
                XmlNodeList nodeList = zjnrNode.SelectNodes("定额子目");
                foreach (XmlNode cNode in nodeList)
                {
                    AppendDexmRec((XmlElement)cNode, tbl, rec);
                }
            }
        }
        /// <summary>
        /// 序列化费用组成
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        private void AppendFyzcToRec(XmlElement node, QdxmRec rec)
        {
            rec.SetValueByColName("人工费", node.GetAttribute("人工费"));
            rec.SetValueByColName("施工机具使用费", node.GetAttribute("施工机具使用费"));
            rec.SetValueByColName("材料费", node.GetAttribute("材料费"));
            rec.SetValueByColName("人工费合价", node.GetAttribute("人工费合价"));
            rec.SetValueByColName("施工机具使用费合价", node.GetAttribute("施工机具使用费合价"));
            rec.SetValueByColName("材料费合价", node.GetAttribute("材料费合价"));
            rec.SetValueByColName("人工费调整单价", node.GetAttribute("人工费调整单价"));
            rec.SetValueByColName("人工费调整合价", node.GetAttribute("人工费调整合价"));
            rec.SetValueByColName("综合费单价", node.GetAttribute("综合费单价"));
            rec.SetValueByColName("综合费合价", node.GetAttribute("综合费合价"));
            rec.SetValueByColName("定额人工费", node.GetAttribute("定额人工费"));
            rec.SetValueByColName("定额材料费", node.GetAttribute("定额材料费"));
            rec.SetValueByColName("定额施工机具使用费", node.GetAttribute("定额施工机具使用费"));
            rec.SetValueByColName("定额人工费合价", node.GetAttribute("定额人工费合价"));
            rec.SetValueByColName("定额材料费合价", node.GetAttribute("定额材料费合价"));
            rec.SetValueByColName("定额施工机具使用费合价", node.GetAttribute("定额施工机具使用费合价"));
            rec.SetValueByColName("材料暂估单价", node.GetAttribute("材料暂估单价"));
            rec.SetValueByColName("材料暂估合价", node.GetAttribute("材料暂估合价"));
        }
        /// <summary>
        /// 序列化定额项目
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        private void AppendDexmRec(XmlElement node, QdxmTable tbl, QdxmRec pRec)
        {
            QdxmRec rec = tbl.NewRecord() as QdxmRec;
            if (pRec != null)
                rec.recDeep = pRec.recDeep + 1;
            else
                rec.recDeep = 1;
            rec.SetValueByColName("项目编码", node.GetAttribute("定额编号"));
            rec.SetValueByColName("项目名称", node.GetAttribute("项目名称"));
            rec.SetValueByColName("计量单位", node.GetAttribute("计量单位"));
            rec.SetValueByColName("综合单价", node.GetAttribute("综合单价"));
            rec.SetValueByColName("工程量", node.GetAttribute("工程量"));
            rec.SetValueByColName("工程量计算式", node.GetAttribute("工程量计算式"));
            rec.SetValueByColName("综合合价", node.GetAttribute("综合合价"));
            rec.SetValueByColName("换算描述", node.GetAttribute("换算描述"));
            rec.SetValueByColName("人工费调整费率", node.GetAttribute("人工费调整费率"));
            rec.SetValueByColName("单价构成文件ID", node.GetAttribute("单价构成文件ID"));
            rec.SetValueByColName("备注", node.GetAttribute("备注"));
            rec.SetValueByColName("定额专业类别", node.GetAttribute("定额专业类别"));
            rec.SetValueByColName("其他信息", node.GetAttribute("其他信息"));
            rec.qdtype = (int)_qdType.qtDexm;
            tbl.recList.Add(rec);

            XmlElement fyzcNode = (XmlElement)node.SelectSingleNode("费用组成");
            if (fyzcNode != null)
                AppendFyzcToRec(fyzcNode, rec);

            XmlElement clmxNode = (XmlElement)node.SelectSingleNode("工料机组成");
            if (clmxNode != null)
                AppendClmxToDeRec(clmxNode, tbl, rec);
        }
        /// <summary>
        /// 序列化明细材料数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        private void AppendClmxToDeRec(XmlElement node, QdxmTable tbl, QdxmRec pRec)
        {
            XmlNodeList nodeList = node.SelectNodes("工料机含量");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                MxclRec rec = tbl.NewDRecord() as MxclRec;
                rec.recDeep = 1;
                rec.SetValueByColName("关联材料代码", cElement.GetAttribute("关联材料代码"));
                rec.SetValueByColName("消耗量", cElement.GetAttribute("消耗量"));
                rec.SetValueByColName("数量", cElement.GetAttribute("数量"));
                rec.SetValueByColName("数量计算方式", cElement.GetAttribute("数量计算方式"));
                rec.SetValueByColName("不计价", cElement.GetAttribute("不计价"));
                pRec.childList.Add(rec);
            }
        }
        /// <summary>
        /// 序列化总价措施清单表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatZjcsqdXml(XmlElement node, DocData doc)
        {
            QdxmTable tbl = new QdxmTable();
            tbl.tableName = "总价措施清单表";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "项目编码", "项目编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "项目名称", "项目名称", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "计量单位", "计量单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "工程量", "工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            tbl.AddNewField(5, "工程量计算式", "工程量计算式", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            tbl.AddNewField(6, "取费基础表达式", "取费基础表达式", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(7, "取费基础说明", "取费基础说明", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(8, "取费基础金额", "取费基础金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(9, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(10, "综合单价", "综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(11, "综合合价", "综合合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(12, "人工费", "人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(13, "人工费合价", "人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(14, "材料费", "材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(15, "材料费合价", "材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(16, "施工机具使用费", "施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(17, "施工机具使用费合价", "施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(18, "材料暂估单价", "材料暂估单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(19, "材料暂估合价", "材料暂估合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(20, "人工费调整费率", "人工费调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(21, "人工费调整单价", "人工费调整单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(22, "人工费调整合价", "人工费调整合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(23, "综合费率", "综合费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(24, "综合费单价", "综合费单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(25, "综合费合价", "综合费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(26, "定额单价", "定额单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(27, "定额合价", "定额合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(28, "定额人工费", "定额人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(29, "定额人工费合价", "定额人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(30, "定额材料费", "定额材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(31, "定额材料费合价", "定额材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(32, "定额施工机具使用费", "定额施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(33, "定额施工机具使用费合价", "定额施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(34, "单价构成文件ID", "单价构成文件ID", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(35, "主要清单标志", "主要清单标志", 6, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(36, "清单类别", "清单类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(37, "定额专业类别", "定额专业类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(38, "项目特征", "项目特征", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(39, "工作内容", "工作内容", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(40, "换算描述", "换算描述", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(41, "费用类别", "费用类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(42, "备注", "备注", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(43, "已标价工程量", "已标价工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(44, "已标价综合单价", "已标价综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(45, "已标价人工费", "已标价人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(46, "已标价材料费", "已标价材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(47, "已标价施工机具使用费", "已标价施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(48, "已标价综合费", "已标价综合费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(49, "其他信息", "其他信息", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(50, "按费率计取", "按费率计取", 15, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(51, "调整费率", "调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(52, "调整后金额", "调整后金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");
            tbl.zgj = CommUtils.GetAttributeDouble(node, "材料及设备暂估价合计");
            tbl.aqsgf = CommUtils.GetAttributeDouble(node, "安全文明施工费");

            foreach (XmlNode cNode in node.ChildNodes)
            {
                if (cNode is XmlElement)
                {
                    switch (cNode.Name)
                    {
                        case "措施分部":
                            AppendCsfbRec((XmlElement)cNode, tbl, null);
                            break;
                        case "措施清单项目":
                            AppendCsQdxmRec((XmlElement)cNode, tbl, null);
                            break;
                    }
                }
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化措施分部
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        private void AppendCsfbRec(XmlElement node, QdxmTable tbl, QdxmRec pRec)
        {
            QdxmRec rec = tbl.NewRecord() as QdxmRec;
            if (pRec != null)
                rec.recDeep = pRec.recDeep + 1;
            else
                rec.recDeep = 1;
            rec.SetValueByColName("项目编码", node.GetAttribute("项目编码"));
            rec.SetValueByColName("项目名称", node.GetAttribute("项目名称"));
            rec.SetValueByColName("备注", node.GetAttribute("备注"));
            rec.qdtype = (int)_qdType.qtFbxm;
            tbl.recList.Add(rec);

            XmlElement fbfyNode = (XmlElement)node.SelectSingleNode("分部费用");
            if (fbfyNode != null)
                AppendFbfyToRec(fbfyNode, rec);


            // 处理子分部
            foreach (XmlNode cNode in node.ChildNodes)
            {
                if (cNode is XmlElement)
                {
                    switch (cNode.Name)
                    {
                        case "措施分部":
                            AppendCsfbRec((XmlElement)cNode, tbl, rec);
                            break;
                        case "措施清单项目":
                            AppendCsQdxmRec((XmlElement)cNode, tbl, rec);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 序列化措施清单项目
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tbl"></param>
        /// <param name="pRec"></param>
        private void AppendCsQdxmRec(XmlElement node, QdxmTable tbl, QdxmRec pRec)
        {
            QdxmRec rec = tbl.NewRecord() as QdxmRec;
            if (pRec != null)
                rec.recDeep = pRec.recDeep + 1;
            else
                rec.recDeep = 1;
            rec.SetValueByColName("项目编码", node.GetAttribute("项目编码"));
            rec.SetValueByColName("项目名称", node.GetAttribute("项目名称"));
            rec.SetValueByColName("计量单位", node.GetAttribute("计量单位"));
            rec.SetValueByColName("综合单价", node.GetAttribute("综合单价"));
            rec.SetValueByColName("工程量", node.GetAttribute("工程量"));
            rec.SetValueByColName("工程量计算式", node.GetAttribute("工程量计算式"));
            rec.SetValueByColName("综合合价", node.GetAttribute("综合合价"));
            rec.SetValueByColName("人工费调整费率", node.GetAttribute("人工费调整费率"));
            rec.SetValueByColName("备注", node.GetAttribute("备注"));
            rec.SetValueByColName("主要清单标志", node.GetAttribute("主要清单标志"));
            rec.SetValueByColName("清单类别", node.GetAttribute("清单类别"));
            rec.SetValueByColName("费用类别", node.GetAttribute("费用类别"));
            rec.SetValueByColName("取费基础表达式", node.GetAttribute("取费基础表达式"));
            rec.SetValueByColName("取费基础说明", node.GetAttribute("取费基础说明"));
            rec.SetValueByColName("取费基础金额", node.GetAttribute("取费基础金额"));
            rec.SetValueByColName("费率", node.GetAttribute("费率"));
            rec.SetValueByColName("按费率计取", node.GetAttribute("按费率计取"));
            rec.SetValueByColName("已标价工程量", node.GetAttribute("已标价工程量"));
            rec.SetValueByColName("已标价综合单价", node.GetAttribute("已标价综合单价"));
            rec.SetValueByColName("已标价人工费", node.GetAttribute("已标价人工费"));
            rec.SetValueByColName("已标价材料费", node.GetAttribute("已标价材料费"));
            rec.SetValueByColName("已标价施工机具使用费", node.GetAttribute("已标价施工机具使用费"));
            rec.SetValueByColName("已标价综合费", node.GetAttribute("已标价综合费"));
            rec.SetValueByColName("其他信息", node.GetAttribute("其他信息"));
            rec.SetValueByColName("调整费率", node.GetAttribute("调整费率"));
            rec.SetValueByColName("调整后金额", node.GetAttribute("调整后金额"));
            rec.qdtype = (int)_qdType.qtQdxm;
            tbl.recList.Add(rec);

            XmlElement fyzcNode = (XmlElement)node.SelectSingleNode("费用组成");
            if (fyzcNode != null)
                AppendFyzcToRec(fyzcNode, rec);

            XmlElement xmtzNode = (XmlElement)node.SelectSingleNode("项目特征");
            if (xmtzNode != null)
            {
                rec.SetValueByColName("项目特征", "");
                XmlNodeList nodeList = xmtzNode.SelectNodes("特征明细");
                foreach (XmlNode cNode in nodeList)
                {
                    XmlElement cElement = (XmlElement)cNode;
                    string ztvlue = cElement.GetAttribute("内容");
                    if (!string.IsNullOrEmpty(ztvlue))
                    {
                        if (string.IsNullOrEmpty(rec.GetValueByColName("项目特征")))
                            rec.SetValueByColName("项目特征", ztvlue);
                        else
                            rec.SetValueByColName("项目特征", rec.GetValueByColName("项目特征") + "\r\n" + ztvlue);
                    }
                }
            }
            XmlElement gznrNode = (XmlElement)node.SelectSingleNode("工作内容");
            if (gznrNode != null)
            {
                rec.SetValueByColName("工作内容", "");
                XmlNodeList nodeList = gznrNode.SelectNodes("内容明细");
                foreach (XmlNode cNode in nodeList)
                {
                    XmlElement cElement = (XmlElement)cNode;
                    string nrvalue = cElement.GetAttribute("内容");
                    if (!string.IsNullOrEmpty(nrvalue))
                    {
                        if (string.IsNullOrEmpty(rec.GetValueByColName("工作内容")))
                            rec.SetValueByColName("工作内容", nrvalue);
                        else
                            rec.SetValueByColName("工作内容", rec.GetValueByColName("工作内容") + "\r\n" + nrvalue);
                    }
                }
            }

            XmlElement zjnrNode = (XmlElement)node.SelectSingleNode("组价内容");
            if (zjnrNode != null)
            {
                XmlNodeList nodeList = zjnrNode.SelectNodes("定额子目");
                foreach (XmlNode cNode in nodeList)
                {
                    AppendDexmRec((XmlElement)cNode, tbl, rec);
                }
            }
        }
        /// <summary>
        /// 序列化单价措施清单表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatDjcsqdXml(XmlElement node, DocData doc)
        {
            QdxmTable tbl = new QdxmTable();
            tbl.tableName = "单价措施清单表";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "项目编码", "项目编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "项目名称", "项目名称", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "计量单位", "计量单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "工程量", "工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            tbl.AddNewField(5, "工程量计算式", "工程量计算式", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            tbl.AddNewField(6, "综合单价", "综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "综合合价", "综合合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "人工费", "人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(9, "人工费合价", "人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(10, "材料费", "材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(11, "材料费合价", "材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(12, "施工机具使用费", "施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(13, "施工机具使用费合价", "施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(14, "材料暂估单价", "材料暂估单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(15, "材料暂估合价", "材料暂估合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(16, "人工费调整费率", "人工费调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(17, "人工费调整单价", "人工费调整单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(18, "人工费调整合价", "人工费调整合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(19, "综合费率", "综合费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(20, "综合费单价", "综合费单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(21, "综合费合价", "综合费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(22, "定额单价", "定额单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(23, "定额合价", "定额合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(24, "定额人工费", "定额人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(25, "定额人工费合价", "定额人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(26, "定额材料费", "定额材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(27, "定额材料费合价", "定额材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(28, "定额施工机具使用费", "定额施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(29, "定额施工机具使用费合价", "定额施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(30, "单价构成文件ID", "单价构成文件ID", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(31, "主要清单标志", "主要清单标志", 6, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(32, "清单类别", "清单类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(33, "定额专业类别", "定额专业类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(34, "项目特征", "项目特征", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(35, "工作内容", "工作内容", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(36, "换算描述", "换算描述", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(37, "费用类别", "费用类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(38, "备注", "备注", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(39, "已标价工程量", "已标价工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(40, "已标价综合单价", "已标价综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(41, "已标价人工费", "已标价人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(42, "已标价材料费", "已标价材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(43, "已标价施工机具使用费", "已标价施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(44, "已标价综合费", "已标价综合费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(45, "其他信息", "其他信息", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);


            tbl.AddNewDField(1, "关联材料代码", "关联材料代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewDField(2, "消耗量", "消耗量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewDField(3, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewDField(4, "不计价", "不计价", 15, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewDField(5, "数量计算方式", "数量计算方式", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");
            tbl.zgj = CommUtils.GetAttributeDouble(node, "材料及设备暂估价合计");

            foreach (XmlNode cNode in node.ChildNodes)
            {
                if (cNode is XmlElement)
                {
                    switch (cNode.Name)
                    {
                        case "措施分部":
                            AppendCsfbRec((XmlElement)cNode, tbl, null);
                            break;
                        case "措施清单项目":
                            AppendCsQdxmRec((XmlElement)cNode, tbl, null);
                            break;
                    }
                }
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 格式化需评审材料及设备的清单消耗量
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatPsclqdgclXml(XmlElement node, DocData doc)
        {
            QdxmTable tbl = new QdxmTable();
            tbl.tableName = "需评审材料及设备的清单消耗量";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "项目编码", "项目编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "项目名称", "项目名称", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "计量单位", "计量单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "工程量", "工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);

            tbl.AddNewDField(1, "招标编码", "招标编码", 15, (int)_alignType.atMiddle, (int)_dataType.dtInt, 0, false, true);
            tbl.AddNewDField(2, "关联材料代码", "关联材料代码", 15, (int)_alignType.atMiddle, (int)_dataType.dtInt, 0);
            tbl.AddNewDField(3, "材料名称", "材料名称", 15, (int)_alignType.atMiddle, (int)_dataType.dtInt, 0);
            tbl.AddNewDField(4, "规格型号", "规格型号", 15, (int)_alignType.atMiddle, (int)_dataType.dtInt, 0);
            tbl.AddNewDField(5, "单位", "单位", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewDField(6, "消耗量", "消耗量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewDField(7, "单价", "单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);

            XmlNodeList nodeList = node.SelectNodes("清单明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                QdxmRec rec = tbl.NewRecord() as QdxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("项目编码", cElement.GetAttribute("项目编码"));
                rec.SetValueByColName("项目名称", cElement.GetAttribute("项目名称"));
                rec.SetValueByColName("计量单位", cElement.GetAttribute("计量单位"));
                rec.SetValueByColName("工程量", cElement.GetAttribute("工程量"));
                rec.qdtype = (int)_qdType.qtQdxm;
                tbl.recList.Add(rec);

                XmlNodeList cNodeList = cNode.SelectNodes("材料及设备耗量明细");
                foreach (XmlNode ccNode in cNodeList)
                {
                    XmlElement ccElement = (XmlElement)ccNode;
                    MxclRec mxRec = tbl.NewDRecord() as MxclRec;
                    mxRec.recDeep = 1;

                    mxRec.SetValueByColName("招标编码", ccElement.GetAttribute("招标编码"));
                    mxRec.SetValueByColName("关联材料代码", ccElement.GetAttribute("关联材料代码"));
                    mxRec.SetValueByColName("材料名称", ccElement.GetAttribute("材料名称"));
                    mxRec.SetValueByColName("规格型号", ccElement.GetAttribute("规格型号"));
                    mxRec.SetValueByColName("单位", ccElement.GetAttribute("单位"));
                    mxRec.SetValueByColName("消耗量", ccElement.GetAttribute("消耗量"));
                    mxRec.SetValueByColName("单价", ccElement.GetAttribute("单价"));
                    tbl.recList.Add(mxRec);
                }
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化其他项目清单
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormateQtxmqdXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "其他项目清单";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            foreach (XmlNode cNode in node)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("名称", cElement.Name);
                rec.SetValueByColName("金额", cElement.GetAttribute("合计"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化暂列金额
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatzljeXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "暂列金额";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "单位", "单位", 28, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(4, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            XmlNodeList nodeList = node.SelectNodes("暂列金额明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("名称", cElement.GetAttribute("项目名称"));
                rec.SetValueByColName("单位", cElement.GetAttribute("计量单位"));
                rec.SetValueByColName("金额", cElement.GetAttribute("金额"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化材料及设备暂估价
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatClzgXml(XmlElement node, DocData doc)
        {
            ClxmTable tbl = new ClxmTable();
            tbl.tableName = "材料及设备暂估价";
            doc.tableList.Add(tbl);


            tbl.AddNewField(1, "招标编码", "招标编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "关联材料代码", "关联材料代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "材料名称", "材料名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "规格型号", "规格型号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(5, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(6, "暂估数量", "暂估数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "暂估单价", "暂估单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "暂估合价", "暂估合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(9, "确认数量", "确认数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(10, "差额单价", "差额单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(11, "差额合价", "差额合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(12, "产地", "产地", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(13, "厂家", "厂家", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(14, "品种", "品种", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(15, "质量档次", "质量档次", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(16, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("材料及设备暂估价明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                ClxmRec rec = tbl.NewRecord() as ClxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("招标编码", cElement.GetAttribute("招标编码"));
                rec.SetValueByColName("关联材料代码", cElement.GetAttribute("关联材料代码"));
                rec.SetValueByColName("材料名称", cElement.GetAttribute("材料名称"));
                rec.SetValueByColName("规格型号", cElement.GetAttribute("规格型号"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("暂估数量", cElement.GetAttribute("暂估数量"));
                rec.SetValueByColName("暂估单价", cElement.GetAttribute("暂估单价"));
                rec.SetValueByColName("暂估合价", cElement.GetAttribute("暂估合价"));
                rec.SetValueByColName("确认数量", cElement.GetAttribute("确认数量"));
                rec.SetValueByColName("差额单价", cElement.GetAttribute("差额单价"));
                rec.SetValueByColName("差额合价", cElement.GetAttribute("差额合价"));
                rec.SetValueByColName("产地", cElement.GetAttribute("产地"));
                rec.SetValueByColName("厂家", cElement.GetAttribute("厂家"));
                rec.SetValueByColName("品种", cElement.GetAttribute("品种"));
                rec.SetValueByColName("质量档次", cElement.GetAttribute("质量档次"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化专业工程暂估价
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatZyzgXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "专业工程暂估价";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "招标编码", "招标编码", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "工程内容", "工程内容", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "暂估金额", "暂估金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(5, "结算金额", "结算金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            XmlNodeList nodeList = node.SelectNodes("专业工程暂估明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("招标编码", cElement.GetAttribute("招标编码"));
                rec.SetValueByColName("名称", cElement.GetAttribute("工程名称"));
                rec.SetValueByColName("工程内容", cElement.GetAttribute("工程内容"));
                rec.SetValueByColName("暂估金额", cElement.GetAttribute("暂估金额"));
                rec.SetValueByColName("结算金额", cElement.GetAttribute("结算金额"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }

        private FyxmRec AppendGchzRecByXpath(XmlElement node, string xpath, FyxmTable tbl, FyxmRec pRec)
        {
            FyxmRec ret = null;
            XmlNode node1 = node.SelectSingleNode(xpath);
            if (node1 != null)
            {
                ret = tbl.NewRecord() as FyxmRec;
                if (pRec != null)
                    ret.recDeep = pRec.recDeep + 1;
                else
                    ret.recDeep = 1;
                ret.SetValueByColName("名称", node1.Name);
                ret.SetValueByColName("金额", node1.Value);
                tbl.recList.Add(ret);
            }
            return ret;
        }
        /// <summary>
        /// 序列化计日工
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatLxxmXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "计日工";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "招标编码", "招标编码", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(3, "单位", "单位", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "暂定数量", "暂定数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(5, "实际数量", "实际数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "综合单价", "综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "暂定合价", "暂定合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "实际合价", "实际合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(9, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(10, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            XmlNode rgNode = node.SelectSingleNode("人工");
            if (rgNode != null)
            {
                FyxmRec rgRec = AppendGchzRecByXpath(node, "人工", tbl, null);

                AppendJrgxmRec(rgNode, tbl, rgRec);
            }

            XmlNode clNode = node.SelectSingleNode("材料");
            if (clNode != null)
            {
                FyxmRec rgRec = AppendGchzRecByXpath(node, "材料", tbl, null);

                AppendJrgxmRec(clNode, tbl, rgRec);
            }

            XmlNode jxNode = node.SelectSingleNode("施工机械");
            if (jxNode != null)
            {
                FyxmRec rgRec = AppendGchzRecByXpath(node, "施工机械", tbl, null);

                AppendJrgxmRec(jxNode, tbl, rgRec);
            }

            XmlNode zhfNode = node.SelectSingleNode("综合费");
            if (zhfNode != null)
            {
                XmlElement zhfElement = (XmlElement)zhfNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("名称", zhfNode.Name);
                rec.SetValueByColName("备注", zhfElement.GetAttribute("取费基数"));
                rec.SetValueByColName("费率", zhfElement.GetAttribute("费率"));
                rec.SetValueByColName("暂定合价", zhfElement.GetAttribute("暂定合价"));
                rec.SetValueByColName("实际合价", zhfElement.GetAttribute("实际合价"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        private void AppendJrgxmRec(XmlNode node, FyxmTable tbl, FyxmRec pRec)
        {
            XmlNodeList nodeList = node.SelectNodes("计日工项目");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                if (pRec != null)
                    rec.recDeep = pRec.recDeep + 1;
                else
                    rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("招标编码", cElement.GetAttribute("招标编码"));
                rec.SetValueByColName("名称", cElement.GetAttribute("名称"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("暂定数量", cElement.GetAttribute("暂定数量"));
                rec.SetValueByColName("实际数量", cElement.GetAttribute("实际数量"));
                rec.SetValueByColName("综合单价", cElement.GetAttribute("综合单价"));
                rec.SetValueByColName("暂定合价", cElement.GetAttribute("暂定合价"));
                rec.SetValueByColName("实际合价", cElement.GetAttribute("实际合价"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));

                tbl.recList.Add(rec);
            }
        }
        /// <summary>
        /// 序列化总承包服务费
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatZcbfwfXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "总承包服务费";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "招标编码", "招标编码", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "项目名称", "项目名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(3, "项目价值", "项目价值", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(4, "计算基础", "计算基础", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(5, "服务内容", "服务内容", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(6, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);


            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            Action<XmlElement, FyxmRec> AppendZcbfwfRec = null;
            AppendZcbfwfRec = (pNode, pRec) =>
                {
                    XmlNodeList nodeList = pNode.SelectNodes("总承包服务费项目");
                    foreach (XmlNode cNode in nodeList)
                    {
                        XmlElement cElement = (XmlElement)cNode;
                        FyxmRec rec = tbl.NewRecord() as FyxmRec;
                        if (pRec != null)
                            rec.recDeep = pRec.recDeep + 1;
                        else
                            rec.recDeep = 1;
                        rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                        rec.SetValueByColName("招标编码", cElement.GetAttribute("招标编码"));
                        rec.SetValueByColName("项目名称", cElement.GetAttribute("项目名称"));
                        rec.SetValueByColName("项目价值", cElement.GetAttribute("项目价值"));
                        rec.SetValueByColName("计算基础", cElement.GetAttribute("计算基础"));
                        rec.SetValueByColName("服务内容", cElement.GetAttribute("服务内容"));
                        rec.SetValueByColName("费率", cElement.GetAttribute("费率"));
                        rec.SetValueByColName("金额", cElement.GetAttribute("金额"));
                        rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                        tbl.recList.Add(rec);

                        AppendZcbfwfRec(cElement, rec);
                    }
                };

            AppendZcbfwfRec(node, null);

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化索赔计价表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatSpbXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "索赔计价表";
            doc.tableList.Add(tbl);

            tbl.AddNewField(2, "项目名称", "项目名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(4, "单位", "单位", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(6, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "单价", "单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "合价", "合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "依据", "依据", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            XmlNodeList nodeList = node.SelectNodes("索赔签证计价费用项");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("项目名称", cElement.GetAttribute("项目名称"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("数量", cElement.GetAttribute("数量"));
                rec.SetValueByColName("单价", cElement.GetAttribute("单价"));
                rec.SetValueByColName("合价", cElement.GetAttribute("合价"));
                rec.SetValueByColName("依据", cElement.GetAttribute("依据"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化现场签证计价表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatQzbXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "现场签证计价表";
            doc.tableList.Add(tbl);

            tbl.AddNewField(2, "项目名称", "项目名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(4, "单位", "单位", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(6, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "单价", "单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "合价", "合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "依据", "依据", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            XmlNodeList nodeList = node.SelectNodes("索赔签证计价费用项");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("项目名称", cElement.GetAttribute("项目名称"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("数量", cElement.GetAttribute("数量"));
                rec.SetValueByColName("单价", cElement.GetAttribute("单价"));
                rec.SetValueByColName("合价", cElement.GetAttribute("合价"));
                rec.SetValueByColName("依据", cElement.GetAttribute("依据"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化其他
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatQtXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "其他";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "排序号", "排序号", 10, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "项目名称", "项目名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(3, "计量单位", "计量单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "计算公式", "计算公式", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(5, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "费用类别", "费用类别", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(8, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            XmlNodeList nodeList = node.SelectNodes("其他项明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("排序号", cElement.GetAttribute("排序号"));
                rec.SetValueByColName("项目名称", cElement.GetAttribute("项目名称"));
                rec.SetValueByColName("计量单位", cElement.GetAttribute("计量单位"));
                rec.SetValueByColName("计算公式", cElement.GetAttribute("计算公式"));
                rec.SetValueByColName("费率", cElement.GetAttribute("费率"));
                rec.SetValueByColName("金额", cElement.GetAttribute("金额"));
                rec.SetValueByColName("费用类别", cElement.GetAttribute("费用类别"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化规费和税金清单
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatGfsjXml(XmlElement node, DocData doc)
        {
            FyxmTable tbl = new FyxmTable();
            tbl.tableName = "规费和税金清单";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "排序号", "排序号", 10, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(7, "行变量", "行变量", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(2, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "计量单位", "计量单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "计算公式", "计算公式", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(4, "计算公式说明", "计算公式说明", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(5, "计算基数金额", "计算基数金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(5, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "费用类别", "费用类别", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(8, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.hj = CommUtils.GetAttributeDouble(node, "合计");

            XmlNodeList nodeList = node.SelectNodes("规费税金费用项");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                FyxmRec rec = tbl.NewRecord() as FyxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("排序号", cElement.GetAttribute("排序号"));
                rec.SetValueByColName("行变量", cElement.GetAttribute("行变量"));
                rec.SetValueByColName("名称", cElement.GetAttribute("名称"));
                rec.SetValueByColName("计算公式", cElement.GetAttribute("计算公式"));
                rec.SetValueByColName("计算公式说明", cElement.GetAttribute("计算公式说明"));
                rec.SetValueByColName("计算基数金额", cElement.GetAttribute("计算基数金额"));
                rec.SetValueByColName("费率", cElement.GetAttribute("费率"));
                rec.SetValueByColName("金额", cElement.GetAttribute("金额"));
                rec.SetValueByColName("费用类别", cElement.GetAttribute("费用类别"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化工料机汇总
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatrcjhzXml(XmlElement node, DocData doc)
        {
            ClxmTable tbl = new ClxmTable();
            tbl.tableName = "工料机汇总";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "代码", "代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "材料名称", "材料名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(3, "规格型号", "规格型号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(4, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(5, "计算类别", "计算类别", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(6, "材料指标分类", "材料指标分类", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(7, "单位系数", "单位系数", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "供应方式", "供应方式", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(9, "主要材料标志", "主要材料标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(10, "材料暂估标志", "材料暂估标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(11, "不计税设备标志", "不计税设备标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(12, "单价不由明细计算标志", "单价不由明细计算标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            tbl.AddNewField(13, "定额单价", "定额单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(14, "材料单价", "材料单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(15, "价格来源", "价格来源", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(16, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(17, "产地", "产地", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(18, "厂家", "厂家", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(19, "质量档次", "质量档次", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(20, "品种", "品种", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(21, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(22, "其他信息", "其他信息", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            tbl.AddNewDField(1, "关联材料代码", "关联材料代码", 15, (int)_alignType.atMiddle, (int)_dataType.dtInt, 0);
            tbl.AddNewDField(2, "消耗量", "消耗量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewDField(3, "不计价", "不计价", 15, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);

            XmlNodeList nodeList = node.SelectNodes("工料机明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                ClxmRec rec = tbl.NewRecord() as ClxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("代码", cElement.GetAttribute("代码"));
                rec.SetValueByColName("材料名称", cElement.GetAttribute("材料名称"));
                rec.SetValueByColName("规格型号", cElement.GetAttribute("规格型号"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("计算类别", cElement.GetAttribute("计算类别"));
                rec.SetValueByColName("材料指标分类", cElement.GetAttribute("材料指标分类"));
                rec.SetValueByColName("单位系数", cElement.GetAttribute("单位系数"));
                rec.SetValueByColName("供应方式", cElement.GetAttribute("供应方式"));
                rec.SetValueByColName("主要材料标志", cElement.GetAttribute("主要材料标志"));
                rec.SetValueByColName("材料暂估标志", cElement.GetAttribute("材料暂估标志"));
                rec.SetValueByColName("不计税设备标志", cElement.GetAttribute("不计税设备标志"));
                rec.SetValueByColName("单价不由明细计算标志", cElement.GetAttribute("单价不由明细计算标志"));
                rec.SetValueByColName("定额单价", cElement.GetAttribute("定额单价"));
                rec.SetValueByColName("材料单价", cElement.GetAttribute("材料单价"));
                rec.SetValueByColName("价格来源", cElement.GetAttribute("价格来源"));
                rec.SetValueByColName("数量", cElement.GetAttribute("数量"));
                rec.SetValueByColName("产地", cElement.GetAttribute("产地"));
                rec.SetValueByColName("厂家", cElement.GetAttribute("厂家"));
                rec.SetValueByColName("质量档次", cElement.GetAttribute("质量档次"));
                rec.SetValueByColName("品种", cElement.GetAttribute("品种"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                rec.SetValueByColName("其他信息", cElement.GetAttribute("其他信息"));
                tbl.recList.Add(rec);

                XmlNodeList cNodeList = cNode.SelectNodes("配合比材料明细");
                foreach (XmlNode ccNode in cNodeList)
                {
                    XmlElement ccElement = (XmlElement)ccNode;
                    MxclRec mxRec = tbl.NewDRecord() as MxclRec;
                    mxRec.recDeep = 1;
                    mxRec.SetValueByColName("关联材料代码", ccElement.GetAttribute("关联材料代码"));
                    mxRec.SetValueByColName("消耗量", ccElement.GetAttribute("消耗量"));
                    mxRec.SetValueByColName("不计价", ccElement.GetAttribute("不计价"));
                    rec.childList.Add(mxRec);
                }
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 序列化发包人提供材料及设备
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatFbrclXml(XmlElement node, DocData doc)
        {
            ClxmTable tbl = new ClxmTable();
            tbl.tableName = "发包人提供材料及设备";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "招标编码", "招标编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "关联材料代码", "关联材料代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(3, "材料名称", "材料名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(4, "规格型号", "规格型号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(5, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(6, "单价", "单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "交货方式", "交货方式", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(9, "送达地点", "送达地点", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(10, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("发包人提供材料及设备明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                ClxmRec rec = tbl.NewRecord() as ClxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("招标编码", cElement.GetAttribute("招标编码"));
                rec.SetValueByColName("关联材料代码", cElement.GetAttribute("关联材料代码"));
                rec.SetValueByColName("材料名称", cElement.GetAttribute("材料名称"));
                rec.SetValueByColName("规格型号", cElement.GetAttribute("规格型号"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("单价", cElement.GetAttribute("单价"));
                rec.SetValueByColName("数量", cElement.GetAttribute("数量"));
                rec.SetValueByColName("交货方式", cElement.GetAttribute("交货方式"));
                rec.SetValueByColName("送达地点", cElement.GetAttribute("送达地点"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 承包人提供材料及设备-造价信息差额法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatzjxxcefXml(XmlElement node, DocData doc)
        {
            ClxmTable tbl = new ClxmTable();
            tbl.tableName = "承包人提供材料及设备-造价信息差额法";
            doc.tableList.Add(tbl);

            tbl.AddNewField(1, "关联材料代码", "关联材料代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            tbl.AddNewField(2, "材料名称", "材料名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(3, "规格型号", "规格型号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(4, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(5, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "基准单价", "基准单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "投标单价", "投标单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(8, "发承包人确认单价", "发承包人确认单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(9, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("造价信息差额法明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                ClxmRec rec = tbl.NewRecord() as ClxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("关联材料代码", cElement.GetAttribute("关联材料代码"));
                rec.SetValueByColName("材料名称", cElement.GetAttribute("材料名称"));
                rec.SetValueByColName("规格型号", cElement.GetAttribute("规格型号"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("数量", cElement.GetAttribute("数量"));
                rec.SetValueByColName("基准单价", cElement.GetAttribute("基准单价"));
                rec.SetValueByColName("投标单价", cElement.GetAttribute("投标单价"));
                rec.SetValueByColName("发承包人确认单价", cElement.GetAttribute("发承包人确认单价"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
        /// <summary>
        /// 承包人提供材料及设备-价格指数差额法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="doc"></param>
        private void FormatJgzscefXml(XmlElement node, DocData doc)
        {
            ClxmTable tbl = new ClxmTable();
            tbl.tableName = "承包人提供材料及设备-价格指数差额法";
            doc.tableList.Add(tbl);

            tbl.AddNewField(2, "材料名称", "材料名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(3, "规格型号", "规格型号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(4, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0, false, true);
            tbl.AddNewField(5, "变值权重", "变值权重", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(6, "基本价格指数", "基本价格指数", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(7, "现行价格指数", "现行价格指数", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            tbl.AddNewField(9, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);

            XmlNodeList nodeList = node.SelectNodes("价格指数差额法明细");
            foreach (XmlNode cNode in nodeList)
            {
                XmlElement cElement = (XmlElement)cNode;
                ClxmRec rec = tbl.NewRecord() as ClxmRec;
                rec.recDeep = 1;
                rec.SetValueByColName("序号", cElement.GetAttribute("序号"));
                rec.SetValueByColName("材料名称", cElement.GetAttribute("材料名称"));
                rec.SetValueByColName("规格型号", cElement.GetAttribute("规格型号"));
                rec.SetValueByColName("单位", cElement.GetAttribute("单位"));
                rec.SetValueByColName("变值权重", cElement.GetAttribute("变值权重"));
                rec.SetValueByColName("基本价格指数", cElement.GetAttribute("基本价格指数"));
                rec.SetValueByColName("现行价格指数", cElement.GetAttribute("现行价格指数"));
                rec.SetValueByColName("备注", cElement.GetAttribute("备注"));
                tbl.recList.Add(rec);
            }

            tbl.ReSetRecStatus();
        }
    }
}
