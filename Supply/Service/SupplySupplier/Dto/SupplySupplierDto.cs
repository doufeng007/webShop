using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace Supply
{
    [AutoMapFrom(typeof(SupplySupplier))]
    public class SupplySupplierDto:CreateSupplySupplierInput
    {
        public Guid Id { get; set; }
    }
}