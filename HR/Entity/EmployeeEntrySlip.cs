using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HR
{
    [Table("EmployeeEntrySlip")]
    public class EmployeeEntrySlip : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }


        #region 表字段



        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EmployeeInterviewId")]
        public Guid EmployeeInterviewId { get; set; }


        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EntryDate")]
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasIdCard")]
        public bool HasIdCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasDiploma")]
        public bool HasDiploma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasGraduate")]
        public bool HasGraduate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasSocialCard")]
        public bool HasSocialCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasPhoto")]
        public bool HasPhoto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasDrivingLicense")]
        public bool HasDrivingLicense { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasVocationalQualification")]
        public bool HasVocationalQualification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasTechnologyLevelCard")]
        public bool HasTechnologyLevelCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasSpecialCard")]
        public bool HasSpecialCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasLeavingCertificate")]
        public bool HasLeavingCertificate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasOtherCertificate")]
        public bool HasOtherCertificate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OtherCertificate")]
        public string OtherCertificate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasContract")]
        public bool HasContract { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasRegistration")]
        public bool HasRegistration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasAbnormalChange")]
        public bool HasAbnormalChange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasJobDuty")]
        public bool HasJobDuty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasTrain")]
        public bool HasTrain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasHRConfirm")]
        public bool HasHRConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HRConfirmDate")]
        public DateTime HRConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EmployeeHRConfirm")]
        public bool EmployeeHRConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EmployeeHRConfirmDate")]
        public DateTime EmployeeHRConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasChangeMailList")]
        public bool HasChangeMailList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasStation")]
        public bool HasStation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasEmail")]
        public bool HasEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Email")]
        public bool Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ComputerProvide")]
        public int ComputerProvide { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("OfficeSupplies")]
        public int OfficeSupplies { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasJobIntroduce")]
        public bool HasJobIntroduce { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasProcessIntroduce")]
        public bool HasProcessIntroduce { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasDepartConfirm")]
        public bool HasDepartConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("DepartConfirmDate")]
        public DateTime DepartConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EmployeeDepConfirm")]
        public bool EmployeeDepConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("EmployeeDepConfirmDate")]
        public DateTime EmployeeDepConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasCompleteSocial")]
        public bool HasCompleteSocial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasCompleteRoster")]
        public bool HasCompleteRoster { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Other")]
        public string Other { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("HasPersonLiableConfirm")]
        public bool HasPersonLiableConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("PersonLiableConfirmDate")]
        public DateTime PersonLiableConfirmDate { get; set; }

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
}
