using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZCYX.FRMSCore.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 返回枚举中的对应的资源文件的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetLocalizedDescription(this object @enum)
        {
            if (@enum == null)
                return null;

            string description = @enum.ToString();

            FieldInfo fieldInfo = @enum.GetType().GetField(description);
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any())
                return attributes[0].Description;

            return description;
        }

        public static List<ExpandoObject> GetEnumList<T>(string value = null, string setEmpty = null)
            where T : new()
        {
            T mt = new T();
            var t = mt.GetType();
            var list = (from int i in Enum.GetValues(t)
                select
                    new 
                    {
                        Text = GetLocalizedDescription(Enum.ToObject(t, i)),
                        Value = i.ToString(),
                        Selected = value != null && (value == i.ToString())
                    }).ToList();
            var result = new List<ExpandoObject>();
            if (!string.IsNullOrEmpty(setEmpty))
            {
                dynamic def = new ExpandoObject();
                def.Text = setEmpty;
                def.Value = "";
                def.Selected = false;
                result.Add(def);
            }
            list.ForEach(x =>
            {
                dynamic def = new ExpandoObject();
                def.Text = x.Text;
                def.Value = x.Value;
                def.Selected = x.Selected;
                result.Add(def);
            });
            return result;
        }
    }
}
