using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    [AutoMapFrom(typeof(B_CWDetail))]
    public class B_CWDetailListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }


        public string UserName { get; set; }

        /// <summary>
        /// RelationUserId
        /// </summary>
        public long? RelationUserId { get; set; }

        /// <summary>
        /// 下级进货时，自身出仓的收货方
        /// </summary>
        public string RelationUserName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public CWDetailTypeEnum Type { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public CWDetailBusinessTypeEnum BusinessType { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid CategroyId { get; set; }


        public string CategroyName { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        public int Number { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
