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
    public class DjjsbRec : RecData
    {
        #region 私有字段
        #endregion

        #region 构造函数
        public DjjsbRec()
        { 
            childList.ItemType = typeof(DjjsMxRec);
        }

        #endregion

        #region 属性定义
        #endregion

        #region 私有方法
        #endregion

        #region 公有方法
        #endregion
    }
}
