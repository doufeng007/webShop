using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [Table("OABidProject")]
    public class OABidProject : FullAuditedEntity<Guid>
    {
        #region 表字段


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectCode")]
        public string ProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectSummary")]
        public string ProjectSummary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectAddress")]
        public string ProjectAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("WriteDate")]
        public DateTime WriteDate { get; set; }

        public string WriteUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectNature")]
        public string ProjectNature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectNatureCode")]
        public string ProjectNatureCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectType")]
        public string ProjectType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectTypeCode")]
        public string ProjectTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Bidder")]
        public string Bidder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BidPreDate")]
        public DateTime BidPreDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BidPreFee")]
        public decimal BidPreFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BidPreContractAmount")]
        public decimal BidPreContractAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BuildUnit")]
        public string BuildUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ContractAmount")]
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Contacts")]
        public string Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ContactsTel")]
        public string ContactsTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SignUnit")]
        public string SignUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SupUnit")]
        public string SupUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("File")]
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }

        #endregion





    }

    public enum OABidProjectStatus
    {
        [Description("新增")]
        新增 = 0,
        [Description("完成")]
        完成 = 1,
        [Description("正在购买招标文件")]
        正在购买招标文件 = 2,
        [Description("购买招标文件完成")]
        购买招标文件完成 = 3,
        [Description("正在资格自审")]
        正在资格自审 = 4,
        [Description("资格自审完成")]
        资格自审完成 = 5,
        [Description("正在项目勘察")]
        正在项目勘察 = 6,
        [Description("项目勘察完成")]
        项目勘察完成 = 7,
        [Description("投标文件审查")]
        投标文件审查 = 8,
        [Description("投标文件审查完成")]
        投标文件审查完成 = 9,
        [Description("投标保证金申请")]
        投标保证金申请 = 10,
        [Description("投标保证金申请完成")]
        投标保证金申请完成 = 11,
        [Description("分析竞争对手")]
        分析竞争对手 = 12,
        [Description("分析竞争对手完成")]
        分析竞争对手完成 = 13,
        [Description("申请项目业务费用")]
        申请项目业务费用 = 14,
        [Description("项目业务费用")]
        项目业务费用 = 15

    }
}
