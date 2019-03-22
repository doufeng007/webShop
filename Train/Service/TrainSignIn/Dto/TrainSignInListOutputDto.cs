using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Train
{
    [AutoMapFrom(typeof(TrainSignIn))]
    public class TrainSignInListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 培训编号
        /// </summary>
        public Guid TrainId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SignInTime { get; set; }

        /// <summary>
        /// 签退时间
        /// </summary>
        public DateTime? SignOutTime { get; set; }


    }
    public class TrainSignInOutputDto
    {
        public string UserName { get; set; }
        public string TrainTitle { get; set; }
        public int SignState { get; set; }
        public DateTime SignDate { get; set; }
    }


}
