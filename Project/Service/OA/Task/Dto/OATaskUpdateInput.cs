using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Abp.File;

namespace Project
{
    [AutoMapTo(typeof(OATask))]
    public class OATaskUpdateInput
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



        public OATaskUpdateInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }




}
