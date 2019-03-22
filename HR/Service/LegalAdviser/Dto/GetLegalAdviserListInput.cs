using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetLegalAdviserListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        
    }
}
