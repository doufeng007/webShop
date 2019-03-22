using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Extensions
{
    public static class ModelColumnLogsHepler
    {
        /// <summary>
        /// 构建修改日志-只记录两个实体修改后的部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">老数据实体</param>
        /// <param name="n">新数据实体</param>
        /// <returns></returns>
        public static List<LogColumnModel> GetColumnLogs<T>(T o, T n)
        {
            Type type = typeof(T);
            var list = new List<LogColumnModel>();
            foreach (var f in type.GetProperties())
            {
                var attr = f.GetCustomAttributes(typeof(LogColumnAttribute), true);
                if (attr.Length > 0)
                {
                    var colattr = (LogColumnAttribute)attr[0];
                    if (colattr.IsLog)
                    {
                        var oldValue = f.GetValue(o);
                        var newValue = f.GetValue(n);
                        if (oldValue == null) oldValue = "空";
                        if (newValue == null) newValue = "空";


                        if (oldValue.ToString() != newValue.ToString())
                        {
                            if (oldValue.ToString().IndexOf(".0000", StringComparison.Ordinal) > 0)
                            {
                                var oldValueNumber = 0m;
                                var newValueNumber = 0m;
                                if (decimal.TryParse(oldValue.ToString(), out oldValueNumber) &&
                                    decimal.TryParse(newValue.ToString(), out newValueNumber))
                                {
                                    if (oldValueNumber == newValueNumber)
                                    {
                                        continue;
                                    }
                                }
                            }
                            var field = string.IsNullOrWhiteSpace(colattr.Name) ? f.Name : colattr.Name;
                            list.Add(new LogColumnModel
                            {
                                FieldName = field,
                                OldValue = oldValue.ToString(),
                                NewValue = newValue.ToString()
                            });
                        }
                    }
                }
            }
            return list;

        }



        public static List<LogColumnModel> GetColumnAllLogs(object o, object n)
        {
            var logs = new List<LogColumnModel>();
            foreach (var f in o.GetType().GetProperties())
            {
                var attr = f.GetCustomAttributes(typeof(LogColumnAttribute), true);
                if (attr.Length > 0)
                {
                    var colattr = (LogColumnAttribute)attr[0];
                    if (colattr.IsLog)
                    {

                        if (f.PropertyType.GetInterface("IEnumerable", false) != null && f.PropertyType.GetInterface("IList", false) != null)
                        {
                            var childType = f.PropertyType.GetGenericArguments()[0];
                            var childValues = f.GetValue(o);
                            var newChilds = f.GetValue(n) as IEnumerable;
                            foreach (var item in (childValues as IEnumerable))
                            {
                                var old_Id = item.GetType().GetProperty("Id").GetValue(item, null);
                                var nameValue = GetNameFieldValue(item);
                                var hasSameId = false;
                                object newEditObj = null;
                                foreach (var itemNew in newChilds)
                                {
                                    var new_Id = itemNew.GetType().GetProperty("Id").GetValue(itemNew, null);
                                    if (new_Id == null) continue;
                                    ///编辑的
                                    if (new_Id.ToString() == old_Id.ToString())
                                    {
                                        hasSameId = true;
                                        newEditObj = itemNew;
                                        break;

                                    }
                                    ///删除的
                                    else
                                    {
                                        continue;

                                    }
                                }
                                if (hasSameId)
                                {
                                    var editlogs = GetColumnAllLogs(item, newEditObj);
                                    foreach (var log in editlogs)
                                    {
                                        log.FieldName = $"{colattr.Name}:{nameValue}->{log.FieldName}";
                                        //log.ChangeType = 2;
                                        logs.Add(log);
                                    }
                                }
                                else
                                {
                                    var deletelog = new LogColumnModel()
                                    {
                                        ChangeType = 3,
                                        FieldName = $"{colattr.Name}:{nameValue}",
                                        NewValue = "",
                                        OldValue = old_Id.ToString(),
                                    };
                                    logs.Add(deletelog);
                                }
                            }
                            foreach (var itemNew in newChilds)
                            {
                                var new_Id = itemNew.GetType().GetProperty("Id").GetValue(itemNew, null);
                                var nameValue = GetNameFieldValue(itemNew);

                                if (new_Id == null)
                                {
                                    var addlog = new LogColumnModel()
                                    {
                                        ChangeType = 1,
                                        FieldName = $"{colattr.Name}:{nameValue}",
                                        //NewValue = new_Id.ToString(),
                                        NewValue = "",
                                        OldValue = "",
                                    };
                                    logs.Add(addlog);
                                }
                                else
                                {
                                    var hasSameId = false;
                                    foreach (var item in (childValues as IEnumerable))
                                    {
                                        var old_Id = item.GetType().GetProperty("Id").GetValue(item, null);
                                        if (new_Id.ToString() == old_Id.ToString())
                                        {
                                            hasSameId = true;
                                            break;
                                        }
                                    }
                                    if (!hasSameId)
                                    {
                                        var addlog = new LogColumnModel()
                                        {
                                            ChangeType = 1,
                                            FieldName = $"{colattr.Name}:{nameValue}",
                                            NewValue = new_Id.ToString(),
                                            //FieldName = $"{colattr.Name}",
                                            //NewValue = nameValue,
                                            OldValue = "",
                                        };
                                        logs.Add(addlog);
                                    }

                                }


                            }


                        }
                        else
                        {
                            var oldValue = f.GetValue(o);

                            var newValue = f.GetValue(n);
                            if (oldValue == null) oldValue = "空";
                            if (newValue == null) newValue = "空";


                            if (oldValue.ToString() != newValue.ToString())
                            {

                                //if (oldValue.ToString().IndexOf(".0000", StringComparison.Ordinal) > 0)
                                if (f.PropertyType == typeof(decimal) || f.PropertyType == typeof(decimal?))
                                {
                                    var oldValueNumber = 0m;
                                    var newValueNumber = 0m;
                                    if (decimal.TryParse(oldValue.ToString(), out oldValueNumber) &&
                                        decimal.TryParse(newValue.ToString(), out newValueNumber))
                                    {
                                        if (oldValueNumber == newValueNumber)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (f.PropertyType == typeof(bool) || f.PropertyType == typeof(bool?))
                                {
                                    var oldValueBoolen = oldValue.ToString() == "空" ? "否" : bool.Parse(oldValue.ToString()) ? "是" : "否";
                                    var newValueBoolen = newValue.ToString() == "空" ? "否" : bool.Parse(newValue.ToString()) ? "是" : "否";
                                    if (oldValueBoolen == newValueBoolen)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        oldValue = oldValueBoolen;
                                        newValue = newValueBoolen;
                                    }
                                }
                                var field = string.IsNullOrWhiteSpace(colattr.Name) ? f.Name : colattr.Name;
                                logs.Add(new LogColumnModel
                                {
                                    FieldName = field,
                                    OldValue = oldValue.ToString(),
                                    NewValue = newValue.ToString()
                                });
                            }
                        }
                    }
                }
            }
            return logs;

        }


        public static string GetNameFieldValue(object value)
        {
            var properties = value.GetType().GetProperties();
            foreach (var p in properties)
            {
                var childattr = p.GetCustomAttributes(typeof(LogColumnAttribute), true);
                if (childattr.Length > 0)
                {
                    var childCol = (LogColumnAttribute)childattr[0];
                    if (childCol.IsNameField)
                    {
                        return p.GetValue(value)?.ToString();
                    }
                }
            }
            return "";
        }


        //public static Dictionary<string, string> MakeContent(List<ProjectAudit> source)
        //{
        //    var ret = new Dictionary<string, string>();
        //    foreach (var log in source)
        //    {
        //        if (log.ChangeType.HasValue)
        //        {
        //            if (log.ChangeType == 1)
        //            {
        //                ret.Add($"新增", $"{log.FieldName}");
        //            }
        //            else if (log.ChangeType == 2)
        //            {
        //                ret.Add($"将{log.FieldName}", $"由{log.OldValue} 改为{log.NewValue}");
        //            }
        //            else
        //            {
        //                ret.Add($"将{log.FieldName}", "删除");
        //            }

        //        }
        //        else
        //        {
        //            ret.Add($"将{log.FieldName}", $"由{log.OldValue} 改为{log.NewValue}");
        //        }
        //    }
        //    return ret;
        //}

    }
}
