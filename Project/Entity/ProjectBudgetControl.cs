using Abp.AutoMapper;
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

namespace Project
{

    [Serializable]

    [Table("ProjectBudgetControl")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class ProjectBudgetControl : FullAuditedEntity<Guid>
    {

        public ProjectBudgetControl DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as ProjectBudgetControl;
            }
        }

        #region 表字段

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Pro_Id")]
        public Guid Pro_Id { get; set; }


        public Guid SingleProjectId { get; set; }


        /// <summary>
        /// 
        /// </summary>

        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("CodeName")]
        public string CodeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ApprovalMoney")]
        public string ApprovalMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SendMoney")]
        public string SendMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ValidationMoney")]
        public string ValidationMoney { get; set; }

        #endregion

    }
}
