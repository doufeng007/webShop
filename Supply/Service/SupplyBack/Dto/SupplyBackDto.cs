using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using Supply.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Supply.Service
{
    /// <summary>
    /// 创建退还申请
    /// </summary>
   public class CreateSupplyBackMainInput: CreateWorkFlowInstance
    {
        ///// <summary>
        ///// 退还物品列表
        ///// </summary>
        //public List<CreateSupplyBackSubInput> SupplyBackSub { get; set; }
        public List< Guid> SupplyBackSub { get; set; }
    }
    public class SupplyBackInput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 0退还1驳回
        /// </summary>
        public int Type { get; set; }
    }
    ///// <summary>
    ///// 创建退还申请明细
    ///// </summary>
    //[AutoMap(typeof(SupplyBackSub))]
    //public class CreateSupplyBackSubInput {
    //    //public Guid? Id { get; set; }

    //    //public Guid? MainId { get; set; }

    //    public Guid SupplyId { get; set; }
    //}
    /// <summary>
    /// 退还申请详情
    /// </summary>
    [AutoMap(typeof(SupplyBackMain))]
    public class SupplyBackMainDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public List<SupplyBackSubDto> SupplyBackSub { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string UserId_Name { get; set; }
        
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
    /// <summary>
    /// 退还申请详情明细
    /// </summary>
    [AutoMap(typeof(SupplyBackSub))]
    public class SupplyBackSubDto {

        public Guid Id { get; set; }
        /// <summary>
        /// 退还申请状态 0:申请中  1：已退还
        /// </summary>
        public int Status { get; set; }
        public string StatusTitle { get; set; }
        /// <summary>
        /// 物品id
        /// </summary>
        public Guid SupplyId { get; set; }
        /// <summary>
        /// 主表id
        /// </summary>
        public Guid MainId { get; set; }
        public string Name { get; set; }

        public string Version { get; set; }

        public decimal Money { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        public string UserId_Name { get; set; }
        /// <summary>
        /// 领用时间
        /// </summary>

        public DateTime StartTime { get; set; }
        /// <summary>
        /// 申请退还时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
