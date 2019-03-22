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
    public class CtaColData : ColData
    {
        #region 私有字段
        private int _ctaStatus;   // 对比结果

        #endregion

        #region 构造函数
        public CtaColData()
        {
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 对比结果
        /// </summary>
        public int ctaStatus { get { return _ctaStatus; } set { _ctaStatus = value; } }
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
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            CtaColData s = sou as CtaColData;
            if (s != null)
            {
                _ctaStatus = s._ctaStatus;
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node"></param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "CtaStatus", _ctaStatus);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node"></param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "CtaStatus", ref _ctaStatus);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _ctaStatus);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _ctaStatus);
        }
        #endregion
    }
}
