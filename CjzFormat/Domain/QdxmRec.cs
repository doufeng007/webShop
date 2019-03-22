using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CjzDataBase;
using System.Xml;
using System.IO;

namespace CjzFormat
{
    /// <summary>
    /// 清单类型
    /// </summary>
    public enum _qdType
    {
        qtNone = 0,
        qtFbxm = 1,
        qtQdxm = 2,
        qtDexm = 3,
        qtClxm = 4,
    }

    public class QdxmRec : RecData
    {
        #region 私有字段
        private int _qdtype;  // 清单类型
        #endregion

        #region 构造函数
        public QdxmRec()
        {
            childList.ItemType = typeof(MxclRec);
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 清单类型
        /// </summary>
        public int qdtype { get { return _qdtype; } set { _qdtype = value; } }

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
            _qdtype = (int)_qdType.qtNone;
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            QdxmRec s = sou as QdxmRec;
            if (s != null)
            {
                _qdtype = s._qdtype;
            }

        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "qdtype", _qdtype);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "qdtype", ref _qdtype);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _qdtype);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _qdtype);
        }

        #endregion
    }
}
