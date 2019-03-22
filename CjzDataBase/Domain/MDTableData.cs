using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace CjzDataBase
{
    public class MDTableData : TableData
    {
        #region 私有字段
        private DataList _dcolList;  // 子表列名列表
        #endregion

        #region 构造函数
        public MDTableData()
        {
            _dcolList = new DataList();
            _dcolList.ItemType = typeof(ColDef);
            InitialDCols();
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 子表列名列表
        /// </summary>
        public DataList dcolList { get { return _dcolList; } } 
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
            _dcolList.Clear();
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            MDTableData s = sou as MDTableData;
            if (s != null)
            {
                _dcolList.AssignFrom(s._dcolList); 
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXMLValue(node, "DColList", _dcolList);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            _dcolList.XMLDecode(node);
            ReadXMLValue(node, "DColList", _dcolList);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            _dcolList.SaveToStream(stream);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            _dcolList.LoadFromStream(stream);
        }

        /// <summary>
        /// 取子表列元素
        /// </summary>
        /// <param name="index">列号</param>
        /// <returns></returns>
        public ColDef GetDColDataByCol(int col)
        {
            return _dcolList.Cast<ColDef>().FirstOrDefault(a => a.colIndx == col);
        }

        /// <summary>
        /// 取子表列名称
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public string GetDColName(int col)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                return coldef.colName;
            else
                return null;
        }
        /// <summary>
        /// 设子表列名称
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetDColName(int col, string v)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                coldef.colName = v;
        }
        /// <summary>
        /// 取子表列标签
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public string GetDColLabel(int col)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                return coldef.colLabel;
            else
                return null;
        }
        /// <summary>
        /// 设子表列标签
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetDColLabel(int col, string v)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                coldef.colLabel = v;
        }
        /// <summary>
        /// 取子表列居中方式
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public int GetDColAlign(int col)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                return coldef.colAlign;
            else
                return 0;
        }
        /// <summary>
        /// 设子表列居中方式
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetDColAlign(int col, int v)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                coldef.colAlign = v;
        }
        /// <summary>
        /// 取子表列宽
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public int GetDColWid(int col)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                return coldef.colWidth;
            else
                return 0;
        }
        /// <summary>
        /// 设子表列宽
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetDColWid(int col, int v)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                coldef.colWidth = v;
        }
        /// <summary>
        /// 取子表小数位数
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public int GetDColDec(int col)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                return coldef.colDec;
            else
                return 0;
        }
        /// <summary>
        /// 设子表小数位数
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetDColDec(int col, int v)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                coldef.colDec = v;
        }
        /// <summary>
        /// 取子表列隐藏状态
        /// </summary>
        /// <param name="col">列号</param>
        /// <returns></returns>
        public bool GetDColHide(int col)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                return coldef.hide;
            else
                return false;
        }
        /// <summary>
        /// 设子表列隐藏状态
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="v">值</param>
        public void SetDColHide(int col, bool v)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                coldef.hide = v;
        }
        /// <summary>
        /// 取子表关键列状态
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool GetDColKeyCol(int col)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                return coldef.keyCol;
            else
                return false;
        }
        /// <summary>
        /// 设子表关键列状态
        /// </summary>
        /// <param name="col"></param>
        /// <param name="v"></param>
        public void SetDColKeyCol(int col, bool v)
        {
            ColDef coldef = GetDColDataByCol(col);
            if (coldef != null)
                coldef.keyCol = v;
        }
        /// <summary>
        /// 移动子表列
        /// </summary>
        /// <param name="oldcol">旧位置</param>
        /// <param name="newcol">新位置</param>
        public void MoveDCol(int oldcol, int newcol)
        {
            ColDef coldef1 = GetDColDataByCol(oldcol);
            ColDef coldef2 = GetDColDataByCol(newcol);
            if ((coldef1 != null) && (coldef2 != null))
            {
                _dcolList.Move(_dcolList.IndexOf(oldcol), _dcolList.IndexOf(newcol));
            }
        }

        /// <summary>
        /// 新建子表记录
        /// </summary>
        /// <returns></returns>
        public virtual RecData NewDRecord()
        {
            RecData rec = new RecData();
            FormDRec(rec);
            return rec;
        }
        /// <summary>
        /// 初始化子表列
        /// </summary>
        public virtual void InitialDCols()
        {
            AddNewDField(0, "序号", "序号", 12, (int)_alignType.atMiddle, (int)_dataType.dtString, 0);
        }
        /// <summary>
        /// 增加子表列
        /// </summary>
        /// <param name="col">列号</param>
        /// <param name="colname">列名</param>
        /// <param name="collabel">列标签，为空则默认为列名</param>
        /// <param name="colalign">列居中方式</param>
        /// <param name="coltype">列数据类型</param>
        /// <param name="coldec">列显示小数位数</param>
        /// <returns></returns>
        public ColDef AddNewDField(int col, string colname, string collabel, int colwidth, int colalign, int coltype, int coldec, bool hide = false, bool keycol = false)
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
            _dcolList.Add(coldef);
            return coldef;
        }
        /// <summary>
        /// 格式化表格记录
        /// </summary>
        /// <param name="rec">表格记录</param>
        public override void FormRec(RecData rec)
        {
            base.FormRec(rec);
            foreach (RecData drec in rec.childList)
            {
                FormDRec(drec);
                drec.ownerList = rec.childList;
            }
        }
        /// <summary>
        /// 格式化子表记录
        /// </summary>
        /// <param name="rec">表格记录</param>
        public virtual void FormDRec(RecData rec)
        {
            rec.fields = _dcolList;
            rec.ownerTable = this;
        }

        #endregion
    }
}
