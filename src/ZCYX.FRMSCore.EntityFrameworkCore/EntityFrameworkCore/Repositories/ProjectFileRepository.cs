using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.AutoMapper;
using System.Data;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.EntityFrameworkCore.Repositories
{
    public class ProjectFileRepository : FRMSCoreRepositoryBase<ProjectFile, Guid>, IProjectFileRepository
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;
        public ProjectFileRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
        public override Task<ProjectFile> InsertAsync(ProjectFile entity)
        {

            ValidateProjectFileForCertainSubmite(entity, entity.ProjectBaseId, _dbContextProvider);
            return base.InsertAsync(entity);
        }

        public override Task<ProjectFile> UpdateAsync(ProjectFile entity)
        {
            ValidateProjectFileForCertainSubmite(entity, entity.ProjectBaseId, _dbContextProvider);
            return base.UpdateAsync(entity);
        }


        public void ValidateProjectFileForCertainSubmite(ProjectFile obj, Guid projectId, IDbContextProvider<FRMSCoreDbContext> dbContextProvider)
        {

            if (obj.IsMust)
            {
                var exit_model = dbContextProvider.GetDbContext().ProjectSupplement.FirstOrDefault(r => r.ProjectBaseId == projectId && r.RelationId == obj.Id && r.TableKey == "ProjectFile");
                if (!obj.HasUpload && obj.PaperFileNumber == 0)
                {
                    if (exit_model == null)
                    {
                        var appFileTypeModel = dbContextProvider.GetDbContext().AappraisalFileType.FirstOrDefault(r => r.Id == obj.AappraisalFileType);
                        if (appFileTypeModel == null)
                            throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "送审资料类型数据异常");
                        var entity = new ProjectSupplement() { Id = Guid.NewGuid(), RelationId = obj.Id, ColumnKey = "", ColumnName = appFileTypeModel.Name, HasSupplement = false, ProjectBaseId = projectId, TableKey = "ProjectFile", TableName = "送审资料" };
                        dbContextProvider.GetDbContext().ProjectSupplement.Add(entity);
                    }
                }
                else
                {
                    if (exit_model != null)
                    {
                        exit_model.Value = "";
                        exit_model.HasSupplement = true;
                    }
                }
            }
            else
            {

            }


        }





    }



}
