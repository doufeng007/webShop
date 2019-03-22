using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CjzDataBase;
using System.Xml;
using System.IO;

namespace CjzContrast
{
    public class CtaTableData : MDTableData
    {
        #region 私有字段
        private CtaTableData _synColTable;  //关联表格
        private TableData _souTable; // 数据源表格

        #endregion

        #region 构造函数
        public CtaTableData()
        {
            colList.ItemType = typeof(CtaColDef);
            recList.ItemType = typeof(CtaRecData);
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 关联表格
        /// </summary>
        public CtaTableData colSynTable { get { return _synColTable; } set { _synColTable = value; } }
        /// <summary>
        /// 数据源表格
        /// </summary>
        public TableData souTable { get { return _souTable; } set { _souTable = value; } }
        /// <summary>
        /// 差异列表
        /// </summary>
        public DataList diffRecList
        {
            get
            {
                Func<CtaRecData, int, bool> HasDifChild = (rec, idx) =>
                {
                    bool r = false;

                    for (int i = idx + 1; i < recList.Count; i++)
                    {
                        CtaRecData rec1 = recList[i] as CtaRecData;
                        if (rec1.recDeep <= rec.recDeep)
                            break;
                        else if ((rec1.recDeep > rec.recDeep) && (rec1.ctaStatus != (int)_ctaStatusEnum.csConsis))
                        {
                            r = true;
                            break;
                        };
                    }

                    return r;
                };

                DataList ret = new DataList();
                ret.ItemType = typeof(CtaRecData);

                for (int i = 0; i < recList.Count - 1; i++)
                {
                    CtaRecData rec = recList[i] as CtaRecData;
                    if ((rec.ctaStatus != (int)_ctaStatusEnum.csConsis) || HasDifChild(rec, i))
                    {
                        CtaRecData newrec = NewRecord() as CtaRecData;
                        newrec.AssignFrom(rec);
                        FormRec(newrec);
                        ret.Add(newrec);
                        newrec.ownerList = ret;
                    }
                }
                return ret;
            }
        }
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
            //_synColTable = null;
            _souTable = null;
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            CtaTableData s = sou as CtaTableData;
            if (s != null)
            {
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            DataList list = diffRecList;
            if (list.Count > 0)
            {
                WriteXMLValue(node, "DiffRecList", list);
            };
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
        }
        /// <summary>
        /// 新建表格记录
        /// </summary>
        /// <returns></returns>
        public override RecData NewRecord()
        {
            CtaRecData rec = new CtaRecData();
            FormRec(rec);
            return rec;
        }

        public override RecData NewDRecord()
        {
            CtaRecData rec = new CtaRecData();
            FormDRec(rec);
            return rec;
        }
        /// <summary>
        /// 通过sid取表格记录
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public CtaRecData GetRecBySid(int sid)
        {
            return recList.Cast<CtaRecData>().FirstOrDefault(a => a.Sid == sid);
        }
        /// <summary>
        /// 通过sid取对比表格记录
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public CtaRecData GetContrastRec(int sid)
        {
            CtaRecData ret = null;
            if (_synColTable != null)
                ret = _synColTable.GetRecBySid(sid);
            return ret;
        }
        /// <summary>
        /// 刷新表格列
        /// </summary>
        public virtual void ReFormFields()
        {
            colList.Clear();
            if (souTable != null)
            {
                foreach (ColDef soucol in souTable.colList)
                {
                    CtaColDef coldef = new CtaColDef();
                    coldef.AssignFrom(soucol);
                    if (soucol.colIndx == 0)           // 这里序号列不对比，其他列都对比
                        coldef.unCheck = true;
                    else
                        coldef.unCheck = false;
                    colList.Add(coldef);
                }

                if (souTable is MDTableData)
                {
                    dcolList.Clear();
                    foreach (ColDef soucol in (souTable as MDTableData).dcolList)
                    {
                        CtaColDef coldef = new CtaColDef();
                        coldef.AssignFrom(soucol);
                        if (soucol.colIndx == 0)           // 这里序号列不对比，其他列都对比
                            coldef.unCheck = true;
                        else
                            coldef.unCheck = false;
                        dcolList.Add(coldef);
                    }
                }
            }
        }
        /// <summary>
        /// 刷新表格数据
        /// </summary>
        public virtual void RefreshRecs()
        {
            recList.Clear();
            if (souTable != null)
            {
                foreach (RecData sourec in souTable.recList)
                {
                    CtaRecData rec = NewRecord() as CtaRecData;

                    rec.AssignFrom(sourec);
                    FormRec(rec);

                    recList.Add(rec);
                }
            }
        }

        public CtaRecData FindRecByKeyCols(CtaRecData sourec, int startpos)
        {
            CtaRecData ret = null;
            for (int i = startpos; i < recList.Count; i++)
            {
                CtaRecData rec = recList[i] as CtaRecData;
                bool find = true;
                foreach (CtaColDef coldef in colList)
                {
                    if (coldef.keyCol)
                    {
                        find = (sourec.GetValue(coldef.colIndx)) == (rec.GetValue(coldef.colIndx));
                        if (find)
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

        #endregion
    }
}
