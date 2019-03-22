using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMapFrom(typeof(OAFixedAssets))]
    public class OAFixedAssetsDto : OAFixedAssetsCreateInput
    {
        public new Guid Id { get; set; }

        public int Status { get; set; }

        public string StatusTitle { get; set; }


    }
}
