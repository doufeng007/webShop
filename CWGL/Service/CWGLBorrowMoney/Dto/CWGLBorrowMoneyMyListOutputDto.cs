using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using CWGL.Enums;
using ZCYX.FRMSCore.Application.Dto;
using Abp.Runtime.Validation;

namespace CWGL
{
    public class CWGLBorrowMoneyMyListOutputDto 
    {

        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        public string UserName { get; set; }
        public bool IsPayBack { get; set; }
        public string DepartmentName { get; set; }
    }

    public class CWGLBorrowMoneyMyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
