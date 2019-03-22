using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(OAFixedAssets))]
    public class OAFixedAssetsListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public string Brand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }


        public string Remark { get; set; }


        public DateTime DateOfManufacture { get; set; }


        public DateTime PostingDate { get; set; }

        public string StatusTitle { get; set; }


    }
}
