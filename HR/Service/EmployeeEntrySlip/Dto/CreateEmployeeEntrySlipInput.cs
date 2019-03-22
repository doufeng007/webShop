using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMapTo(typeof(EmployeeEntrySlip))]
    public class CreateEmployeeEntrySlipInput: CreateWorkFlowInstance
    {
        public Guid EmployeeInterviewId { get; set; }

        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EntryDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasIdCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasDiploma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasGraduate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasSocialCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasPhoto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasDrivingLicense { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasVocationalQualification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasTechnologyLevelCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasSpecialCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasLeavingCertificate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasOtherCertificate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OtherCertificate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasContract { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasRegistration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasAbnormalChange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasJobDuty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasTrain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasHRConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime HRConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EmployeeHRConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EmployeeHRConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasChangeMailList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasStation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ComputerProvide { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int OfficeSupplies { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasJobIntroduce { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasProcessIntroduce { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasDepartConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DepartConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EmployeeDepConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EmployeeDepConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasCompleteSocial { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasCompleteRoster { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasPersonLiableConfirm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PersonLiableConfirmDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
    }
}
