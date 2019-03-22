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
    public class CtaData : DataPacket
    {
        #region 私有字段
        private string _fileName;  //  主CJZ文件
        private string _ctaFileName; //  对比CJZ文件
        private CjzData _cjzData;  //  源CJZ数据对象
        private CjzData _ctaCjzData;  //  对比CJZ数据对象
        private DataList _docList;  //  工程列表
        #endregion

        #region 构造函数
        public CtaData()
        {
            _docList = new DataList();
            _docList.ItemType = typeof(CtaDocData);
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 主CJZ文件
        /// </summary>
        public string fileName { get { return _fileName; } set { _fileName = value; } }
        /// <summary>
        /// 对比CJZ文件
        /// </summary>
        public string ctaFileName { get { return _ctaFileName; } set { _ctaFileName = value; } }
        /// <summary>
        /// 源CJZ数据对象
        /// </summary>
        public CjzData cjzData { get { return _cjzData; } set { _cjzData = value; } }
        /// <summary>
        /// 对比CJZ数据对象
        /// </summary>
        public CjzData ctaCjzData { get { return _ctaCjzData; } set { _ctaCjzData = value; } }
        /// <summary>
        /// 工程列表
        /// </summary>
        public DataList docList { get { return _docList; } }
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
            _fileName = null;  // 主CJZ文件
            _ctaFileName = null;  // 对比CJZ文件
            _cjzData = null;  // 源CJZ数据对象
            _ctaCjzData = null;  //对比CJZ数据对象
            _docList.Clear();  // 工程列表
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            CtaData s = sou as CtaData;
            if (s != null)
            {
                _fileName = s._fileName;  // 主CJZ文件
                _ctaFileName = s._ctaFileName;  // 对比CJZ文件
                _docList.AssignFrom(s._docList);  // 工程列表
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "FileName", _fileName);
            WriteXmlAttrValue(node, "CtaFileName", _ctaFileName);
            WriteXMLValue(node, "DocList", _docList);

        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "FileName", ref _fileName);
            ReadXmlAttrValue(node, "CtaFileName", ref _ctaFileName);
            ReadXMLValue(node, "DocList", _docList);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _fileName);
            WriteToStream(stream, _ctaFileName);
            _docList.SaveToStream(stream);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _fileName);
            ReadFromStream(stream, out _ctaFileName);
            _docList.LoadFromStream(stream);
        }
        #endregion
        

    }
}
