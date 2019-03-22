using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR
{
    [AutoMapTo(typeof(QuestionLibrary))]
    public class CreateQuestionLibraryInput 
    {
        #region 表字段

        /// <summary>
        /// 标题
        /// </summary>
        [Required,MaxLength(10)]
        public string Title { get; set; }

        /// <summary>
        /// 题库分类
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(50)]
        public string Remark { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; }


        public CreateQuestionLibraryInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }


        #endregion
    }
}