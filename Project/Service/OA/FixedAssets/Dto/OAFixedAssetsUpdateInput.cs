using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAFixedAssets))]
    public class OAFixedAssetsUpdateInput
    {
        public Guid? Id { get; set; }

        public Guid? PurchaseId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateOfManufacture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BuyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PostingDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BuyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BuyTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeeSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeeSourceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServiceLife { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal OriginalValue { get; set; }


        public decimal ReferPrice { get; set; }

        public decimal RealPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }



        public List<GetAbpFilesOutput> FileList { get; set; }


        public OAFixedAssetsUpdateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }
}
