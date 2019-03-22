using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.WorkFlow
{
    [Table("AppLibrary")]
    public class AppLibrary : IEntity<Guid>
    {
        public bool IsTransient()
        {
            return false;
        }

        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DisplayName("Address")]
        public string Address { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [DisplayName("Type")]
        public Guid Type { get; set; }

        /// <summary>
        /// 打开方式{0-默认,1-弹出模态窗口,2-弹出窗口,3-新窗口}
        /// </summary>
        [DisplayName("OpenMode")]
        public int OpenMode { get; set; }

        /// <summary>
        /// 弹出窗口宽度
        /// </summary>
        [DisplayName("Width")]
        public int Width { get; set; }

        /// <summary>
        /// 弹出窗口高度
        /// </summary>
        [DisplayName("Height")]
        public int Height { get; set; }

        /// <summary>
        /// 其它参数
        /// </summary>
        [DisplayName("Params")]
        public string Params { get; set; }

        /// <summary>
        /// 管理人员
        /// </summary>
        [DisplayName("Manager")]
        public string Manager { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("Note")]
        public string Note { get; set; }

        /// <summary>
        /// 唯一标识符，流程应用时为流程ID，表单应用时对应表单ID
        /// </summary>
        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [DisplayName("Ico")]
        public string Ico { get; set; }

        #endregion




    }
}