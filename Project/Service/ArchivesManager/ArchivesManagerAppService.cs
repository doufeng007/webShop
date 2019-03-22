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
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore.Model;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Extensions;

namespace Project
{
    public class ArchivesManagerAppService : FRMSCoreAppServiceBase, IArchivesManagerAppService
    {
        private readonly IRepository<ArchivesManager, Guid> _archivesManagerRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        private readonly IRepository<ArchivesFile, Guid> _archivesFileRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IProjectFileRepository _projectFileRepository;
        private readonly IRepository<AappraisalFileType> _aappraisalFileTypeRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        public ArchivesManagerAppService(IRepository<ArchivesManager, Guid> archivesManagerRepository, IRepository<ArchivesFile, Guid> archivesFileRepository, IRepository<User, long> userRepository, IProjectBaseRepository projectBaseRepository
            , IProjectFileRepository projectFileRepository, WorkFlowCacheManager workFlowCacheManager, ProjectAuditManager projectAuditManager,
            IRepository<AappraisalFileType> aappraisalFileTypeRepository, IWorkFlowTaskRepository workFlowTaskRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository)
        {
            _archivesManagerRepository = archivesManagerRepository;
            _archivesFileRepository = archivesFileRepository;
            _userRepository = userRepository;
            _projectBaseRepository = projectBaseRepository;
            _projectFileRepository = projectFileRepository;
            _aappraisalFileTypeRepository = aappraisalFileTypeRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowCacheManager = workFlowCacheManager;
            _projectAuditManager = projectAuditManager;
            _singleProjectInfoRepository = singleProjectInfoRepository;
        }

        public async Task CreateOrUpdateArchivesManager(CreateOrUpdateArchivesManagerInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateArchivesManagerAsync(input);
            }
            else
            {
                await CreateArchivesManagerAsync(input);
            }
        }

        public async Task<GetArchivesManagerForEditOutput> GetArchivesManagerForEdit(NullableIdDto<Guid> input)
        {
            if (!input.Id.HasValue)
                return new GetArchivesManagerForEditOutput();
            var query = from a in _archivesManagerRepository.GetAll()
                        join file in _archivesFileRepository.GetAll() on a.Id equals file.ArchivesId into g
                        join project in _projectBaseRepository.GetAll() on a.ProjectId equals project.Id into p
                        from pro in p.DefaultIfEmpty()
                        where a.Id == input.Id.Value
                        select new { a = a, files = g, ProjectName = pro == null ? "" : pro.ProjectName, ProjectCode = pro == null ? "" : pro.ProjectCode, SingleProjectName = pro == null ? "" : pro.SingleProjectName, SingleProjectCode = pro == null ? "" : pro.SingleProjectCode };
            var data = await query.FirstOrDefaultAsync();
            var output = data.a.MapTo<GetArchivesManagerForEditOutput>();
            output.ProjectName = data.ProjectName;
            output.SingleProjectName = data.SingleProjectName;
            output.ProjectCode = data.ProjectCode;
            output.SingleProjectCode = data.SingleProjectCode;
            foreach (var file in data.files)
            {
                var entity = file.MapTo<CreateOrUpdateArchivesFileInput>();
                output.Files.Add(entity);
            }
            return output;
        }



