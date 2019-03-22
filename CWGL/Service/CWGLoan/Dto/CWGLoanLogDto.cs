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
	[AutoMapFrom(typeof(CWGLoan))]
    public class CWGLoanLogDto
    {
        #region 表字段
        /// <summary>
        /// 贷款金额
        /// </summary>
        [LogColumn(@"贷款金额", IsLog = true)]
        public decimal Money { get; set; }

        /// <summary>
        /// 贷款用途及备注
        /// </summary>
        [LogColumn(@"贷款用途及备注", IsLog = true)]
        public string Remark { get; set; }

        #endregion
    }
}