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
    /// 列数据类
    /// </summary>
    public class ColData : DataPacket
    {
        #region 私有字段
        private int _colIndex; //  列标识
        private string _express; //  数据值

        #endregion

        #region 构造函数
        public ColData()
        {
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 列标识
        /// </summary>
        public int colIndex { get { return _colIndex; } set { _colIndex = value; } }
        /// <summary>
        /// 数据值
        /// </summary>
        public string express { get { return _express; } set { _express = value; } }
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
            _colIndex = 0;
            _express = null;
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            ColData s = sou as ColData;
            if (s != null)
            {
                _colIndex = s.colIndex;
                _express = s._express;
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node"></param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "colIndex", _colIndex);
            WriteXmlAttrValue(node, "express", _express);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node"></param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "colIndex", ref _colIndex);
            ReadXmlAttrValue(node, "express", ref _express);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _colIndex);
            WriteToStream(stream, _express);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _colIndex);
            ReadFromStream(stream, out _express);
        }
        #endregion
    }
}
