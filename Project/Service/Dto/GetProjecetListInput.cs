using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Validation;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetProjectListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }



        public DateTime? sendTime1 { get; set; }

        public DateTime? sendTime2 { get; set; }
        /// <summary>
        /// 项目评审组
        /// </summary>
        public Guid? GroupId { get; set; }
        /// <summary>
        /// 送审单位
        /// </summary>
        public int? SendUnitId { get; set; }
        /// <summary>
        /// 委托文号
        /// </summary>
        public string EntrustmentNumber { get; set; }

        public Guid? StepId { get; set; }


        public Guid? SearchId { get; set; }

        /// <summary>
        /// 查询重点项目
        /// </summary>
        public bool IsImportent { get; set; }
        /// <summary>
        /// 查询我的关注
        /// </summary>
        public bool IsFollow { get; set; }
        /// <summary>
        /// 0 待办 1完成
        /// </summary>
        public int? IsComplete { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " SendTime desc";
            }
        }
    }
    public class ProjectByXmfzrListOutput
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreationTime { get; set; }
    }
    public class GetProjectByXmfzrInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IsXmfzr { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

}
