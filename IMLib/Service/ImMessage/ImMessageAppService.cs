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
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Authorization.Users;
using SearchAll;

namespace IMLib
{
    public class ImMessageAppService : FRMSCoreAppServiceBase, IImMessageAppService
    { 
        private readonly IRepository<ImMessage, Guid> _repository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private ImSearchAppService _imSearchAppService;
        public ImMessageAppService(IRepository<ImMessage, Guid> repository, IRepository<User, long> userRepository, IAbpFileRelationAppService abpFileRelationAppService, ImSearchAppService imSearchAppService
        )
        {
            this._repository = repository;
            _userRepository = userRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _imSearchAppService = imSearchAppService;
        }

        public async Task<List<ImSearchCount>> GetSearchList(GetImSearchInput input)
        {
           return await _imSearchAppService.GetSearchList(input);
        }
        public async Task<List<ImSearch>> GetListByIds(Guid[] input)
        {
            return await _imSearchAppService.GetListByIds(input);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<ImMessageListOutputDto>> GetList(GetImMessageListInput input)
        {
            //var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
            //			join b in _userRepository.GetAll() on a.UserId equals b.Id
            //                     select new ImMessageListOutputDto()
            //                     {
            //                         Id = a.Id,
            //                         To = a.To,
            //                         UserId = a.UserId,
            //                         UserName = b.Name,
            //                         CreationTime = a.CreationTime,
            //                         Type = a.Type,
            //                         Msg = a.Msg,
            //                         FileName = a.FileName,
            //                         RoomType = a.RoomType,
            //                         ChatType = a.ChatType

            //                     };
            //var toalCount = await query.CountAsync();
            //var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            var searchinput = new GetImSearchListInput()
            {
                MaxResultCount=input.MaxResultCount,
                SkipCount=input.SkipCount,
                SearchKey=input.SearchKey,
                To = input.To,
                Type = input.Type,
            };
            
           var imList= await _imSearchAppService.GetList(searchinput);
            var query = from a in imList.Items
                        join b in _userRepository.GetAll() on a.UserId equals b.Id
                        select new ImMessageListOutputDto()
                        {
                            Id = a.Id,
                            To = a.To,
                            UserId = a.UserId,
                            UserName = b.Name,
                            CreationTime = a.CreationTime,
                            Type = a.Type,
                            Msg = a.Msg,
                            FileName = a.FileName,
                            RoomType = a.RoomType,
                            ChatType = a.ChatType

                        };
            var ret = query.OrderByDescending(x=>x.CreationTime).ToList();
            foreach (var item in ret)
            {
               var file= _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = item.Id.ToString(), BusinessType = (int)AbpFileBusinessType.ImFile }).FirstOrDefault();
                if (file != null)
                {
                    item.FileName = file.FileName;
                    item.FileSize = file.FileSize;
                    item.FIleId = file.Id;
                }
            }
            return new PagedResultDto<ImMessageListOutputDto>(imList.TotalCount, ret);
        }
        /// <summary>
        /// 添加一个ImMessage
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateImMessageInput input)
        {
            var id = Guid.NewGuid();
            //var newmodel = new ImMessage()
            //{
            //    Id = id,
            //    To = input.To,
            //    Type = input.Type,
            //    Msg = input.Msg,
            //    UserId = AbpSession.UserId.Value,
            //    RoomType = input.RoomType,
            //    ChatType = input.ChatType
            //};
            var model = new ImSearchCreateInput()
            {
                Id = id,
                To = input.To,
                Type = input.Type,
                Msg = input.Msg,
                UserId = AbpSession.UserId.Value,
                RoomType = input.RoomType,
                ChatType = input.ChatType,
                CreationTime = DateTime.Now
            };
            await _imSearchAppService.Create(model);
            if (input.FileList != null)
            {
                // newmodel.FileName = input.FileList.FirstOrDefault() ?.FileName;
                model.FileName = input.FileList.FirstOrDefault()?.FileName;
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }

                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.ImFile,
                    Files = fileList
                });
            }
            //await _repository.InsertAsync(newmodel);

        }
        public async Task Delete(EntityDto<Guid> input)
        {
            await _imSearchAppService.Delete(input);
        }

    }
}