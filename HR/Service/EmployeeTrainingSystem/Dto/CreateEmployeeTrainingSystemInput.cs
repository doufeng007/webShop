using System;
using System.Collections.Generic;
using System.Text;
using HR.Enum;

namespace HR
{
    public class CreateEmployeeTrainingSystemInput
    {
        public string Title { get; set; }

        /// <summary>
        /// 制度内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 制度类型
        /// </summary>
        public TrainingSystemType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Guid> PortsIds { get; set; }
    }
}
