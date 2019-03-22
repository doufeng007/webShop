using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace Project
{
    [Serializable]
    [Table("ProjectBase")]
    [TableNameAtribute("基本信息")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectBase : FullAuditedEntity<Guid>
    {
        public ProjectBase DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as ProjectBase;
            }
        }

        #region 表字段


        /// <summary>
        /// 财评类型
        /// </summary>
        [RequiredColumnAttribute("财评类型", 0, "基本信息")]
        [DisplayName("AppraisalTypeId")]
        public int AppraisalTypeId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [RequiredColumn("项目名称", 0, "基本信息")]
        [LogColumn("项目名称", true)]
        [DisplayName("ProjectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        [RequiredColumnAttribute("项目编码", 0, "基本信息")]
        [LogColumnAttribute("项目编码", true)]
        [DisplayName("ProjectCode")]
        public string ProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SingleProjectName")]
        public string SingleProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SingleProjectCode")]
        public string SingleProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SendUnit")]
        public int SendUnit { get; set; }

        /// <summary>
        /// 主管部门编码
        /// </summary>
        [DisplayName("CompetentUnit")]
        public int? CompetentUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [RequiredColumn("委托文号", 0, "基本信息")]
        [DisplayName("EntrustmentNumber")]
        public string EntrustmentNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SendTotalBudget")]
        public decimal? SendTotalBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[RequiredColumnAttribute("单项预算")]
        //[LogColumnAttribute("单项预算", true)]
        [DisplayName("SingleBudget")]
        public decimal? SingleBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SafaBudget")]
        public decimal? SafaBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectAdress")]
        public string ProjectAdress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectNature")]
        public string ProjectNature { get; set; }



        [DisplayName("ProjectNature1")]
        public string ProjectNature1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Industry")]
        public string Industry { get; set; }


        public string Industry_Name { get; set; }

        /// <summary>
        /// 评审组
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Contacts")]
        public string Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Contacts_Tel")]
        public string ContactsTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SendTime")]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RequiredColumn("业务股室", 0, "基本信息")]
        [DisplayName("UnitRoom")]
        public string UnitRoom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PersonLiable")]
        public string PersonLiable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Days")]
        public int Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>

        //[RequiredColumnAttribute("批复单位", 8, "控制表")]
        [DisplayName("ApprovalUnit")]
        public string ApprovalUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>

        [DisplayName("ApprovalNum")]
        //[RequiredColumnAttribute("批准文号", 8, "控制表")]
        public string ApprovalNum { get; set; }

        public int AuditType { get; set; }


        public bool? Is_Important { get; set; }


        public bool? IsReturnAudit { get; set; }


        public string ReturnAuditSmmary { get; set; }


        /// <summary>
        /// 0 未停滞 1 提交停滞申请 2 审核通过 3 审核驳回 4 提交解除停滞申请  5 解除通过 6 解除驳回
        /// </summary>
        public int IsStop { get; set; }


        public string StopSmmary { get; set; }


        public int Status { get; set; }


        /// <summary>
        /// 审定金额
        /// </summary>
        public decimal? AuditAmount { get; set; }


        /// <summary>
        /// 审定时间
        /// </summary>
        public DateTime? AuditTime { get; set; }

        public bool? IsIncludeSingle { get; set; }


        public string AuditUnit { get; set; }


        public bool? HasFinancialReview { get; set; }

        #endregion
        /// <summary>
        /// 项目评审状态
        /// </summary>
        public ProjectStatus? ProjectStatus { get; set; }

        /// <summary>
        /// 分派的部门ID（l_orgid）
        /// </summary>
        public string DeparmentId { get; set; }

        public Guid? Area_Id { get; set; }


        public Guid? Gov_ProjectId { get; set; }

        public string Gov_Code { get; set; }


        #region BudgetInfo  预算 概算

        /// </summary>
        public decimal? FeasibilityStudyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SurveyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? DesignFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ResearchTestFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? EnvironmentalImpactFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? OtherFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? LandAcquisitionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? LandReclamationFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ConstructionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? InstallFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? DeviceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SupervisorFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? LandUseTax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? FarmlandOccupationTax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? vehicleAndVesselTax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? StampDutyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? TemporaryFacilitiesFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? CulturaRrelicsProtectionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ForestRrestorationFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SafetyProductionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SafetyAssessmentFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? NetworkRentalFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SystemoperationFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? BuldManagerFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ACMF { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? EngineeringInsuranceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? BiddingFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ContractNotarialFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? AuditFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? EngineeringInspectionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? EquipmentInspectionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? CombinedTestFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? AttorneyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ChannelMaintenanceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? NavigationAidsFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? AerialSurveyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? OtherFee2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? UnforeseenFee { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal? PreliminaryFee { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal? ExpropriationFee { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal? ProjectDeviceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? OtherTotalFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? TotalFee { get; set; }

        #endregion

        #region
        public int? PurchaseGovId { get; set; }
        #endregion


        #region  绩效
        public string EvaluateMatter { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyContactTel { get; set; }
        public DateTime? EvaluateStartDate { get; set; }
        public DateTime? EvaluateEndDate { get; set; }
        #endregion

        #region 专项核查
        public string CheckMatter { get; set; }
        #endregion


        #region  日常要求
        public string ConsultationMatter { get; set; }

        #endregion


        #region 项目调整预算

        public decimal? ContractFee { get; set; }
        public decimal? BudgetInvestmentFee { get; set; }
        public string AdjustMatter { get; set; }

        #endregion  

        /// <summary>
        /// 项目准备开始阶段
        /// </summary>
        public DateTime? ReadyStartTime { get; set; }
        /// <summary>
        /// 项目准备结束时间
        /// </summary>
        public DateTime? ReadyEndTime { get; set; }
    }
    [Flags]
    public enum ProjectStatus
    {
        待审 = 0,
        在审 = 1,
        初审 = 2,//熟悉图纸
        计量 = 4,//建模及算量
        记价 = 8,//清单及算价
        内核 = 16,//汇总复核
        复核 = 32,
        总核 = 64
    }


    public class ProjectListDto : Entity<Guid>
    {

        public string ProjectName { get; set; }

        public string ProjectCode { get; set; }

        public int AppraisalTypeId { get; set; }

        public string AppraisalTypeName { get; set; }


        public decimal? SendTotalBudget { get; set; }


        public string GroupName { get; set; }

        public string SendUnitName { get; set; }

        public int Status { get; set; }

        public Guid? StepId { get; set; }


        public string StepName { get; set; }

        public DateTime? SendTime { get; set; }


        public Guid? FlowID { get; set; }

        public string EntrustmentNumber { get; set; }
        public Guid? GroupID { get; set; }

        public bool? IsImportant { get; set; }



        //public int TotalCount { get; set; }
        /// <summary>
        /// 是否关注当前项目
        /// </summary>
        public bool IsFollow { get; set; }

        public bool IsReturnAudit { get; set; }

        /// <summary>
        /// 0 未停滞 1 提交停滞申请 2 审核通过 3 审核驳回 4 提交解除停滞申请  5 解除通过 6 解除驳回
        /// </summary>
        public int IsStop { get; set; }

        public DateTime CreationTime { get; set; }

        public long CreatorUserId { get; set; }





        public ProjectListDto()
        {

        }

    }






    [AutoMapFrom(typeof(ProjectListDto))]
    public class ProjectListWithStatusSummaryDto : ProjectListDto
    {
        public string StatusSummary
        {
            get; set;
        }

        public bool IsCanAdditionalProject { get; set; } = false;

    }


    public class ProjectListGroupWithCodeDto : Entity<Guid>
    {

        public string ProjectName { get; set; }

        public string ProjectCode { get; set; }

        public int AppraisalTypeId { get; set; }

        public string AppraisalTypeName { get; set; }


        public decimal? SendTotalBudget { get; set; }


        public string SendUnitName { get; set; }

        public DateTime? SendTime { get; set; }



        public string EntrustmentNumber { get; set; }

        public bool IsImportant { get; set; }



        //public int TotalCount { get; set; }
        /// <summary>
        /// 是否关注当前项目
        /// </summary>
        public bool IsFollow { get; set; }



        public DateTime CreationTime { get; set; }

        public long CreatorUserId { get; set; }


        public bool HasComplete { get; set; }


    }


    public class ProjectSingleListDto : Entity<Guid>
    {
        public string ProjectSingleName { get; set; }

        public string ProjectSingleCode { get; set; }


        public int Status { get; set; }


        public string StatusSummary { get; set; }

        public Guid? StepId { get; set; }


        public string StepName { get; set; }


        public Guid? FlowID { get; set; }

        public Guid? GroupID { get; set; }

        public string GroupName { get; set; }


        ////public int TotalCount { get; set; }
        ///// <summary>
        ///// 是否关注当前项目
        ///// </summary>
        //public bool IsFollow { get; set; }

        public bool IsReturnAudit { get; set; }

        /// <summary>
        /// 0 未停滞 1 提交停滞申请 2 审核通过 3 审核驳回 4 提交解除停滞申请  5 解除通过 6 解除驳回
        /// </summary>
        public ProjectStopTypeEnum IsStop { get; set; }

        public DateTime CreationTime { get; set; }

        public long CreatorUserId { get; set; }

        public bool IsCanAdditionalProject { get; set; }
    }














}