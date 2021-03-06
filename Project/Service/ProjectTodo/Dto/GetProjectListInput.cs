﻿using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
  

    public class GetProjectTodoListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }


        public string SearchName { get; set; }


        public DateTime? sendTime1 { get; set; }

        public DateTime? sendTime2 { get; set; }

        public Guid? StepId { get; set; }

        public int? UserId { get; set; }
        public Guid? GovId { get; set; }

        /// <summary>
        /// 0 待办 1完成
        /// </summary>
        public int IsComplete { get; set; }

        /// <summary>
        ///  0表示所有类别 1 表示项目类   2 表示非项目类
        /// </summary>
        public int TaskType { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " ReceiveTime desc";
            }
        }
    }

    


}
