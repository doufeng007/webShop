using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using CjzDataBase;

namespace CjzFormat
{
    /// <summary>
    /// 材料记录
    /// </summary>
    public class ClxmRec : RecData
    {
        #region 私有字段
        #endregion

        #region 构造函数
        public ClxmRec()
        { 
            childList.ItemType = typeof(MxclRec);
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
