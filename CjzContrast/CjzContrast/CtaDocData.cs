using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Reflection;
using CjzDataBase;

namespace CjzContrast
{
    public class CtaDocData : DocData
    {
        #region 私有字段
        private DocData _docData;  //  源工程数据对象
        private DocData _ctaDocData; //  对比工程数据对象
        private string _docDataId;  // 源工程数据对象Id
        private string _ctaDocDataId; // 对比工程数据对象Id
        private int _ctaStatus;  //  对比结果
        private int _errorCount; //  错误数量

        #endregion

        #region 构造函数
        public CtaDocData()
        {
            tableList.ItemType = typeof(CtaDzTableData);
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 源工程数据对象.
        /// </summary>
        public DocData docData { get { return _docData; } set { _docData = value; } }
        /// <summary>
        /// 对比工程数据对象.
        /// </summary>
        public DocData ctaDocData { get { return _ctaDocData; } set { _ctaDocData = value; } }
        /// <summary>
        /// 源工程数据对象Id
        /// </summary>
        public string docDataId { get { return _docDataId; } set { _docDataId = value; } }
        /// <summary>
        /// 对比工程数据对象Id
        /// </summary>
        public string ctaDocDataId { get { return _ctaDocDataId; } set { _ctaDocDataId = value; } }
        /// <summary>
        /// 对比结果
        /// </summary>
        public int ctaStatus { get { return _ctaStatus; } set { _ctaStatus = value; } }
        /// <summary>
        /// 错误数量
        /// </summary>
        public int errorCount { get { return _errorCount; } set { _errorCount = value; } }

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
            _docData = null;   // 源工程数据对象
            _ctaDocData = null;   //	对比工程数据对象
            _docDataId = null;
            _ctaDocDataId = null;
            _ctaStatus = (int)_ctaStatusEnum.csConsis;
            _errorCount = 0;
        }

        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            CtaDocData s = sou as CtaDocData;
            if (s != null)
            {
                _docDataId = s._docDataId;
                _ctaDocDataId = s._ctaDocDataId;
                _ctaStatus = s._ctaStatus;
                _errorCount = s._errorCount;
            }

        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "DocDataId", _docDataId);
            WriteXmlAttrValue(node, "CtaDocDataId", _ctaDocDataId);
            WriteXmlAttrValue(node, "CtaStatus", _ctaStatus);
            WriteXmlAttrValue(node, "ErrorCount", _errorCount);
            XmlNode nodes = node.OwnerDocument.CreateElement("TableList");
            node.AppendChild(nodes);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "DocDataId", ref _docDataId);
            ReadXmlAttrValue(node, "CtaDocDataId", ref _ctaDocDataId);
            ReadXmlAttrValue(node, "CtaStatus", ref _ctaStatus);
            ReadXmlAttrValue(node, "ErrorCount", ref _errorCount);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _docDataId);
            WriteToStream(stream, _ctaDocDataId);
            WriteToStream(stream, _ctaStatus);
            WriteToStream(stream, _errorCount);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _docDataId);
            ReadFromStream(stream, out _ctaDocDataId);
            ReadFromStream(stream, out _ctaStatus);
            ReadFromStream(stream, out _errorCount);
        }
        #endregion
    }
}
