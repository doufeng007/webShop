using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using Abp;
using SearchAll;

namespace IMLib
{
    public class IM_InquiryAppService : FRMSCoreAppServiceBase, IIM_InquiryAppService
    {
        private readonly IRepository<IM_Inquiry, Guid> _repository;
        private readonly IRepository<IM_InquiryResult, Guid> _resultRepository;
        private readonly IRepository<WorkFlowTask, Guid> _workflowTaskRepository;
        private readonly IRepository<ImMessage, Guid> _messageRepository;
        private readonly ImMessageAppService _imSearchAppService;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        public IM_InquiryAppService(IRepository<IM_Inquiry, Guid> repository, IRepository<IM_InquiryResult, Guid> resultRepository, IRepository<WorkFlowTask, Guid> workflowTaskRepository
            , IRepository<ImMessage, Guid> messageRepository, ImMessageAppService imSearchAppService, IAbpFileRelationAppService abpFileRelationAppService)
        {
            this._repository = repository;
            _resultRepository = resultRepository;
            _workflowTaskRepository = workflowTaskRepository;
            _messageRepository = messageRepository;
            _imSearchAppService = imSearchAppService;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<List<InquiryOutput>> GetList(GetIM_InquiryListInput input)
        {
            var ret = new List<InquiryOutput>();
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowWorkTaskAppService>();
            var todoInquirys = await service.GetFlowInquiryList(new GetFlowInquiryInput() { FlowId = input.FlowId, TaskId = input.TaskId, MaxResultCount = 1000, SkipCount = 0 });
            foreach (var item in todoInquirys.Items)
            {
                ret.Add(new InquiryOutput()
                {
                    Index = 0,
                    Inquiry = "",
                    InquiryType = InquiryType.待办征询,
                    ReceiveTime = item.ReceiveTime,
                    SenderName = item.SenderName,
                    Tasks = item.Tasks
                });
            }






            var query_t = from a in _resultRepository.GetAll()
                          join b in _repository.GetAll() on a.InquiryId equals b.Id
                          join relpyUser in UserManager.Users on a.ReplyUserId equals relpyUser.Id into g
                          from relpyUser in g.DefaultIfEmpty()
                          join u in UserManager.Users on b.CreatorUserId equals u.Id
                          where b.TaskId == input.TaskId
                          select new { a, relpyUser, b, u };

            var source = query_t.OrderBy(r => r.a.CreationTime).Take(3).ToList();
            var needEsList = source.Where(r => !r.a.ReplyUserId.HasValue).ToList();
            var notNeedEsList = source.Where(r => r.a.ReplyUserId.HasValue);
            var dataNotNeedEs = notNeedEsList.Select(item => new InquiryResultOutput()
            {
                Id = item.a.Id,
                Comment = item.a.ReplyContent,
                CompletedTime1 = item.a.ReplyDateTime.Value,
                InquiryId = item.b.Id,
                MessageType = item.a.MeaageType,
                ReceiveName = item.relpyUser.Name,
                ReceiveTime = item.b.CreationTime,
                SenderName = item.u.Name,
            }).ToList();
            var dataNeedEs = new List<InquiryResultOutput>();
            if (needEsList.Count() > 0)
            {

                var imList = await _imSearchAppService.GetListByIds(needEsList.Select(r => r.a.MessageId).ToArray());
                var query_2 = from a in imList
                              join b in needEsList on a.Id equals b.a.MessageId
                              join c in UserManager.Users on a.UserId equals c.Id
                              select new InquiryResultOutput
                              {
                                  Id = b.a.Id,
                                  Comment = a.Msg,
                                  CompletedTime1 = a.CreationTime,
                                  InquiryId = b.b.Id,
                                  MessageType = a.Type,
                                  ReceiveName = c.Name,
                                  ReceiveTime = b.b.CreationTime,
                                  SenderName = b.u.Name,
                                  ReplyUserId = a.UserId
                              };
                dataNeedEs = query_2.ToList();

                dataNotNeedEs.AddRange(dataNeedEs);
            }

            var im_Inquirys = dataNotNeedEs.GroupBy(r => new { r.InquiryId, r.ReceiveTime, r.SenderName }).Select(p => new InquiryOutput
            {
                Index = 0,
                Id = p.Key.InquiryId,
                Inquiry = "",
                InquiryType = InquiryType.讨论组征询,
                ReceiveTime = p.Key.ReceiveTime,
                SenderName = p.Key.SenderName,
                Tasks = p.Select(m => new FlowInquiryTask() { Id = m.Id, Comment = m.Comment, CompletedTime1 = m.CompletedTime1, ReceiveName = m.ReceiveName }).OrderBy(r => r.CompletedTime1).Take(3).ToList()
            }).ToList();

            foreach (var item in im_Inquirys)
            {
                ret.Add(item);
            }
            ret = ret.OrderBy(r => r.ReceiveTime).ToList();

            foreach (var item in ret)
            {
                if (item.InquiryType == InquiryType.待办征询)
                {
                    foreach (var task in item.Tasks)
                    {
                        task.Files = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = task.Id.ToString(), BusinessType = (int)AbpFileBusinessType.流程意见 });
                    }
                }
                else
                {
                    foreach (var task in item.Tasks)
                    {
                        task.Files = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = task.Id.ToString(), BusinessType = (int)AbpFileBusinessType.ImFile });
                    }
                }
            }
            Task.Run(() =>
            {

                foreach (var item in dataNeedEs)
                {
                    var entity = _resultRepository.Get(item.Id);
                    entity.ReplyContent = item.Comment;
                    entity.ReplyDateTime = item.CompletedTime1;
                    entity.ReplyUserId = item.ReplyUserId;
                    entity.MeaageType = item.MessageType;
                    _resultRepository.Update(entity);
                }
            });
            return ret;
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<InquiryOutput> Get(EntityDto<Guid> input)
        {
            var query_t = from a in _resultRepository.GetAll()
                          join b in _repository.GetAll() on a.InquiryId equals b.Id
                          join relpyUser in UserManager.Users on a.ReplyUserId equals relpyUser.Id into g
                          from relpyUser in g.DefaultIfEmpty()
                          join u in UserManager.Users on b.CreatorUserId equals u.Id
                          where b.Id == input.Id
                          select new { a, relpyUser, b, u };

            var source = query_t.OrderBy(r => r.a.CreationTime).ToList();
            var needEsList = source.Where(r => !r.a.ReplyUserId.HasValue).ToList();
            var notNeedEsList = source.Where(r => r.a.ReplyUserId.HasValue);
            var dataNotNeedEs = notNeedEsList.Select(r => new InquiryResultOutput()
            {
                Id = r.a.Id,
                Comment = r.a.ReplyContent,
                CompletedTime1 = r.a.ReplyDateTime.Value,
                InquiryId = r.b.Id,
                MessageType = r.a.MeaageType,
                ReceiveName = r.relpyUser.Name,
                ReceiveTime = r.b.CreationTime,
                SenderName = r.u.Name,
            }).ToList();
            var dataNeedEs = new List<InquiryResultOutput>();
            if (needEsList.Count() > 0)
            {
                var imList = await _imSearchAppService.GetListByIds(needEsList.Select(r => r.a.MessageId).ToArray());
                var query_2 = from a in imList
                              join b in needEsList on a.Id equals b.a.MessageId
                              join c in UserManager.Users on a.UserId equals c.Id
                              select new InquiryResultOutput
                              {
                                  Id = b.a.Id,
                                  Comment = a.Msg,
                                  CompletedTime1 = a.CreationTime,
                                  InquiryId = b.b.Id,
                                  MessageType = a.Type,
                                  ReceiveName = c.Name,
                                  ReceiveTime = b.b.CreationTime,
                                  SenderName = b.u.Name,
                                  ReplyUserId = a.UserId
                              };
                dataNeedEs = query_2.ToList();

                dataNotNeedEs.AddRange(dataNeedEs);
            }

            var ret = new InquiryOutput();
            var item = dataNotNeedEs.FirstOrDefault();
            ret.Id = item.InquiryId;
            ret.InquiryType = InquiryType.讨论组征询;
            ret.ReceiveTime = item.ReceiveTime;
            ret.SenderName = item.SenderName;
            ret.Tasks = dataNotNeedEs.Select(m => new FlowInquiryTask() { Comment = m.Comment, CompletedTime1 = m.CompletedTime1, ReceiveName = m.ReceiveName }).OrderBy(r => r.CompletedTime1).ToList();

            foreach (var task in ret.Tasks)
            {
                task.Files = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = task.Id.ToString(), BusinessType = (int)AbpFileBusinessType.ImFile });
            }


            Task.Run(() =>
            {
                foreach (var r in dataNeedEs)
                {
                    var entity = _resultRepository.Get(r.Id);
                    entity.ReplyContent = r.Comment;
                    entity.ReplyDateTime = r.CompletedTime1;
                    entity.ReplyUserId = r.ReplyUserId;
                    entity.MeaageType = r.MessageType;
                }
            });

            return ret;
        }


        /// <summary>
        /// 添加一个IM_Inquiry
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateIM_InquiryInput input)
        {
            var newmodel = new IM_Inquiry()
            {
                Id = Guid.NewGuid(),
                IM_GroupId = input.IM_GroupId,
                IM_GroupName = input.IM_GroupName,
                TaskId = input.TaskId
            };

            //foreach (var item in ret)
            //{
            //    var file = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.ImFile }).FirstOrDefault();
            //    if (file != null)
            //    {
            //        item.FileName = file.FileName;
            //        item.FileSize = file.FileSize;
            //        item.FIleId = file.Id;
            //    }
            //}


            foreach (var item in input.MessageIds)
            {
                var entity = new IM_InquiryResult()
                {
                    Id = Guid.NewGuid(),
                    IM_GroupId = input.IM_GroupId,
                    InquiryId = newmodel.Id,
                    ReplyContent = "",
                    ReplyUserId = null,
                    MessageId = item
                };
                await _resultRepository.InsertAsync(entity);
            }
            await _repository.InsertAsync(newmodel);

        }


    }
}