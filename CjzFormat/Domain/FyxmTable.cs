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
    class FyxmTable: TableData
    {
        #region 私有字段
        private double _hj;  // 合计
        #endregion

        #region 构造函数

        #endregion

        #region 属性定义
        /// <summary>
        /// 合计
        /// </summary>
        public double hj { get { return _hj; } set { _hj = value; } }
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
            _hj = 0;
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            FyxmTable s = sou as FyxmTable;
            if (s != null)
            {
                _hj = s._hj;
            }
        }

        /// <summary>
        /// 将对象写入XML节点
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLEncode(XmlNode node)
        {
            base.XMLEncode(node);
            WriteXmlAttrValue(node, "hj", _hj);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "hj", ref _hj);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _hj);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _hj);
        }
        /// <summary>
        /// 新建表格记录
        /// </summary>
        /// <returns></returns>
        public override RecData NewRecord()
        {
            FyxmRec rec = new FyxmRec();
            FormRec(rec);
            return rec;
        }
        /// <summary>
        /// 初始化列
        /// </summary>
        public override void InitialCols()
        {
            base.InitialCols();
            //AddNewField(1, "编号", "编号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(2, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(3, "公式", "公式", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(4, "计算式", "计算式", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(5, "工程量", "工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(6, "单位", "单位", 28, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(7, "单价", "单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(8, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(9, "金额", "金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(10, "费用变量", "费用变量", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(11, "费用类别", "费用类别", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(12, "招标编码", "招标编码", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
        }

        #endregion
    }
}
