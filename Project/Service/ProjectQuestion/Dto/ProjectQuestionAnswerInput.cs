using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project
{
    public class ProjectQuestionAnswerInput
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 回复
        /// </summary>
       
        public string Answer { get; set; }
    }
}
