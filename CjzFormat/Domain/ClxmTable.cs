using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CjzDataBase;

namespace CjzFormat
{
    public class ClxmTable : MDTableData
    {
        #region 私有字段
        #endregion

        #region 构造函数
        #endregion

        #region 属性定义
        #endregion

        #region 私有方法
        #endregion

        #region 公有方法
        /// <summary>
        /// 新建表格记录
        /// </summary>
        /// <returns></returns>
        public override RecData NewRecord()
        {
            ClxmRec rec = new ClxmRec();
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
            //AddNewField(1, "材料Id", "材料Id", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(2, "代码", "代码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(3, "名称", "名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(4, "规格型号", "规格型号", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(5, "单位", "单位", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(6, "数量", "数量", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(7, "定额价", "定额价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(8, "预算价", "预算价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(9, "定额合价", "定额合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(10, "预算合价", "预算合价", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(11, "计算类别", "计算类别", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(12, "材料指标分类", "材料指标分类", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(13, "单位系数", "单位系数", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewField(14, "供应方式", "供应方式", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(15, "主要材料标志", "主要材料标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(16, "材料暂估标志", "材料暂估标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(17, "不计税设备标志", "不计税设备标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(18, "单价不由明细计算标志", "单价不由明细计算标志", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);
            //AddNewField(19, "产地", "产地", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(20, "厂家", "厂家", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(21, "质量档次", "质量档次", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(22, "品种", "品种", 8, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(23, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(24, "招标编码", "招标编码", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(25, "其他信息", "其他信息", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
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
