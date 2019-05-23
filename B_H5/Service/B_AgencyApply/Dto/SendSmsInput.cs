using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace B_H5.Service.B_AgencyApply.Dto
{
    public class SendSmsInput
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "必须填写手机号码")]
        public string Phone { get; set; }
    }
}
