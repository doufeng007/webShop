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
    public enum _ctaStatusEnum
    {
        csConsis = 0,  // 一致
        csDiff   = 1,  // 不一致
        csAdd    = 2,  // 多项
        csDecrease = 3,  // 漏项
    }

    public class CtaRecData: RecData
    {
        #region 私有字段
        private int _ctaStatus;  //  对比结果
        private RecData _souRecData; //  源数据记录
        private string _souRecId;  //  源CJZ源数据记录标识
        private string _souTableId;  //  对比CJZ源数据表格标识
        private string _souDocId;  //  源数据工程标识
        #endregion

        #region 构造函数
        public CtaRecData()
        {
            colList.ItemType = typeof(CtaColData);
            childList.ItemType = typeof(CtaRecData);
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 对比结果
        /// </summary>
        public int ctaStatus { get { return _ctaStatus; } set { _ctaStatus = value; } }
        /// <summary>
        /// 源数据记录
        /// </summary>
        public RecData souRecData { get { return _souRecData; } set { _souRecData = value; } }
        /// <summary>
        /// 源CJZ源数据记录标识
        /// </summary>
        public string souRecId { get { return _souRecId; } set { _souRecId = value; } }
        /// <summary>
        /// 对比CJZ源数据表格标识
        /// </summary>
        public string souTableId { get { return _souTableId; } set { _souTableId = value; } }
        /// <summary>
        /// 源数据工程标识
        /// </summary>
        public string souDocId { get { return _souDocId; } set { _souDocId = value; } }
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
            _ctaStatus = 0;
            _souRecData = null;
            _souRecId = null;
            _souTableId = null;
            _souDocId = null; 
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            CtaRecData s = sou as CtaRecData;
            if (s != null)
            {
                _ctaStatus = s._ctaStatus;
                _souRecId = s._souRecId;
                _souTableId = s._souTableId;
                _souDocId = s._souDocId;  
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
            WriteXmlAttrValue(node, "SouRecId", _souRecId);
            WriteXmlAttrValue(node, "SouTableId", _souTableId);
            WriteXmlAttrValue(node, "SouDocId", _souDocId);

        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "CtaStatus", ref _ctaStatus);
            ReadXmlAttrValue(node, "SouRecId", ref _souRecId);
            ReadXmlAttrValue(node, "SouTableId", ref _souTableId);
            ReadXmlAttrValue(node, "SouDocId", ref _souDocId);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _ctaStatus);
            WriteToStream(stream, _souRecId);
            WriteToStream(stream, _souTableId);
            WriteToStream(stream, _souDocId);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _ctaStatus);
            ReadFromStream(stream, out _souRecId);
            ReadFromStream(stream, out _souTableId);
            ReadFromStream(stream, out _souDocId);
        }
        /// <summary>
        /// 新建列记录
        /// </summary>
        /// <returns></returns>
        public override ColData NewColData()
        {
            return new CtaColData();
        }
        #endregion
    }
}
