using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace CWGL
{
    [AutoMapFrom(typeof(CWGLTravelReimbursement))]
    public class CWGLTravelReimbursementOutputDto : WorkFlowTaskCommentResult
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 报销人
        /// </summary>
        public string UserId_Name { get; set; }


        public long UserId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string OrgId_Name { get; set; }

        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 时长
        /// </summary>
        public decimal? Hours { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Reason { get; set; }


        public string Destination { get; set; }

        /// <summary>
        /// 出发地点
        /// </summary>
        public string FromPosition { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        public string TranTypeName { get; set; }


        /// <summary>
        /// 补充说明
        /// </summary>
        public string Note { get; set; }


        /// <summary>
        /// 电子资料
        /// </summary>
        public int? Nummber { get; set; }

        /// <summary>
        /// 关联备用金
        /// </summary>
        public Guid? BorrowMoneyId { get; set; }

        /// <summary>
        /// 备用金
        /// </summary>
        public decimal BorrowMoney { get; set; }

        /// <summary>
        /// 报销处理结果
        /// </summary>
        public int ResultType { get; set; }


        public Guid WorkOutId { get; set; }

        /// <summary>
        /// 报销合计金额
        /// </summary>
        public decimal TotalMoney { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();


        public List<CWGLTravelReimbursementDetailOutputDto> Details { get; set; } = new List<CWGLTravelReimbursementDetailOutputDto>();
    }
}
