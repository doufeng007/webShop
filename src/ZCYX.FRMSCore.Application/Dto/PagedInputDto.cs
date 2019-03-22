using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using System;
using Abp.Runtime.Validation;

namespace ZCYX.FRMSCore.Application.Dto
{
    public class PagedInputDto : IPagedResultRequest
    {
        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public PagedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }

    public class AppConsts
    {
        /// <summary>
        /// Default page size for paged requests.
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// Maximum allowed page size for paged requests.
        /// </summary>
        public const int MaxPageSize = 1000;
    }


    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest, IShouldNormalize
    {
        public string SearchKey { get; set; }

        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime desc";
            }
        }
    }

    public class WorkFlowPagedAndSortedInputDto : PagedAndSortedInputDto, ISortedResultRequest
    {
        public Guid FlowId { get; set; }
        public bool GetMy { get; set; }

        public WorkFlowPagedAndSortedInputDto()
        {

        }
    }

    public class PageSubDto : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }




    public class GetListInput : WorkFlowPagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }

        public string Status { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime desc";
            }
        }
    }

}