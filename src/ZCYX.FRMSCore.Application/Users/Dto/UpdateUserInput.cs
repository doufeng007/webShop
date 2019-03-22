using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Users.Dto
{
    public class UpdateUserInput
    {
        [Required]
        public UserEditDto User { get; set; }

        [Required]
        public string[] AssignedRoleNames { get; set; }

        public bool SendActivationEmail { get; set; }

        public bool SetRandomPassword { get; set; }

        
        /// <summary>
        /// 用户岗位
        /// </summary>
        public List<Guid> PostIds { get; set; }
        /// <summary>
        /// 主岗位
        /// </summary>
        public Guid MainPostId { get; set; }
        public List<Guid> RealtionSystemIds { get; set; }
       
        public UpdateUserInput()
        {
            this.User = new UserEditDto();
            PostIds = new List<Guid>();
            RealtionSystemIds = new List<Guid>();
        }
    }
    public class UserOutput
    {
        public long Id{ get; set; }
        public string UserName{ get; set; }
    }
}
