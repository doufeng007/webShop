using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR
{
    [AutoMapTo(typeof(LegalAdviser))]
    public class CreateLegalAdviserInput
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Function
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// ScaleNum
        /// </summary>
        public int ScaleNum { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Head
        /// </summary>
        public string Head { get; set; }

        /// <summary>
        /// Tel
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// BankNum
        /// </summary>
        public string BankNum { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// BankDeposit
        /// </summary>
        public string BankDeposit { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }


        public CreateLegalAdviserInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }


        #endregion
    }

    public class UpdateLegalAdviserInput: CreateLegalAdviserInput
    {
        public Guid Id { get; set; }
    }
}