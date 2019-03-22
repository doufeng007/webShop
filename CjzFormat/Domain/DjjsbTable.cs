using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CjzDataBase;

namespace CjzFormat
{
    public class DjjsbTable : MDTableData
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
            DjjsbRec rec = new DjjsbRec();
            FormRec(rec);
            return rec;
        }

        public override RecData NewDRecord()
        {
            DjjsMxRec drec = new DjjsMxRec();
            FormDRec(drec);
            return drec;
        }
        /// <summary>
        /// 初始化列
        /// </summary>
        public override void InitialCols()
        {
            base.InitialCols();
            //AddNewField(1, "Id", "Id", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(2, "名称", "名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(3, "备注", "备注", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
        }

        /// <summary>
        /// 初始化子表列
        /// </summary>
        public override void InitialDCols()
        {
            base.InitialDCols();
            //AddNewDField(1, "行变量", "行变量", 15, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(2, "项目名称", "项目名称", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(3, "计算公式", "计算公式", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(4, "计算公式说明", "计算公式说明", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(5, "费用类别", "费用类别", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewDField(6, "费率", "费率", 15, (int)_alignType.atRight, (int)_dataType.dtFloat, 2);
            //AddNewDField(7, "备注", "备注", 8, (int)_alignType.atMiddle, (int)_dataType.dtBoolean, 0);

        }
        #endregion
    }
}
