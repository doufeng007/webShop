using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_TeamSaleBonus))]
    public class CreateB_TeamSaleBonusInput 
    {
        #region 表字段


        public List<CreateB_TeamSaleBonusDetailInput> Details { get; set; } = new List<CreateB_TeamSaleBonusDetailInput>();




        #endregion
    }
}