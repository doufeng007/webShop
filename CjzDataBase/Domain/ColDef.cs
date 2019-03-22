using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace CjzDataBase
{
    /// <summary>
    /// 居中方式
    /// </summary>
    public enum _alignType
    {
        atLeft = 0,
        atMiddle = 1,
        atRight = 2,
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum _dataType
    {
        dtString = 0,
        dtInt = 1,
        dtBoolean = 2,
        dtFloat = 3,
    }

    /// <summary>
    /// 列属性类
    /// </summary>
    public class ColDef : DataPacket
    {
        #region 私有字段
        private int _colIndx;  // 列标识 
        private string _colName;  // 列名
        private string _colLabel;  // 列标签
        private int _colWidth;  // 列宽
        private int _colAlign;  // 列居中方式
        private int _colType;  // 列数据类型
        private int _colDec;  // 列数据显示小数位数
        private bool _hide;   // 隐藏列
        private bool _keyCol; // 标识列，用作对比时的关键字段


        #endregion

        #region 构造函数
        public ColDef()
        {
            _colWidth = 20;
            _colAlign = (int)_alignType.atLeft;
            _colType = (int)_dataType.dtString;
            _colDec = 2;
            _hide = false;
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 列标识
        /// </summary>
        public int colIndx { get { return _colIndx;} set { _colIndx = value;}} 
        /// <summary>
        /// 列名
        /// </summary>
        public string colName { get { return _colName; } set { _colName = value; } } 
        /// <summary>
        /// 列标签
        /// </summary>
        public string colLabel { get { return _colLabel; } set { _colLabel = value; } } 
        /// <summary>
        /// 列宽
        /// </summary>
        public int colWidth { get { return _colWidth; } set { _colWidth = value; } }  
        /// <summary>
        /// 列居中方式
        /// </summary>
        public int colAlign { get { return _colAlign; } set { _colAlign = value; } } 
        /// <summary>
        /// 列数据类型
        /// </summary>
        public int colType { get { return _colType; } set { _colType = value; } }  
        /// <summary>
        /// 列数据小数位数
        /// </summary>
        public int colDec { get { return _colDec; } set { _colDec = value; } }
        /// <summary>
        /// 隐藏列
        /// </summary>
        public bool hide { get { return _hide; } set { _hide = value; } }
        /// <summary>
        /// 标识列，用作对比时的关键字段
        /// </summary>
        public bool keyCol { get { return _keyCol; } set { _keyCol = value; } }
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
            _colIndx = 0;
            _colName = null;
            _colLabel = null;
            _colWidth = 0;
            _colAlign = 0;
            _colType = 0;
            _colDec = 0;
            _hide = false;
            _keyCol = false;
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            ColDef s = sou as ColDef;
            if (s != null)
            {
                _colIndx = s._colIndx;
                _colName = s._colName;
                _colLabel = s._colLabel;
                _colWidth = s._colWidth;
                _colAlign = s._colAlign;
                _colType = s._colType;
                _colDec = s._colDec;
                _hide = s._hide;
                _keyCol = s._keyCol;
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "ColIndx", _colIndx);
            WriteXmlAttrValue(node, "ColName", _colName);
            WriteXmlAttrValue(node, "ColLabel", _colLabel);
            WriteXmlAttrValue(node, "ColWid", _colWidth);
            WriteXmlAttrValue(node, "ColAlign", _colAlign);
            WriteXmlAttrValue(node, "ColType", _colType);
            WriteXmlAttrValue(node, "ColDec", _colDec);
            WriteXmlAttrValue(node, "Hide", _hide);
            WriteXmlAttrValue(node, "KeyCol", _keyCol);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "ColIndx", ref _colIndx);
            ReadXmlAttrValue(node, "ColName", ref _colName);
            ReadXmlAttrValue(node, "ColLabel", ref _colLabel);
            ReadXmlAttrValue(node, "ColWid", ref _colWidth);
            ReadXmlAttrValue(node, "ColAlign", ref _colAlign);
            ReadXmlAttrValue(node, "ColType", ref _colType);
            ReadXmlAttrValue(node, "ColDec", ref _colDec);
            ReadXmlAttrValue(node, "Hide", ref _hide);
            ReadXmlAttrValue(node, "KeyCol", ref _keyCol);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _colIndx);
            WriteToStream(stream, _colName);
            WriteToStream(stream, _colLabel);
            WriteToStream(stream, _colWidth);
            WriteToStream(stream, _colAlign);
            WriteToStream(stream, _colType);
            WriteToStream(stream, _colDec);
            WriteToStream(stream, _hide);
            WriteToStream(stream, _keyCol);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _colIndx);
            ReadFromStream(stream, out _colName);
            ReadFromStream(stream, out _colLabel);
            ReadFromStream(stream, out _colWidth);
            ReadFromStream(stream, out _colAlign);
            ReadFromStream(stream, out _colType);
            ReadFromStream(stream, out _colDec);
            ReadFromStream(stream, out _hide);
            ReadFromStream(stream, out _keyCol);
        }
        #endregion
    }
}
