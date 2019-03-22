using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using CjzDataBase;

namespace CjzContrast
{
    public class CtaDzTableData : CtaTableData
    {
        #region 私有字段
        private int _ctaStatus;  //  对比结果
        private int _errorCount; //  错误数量
        private CtaTableData _dzTable;  //对比表格

        #endregion

        #region 构造函数
        public CtaDzTableData()
        {
            _dzTable = new CtaTableData();
            _dzTable.colSynTable = this;

            this.colSynTable = _dzTable;
        }

        #endregion

        #region 属性定义


        /// <summary>
        /// 对比结果
        /// </summary>
        public int ctaStatus { get { return _ctaStatus; } set { _ctaStatus = value; } }
        /// <summary>
        /// 错误数量
        /// </summary>
        public int errorCount { get { return _errorCount; } set { _errorCount = value; } }
        /// <summary>
        /// 关联表格
        /// </summary>
        public CtaTableData dzTable { get { return _dzTable; } }
        
        #endregion

        #region 私有方法

        #endregion

        #region 公有方法
        /// <summary>
        /// 初始化所有字段
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            _ctaStatus = (int)_ctaStatusEnum.csConsis;
            _errorCount = 0;
            _dzTable.Clear();
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            CtaDzTableData s = sou as CtaDzTableData;
            if (s != null)
            {
                _ctaStatus = s._ctaStatus;
                _errorCount = s._errorCount;
                _dzTable.AssignFrom(s._dzTable);
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "CtaStatus", _ctaStatus);
            WriteXmlAttrValue(node, "ErrorCount", _errorCount);
            WriteXMLValue(node, "DzTable", _dzTable);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "CtaStatus", ref _ctaStatus);
            ReadXmlAttrValue(node, "ErrorCount", ref _errorCount);
            ReadXMLValue(node, "DzTable", _dzTable);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _ctaStatus);
            WriteToStream(stream, _errorCount);
            _dzTable.SaveToStream(stream);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _ctaStatus);
            ReadFromStream(stream, out _errorCount);
            _dzTable.LoadFromStream(stream);
        }
        /// <summary>
        /// 刷新对照关系
        /// </summary>
        public void FormCtaRelates()
        {
            ClearAllRecsTags();

            int startPos = 0;
            int asid = 1;
            foreach (CtaRecData rec in recList)
            {
                CtaRecData drec = dzTable.FindRecByKeyCols(rec, startPos);
                if (drec != null)
                {
                    startPos = dzTable.recList.IndexOf(drec) + 1;
                    rec.Sid = asid;
                    drec.Sid = asid;
                    asid++;

                    if (CompareRec(rec, drec) != 0)
                    {
                        rec.ctaStatus = (int)_ctaStatusEnum.csDiff;
                        drec.ctaStatus = rec.ctaStatus;
                    }

                    FormChildCtaRelates(rec, drec);
                }
                else
                {
                    rec.ctaStatus = (int)_ctaStatusEnum.csAdd;
                };
            }
            foreach (CtaRecData drec in dzTable.recList)
            {
                if (drec.Sid == 0)
                {
                    drec.ctaStatus = (int)_ctaStatusEnum.csDecrease;
                }
            }

            // 统计错误数量
            foreach (CtaRecData rec in recList)
            {
                if (rec.ctaStatus != (int)_ctaStatusEnum.csConsis)  // 多项或不一致
                    _errorCount++;
            }
            foreach (CtaRecData rec in dzTable.recList)
            {
                if (rec.ctaStatus == (int)_ctaStatusEnum.csDecrease)  // 漏项
                    _errorCount++;
            }
        }
        /// <summary>
        /// 刷新子记录对照关系
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dlist"></param>
        private void FormChildCtaRelates(CtaRecData prec, CtaRecData dprec)
        {
            int startPos = 0;
            int asid = 1;
            foreach (CtaRecData rec in prec.childList)
            {
                CtaRecData drec = FindDRecByKeyCols(rec, startPos, dprec.childList);
                if (drec != null)
                {
                    startPos = prec.childList.IndexOf(rec) + 1;
                    rec.Sid = asid;
                    drec.Sid = asid;
                    asid++;

                    if (CompareRec(rec, drec, true) != 0)
                    {
                        rec.ctaStatus = (int)_ctaStatusEnum.csDiff;  // 不一致
                        drec.ctaStatus = rec.ctaStatus;
                        // 父项设为不一致
                        prec.ctaStatus = (int)_ctaStatusEnum.csDiff;
                        dprec.ctaStatus = prec.ctaStatus;
                    }
                }
                else
                {
                    rec.ctaStatus = (int)_ctaStatusEnum.csAdd;  // 多项
                    // 父项设为不一致
                    prec.ctaStatus = (int)_ctaStatusEnum.csDiff;
                    dprec.ctaStatus = prec.ctaStatus;
                };
            }
            foreach (CtaRecData drec in dprec.childList)
            {
                if (drec.Sid == 0)
                {
                    drec.ctaStatus = (int)_ctaStatusEnum.csDecrease;  // 漏项
                    // 父项设为不一致
                    prec.ctaStatus = (int)_ctaStatusEnum.csDiff;
                    dprec.ctaStatus = prec.ctaStatus;

                }
            }
        }
        /// <summary>
        /// 根据关键列查找对应记录
        /// </summary>
        /// <param name="sourec"></param>
        /// <param name="startpos"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private CtaRecData FindDRecByKeyCols(CtaRecData sourec, int startpos, DataList list)
        {
            CtaRecData ret = null;
            for (int i = startpos; i < list.Count; i++)
            {
                CtaRecData rec = list[i] as CtaRecData;
                bool find = true;
                foreach (CtaColDef coldef in dcolList)
                {
                    if (coldef.keyCol)
                    {
                        find = (sourec.GetValue(coldef.colIndx)) == (rec.GetValue(coldef.colIndx));
                        if (!find)
                            break;
                    }
                }
                if (find)
                {
                    ret = rec;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// 比较记录一致
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="drec"></param>
        /// <param name="compd">比较子表记录</param>
        /// <returns></returns>
        public int CompareRec(CtaRecData rec, CtaRecData drec, bool compd = false)
        {
            int ret = 0;

            DataList fields = colList;
            if (compd)
                fields = dcolList;
            foreach (CtaColDef coldef in fields)
            {
                if (!coldef.unCheck)
                {
                    CtaColData coldata = rec.GetColDataByCol(coldef.colIndx) as CtaColData;
                    CtaColData dcoldata = drec.GetColDataByCol(coldef.colIndx) as CtaColData;
                    bool issame = true;
                    switch (coldef.colType)
                    {
                        case (int)_dataType.dtInt:
                        case (int)_dataType.dtFloat:
                            {
                                double v1 = 0, v2 = 0;
                                if (coldata != null)
                                    double.TryParse(coldata.express, out v1);
                                if (dcoldata != null)
                                    double.TryParse(dcoldata.express, out v2);
                                issame = (v1 == v2);
                                break;
                            }
                        case (int)_dataType.dtBoolean:
                            {
                                bool v1 = false, v2 = false;
                                if (coldata != null)
                                    bool.TryParse(coldata.express, out v1);
                                if (dcoldata != null)
                                    bool.TryParse(dcoldata.express, out v2);
                                issame = (v1 == v2);
                                break;
                            }
                        default:
                            {
                                string v1 = ""; string v2 = "";
                                if (coldata != null)
                                    v1 = coldata.express;
                                if (dcoldata != null)
                                    v2 = dcoldata.express;
                                issame = (v1 == v2);
                                break;
                            }
                    }
                    if (!issame)
                    {
                        if (coldata != null)
                            coldata.ctaStatus = (int)_ctaStatusEnum.csDiff;
                        if (dcoldata != null)
                            dcoldata.ctaStatus = (int)_ctaStatusEnum.csDiff;
                        ret = (int)_ctaStatusEnum.csDiff;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 清除对照关系
        /// </summary>
        public void ClearAllRecsTags()
        {
            foreach (CtaRecData rec in recList)
            {
                rec.ctaStatus = (int)_ctaStatusEnum.csConsis;
                rec.Sid = 0;

                foreach (CtaRecData drec in rec.childList)
                {
                    drec.ctaStatus = (int)_ctaStatusEnum.csConsis;
                    drec.Sid = 0;
                }
            }

            _ctaStatus = (int)_ctaStatusEnum.csConsis;
            _errorCount = 0;
        }
        #endregion
    }
}
