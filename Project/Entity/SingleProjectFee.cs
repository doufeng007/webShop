using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;

namespace Project
{
    [Serializable]
    [Table("SingleProjectFee")]
    public class SingleProjectFee : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// ProjectId
        /// </summary>
        [DisplayName(@"ProjectId")]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// SingleProjectId
        /// </summary>
        [DisplayName(@"SingleProjectId")]
        public Guid SingleProjectId { get; set; }

        /// <summary>
        /// FeasibilityStudyFee
        /// </summary>
        [DisplayName(@"FeasibilityStudyFee")]
        public decimal? FeasibilityStudyFee { get; set; }

        /// <summary>
        /// SurveyFee
        /// </summary>
        [DisplayName(@"SurveyFee")]
        public decimal? SurveyFee { get; set; }

        /// <summary>
        /// DesignFee
        /// </summary>
        [DisplayName(@"DesignFee")]
        public decimal? DesignFee { get; set; }

        /// <summary>
        /// ResearchTestFee
        /// </summary>
        [DisplayName(@"ResearchTestFee")]
        public decimal? ResearchTestFee { get; set; }

        /// <summary>
        /// EnvironmentalImpactFee
        /// </summary>
        [DisplayName(@"EnvironmentalImpactFee")]
        public decimal? EnvironmentalImpactFee { get; set; }

        /// <summary>
        /// OtherFee
        /// </summary>
        [DisplayName(@"OtherFee")]
        public decimal? OtherFee { get; set; }

        /// <summary>
        /// LandAcquisitionFee
        /// </summary>
        [DisplayName(@"LandAcquisitionFee")]
        public decimal? LandAcquisitionFee { get; set; }

        /// <summary>
        /// LandReclamationFee
        /// </summary>
        [DisplayName(@"LandReclamationFee")]
        public decimal? LandReclamationFee { get; set; }

        /// <summary>
        /// ConstructionFee
        /// </summary>
        [DisplayName(@"ConstructionFee")]
        public decimal? ConstructionFee { get; set; }

        /// <summary>
        /// InstallFee
        /// </summary>
        [DisplayName(@"InstallFee")]
        public decimal? InstallFee { get; set; }

        /// <summary>
        /// DeviceFee
        /// </summary>
        [DisplayName(@"DeviceFee")]
        public decimal? DeviceFee { get; set; }

        /// <summary>
        /// SupervisorFee
        /// </summary>
        [DisplayName(@"SupervisorFee")]
        public decimal? SupervisorFee { get; set; }

        /// <summary>
        /// LandUseTax
        /// </summary>
        [DisplayName(@"LandUseTax")]
        public decimal? LandUseTax { get; set; }

        /// <summary>
        /// FarmlandOccupationTax
        /// </summary>
        [DisplayName(@"FarmlandOccupationTax")]
        public decimal? FarmlandOccupationTax { get; set; }

        /// <summary>
        /// vehicleAndVesselTax
        /// </summary>
        [DisplayName(@"vehicleAndVesselTax")]
        public decimal? vehicleAndVesselTax { get; set; }

        /// <summary>
        /// StampDutyFee
        /// </summary>
        [DisplayName(@"StampDutyFee")]
        public decimal? StampDutyFee { get; set; }

        /// <summary>
        /// TemporaryFacilitiesFee
        /// </summary>
        [DisplayName(@"TemporaryFacilitiesFee")]
        public decimal? TemporaryFacilitiesFee { get; set; }

        /// <summary>
        /// CulturaRrelicsProtectionFee
        /// </summary>
        [DisplayName(@"CulturaRrelicsProtectionFee")]
        public decimal? CulturaRrelicsProtectionFee { get; set; }

        /// <summary>
        /// ForestRrestorationFee
        /// </summary>
        [DisplayName(@"ForestRrestorationFee")]
        public decimal? ForestRrestorationFee { get; set; }

        /// <summary>
        /// SafetyProductionFee
        /// </summary>
        [DisplayName(@"SafetyProductionFee")]
        public decimal? SafetyProductionFee { get; set; }

