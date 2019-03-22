using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public class ProjectTodoStaticDto
    {
        public int TodoCount { get; set; }
        /// <summary>
        /// 在办项目数量
        /// </summary>
        public int DoingCount { get; set; }
        /// <summary>
        /// 已办项目数量
        /// </summary>
        public int DoneCount { get; set; }
        /// <summary>
        /// 在办项目金额
        /// </summary>
        public decimal DoingSum { get; set; }
        /// <summary>
        /// 已办项目金额
        /// </summary>
        public decimal DoneSum { get; set; }

    }
}
