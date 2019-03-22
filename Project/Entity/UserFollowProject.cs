using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 用户项目关注表
    /// </summary>
    [Table("UserFollowProject")]
    public class UserFollowProject:Entity<Guid>
    {
        public long Userid { get; set; }

        public Guid Projectid { get; set; }


        public string ProjectCode { get; set; }
    }
}
