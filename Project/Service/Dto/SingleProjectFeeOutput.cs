using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Project
{
    [AutoMap(typeof(SingleProjectFee))]
    public class SingleProjectFeeOutput
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid SingleProjectId { get; set; }

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


        public decimal? ContractFee { get; set; }


        public decimal? BudgetInvestmentFee { get; set; }


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
    }

    [AutoMapFrom(typeof(SingleProjectFee))]
    public class SingleProjectFeeChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }

        #region BudgetInfo


        /// <summary>
        /// 可行性研究费
        /// </summary>
        [LogColumn(@"可行性研究费", IsLog = true)]
        public decimal? FeasibilityStudyFee { get; set; }

        /// <summary>
        /// 勘察费
        /// </summary>
        [LogColumn(@"勘察费", IsLog = true)]
        public decimal? SurveyFee { get; set; }

        /// <summary>
        /// 设计费
        /// </summary>
        [LogColumn(@"设计费", IsLog = true)]
        public decimal? DesignFee { get; set; }

        /// <summary>
        /// 研究试验费
        /// </summary>
        [LogColumn(@"研究试验费", IsLog = true)]
        public decimal? ResearchTestFee { get; set; }

        /// <summary>
        /// 环境影响评价费
        /// </summary>
        [LogColumn(@"环境影响评价费", IsLog = true)]
        public decimal? EnvironmentalImpactFee { get; set; }

        /// <summary>
        /// 其它费用
        /// </summary>
        [LogColumn(@"其它费用", IsLog = true)]
        public decimal? OtherFee { get; set; }

        /// <summary>
        /// 土地征用及迁移补偿费
        /// </summary>
        [LogColumn(@"土地征用及迁移补偿费", IsLog = true)]
        public decimal? LandAcquisitionFee { get; set; }

        /// <summary>
        /// 土地复垦及补偿费
        /// </summary>
        [LogColumn(@"土地复垦及补偿费", IsLog = true)]
        public decimal? LandReclamationFee { get; set; }

        /// <summary>
        /// 建筑工程费
        /// </summary>
        [LogColumn(@"建筑工程费", IsLog = true)]
        public decimal? ConstructionFee { get; set; }

        /// <summary>
        /// 安装工程费
        /// </summary>
        [LogColumn(@"安装工程费", IsLog = true)]
        public decimal? InstallFee { get; set; }

        /// <summary>
        /// 设备等购置费
        /// </summary>
        [LogColumn(@"设备等购置费", IsLog = true)]
        public decimal? DeviceFee { get; set; }

        /// <summary>
        /// 监理费
        /// </summary>
        [LogColumn(@"监理费", IsLog = true)]
        public decimal? SupervisorFee { get; set; }

        /// <summary>
        /// 土地使用税
        /// </summary>
        [LogColumn(@"土地使用税", IsLog = true)]
        public decimal? LandUseTax { get; set; }

        /// <summary>
        /// 耕地占用税
        /// </summary>
        [LogColumn(@"耕地占用税", IsLog = true)]
        public decimal? FarmlandOccupationTax { get; set; }

        /// <summary>
        /// 车船税
        /// </summary>
        [LogColumn(@"车船税", IsLog = true)]
        public decimal? vehicleAndVesselTax { get; set; }

        /// <summary>
        /// 印花税
        /// </summary>
        [LogColumn(@"印花税", IsLog = true)]
        public decimal? StampDutyFee { get; set; }

        /// <summary>
        /// 临时设施费
        /// </summary>
        [LogColumn(@"临时设施费", IsLog = true)]
        public decimal? TemporaryFacilitiesFee { get; set; }

        /// <summary>
        /// 文物保护费
        /// </summary>
        [LogColumn(@"文物保护费", IsLog = true)]
        public decimal? CulturaRrelicsProtectionFee { get; set; }

        /// <summary>
        /// 森林植被恢复费
        /// </summary>
        [LogColumn(@"森林植被恢复费", IsLog = true)]
        public decimal? ForestRrestorationFee { get; set; }

        /// <summary>
        /// 安全生产费
        /// </summary>
        [LogColumn(@"安全生产费", IsLog = true)]
        public decimal? SafetyProductionFee { get; set; }

        /// <summary>
        /// 安全鉴定费
        /// </summary>
        [LogColumn(@"安全鉴定费", IsLog = true)]
        public decimal? SafetyAssessmentFee { get; set; }

        /// <summary>
        /// 网络租赁费
        /// </summary>
        [LogColumn(@"网络租赁费", IsLog = true)]
        public decimal? NetworkRentalFee { get; set; }

        /// <summary>
        /// 系统运行维护监理费
        /// </summary>
        [LogColumn(@"系统运行维护监理费", IsLog = true)]
        public decimal? SystemoperationFee { get; set; }

        /// <summary>
        /// 项目建设管理费
        /// </summary>
        [LogColumn(@"项目建设管理费", IsLog = true)]
        public decimal? BuldManagerFee { get; set; }

        /// <summary>
        /// 代建管理费
        /// </summary>
        [LogColumn(@"代建管理费", IsLog = true)]
        public decimal? ACMF { get; set; }

        /// <summary>
        /// 工程保险费
        /// </summary>
        [LogColumn(@"工程保险费", IsLog = true)]
        public decimal? EngineeringInsuranceFee { get; set; }

        /// <summary>
        /// 招投标费
        /// </summary>
        [LogColumn(@"招投标费", IsLog = true)]
        public decimal? BiddingFee { get; set; }

        /// <summary>
        /// 合同公证费
        /// </summary>
        [LogColumn(@"合同公证费", IsLog = true)]
        public decimal? ContractNotarialFee { get; set; }

        /// <summary>
        /// 社会中介机构审计(查)费
        /// </summary>
        [LogColumn(@"社会中介机构审计(查)费", IsLog = true)]
        public decimal? AuditFee { get; set; }

        /// <summary>
        /// 工程检测费
        /// </summary>
        [LogColumn(@"工程检测费", IsLog = true)]
        public decimal? EngineeringInspectionFee { get; set; }

        /// <summary>
        /// 设备检验费
        /// </summary>
        [LogColumn(@"设备检验费", IsLog = true)]
        public decimal? EquipmentInspectionFee { get; set; }

        /// <summary>
        /// 负荷联合试车费
        /// </summary>
        [LogColumn(@"负荷联合试车费", IsLog = true)]
        public decimal? CombinedTestFee { get; set; }

        /// <summary>
        /// 律师代理费
        /// </summary>
        [LogColumn(@"律师代理费", IsLog = true)]
        public decimal? AttorneyFee { get; set; }

        /// <summary>
        /// 航道维护费
        /// </summary>
        [LogColumn(@"航道维护费", IsLog = true)]
        public decimal? ChannelMaintenanceFee { get; set; }

        /// <summary>
        /// 航标设施费
        /// </summary>
        [LogColumn(@"航标设施费", IsLog = true)]
        public decimal? NavigationAidsFee { get; set; }

        /// <summary>
        /// 航测费
        /// </summary>
        [LogColumn(@"航测费", IsLog = true)]
        public decimal? AerialSurveyFee { get; set; }

        /// <summary>
        /// 其他待摊投资性质支出
        /// </summary>
        [LogColumn(@"其他待摊投资性质支出", IsLog = true)]
        public decimal? OtherFee2 { get; set; }

        /// <summary>
        /// 不可预见费
        /// </summary>
        [LogColumn(@"不可预见费", IsLog = true)]
        public decimal? UnforeseenFee { get; set; }

        /// <summary>
        /// 项目前期总费用
        /// </summary>
        [LogColumn(@"项目前期总费用", IsLog = true)]
        public decimal? PreliminaryFee { get; set; }

        /// <summary>
        /// 征地总费用
        /// </summary>
        [LogColumn(@"征地总费用", IsLog = true)]
        public decimal? ExpropriationFee { get; set; }

        /// <summary>
        /// 工程设备总费用
        /// </summary>
        [LogColumn(@"工程设备总费用", IsLog = true)]
        public decimal? ProjectDeviceFee { get; set; }

        /// <summary>
        /// 其它总费用
        /// </summary>
        [LogColumn(@"其它总费用", IsLog = true)]
        public decimal? OtherTotalFee { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [LogColumn(@"总费用", IsLog = true)]
        public decimal? TotalFee { get; set; }

        #endregion


        //public decimal? ContractFee { get; set; }


        //public decimal? BudgetInvestmentFee { get; set; }


     
    }
}
