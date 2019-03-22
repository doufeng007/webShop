using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace Project
{
    [Serializable]
    [TableNameAtribute("预算控制评审结果表")]
    [Table("ProjectBudgetControlAuditResult")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectBudgetControlAuditResult : FullAuditedEntity<Guid>
    {

        public ProjectBudgetControlAuditResult DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as ProjectBudgetControlAuditResult;
            }
        }

        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Pro_Id")]
        public Guid Pro_Id { get; set; }


        [DisplayName("ControlId")]
        public Guid ControlId { get; set; }


        public long UserId { get; set; }


        public int UserAuditRoleId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ValidationMoney")]
        public string ValidationMoney { get; set; }

        #endregion

    }


}
