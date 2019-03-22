using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using System.IO;

namespace Project
{

    public class GetProjectAuditSmmaryOutput
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }

        public string SingleProjectName { get; set; }

        public string SingleProjectCode { get; set; }

        public string ReturnAuditSmmary { get; set; }
        public decimal? SendTotalBudget { get; set; }

    }

    public class GetProjectBudgetForEditOutput
    {


        public CreateOrUpdateProjectBaseInput BaseOutput { get; set; }


        public List<SingleProjectInfoOutput> SingleProjectInfos { get; set; }


        public ProjectAuditTotalResultOutput AuditResultOutput { get; set; }

        public List<CreateOrUpdateProjectAuditMembersInput> ProjectAuditMembersOutput { get; set; }








        public GetProjectBudgetForEditOutput()
        {
            this.BaseOutput = new CreateOrUpdateProjectBaseInput();
            //this.ProjectBudgetControls = new List<CreateOrUpdateProjectBudgetControlInput>();
            this.SingleProjectInfos = new List<SingleProjectInfoOutput>();
            AuditResultOutput = new ProjectAuditTotalResultOutput();
            this.ProjectAuditMembersOutput = new List<CreateOrUpdateProjectAuditMembersInput>();
        }

        public GetProjectBudgetForEditOutput(CreateOrUpdateProJectBudgetManagerInput input)
        {
            this.BaseOutput = new CreateOrUpdateProjectBaseInput()
            {
                Id = input.Id,
                AppraisalTypeId = input.AppraisalTypeId,
                ProjectName = input.ProjectName,
                ProjectCode = input.ProjectCode,
                SingleProjectName = input.SingleProjectName,
                SingleProjectCode = input.SingleProjectCode,
                SendUnit = input.SendUnit,
                CompetentUnit = input.CompetentUnit,

                EntrustmentNumber = input.EntrustmentNumber,
                SendTotalBudget = input.SendTotalBudget,
                SingleBudget = input.SingleBudget,
                SafaBudget = input.SafaBudget,
                ProjectAdress = input.ProjectAdress,
                ProjectNature = input.ProjectNature,
                ProjectNature1 = input.ProjectNature1,
                Industry = input.Industry,
                Contacts = input.Contacts,
                ContactsTel = input.ContactsTel,
                SendTime = input.SendTime,
                UnitRoom = input.UnitRoom,
                PersonLiable = input.PersonLiable,
                Days = input.Days,
                Remark = input.Remark,
                ApprovalUnit = input.ApprovalUnit,
                ApprovalNum = input.ApprovalNum,
                Is_Important = input.Is_Important,
                IsIncludeSingle = input.IsIncludeSingle,
                Area_Id = input.Area_Id,
                Gov_Code = input.Gov_Code,
                Gov_ProjectId = input.Gov_ProjectId,
                ACMF = input.ACMF,
                AerialSurveyFee = input.AerialSurveyFee,
                AttorneyFee = input.AttorneyFee,
                AuditFee = input.AuditFee,
                BiddingFee = input.BiddingFee,
                BuldManagerFee = input.BuldManagerFee,
                ChannelMaintenanceFee = input.ChannelMaintenanceFee,
                CombinedTestFee = input.CombinedTestFee,
                ConstructionFee = input.ConstructionFee,
                ContractNotarialFee = input.ContractNotarialFee,
                CulturaRrelicsProtectionFee = input.CulturaRrelicsProtectionFee,
                DesignFee = input.DesignFee,
                DeviceFee = input.DeviceFee,
                EngineeringInspectionFee = input.EngineeringInspectionFee,
                EngineeringInsuranceFee = input.EngineeringInsuranceFee,
                EnvironmentalImpactFee = input.EnvironmentalImpactFee,
                EquipmentInspectionFee = input.EquipmentInspectionFee,
                FarmlandOccupationTax = input.FarmlandOccupationTax,
                FeasibilityStudyFee = input.FeasibilityStudyFee,
                ForestRrestorationFee = input.ForestRrestorationFee,
                InstallFee = input.InstallFee,
                LandAcquisitionFee = input.LandAcquisitionFee,
                LandReclamationFee = input.LandReclamationFee,
                LandUseTax = input.LandUseTax,
                NavigationAidsFee = input.NavigationAidsFee,
                NetworkRentalFee = input.NetworkRentalFee,
                OtherFee = input.OtherFee,
                OtherFee2 = input.OtherFee2,
                ResearchTestFee = input.ResearchTestFee,
                SafetyAssessmentFee = input.SafetyAssessmentFee,
                SafetyProductionFee = input.SafetyProductionFee,
                StampDutyFee = input.StampDutyFee,
                SupervisorFee = input.SupervisorFee,
                SurveyFee = input.SurveyFee,
                SystemoperationFee = input.SystemoperationFee,
                TemporaryFacilitiesFee = input.TemporaryFacilitiesFee,
                UnforeseenFee = input.UnforeseenFee,
                vehicleAndVesselTax = input.vehicleAndVesselTax,
                AdjustMatter = input.AdjustMatter,
                // AreaName = "",
                AuditType = input.AuditType,
                BudgetInvestmentFee = input.BudgetInvestmentFee,
                CheckMatter = input.CheckMatter,
                CompanyContact = input.CompanyContact,
                CompanyContactTel = input.CompanyContactTel,
                ConsultationMatter = input.ConsultationMatter,
                ContractFee = input.ContractFee,
                EvaluateEndDate = input.EvaluateEndDate,
                EvaluateMatter = input.EvaluateMatter,
                EvaluateStartDate = input.EvaluateStartDate,


            };
        }

        //public void CalculateFee()
        //{
        //    this.PreliminaryFee = (this.BaseOutput.FeasibilityStudyFee ?? 0) + (this.BaseOutput.SurveyFee ?? 0) + (this.BaseOutput.DesignFee ?? 0) + (this.BaseOutput.ResearchTestFee ?? 0) + (this.BaseOutput.EnvironmentalImpactFee ?? 0) + (this.BaseOutput.OtherFee ?? 0);
        //    this.ExpropriationFee = (this.BaseOutput.LandAcquisitionFee ?? 0) + (this.BaseOutput.LandReclamationFee ?? 0);
        //    this.ProjectDeviceFee = (this.BaseOutput.DeviceFee ?? 0) + (this.BaseOutput.InstallFee ?? 0) + (this.BaseOutput.ConstructionFee ?? 0);
        //    this.OtherTotalFee = (this.BaseOutput.SupervisorFee ?? 0) + (this.BaseOutput.LandUseTax ?? 0) + (this.BaseOutput.FarmlandOccupationTax ?? 0) + (this.BaseOutput.vehicleAndVesselTax ?? 0) + (this.BaseOutput.StampDutyFee ?? 0) + (this.BaseOutput.TemporaryFacilitiesFee ?? 0) + (this.BaseOutput.CulturaRrelicsProtectionFee ?? 0) + (this.BaseOutput.ForestRrestorationFee ?? 0) + (this.BaseOutput.SafetyProductionFee ?? 0) + (this.BaseOutput.SafetyAssessmentFee ?? 0) + (this.BaseOutput.NetworkRentalFee ?? 0) + (this.BaseOutput.BuldManagerFee ?? 0)
        //        + (this.BaseOutput.ACMF ?? 0) + (this.BaseOutput.EngineeringInsuranceFee ?? 0) + (this.BaseOutput.BiddingFee ?? 0) + (this.BaseOutput.ContractNotarialFee ?? 0) + (this.BaseOutput.EngineeringInspectionFee ?? 0)
        //        + (this.BaseOutput.EquipmentInspectionFee ?? 0) + (this.BaseOutput.CombinedTestFee ?? 0) + (this.BaseOutput.AttorneyFee ?? 0) + (this.BaseOutput.ChannelMaintenanceFee ?? 0) + (this.BaseOutput.NavigationAidsFee ?? 0)
        //        + (this.BaseOutput.AerialSurveyFee ?? 0) + (this.BaseOutput.UnforeseenFee ?? 0) + (this.BaseOutput.SystemoperationFee ?? 0) + (this.BaseOutput.AuditFee ?? 0) + (this.BaseOutput.OtherFee2 ?? 0);
        //    this.TotalFee = this.PreliminaryFee + this.ExpropriationFee + this.ProjectDeviceFee + this.OtherTotalFee;
        //}

    }





    public class GetProjectForEditInput
    {
        public Guid Id { get; set; }

        public Guid? StepId { get; set; }

        public int? AppraisalTypeId { get; set; }

        /// <summary>
        /// 按评审角色获取项目信息 （主要针对 送审资料） 为空获取所有；
        /// </summary>
        public int? AuditRoleId { get; set; }
    }

    public class GetSingleProjectInput
    {
        public Guid Id { get; set; }

        public Guid? StepId { get; set; }

        public int? AppraisalTypeId { get; set; }

        /// <summary>
        /// 按评审角色获取项目信息 （主要针对 送审资料） 为空获取所有；
        /// </summary>
        public int? AuditRoleId { get; set; }
    }

}
