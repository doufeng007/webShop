using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Project
{
    public class RabbitMqPublish
    {
        public static ConnectionFactory RabbitFactory = new ConnectionFactory()
        {
            HostName = System.Configuration.ConfigurationManager.AppSettings["rabbitMQHost"],
            Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["rabbitMQPort"]),
            UserName = System.Configuration.ConfigurationManager.AppSettings["rabbitMQUserName"],
            Password = System.Configuration.ConfigurationManager.AppSettings["rabbitMQPwd"]
        };



        public RabbitMqPublish()
        {

        }




        public static void PushInfo(string queueName, string message, string exchangeName = "")
        {
            using (var connection = RabbitFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //声明queue
                channel.QueueDeclare(queue: queueName, //队列名
                    durable: false, //是否持久化
                    exclusive: false, //排它性
                    autoDelete: false, //一旦客户端连接断开则自动删除queue
                    arguments: null); //如果安装了队列优先级插件则可以设置优先级

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchangeName, //exchange名称
                    routingKey: queueName, //如果存在exchange,则消息被发送到名称为hello的queue的客户端
                    basicProperties: null,
                    body: body); //消息体

            }
        }




    }

}
