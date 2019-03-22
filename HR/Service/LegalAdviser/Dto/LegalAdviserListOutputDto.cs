using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    [AutoMapFrom(typeof(LegalAdviser))]
    public class LegalAdviserListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

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

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAcount { get; set; }


        public string EmailAddress { get; set; }

      


    }
}