        /// <summary>
        /// SafetyAssessmentFee
        /// </summary>
        [DisplayName(@"SafetyAssessmentFee")]
        public decimal? SafetyAssessmentFee { get; set; }

        /// <summary>
        /// NetworkRentalFee
        /// </summary>
        [DisplayName(@"NetworkRentalFee")]
        public decimal? NetworkRentalFee { get; set; }

        /// <summary>
        /// SystemoperationFee
        /// </summary>
        [DisplayName(@"SystemoperationFee")]
        public decimal? SystemoperationFee { get; set; }

        /// <summary>
        /// BuldManagerFee
        /// </summary>
        [DisplayName(@"BuldManagerFee")]
        public decimal? BuldManagerFee { get; set; }

        /// <summary>
        /// ACMF
        /// </summary>
        [DisplayName(@"ACMF")]
        public decimal? ACMF { get; set; }

        /// <summary>
        /// EngineeringInsuranceFee
        /// </summary>
        [DisplayName(@"EngineeringInsuranceFee")]
        public decimal? EngineeringInsuranceFee { get; set; }

        /// <summary>
        /// BiddingFee
        /// </summary>
        [DisplayName(@"BiddingFee")]
        public decimal? BiddingFee { get; set; }

        /// <summary>
        /// ContractNotarialFee
        /// </summary>
        [DisplayName(@"ContractNotarialFee")]
        public decimal? ContractNotarialFee { get; set; }

        /// <summary>
        /// AuditFee
        /// </summary>
        [DisplayName(@"AuditFee")]
        public decimal? AuditFee { get; set; }

        /// <summary>
        /// EngineeringInspectionFee
        /// </summary>
        [DisplayName(@"EngineeringInspectionFee")]
        public decimal? EngineeringInspectionFee { get; set; }

        /// <summary>
        /// EquipmentInspectionFee
        /// </summary>
        [DisplayName(@"EquipmentInspectionFee")]
        public decimal? EquipmentInspectionFee { get; set; }

        /// <summary>
        /// CombinedTestFee
        /// </summary>
        [DisplayName(@"CombinedTestFee")]
        public decimal? CombinedTestFee { get; set; }

        /// <summary>
        /// AttorneyFee
        /// </summary>
        [DisplayName(@"AttorneyFee")]
        public decimal? AttorneyFee { get; set; }

        /// <summary>
        /// ChannelMaintenanceFee
        /// </summary>
        [DisplayName(@"ChannelMaintenanceFee")]
        public decimal? ChannelMaintenanceFee { get; set; }

        /// <summary>
        /// NavigationAidsFee
        /// </summary>
        [DisplayName(@"NavigationAidsFee")]
        public decimal? NavigationAidsFee { get; set; }

        /// <summary>
        /// AerialSurveyFee
        /// </summary>
        [DisplayName(@"AerialSurveyFee")]
        public decimal? AerialSurveyFee { get; set; }

        /// <summary>
        /// OtherFee2
        /// </summary>
        [DisplayName(@"OtherFee2")]
        public decimal? OtherFee2 { get; set; }

        /// <summary>
        /// UnforeseenFee
        /// </summary>
        [DisplayName(@"UnforeseenFee")]
        public decimal? UnforeseenFee { get; set; }

        /// <summary>
        /// PreliminaryFee
        /// </summary>
        [DisplayName(@"PreliminaryFee")]
        public decimal? PreliminaryFee { get; set; }

        /// <summary>
        /// ExpropriationFee
        /// </summary>
        [DisplayName(@"ExpropriationFee")]
        public decimal? ExpropriationFee { get; set; }

        /// <summary>
        /// ProjectDeviceFee
        /// </summary>
        [DisplayName(@"ProjectDeviceFee")]
        public decimal? ProjectDeviceFee { get; set; }

        /// <summary>
        /// OtherTotalFee
        /// </summary>
        [DisplayName(@"OtherTotalFee")]
        public decimal? OtherTotalFee { get; set; }

        /// <summary>
        /// TotalFee
        /// </summary>
        [DisplayName(@"TotalFee")]
        public decimal? TotalFee { get; set; }


        public decimal? ContractFee { get; set; }
        public decimal? BudgetInvestmentFee { get; set; }


        #endregion
    }
}