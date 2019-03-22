using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace Train
{
	[AutoMapFrom(typeof(UserCourseComment))]
    public class UserCourseCommentLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [LogColumn(@"用户编号", IsLog = true)]
        public int UserId { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        [LogColumn(@"课程编号", IsLog = true)]
        public Guid CourseId { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [LogColumn(@"评价内容", IsLog = true)]
        public string Comment { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        #endregion
    }
}