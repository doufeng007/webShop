using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Diagnostics;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Newtonsoft.Json.Linq;
using Abp.Application.Services;
using System.Linq.Dynamic.Core;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Model;

namespace Abp.File
{
    public class AbpFileRelationAppService : ApplicationService, IAbpFileRelationAppService
    {
        private readonly IRepository<AbpFileRelation, Guid> _abpFileRelationRepository;
        private readonly IRepository<AbpFile, Guid> _abpFileRepository;

        public AbpFileRelationAppService(IRepository<AbpFileRelation, Guid> abpFileRelationRepository,
            IRepository<AbpFile, Guid> abpFileRepository)
        {
            _abpFileRelationRepository = abpFileRelationRepository;
            _abpFileRepository = abpFileRepository;
        }


        public async Task<List<GetAbpFilesOutput>> GetListAsync(GetAbpFilesInput input)
        {
            var query = from a in _abpFileRepository.GetAll()
                        join b in _abpFileRelationRepository.GetAll() on a.Id equals b.FileId
                        where b.BusinessId == input.BusinessId && b.BusinessType == input.BusinessType
                        && !b.IsDeleted
                        orderby b.CreationTime
                        orderby a.FileName
                        select new { File = a, Sort = b.Sort };
            var ret = new List<GetAbpFilesOutput>();
            var data = await query.ToListAsync();
            foreach (var item in data)
            {
                var entity = new GetAbpFilesOutput()
                {
                    FileName = item.File.FileName,
                    FileSize = item.File.FileSize,
                    Id = item.File.Id,
                    Sort = item.Sort
                };
                ret.Add(entity);
            }
            return ret;
        }


        public List<GetAbpFilesOutput> GetList(GetAbpFilesInput input)
        {
            var query = from a in _abpFileRepository.GetAll()
                        join b in _abpFileRelationRepository.GetAll() on a.Id equals b.FileId
                        where b.BusinessId == input.BusinessId && b.BusinessType == input.BusinessType
                        && !b.IsDeleted
                        orderby a.CreationTime descending
                        orderby a.FileName
                        select new { File = a, Sort = b.Sort };
            var ret = new List<GetAbpFilesOutput>();
            foreach (var item in query)
            {
                var entity = new GetAbpFilesOutput()
                {
                    FileName = item.File.FileName,
                    FileSize = item.File.FileSize,
                    Id = item.File.Id,
                    Sort = item.Sort
                };
                ret.Add(entity);
            }
            return ret;
        }


        public async Task<List<GetMultiAbpFilesOutput>> GetMultiListAsync(GetMultiAbpFilesInput input)
        {
            var businessType = (int)input.BusinessType;
            if (input.BusinessIds == null || input.BusinessIds.Count == 0)
                throw new UserFriendlyException(0, "业务参数错误");
            var query = from a in _abpFileRepository.GetAll()
                        join b in _abpFileRelationRepository.GetAll() on a.Id equals b.FileId
                        where input.BusinessIds.Contains(b.BusinessId) && b.BusinessType == businessType
                        && !b.IsDeleted
                        orderby b.CreationTime
                        orderby a.FileName
                        select new { File = a, Sort = b.Sort, BusinessId = b.BusinessId };
            var ret = new List<GetMultiAbpFilesOutput>();
            var data = await query.ToListAsync();
            var dataGroup = data.GroupBy(r => r.BusinessId).ToList();
            foreach (var item in dataGroup)
            {
                var entity = new GetMultiAbpFilesOutput()
                {
                    BusinessId = item.Key,
                    Files = item.Select(r => new GetAbpFilesOutput()
                    {
                        FileName = r.File.FileName,
                        FileSize = r.File.FileSize,
                        Id = r.File.Id,
                        Sort = r.Sort
                    }).ToList(),
                };
                ret.Add(entity);
            }
            return ret;
        }


