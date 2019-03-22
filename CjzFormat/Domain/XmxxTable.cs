using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CjzDataBase;

namespace CjzFormat
{
    public class XmxxTable: TableData
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
            XmxxRec rec = new XmxxRec();
            FormRec(rec);
            return rec;
        }
        /// <summary>
        /// 初始化列
        /// </summary>
        public override void InitialCols()
        {
            base.InitialCols();
            //AddNewField(1, "编码", "编码", 20, (int)_alignType.atLeft, (int)_dataType.dtString, 0, true);
            //AddNewField(2, "名称", "名称", 40, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(3, "内容", "内容", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0);
            //AddNewField(4, "说明", "说明", 30, (int)_alignType.atLeft, (int)_dataType.dtString, 0, true);
        }

        #endregion
    }
}
