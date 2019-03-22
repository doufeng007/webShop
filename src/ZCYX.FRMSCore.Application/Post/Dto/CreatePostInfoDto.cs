using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapTo(typeof(PostInfo))]
    public class CreatePostInfoDto
    {
        public string Name { get; set; }

        public string Summary { get; set; }


    }
}
