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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using WebClientServer;
using Abp.Domain.Uow;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class AccountantCourseAppService : FRMSCoreAppServiceBase, IAccountantCourseAppService
    {
        private readonly IRepository<AccountantCourse, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IDynamicRepository _dynamicRepository;
        private IHostingEnvironment hostingEnv;
        //private readonly IAbpFileAppService _abpFileRelationAppService;

        private readonly IConfigurationRoot _appConfiguration;

        public AccountantCourseAppService(IRepository<AccountantCourse, Guid> repository, IDynamicRepository dynamicRepository
            , IHostingEnvironment env
            )
        {
            this._repository = repository;
            //_abpFileRelationAppService = abpFileRelationAppService;
            hostingEnv = env;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
            _dynamicRepository = dynamicRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<AccountantCourseListOutputDto>> GetList(GetAccountantCourseListInput input)
        {
            var toalCount = 0;
            var ret = new List<AccountantCourseListOutputDto>();
            if (input.IsOnlyRoot)
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            where !a.Pid.HasValue
                            select new AccountantCourseListOutputDto()
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Pid = a.Pid,
                                parent_left = a.parent_left,
                                parent_right = a.parent_right,
                                CreationTime = a.CreationTime
                            };
                toalCount = await query.CountAsync();
                ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            }
            else
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            select new AccountantCourseListOutputDto()
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Pid = a.Pid,
                                parent_left = a.parent_left,
                                parent_right = a.parent_right,
                                CreationTime = a.CreationTime
                            };
                if (input.Pid.HasValue)
                {
                    var pModel = await _repository.GetAsync(input.Pid.Value);
                    if (input.IsAllChilds)
                    {
                        query = query.Where(r => r.parent_left > pModel.parent_left && r.parent_right < pModel.parent_right);
                    }
                    else
                    {
                        query = query.Where(r => r.Pid == pModel.Id);
                    }
                }
                toalCount = await query.CountAsync();
                ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            }
            return new PagedResultDto<AccountantCourseListOutputDto>(toalCount, ret);
        }





        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<AccountantCourseOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<AccountantCourseOutputDto>();
        }
        /// <summary>
        /// 添加一个AccountantCourse
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task<AccountantCourseOutputDto> Create(CreateAccountantCourseInput input)
        {
            var parent_Model = await _repository.GetAsync(input.Pid);
            var updateLeftNodes = _repository.GetAll().Where(r => r.parent_left > parent_Model.parent_left);
            foreach (var updateLeftNode in updateLeftNodes)
            {
                updateLeftNode.parent_left = updateLeftNode.parent_left + 2;
            }
            var updateRightNodes = _repository.GetAll().Where(r => r.parent_right > parent_Model.parent_left);
            foreach (var updateRightNode in updateRightNodes)
            {
                updateRightNode.parent_right = updateRightNode.parent_right + 2;
            }
            var newmodel = new AccountantCourse()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Pid = parent_Model.Id,
            };
            newmodel.parent_left = parent_Model.parent_left + 1;
            newmodel.parent_right = parent_Model.parent_left + 2;
            await _repository.InsertAsync(newmodel);
            RefreshCL();
            var ret = newmodel.MapTo<AccountantCourseOutputDto>();
            return ret;
        }

        /// <summary>
        /// 修改一个AccountantCourse
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateAccountantCourseInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                dbmodel.Name = input.Name;
                await _repository.UpdateAsync(dbmodel);
                RefreshCL();
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

        // <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            var deleteModel = await _repository.GetAsync(input.Id);
            var updateLeftNodes = _repository.GetAll().Where(r => r.parent_left > deleteModel.parent_left);
            foreach (var updateLeftNode in updateLeftNodes)
            {
                updateLeftNode.parent_left = updateLeftNode.parent_left - 2;
            }
            var updateRightNodes = _repository.GetAll().Where(r => r.parent_right > deleteModel.parent_left);
            foreach (var updateRightNode in updateRightNodes)
            {
                updateRightNode.parent_right = updateRightNode.parent_right - 2;
            }
            await _repository.DeleteAsync(x => x.Id == input.Id);

            RefreshCL();

        }


        public async Task ImportFile(ImportFileInputDto input)
        {
            string filePath = hostingEnv.WebRootPath + $@"/Files/upload/temp/";
            string fileFullName = filePath + input.FileRelationPath;
            var retData = new List<AccountantCourseOutputDto>();
            if (System.IO.File.Exists(fileFullName))
            {
                var firstNode = new AccountantCourseOutputDto()
                {
                    Name = "会计科目",
                    Id = Guid.NewGuid(),
                    parent_left = 0,
                    parent_right = 1,
                };

                var unKnowId = _appConfiguration.GetValue<string>("App:ACOtherId").ToGuid();
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    _repository.Delete(r => r.Id == unKnowId);
                }
                ///增加统一id的未知节点
                retData.Add(new AccountantCourseOutputDto() { Id = _appConfiguration.GetValue<string>("App:ACOtherId").ToGuid(), Name = "未知", Pid = firstNode.Id });

                DataTable dt = new DataTable();
                #region 导入execle
                using (FileStream stream = System.IO.File.Open(fileFullName, FileMode.Open, FileAccess.Read))
                {
                    //创建 XSSFWorkbook和ISheet实例
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    //获取sheet的首行
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;
                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        //Column 添加ColumnName
                        var dtColumn = new DataColumn(headerRow.GetCell(i).StringCellValue, typeof(string));
                        dt.Columns.Add(dtColumn);
                    }

                    int rowCount = sheet.LastRowNum;
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        object[] valuelist = new object[cellCount];
                        IRow row = sheet.GetRow(i);
                        var newRow = dt.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            //遍历添加Column的数据
                            if (row.GetCell(j) != null)
                            {
                                valuelist.SetValue(row.GetCell(j).ToString(), j);
                                newRow[j] = row.GetCell(j).ToString();
                            }
                        }
                        //遍历将Column的数据添加到Datarow
                        dt.Rows.Add(newRow);
                    }


                }
                #endregion
                var listData = new List<string>();
                foreach (DataRow item in dt.Rows)
                {
                    var rowData = item[0].ToString();
                    listData.Add(rowData);
                    var arrys = rowData.Split('/');
                    foreach (var obj in arrys)
                    {
                        if (retData.Any(r => r.Name == obj.ToString()))
                            continue;
                        else
                        {
                            var entity = new AccountantCourseOutputDto()
                            {
                                Id = Guid.NewGuid(),
                                Name = obj.ToString(),
                            };
                            retData.Add(entity);
                        }

                    }

                }

                var childData = listData.Where(r => r.Split('/').Count() > 1);
                foreach (var item in childData)
                {
                    var arrys = item.Split('/');
                    var arrysCount = arrys.Count();
                    for (int i = 0; i < arrysCount - 1; i++)
                    {
                        var childeNode = retData.FirstOrDefault(r => r.Name == arrys[i + 1]);
                        if (!childeNode.Pid.HasValue)
                        {
                            var parentNode = retData.FirstOrDefault(r => r.Name == arrys[i]);
                            childeNode.Pid = parentNode.Id;
                        }
                    }

                }


                var levetOneNodes = retData.Where(r => !r.Pid.HasValue);
                foreach (var item in levetOneNodes)
                {
                    item.Pid = firstNode.Id;
                }
                retData.Add(firstNode);



                ManagerAC(retData.SingleOrDefault(r => !r.Pid.HasValue), retData);


                _repository.Delete(r => r.Id != Guid.Empty);
                foreach (var item in retData)
                {
                    var entity = new AccountantCourse()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        parent_left = item.parent_left,
                        parent_right = item.parent_right,
                        Pid = item.Pid,
                    };
                    await _repository.InsertAsync(entity);
                }
                RefreshCL();
            }





        }






        private void ManagerAC(AccountantCourseOutputDto currentNode, List<AccountantCourseOutputDto> source)
        {
            var childNodes = source.Where(r => r.Pid == currentNode.Id);

            foreach (var item in childNodes)
            {

                var updateLeftNodes = source.Where(r => r.parent_left > currentNode.parent_left);
                foreach (var updateLeftNode in updateLeftNodes)
                {
                    updateLeftNode.parent_left = updateLeftNode.parent_left + 2;
                }
                var updateRightNodes = source.Where(r => r.parent_right > currentNode.parent_left);
                foreach (var updateRightNode in updateRightNodes)
                {
                    updateRightNode.parent_right = updateRightNode.parent_right + 2;
                }

                item.parent_left = currentNode.parent_left + 1;
                item.parent_right = currentNode.parent_left + 2;



            }
            foreach (var item in childNodes)
            {
                ManagerAC(item, source);
            }

        }


        private void RefreshCL()
        {
            var clUrl = _appConfiguration["CLService:clUrl"];
            var requestUrl = $"{clUrl}/refresh ";
            var param = new { tenantid = AbpSession.TenantId };
            Task.Run(() =>
            {
                var result = HttpClientHelper.PostResponse(requestUrl, param);
                Abp.Logging.LogHelper.Logger.Info($"访问财来接口：{requestUrl},返回结果:{result}");
            });
        }
    }
}