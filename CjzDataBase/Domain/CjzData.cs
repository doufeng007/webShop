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
    /// CJZ序列化数据类
    /// </summary>
    public class CjzData : DataPacket
    {
        #region 私有字段
        string _fileName;  // 文件名称
        string _projectCode;  // 项目编号
        string _projectName;  // 项目名称
        string _bidTitle;  // 标段名称
        string _constructionUnit;  //建设单位
        string _projectAddress;  // 工程地点
        string _projectScale;  // 工程规模
        string _standardName;  // 标准名称
        string _standardVersion;  // 版本号
        string _calcMethod;  // 计价方式
        string _dataType;  // 数据类型
        string _listPriceRule;  // 清单计价规则
        string _fixedPriceRul;  // 定额计价规则
        string _description;  // 编制说明
        string _writeDate; //编制日期
        double _projectPrice;  // 工程费用
        DataList _docList;  // 工程列表


        #endregion

        #region 构造函数
        public CjzData()
        {
            _docList = new DataList();
            _docList.ItemType = typeof(DocData);
        
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 文件名称
        /// </summary>
        public string fileName { get { return _fileName; } set { _fileName = value; }}
        /// <summary>
        /// 项目编号
        /// </summary>
        public string projectCode { get { return _projectCode; } set { _projectCode = value;}}
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectName { get { return _projectName; } set { _projectName = value; } }
        /// <summary>
        /// 标段名称
        /// </summary>
        public string bidTitle { get { return _bidTitle; } set { _bidTitle = value; } }
        /// <summary>
        /// 建设单位
        /// </summary>
        public string constructionUnit { get { return _constructionUnit; } set { _constructionUnit = value; } }
        /// <summary>
        /// 工程地点
        /// </summary>
        public string projectAddress { get { return _projectAddress; } set { _projectAddress = value; } }
        /// <summary>
        /// 工程规模
        /// </summary>
        public string projectScale { get { return _projectScale; } set { _projectScale = value; } }
        /// <summary>
        /// 标准名称
        /// </summary>
        public string standardName { get { return _standardName; } set { _standardName = value; } }
        /// <summary>
        /// 版本号
        /// </summary>
        public string standardVersion { get { return _standardVersion; } set { _standardVersion = value; } }
        /// <summary>
        /// 计价方式
        /// </summary>
        public string calcMethod { get { return _calcMethod; } set { _calcMethod = value; } }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string dataType { get { return _dataType; } set { _dataType = value; } }
        /// <summary>
        /// 清单计价规则
        /// </summary>
        public string listPriceRule { get { return _listPriceRule; } set { _listPriceRule = value; } }
        /// <summary>
        /// 定额计价规则
        /// </summary>
        public string fixedPriceRul { get { return _fixedPriceRul; } set { _fixedPriceRul = value; } }
        /// <summary>
        /// 编制说明
        /// </summary>
        public string description { get { return _description; } set { _description = value; } }
        /// <summary>
        /// 编制说明
        /// </summary>
        public string writeDate { get { return _writeDate; } set { _writeDate = value; } }

        /// <summary>
        /// 工程费用
        /// </summary>
        public double projectPrice { get { return _projectPrice; } set { _projectPrice = value; } }
        /// <summary>
        /// 工程列表
        /// </summary>
        public DataList docList { get { return _docList; } }
        /// <summary>
        /// 工程数量
        /// </summary>
        public int DocCount { get { return _docList.Count; } }

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
            _fileName = null;  // 文件名称
            _projectCode = null;  // 项目编号
            _projectName = null;  // 项目名称
            _bidTitle = null;  // 标段名称
            _constructionUnit = null;  //建设单位
            _projectAddress = null; // 工程地点
            _projectScale = null;  // 工程规模
            _standardName = null;  // 标准名称
            _standardVersion = null;  // 版本号
            _calcMethod = null;  // 计价方式
            _dataType = null;  // 数据类型
            _listPriceRule = null;  // 清单计价规则
            _fixedPriceRul = null;  // 定额计价规则
            _description = null;
            _projectPrice = 0;
            _docList.Clear();  // 工程列表
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            CjzData s = sou as CjzData;
            if (s != null)
            {
                _fileName = s._fileName;  // 文件名称
                _projectCode = s._projectCode;  // 项目编号
                _projectName = s._projectName;  // 项目名称
                _bidTitle = s._bidTitle;  // 标段名称
                _constructionUnit = s._constructionUnit;  //建设单位
                _projectAddress = s._projectAddress;  // 工程地点
                _projectScale = s._projectScale;  // 工程规模
                _standardName = s._standardName;  // 标准名称
                _standardVersion = s.standardVersion;  // 版本号
                _calcMethod = s._calcMethod;  // 计价方式
                _dataType = s._dataType;  // 数据类型
                _listPriceRule = s._listPriceRule;  // 清单计价规则
                _fixedPriceRul = s._fixedPriceRul;  // 定额计价规则
                _description = s._description;
                _projectPrice = s._projectPrice;
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
            WriteXmlAttrValue(node, "ProjectCode", _projectCode);
            WriteXmlAttrValue(node, "ProjectName", _projectName);
            WriteXmlAttrValue(node, "BidTitle", _bidTitle);
            WriteXmlAttrValue(node, "ConstructionUnit", _constructionUnit);
            WriteXmlAttrValue(node, "ProjectAddress", _projectAddress);
            WriteXmlAttrValue(node, "ProjectScale", projectScale);
            WriteXmlAttrValue(node, "StandardName", _standardName);
            WriteXmlAttrValue(node, "StandardVersion", standardVersion);
            WriteXmlAttrValue(node, "CalcMethod", _calcMethod);
            WriteXmlAttrValue(node, "DataType", _dataType);
            WriteXmlAttrValue(node, "ListPriceRule", _listPriceRule);
            WriteXmlAttrValue(node, "FixedPriceRul", _fixedPriceRul);
            WriteXmlAttrValue(node, "Description", _description);
            WriteXmlAttrValue(node, "ProjectPrice", _projectPrice);
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
            ReadXmlAttrValue(node, "ProjectCode", ref _projectCode);
            ReadXmlAttrValue(node, "ProjectName", ref _projectName);
            ReadXmlAttrValue(node, "BidTitle", ref _bidTitle);
            ReadXmlAttrValue(node, "ConstructionUnit", ref _constructionUnit);
            ReadXmlAttrValue(node, "ProjectAddress", ref _projectAddress);
            ReadXmlAttrValue(node, "ProjectScale", ref  _projectScale);
            ReadXmlAttrValue(node, "StandardName", ref _standardName);
            ReadXmlAttrValue(node, "StandardVersion", ref  _standardVersion);
            ReadXmlAttrValue(node, "CalcMethod", ref _calcMethod);
            ReadXmlAttrValue(node, "DataType", ref _dataType);
            ReadXmlAttrValue(node, "ListPriceRule", ref _listPriceRule);
            ReadXmlAttrValue(node, "FixedPriceRul", ref _fixedPriceRul);
            ReadXmlAttrValue(node, "Description", ref _description);
            ReadXmlAttrValue(node, "ProjectPrice", ref _projectPrice);
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
            WriteToStream(stream, _projectCode);
            WriteToStream(stream, _projectName);
            WriteToStream(stream, _bidTitle);
            WriteToStream(stream, _constructionUnit);
            WriteToStream(stream, _projectAddress);
            WriteToStream(stream, _projectScale);
            WriteToStream(stream, _standardName);
            WriteToStream(stream, _standardVersion);
            WriteToStream(stream, _calcMethod);
            WriteToStream(stream, _dataType);
            WriteToStream(stream, _listPriceRule);
            WriteToStream(stream, _fixedPriceRul);
            WriteToStream(stream, _description);
            WriteToStream(stream, _projectPrice);
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
            ReadFromStream(stream, out _projectCode);
            ReadFromStream(stream, out _projectName);
            ReadFromStream(stream, out _bidTitle);
            ReadFromStream(stream, out _constructionUnit);
            ReadFromStream(stream, out _projectAddress);
            ReadFromStream(stream, out _projectScale);
            ReadFromStream(stream, out _standardName);
            ReadFromStream(stream, out _standardVersion);
            ReadFromStream(stream, out _calcMethod);
            ReadFromStream(stream, out _dataType);
            ReadFromStream(stream, out _listPriceRule);
            ReadFromStream(stream, out _fixedPriceRul);
            ReadFromStream(stream, out _description);
            ReadFromStream(stream, out _projectPrice);
            _docList.LoadFromStream(stream);
        }

        #endregion

    }
}
