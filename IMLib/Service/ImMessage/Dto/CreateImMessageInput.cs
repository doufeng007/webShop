using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace IMLib
{
    [AutoMapTo(typeof(ImMessage))]
    public class CreateImMessageInput 
    {
        #region 表字段

        /// <summary>
        /// 组编号
        /// </summary>
        [MaxLength(50,ErrorMessage = "组编号长度必须小于50")]
        [Required(ErrorMessage = "必须填写组编号")]
        public string To { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [MaxLength(20,ErrorMessage = "类型长度必须小于20")]
        [Required(ErrorMessage = "必须填写类型")]
        public string Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Msg { get; set; }


        /// <summary>
        /// 是否聊天室
        /// </summary>
        public bool RoomType { get; set; }

        /// <summary>
        /// 聊天室类型
        /// </summary>
        [MaxLength(20,ErrorMessage = "聊天室类型长度必须小于20")]
        public string ChatType { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }

        #endregion
    }
}