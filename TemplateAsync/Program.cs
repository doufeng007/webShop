using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TemplateAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.AsyncTemplate();
            Console.WriteLine("模板同步完成");
        }
        private static SqlSugarClient Client
        {
            get
            {
                var sqlConn = ConfigHelper.GetSectionValue("sqlconnection");
                if (string.IsNullOrWhiteSpace(sqlConn))
                {
                    throw new Exception("请先配置连接字符串");
                }
                var Db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = sqlConn,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    IsShardSameThread = true //设为true相同线程是同一个SqlConnection
                                             //InitKeyType = InitKeyType.Attribute
                });
                return Db;
            }
        }
        /// <summary>
        /// 同步模板文件到数据库
        /// </summary>
        public static void AsyncTemplate()
        {
            var temPath = ConfigHelper.GetSectionValue("templatepath");
            if (string.IsNullOrWhiteSpace(temPath))
            {
                throw new Exception("请先配置模板文件路径");
            }
            if (Directory.Exists(temPath) == false)
            {
                throw new Exception($"文件夹{temPath}不存在。");
            }
           
            var vuefiles = Directory.GetFiles(temPath, "*.vue");
            foreach (var file in vuefiles)
            {
                var filename = Path.GetFileName(file);
                var template = new Dictionary<string, dynamic>();
                var templateType = 0;
                if (filename.Contains("-edit"))
                {
                    templateType = 0;
                }
                else
                {
                    templateType = 1;
                }
                var lastModificationTime = File.GetLastWriteTime(file);
                template.Add("LastModificationTime", lastModificationTime);
                var temtext= File.ReadAllText(file, Encoding.UTF8);
                template.Add("VueTemplate", temtext);
                //var t = new WorkFlowTemplate();
                //t.VueTemplate = temtext;
                //t.LastModificationTime = lastModificationTime;
                var workFlowModelId = Guid.Parse(filename.Replace("-edit.vue", "").Replace("-detail.vue", ""));
                var ex=  Client.Updateable(template).AS("WorkFlowTemplate").Where("WorkFlowModelId=@WorkFlowModelId and TemplateType=@TemplateType and (LastModificationTime!=@LastModificationTimeW or LastModificationTime is null)", new
                {
                    WorkFlowModelId = workFlowModelId,
                    TemplateType = templateType,
                    LastModificationTimeW = lastModificationTime
                });
                var sql = ex.ToSql();
                var r = Client.Ado.ExecuteCommand(sql.Key,sql.Value);
                if (r == 1)
                {
                    Console.WriteLine($"模板{filename}已更新");
                }
                else {
                    Console.WriteLine($"模板{filename}跳过更新");
                }
            }
           
        }

    }
    public class WorkFlowTemplate {

        //public Guid WorkFlowModelId { get; set; }

        //public int TemplateType { get; set; }

        public string VueTemplate { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
    public static class ConfigHelper
    {
        private static IConfiguration _configuration;

        static ConfigHelper()
        {
            //在当前目录或者根目录中寻找appsettings.json文件
            var fileName = "appsettings.json";

            var directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            if (!File.Exists(filePath))
            {
                var length = directory.IndexOf("/bin");
                filePath = $"{directory.Substring(0, length)}/{fileName}";
            }

            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);

            _configuration = builder.Build();
        }

        public static string GetSectionValue(string key)
        {
            return _configuration.GetSection(key).Value;
        }
    }
}
