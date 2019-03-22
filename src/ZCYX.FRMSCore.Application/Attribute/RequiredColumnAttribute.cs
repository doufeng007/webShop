using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class RequiredColumnAttribute : Attribute
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目类型（哪个类型需要检查的字段）
        /// </summary>
        public int ProjectTypeId { get; set; }

        /// <summary>
        /// 字段类别名称
        /// </summary>
        public string CategoryName { get; set; }

        public RequiredColumnAttribute(string name, int projectTypeId, string categoryName)
        {
            Name = name;
            ProjectTypeId = projectTypeId;
            CategoryName = categoryName;
        }
    }
}
