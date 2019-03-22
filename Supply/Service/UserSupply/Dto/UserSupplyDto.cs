using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply

{
    public class UserSupplyDto
    {
        public Guid Id { get; set; }

        public SupplyDto Supply { get; set; }

        public long UserId { get; set; }

        public string UserId_Name { get; set; }


        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }


        public UserSupplyDto()
        {

        }

    }


}
