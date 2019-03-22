using Abp.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class CreateOrUpdateProjectResultInput
    {

        public Guid? Id { get; set; }


        public Guid ProjectBaseId { get; set; }

        public List<GetAbpFilesOutput> Files { get; set; }

        public GetAbpFilesOutput CjzFile { get; set; }


        public List<CreateOrUpdateProjectBudgetControlInput> ProjectBudgetControlsInput { get; set; }


        /// <summary>
        /// 对应ProjectAuditRole
        /// </summary>
        public int AuditRoleId { get; set; }

        public string Remark { get; set; }


        public decimal? AuditAmount { get; set; }


        public CreateOrUpdateProjectResultInput()
        {
            this.Files = new List<GetAbpFilesOutput>();
            this.CjzFile = new GetAbpFilesOutput();
            ProjectBudgetControlsInput = new List<CreateOrUpdateProjectBudgetControlInput>();
        }
    }


    public class CreateUpdateProjectAuditInput
    {
        public Guid ProjectBaseId { get; set; }


        public List<CreateUpdateProjectAuditWithFinishInput> Results { get; set; }
        /// <summary>
        /// 1 评审 2 汇总
        /// </summary>
        public int Action { get; set; }


        public CreateUpdateProjectAuditInput()
        {
            this.Results = new List<CreateUpdateProjectAuditWithFinishInput>();
        }

    }

    public class CreateUpdateProjectAuditWithFinishInput
    {
        public List<AbpFileListInput> Files { get; set; }

        public AbpFileListInput CjzFile { get; set; }

        public string Remark { get; set; }


        public decimal? AuditAmount { get; set; }

        public Guid AllotId { get; set; }


        public Guid? Id { get; set; }

        /// <summary>
        /// 工程师自己确定的完成比例
        /// </summary>
        public decimal? SurePersent { get; set; }
        //public CreateUpdateProjectAuditWithFinishInput()
        //{
        //    this.Files = new List<AbpFileListInput>();
        //    this.CjzFile = new AbpFileListInput();
        //}
    }


}
