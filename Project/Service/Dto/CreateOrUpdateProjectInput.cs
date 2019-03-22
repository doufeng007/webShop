using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using Abp.File;

namespace Project
{
    [AutoMap(typeof(ProjectBase))]
    public class CreateOrUpdateProjectBaseInput
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// 财评类型
        /// </summary>

        public int AppraisalTypeId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>

        public string ProjectName { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>

        public string ProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <summary>
        /// 项目评审状态
        /// </summary>
        public ProjectStatus? ProjectStatus { get; set; }
        public string SingleProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string SingleProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SendUnit { get; set; }

        public string SendUnitName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CompetentUnit { get; set; }

        public string CompetentUnit_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EntrustmentNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "报审金额输入错误。")]
        public decimal? SendTotalBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public decimal? SingleBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "建安预算输入错误。")]
        public decimal? SafaBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectAdress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectNature { get; set; }

        public string ProjectNature1 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Industry { get; set; }

        public string Industry_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContactsTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? UnitRoom { get; set; }
        public string UnitRoomName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PersonLiable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovalUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovalNum { get; set; }


        public int AuditType { get; set; }



        public bool? Is_Important { get; set; }

        public bool? IsIncludeSingle { get; set; }

        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsFollow { get; set; }


        public Guid? Area_Id { get; set; }

        public string AreaName { get; set; }
        /// <summary>
        /// 分派部门id
        /// </summary>
        public string DeparmentId { get; set; }
        /// <summary>
        /// 分派部门名称
        /// </summary>
        public string DeparmentId_Name { get; set; }

        public Guid? Gov_ProjectId { get; set; }

        public string Gov_Code { get; set; }

        public bool? HasFinancialReview { get; set; }

        #region BudgetInfo

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


        public string EvaluateMatter { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyContactTel { get; set; }
        public DateTime? EvaluateStartDate { get; set; }
        public DateTime? EvaluateEndDate { get; set; }

        public string CheckMatter { get; set; }

        public string ConsultationMatter { get; set; }


        public decimal? ContractFee { get; set; }
        public decimal? BudgetInvestmentFee { get; set; }
        public string AdjustMatter { get; set; }

        #endregion

    }

    [AutoMap(typeof(ProjectBudget))]
    public class CreateOrUpdateProJectBudgetInput
    {






    }

    [AutoMap(typeof(SingleProjectInfo))]
    public class CreateOrUpdateSingleProjectInput
    {

        public Guid? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string SingleProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string SingleProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public decimal SingleProjectbudget { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public decimal SingleProjectSafaBudget { get; set; }


        /// <summary>
        /// 预算明细
        /// </summary>
        public CreateSingleProjectFeeInput SingleFee { get; set; } = new CreateSingleProjectFeeInput();

        /// <summary>
        /// 行业
        /// </summary>
        public string Industry { get; set; }

        public string Industry_Name { get; set; }

        /// <summary>
        /// 项目性质
        /// </summary>
        public string ProjectNature { get; set; }

        /// <summary>
        /// 送审资料
        /// </summary>
        public List<CreateOrUpdateProjectFileInput> ProjectFiles { get; set; } = new List<CreateOrUpdateProjectFileInput>();

        /// <summary>
        /// 控制表
        /// </summary>
        public List<CreateOrUpdateProjectBudgetControlInput> ProjectBudgetControls { get; set; } = new List<CreateOrUpdateProjectBudgetControlInput>();



    }

    [AutoMap(typeof(ProjectBudgetControl))]
    public class CreateOrUpdateProjectBudgetControlInput
    {
        #region 表字段
        public Guid? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? Pro_Id { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovalMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SendMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ValidationMoney { get; set; }

        #endregion
    }

    [AutoMap(typeof(ProjectAuditMember))]
    public class CreateOrUpdateProjectAuditMembersInput
    {
        public Guid? Id { get; set; }

        public Guid? ProjectBaseId { get; set; }

        public long UserId { get; set; }


        public string UserName { get; set; }




        public int UserAuditRole { get; set; }

        public string UserAuditRoleName { get; set; }



        public string FlowId { get; set; }

        /// <summary>
        /// 分派工作描述
        /// </summary>
        public string WorkDes { get; set; }

        /// <summary>
        /// 分派工作时间
        /// </summary>
        public int? WorkDays { get; set; }

        public string FinishItems { get; set; }

        public string FinishItemsStr { get; set; }
        /// <summary>
        /// 允许查看的文件ids
        /// </summary>
        public string AappraisalFileTypes { get; set; }

        public Guid? GroupId { get; set; }

        public bool IsGroup { get; set; }

    }

    [AutoMap(typeof(ProjectFile))]
    public class CreateOrUpdateProjectFileInput
    {
        public Guid? Id { get; set; }

        public int AappraisalFileType { get; set; }

        public string AappraisalFileTypeName { get; set; }

        public Guid? ProjectBaseId { get; set; }


        public Guid? SingleProjectId { get; set; }


        public bool HasUpload { get; set; }

        public string FileName { get; set; }

        public bool IsPaperFile { get; set; }


        public string FilePath { get; set; }

        public bool IsMust { get; set; }

        public int? PaperFileNumber { get; set; }

        public bool? Back { get; set; }
        public List<GetAbpFilesOutput> Files { get; set; }

        
        public CreateOrUpdateProjectFileInput()
        {
            this.Files = new EditableList<GetAbpFilesOutput>();

        }


    }


    public class CreateOrUpdateProJectBudgetManagerInput : CreateWorkFlowInstance
    {
        /// <summary>
        /// 是否需要指定评审人员  默认值为true
        /// </summary>
        public bool IsNeedAppointAuditUsers { get; set; } = true;

        /// <summary>
        ///  0 正常提交 1 强行提交
        /// </summary>
        public int CertainSubmite { get; set; }

        /// <summary>
        ///  0 正常 1变更 2补充
        /// </summary>
        public int ChangeOrAddotional { get; set; }

        #region BaseInfo
        public Guid? Id { get; set; }
        /// <summary>
        /// 财评类型
        /// </summary>

        public int AppraisalTypeId { get; set; }
        /// 评审部门
        /// </summary>
        public string AuditUnit { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>


        public string ProjectName { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>

        public string ProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string SingleProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string SingleProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SendUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CompetentUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EntrustmentNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SendTotalBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SingleBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? SafaBudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectAdress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectNature { get; set; }

        public string ProjectNature1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContactsTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SendTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public int? UnitRoom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PersonLiable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovalUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovalNum { get; set; }


        public int AuditType { get; set; }

        public bool? Is_Important { get; set; }


        public bool? IsIncludeSingle { get; set; }


        public Guid? Area_Id { get; set; }

        public Guid? Gov_ProjectId { get; set; }

        public string Gov_Code { get; set; }

        #endregion


        #region BudgetInfo


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


        public string EvaluateMatter { get; set; }
        public string CompanyContact { get; set; }
        public string CompanyContactTel { get; set; }
        public DateTime? EvaluateStartDate { get; set; }
        public DateTime? EvaluateEndDate { get; set; }
        public string CheckMatter { get; set; }

        public string ConsultationMatter { get; set; }


        public decimal? ContractFee { get; set; }
        public decimal? BudgetInvestmentFee { get; set; }
        public string AdjustMatter { get; set; }

        #endregion



        public List<CreateOrUpdateSingleProjectInput> SingleProjectInfos { get; set; }

        


        


        public CreateOrUpdateProJectBudgetManagerInput()
        {
            SingleProjectInfos = new List<CreateOrUpdateSingleProjectInput>();
        }
    }



    public class ProJectBudgetUpdateChangeInput : CreateOrUpdateProJectBudgetManagerInput
    {
        public bool IsChangeSingle { get; set; }
        public int? ChangeType { get; set; }

        public Guid? SingleProjectId { get; set; }
    }


    public class GetExitProjectListInput
    {
        public string query { get; set; }


    }



}
