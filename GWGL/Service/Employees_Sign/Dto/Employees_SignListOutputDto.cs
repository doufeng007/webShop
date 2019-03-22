using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace GWGL
{
    public class Employees_SignListOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public string User_Name { get; set; }


        public string User_OrgName { get; set; }

        /// <summary>
        /// SignType
        /// </summary>
        public string SignType_Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status_Title { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        public DateTime? ActiveDate { get; set; }


        


    }
}
