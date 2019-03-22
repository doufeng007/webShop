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
    public class UpdateOAFixedAssetsStatusInput
    {
        public Guid BusinessId { get; set; }



        public OAFixedAssetsStatus Status { get; set; }


        public OAFixedAssetsStatus ToStatus { get; set; }
    }


}
