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
    /// 数据基类
    /// </summary>
    public class DataPacket
    {

        #region 私有字段

        private string _UniqId;        // 数据标识
        private string _parentUid;
        private int _Tag;              // 数据标识号
        private int _Sid;              // 数据顺序号 
        private int _Version;          // 数据版本号

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化新实例
        /// </summary>
        public DataPacket()
        {
            _UniqId = Guid.NewGuid().ToString();
            _Tag = 0;
            _Sid = 0;
            _Version = 1000;
        
        }

        #endregion

        #region 属性定义

        /// <summary>
        /// 数据标识.
        /// </summary>
        public string UniqId { get {return _UniqId;} set { _UniqId = value; }}

        public string parentUid { get { return _parentUid; } set { _parentUid = value; } }

        /// <summary>
        /// 数据标识号.
        /// </summary>
        public int Tag { get { return _Tag; } set { _Tag = value; } }
        /// <summary>
        /// 数据顺序号.
        /// </summary>
        public int Sid { get { return _Sid; } set { _Sid = value; } }
        /// <summary>
        /// 数据版本号.
        /// </summary>
        public int Version { get { return _Version; } set { _Version = value; } }
        /// <summary>
        /// XML序列化文本
        /// </summary>
        public virtual string XMLText
        {
            get
            {
                XmlDocument xmlDocument = GetXmlDocument(string.Empty);
                return xmlDocument.InnerXml;
            }
            set
            {
                Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(value);
                    XmlNode xmlNode = this.GetRootNode(xmlDocument);
                    if (xmlNode != null)
                    {
                        XMLDecode(xmlNode);
                    }
                }
            }
        }

        #endregion

        #region 私有方法
        private string RootNodeName()
        {
            return "FDataPacket";
        }
        private static XmlDocument GetXmlDocument(string rootname, out XmlNode node, string encoding)
        {
            XmlDocument xmlDocument = new XmlDocument();
            if (!string.IsNullOrEmpty(encoding))
            {
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", encoding, null));
            }
            if (string.IsNullOrEmpty(rootname))
            {
                node = xmlDocument;
            }
            else
            {
                node = xmlDocument.CreateElement(rootname);
                xmlDocument.AppendChild(node);
            }
            return xmlDocument;
        }
        private XmlDocument GetXmlDocument(string encoding)
        {
            XmlNode node;
            XmlDocument result = DataPacket.GetXmlDocument(this.RootNodeName(), out node, encoding);
            this.XMLEncode(node);
            return result;
        }
        private XmlNode GetRootNode(XmlDocument xmldocument)
        {
            string text = this.RootNodeName();
            XmlNode result;
            if (string.IsNullOrEmpty(text))
            {
                result = xmldocument;
            }
            else
            {
                result = DataPacket.SelectChildNode(xmldocument, text);
            }
            return result;
        }
        #endregion

        #region 保护方法

        #endregion

        #region 公有方法

        /// <summary>
        /// 初始化所有字段
        /// </summary>
        public virtual void Clear()
        {
            this._Tag = 0;
            this._Sid = 0;
            this._UniqId = null;
        }

        /// <summary>
        /// 取类型短名称
        /// </summary>
        /// <returns>类型短名称</returns>
        public string GetTypeName()
        {
            return base.GetType().Name;
        }

        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public virtual void AssignFrom(DataPacket sou)
        {
            this._Tag = sou._Tag;
            this._Sid = sou._Sid;
            this._UniqId = sou._UniqId;
        }

        /// <summary>
        /// 创建副本
        /// </summary>
        /// <returns></returns>
        public DataPacket Clone()
        {
            DataPacket dataPacket = (DataPacket)Activator.CreateInstance(base.GetType());
            dataPacket.AssignFrom(this);
            return dataPacket;
        }

        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <returns>子节点</returns>
        public static XmlNode NewXmlChildNode(XmlNode node, string mc)
        {
            XmlNode xmlNode = node.OwnerDocument.CreateElement(mc);
            node.AppendChild(xmlNode);
            return xmlNode;
        }
        /// <summary>
        /// 返回子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <returns></returns>
        public static XmlNode SelectChildNode(XmlNode node, string mc)
        {
            XmlNode result = null;
            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                if (string.Compare(xmlNode.Name, mc, true) == 0)
                {
                    result = xmlNode;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 从Xml文件中读出对象
        /// </summary>
        /// <param name="FileName">文件名称</param>
        public void LoadFromXmlFile(string FileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(FileName);
            this.Clear();
            XmlNode xmlNode = xmlDocument.DocumentElement;
            if (xmlNode != null)
            {
                this.XMLDecode(xmlNode);
            }
        }

        /// <summary>
        /// 将对象写到Xml文件
        /// </summary>
        /// <param name="FileName">文件名称</param>
        public void WriteToXmlFile(string FileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode node = xmlDocument.CreateElement("DataPacket");
            xmlDocument.AppendChild(node);
            XMLEncode(node);
            xmlDocument.Save(FileName);
        }

        #region 各种类型对象读写Xml属性

        /// <summary>
        /// 写属性值到Xml节点
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="atrName">属性名称</param>
        /// <param name="Value">属性值</param>
        public static void WriteXmlAttribStr(XmlNode node, string atrName, string Value)
        {
            XmlAttribute xmlAttribute = node.OwnerDocument.CreateAttribute(atrName);
            xmlAttribute.Value = Value;
            node.Attributes.SetNamedItem(xmlAttribute);
        }
        /// <summary>
        /// 从Xml节点读出属性值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="atrName">属性名称</param>
        /// <returns>属性值</returns>
        public static string ReadXmlAttribStr(XmlNode node, string atrName)
        {
            XmlNode namedItem = node.Attributes.GetNamedItem(atrName);
            string text = (namedItem != null) ? namedItem.InnerText : null;
            if (string.IsNullOrEmpty(text))
            {
                text = null;
            }
            return text;
        }
        /// <summary>
        /// 写属性值到Xml节点, string 类型值, 缺省值为空串。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, string Value)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                WriteXmlAttribStr(node, mc, Value);
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, string 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref string Value)
        {
            Value = ReadXmlAttribStr(node, mc);
        }
        /// <summary>
        /// 写属性值到Xml节点, short 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, short Value)
        {
            if (Value != 0)
            {
                WriteXmlAttrValue(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, short 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref short Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!short.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写属性值到Xml节点, ushort 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, ushort Value)
        {
            if (Value != 0)
            {
                WriteXmlAttribStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, ushort 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref ushort Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!ushort.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写属性值到Xml节点, int 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, int Value)
        {
            if (Value != 0)
            {
                WriteXmlAttribStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, int 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref int Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!int.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写属性值到Xml节点, uint 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, uint Value)
        {
            if (Value != 0u)
            {
                WriteXmlAttribStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, uint 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref uint Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = 0u;
                return;
            }
            if (!uint.TryParse(text, out Value))
            {
                Value = 0u;
            }
        }
        /// <summary>
        /// 写属性值到Xml节点, long 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, long Value)
        {
            if (Value != 0L)
            {
                WriteXmlAttribStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, long 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref long Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = 0L;
                return;
            }
            if (!long.TryParse(text, out Value))
            {
                Value = 0L;
            }
        }
        /// <summary>
        /// 写属性值到Xml节点, bool 类型值, 缺省值为 false 。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, bool Value)
        {
            if (Value)
            {
                WriteXmlAttribStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, bool 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref bool Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = false;
                return;
            }
            if (!bool.TryParse(text, out Value))
            {
                Value = false;
            }
        }
        /// <summary>
        /// 写属性值到Xml节点, double 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, double Value)
        {
            if (Value != 0.0)
            {
                WriteXmlAttribStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, double 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref double Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = 0.0;
                return;
            }
            if (!double.TryParse(text, out Value))
            {
                Value = 0.0;
            }
        }
        /// <summary>
        /// 写值到Xml节点属性, decimal 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, decimal Value)
        {
            if (Value != 0m)
            {
                WriteXmlAttribStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, decimal 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref decimal Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = 0m;
                return;
            }
            if (!decimal.TryParse(text, out Value))
            {
                Value = 0m;
            }
        }
        /// 写属性值到Xml节点, DateTime 类型值, 缺省值为最小日期 DateTime.MinValue 。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, DateTime Value)
        {
            if (Value != DateTime.MinValue)
            {
                WriteXmlAttribStr(node, mc, CommUtils.DateTimeToString(Value));
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, DateTime 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref DateTime Value)
        {
            string text = ReadXmlAttribStr(node, mc);
            if (text == null)
            {
                Value = DateTime.MinValue;
                return;
            }
            Value = CommUtils.StringToDateTime(text);
        }
        /// <summary>
        /// 写属性值到Xml节点, byte[] 类型值, 缺省值为空数组。
        /// 以数组长度开头，然后是各下标数据，以‘，’分开。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlAttrValue(XmlNode node, string mc, byte[] Value)
        {
            string value = CommUtils.ArraybyteToStr(Value);
            if (!string.IsNullOrEmpty(value))
            {
                WriteXmlAttribStr(node, mc, value);
            }
        }
        /// <summary>
        /// 从Xml节点读出属性值, byte[] 类型
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlAttrValue(XmlNode node, string mc, ref byte[] Value)
        {
            string str = ReadXmlAttribStr(node, mc);
            Value = CommUtils.StrToArraybyte(str);
        }

        #endregion

        #region 各种格式对象读写Xml节点

        /// <summary>
        /// 从Xml子节点读出子节点字符串
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <returns>字符串</returns>
        public static string ReadXMLStr(XmlNode node, string mc)
        {
            string result = null;
            XmlNode xmlNode = SelectChildNode(node, mc);
            if (xmlNode != null)
            {
                result = xmlNode.InnerText;
                node.RemoveChild(xmlNode);
            }
            return result;
        }
        /// <summary>
        /// 写字符串到子节点
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value">字符串</param>
        public static void WriteXMLStr(XmlNode node, string mc, string Value)
        {
            XmlNode xmlNode = node.OwnerDocument.CreateElement(mc);
            xmlNode.InnerText = Value;
            node.AppendChild(xmlNode);
        }
        /// <summary>
        /// 写值到Xml子节点, string 类型值, 缺省值为空串。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, string Value)
        {
            if (Value != null && Value != "")
            {
                WriteXMLStr(node, mc, Value);
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, string 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref string Value)
        {
            Value = ReadXMLStr(node, mc);
        }
        /// <summary>
        /// 写值到Xml子节点, short 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, short Value)
        {
            if (Value != 0)
            {
                DataPacket.WriteXMLValue(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, short 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref short Value)
        {
            string text = DataPacket.ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!short.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, ushort 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, ushort Value)
        {
            if (Value != 0)
            {
                WriteXMLStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, ushort 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref ushort Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!ushort.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, int 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, int Value)
        {
            if (Value != 0)
            {
                WriteXMLStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, int 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref int Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!int.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, uint 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, uint Value)
        {
            if (Value != 0u)
            {
                WriteXMLStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, uint 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref uint Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = 0u;
                return;
            }
            if (!uint.TryParse(text, out Value))
            {
                Value = 0u;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, long 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, long Value)
        {
            if (Value != 0L)
            {
                WriteXMLStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, long 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref long Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = 0L;
                return;
            }
            if (!long.TryParse(text, out Value))
            {
                Value = 0L;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, bool 类型值, 缺省值为 false 。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, bool Value)
        {
            if (Value)
            {
                WriteXMLStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, bool 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref bool Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = false;
                return;
            }
            if (!bool.TryParse(text, out Value))
            {
                Value = false;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, double 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, double Value)
        {
            if (Value != 0.0)
            {
                WriteXMLStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, double 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref double Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = 0.0;
                return;
            }
            if (!double.TryParse(text, out Value))
            {
                Value = 0.0;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, decimal 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, decimal Value)
        {
            if (Value != 0m)
            {
                WriteXMLStr(node, mc, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, decimal 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref decimal Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = 0m;
                return;
            }
            if (!decimal.TryParse(text, out Value))
            {
                Value = 0m;
            }
        }
        /// <summary>
        /// 写值到Xml子节点, DateTime 类型值, 缺省值为最小日期 DateTime.MinValue 。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, DateTime Value)
        {
            if (Value != DateTime.MinValue)
            {
                WriteXMLStr(node, mc, CommUtils.DateTimeToString(Value));
            }
        }
        /// <summary>
        /// 从Xml子节点读出值, DateTime 类型值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, ref DateTime Value)
        {
            string text = ReadXMLStr(node, mc);
            if (text == null)
            {
                Value = DateTime.MinValue;
                return;
            }
            Value = CommUtils.StringToDateTime(text);
        }
        
        /// <summary>
        /// 写值到Xml节点, string 类型值, 缺省值为空串。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, string Value)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                WriteXmlNodeStr(node, Value);
            }
        }
        /// <summary>
        /// 从Xml节点读出值, string 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref string Value)
        {
            Value = ReadXmlNodeStr(node);
        }
        /// <summary>
        /// 写值到Xml节点, short 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, short Value)
        {
            if (Value != 0)
            {
                WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, short 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref short Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!short.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写值到Xml节点, ushort 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, ushort Value)
        {
            if (Value != 0)
            {
                WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, ushort 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref ushort Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!ushort.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写值到Xml节点, int 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, int Value)
        {
            if (Value != 0)
            {
                WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, int 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref int Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = 0;
                return;
            }
            if (!int.TryParse(text, out Value))
            {
                Value = 0;
            }
        }
        /// <summary>
        /// 写值到Xml节点, uint 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, uint Value)
        {
            if (Value != 0u)
            {
                WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, uint 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref uint Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = 0u;
                return;
            }
            if (!uint.TryParse(text, out Value))
            {
                Value = 0u;
            }
        }
        /// <summary>
        /// 写值到Xml节点, long 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, long Value)
        {
            if (Value != 0L)
            {
                WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, long 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref long Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = 0L;
                return;
            }
            if (!long.TryParse(text, out Value))
            {
                Value = 0L;
            }
        }
        /// <summary>
        /// 写值到Xml节点, bool 类型值, 缺省值为 false 。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, bool Value)
        {
            if (Value)
            {
                WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, bool 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref bool Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = false;
                return;
            }
            if (!bool.TryParse(text, out Value))
            {
                Value = false;
            }
        }
        /// <summary>
        /// 写值到Xml节点, double 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, double Value)
        {
            if (Value != 0.0)
            {
               WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, double 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref double Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = 0.0;
                return;
            }
            if (!double.TryParse(text, out Value))
            {
                Value = 0.0;
            }
        }
        /// <summary>
        /// 写值到Xml节点, decimal 类型值, 缺省值为0。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, decimal Value)
        {
            if (Value != 0m)
            {
               WriteXmlNodeStr(node, Value.ToString());
            }
        }
        /// <summary>
        /// 从Xml节点读出值, decimal 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref decimal Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = 0m;
                return;
            }
            if (!decimal.TryParse(text, out Value))
            {
                Value = 0m;
            }
        }
        /// <summary>
        /// 写值到Xml节点, DateTime 类型值, 缺省值为最小日期 DateTime.MinValue 。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void WriteXmlNodeValue(XmlNode node, string mc, DateTime Value)
        {
            if (Value != DateTime.MinValue)
            {
                WriteXmlNodeStr(node, CommUtils.DateTimeToString(Value));
            }
        }
        /// <summary>
        /// 从Xml节点读出值, DateTime 类型值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mc"></param>
        /// <param name="Value"></param>
        public static void ReadXmlNodeValue(XmlNode node, string mc, ref DateTime Value)
        {
            string text = ReadXmlNodeStr(node);
            if (text == null)
            {
                Value = DateTime.MinValue;
                return;
            }
            Value = CommUtils.StringToDateTime(text);
        }
        /// <summary>
        /// 从Xml节点读出字符串值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <returns>读出的字符串,空串用null表达</returns>
        public static string ReadXmlNodeStr(XmlNode node)
        {
            string text = node.InnerText;
            if (text == "")
            {
                text = null;
            }
            return text;
        }
        /// <summary>
        /// 写字符串值到Xml节点
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="Value">字符串值</param>
        public static void WriteXmlNodeStr(XmlNode node, string Value)
        {
            node.InnerText = Value;
        }
        
        #endregion

        /// <summary>
        /// DataPacket对象写入Xml子节点
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void WriteXMLValue(XmlNode node, string mc, DataPacket Value)
        {
            if (Value != null)
            {
                XmlNode xmlNode = node.OwnerDocument.CreateElement(mc);
                node.AppendChild(xmlNode);
                Value.XMLEncode(xmlNode);
            }
        }
        /// <summary>
        /// 从Xml子节点读出 <see cref="DataPacket" />对象。需与写入对象一致。
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="mc">子节点名称</param>
        /// <param name="Value"></param>
        public static void ReadXMLValue(XmlNode node, string mc, DataPacket Value)
        {
            XmlNode xmlNode = DataPacket.SelectChildNode(node, mc);
            if (xmlNode != null)
            {
                Value.XMLDecode(xmlNode);
                return;
            }
            if (Value != null)
            {
                Value.Clear();
            }
        }
        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node"></param>
        public virtual void XMLEncode(XmlNode node)
        {
            WriteXMLValue(node, "Tag", this._Tag);
            WriteXMLValue(node, "Sid", this._Sid);
            //WriteXMLValue(node, "UniqId", this._UniqId);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node"></param>
        public virtual void XMLDecode(XmlNode node)
        {
            ReadXMLValue(node, "Tag", ref this._Tag);
            ReadXMLValue(node, "Sid", ref this._Sid);
            //ReadXMLValue(node, "UniqId", ref _UniqId);
        }

        #region 将各种格式的数据读写流

        public static void WriteToStream(Stream stream, byte value)
        {
            stream.WriteByte(value);
        }
        public static void ReadFromStream(Stream stream, out byte value)
        {
            int num = stream.ReadByte();
            if (num == -1)
            {
                throw new Exception("Out of stream");
            }
            value = (byte)num;
        }
        public static void WriteToStream(Stream stream, bool value)
        {
            byte value2;
            if (value)
                value2 = 255;
            else
                value2 = 0;
            stream.WriteByte(value2);
        }
        public static void ReadFromStream(Stream stream, out bool value)
        {
            int num = stream.ReadByte();
            if (num == -1)
            {
                throw new Exception("Out of stream");
            }
            value = (num != 0);
        }
        public static void WriteToStream(Stream stream, int value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 4);
        }
        public static void ReadFromStream(Stream stream, out int value)
        {
            byte[] array = new byte[4];
            if (stream.Read(array, 0, 4) == 0)
            {
                throw new Exception("Out of stream");
            }
            value = BitConverter.ToInt32(array, 0);
        }
        public static void WriteToStream(Stream stream, uint value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 4);
        }
        public static void ReadFromStream(Stream stream, out uint value)
        {
            byte[] array = new byte[4];
            if (stream.Read(array, 0, 4) == 0)
            {
                throw new Exception("Out of stream");
            }
            value = (uint)BitConverter.ToInt32(array, 0);
        }
        public static void WriteToStream(Stream stream, short value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 2);
        }
        public static void ReadFromStream(Stream stream, out short value)
        {
            byte[] array = new byte[2];
            if (stream.Read(array, 0, 2) == 0)
            {
                throw new Exception("Out of stream");
            }
            value = BitConverter.ToInt16(array, 0);
        }
        public static void WriteToStream(Stream stream, ushort value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 2);
        }
        public static void ReadFromStream(Stream stream, out ushort value)
        {
            byte[] array = new byte[2];
            if (stream.Read(array, 0, 2) == 0)
            {
                throw new Exception("Out of stream");
            }
            value = (ushort)BitConverter.ToInt16(array, 0);
        }
        public static void WriteToStream(Stream stream, long value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 8);
        }
        public static void ReadFromStream(Stream stream, out long value)
        {
            byte[] array = new byte[8];
            if (stream.Read(array, 0, 8) == 0)
            {
                throw new Exception("Out of stream");
            }
            value = BitConverter.ToInt64(array, 0);
        }
        public static void WriteToStream(Stream stream, double value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 8);
        }
        public static void ReadFromStream(Stream stream, out double value)
        {
            byte[] array = new byte[8];
            if (stream.Read(array, 0, 8) == 0)
            {
                throw new Exception("Out of stream");
            }
            value = BitConverter.ToDouble(array, 0);
        }
        public static void WriteToStream(Stream stream, DateTime value)
        {
            WriteToStream(stream, value.ToOADate());
        }
        public static void ReadFromStream(Stream stream, out DateTime value)
        {
            double d;
            ReadFromStream(stream, out d);
            value = DateTime.FromOADate(d);
        }
        public static void WriteToStream(Stream stream, Guid value)
        {
            stream.Write(value.ToByteArray(), 0, 16);
        }
        public static void ReadFromStream(Stream stream, out Guid value)
        {
            byte[] array = new byte[16];
            if (stream.Read(array, 0, 16) == 0)
            {
                throw new Exception("Out of stream");
            }
            value = new Guid(array);
        }
        public static void WriteToStreamUTF8(Stream stream, string value)
        {
            int num;
            if (string.IsNullOrEmpty(value))
            {
                num = 0;
                WriteToStream(stream, num);
                return;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            num = bytes.Length;
            byte value2 = 0;
            WriteToStream(stream, num);
            stream.Write(bytes, 0, num);
            stream.WriteByte(value2);
        }
        public static void ReadFromStreamUTF8(Stream stream, out string value)
        {
            int num;
            ReadFromStream(stream, out num);
            if (num <= 0)
            {
                value = null;
                return;
            }
            byte[] array = new byte[num + 1];
            if (stream.Read(array, 0, num + 1) < num + 1)
            {
                throw new Exception("Out of stream");
            }
            value = Encoding.UTF8.GetString(array, 0, num);
        }
        public static void WriteToStream(Stream stream, string value)
        {
            ushort value2;
            if (string.IsNullOrEmpty(value))
            {
                value2 = 0;
                WriteToStream(stream, value2);
                return;
            }
            byte[] bytes = Encoding.GetEncoding(936).GetBytes(value);
            int num = bytes.Length;
            byte value3 = 0;
            if (num >= 65535)
            {
                value2 = 65535;
                WriteToStream(stream, value2);
                WriteToStream(stream, num);
                stream.Write(bytes, 0, num);
                stream.WriteByte(value3);
                return;
            }
            value2 = (ushort)num;
            WriteToStream(stream, value2);
            stream.Write(bytes, 0, num);
            stream.WriteByte(value3);
        }
        public static void ReadFromStream(Stream stream, out string value)
        {
            ushort num;
            ReadFromStream(stream, out num);
            int num2;
            if (num == 65535)
            {
                ReadFromStream(stream, out num2);
            }
            else
            {
                num2 = (int)num;
            }
            if (num2 <= 0)
            {
                value = null;
                return;
            }
            byte[] array = new byte[num2 + 1];
            if (stream.Read(array, 0, num2 + 1) < num2 + 1)
            {
                throw new Exception("Out of stream");
            }
            value = Encoding.GetEncoding(936).GetString(array, 0, num2);
        }

        #endregion

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public virtual void SaveToStream(Stream stream)
        {
            WriteToStream(stream, _Tag);
            WriteToStream(stream, _Sid);
            WriteToStream(stream, _UniqId);
            WriteToStream(stream, _Version);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public virtual void LoadFromStream(Stream stream)
        {
            ReadFromStream(stream, out _Tag);
            ReadFromStream(stream, out _Sid);
            ReadFromStream(stream, out _UniqId);
            ReadFromStream(stream, out _Version);
        }

        #endregion
    }
}
