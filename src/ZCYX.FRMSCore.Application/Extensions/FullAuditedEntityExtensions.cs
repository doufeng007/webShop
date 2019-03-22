using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MessagePack;
using Newtonsoft.Json;

namespace ZCYX.FRMSCore.Extensions
{
    public static class FullAuditedEntityExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            try
            {
                using (Stream objectStream = new MemoryStream())
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(objectStream, obj);
                    objectStream.Seek(0, SeekOrigin.Begin);
                    return (T)formatter.Deserialize(objectStream);
                }
            }
            catch (Exception e)
            {
                //当实体没有添加序列化标识时，可以采用json序列化
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
            }
        }


        public static List<LogColumnModel> GetColumnLogs<T>(this T obj, T newObj)
        {
            return ModelColumnLogsHepler.GetColumnLogs(obj, newObj);
        }

        public static List<LogColumnModel> GetColumnAllLogs(this object obj, object newObj)
        {
            return ModelColumnLogsHepler.GetColumnAllLogs(obj, newObj);
        }

        
    }





}
