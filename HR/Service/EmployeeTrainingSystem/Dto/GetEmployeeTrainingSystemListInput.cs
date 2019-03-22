using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.Enum;
using ZCYX.FRMSCore.Application.Dto;

namespace HR
{
    public class GetEmployeeTrainingSystemListInput
    {
        /// <summary>
        /// 制度类型
        /// </summary>
        public TrainingSystemType Type { get; set; }

        public int ShowAllTrain { get; set; }
    }
}
