using Abp.Extensions;
using Abp.WorkFlowDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace Abp.WorkFlow
{
    public class WorkFlowHelper
    {
        public WorkFlowInstalled GetWorkFlowRunModel(string jsonString, out string errMsg)
        {
            errMsg = "";
            var wfInstalled = new WorkFlowInstalled();
            var json = LitJson.JsonMapper.ToObject(jsonString);

            #region 载入基本信息
            string id = json["id"].ToString();
            if (!id.IsGuid())
            {
                errMsg = "流程ID错误";
                return null;
            }
            else
            {
                wfInstalled.ID = id.ToGuid();
            }

            string name = json["name"].ToString();
            if (name.IsNullOrEmpty())
            {
                errMsg = "流程名称为空";
                return null;
            }
            else
            {
                wfInstalled.Name = name.Trim();
            }

            string type = json["type"].ToString();
            var dicManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowDictionaryManager>();
            wfInstalled.Type = type.IsNullOrEmpty() ? dicManager.GetIDByCode("FlowTypes").ToString() : type.Trim();

            string manager = json["manager"].ToString();
            if (manager.IsNullOrEmpty())
            {
                errMsg = "流程管理者为空";
                return null;
            }
            else
            {
                wfInstalled.Manager = manager;
            }

            string instanceManager = json["instanceManager"].ToString();
            if (instanceManager.IsNullOrEmpty())
            {
                errMsg = "流程实例管理者为空";
                return null;
            }
            else
            {
                wfInstalled.InstanceManager = instanceManager;
            }
            var workFlowOrganizationUnitsManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            wfInstalled.RemoveCompleted = json["removeCompleted"].ToString().ToInt();
            wfInstalled.Debug = json["debug"].ToString().ToInt();
            wfInstalled.DebugUsers = workFlowOrganizationUnitsManager.GetAllUsers(json["debugUsers"].ToString());
            wfInstalled.Note = json["note"].ToString();
            wfInstalled.FlowType = json.ContainsKey("flowType") ? json["flowType"].ToString().ToInt() : 0;

            var dataBases = new List<WorkFlowDataBases>();
            var dbs = json["databases"];
            if (dbs.IsArray)
            {
                foreach (LitJson.JsonData db in dbs)
                {
                    dataBases.Add(new WorkFlowDataBases()
                    {
                        LinkID = db["link"].ToString().ToGuid(),
                        LinkName = db["linkName"].ToString(),
                        Table = db["table"].ToString(),
                        PrimaryKey = db["primaryKey"].ToString()
                    });
                }
            }
            wfInstalled.DataBases = dataBases;

            var titleField = json["titleField"];
            if (titleField.IsObject)
            {
                wfInstalled.TitleField = new WorkFlowTitleField()
                {
                    Field = titleField["field"].ToString(),
                    LinkID = titleField["link"].ToString().ToGuid(),
                    LinkName = "",
                    Table = titleField["table"].ToString()
                };
            }
            #endregion

            #region 载入步骤信息
            var stepsList = new List<WorkFlowStep>();
            LitJson.JsonData steps = json["steps"];
            if (steps.IsArray)
            {
                foreach (LitJson.JsonData step in steps)
                {
                    #region 行为
                    LitJson.JsonData behavior = step["behavior"];
                    var behavior1 = new WorkFlowBehavior();
                    if (behavior.IsObject)
                    {
                        behavior1.BackModel = behavior["backModel"].ToString().ToInt();
                        behavior1.BackStepID = behavior["backStep"].ToString().ToGuid();
                        behavior1.BackType = behavior["backType"].ToString().ToInt();
                        behavior1.DefaultHandler = behavior["defaultHandler"].ToString();
                        behavior1.FlowType = behavior["flowType"].ToString().ToInt();
                        behavior1.HandlerStepID = behavior["handlerStep"].ToString().ToGuid();
                        behavior1.HandlerType = behavior["handlerType"].ToString().ToInt();
                        behavior1.HanlderModel = behavior["hanlderModel"].ToString().ToInt(3);
                        behavior1.Percentage = behavior["percentage"].ToString().IsDecimal() ? behavior["percentage"].ToString().ToDecimal() : decimal.MinusOne;
                        behavior1.RunSelect = behavior["runSelect"].ToString().ToInt();
                        behavior1.SelectRange = behavior["selectRange"].ToString();
                        behavior1.ValueField = behavior["valueField"].ToString();
                        behavior1.Countersignature = behavior.ContainsKey("countersignature") ? behavior["countersignature"].ToString().ToInt() : 0;
                        behavior1.CountersignaturePercentage = behavior.ContainsKey("countersignaturePercentage") ? behavior["countersignaturePercentage"].ToString().ToDecimal() : decimal.MinusOne;
                        behavior1.SubFlowStrategy = behavior.ContainsKey("subflowstrategy") ? behavior["subflowstrategy"].ToString().ToInt() : int.MinValue;
                        behavior1.CopyFor = behavior.ContainsKey("copyFor") ? behavior["copyFor"].ToString() : "";
                        behavior1.ConcurrentModel = behavior.ContainsKey("concurrentModel") ? behavior["concurrentModel"].ToString().ToInt(0) : 0;
                        behavior1.CustomEvent = behavior.ContainsKey("customEvent") ? behavior["customEvent"].ToString() : "";
                    }
                    #endregion

                    #region 事件
                    LitJson.JsonData event1 = step["event"];
                    var  event2 = new WorkFlowEvent();
                    if (event1.IsObject)
                    {
                        event2.BackAfter = event1["backAfter"].ToString();
                        event2.BackBefore = event1["backBefore"].ToString();
                        event2.SubmitAfter = event1["submitAfter"].ToString();
                        event2.SubmitBefore = event1["submitBefore"].ToString();
                        event2.SubFlowActivationBefore = event1.ContainsKey("subflowActivationBefore") ? event1["subflowActivationBefore"].ToString() : "";
                        event2.SubFlowCompletedBefore = event1.ContainsKey("subflowCompletedBefore") ? event1["subflowCompletedBefore"].ToString() : "";
                    }
                    #endregion
                    #region 表单
                    LitJson.JsonData forms = step["forms"];
                    var formList = new List<WFForm>();
                    if (forms.IsArray)
                    {
                        foreach (LitJson.JsonData form in forms)
                        {
                            formList.Add(new WFForm()
                            {
                                ID = form["id"].ToString().ToGuid(),
                                Name = form["name"].ToString(),
                                Sort = form["srot"].ToString().ToInt()
                            });
                        }
                    }
                    if (formList.Count == 0)
                    {
                        //errMsg = string.Format("步骤[{0}]未设置表单", step["name"].ToString());
                        //return null;
                    }
                    #endregion
                    #region 字段状态
                    LitJson.JsonData fieldStatus = step["fieldStatus"];
                    var fieldStatusList = new List<FieldStatus>();
                    if (fieldStatus.IsArray)
                    {
                        foreach (LitJson.JsonData field in fieldStatus)
                        {
                            fieldStatusList.Add(new FieldStatus()
                            {
                                Check = field["check"].ToString().ToInt(),
                                Field = field["field"].ToString(),
                                Status1 = field["status"].ToString().ToInt()
                            });
                        }
                    }
                    #endregion
                    #region 坐标/基本信息
                    LitJson.JsonData position = step["position"];
                    decimal x = 0, y = 0;
                    if (position.IsObject)
                    {
                        x = position["x"].ToString().ToDecimal();
                        y = position["y"].ToString().ToDecimal();
                    }

                    stepsList.Add(new WorkFlowStep()
                    {
                        Archives = step["archives"].ToString().ToInt(),
                        ArchivesParams = step["archivesParams"].ToString(),
                        TodoType = step["todoType"].ToString().ToInt(),
                        Behavior = behavior1,
                        //Buttons = buttionList,
                        DataSaveType = step.ContainsKey("dataSaveType") ? step["dataSaveType"].ToString().ToInt(0) : 0,
                        DataSaveTypeWhere = step.ContainsKey("dataSaveTypeWhere") ? step["dataSaveTypeWhere"].ToString() : "",
                        Event = event2,
                        ExpiredPrompt = step["expiredPrompt"].ToString().ToInt(),
                        Forms = formList,
                        FieldStatus = fieldStatusList,
                        ID = step["id"].ToString().ToGuid(),
                        Type = step.ContainsKey("type") ? step["type"].ToString() : "normal",
                        LimitTime = step["limitTime"].ToString().ToDecimal(),
                        Name = step["name"].ToString(),
                        Note = step["note"].ToString(),
                        OpinionDisplay = step["opinionDisplay"].ToString().ToInt(),
                        OtherTime = step["otherTime"].ToString().ToDecimal(),
                        SignatureType = step["signatureType"].ToString().ToInt(),
                        WorkTime = step["workTime"].ToString().ToDecimal(),
                        SubFlowID = step.ContainsKey("subflow") ? step["subflow"].ToString() : "",
                        SubFlowTaskType = step.ContainsKey("subflowTaskType") ? step["subflowTaskType"].ToString().ToInt(0) : 0,
                        Position_x = x,
                        Position_y = y,
                        SendShowMsg = step.ContainsKey("sendShowMsg") ? step["sendShowMsg"].ToString() : "",
                        BackShowMsg = step.ContainsKey("backShowMsg") ? step["backShowMsg"].ToString() : ""
                    });
                    #endregion

                }
            }

            if (1 == wfInstalled.FlowType)
            {

            }

            wfInstalled.Steps = stepsList;
            if (stepsList.Count == 0)
            {
                errMsg = "流程至少需要一个步骤";
                return null;
            }
            #endregion

            #region 载入连线信息

            var linesList = new List<WorkFlowLine>();
            LitJson.JsonData lines = json.ContainsKey("lines") ? json["lines"] : null;
            if (lines != null && lines.IsArray)
            {
                foreach (LitJson.JsonData line in lines)
                {
                    linesList.Add(new WorkFlowLine()
                    {
                        ID = line["id"].ToString().ToGuid(),
                        FromID = line["from"].ToString().ToGuid(),
                        ToID = line["to"].ToString().ToGuid(),
                        CustomMethod = line["customMethod"].ToString(),
                        SqlWhere = line["sql"].ToString(),
                        NoAccordMsg = line.ContainsKey("noaccordMsg") ? line["noaccordMsg"].ToString() : "",
                        Organize = line.ContainsKey("organize") ? line["organize"].ToJson() : ""
                        /*
                        Organize_SenderIn = line.ContainsKey("organize_senderin") ? line["organize_senderin"].ToString() : "",
                        Organize_SenderNotIn = line.ContainsKey("organize_sendernotin") ? line["organize_sendernotin"].ToString() : "",
                        Organize_SponsorIn = line.ContainsKey("organize_sponsorin") ? line["organize_sponsorin"].ToString() : "",
                        Organize_SponsorNotIn = line.ContainsKey("organize_sponsornotin") ? line["organize_sponsornotin"].ToString() : "",
                        Organize_SenderLeader = line.ContainsKey("organize_senderleader") ? line["organize_senderleader"].ToString() : "",
                        Organize_SenderChargeLeader = line.ContainsKey("organize_senderchargeleader") ? line["organize_senderchargeleader"].ToString() : "",
                        Organize_SponsorLeader = line.ContainsKey("organize_sponsorleader") ? line["organize_sponsorleader"].ToString() : "",
                        Organize_SponsorChargeLeader = line.ContainsKey("organize_sponsorchargeleader") ? line["organize_sponsorchargeleader"].ToString() : "",
                        Organize_NotSenderLeader = line.ContainsKey("organize_notsenderleader") ? line["organize_notsenderleader"].ToString() : "",
                        Organize_NotSenderChargeLeader = line.ContainsKey("organize_notsenderchargeleader") ? line["organize_notsenderchargeleader"].ToString() : "",
                        Organize_NotSponsorLeader = line.ContainsKey("organize_notsponsorleader") ? line["organize_notsponsorleader"].ToString() : "",
                        Organize_NotSponsorChargeLeader = line.ContainsKey("organize_notsponsorchargeleader") ? line["organize_notsponsorchargeleader"].ToString() : ""
                         */
                    });
                }
            }

            wfInstalled.Lines = linesList;

            #endregion

            #region 载入其它信息
            //得到第一步
            List<Guid> firstStepIDList = new List<Guid>();
            foreach (var step in wfInstalled.Steps)
            {
                if (wfInstalled.Lines.Where(p => p.ToID == step.ID).Count() == 0)
                {
                    firstStepIDList.Add(step.ID);
                }
            }
            if (firstStepIDList.Count == 0)
            {
                errMsg = "流程没有开始步骤";
                return null;
            }
            /*
            else if (firstStepIDList.Count > 1)
            {
                errMsg = "流程有多个开始步骤";
                return null;
            }

            Guid lastStepID = Guid.Empty;
            foreach (var step in wfInstalled.Steps)
            {
                if (wfInstalled.Lines.Where(p => p.FromID == step.ID).Count() == 0)
                {
                    lastStepID = step.ID;
                    break;
                }
            }
            if (lastStepID == Guid.Empty)
            {
                errMsg = "流程没有结束步骤";
                return null;
            }
            */
            //var wf = dataWorkFlow.Get(wfInstalled.ID);
            //if (wf != null)
            //{
            //    wfInstalled.CreateTime = wf.CreateDate;
            //    wfInstalled.CreateUser = wf.CreateUserID.ToString();
            //    wfInstalled.DesignJSON = wf.DesignJSON;
            //    wfInstalled.FirstStepID = firstStepIDList.First();
            //    wfInstalled.InstallTime = RoadFlow.Utility.DateTimeNew.Now;
            //    wfInstalled.InstallUser = Platform.Users.CurrentUserID.ToString();
            //    wfInstalled.RunJSON = jsonString;
            //    wfInstalled.Status = wf.Status;
            //}
            #endregion

            return wfInstalled;
        }
    }
}
