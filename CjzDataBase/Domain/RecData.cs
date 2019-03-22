using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace CjzDataBase
{
    class SortByColIndex : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            int ret = 0;
            if ((x is ColData) && (y is ColData))
            {
                int col1 = (x as ColData).colIndex;
                int col2 = (y as ColData).colIndex;
                if (col1 > col2)
                    ret = 1;
                else if (col1 < col2)
                    ret = -1;
                else
                    ret = 0;
            };
            return ret;
        }
    }

    public enum _recStatusEnum
    {
        rsData = 0,
        rsTotal = 1,
    }

    /// <summary>
    /// 表格记录基类
    /// </summary>
    public class RecData : DataPacket
    {
        #region 私有字段
        private string _recId;  // 记录标识
        private int _recDeep;  //  记录级别
        private string _recNum;  //  记录序号
        private DataList _colList;  //  列属性列表
        private DataList _childList;  //  子记录列表
        private DataList _fields;  // 字段列表
        private _recStatusEnum _recStatus;  // 状态
        private TableData _ownerTable; // 所属表格
        private DataList _ownerList; // 所属列表

        #endregion

        #region 构造函数
        public RecData()
        {
            _recId = Guid.NewGuid().ToString();
            _recDeep = 1;
            _colList = new DataList();
            _colList.ItemType = typeof(ColData);
            _childList = new DataList();
            _childList.ItemType = typeof(RecData);
            _recStatus = _recStatusEnum.rsData;

        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 记录标识
        /// </summary>
        public string recId { get { return _recId; } set { _recId = value; } }
        /// <summary>
        /// 记录级别
        /// </summary>
        public int recDeep { get { return _recDeep; } set { _recDeep = value; } }
        /// <summary>
        /// 记录序号
        /// </summary>
        public string recNum { get { return _recNum; } set { _recNum = value; } }
        /// <summary>
        /// 列属性列表
        /// </summary>
        public DataList colList { get { return _colList; } }
        /// <summary>
        /// 子记录列表
        /// </summary>
        public DataList childList { get { return _childList; } }
        /// <summary>
        /// 字段列表
        /// </summary>
        public DataList fields { get { return _fields; } set { _fields = value; } }
        /// <summary>
        /// 列数量
        /// </summary>
        public int colcount
        {
            get
            {
                if (_fields != null)
                    return _fields.Count;
                else
                    return _colList.Count;
            }
        }
        /// <summary>
        /// 记录状态
        /// </summary>
        public _recStatusEnum recStatus { get { return _recStatus; } set { _recStatus = value; } }
        /// <summary>
        /// 所属表格
        /// </summary>
        public TableData ownerTable { get { return _ownerTable; } set { _ownerTable = value; } }
        /// <summary>
        /// 所属列表
        /// </summary>
        public DataList ownerList { get { return _ownerList; } set { _ownerList = value; } }
        /// <summary>
        /// 取父项
        /// </summary>
        public RecData parentRec
        {
            get
            {
                RecData ret = null;
                if (_ownerList != null)
                {
                    int idx = _ownerList.IndexOf(this);
                    if (idx > 0)
                    {
                        for (int i = idx - 1; i >= 0; i--)
                        {
                            RecData arec = _ownerList[i] as RecData;
                            if (arec._recDeep == this.recDeep - 1)
                            {
                                ret = arec;
                                break;
                            }
                        }
                    }
                };

                return ret;
            }
        }
        /// <summary>
        /// 取子项数量
        /// </summary>
        public int childCount
        {
            get
            {
                int ret = 0;
                if (_ownerList != null)
                {
                    int idx = _ownerList.IndexOf(this);
                    if (idx >= 0)
                    {
                        for (int i = idx + 1; i < _ownerList.Count; i++)
                        {
                            RecData arec = _ownerList[i] as RecData;
                            if (arec.recDeep > this.recDeep)
                                ret++;
                            else
                                if (arec._recDeep <= this.recDeep)
                                break;
                        }
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
            _recId = null;
            _recDeep = 1;
            _recStatus = _recStatusEnum.rsData;
            _recNum = "";
            _colList.Clear();
            _childList.Clear();
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            RecData s = sou as RecData;
            if (s != null)
            {
                _recId = s._recId;
                _recDeep = s._recDeep;
                _recNum = s._recNum;
                _recStatus = s._recStatus;
                _colList.AssignFrom(s._colList);
                _childList.AssignFrom(s._childList);
            }

        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "RecId", _recId);
            WriteXmlAttrValue(node, "RecDeep", _recDeep);
            WriteXmlAttrValue(node, "RecNum", _recNum);
            WriteXmlAttrValue(node, "RecStatus", (int)_recStatus);
            WriteXmlAttrValue(node, "ChildCount", childCount);
            RecData parRec = parentRec;
            if (parRec != null)
                WriteXmlAttrValue(node, "ParRecId", parRec.UniqId);
            if (_colList.Count > 0)
                WriteXMLValue(node, "ColList", _colList);
            if (_childList.Count > 0)
                WriteXMLValue(node, "ChildList", _childList);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "RecId", ref _recId);
            ReadXmlAttrValue(node, "RecDeep", ref _recDeep);
            ReadXmlAttrValue(node, "RecNum", ref _recNum);
            ReadXmlAttrValue(node, "RecNum", ref _recNum);
            int status = 0;
            ReadXmlAttrValue(node, "RecStatus", ref status);
            _recStatus = (_recStatusEnum)status;
            ReadXMLValue(node, "ColList", _colList);
            ReadXMLValue(node, "ChildList", _childList);

            _childList.XMLDecode(node);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _recId);
            WriteToStream(stream, _recDeep);
            WriteToStream(stream, _recNum);
            WriteToStream(stream, (int)_recStatus);
            _colList.SaveToStream(stream);
            _childList.SaveToStream(stream);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _recId);
            ReadFromStream(stream, out _recDeep);
            ReadFromStream(stream, out _recNum);
            int status = 0;
            ReadFromStream(stream, out status);
            _recStatus = (_recStatusEnum)status;
            _colList.LoadFromStream(stream);
            _childList.LoadFromStream(stream);
        }
        /// <summary>
        /// 取列数据
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public ColData GetColDataByCol(int col)
        {
            return _colList.Cast<ColData>().FirstOrDefault(a => a.colIndex == col);
        }
        /// <summary>
        /// 根据列号取实际值
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public string GetValue(int col)
        {
            ColData coldata = GetColDataByCol(col);
            if (coldata != null)
                return coldata.express;
            else
                return null;
        }
        /// <summary>
        /// 根据列号设值
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetValue(int col, string v)
        {
            ColData coldata = GetColDataByCol(col);
            if (coldata != null)
                coldata.express = v;
            else
            {
                coldata = NewColData();
                coldata.colIndex = col;
                coldata.express = v;
                _colList.Add(coldata);
            };
        }
        /// <summary>
        /// 根据列号取显示值
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public string GetDspValue(int col)
        {
            string ret = null;
            ColData coldata = GetColDataByCol(col);
            if (coldata != null)
            {
                ret = coldata.express;
                if (_fields != null)
                {
                    ColDef coldef = _fields.Cast<ColDef>().FirstOrDefault(a => a.colIndx == col);
                    if (coldef != null)
                    {
                        switch (coldef.colType)
                        {
                            case (int)_dataType.dtString:
                                ret = coldata.express;
                                break;
                            case (int)_dataType.dtInt:
                                ret = int.Parse(coldata.express).ToString();
                                break;
                            case (int)_dataType.dtBoolean:
                                ret = Boolean.Parse(coldata.express).ToString();
                                break;
                            case (int)_dataType.dtFloat:
                                ret = float.Parse(coldata.express).ToString("F" + coldef.colDec.ToString());
                                break;
                            default:
                                ret = coldata.express;
                                break;
                        };
                    };
                };
            };
            return ret;
        }
        /// <summary>
        /// 根据列名取列数据
        /// </summary>
        /// <param name="colname">列名</param>
        /// <returns></returns>
        public ColData GetColDataByColName(string colname)
        {
            if (_fields != null)
            {
                ColDef coldef = _fields.Cast<ColDef>().FirstOrDefault(a => a.colName == colname);
                if (coldef != null)
                {
                    return GetColDataByCol(coldef.colIndx);
                }
                else
                    return null;
            }
            else
                return null;
        }
        /// <summary>
        /// 根据列名取实际值
        /// </summary>
        /// <param name="colname">列名</param>
        /// <returns></returns>
        public string GetValueByColName(string colname)
        {
            ColData coldata = GetColDataByColName(colname);
            if (coldata != null)
                return coldata.express;
            else
                return null;

        }
        /// <summary>
        /// 根据列名设值
        /// </summary>
        /// <param name="colname">列名</param>
        /// <param name="v"></param>
        public void SetValueByColName(string colname, string v)
        {
            ColData coldata = GetColDataByColName(colname);
            if (coldata != null)
                coldata.express = v;
            else
            {
                if (_fields != null)
                {
                    ColDef coldef = _fields.Cast<ColDef>().FirstOrDefault(a => a.colName == colname);
                    if (coldef != null)
                        SetValue(coldef.colIndx, v);
                };
            };
        }
        /// <summary>
        /// 根据列名取显示值
        /// </summary>
        /// <param name="colname">列名</param>
        /// <returns></returns>
        public string GetDspValueByColName(string colname)
        {
            ColData coldata = GetColDataByColName(colname);
            if (coldata != null)
                return GetDspValue(coldata.colIndex);
            else
                return null;
        }
        /// <summary>
        /// 新建列记录
        /// </summary>
        /// <returns></returns>
        public virtual ColData NewColData()
        {
            return new ColData();
        }

        /// <summary>
        /// 补全ColList数据
        /// </summary>
        public virtual void CheckColListData()
        {
            if (_fields != null)
            {
                foreach (ColDef colDef in _fields)
                {
                    ColData colData = GetColDataByCol(colDef.colIndx);
                    if (colData is null)
                    {
                        colData = NewColData();
                        colData.colIndex = colDef.colIndx;
                        colList.Add(colData);
                    }
                }
            }
            _colList.Sort(new SortByColIndex());
        }
        #endregion
    }
}
