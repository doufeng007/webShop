using Abp.AutoMapper;
using Abp.File;
using Abp.Runtime.Validation;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply.Service
{
    /// <summary>
    /// 申请用品维修
    /// </summary>

    public class CreateSupplyRepairDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }


        public Guid UserSupplyId { get; set; }
        /// <summary>
        /// 用品id
        /// </summary>
        //public Guid SupplyId { get; set; }
        /// <summary>
        /// 维修时限
        /// </summary>
        public DateTime RepairEndTime { get; set; }
        /// <summary>
        /// 故障描述
        /// </summary>
        public string Des { get; set; }

        public bool IsImportant { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; }


        /// <summary>
        /// 维修结果
        /// </summary>
        public RepairResultEnum RepairResult { get; set; }


        /// <summary>
        /// 验收记录
        /// </summary>
        public string CheckRemark { get; set; }

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason { get; set; }
    }

    public class SupplyRepairDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Version { get; set; }
        public string Code { get; set; }
        public long UserId { get; set; }
        public string UserId_Name { get; set; }
        /// <summary>
        /// 用品id
        /// </summary>

        public Guid SupplyId { get; set; }
        /// <summary>
        /// 维修时限
        /// </summary>
        public DateTime RepairEndTime { get; set; }


        /// <summary>
        /// 故障描述
        /// </summary>
        public string Des { get; set; }

        public bool IsImportant { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; }

        public DateTime CreationTime { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }
        public decimal Money { get; set; }

        /// <summary>
        /// 维修结果
        /// </summary>
        public RepairResultEnum RepairResult { get; set; }


        public string RepairResult_Title { get; set; }

        /// <summary>
        /// 验收记录
        /// </summary>
        public string CheckRemark { get; set; }

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason { get; set; }

        public int SortStatus { get; set; }

        public Guid? SupplierId { get; set; }


        public string SupplierName { get; set; }
    }



    public class SupplyRepairOutDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Version { get; set; }
        public string Code { get; set; }
        public long UserId { get; set; }
        public string UserId_Name { get; set; }
        /// <summary>
        /// 用品id
        /// </summary>

        public Guid SupplyId { get; set; }
        /// <summary>
        /// 维修时限
        /// </summary>
        public DateTime RepairEndTime { get; set; }


        /// <summary>
        /// 故障描述
        /// </summary>
        public string Des { get; set; }

        public bool IsImportant { get; set; }
        public List<GetAbpFilesOutput> FileList { get; set; }

        public DateTime CreationTime { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }
        public decimal Money { get; set; }

        /// <summary>
        /// 维修结果
        /// </summary>
        public RepairResultEnum RepairResult { get; set; }

        /// <summary>
        /// 验收记录
        /// </summary>
        public string CheckRemark { get; set; }

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason { get; set; }


        public Guid? SupplierId { get; set; }


        public string SupplierName { get; set; }
    }


    public class SupplyRepairListInputDtondSortedInputDto : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public string Status { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}



