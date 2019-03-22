using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project

{
    [AutoMapFrom(typeof(OATask))]
    public class OATaskDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Priority { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string PriorityCode { get; set; }



        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public long ValUser { get; set; }


        public string ValUser1 { get; set; }




        public string ExecutorUser { get; set; }


        public int CreateByBusinessRole { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }




        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        public string NotifyUsers { get; set; }


        public List<CreateOrUpdateOATaskUserInput> Users { get; set; }



        public List<GetAbpFilesOutput> FileList { get; set; }

        public string ExecutorUser_Name { get; set; }

        public string ValUser_Name { get; set; }


        public OATaskDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
            this.Users = new List<CreateOrUpdateOATaskUserInput>();
        }

    }


}
