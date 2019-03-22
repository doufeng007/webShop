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
    class QdxmTable: MDTableData
    {
        #region 私有字段
        private double _hj;  // 合计
        private double _zgj; // 暂估价
        private double _aqsgf; // 安全文明施工费

        #endregion

        #region 构造函数
        public QdxmTable()
        { 
        }

        #endregion

        #region 属性定义
        /// <summary>
        /// 合计
        /// </summary>
        public double hj { get { return _hj; } set { _hj = value; } }
        /// <summary>
        /// 暂估价
        /// </summary>
        public double zgj { get { return _zgj; } set { _zgj = value; } }
        /// <summary>
        /// 安全文明施工费
        /// </summary>
        public double aqsgf { get { return _aqsgf; } set { _aqsgf = value; } }
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
            _zgj = 0;
            _aqsgf = 0;
        }
        /// <summary>
        /// 复制数据对象
        /// </summary>
        /// <param name="sou">源对象,需从DataPacket继承</param>
        public override void AssignFrom(DataPacket sou)
        {
            base.AssignFrom(sou);
            QdxmTable s = sou as QdxmTable;
            if (s != null)
            {
                _hj = s._hj;
                _zgj = s._zgj;
                _aqsgf = s._aqsgf;
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
            WriteXmlAttrValue(node, "zgj", _zgj);
            WriteXmlAttrValue(node, "aqsgf", _aqsgf);
        }
        /// <summary>
        /// 从XML节点中读出对象
        /// </summary>
        /// <param name="node">XML节点</param>
        public override void XMLDecode(XmlNode node)
        {
            base.XMLDecode(node);
            ReadXmlAttrValue(node, "hj", ref _hj);
            ReadXmlAttrValue(node, "zgj", ref _zgj);
            ReadXmlAttrValue(node, "aqsgf", ref _aqsgf);
        }

        /// <summary>
        /// 将对象写入到流
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            WriteToStream(stream, _hj);
            WriteToStream(stream, _zgj);
            WriteToStream(stream, _aqsgf);
        }
        /// <summary>
        /// 从流中读出对象
        /// </summary>
        /// <param name="stream">I/O流</param>
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            ReadFromStream(stream, out _hj);
            ReadFromStream(stream, out _zgj);
            ReadFromStream(stream, out _aqsgf);
        }

        /// <summary>
        /// 新建表格记录
        /// </summary>
        /// <returns></returns>
        public override RecData NewRecord()
        {
            QdxmRec rec = new QdxmRec();
            FormRec(rec);
            return rec;
        }

        public override RecData NewDRecord()
        {
            MxclRec drec = new MxclRec();
            FormDRec(drec);
            return drec;
        }

        /// <summary>
        /// 初始化列
        /// </summary>
        public override void InitialCols()
        {
            base.InitialCols();
            //AddNewField(1, "项目编码", "项目编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(2, "项目名称", "项目名称", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(3, "计量单位", "计量单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(4, "工程量", "工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            //AddNewField(4, "工程量计算式", "工程量计算式", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            //AddNewField(5, "消耗量", "消耗量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 3);
            //AddNewField(6, "综合单价", "综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(7, "综合合价", "综合合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(8, "人工费", "人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(9, "人工费合价", "人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(10, "材料费", "材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(1, "材料费合价", "材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(12, "施工机具使用费", "施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(13, "施工机具使用费合价", "施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(14, "管理费率", "管理费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(15, "管理费", "管理费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(16, "管理费合价", "管理费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(17, "主材费", "主材费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(18, "主材合价", "主材合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(19, "设备单价", "设备单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(20, "设备合价", "设备合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(21, "材料暂估单价", "材料暂估单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(22, "材料暂估合价", "材料暂估合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(23, "最高限价", "最高限价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(24, "最低限价", "最低限价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(25, "利润率", "利润率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(26, "利润", "利润", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(27, "利润合价", "利润合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(28, "人工费调整费率", "人工费调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(29, "人工费调整单价", "人工费调整单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(30, "人工费调整合价", "人工费调整合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(31, "材料费调整费率", "材料费调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(32, "材料费调整单价", "材料费调整单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(33, "材料费调整合价", "材料费调整合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(34, "机械费调整费率", "机械费调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(35, "机械费调整单价", "机械费调整单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(36, "机械费调整合价", "机械费调整合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(37, "综合费率", "综合费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(38, "综合费单价", "综合费单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(39, "综合费合价", "综合费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(40, "定额单价", "定额单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(41, "定额合价", "定额合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(42, "定额人工费", "定额人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(43, "定额人工费合价", "定额人工费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(44, "定额材料费", "定额材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(45, "定额材料费合价", "定额材料费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(46, "定额施工机具使用费", "定额施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(47, "定额施工机具使用费合价", "定额施工机具使用费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(48, "定额管理费率", "定额管理费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(49, "定额管理费单价", "定额管理费单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(50, "定额管理费合价", "定额管理费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(51, "定额利润率", "定额利润率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(52, "定额利润单价", "定额利润单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(53, "定额利润合价", "定额利润合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(54, "定额综合费率", "定额综合费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(55, "定额综合费单价", "定额综合费单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(56, "定额综合费合价", "定额综合费合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(57, "单价构成文件ID", "单价构成文件ID", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(58, "主要清单标志", "主要清单标志", 6, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(59, "暂估清单", "暂估清单", 6, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(60, "清单类别", "清单类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(61, "定额专业类别", "定额专业类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(62, "项目特征", "项目特征", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(63, "工作内容", "工作内容", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(64, "换算描述", "换算描述", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(65, "备注", "备注", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(66, "费用类别", "费用类别", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(67, "费用变量", "费用变量", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(68, "取费基础表达式", "取费基础表达式", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(69, "取费基础说明", "取费基础说明", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(70, "取费基础金额", "取费基础金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(71, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(72, "按费率计取", "按费率计取", 15, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(73, "已标价工程量", "已标价工程量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(74, "已标价综合单价", "已标价综合单价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(75, "已标价人工费", "已标价人工费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(76, "已标价材料费", "已标价材料费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(77, "已标价施工机具使用费", "已标价施工机具使用费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(78, "已标价综合费", "已标价综合费", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(79, "其他信息", "其他信息", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(80, "数量计算方式", "数量计算方式", 15, (int)_alignType.atMiddle, (int)_dataType.dtInt, 0);
            //AddNewField(81, "不计价", "不计价", 15, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(82, "调整费率", "调整费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(83, "调整后金额", "调整后金额", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(84, "招标编码", "招标编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
        }

        /// <summary>
        /// 初始化子表列
        /// </summary>
        public override void InitialDCols()
        {
            base.InitialDCols();
            //AddNewDField(1, "关联材料代码", "关联材料代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(2, "代码", "代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0, true);
            //AddNewDField(3, "名称", "名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(4, "型号规格", "型号规格", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(5, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(6, "消耗量", "消耗量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewDField(7, "不计价", "不计价", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
        }

        #endregion
    }
}
