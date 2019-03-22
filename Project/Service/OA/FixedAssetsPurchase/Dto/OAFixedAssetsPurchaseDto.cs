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
    [AutoMapFrom(typeof(OAFixedAssetsPurchase))]
    public class OAFixedAssetsPurchaseDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string ApplyUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplyTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeeSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeeSourceCode { get; set; }


        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        public string Reason { get; set; }



        public List<GetAbpFilesOutput> FileList { get; set; }


        public List<OAFixedAssetsDto> FixedAssetss { get; set; }

        public string ApplyUserId_Name { get; set; }

        public OAFixedAssetsPurchaseDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
            this.FixedAssetss = new List<OAFixedAssetsDto>();
        }

    }


}
