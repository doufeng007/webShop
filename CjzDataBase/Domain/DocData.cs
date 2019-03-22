using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Reflection;

namespace CjzDataBase
{
    /// <summary>
    /// 工程类
    /// </summary>
    public class DocData : DataPacket
    {
        #region 私有字段

        private string _docName;   // 工程名称
        private string _docCategory;   // 工程类别
        private string _docScale;   //	工程规模
        private string _docSpecialty;   //	工程专业
        private int    _docLevel;   //	工程级别
        private string _docType;   //	工程类型
        private string _fefFile;   //	关联文件
        private DataList _tableList;   //	表格列表


        #endregion

        #region 构造函数
        public DocData()
        {
            _tableList = new DataList();
            _tableList.ItemType = typeof(TableData);
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 工程名称.
        /// </summary>
        public string docName { get { return _docName;} set { _docName = value;}}  
        /// <summary>
        /// 工程类别.
        /// </summary>
        public string docCategory { get { return _docCategory; } set { _docCategory = value; } } 
        /// <summary>
        /// 工程规模.
        /// </summary>
        public string docScale { get { return _docScale; } set { _docScale = value; } }  
        /// <summary>
        /// 工程专业.
        /// </summary>
        public string docSpecialty { get { return _docSpecialty; } set { _docSpecialty = value; } }   
        /// <summary>
        /// 工程级别.
        /// </summary>
        public int docLevel { get { return _docLevel; } set { _docLevel = value; } }  
        /// <summary>
        /// 工程类型.
        /// </summary>
        public string docType { get { return _docType; } set { _docType = value; } } 
        /// <summary>
        /// 关联文件.
        /// </summary>
        public string defFile { get { return _fefFile; } set { _fefFile = value; } }
        /// <summary>
        /// 表格列表.
        /// </summary>
        public DataList tableList { get { return _tableList; } }  

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
            _docName = null;   // 工程名称
            _docCategory = null;   // 工程类别
            _docScale = null;   //	工程规模
            _docSpecialty = null;   //	工程专业
            _docLevel = 1;   //	工程级别
            _docType = null;   //	工程类别
            _fefFile = null;   //	关联文件
            _tableList.Clear();   //	表格列表
        }

        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            DocData s = sou as DocData;
            if (s != null)
            {
                _docName = s._docName;   // 工程名称
                _docCategory = s._docCategory;   // 工程类别
                _docScale = s._docScale;   //	工程规模
                _docSpecialty = s._docSpecialty;   //	工程专业
                _docLevel = s._docLevel;   //	工程级别
                _docType = s._docType;   //	工程类别
                _fefFile = s._fefFile;   //	关联文件
                _tableList.AssignFrom(s._tableList);   //	表格列表
            }

        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "DocName", _docName);   // 工程名称
            WriteXmlAttrValue(node, "DocCategory", _docCategory);   // 工程类别
            WriteXmlAttrValue(node, "DocScale", _docScale);   //	工程规模
            WriteXmlAttrValue(node, "DocSpecialty", _docSpecialty);   //	工程专业
            WriteXmlAttrValue(node, "DocLevel", _docLevel);   //	工程级别
            WriteXmlAttrValue(node, "DocType", _docType);   //	工程类别
            WriteXmlAttrValue(node, "RefFile", _fefFile);   //	关联文件
            //_tableList.XMLEncode(node);   //	表格列表
            XmlNode nodes = node.OwnerDocument.CreateElement("TableList");
            node.AppendChild(nodes);
            foreach (TableData table in _tableList)
            {
                XmlNode tablenode = node.OwnerDocument.CreateElement("Table");
                nodes.AppendChild(tablenode);
                WriteXmlAttrValue(tablenode, "TableTypeName", table.GetType().FullName);
                table.XMLEncode(tablenode);
            };
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "DocName", ref _docName);   // 工程名称
            ReadXmlAttrValue(node, "DocCategory", ref _docCategory);   // 工程类别
            ReadXmlAttrValue(node, "DocScale", ref _docScale);   //	工程规模
            ReadXmlAttrValue(node, "DocSpecialty", ref _docSpecialty);   //	工程专业
            ReadXmlAttrValue(node, "DocLevel", ref _docLevel);   //	工程级别
            ReadXmlAttrValue(node, "DocType", ref _docType);   //	工程类别
            ReadXmlAttrValue(node, "RefFile", ref _fefFile);   //	关联文件
            //_tableList.XMLDecode(node);   //	表格列表
            // 这里处理表格反序列化
            XmlNode nodes = node.SelectSingleNode("TableList");
            if (nodes != null)
            {
                foreach (XmlNode cNode in nodes.ChildNodes)
                {
                    string tableTypeName = "";
                    ReadXmlAttrValue(cNode, "TableTypeName", ref tableTypeName);
                    if (!string.IsNullOrEmpty(tableTypeName))
                    {
                        Assembly asm = Assembly.GetExecutingAssembly();
                        TableData tbl = asm.CreateInstance(tableTypeName) as TableData;
                        if (tbl != null)
                        {
                            tbl.XMLDecode(cNode);
                            _tableList.Add(tbl);
                        }
                    }
                }

            };
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _docName);   // 工程名称
            WriteToStream(stream, _docCategory);   // 工程类别
            WriteToStream(stream, _docScale);   //	工程规模
            WriteToStream(stream, _docSpecialty);   //	工程专业
            WriteToStream(stream, _docLevel);   //	工程级别
            WriteToStream(stream, _docType);   //	工程类别
            WriteToStream(stream, _fefFile);   //	关联文件
            //_tableList.SaveToStream(stream);   //	表格列表
            WriteToStream(stream, _tableList.Count);
            foreach (TableData table in _tableList)
            {
                WriteToStream(stream, table.GetType().FullName);
                table.SaveToStream(stream);
            };
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _docName);   // 工程名称
            ReadFromStream(stream, out _docCategory);   // 工程类别
            ReadFromStream(stream, out _docScale);   //	工程规模
            ReadFromStream(stream, out _docSpecialty);   //	工程专业
            ReadFromStream(stream, out _docLevel);   //	工程级别
            ReadFromStream(stream, out _docType);   //	工程类别
            ReadFromStream(stream, out _fefFile);   //	关联文件
            //_tableList.LoadFromStream(stream);   //	表格列表
            int cnt = 0;
            ReadFromStream(stream, out cnt);
            for (int i = 1; i <= cnt; i++)
            {
                string tabletypename;
                ReadFromStream(stream, out tabletypename);
                if (!string.IsNullOrEmpty(tabletypename))
                {
                    Assembly asm = Assembly.GetExecutingAssembly();
                    TableData tbl = asm.CreateInstance(tabletypename) as TableData;
                    if (tbl != null)
                    {
                        tbl.LoadFromStream(stream);
                        _tableList.Add(tbl);
                    }
                }
            }
        }
        /// <summary>
        /// 根据表名取表格
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public TableData TableByName(string tablename)
        {
            return _tableList.Cast<TableData>().FirstOrDefault(a => a.tableName == tablename);
        }
        #endregion
    }
}
