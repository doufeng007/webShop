using Abp.Domain.Repositories;
using Abp.WorkFlow;
using Docment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using ZCYX.FRMSCore.Model;
using Project;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.Web.Host
{
    /// <summary>
    /// 创建归档请求
    /// </summary>
    public class DocmentEvent
    {
        /// <summary>
        /// 创建待归档记录
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string CreateDocment(WorkFlowCustomEventParams eventParams)
        {
            var docmentService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDocmentAppService>();
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var flow = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlow, Guid>>();
            var taskModel = repository.Get(eventParams.TaskID);
            var flowModel = flow.Get(eventParams.FlowID);
            docmentService.Create(new CreateDocmentInput()
            {
                Attr = DocmentAttr.纸质,
                Des = taskModel.Title,
                FlowId = new Guid("1734da80-9ddf-4bc3-8f1a-d85fa80868d5"),
                FlowTitle = flowModel.Name + "-归档请求",
                Name = taskModel.Title + "-档案待收",
               
                Type = new Guid("71300f49-4542-43c5-1b44-08d5bbbe5313"),
                UserId = taskModel.SenderID,

            });
            return "成功";
        }


        /// <summary>
        /// 项目评审-归档子流程激活事件
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public WorkFlowExecute CreateDocmentForProject(WorkFlowCustomEventParams eventParams)
        {
            var dmService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDispatchMessageAppService>();
            var data = dmService.CreateActive(eventParams.InstanceID,eventParams.NextRecevieUserId);
            var ret = new WorkFlowExecute();
            //ret.Title = "项目评审发文";
            ret.InstanceID = data.ToString();
            return ret;
        }

        /// <summary>
        /// 项目评审-归档完成后添加在档记录
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string DocmentForProject(WorkFlowCustomEventParams eventParams)
        {
            var docmentService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDocmentAppService>();
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlowTask, Guid>>();
            var flow = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<WorkFlow, Guid>>();
            var archive = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<ArchivesManager, Guid>>();
            var id = eventParams.InstanceID.ToGuid();
            var a = archive.Get(id);
            var taskModel = repository.Get(eventParams.TaskID);
            var flowModel = flow.Get(eventParams.FlowID);
            
            var docid= docmentService.Add(new CreateDocmentInput()
            {
                Attr = DocmentAttr.纸质,
                IsProject = true,
                ArchiveId = a.Id,
                Des = a.ArchivesName,
                FlowId = new Guid("1734da80-9ddf-4bc3-8f1a-d85fa80868d5"),
                FlowTitle = a.ArchivesName + "-资料归档",
                Name = a.ArchivesName ,
                Status= DocmentStatus.归档中,
                Location=a.Location,
                Type = new Guid("71300f49-4542-43c5-1b44-08d5bbbe5313"),
                UserId = taskModel.ReceiveID,
            });
            
            return "成功";
        }
        /// <summary>
        /// 项目评审-归档子流程激活事件
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public WorkFlowExecute CreateArchivesManagerForProject(WorkFlowCustomEventParams eventParams)
        {
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IArchivesManagerAppService>();
            var data = service.CreateArchivesManagerActive(eventParams.InstanceID);
            var ret = new WorkFlowExecute();
            ret.Title = "项目评审归档";
            ret.InstanceID = data.ToString();
            return ret;
        }

        /// <summary>
        /// 在发起移交、销毁、借阅流程后更新档案的状态
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string ChangeDocmentMoveStatus(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentMove, Guid>>();
            var move = repository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (move == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案移交数据");
            }
            var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
            var doc = drepository.FirstOrDefault(move.DocmentId);
            if (doc == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
            }
            doc.Status = (int)DocmentStatus.已移交;
            if (string.IsNullOrWhiteSpace(move.No) == false)
            {
                doc.No = move.No;
            }
            if (string.IsNullOrWhiteSpace(move.Location) == false)
            {
                doc.Location = move.Location;
            }
            drepository.Update(doc);
            return "成功";
        }
        public static string ChangeDocmentDestroyStatus(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentDestroy, Guid>>();
            var destroy = repository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (destroy == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案销毁数据");
            }
            var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
            var doc = drepository.FirstOrDefault(destroy.DocmentId);
            if (doc == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
            }
            doc.Status = (int)DocmentStatus.已销毁;
            drepository.Update(doc);
            return "成功";
        }
        ///// <summary>
        ///// 档案归还后重置状态(已作废，改为扫码后调用接口重置档案状态)
        ///// </summary>
        ///// <param name="eventParams"></param>
        ///// <returns></returns>
        //[Obsolete]
        //public static string ChangeDocmentBorrowStatus(WorkFlowCustomEventParams eventParams)
        //{
        //    var brepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentBorrow, Guid>>();
        //    var borrow = brepository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
        //    if (borrow == null)
        //    {
        //        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案借阅数据");
        //    }
        //    var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
        //    var doc = drepository.FirstOrDefault(borrow.DocmentId);
        //    if (doc == null)
        //    {
        //        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
        //    }
        //    doc.Status = (int)DocmentStatus.在档;
        //    drepository.Update(doc);
        //    return "成功";
        //}
        /// <summary>
        /// 待入档档案驳回申请后重置状态
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string ResetWartingDocmentStatus(WorkFlowCustomEventParams eventParams)
        {

            var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
            var doc = drepository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (doc == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
            }
            doc.Status = -2;
            drepository.Update(doc);
            return "成功";
        }
        ///// <summary>
        ///// 档案借阅-领导审核通过后，生成借阅验证码(接口作废，不在使用验证码)
        ///// </summary>
        ///// <param name="eventParams"></param>
        ///// <returns></returns>
        //[Obsolete]
        //public static bool SetVerifyForBorrow(WorkFlowCustomEventParams eventParams)
        //{
        //    var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentBorrow, Guid>>();
        //    var id = Guid.Parse(eventParams.InstanceID);
        //    var record = repository.Get(id);
        //    var verify = (new Random()).Next(1000, 9999).ToString();
        //    record.Verify = verify;
        //    repository.Update(record);//生成验证码

        //    //发送验证码给申请人【事务通知】
        //    var repository2 = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
        //    var dom = repository2.Get(record.DocmentId);
        //    var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectNoticeManager>();
        //    var noticeInput = new Application.NoticePublishInputForWorkSpaceInput();
        //    noticeInput.ProjectId = id;
        //    noticeInput.Content = $"你申请借阅《{dom.Name}》已通过审核，验证码：{verify}";
        //    noticeInput.Title = $"借阅《{dom.Name}》申请成功";
        //    noticeInput.NoticeUserIds = record.CreatorUserId.Value.ToString();
        //    noticeInput.NoticeType = 1;
        //    noticeService.CreateOrUpdateNoticeSync(noticeInput);
        //    return true;
        //}
        ///// <summary>
        ///// 档案管理员验证验证码后状态重置为借出
        ///// </summary>
        ///// <param name="eventParams"></param>
        ///// <returns></returns>
        //public static string ChangeDocmentBorrow(WorkFlowCustomEventParams eventParams)
        //{
        //    var brepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentBorrow, Guid>>();
        //    var borrow = brepository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
        //    if (borrow == null)
        //    {
        //        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案借阅数据");
        //    }
        //    var subrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentBorrowSub, Guid>>();
        //    var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
        //    var sub = subrepository.GetAll().Where(ite => ite.BorrowId == borrow.Id);
        //    if (sub != null)
        //    {
        //        foreach (var s in sub)
        //        {
        //            var doc = drepository.FirstOrDefault(s.DocmentId);
        //            if (doc == null)
        //            {
        //                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
        //            }
        //            if (doc.Attr == DocmentAttr.纸质)
        //            {
        //                doc.Status = (int)DocmentStatus.使用中;
        //            }
        //            drepository.Update(doc);
        //        }
        //    }
        //    return "成功";
        //}
        ///// <summary>
        ///// 判断是否纸质档案
        ///// </summary>
        ///// <param name="eventParams"></param>
        ///// <returns></returns>
        //public bool IsPaperDocment(WorkFlowCustomEventParams eventParams)
        //{
        //    var brepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentBorrow, Guid>>();
        //    var borrow = brepository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
        //    if (borrow == null)
        //    {
        //        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案借阅数据");
        //    }
        //    var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
        //    var doc = drepository.FirstOrDefault(borrow.DocmentId);
        //    if (doc == null)
        //    {
        //        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
        //    }
        //    if (doc.Attr == DocmentAttr.纸质)
        //    {
        //        return true;
        //    }
        //    return false;
        //}


        /// <summary>
        /// 档案销毁申请驳回后重置状态
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string ResetDocmentDestroyStatus(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentDestroy, Guid>>();
            var destroy = repository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (destroy == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案销毁数据");
            }
            var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
            var doc = drepository.FirstOrDefault(destroy.DocmentId);
            if (doc == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
            }
            doc.Status = (int)DocmentStatus.在档;
            drepository.Update(doc);
            return "成功";
        }

        /// <summary>
        /// 档案移交申请驳回后重置状态
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string ResetDocmentMoveStatus(WorkFlowCustomEventParams eventParams)
        {
            var repository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentMove, Guid>>();
            var move = repository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (move == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案移交数据");
            }
            var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
            var doc = drepository.FirstOrDefault(move.DocmentId);
            if (doc == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
            }
            doc.Status = (int)DocmentStatus.在档;
            return "成功";
        }

        /// <summary>
        /// 档案借阅申请驳回后重置状态
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public static string ResetDocmentBorrowStatus(WorkFlowCustomEventParams eventParams)
        {
            var brepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentBorrow, Guid>>();
            var borrow = brepository.FirstOrDefault(Guid.Parse(eventParams.InstanceID));
            if (borrow == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案借阅数据");
            }
            var subrepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentBorrowSub, Guid>>();
            var drepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<DocmentList, Guid>>();
            var sub = subrepository.GetAll().Where(ite => ite.BorrowId == borrow.Id);
            if (sub != null) {
                foreach (var s in sub) {
                    s.Status = BorrowSubStatus.驳回;
                    var doc = drepository.FirstOrDefault(s.DocmentId);
                    if (doc == null)
                    {
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到档案数据");
                    }
                    doc.Status = (int)DocmentStatus.在档;
                }
            }
            return "成功";
        }
        /// <summary>
        /// 发起人是否领导
        /// </summary>
        /// <param name="eventParams"></param>
        /// <returns></returns>
        public bool IsLeader(WorkFlowCustomEventParams eventParams) {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDocmentAppService>();
            return _service.IsLeader();
        }
        public bool IsNotLeader(WorkFlowCustomEventParams eventParams)
        {
            var _service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IDocmentAppService>();
            return !_service.IsLeader();
        }
    }
}
