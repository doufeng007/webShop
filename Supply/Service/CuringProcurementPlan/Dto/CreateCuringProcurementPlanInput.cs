using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;

namespace Supply
{
    [AutoMapTo(typeof(CuringProcurementPlan))]
    public class CreateCuringProcurementPlanInput
    {
        #region 表字段
        /// <summary>
        /// MainId
        /// </summary>
        public Guid MainId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Money
        /// </summary>
        public string Money { get; set; }

        /// <summary>
        /// Des
        /// </summary>
        public string Des { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///   审批中 = 0,同意 = 1,驳回 = 2
        /// </summary>
        public int Status { get; set; }

        public int BusinessType { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }


        public CreateCuringProcurementPlanInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }


        #endregion
    }



    public class UpdateCuringProcurementPlanInput : CreateCuringProcurementPlanInput
    {
        public Guid Id { get; set; }
    }

    [AutoMapTo(typeof(CuringProcurementPlan))]
    public class CreateOrUpdateCuringProcurementPlanInput : CreateCuringProcurementPlanInput
    {
        public Guid? Id { get; set; }
    }
}