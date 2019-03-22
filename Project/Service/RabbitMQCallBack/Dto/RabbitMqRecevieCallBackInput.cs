using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class RabbitMqRecevieCallBackInput
    {
        public RabbitMqMessageType MessageType { get; set; }

        public string Parameter { get; set; }
    }

    public class RabbitMqRecevieCallBackOutput
    {
        public RabbitMqCallBackResultType Result { get; set; }
    }

    public enum RabbitMqMessageType
    {
        项目数据 = 1,
        项目评审结果 = 2,
    }

    public enum RabbitMqCallBackResultType
    {
        Succesefull = 1,
        Fail = 2,
        Agin =3,

    }

    



}
