using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    /// <summary>
    /// 代理列表参数
    /// </summary>
    public class GetB_AgencyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 获取该用户的代理数据 为空则获取所有
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 代理级别-数据字典对应id 为空则全部
        /// </summary>
        public Guid? AgencyLevelId { get; set; }



        /// <summary>
        /// 代理类别
        /// </summary>
        public B_AgencyTypeEnum Type { get; set; }


        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }



    public class GetB_AgencyManagerListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 获取该用户的代理数据 为空则获取所有
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 代理级别-数据字典对应id 为空则全部
        /// </summary>
        public Guid? AgencyLevelId { get; set; }


        /// <summary>
        ///  
        /// </summary>
        public B_AgencyAcountStatusEnum? Status { get; set; }


        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
