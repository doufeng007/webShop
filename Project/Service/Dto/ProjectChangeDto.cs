using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Project
{
    /// <summary>
    /// 项目变更对比Dto
    /// </summary>
    public class ProjectChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// 财评类型
        /// </summary>

        //public string AppraisalTypeId_Name { get; set; }


        ///// 评审部门
        ///// </summary>
        //public string AuditUnit { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>

        [LogColumn("项目名称", IsLog = true)]
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        [LogColumn("项目编码", IsLog = true)]
        public string ProjectCode { get; set; }



        /// <summary>
        /// ConstructionOrganizations
        /// </summary>
        [LogColumn("送审单位", IsLog = true)]
        public string SendUnit_Name { get; set; }

        /// <summary>
        /// ChargeOrganizations
        /// </summary>
        [LogColumn("主管部门", IsLog = true)]
        public string CompetentUnit_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("委托文号", IsLog = true)]
        public string EntrustmentNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("报审金额", IsLog = true)]
        public decimal? SendTotalBudget { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [LogColumn("建安预算", IsLog = true)]
        public decimal? SafaBudget { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public string ProjectAdress { get; set; }


        /// <summary>
        ///  1 2 3 
        /// </summary>
        [LogColumn("项目性质", IsLog = true)]
        public string ProjectNature1_Name { get; set; }

        /// <summary>
        /// 数据字典
        /// </summary>
        [LogColumn("行业", IsLog = true)]
        public string Industry_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("联系人", IsLog = true)]
        public string Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("联系电话", IsLog = true)]
        public string ContactsTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("送审时间", IsLog = true)]
        public DateTime SendTime { get; set; } = DateTime.Now;

        /// <summary>
        /// BusinessDepartment
        /// </summary>
        [LogColumn("业务股室", IsLog = true)]
        public string UnitRoom_Name { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [LogColumn("工日", IsLog = true)]
        public int Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("其它信息", IsLog = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("项目批准单位", IsLog = true)]
        public string ApprovalUnit_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("批准文号", IsLog = true)]
        public string ApprovalNum { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [LogColumn("重点项目", IsLog = true)]
        public bool? Is_Important { get; set; }

        /// <summary>
        /// 控制表
        /// </summary>
        [LogColumn("单项信息")]
        public List<SingleProjectInfoChangeDto> SingleProject { get; set; } = new List<SingleProjectInfoChangeDto>();

    }


    /// <summary>
    ///项目-控制表变更对比Dto
    /// </summary>
    public class ProjectControlChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [LogColumn("控制类型")]
        public string CodeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("名称", IsNameField = true)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("批准")]
        public string ApprovalMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("送审")]
        public string SendMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("审定")]
        public string ValidationMoney { get; set; }



    }

    /// <summary>
    /// 项目-资料清单变更对比Dto
    /// </summary>
    public class ProjcetFileChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("资料名称", IsNameField = true)]
        public string AappraisalFileType_Name { get; set; }



        [LogColumn("是否退还")]
        public bool? Back { get; set; }

        [LogColumn("纸质资料")]
        public int? PaperFileNumber { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        [LogColumn("电子资料")]
        public List<AbpFileChangeDto> Files { get; set; }

    }
}
