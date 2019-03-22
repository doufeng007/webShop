using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace CWGL
{
    public class CWGLTravelReimbursementLogDto
    {
        #region 表字段
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// 金额
        /// </summary>
        [LogColumn(@"金额", IsLog = true)]
        public decimal Money { get; set; }

        /// <summary>
        /// 事由摘要
        /// </summary>
        [LogColumn(@"补充说明", IsLog = true)]
        public string Note { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        [LogColumn(@"纸质资料份数", IsLog = true)]
        public int? Nummber { get; set; }

        [LogColumn(@"报销明细", IsLog = true)]
        public List<CWGLTravelReimbursementDetailLogDto> Detail { get; set; } = new List<CWGLTravelReimbursementDetailLogDto>();


        [LogColumn("电子资料")]
        public List<AbpFileChangeDto> Files { get; set; } = new List<AbpFileChangeDto>();

        #endregion
    }
}