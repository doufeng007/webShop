using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Abp.File;
using Abp.WorkFlow.Service.Dto;

namespace Project
{
    [AutoMapTo(typeof(OATask))]
    public class OATaskCreateInput : CreateWorkFlowInstance
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 优先级 1 搞 2中 3低
        /// </summary>
        public string Priority { get; set; }


        /// <summary>
        /// 开始日期
        /// </summary>

        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结算日期
        /// </summary>

        public DateTime EndDate { get; set; }



        /// <summary>
        /// 任务验收人  只能选择一个人
        /// </summary>
        public long ValUser { get; set; }

        /// <summary>
        /// 任务执行人  选择多个
        /// </summary>
        public string ExecutorUser { get; set; }


        public int CreateByBusinessRole { get; set; } = 0;



        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }



        public OATaskCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }

    [AutoMapTo(typeof(OATaskUser))]
    public class CreateOrUpdateOATaskUserInput
    {
        public Guid? OATaskId { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

    }



}
