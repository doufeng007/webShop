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
    [TableNameAtribute("预算明细")]
    [Table("ProjectBudget")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectBudget : FullAuditedEntity<Guid>
    {
        #region 表字段



        public ProjectBudget DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as ProjectBudget;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BaseId")]
        public Guid BaseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FeasibilityStudyFee")]
        public decimal? FeasibilityStudyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SurveyFee")]
        public decimal? SurveyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("DesignFee")]
        public decimal? DesignFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ResearchTestFee")]
        public decimal? ResearchTestFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EnvironmentalImpactFee")]
        public decimal? EnvironmentalImpactFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OtherFee")]
        public decimal? OtherFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("LandAcquisitionFee")]
        public decimal? LandAcquisitionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("LandReclamationFee")]
        public decimal? LandReclamationFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ConstructionFee")]
        public decimal? ConstructionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("InstallFee")]
        public decimal? InstallFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("DeviceFee")]
        public decimal? DeviceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SupervisorFee")]
        public decimal? SupervisorFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("LandUseTax")]
        public decimal? LandUseTax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("FarmlandOccupationTax")]
        public decimal? FarmlandOccupationTax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("vehicleAndVesselTax")]
        public decimal? vehicleAndVesselTax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("StampDutyFee")]
        public decimal? StampDutyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("TemporaryFacilitiesFee")]
        public decimal? TemporaryFacilitiesFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("CulturaRrelicsProtectionFee")]
        public decimal? CulturaRrelicsProtectionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ForestRrestorationFee")]
        public decimal? ForestRrestorationFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SafetyProductionFee")]
        public decimal? SafetyProductionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SafetyAssessmentFee")]
        public decimal? SafetyAssessmentFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("NetworkRentalFee")]
        public decimal? NetworkRentalFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SystemoperationFee")]
        public decimal? SystemoperationFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BuldManagerFee")]
        public decimal? BuldManagerFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ACMF")]
        public decimal? ACMF { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EngineeringInsuranceFee")]
        public decimal? EngineeringInsuranceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("BiddingFee")]
        public decimal? BiddingFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ContractNotarialFee")]
        public decimal? ContractNotarialFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AuditFee")]
        public decimal? AuditFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EngineeringInspectionFee")]
        public decimal? EngineeringInspectionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EquipmentInspectionFee")]
        public decimal? EquipmentInspectionFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("CombinedTestFee")]
        public decimal? CombinedTestFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AttorneyFee")]
        public decimal? AttorneyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ChannelMaintenanceFee")]
        public decimal? ChannelMaintenanceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("NavigationAidsFee")]
        public decimal? NavigationAidsFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AerialSurveyFee")]
        public decimal? AerialSurveyFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OtherFee2")]
        public decimal? OtherFee2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UnforeseenFee")]
        public decimal? UnforeseenFee { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PreliminaryFee")]
        public decimal? PreliminaryFee { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ExpropriationFee")]
        public decimal? ExpropriationFee { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectDeviceFee")]
        public decimal? ProjectDeviceFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OtherTotalFee")]
        public decimal? OtherTotalFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("TotalFee")]
        public decimal? TotalFee { get; set; }



        #endregion
    }


}