        public async Task<PagedResultDto<ArchivesManagerListOutputDto>> GetArchivesManagers(GetArchivesManagerListInput input)
        {
            var query = from a in _archivesManagerRepository.GetAll()
                        //join file in _archivesFileRepository.GetAll() on a.Id equals file.ArchivesId into g
                        join project in _projectBaseRepository.GetAll() on a.ProjectId equals project.Id into p
                        from pro in p.DefaultIfEmpty()
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x => x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new ArchivesManagerListOutputDto
                        {
                            Id = a.Id,
                            ArchivesName = a.ArchivesName,
                            ArchivesType = a.ArchivesType,
                            SecrecyLevel = a.SecrecyLevel,
                            ProjectName = pro == null ? "" : pro.ProjectName,
                            ProjectCode = pro == null ? "" : pro.ProjectCode,
                            SingleProjectName = pro == null ? "" : pro.SingleProjectName,
                            SingleProjectCode = pro == null ? "" : pro.SingleProjectCode,
                            CreationTime = a.CreationTime,
                            Status = a.Status,
                            CreatorUserId = a.CreatorUserId,
                            ProjectId = a.ProjectId,
                            OpenModel = openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };

            if (!string.IsNullOrWhiteSpace(input.SearchKey))
            {
                query = query.Where(r => r.ProjectName.Contains(input.SearchKey) || r.ArchivesName.Contains(input.SearchKey));
            }
            if (input.ArchivesManagerType.HasValue)
            {
                query = query.Where(r => r.ArchivesType == input.ArchivesManagerType);
            }
            if (input.startDate.HasValue)
            {
                query = query.Where(r => r.CreationTime >= input.startDate.Value);
            }
            if (input.endDate.HasValue)
            {
                query = query.Where(r => r.CreationTime <= input.endDate.Value);
            }
            
            var count = await query.CountAsync();
            var archivesManagers = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();

            foreach (var r in archivesManagers)
            {
                r.InstanceId = r.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, r as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<ArchivesManagerListOutputDto>(count, archivesManagers);

        }


        public async Task DeleteArchivesManager(EntityDto<Guid> input)
        {
            var model = await _archivesManagerRepository.GetAsync(input.Id);
            _archivesFileRepository.GetAll().Where(r => r.ArchivesId == input.Id).ForEachAsync(r => _archivesFileRepository.DeleteAsync(r));
            await _archivesManagerRepository.DeleteAsync(model);
        }



        protected virtual async Task UpdateArchivesManagerAsync(CreateOrUpdateArchivesManagerInput input)
        {
            var model = await _archivesManagerRepository.GetAsync(input.Id.Value);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            var old_Model = model.DeepClone();
            var filedbModel = await _archivesFileRepository.GetAll().Where(r => r.ArchivesId == model.Id).ToListAsync();
            var add_fileModel = input.Files.Where(r => r.Id == null).ToList();
            add_fileModel.ForEach(r => { r.ArchivesId = model.Id; _archivesFileRepository.InsertAsync(r.MapTo<ArchivesFile>()); });
            var less_fileControlIds = filedbModel.Select(r => r.Id).ToList().Except(input.Files.Where(r => r.Id.HasValue).Select(o => o.Id.Value).ToList()).ToList();
            less_fileControlIds.ForEach(r => { _archivesFileRepository.Delete(filedbModel.FirstOrDefault(o => o.Id == r)); });
            var update_files = input.Files.Where(r => r.Id != null && !less_fileControlIds.Contains(r.Id.Value)).ToList();
            update_files.ForEach(r =>
            {
                var db_file = filedbModel.FirstOrDefault(o => o.Id == r.Id.Value);
                r.MapTo(db_file);
                _archivesFileRepository.UpdateAsync(db_file);
            });

            input.MapTo(model);
            await _archivesManagerRepository.UpdateAsync(model);
            await CurrentUnitOfWork.SaveChangesAsync();
            if (input.IsUpdateForChange)
            {
                var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                var logs = GetChangeModel(old_Model).GetColumnAllLogs(GetChangeModel(model));
                await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
            }
        }
        private CreateOrUpdateArchivesManagerInput GetChangeModel(ArchivesManager model)
        {
            /// 如果有外键数据 在这里转换
            var ret = model.MapTo<CreateOrUpdateArchivesManagerInput>();
            var doc = _archivesManagerRepository.Get(ret.Id.Value);
            
            return ret;
        }
        protected virtual async Task CreateArchivesManagerAsync(CreateOrUpdateArchivesManagerInput input)
        {
            var archivesManager = new ArchivesManager();
            input.MapTo(archivesManager);
            var id = Guid.NewGuid();
            archivesManager.Id = id;
            await _archivesManagerRepository.InsertAsync(archivesManager);
            foreach (var file in input.Files)
            {
                var entity = new ArchivesFile();
                file.Id = Guid.NewGuid();
                file.ArchivesId = id;
                file.MapTo(entity);
                await _archivesFileRepository.InsertAsync(entity);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }


        public Guid CreateArchivesManagerActive(string instaceId)
        {
            var projectId = instaceId.ToGuid();
            var data = new CreateOrUpdateArchivesManagerInput();
            data.Id = Guid.NewGuid();
            var files = from f in _projectFileRepository.GetAll()
                        join filetype in _aappraisalFileTypeRepository.GetAll() on f.AappraisalFileType equals filetype.Id
                        where f.SingleProjectId == projectId
                        select new { f.Id, f.FileName, f.FilePath, f.IsPaperFile, filetype.Name, f.PaperFileNumber };
            
            var projectModel = _singleProjectInfoRepository.Get(projectId);
            data.ProjectId = projectModel.Id;
            foreach (var file in files)
            {
                //if(string.IsNullOrWhiteSpace(file.FileName))
                //    continue;
                var entity = new CreateOrUpdateArchivesFileInput() { FileName = file.FileName, Id = file.Id, FileType = file.Name, IsPaper = file.IsPaperFile, PaperNumber = file.PaperFileNumber };
                if (!string.IsNullOrWhiteSpace(file.FileName))
                    entity.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(file.FileName);
                data.Files.Add(entity);
            }

            var archivesManager = new ArchivesManager();
            archivesManager.Id = data.Id.Value;
            archivesManager.ArchivesName = projectModel.SingleProjectName+"-归档";
            archivesManager.ProjectId = data.ProjectId;
            _archivesManagerRepository.InsertAsync(archivesManager);

            foreach (var file in data.Files)
            {
                var entity = new ArchivesFile();
                file.Id = Guid.NewGuid();
                file.ArchivesId = archivesManager.Id;
                file.MapTo(entity);
                _archivesFileRepository.Insert(entity);
            }
            return archivesManager.Id;
        }


        public async Task<Guid> CreateArchivesManager(CreateOrUpdateArchivesManagerInput input)
        {
            var archivesManager = new ArchivesManager();
            input.MapTo(archivesManager);
            var id = Guid.NewGuid();
            archivesManager.Id = id;
            await _archivesManagerRepository.InsertAsync(archivesManager);
            foreach (var file in input.Files)
            {
                var entity = new ArchivesFile();
                file.Id = Guid.NewGuid();
                file.ArchivesId = id;
                file.MapTo(entity);
                await _archivesFileRepository.InsertAsync(entity);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return id;

        }


        public async Task<GetArchivesManagerForEditOutput> GetArchivesForWFProject(Guid projectId)
        {
            var projectModel = await _projectBaseRepository.GetAsync(projectId);
            if (projectModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            var data = new GetArchivesManagerForEditOutput();
            var archivesModel = await _archivesManagerRepository.GetAll().FirstOrDefaultAsync(r => r.ProjectId == projectId);
            if (archivesModel == null)
            {
                data.Id = null;
                var files = from f in _projectFileRepository.GetAll()
                            join filetype in _aappraisalFileTypeRepository.GetAll() on f.AappraisalFileType equals filetype.Id
                            where f.ProjectBaseId == projectId
                            select new { f.Id, f.FileName, f.FilePath, f.IsPaperFile, filetype.Name, f.PaperFileNumber };

                data.ProjectId = projectModel.Id;
                data.ProjectName = projectModel.ProjectName;
                data.ProjectCode = projectModel.ProjectCode;
                data.SingleProjectCode = projectModel.SingleProjectCode;
                data.SingleProjectName = projectModel.SingleProjectName;
                data.AppraisalTypeId = projectModel.AppraisalTypeId;
                foreach (var file in files)
                {
                    //if(string.IsNullOrWhiteSpace(file.FileName))
                    //    continue;
                    var entity = new CreateOrUpdateArchivesFileInput() { FileName = file.FileName, Id = file.Id, FileType = file.Name, IsPaper = file.IsPaperFile, PaperNumber = file.PaperFileNumber };
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                        entity.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(file.FileName);
                    data.Files.Add(entity);
                }
            }
            else
            {
                data.ArchivesName = archivesModel.ArchivesName;
                data.ArchivesNumber = archivesModel.ArchivesNumber;
                data.ArchivesNumber1 = archivesModel.ArchivesNumber1;
                data.ArchivesType = archivesModel.ArchivesType;
                data.Id = archivesModel.Id;
                data.Location = archivesModel.Location;
                data.PageNumber = archivesModel.PageNumber;
                data.ProjectCode = projectModel.ProjectCode;
                data.AppraisalTypeId = projectModel.AppraisalTypeId;
                data.ProjectId = projectId;
                data.ProjectName = projectModel.ProjectName;
                data.SingleProjectCode = projectModel.SingleProjectCode;
                data.SingleProjectName = projectModel.SingleProjectName;
                data.SecrecyLevel = archivesModel.SecrecyLevel;
                data.Summary = archivesModel.Summary;
                data.VolumeNumber = archivesModel.VolumeNumber;
                var files = from f in _archivesFileRepository.GetAll()
                            where f.ArchivesId == archivesModel.Id
                            select f;

                files.MapTo(data.Files);
                foreach (var filemodel in data.Files)
                {
                    if (!string.IsNullOrWhiteSpace(filemodel.FileName))
                        filemodel.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(filemodel.FileName);
                }
            }
            return data;
        }


        public async Task<GetArchivesManagerForEditOutput> GetArchivesForWFProjectWithId(string instanceId)
        {
            var id = instanceId.ToGuid();
            var model = await _archivesManagerRepository.GetAsync(id);
            if (!model.ProjectId.HasValue) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            var projectId = model.ProjectId.Value;
            var projectModel = await _singleProjectInfoRepository.GetAsync(projectId);
            if (projectModel == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            var projectBase = await _projectBaseRepository.GetAsync(projectModel.ProjectId);
            if (projectBase == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据异常");
            var data = new GetArchivesManagerForEditOutput();
            var archivesModel = await _archivesManagerRepository.GetAll().FirstOrDefaultAsync(r => r.ProjectId == projectId);
            if (archivesModel == null)
            {
                data.Id = null;
                var files = from f in _projectFileRepository.GetAll()
                            join filetype in _aappraisalFileTypeRepository.GetAll() on f.AappraisalFileType equals filetype.Id
                            where f.ProjectBaseId == projectId
                            select new { f.Id, f.FileName, f.FilePath, f.IsPaperFile, filetype.Name, f.PaperFileNumber };

                data.ProjectId = projectModel.Id;
                data.ProjectName = projectModel.SingleProjectName;
                data.ProjectCode = projectModel.ProjectCode;
                data.SingleProjectCode = projectModel.SingleProjectCode;
                data.SingleProjectName = projectModel.SingleProjectName;
                data.AppraisalTypeId = projectBase.AppraisalTypeId;
                foreach (var file in files)
                {
                    //if(string.IsNullOrWhiteSpace(file.FileName))
                    //    continue;
                    var entity = new CreateOrUpdateArchivesFileInput() { FileName = file.FileName, Id = file.Id, FileType = file.Name, IsPaper = file.IsPaperFile, PaperNumber = file.PaperFileNumber };
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                        entity.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(file.FileName);
                    data.Files.Add(entity);
                }
            }
            else
            {
                data.ArchivesName = archivesModel.ArchivesName;
                data.ArchivesNumber = archivesModel.ArchivesNumber;
                data.ArchivesNumber1 = archivesModel.ArchivesNumber1;
                data.ArchivesType = archivesModel.ArchivesType;
                data.Id = archivesModel.Id;
                data.Location = archivesModel.Location;
                data.PageNumber = archivesModel.PageNumber;
                data.ProjectCode = projectModel.ProjectCode;
                data.AppraisalTypeId = projectBase.AppraisalTypeId;
                data.ProjectId = projectId;
                data.ProjectName = projectModel.SingleProjectName;
                data.SingleProjectCode = projectModel.SingleProjectCode;
                data.SingleProjectName = projectModel.SingleProjectName;
                data.SecrecyLevel = archivesModel.SecrecyLevel;
                data.Summary = archivesModel.Summary;
                data.VolumeNumber = archivesModel.VolumeNumber;
                var files = from f in _archivesFileRepository.GetAll()
                            where f.ArchivesId == archivesModel.Id
                            select f;

                files.MapTo(data.Files);
                foreach (var filemodel in data.Files)
                {
                    if (!string.IsNullOrWhiteSpace(filemodel.FileName))
                        filemodel.Files = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(filemodel.FileName);
                }
            }
            return data;
        }
    }
}
