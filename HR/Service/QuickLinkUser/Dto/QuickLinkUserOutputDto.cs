using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace HR
{
    public class QuickLinkWithUserOutputDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }


        public string Name { get; set; }


        public string Link { get; set; }


        public int Sort { get; set; }


        public bool IsSelect { get; set; }


    }
}
