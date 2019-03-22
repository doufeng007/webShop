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
    [AutoMapTo(typeof(OAFixedAssetsReturn))]
    public class OAFixedAssetsReturnCreateInput: CreateWorkFlowInstance
    {

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid UseApplyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid FAId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FAName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }



        public List<GetAbpFilesOutput> FileList { get; set; }



        public OAFixedAssetsReturnCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }



    
  
}
