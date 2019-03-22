using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Project
{

    [AutoMapFrom(typeof(SingleProjectInfo))]
    public class SingleProjectInfoOutput
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ProjectId { get; set; }

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
        public SingleProjectFeeOutput SingleFee { get; set; } = new SingleProjectFeeOutput();

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


        public List<ProjectBudgetControlOutput> ProjectBudgetControls { get; set; } = new List<ProjectBudgetControlOutput>();



        /// <summary>
        /// 分派的部门ID（l_orgid）
        /// </summary>
        public string DeparmentId { get; set; }


        public string DeparmentId_Name { get; set; }


        public Guid? GroupId { get; set; }

        public string GroupName { get; set; }


        public string ProjectCode { get; set; }

        public bool? IsReturnAudit { get; set; }

        public int IsStop { get; set; }


        public int Status { get; set; }


        public string ReturnAuditSmmary { get; set; }


        public ProjectStatus? ProjectStatus { get; set; }



        public DateTime? ReadyEndTime { get; set; }


        public DateTime? ReadyStartTime { get; set; }

        public decimal? AuditAmount { get; set; }

    }



    public class SingleProjectInfoChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }




        /// <summary>
        /// 
        /// </summary>
        [LogColumn("单项名称")]
        public string SingleProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("单项编码")]
        public string SingleProjectCode { get; set; }

        [LogColumn("单项预算")]
        public decimal SingleProjectbudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("建安预算")]
        public decimal SingleProjectSafaBudget { get; set; }

        ///// <summary>
        ///// 预算明细
        ///// </summary>
        //[LogColumn("预算明细")]
        //public SingleProjectFeeChangeDto SingleFee { get; set; } = new SingleProjectFeeChangeDto();

        #region BudgetInfo


        /// <summary>
        /// 可行性研究费
        /// </summary>
        [LogColumn(@"预算明细-可行性研究费", IsLog = true)]
        public decimal? FeasibilityStudyFee { get; set; }

        /// <summary>
        /// 勘察费
        /// </summary>
        [LogColumn(@"预算明细-勘察费", IsLog = true)]
        public decimal? SurveyFee { get; set; }

        /// <summary>
        /// 设计费
        /// </summary>
        [LogColumn(@"预算明细-设计费", IsLog = true)]
        public decimal? DesignFee { get; set; }

        /// <summary>
        /// 研究试验费
        /// </summary>
        [LogColumn(@"预算明细-研究试验费", IsLog = true)]
        public decimal? ResearchTestFee { get; set; }

        /// <summary>
        /// 环境影响评价费
        /// </summary>
        [LogColumn(@"预算明细-环境影响评价费", IsLog = true)]
        public decimal? EnvironmentalImpactFee { get; set; }

        /// <summary>
        /// 其它费用
        /// </summary>
        [LogColumn(@"预算明细-其它费用", IsLog = true)]
        public decimal? OtherFee { get; set; }

        /// <summary>
        /// 土地征用及迁移补偿费
        /// </summary>
        [LogColumn(@"预算明细-土地征用及迁移补偿费", IsLog = true)]
        public decimal? LandAcquisitionFee { get; set; }

        /// <summary>
        /// 土地复垦及补偿费
        /// </summary>
        [LogColumn(@"预算明细-土地复垦及补偿费", IsLog = true)]
        public decimal? LandReclamationFee { get; set; }

        /// <summary>
        /// 建筑工程费
        /// </summary>
        [LogColumn(@"预算明细-建筑工程费", IsLog = true)]
        public decimal? ConstructionFee { get; set; }

        /// <summary>
        /// 安装工程费
        /// </summary>
        [LogColumn(@"预算明细-安装工程费", IsLog = true)]
        public decimal? InstallFee { get; set; }

        /// <summary>
        /// 设备等购置费
        /// </summary>
        [LogColumn(@"预算明细-设备等购置费", IsLog = true)]
        public decimal? DeviceFee { get; set; }

        /// <summary>
        /// 监理费
        /// </summary>
        [LogColumn(@"预算明细-监理费", IsLog = true)]
        public decimal? SupervisorFee { get; set; }

        /// <summary>
        /// 土地使用税
        /// </summary>
        [LogColumn(@"预算明细-土地使用税", IsLog = true)]
        public decimal? LandUseTax { get; set; }

        /// <summary>
        /// 耕地占用税
        /// </summary>
        [LogColumn(@"预算明细-耕地占用税", IsLog = true)]
        public decimal? FarmlandOccupationTax { get; set; }

        /// <summary>
        /// 车船税
        /// </summary>
        [LogColumn(@"预算明细-车船税", IsLog = true)]
        public decimal? vehicleAndVesselTax { get; set; }

        /// <summary>
        /// 印花税
        /// </summary>
        [LogColumn(@"预算明细-印花税", IsLog = true)]
        public decimal? StampDutyFee { get; set; }

        /// <summary>
        /// 临时设施费
        /// </summary>
        [LogColumn(@"预算明细-临时设施费", IsLog = true)]
        public decimal? TemporaryFacilitiesFee { get; set; }

        /// <summary>
        /// 文物保护费
        /// </summary>
        [LogColumn(@"预算明细-文物保护费", IsLog = true)]
        public decimal? CulturaRrelicsProtectionFee { get; set; }

        /// <summary>
        /// 森林植被恢复费
        /// </summary>
        [LogColumn(@"预算明细-森林植被恢复费", IsLog = true)]
        public decimal? ForestRrestorationFee { get; set; }

        /// <summary>
        /// 安全生产费
        /// </summary>
        [LogColumn(@"预算明细-安全生产费", IsLog = true)]
        public decimal? SafetyProductionFee { get; set; }

        /// <summary>
        /// 安全鉴定费
        /// </summary>
        [LogColumn(@"预算明细-安全鉴定费", IsLog = true)]
        public decimal? SafetyAssessmentFee { get; set; }

        /// <summary>
        /// 网络租赁费
        /// </summary>
        [LogColumn(@"预算明细-网络租赁费", IsLog = true)]
        public decimal? NetworkRentalFee { get; set; }

        /// <summary>
        /// 系统运行维护监理费
        /// </summary>
        [LogColumn(@"预算明细-系统运行维护监理费", IsLog = true)]
        public decimal? SystemoperationFee { get; set; }

        /// <summary>
        /// 项目建设管理费
        /// </summary>
        [LogColumn(@"预算明细-项目建设管理费", IsLog = true)]
        public decimal? BuldManagerFee { get; set; }

        /// <summary>
        /// 代建管理费
        /// </summary>
        [LogColumn(@"预算明细-代建管理费", IsLog = true)]
        public decimal? ACMF { get; set; }

        /// <summary>
        /// 工程保险费
        /// </summary>
        [LogColumn(@"预算明细-工程保险费", IsLog = true)]
        public decimal? EngineeringInsuranceFee { get; set; }

        /// <summary>
        /// 招投标费
        /// </summary>
        [LogColumn(@"预算明细-招投标费", IsLog = true)]
        public decimal? BiddingFee { get; set; }

        /// <summary>
        /// 合同公证费
        /// </summary>
        [LogColumn(@"预算明细-合同公证费", IsLog = true)]
        public decimal? ContractNotarialFee { get; set; }

        /// <summary>
        /// 社会中介机构审计(查)费
        /// </summary>
        [LogColumn(@"预算明细-社会中介机构审计(查)费", IsLog = true)]
        public decimal? AuditFee { get; set; }

        /// <summary>
        /// 工程检测费
        /// </summary>
        [LogColumn(@"预算明细-工程检测费", IsLog = true)]
        public decimal? EngineeringInspectionFee { get; set; }

        /// <summary>
        /// 设备检验费
        /// </summary>
        [LogColumn(@"预算明细-设备检验费", IsLog = true)]
        public decimal? EquipmentInspectionFee { get; set; }

        /// <summary>
        /// 负荷联合试车费
        /// </summary>
        [LogColumn(@"预算明细-负荷联合试车费", IsLog = true)]
        public decimal? CombinedTestFee { get; set; }

        /// <summary>
        /// 律师代理费
        /// </summary>
        [LogColumn(@"预算明细-律师代理费", IsLog = true)]
        public decimal? AttorneyFee { get; set; }

        /// <summary>
        /// 航道维护费
        /// </summary>
        [LogColumn(@"预算明细-航道维护费", IsLog = true)]
        public decimal? ChannelMaintenanceFee { get; set; }

        /// <summary>
        /// 航标设施费
        /// </summary>
        [LogColumn(@"预算明细-航标设施费", IsLog = true)]
        public decimal? NavigationAidsFee { get; set; }

        /// <summary>
        /// 航测费
        /// </summary>
        [LogColumn(@"预算明细-航测费", IsLog = true)]
        public decimal? AerialSurveyFee { get; set; }

        /// <summary>
        /// 其他待摊投资性质支出
        /// </summary>
        [LogColumn(@"预算明细-其他待摊投资性质支出", IsLog = true)]
        public decimal? OtherFee2 { get; set; }

        /// <summary>
        /// 不可预见费
        /// </summary>
        [LogColumn(@"预算明细-不可预见费", IsLog = true)]
        public decimal? UnforeseenFee { get; set; }

        /// <summary>
        /// 项目前期总费用
        /// </summary>
        [LogColumn(@"预算明细-项目前期总费用", IsLog = true)]
        public decimal? PreliminaryFee { get; set; }

        /// <summary>
        /// 征地总费用
        /// </summary>
        [LogColumn(@"预算明细-征地总费用", IsLog = true)]
        public decimal? ExpropriationFee { get; set; }

        /// <summary>
        /// 工程设备总费用
        /// </summary>
        [LogColumn(@"工程设备总费用", IsLog = true)]
        public decimal? ProjectDeviceFee { get; set; }

        /// <summary>
        /// 其它总费用
        /// </summary>
        [LogColumn(@"预算明细-其它总费用", IsLog = true)]
        public decimal? OtherTotalFee { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [LogColumn(@"预算明细-总费用", IsLog = true)]
        public decimal? TotalFee { get; set; }

        #endregion
        /// <summary>
        /// 行业
        /// </summary>
        [LogColumn("行业")]
        public string Industry { get; set; }

        /// <summary>
        /// 项目性质
        /// </summary>
        [LogColumn("项目性质")]
        public string ProjectNature { get; set; }

        #region  控制表

        /// <summary>
        /// 控制表
        /// </summary>
        [LogColumn("控制表")]
        public List<ProjectControlChangeDto> Controls { get; set; } = new List<ProjectControlChangeDto>();

        #endregion



        /// <summary>
        /// 送审资料
        /// </summary>
        [LogColumn("送审资料")]
        public List<ProjcetFileChangeDto> ProjectFiles { get; set; } = new List<ProjcetFileChangeDto>();




    }
}
