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
    [AutoMap(typeof(ProjectSupplement))]

    public class ProjectSupplementListDto
    {
        public Guid Id { get; set; }
        public string TableName { get; set; }

        public string ColumnName { get; set; }

    }
}
