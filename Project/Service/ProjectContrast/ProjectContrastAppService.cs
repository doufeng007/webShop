using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using System.IO;
using Abp.Domain.Repositories;
using Newtonsoft.Json.Linq;
using Abp.UI;
using Abp.Runtime.Session;
using CjzFormat;
using CjzContrast;
using CjzDataBase;
using MongoDB.Driver;
using MongoDB.Bson;
using Abp.Application.Services;
using Abp.File;
using ZCYX.FRMSCore.Application;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectContrastAppService : ApplicationService, IProjectContrastAppService
    {


        private readonly IRepository<CJZFileCompareResult, Guid> _cjzFileCompareResultRepository;
        private readonly IProjectFileRepository _projectFileRepository;
        private readonly IRepository<AbpFile, Guid> _abpFilerepository;
        private readonly IRepository<AbpFileRelation, Guid> _abpFileRelationRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;





        public ProjectContrastAppService(IRepository<CJZFileCompareResult, Guid> cjzFileCompareResultRepository, IProjectFileRepository projectFileRepository
            , IRepository<AbpFile, Guid> abpFilerepository, IProjectBaseRepository projectBaseRepository, IRepository<AbpFileRelation, Guid> abpFileRelationRepository
            , IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository)
        {
            _cjzFileCompareResultRepository = cjzFileCompareResultRepository;
            _projectFileRepository = projectFileRepository;
            _abpFilerepository = abpFilerepository;
            _projectBaseRepository = projectBaseRepository;
            _abpFileRelationRepository = abpFileRelationRepository;
            _singleProjectInfoRepository = singleProjectInfoRepository;
        }





        public GetProjectContrastOutput GetProjectContrastResult()
        {
            var helper = new FormatSvr();
            var ret = helper.FormatCjz(@"d:\721四川省团校综合楼装饰装修项目_投标报价.CJZ", 0);
            return new GetProjectContrastOutput() { FileData = ret };
        }


        public List<ProjectContrastResultData> GetProjectContrastResultData(EntityDto<string> intput)
        {

            var list = new List<ProjectContrastResultData>();
            var id = intput.Id;
            if (id == "1")
            {
                var item1 = new ProjectContrastResultData()
                {
                    ParentId = "1",
                    Id = "1.1",
                    Text = "第一层子1",
                    NodeType = 0

                };

                var item2 = new ProjectContrastResultData()
                {
                    ParentId = "1",
                    Id = "1.2",
                    Text = "第一层子2",
                    NodeType = 0

                };

                var item3 = new ProjectContrastResultData()
                {
                    ParentId = "1",
                    Id = "1.3",
                    Text = "第一层子3",
                    NodeType = 0

                };
                list.Add(item1);
                list.Add(item2);
                list.Add(item3);
            }
            else if (id == "2")
            {
                var item1 = new ProjectContrastResultData()
                {
                    ParentId = "2",
                    Id = "2.1",
                    Text = "第一层子1",
                    NodeType = 0

                };

                var item2 = new ProjectContrastResultData()
                {
                    ParentId = "2",
                    Id = "2.2",
                    Text = "第一层子2",
                    NodeType = 0

                };

                var item3 = new ProjectContrastResultData()
                {
                    ParentId = "2",
                    Id = "2.3",
                    Text = "第一层子3",
                    NodeType = 0

                };

                list.Add(item1);
                list.Add(item2);
                list.Add(item3);

            }
            else if (id == "1.1")
            {
                var item1 = new ProjectContrastResultData()
                {
                    ParentId = "1.1",
                    Id = "1.1.1",
                    Text = "第一层子1",
                    NodeType = 1,
                    OldValue = "123213",
                    NewValue = "123234FW"


                };

                list.Add(item1);


            }

            return list;
        }


        public async Task<ProjectCjzCompareData> GetProjectContrastResultDataV2(GetProjectContrastInput input)
        {
            var projectCjz = new ProjectCjzCompareData();

            string cjzfile = "";
            string cjzfile1 = "";
            if (input.Id == "1")
            {
                var filemodel = await GetCompareFileIds(input.ProjectId, input.CompareType);
                projectCjz.File1Info = filemodel.Item1;
                projectCjz.File2Info = filemodel.Item2;
                var cjzfileModel = _abpFilerepository.Get(filemodel.Item1.Id);
                cjzfile = cjzfileModel.FilePath;
                if (filemodel.Item2.FileName == "数据库")
                {
                    var result = await GetProjectContrastResultDataFromDB(new GetProjectContrastInput() { CompareType = input.CompareType, FilePath1 = cjzfile, Id = "1", ProjectId = input.ProjectId });
                    projectCjz.Result = result;
                    return projectCjz;
                }
                else
                {
                    var cjzfileModel1 = _abpFilerepository.Get(filemodel.Item2.Id);
                    cjzfile1 = cjzfileModel1.FilePath;
                }

            }
            else
            {
                cjzfile = @"d:\test\721四川省团校综合楼装饰装修项目_投标报价.CJZ";
                cjzfile1 = @"d:\test\721四川省团校综合楼装饰装修项目_投标报价 - 副本.CJZ";
            }

            ContrastItf svr = new ContrastSvr();
            if (cjzfile.IsNullOrWhiteSpace() || cjzfile1.IsNullOrWhiteSpace())
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "文件不存在");
            CtaData ctaData = svr.ContrastCjz(cjzfile, cjzfile1, 0);
            foreach (CtaDocData catdocItem in ctaData.docList)
            {
                var leftMenu = new ProjectCjzLeftMenu();
                leftMenu.Id = catdocItem.docDataId;
                leftMenu.Parent = catdocItem.parentUid;
                leftMenu.Text = catdocItem.docName;
                leftMenu.Level = catdocItem.docLevel;
                leftMenu.NodeType = catdocItem.docType;



                var tableList = new List<ProjectCjzCategoryTable>();
                foreach (CtaDzTableData ctatableItem in catdocItem.tableList)
                {

                    if (ctatableItem.errorCount > 0)
                    {
                        var projectTable = new ProjectCjzCategoryTable();
                        projectTable.Id = ctatableItem.UniqId;
                        projectTable.MenuId = leftMenu.Id;
                        projectTable.Name = ctatableItem.tableName;


                        int? xmtzColIndex = null;
                        foreach (CtaColDef colItem in ctatableItem.colList)
                        {
                            var projectTableCol = new ProjectCjzCategoryTableCol();
                            projectTableCol.Id = colItem.UniqId;
                            projectTableCol.ColName = colItem.colName;
                            if (colItem.colName == "项目特征")
                            {
                                xmtzColIndex = colItem.colIndx;
                            }
                            projectTable.ColList.Add(projectTableCol);
                        }


                        var maxDeep = 0;
                        foreach (CtaRecData recItem in ctatableItem.diffRecList)
                        {


                            var projectRowItem = new ProjectCjzDataRow();
                            projectRowItem.Id = recItem.UniqId;
                            projectRowItem.Sid = recItem.Sid;
                            foreach (CtaColDef colItem in ctatableItem.colList)
                            {
                                CtaColData celldate = recItem.GetColDataByCol(colItem.colIndx) as CtaColData;
                                var cellItem = new ProjectCjzCellData();
                                if (celldate != null)
                                {
                                    cellItem.Value = celldate.express;
                                    cellItem.Status = celldate.ctaStatus;
                                    cellItem.Index = colItem.colIndx;
                                }
                                else
                                {
                                    cellItem.Status = (int)_ctaStatusEnum.csDecrease;
                                }
                                projectRowItem.CellDatas.Add(cellItem);

                            }
                            if (recItem.childCount > 0)
                            {
                                projectRowItem.HasChildRow = true;
                                projectRowItem.ChildRowCount = recItem.childCount;
                            }
                            projectRowItem.Deep = recItem.recDeep;
                            projectRowItem.XMTZ = xmtzColIndex.HasValue ? recItem.GetValue(xmtzColIndex.Value) : "";
                            projectTable.NRecList.Add(projectRowItem);
                            if (maxDeep < recItem.recDeep)
                                maxDeep = recItem.recDeep;


                        }
                        foreach (CtaRecData recItem in ctatableItem.dzTable.diffRecList)
                        {

                            var projectRowItem = new ProjectCjzDataRow();
                            projectRowItem.Id = recItem.UniqId;
                            projectRowItem.Sid = recItem.Sid;

                            foreach (CtaColDef colItem in ctatableItem.colList)
                            {
                                CtaColData celldate = recItem.GetColDataByCol(colItem.colIndx) as CtaColData;
                                var cellItem = new ProjectCjzCellData();
                                if (celldate != null)
                                {
                                    cellItem.Value = celldate.express;
                                    cellItem.Status = celldate.ctaStatus;
                                    cellItem.Index = colItem.colIndx;

                                }
                                else
                                {
                                    cellItem.Status = (int)_ctaStatusEnum.csDecrease;
                                }
                                projectRowItem.CellDatas.Add(cellItem);

                            }
                            if (recItem.childCount > 0)
                            {
                                projectRowItem.HasChildRow = true;
                                projectRowItem.ChildRowCount = recItem.childCount;
                            }
                            projectRowItem.Deep = recItem.recDeep;
                            projectRowItem.XMTZ = xmtzColIndex.HasValue ? recItem.GetValue(xmtzColIndex.Value) : "";
                            projectTable.ORecList.Add(projectRowItem);
                            if (maxDeep < recItem.recDeep)
                                maxDeep = recItem.recDeep;
                        }

                        var currentIndex = 0;
                        var newSid = 0;


                        foreach (var oentity in projectTable.ORecList)
                        {

                            if (oentity.Sid != 0)
                                continue;
                            var oindex = projectTable.ORecList.IndexOf(oentity);
                            newSid = newSid - 1;
                            var insertcelldatas = new List<ProjectCjzCellData>();
                            for (var i = 0; i < projectTable.ColList.Count; i++)
                            {
                                insertcelldatas.Add(new ProjectCjzCellData() { Status = (int)_ctaStatusEnum.csDecrease });
                            }
                            insertcelldatas[0].Value = oentity.CellDatas[0].Value;
                            if (currentIndex == 0)
                            {
                                projectTable.NRecList.Insert(oindex, new ProjectCjzDataRow() { Sid = newSid, CellDatas = insertcelldatas, Deep = oentity.Deep, HasChildRow = oentity.HasChildRow, ChildRowCount = oentity.ChildRowCount });
                            }
                            else
                            {
                                projectTable.NRecList.Insert(currentIndex + 1, new ProjectCjzDataRow() { Sid = newSid, CellDatas = insertcelldatas, Deep = oentity.Deep, HasChildRow = oentity.HasChildRow, ChildRowCount = oentity.ChildRowCount });
                            }
                            oentity.Sid = newSid;
                            currentIndex = oindex;
                        }

                        currentIndex = 0;
                        foreach (var nentity in projectTable.NRecList)
                        {
                            var nindex = projectTable.NRecList.IndexOf(nentity);
                            if (nentity.Sid != 0)
                                continue;
                            newSid = newSid - 1;
                            var insertcelldatas = new List<ProjectCjzCellData>();
                            for (var i = 0; i < projectTable.ColList.Count; i++)
                            {
                                insertcelldatas.Add(new ProjectCjzCellData() { Status = (int)_ctaStatusEnum.csDecrease });
                            }
                            insertcelldatas[0].Value = nentity.CellDatas[0].Value;
                            if (currentIndex == 0)
                            {
                                projectTable.ORecList.Insert(nindex, new ProjectCjzDataRow() { Sid = newSid, CellDatas = insertcelldatas, Deep = nentity.Deep, HasChildRow = nentity.HasChildRow, ChildRowCount = nentity.ChildRowCount });
                            }
                            else
                            {
                                projectTable.ORecList.Insert(currentIndex + 1, new ProjectCjzDataRow() { Sid = newSid, CellDatas = insertcelldatas, Deep = nentity.Deep, HasChildRow = nentity.HasChildRow, ChildRowCount = nentity.ChildRowCount });
                            }
                            nentity.Sid = newSid;
                            currentIndex = nindex;
                        }
                        projectTable.MaxDeep = maxDeep - 1;
                        leftMenu.Tables.Add(projectTable);
                    }
                }

                projectCjz.Result.Menus.Add(leftMenu);

            }


            var item =
               _cjzFileCompareResultRepository.GetAll()
                   .FirstOrDefault(r => r.Pro_Id == input.ProjectId && r.CompareType == input.CompareType);
            if (item == null)
            {
                item = new CJZFileCompareResult();
                item.Id = Guid.NewGuid();
                item.SourceFileId = input.FilePath1 ?? "";
                item.TargetFileId = input.FilePath2 ?? "";
                item.CompareType = input.CompareType;
                item.Result = Newtonsoft.Json.JsonConvert.SerializeObject(projectCjz);
                item.Pro_Id = input.ProjectId;
                _cjzFileCompareResultRepository.Insert(item);
            }
            else
            {
                item.SourceFileId = input.FilePath1;
                item.TargetFileId = input.FilePath2;
                item.Result = Newtonsoft.Json.JsonConvert.SerializeObject(projectCjz);
                _cjzFileCompareResultRepository.Update(item);
            }
            projectCjz.Result.Id = item.Id;




            return projectCjz;
        }


        private async Task<Tuple<ProjectCompareFileInfo, ProjectCompareFileInfo>> GetCompareFileIds(Guid projectId,
            int compareType)
        {
            var projcetModel = await _singleProjectInfoRepository.GetAsync(projectId);
            var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer
                .Resolve<IProjectResultAppService>();
            switch ((CJZCompareEnum) compareType)
            {
                case CJZCompareEnum.送审资料与数据库对比:
                    var projectFile = await _projectFileRepository.FirstOrDefaultAsync(r =>
                        r.AappraisalFileType == 286 && r.SingleProjectId == projectId);
                    if (projectFile == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "送审资料缺少CJZ文件");
                    var projectCjzFile = await _abpFileRelationRepository.FirstOrDefaultAsync(r =>
                        r.BusinessId == projectFile.Id.ToString() && r.BusinessType == (int) AbpFileBusinessType.送审资料);
                    if (projectCjzFile == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "送审资料缺少CJZ文件");
                    var projectCjzFileModel = await _abpFilerepository.GetAsync(projectCjzFile.FileId);
                    return new Tuple<ProjectCompareFileInfo, ProjectCompareFileInfo>(
                        new ProjectCompareFileInfo()
                        {
                            FileName = projectCjzFileModel.FileName,
                            Id = projectCjzFileModel.Id
                        }, new ProjectCompareFileInfo() {FileName = "数据库"});
                case CJZCompareEnum.初审结果与数据库对比:
                    //var auditResult = await service.GetAuditMemberResult(projectId, (int)AuditRoleEnum.工程评审);
                    //return new Tuple<ProjectCompareFileInfo, ProjectCompareFileInfo>(new ProjectCompareFileInfo() { FileName = auditResult.CjzFile.FileName, Id = auditResult.CjzFile.Id, UserId = auditResult.UserId, UserName = auditResult.UserName }, new ProjectCompareFileInfo() { FileName = "数据库" });
                    throw new UserFriendlyException("暂不实现");
                case CJZCompareEnum.复核结果与初审结果对比:
                    //var auditResult1 = await service.GetAuditMemberResult(projectId, (int)AuditRoleEnum.工程评审);
                    //var auditResult2 = await service.GetAuditMemberResult(projectId, (int)AuditRoleEnum.复核人二);
                    //return new Tuple<ProjectCompareFileInfo, ProjectCompareFileInfo>(new ProjectCompareFileInfo() { FileName = auditResult1.CjzFile.FileName, Id = auditResult1.CjzFile.Id, UserId = auditResult1.UserId, UserName = auditResult1.UserName },
                    //    new ProjectCompareFileInfo() { FileName = auditResult2.CjzFile.FileName, Id = auditResult2.CjzFile.Id, UserId = auditResult2.UserId, UserName = auditResult2.UserName });
                    throw new UserFriendlyException("暂不实现");
                case CJZCompareEnum.二级复核结果与复核结果对比:

                    var auditResultFirstCollect =
                        await service.GetAuditMemberResult(projectId, (int) AuditRoleEnum.联系人一);
                    if (auditResultFirstCollect.CjzFile == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "复核结果缺少CJZ文件");
                    var auditResultSecondF = await service.GetAuditMemberResult(projectId, (int) AuditRoleEnum.复核人二);
                    if (auditResultSecondF.CjzFile == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "二级复核结果缺少CJZ文件");
                    return new Tuple<ProjectCompareFileInfo, ProjectCompareFileInfo>(
                        new ProjectCompareFileInfo()
                        {
                            FileName = auditResultFirstCollect.CjzFile.FileName,
                            Id = auditResultFirstCollect.CjzFile.Id,
                            UserId = auditResultFirstCollect.UserId,
                            UserName = auditResultFirstCollect.UserName
                        },
                        new ProjectCompareFileInfo()
                        {
                            FileName = auditResultSecondF.CjzFile.FileName,
                            Id = auditResultSecondF.CjzFile.Id,
                            UserId = auditResultSecondF.UserId,
                            UserName = auditResultSecondF.UserName
                        });
                    break;
                case CJZCompareEnum.三级复核结果与二级复核结果对比:
                    var auditResult3 = await service.GetAuditMemberResult(projectId, (int) AuditRoleEnum.复核人二);
                    if (auditResult3.CjzFile == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "二级复核结果缺少CJZ文件");
                    var auditResult4 = await service.GetAuditMemberResult(projectId, (int) AuditRoleEnum.复核人三);
                    if (auditResult4.CjzFile == null)
                        throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "三级复核结果缺少CJZ文件");
                    return new Tuple<ProjectCompareFileInfo, ProjectCompareFileInfo>(
                        new ProjectCompareFileInfo()
                        {
                            FileName = auditResult3.CjzFile.FileName,
                            Id = auditResult3.CjzFile.Id,
                            UserId = auditResult3.UserId,
                            UserName = auditResult3.UserName
                        },
                        new ProjectCompareFileInfo()
                        {
                            FileName = auditResult4.CjzFile.FileName,
                            Id = auditResult4.CjzFile.Id,
                            UserId = auditResult4.UserId,
                            UserName = auditResult4.UserName
                        });
                default:
                    return new Tuple<ProjectCompareFileInfo, ProjectCompareFileInfo>(new ProjectCompareFileInfo(),
                        new ProjectCompareFileInfo());
            }
        }






        public async Task<ProjectCjzData> GetProjectContrastResultDataFromDB(GetProjectContrastInput input)
        {
            var projectCjz = new ProjectCjzData();

            var h = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<MongoDBHelper>();
            //var h = new MongoDBHelper();
            try
            {
                string cjzfile = "";
                string cjzfile1 = "";
                if (input.Id == "1")
                {
                    var fileId = Guid.Empty;
                    if (Guid.TryParse(input.FilePath1, out fileId))
                    {
                        var cjzfileModel = _abpFilerepository.Get(input.FilePath1.ToGuid());
                        cjzfile = cjzfileModel.FilePath;
                    }
                    else
                    {
                        cjzfile = input.FilePath1;
                    }

                    cjzfile1 = "MongonDB";


                }
                else
                {
                    cjzfile = @"d:\test\721四川省团校综合楼装饰装修项目_投标报价.CJZ";
                    cjzfile1 = "MongonDB";
                }

                var svr = new FormatSvr();
                var ctaData = svr.FormatCjz(cjzfile, 2);
                var areaModel = await h.GetAreaCode("南部县");
                foreach (DocData catdocItem in ctaData.docList)
                {
                    var leftMenu = new ProjectCjzLeftMenu();
                    leftMenu.Id = catdocItem.UniqId;
                    leftMenu.Parent = catdocItem.parentUid;
                    leftMenu.Text = catdocItem.docName;
                    leftMenu.Level = catdocItem.docLevel;
                    leftMenu.NodeType = catdocItem.docType;
                    var tableList = new List<ProjectCjzCategoryTable>();
                    if (catdocItem.docType == "DWGC")
                    {

                        foreach (TableData ctatableItem in catdocItem.tableList)
                        {
                            if (ctatableItem.tableName == "分部分项清单")
                            {
                                var projectTable = new ProjectCjzCategoryTable();
                                projectTable.Id = ctatableItem.UniqId;
                                projectTable.MenuId = leftMenu.Id;
                                projectTable.Name = ctatableItem.tableName;


                                int? xmtzColIndex = null;
                                int? projectNoCol = null;
                                int? materialNameCol = null;
                                foreach (ColDef colItem in ctatableItem.colList)
                                {
                                    var projectTableCol = new ProjectCjzCategoryTableCol();
                                    projectTableCol.Id = colItem.UniqId;
                                    projectTableCol.ColName = colItem.colName;
                                    if (colItem.colName == "项目特征")
                                    {
                                        xmtzColIndex = colItem.colIndx;
                                    }
                                    else if (colItem.colName == "项目编码")
                                    {
                                        projectNoCol = colItem.colIndx;
                                    }
                                    else if (colItem.colName == "项目名称")
                                    {
                                        materialNameCol = colItem.colIndx;
                                    }
                                    projectTable.ColList.Add(projectTableCol);
                                }


                                var maxDeep = 0;
                                foreach (RecData recItem in ctatableItem.recList)
                                {


                                    var projectRowItem = new ProjectCjzDataRow();
                                    projectRowItem.Id = recItem.UniqId;
                                    projectRowItem.Sid = recItem.Sid;
                                    foreach (ColDef colItem in ctatableItem.colList)
                                    {
                                        var celldate = recItem.GetColDataByCol(colItem.colIndx);
                                        var cellItem = new ProjectCjzCellData();
                                        if (celldate != null)
                                        {
                                            cellItem.Value = celldate.express;
                                            //cellItem.Status = celldate.ctaStatus;
                                            cellItem.Index = colItem.colIndx;
                                            cellItem.ColName = colItem.colName;
                                        }
                                        else
                                        {
                                            cellItem.Status = (int)_ctaStatusEnum.csDecrease;
                                        }
                                        projectRowItem.CellDatas.Add(cellItem);

                                    }
                                    if (recItem.childCount > 0)
                                    {
                                        projectRowItem.HasChildRow = true;
                                        projectRowItem.ChildRowCount = recItem.childCount;
                                    }
                                    projectRowItem.Deep = recItem.recDeep;
                                    projectRowItem.XMTZ = xmtzColIndex.HasValue
                                        ? recItem.GetValue(xmtzColIndex.Value)
                                        : "";
                                    projectRowItem.ProjectNo = projectNoCol.HasValue
                                        ? recItem.GetValue(projectNoCol.Value)
                                        : "";
                                    projectRowItem.MaterialName = materialNameCol.HasValue
                                        ? recItem.GetValue(materialNameCol.Value)
                                        : "";
                                    projectTable.NRecList.Add(projectRowItem);
                                    if (maxDeep < recItem.recDeep)
                                        maxDeep = recItem.recDeep;


                                }
                                projectTable.MaxDeep = maxDeep - 1;

                                await h.GetFBFXQDStatus(projectTable, areaModel.County_or_district);
                                await h.GetFBFXQDXMTZStatus(projectTable);
                                if (projectTable.NRecList.Any(r => r.Status != 0))
                                {
                                    projectTable.NRecList = projectTable.NRecList.Where(r => r.Status != 0).ToList();
                                    leftMenu.Tables.Add(projectTable);
                                }

                            }
                        }
                    }
                    projectCjz.Menus.Add(leftMenu);
                }

                var item = _cjzFileCompareResultRepository.GetAll()
                       .FirstOrDefault(r => r.Pro_Id == input.ProjectId && r.CompareType == input.CompareType);
                if (item == null)
                {
                    item = new CJZFileCompareResult();
                    item.Id = Guid.NewGuid();
                    item.SourceFileId = input.FilePath1;
                    item.TargetFileId = "MongonDB";
                    item.CompareType = input.CompareType;
                    item.Result = Newtonsoft.Json.JsonConvert.SerializeObject(projectCjz);
                    _cjzFileCompareResultRepository.Insert(item);
                }
                else
                {
                    item.SourceFileId = input.FilePath1;
                    item.TargetFileId = "MongonDB";
                    item.Result = Newtonsoft.Json.JsonConvert.SerializeObject(projectCjz);
                    _cjzFileCompareResultRepository.Update(item);
                }
                projectCjz.Id = item.Id;
            }
            catch (Exception e)
            {

            }


            return projectCjz;
        }



        public async Task UpdateCompareResultRemark(UpdateCompareResultRemarkInput input)
        {
            var query = await _cjzFileCompareResultRepository.GetAsync(input.Id);
            query.Remark = input.Remark;
            await _cjzFileCompareResultRepository.UpdateAsync(query);
        }


        public async Task InsertIntoMongoDB(InsertIntoMongoDBInput input)
        {
            try
            {
                if (input.FilePath.IsNullOrEmpty())
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "未传入需要导入的CJZ文件");
                //var tmpFile = new FileInfo(input.FilePath.DesDecrypt());
                var tmpFile = new FileInfo(input.FilePath);

                if (!tmpFile.Exists)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到要导入的文件");
                }
                var datalist = new List<BsonDocument>();
                var datalistGLJ = new List<BsonDocument>();
                var datalistSub = new List<BsonDocument>();
                var helper = new FormatSvr();
                var ret = helper.FormatCjz(tmpFile.FullName, 2);
                var projectId = Guid.NewGuid().ToString("N");
                var projectName = ret.projectName;
                //var projectName = "南部县小元乡小学新建综合楼运动场及附属工程";
                var h = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<MongoDBHelper>();
                if (input.IncludFBFXQD)
                {
                    //工料机
                    var gljList = new List<CJZGLJDto>();
                    foreach (DocData docItem in ret.docList)
                    {
                        if (docItem.docType == "DWGC")
                        {
                            foreach (TableData tableItem in docItem.tableList)
                            {
                                if (tableItem.tableName == "工料机汇总")
                                {
                                    var unitCol = 0;
                                    var materialSinglePriceCol = 0;
                                    var materialNameCol = 0;
                                    var fixedPriceCol = 0;
                                    var speceCol = 0;
                                    var codeCol = 0;
                                    var quantityCol = 0;
                                    foreach (ColDef colItem in tableItem.colList)
                                    {
                                        if (colItem.colName == "单位")
                                        {
                                            unitCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "材料单价")
                                        {
                                            materialSinglePriceCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "材料名称")
                                        {
                                            materialNameCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "定额单价")
                                        {
                                            fixedPriceCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "规格型号")
                                        {
                                            speceCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "代码")
                                        {
                                            codeCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "数量")
                                        {
                                            quantityCol = colItem.colIndx;
                                        }
                                    }

                                    foreach (RecData recData in tableItem.recList)
                                    {
                                        var clEntity = new CJZGLJDto()
                                        {
                                            unit = recData.GetValue(unitCol) ?? "",
                                            material_single_price = recData.GetValue(materialSinglePriceCol) ?? "",
                                            material_name = recData.GetValue(materialNameCol) ?? "",
                                            fixed_price = recData.GetValue(fixedPriceCol) ?? "",
                                            specification = recData.GetValue(speceCol) ?? "",
                                            code = recData.GetValue(codeCol) ?? "",
                                            quantity = recData.GetValue(quantityCol) ?? "",
                                            project_name = docItem.docName,
                                            project_time = ret.writeDate,
                                            //project_type = docItem.docName,

                                        };
                                        gljList.Add(clEntity);
                                    }
                                }


                            }

                        }
                    }
                    //分部分项清单
                    foreach (DocData docItem in ret.docList)
                    {
                        if (docItem.docType == "DWGC")
                        {
                            foreach (TableData tableItem in docItem.tableList)
                            {
                                if (tableItem.tableName == "分部分项清单")
                                {
                                    var xmtzCol = 0;
                                    var projectNoCol = 0;
                                    var singlePriceCol = 0;
                                    var materialNameCol = 0;
                                    var unitCol = 0;
                                    var totalPriceCol = 0;
                                    var quantityCol = 0;

                                    var codeIndexChilde = 0;
                                    var wastageIndexChilde = 0;
                                    var quantityIndexChilde = 0;
                                    var calTypeIndexChilde = 0;
                                    var nocalPriceIndexChilde = 0;
                                    foreach (ColDef colItem in tableItem.colList)
                                    {
                                        if (colItem.colName == "项目特征")
                                        {
                                            xmtzCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "项目编码")
                                        {
                                            projectNoCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "综合单价")
                                        {
                                            singlePriceCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "项目名称")
                                        {
                                            materialNameCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "计量单位")
                                        {
                                            unitCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "综合合价")
                                        {
                                            totalPriceCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "工程量")
                                        {
                                            quantityCol = colItem.colIndx;
                                        }
                                    }
                                    var qdxmtable = tableItem as MDTableData;
                                    foreach (ColDef colItem in qdxmtable.dcolList)
                                    {
                                        if (colItem.colName == "关联材料代码")
                                        {
                                            codeIndexChilde = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "消耗量")
                                        {
                                            wastageIndexChilde = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "数量")
                                        {
                                            quantityIndexChilde = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "数量计算方式")
                                        {
                                            calTypeIndexChilde = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "不计价")
                                        {
                                            nocalPriceIndexChilde = colItem.colIndx;
                                        }
                                        else
                                        {

                                        }
                                    }

                                    int? qdDeep = null;
                                    var qdCategoryName = "";
                                    foreach (RecData recData in tableItem.recList)
                                    {
                                        var project_no = recData.GetValue(projectNoCol);
                                        if (!qdDeep.HasValue)
                                        {
                                            if (string.IsNullOrWhiteSpace(project_no) || project_no.Length < 12)
                                            {
                                                if (recData.recDeep == 1)
                                                {
                                                    qdCategoryName = recData.GetValue(materialNameCol) ?? "";
                                                }
                                                continue;
                                            }
                                            var entity = new BsonDocument(){
                                                {"_id",$"南部县-{Guid.NewGuid():N}" },
                                                {"specification_and_models",recData.GetValue(xmtzCol)??"" },
                                                { "project_no", recData.GetValue(projectNoCol)??""},
                                                { "county_or_district", "南部县"},
                                                { "single_price", recData.GetValue(singlePriceCol)??""},
                                                { "city_or_state", "南充"},
                                                { "material_name", recData.GetValue(materialNameCol)??"" },
                                                { "project_name", projectName },
                                                { "unit", recData.GetValue(unitCol)??""},
                                                { "total_price", recData.GetValue(totalPriceCol)??""},
                                                { "project_id", projectId},
                                                { "categoryname", qdCategoryName},
                                                { "quantity", recData.GetValue(quantityCol)??""}};
                                            datalist.Add(entity);
                                            qdDeep = recData.recDeep;

                                        }
                                        else
                                        {
                                            if (recData.recDeep < qdDeep)
                                            {
                                                if (recData.recDeep == 1)
                                                {
                                                    qdCategoryName = recData.GetValue(materialNameCol) ?? "";
                                                }
                                                continue;

                                            }
                                            else if (recData.recDeep == qdDeep)
                                            {
                                                if (string.IsNullOrWhiteSpace(project_no) || project_no.Length < 12)
                                                    continue;
                                                else
                                                {
                                                    var entity = new BsonDocument(){
                                                {"_id",$"南部县-{Guid.NewGuid():N}" },
                                                {"specification_and_models",recData.GetValue(xmtzCol)??"" },
                                                { "project_no", recData.GetValue(projectNoCol)??""},
                                                { "county_or_district", "南部县"},
                                                { "single_price", recData.GetValue(singlePriceCol)??""},
                                                { "city_or_state", "南充"},
                                                { "material_name", recData.GetValue(materialNameCol)??"" },
                                                { "project_name", projectName },
                                                { "unit", recData.GetValue(unitCol)??""},
                                                { "total_price", recData.GetValue(totalPriceCol)??""},
                                                { "project_id", projectId},
                                                { "categoryname", qdCategoryName},
                                                { "quantity", recData.GetValue(quantityCol)??""},};
                                                    datalist.Add(entity);
                                                }
                                            }
                                            else if (recData.recDeep == (qdDeep + 1))
                                            {
                                                if (string.IsNullOrWhiteSpace(project_no))
                                                    continue;
                                                else
                                                {
                                                    var parentRec = recData.parentRec;
                                                    //var entity = new BsonDocument(){
                                                    //    {"_id",$"南部县-{Guid.NewGuid():N}" },
                                                    //    { "project_no", recData.GetValue(projectNoCol)??"" },
                                                    //    { "parent_no1", parentRec.GetValue(projectNoCol)??"" },
                                                    //    { "parent_no2", "" },
                                                    //    { "material_name", recData.GetValue(materialNameCol)??"" },
                                                    //    { "unit", recData.GetValue(unitCol)??""},
                                                    //    {"wastage",""},
                                                    //    { "quantity", recData.GetValue(quantityCol)??""},
                                                    //    { "single_price", recData.GetValue(singlePriceCol)??""},
                                                    //    { "total_price", recData.GetValue(totalPriceCol)??""},
                                                    //    { "quantity_calculation_method", ""},
                                                    //    { "nocalprice", ""},
                                                    //    { "project_name", projectName },
                                                    //    { "project_id", projectId}};

                                                    var entity1 = new CJZSubFBFXQDDto()
                                                    {
                                                        _id = $"南部县-{Guid.NewGuid():N}",
                                                        project_no = recData.GetValue(projectNoCol) ?? "",
                                                        parent_no1 = parentRec.GetValue(projectNoCol) ?? "",
                                                        parent_no2 = "",
                                                        material_name = recData.GetValue(materialNameCol) ?? "",
                                                        unit = recData.GetValue(unitCol) ?? "",
                                                        wastage = "",
                                                        quantity = recData.GetValue(quantityCol) ?? "",
                                                        single_price = recData.GetValue(singlePriceCol) ?? "",
                                                        total_price = recData.GetValue(totalPriceCol) ?? "",
                                                        quantity_calculation_method = "",
                                                        nocalprice = "",
                                                        project_name = projectName,
                                                        project_id = projectId,
                                                    };
                                                    entity1.ChangUnit();

                                                    datalistSub.Add(entity1.ToBsonDocument());
                                                    var childtable = recData.childList;
                                                    if (childtable.ItemType == typeof(MxclRec))
                                                    {
                                                        var paren_parentRec = recData.parentRec.parentRec;
                                                        foreach (RecData child_recData in childtable)
                                                        {
                                                            var entity_code = child_recData.GetValue(codeIndexChilde);
                                                            var entity_wastage = child_recData.GetValue(wastageIndexChilde);
                                                            var entity_quantity = child_recData.GetValue(quantityIndexChilde);
                                                            var entity_calType = child_recData.GetValue(calTypeIndexChilde);
                                                            var entity_nocalPrice = child_recData.GetValue(nocalPriceIndexChilde);
                                                            var entity_GLJ = gljList.FirstOrDefault(r => r.code == entity_code);

                                                            var entity_CLMX = new BsonDocument(){
                                                                {"_id",$"南部县-{Guid.NewGuid():N}" },
                                                                { "project_no",""},
                                                                { "parent_no1", parentRec.GetValue(projectNoCol)??"" },
                                                                { "parent_no2", recData.GetValue(projectNoCol)??"" },
                                                                { "material_name", entity_GLJ.material_name },
                                                                { "unit", entity_GLJ.unit},
                                                                {"wastage",entity_wastage},
                                                                {"quantity",entity_quantity},
                                                                { "single_price", entity_GLJ.material_single_price},
                                                                { "total_price", ""},
                                                                { "quantity_calculation_method", entity_calType},
                                                                { "nocalprice", entity_nocalPrice},
                                                                { "project_name", projectName },
                                                                { "project_id", projectId}
                                                            };
                                                            datalistSub.Add(entity_CLMX);
                                                        }



                                                    }




                                                }
                                            }
                                            else if (recData.recDeep == (qdDeep + 2))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                continue;
                                            }

                                        }


                                    }



                                }
                            }

                        }
                    }
                    h.InsertMany(datalist, MongoDBHelper.fbfxTableName);
                    foreach (var item in gljList)
                    {
                        var entity = new BsonDocument()
                    {
                        {"_id",$"{Guid.NewGuid():N}" },
                        {"unit",item.unit },
                        { "material_single_price", item.material_single_price},
                        { "material_name", item.material_name},
                        { "fixed_price", item.fixed_price},
                        { "specification", item.specification},
                        { "code", item.code },
                        { "quantity", item.quantity },
                        { "project_name", item.project_name},
                        { "project_time", item.project_time}
                    };
                        datalistGLJ.Add(entity);
                    }
                    h.InsertMany(datalistSub, MongoDBHelper.fbfxSubTableName);
                }
                if (input.IncludGLJ)
                {
                    h.InsertMany(datalistGLJ, MongoDBHelper.labormaterialmachinecjzTableName);
                }

                if (input.IncludGCZJHZ)
                {
                    var data_gczjHZ = new List<BsonDocument>();
                    //工程造价汇总
                    foreach (DocData docItem in ret.docList)
                    {
                        if (docItem.docType == "ZGC")
                        {
                            foreach (TableData tableItem in docItem.tableList)
                            {
                                if (tableItem.tableName == "工程造价汇总")
                                {
                                    var nameCol = 0;
                                    var feeCol = 0;
                                    foreach (ColDef colItem in tableItem.colList)
                                    {
                                        if (colItem.colName == "名称")
                                        {
                                            nameCol = colItem.colIndx;
                                        }
                                        else if (colItem.colName == "金额")
                                        {
                                            feeCol = colItem.colIndx;
                                        }
                                        else { }
                                    }

                                    foreach (RecData recData in tableItem.recList)
                                    {
                                        var entity = new BsonDocument()
                                    {
                                        {"_id",$"南部县-{Guid.NewGuid():N}" },
                                        {"name",recData.GetValue(nameCol)??"" },
                                        { "fee", recData.GetValue(feeCol)??""},
                                        { "project_id", projectId}
                                    };
                                        data_gczjHZ.Add(entity);
                                    }
                                }


                            }
                            break;
                        }
                    }
                    h.InsertMany(data_gczjHZ, MongoDBHelper.engineeringcostcollectTableName);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<List<FileUploadFiles>> GetProjectFileForCompare(
            GetProjectFileForCompareInput intput)
        {
            var query = await _projectFileRepository.GetAll()
                    .Where(r => r.ProjectBaseId == intput.ProjectId && r.AappraisalFileType == intput.AappraisalFileType).FirstOrDefaultAsync();
            if (query != null)
            {
                var entity = query.FileName;
                if (!string.IsNullOrWhiteSpace(entity))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileUploadFiles>>(entity);
            }
            return new List<FileUploadFiles>();
        }




    }


}
