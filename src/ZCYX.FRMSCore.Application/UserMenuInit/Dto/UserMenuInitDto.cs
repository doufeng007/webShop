using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{

    public class UserMenuInitFlag
    {
        /// <summary>
        /// 是否完成初始化
        /// </summary>
        public bool HasComplateInit { get; set; }

    }

    public class UserMenuInitInput
    {
        /// <summary>
        /// 是否完成初始化
        /// </summary>
        public Guid MenuId { get; set; }

    }


    public class InitMenuPagedResultDto<T> : PagedResultDto<T>
    {
        public InitMenuPagedResultDto() : base()
        { }
        public InitMenuPagedResultDto(int totalCount, IReadOnlyList<T> items) : base(totalCount, items)
        {

        }

        public InitMenuPagedResultDto(int totalCount, IReadOnlyList<T> items, bool hasComplateInit) : base(totalCount, items)
        {
            this.HasComplateInit = hasComplateInit;
        }
        /// <summary>
        /// 是否完成初始化
        /// </summary>
        public bool HasComplateInit { get; set; }
    }
}
