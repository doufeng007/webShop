using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Abp.File;
using Abp.WorkFlow.Service.Dto;

namespace Project
{
    public class UpdateOABidProjectStatusInput
    {
        public Guid BusinessId { get; set; }

        public OABidProjectStatus FromStatus { get; set; }

        public OABidProjectStatus ToStatus { get; set; }

    }


}
