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

namespace Supply
{
    public class UserSupplyBatchCreateInput
    {
        /// <summary>
        /// 用品与用户管理id 新增为空
        /// </summary>
        public Guid? Id { get; set; }

        public string Unit { get; set; }
        /// <summary>
        /// 用品id  新增为空
        /// </summary>
        public Guid? SupplyId { get; set; }

        public string Name { get; set; }


        public string Version { get; set; }


        public decimal Money { get; set; }


        public int Type { get; set; }


        public string Code { get; set; }


        /// <summary>
        /// 用品所属者
        /// </summary>
        public string Supply_UserId { get; set; }


        /// <summary>
        /// 保存自己的用品传空；
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 领用日期
        /// </summary>
        public DateTime StartTime { get; set; }


        /// <summary>
        /// 检定到期日期
        /// </summary>
        public DateTime? EndTime { get; set; }



        public UserSupplyBatchCreateInput()
        {

        }
    }





}