        public List<GetMultiAbpFilesOutput> GetMultiList(GetMultiAbpFilesInput input)
        {
            var businessType = (int)input.BusinessType;
            if (input.BusinessIds == null || input.BusinessIds.Count == 0)
                throw new UserFriendlyException(0, "业务参数错误");
            var query = from a in _abpFileRepository.GetAll()
                        join b in _abpFileRelationRepository.GetAll() on a.Id equals b.FileId
                        where input.BusinessIds.Contains(b.BusinessId) && b.BusinessType == businessType
                        && !b.IsDeleted
                        orderby b.CreationTime
                        orderby a.FileName
                        select new { File = a, Sort = b.Sort, BusinessId = b.BusinessId };
            var ret = new List<GetMultiAbpFilesOutput>();
            var data = query.ToList();
            var dataGroup = data.GroupBy(r => r.BusinessId).ToList();
            foreach (var item in dataGroup)
            {
                var entity = new GetMultiAbpFilesOutput()
                {
                    BusinessId = item.Key,
                    Files = item.Select(r => new GetAbpFilesOutput()
                    {
                        FileName = r.File.FileName,
                        FileSize = r.File.FileSize,
                        Id = r.File.Id,
                        Sort = r.Sort
                    }).ToList(),
                };
                ret.Add(entity);
            }
            return ret;
        }


        public async Task<GetAbpFilesOutput> GetAsync(GetAbpFilesInput input)
        {
            var list = await GetListAsync(input);
            return list.FirstOrDefault();
        }

        public GetAbpFilesOutput Get(GetAbpFilesInput input)
        {
            var list = GetList(input);
            return list.FirstOrDefault();
        }

        public async Task CreateAsync(CreateFileRelationsInput input)
        {
            if (input.Files == null)
            {
                return;
            }
            foreach (var item in input.Files)
            {
                if (item.Id == Guid.Empty)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "文件id 不能为Guid.Empty");
                var entity = new AbpFileRelation()
                {
                    Id = Guid.NewGuid(),
                    BusinessId = input.BusinessId,
                    BusinessType = input.BusinessType,
                    FileId = item.Id,
                    Sort = item.Sort
                };

                await _abpFileRelationRepository.InsertAsync(entity);

            }

        }

        public void Create(CreateFileRelationsInput input)
        {
            foreach (var item in input.Files)
            {
                if (item.Id == Guid.Empty)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "文件id 不能为Guid.Empty");
                var entity = new AbpFileRelation()
                {
                    Id = Guid.NewGuid(),
                    BusinessId = input.BusinessId,
                    BusinessType = input.BusinessType,
                    FileId = item.Id,
                    Sort = item.Sort
                };

                _abpFileRelationRepository.Insert(entity);
            }

        }

        public async Task UpdateAsync(CreateFileRelationsInput input)
        {
            var exit_files = _abpFileRelationRepository.GetAll().Where(r => r.BusinessId == input.BusinessId && r.BusinessType == input.BusinessType);
            foreach (var item in exit_files)
            {
                await _abpFileRelationRepository.DeleteAsync(item);
            }

            foreach (var item in input.Files)
            {
                if (item.Id == Guid.Empty)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "文件id 不能为Guid.Empty");
                var entity = new AbpFileRelation()
                {
                    Id = Guid.NewGuid(),
                    BusinessId = input.BusinessId,
                    BusinessType = input.BusinessType,
                    FileId = item.Id,
                    Sort = item.Sort
                };
                await _abpFileRelationRepository.InsertAsync(entity);

            }

        }

        public void Update(CreateFileRelationsInput input)
        {
            var exit_files = _abpFileRelationRepository.GetAll().Where(r => r.BusinessId == input.BusinessId && r.BusinessType == input.BusinessType);
            foreach (var item in exit_files)
            {
                _abpFileRelationRepository.Delete(item);
            }

            foreach (var item in input.Files)
            {
                if (item.Id == Guid.Empty)
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "文件id 不能为Guid.Empty");
                var entity = new AbpFileRelation()
                {
                    Id = Guid.NewGuid(),
                    BusinessId = input.BusinessId,
                    BusinessType = input.BusinessType,
                    FileId = item.Id,
                    Sort = item.Sort
                };

                _abpFileRelationRepository.Insert(entity);
            }

        }

        public async Task CreateListAsync(string businessId, AbpFileBusinessType businessType, List<GetAbpFilesOutput> files)
        {
            if (files != null && files.Any())
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in files)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = businessId,
                    BusinessType = (int)businessType,
                    Files = fileList
                });
            }
        }

        public async Task UpdateListAsync(string businessId, AbpFileBusinessType businessType,
            List<GetAbpFilesOutput> files)
        {
            if (files != null && files.Any())
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in files)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = businessId,
                    BusinessType = (int)businessType,
                    Files = fileList
                });
            }
        }
    }



}

