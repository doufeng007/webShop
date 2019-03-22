using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.File;

namespace IMLib
{
    [AutoMapFrom(typeof(ImMessage))]
    public class ImMessageListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 组编号
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public Guid? FIleId { get; set; }

        /// <summary>
        /// 是否聊天室
        /// </summary>
        public bool? RoomType { get; set; }

        /// <summary>
        /// 
        /// 聊天室类型
        /// </summary>
        public string ChatType { get; set; }


    }
}
