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
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Hosting;
using NPOI.SS.Util;

namespace HR
{
    public class DailyAppService : FRMSCoreAppServiceBase, IDailyAppService
    { 
        private readonly IRepository<Daily, Guid> _repository;

        private IHostingEnvironment hostingEnv;
        public DailyAppService(IRepository<Daily, Guid> repository, IHostingEnvironment env

        )
        {
            this._repository = repository;

            this.hostingEnv = env;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<DailyListOutputDto>> GetList(GetDailyListInput input)
        {
            
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted )
                        select new DailyListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Department = a.Department,
                            Personnel = a.Personnel,
                            Content = a.Content,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            OverState = a.OverState,
                            Note = a.Note
							
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.Department.Contains(input.SearchKey) || x.Personnel.Contains(input.SearchKey) || x.Content.Contains(input.SearchKey) || x.OverState.Contains(input.SearchKey) && x.Note.Contains(input.SearchKey));
            if (input.StartTime.HasValue)
                query = query.Where(x=>x.StartTime.Value.Date==input.StartTime.Value.Date);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<DailyListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<DailyOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<DailyOutputDto>();
		}
        
        public string GetOutput(DateTime? datetime)
        {
            string filePath = hostingEnv.WebRootPath + $@"\Files\yanfabu.xlsx";
            var file =new  FileInfo(filePath);
            NPOI.SS.UserModel.IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook(file);
            
            NPOI.SS.UserModel.ISheet sheet = workbook.GetSheetAt(0);

   

            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted) select a;
            var date = DateTime.Now;
            if (datetime.HasValue)
                date = datetime.Value.Date;
            query = query.Where(x => x.StartTime.Value.Date == date.Date);
            var list = query.OrderBy(x=>x.Department).ThenBy(x=>x.Personnel).ToList();
            int count = 1;
            int num = 2;
            int iCount = list.Count;
            for (int i = 0; i < iCount - 1; i++)
            {
                sheet.CopyRow(2, 2 + 1 + i);
            }
            var dlist = new List<DailyGroupBy>();
            var plist = new List<DailyGroupBy>();
            for (int i = 0; i <list.Count ; i++)
            {
                var model = list[i];
                if (dlist.Count(x => x.Name == model.Department) > 0)
                {
                    dlist.First(x => x.Name == model.Department).Count++;
                }
                else {
                    dlist.Add(new DailyGroupBy() { Name=model.Department,Count=1});
                }
                if (plist.Count(x => x.Name == model.Personnel) > 0)
                {
                    plist.First(x => x.Name == model.Personnel).Count++;
                }
                else {
                    plist.Add(new DailyGroupBy() { Name=model.Personnel, Count=1});
                }
                IRow row = sheet.GetRow(num);
                row.RowStyle = sheet.GetRow(2).RowStyle;
                row.Height = sheet.GetRow(2).Height;
                row.GetCell(0).SetCellValue(count);
                row.GetCell(1).SetCellValue(model.Department);
                row.GetCell(2).SetCellValue(model.Personnel);
                row.GetCell(3).SetCellValue(model.Content);
                row.GetCell(4).SetCellValue(model.StartTime.Value.ToString("yyyy-MM-dd"));
                row.GetCell(5).SetCellValue(model.EndTime.Value.ToString("yyyy-MM-dd"));
                row.GetCell(6).SetCellValue(model.OverState);
                row.GetCell(7).SetCellValue(model.Note);
                count++;
                num++;
            }
            int pint = 2;
            foreach (var item in plist)
            {
                var s = pint + item.Count - 1;
                sheet.AddMergedRegion(new CellRangeAddress(pint, s, 2, 2));
                pint += item.Count;
            }
            int dint =2;
            foreach (var item in dlist)
            {
                var s = dint + item.Count - 1;
                sheet.AddMergedRegion(new CellRangeAddress(dint, s, 1, 1));
                dint += item.Count;
            }

            ICellStyle cellstyle = workbook.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;
            cellstyle.WrapText = true;


            byte[] buffer = new byte[1024 * 5];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }
            var id = date.ToString("yyyy-MM-dd");
            string fileName = hostingEnv.WebRootPath + $@"\Files\upload\"+id+ ".xlsx";
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
                buffer = null;
            }
            return "/api/AbpFile/ShowFile?filename=" + id;
        }
        /// <summary>
        /// 添加一个Daily
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateDailyInput input)
        {
            var newmodel = new Daily()
            {
                Id = Guid.NewGuid(),
                Department = input.Department,
                Personnel = input.Personnel,
                Content = input.Content,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                OverState = input.OverState,
                Note = input.Note
            };
            await _repository.InsertAsync(newmodel);
        }

		/// <summary>
        /// 修改一个Daily
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateDailyInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.Id = input.Id;
			   dbmodel.Department = input.Department;
			   dbmodel.Personnel = input.Personnel;
			   dbmodel.Content = input.Content;
			   dbmodel.StartTime = input.StartTime;
			   dbmodel.EndTime = input.EndTime;
			   dbmodel.OverState = input.OverState;
			   dbmodel.Note = input.Note;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		
		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}