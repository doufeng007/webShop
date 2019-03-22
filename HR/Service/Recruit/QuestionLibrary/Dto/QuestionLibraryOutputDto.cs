using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using Castle.Components.DictionaryAdapter;

namespace HR
{
    [AutoMapFrom(typeof(QuestionLibrary))]
    public class QuestionLibraryOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 题库分类
        /// </summary>
        public Guid TypeId { get; set; }
        public string TypeId_Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }


        public QuestionLibraryOutputDto()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }

    }
}
