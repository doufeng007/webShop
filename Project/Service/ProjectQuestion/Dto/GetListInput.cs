using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public  class GetListInput: PagedInputDto
    {
        public Guid ProjectId { get; set; }
    }
}
