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

    /// <summary>
    /// 表格基类
    /// </summary>
    public class TableData : DataPacket
    {
        #region 私有字段
        private string _tableName;  //	表格名称
        private DataList _colList; //  列名列表
        private DataList _recList; //  工程规模

        #endregion

        #region 构造函数
        public TableData()
        {
            _colList = new DataList();
            _colList.ItemType = typeof(ColDef);
            _recList = new DataList();
            _recList.ItemType = typeof(RecData);
            InitialCols();
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 工程名称.
        /// </summary>
        public string tableName { get { return _tableName; } set { _tableName = value; } }
        /// <summary>
        /// 工程名称.
        /// </summary>
        public DataList colList { get { return _colList; } }
        /// <summary>
        /// 工程名称.
        /// </summary>
        public DataList recList { get { return _recList; } }
        /// <summary>
        /// 显示列数
        /// </summary>
        public int colcount
        {
            get
            {
                int n = 0;
                foreach (ColDef coldef in _colList)
                {
                    if (!coldef.hide)
                        n = n + 1;
                };
                return n;
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
            _tableName = null;
            _colList.Clear();
            _recList.Clear();
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            TableData s = sou as TableData;
            if (s != null)
            {
                _tableName = s.tableName;
                _colList.AssignFrom(s._colList);
                _recList.AssignFrom(s._recList);
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "TableName", _tableName);
            WriteXMLValue(node, "ColList", _colList);
            WriteXMLValue(node, "RecList", _recList);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "TableName", ref _tableName);
            ReadXMLValue(node, "ColList", _colList);
            ReadXMLValue(node, "RecList", _recList);
            FormRecs();
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _tableName);
            _colList.SaveToStream(stream);
            _recList.SaveToStream(stream);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _tableName);
            _colList.LoadFromStream(stream);
            _recList.LoadFromStream(stream);
            FormRecs();
        }

        /// <summary>
        /// 取列元素
        /// </summary>
        /// <param name="index">列号</param>
        /// <returns></returns>
        public ColDef GetColDataByCol(int col)
        {
            return _colList.Cast<ColDef>().FirstOrDefault(a => a.colIndx == col);
        }
        /// <summary>
        /// 根据名称取列号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetColByName(string name)
        {
            ColDef coldef = _colList.Cast<ColDef>().FirstOrDefault(a => a.colName == name);
            if (coldef != null)
                return coldef.colIndx;
            else
                return -1;

        }
        /// <summary>
        /// 取列名称
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public string GetColName(int col)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                return coldef.colName;
            else
                return null;
        }
        /// <summary>
        /// 设列名称
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetColName(int col, string v)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                coldef.colName = v;
        }
        /// <summary>
        /// 取列标签
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public string GetColLabel(int col)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                return coldef.colLabel;
            else
                return null;
        }
        /// <summary>
        /// 设列标签
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetColLabel(int col, string v)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                coldef.colLabel = v;
        }
        /// <summary>
        /// 取列居中方式
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public int GetColAlign(int col)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                return coldef.colAlign;
            else
                return 0;
        }
        /// <summary>
        /// 设列居中方式
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetColAlign(int col, int v)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                coldef.colAlign = v;
        }
        /// <summary>
        /// 取列宽
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public int GetColWid(int col)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                return coldef.colWidth;
            else
                return 0;
        }
        /// <summary>
        /// 设列宽
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetColWid(int col, int v)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                coldef.colWidth = v;
        }
        /// <summary>
        /// 取小数位数
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public int GetColDec(int col)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                return coldef.colDec;
            else
                return 0;
        }
        /// <summary>
        /// 设小数位数
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetColDec(int col, int v)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                coldef.colDec = v;
        }
        /// <summary>
        /// 取列隐藏状态
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public bool GetColHide(int col)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                return coldef.hide;
            else
                return false;
        }
        /// <summary>
        /// 设列隐藏状态
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetColHide(int col, bool v)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                coldef.hide = v;
        }
        /// <summary>
        /// 取关键列状态
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool GetColKeyCol(int col)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                return coldef.keyCol;
            else
                return false;
        }
        /// <summary>
        /// 设关键列状态
        /// </summary>
        /// <param name="col"></param>
        /// <param name="v"></param>
        public void SetColKeyCol(int col, bool v)
        {
            ColDef coldef = GetColDataByCol(col);
            if (coldef != null)
                coldef.keyCol = v;
        }
        /// <summary>
        /// 移动列
        /// </summary>
        /// <param name="oldcol">旧位置</param>
        /// <param name="newcol">新位置</param>
        public void MoveCol(int oldcol, int newcol)
        {
            ColDef coldef1 = GetColDataByCol(oldcol);
            ColDef coldef2 = GetColDataByCol(newcol);
            if ((coldef1 != null) && (coldef2 != null))
            {
                _colList.Move(_colList.IndexOf(oldcol), _colList.IndexOf(newcol));
            }
        }

        /// <summary>
        /// 取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataPacket GetRecById(string id)
        {
            return _recList.Cast<DataPacket>().FirstOrDefault(a => a.UniqId == id);
        }
        /// <summary>
        /// 转换实际列号
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public int TransDisplayCol(int col)
        {
            int orgcol = -1;
            int n = 0;
            foreach (ColDef coldef in _colList)
            {
                if (!coldef.hide)
                {
                    n++;
                    if (col == n)
                    {
                        orgcol = coldef.colIndx;
                        break;
                    };
                }
            };
            return orgcol;
        }
        /// <summary>
        /// 新建表格记录
        /// </summary>
        /// <returns></returns>
        public virtual RecData NewRecord()
        {
            RecData rec = new RecData();
            FormRec(rec);
            return rec;
        }
        /// <summary>
        /// 格式化表格记录
        /// </summary>
        /// <param name="rec">表格记录</param>
        public virtual void FormRec(RecData rec)
        {
            rec.fields = _colList;
            rec.ownerTable = this;
            rec.ownerList = _recList;
        }
        /// <summary>
        /// 格式化所有表格记录
        /// </summary>
        public virtual void FormRecs()
        {
            foreach (RecData rec in _recList)
            {
                FormRec(rec);
            }
        }
        /// <summary>
        /// 初始化列
        /// </summary>
        public virtual void InitialCols()
        {
            AddNewField(0, "序号", "序号", 12, (int)_alignType.atMiddle, (int)_dataType.dtString, 0);
        }
        /// <summary>
        /// 增加列
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="colname">列名</param>
        /// <param name="collabel">列标签，为空则默认为列名</param>
        /// <param name="colalign">列居中方式</param>
        /// <param name="coltype">列数据类型</param>
        /// <param name="coldec">列显示小数位数</param>
        /// <returns></returns>
        public ColDef AddNewField(int col, string colname, string collabel, int colwidth, int colalign, int coltype, int coldec, bool hide = false, bool keycol = false)
        {
            ColDef coldef = new ColDef();
            coldef.colIndx = col;
            coldef.colName = colname;
            coldef.colLabel = collabel;
            if (string.IsNullOrEmpty(collabel))
                coldef.colLabel = colname;
            coldef.colWidth = colwidth;
            coldef.colAlign = colalign;
            coldef.colType = coltype;
            coldef.colDec = coldec;
            coldef.hide = hide;
            coldef.keyCol = keycol;
            _colList.Add(coldef);
            return coldef;
        }
        /// <summary>
        /// 取父记录
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public RecData GetParentRec(RecData rec)
        {
            RecData ret = null;

            int idx = recList.IndexOf(rec);
            if (idx >= 0)
            {
                for (int i = idx - 1; i >= 0; i--)
                {
                    RecData arec = recList[i] as RecData;
                    if (rec.recDeep == arec.recDeep - 1)
                    {
                        ret = arec;
                        break;
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// 是否有子项
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="ownerlist"></param>
        /// <returns></returns>
        public bool HasChild(RecData rec, DataList ownerlist)
        {
            bool ret = false;
            int idx = ownerlist.IndexOf(rec);
            if (idx < ownerlist.Count - 1)
            {
                RecData arec = ownerlist[idx + 1] as RecData;
                if (arec.recDeep > rec.recDeep)
                    ret = true;
            }
            return ret;
        }
        /// <summary>
        /// 是否有子项（主表记录）
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public bool HasChild(RecData rec)
        {
            return HasChild(rec, recList);
        }
        /// <summary>
        /// 重设记录状态（序号重排）
        /// </summary>
        public void ReSetRecStatus()
        {
            int[] deepArr = new int[15];
            for (int i = 0; i < 15; i++)
            {
                deepArr[i] = 0;
            };

            for (int i = 0; i < recList.Count - 1; i++)
            {
                RecData rec = recList[i] as RecData;
                // 设记录状态，是否含有子项
                if (HasChild(rec))
                    rec.recStatus = _recStatusEnum.rsTotal;
                else
                    rec.recStatus = _recStatusEnum.rsData;

                // 设记录序号
                deepArr[rec.recDeep] = deepArr[rec.recDeep] + 1;
                //rec.recNum = "";
                rec.SetValue(0, "");
                for (int j = 1; j <= rec.recDeep; j++)
                {
                    if (string.IsNullOrEmpty(rec.recNum))
                        rec.SetValue(0, deepArr[j].ToString());
                    else
                        rec.SetValue(0, rec.GetValue(0) + "." + deepArr[j].ToString());
                }
                for (int j = rec.recDeep + 1; j < 15; j++)
                {
                    deepArr[j] = 0;
                }
                //补全colList
                rec.CheckColListData();
                //rec.colList.Sort(new SortByColIndex());
                //rec.colList.Cast<ColData>().OrderBy(a => a.colIndex).ToList().Distinct();

                #region 设子记录信息
                int[] ddeepArr = new int[15];
                for (int di = 0; di < 15; di++)
                {
                    ddeepArr[di] = 0;
                };

                for (int di = 0; di < rec.childList.Count - 1; di++)
                {
                    RecData drec = rec.childList[di] as RecData;
                    if (HasChild(drec, rec.childList))
                        drec.recStatus = _recStatusEnum.rsTotal;
                    else
                        drec.recStatus = _recStatusEnum.rsData;

                    ddeepArr[drec.recDeep] = ddeepArr[drec.recDeep] + 1;
                    drec.recNum = "";
                    for (int dj = 1; dj <= drec.recDeep; dj++)
                    {
                        if (string.IsNullOrEmpty(drec.recNum))
                            drec.recNum = ddeepArr[dj].ToString();
                        else
                            drec.recNum = drec.recNum + "." + ddeepArr[dj].ToString();
                    }
                    // 清空低级别计数
                    for (int dj = drec.recDeep + 1; dj < 15; dj++)
                    {
                        ddeepArr[dj] = 0;
                    }
                    // 列记录排序
                    drec.colList.Sort(new SortByColIndex());
                    drec.CheckColListData();
                }
                #endregion
            }
        }

        #endregion
    }
}
