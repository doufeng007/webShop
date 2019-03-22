using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(SingleProjectFee))]
    public class CreateSingleProjectFeeInput
    {
        //public Guid? Id { get; set; }

        //public Guid ProjectId { get; set; }

        //public Guid SingleProjectId { get; set; }

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
    }
}
