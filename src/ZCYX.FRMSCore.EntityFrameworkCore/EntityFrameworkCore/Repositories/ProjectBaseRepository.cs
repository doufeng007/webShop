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

namespace ZCYX.FRMSCore.EntityFrameworkCore.Repositories
{
    public class ProjectBaseRepository : FRMSCoreRepositoryBase<ProjectBase, Guid>, IProjectBaseRepository
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;
        public ProjectBaseRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
        public override Task<ProjectBase> InsertAsync(ProjectBase entity)
        {

            ValidateForPorject.ValidateProjectForCertainSubmite(entity, entity.Id, _dbContextProvider);
            return base.InsertAsync(entity);
        }

        public override Task<ProjectBase> UpdateAsync(ProjectBase entity)
        {
            ValidateForPorject.ValidateProjectForCertainSubmite(entity, entity.Id, _dbContextProvider);
            return base.UpdateAsync(entity);
        }




        public override Task DeleteAsync(ProjectBase entity)
        {
            ValidateForPorject.DeleteProjectSupplement(entity, entity.Id, _dbContextProvider);
            return base.DeleteAsync(entity);
        }

        public List<ValidateForPorjectResult> GetValidateModelResult<T>(T obj)
        {
            if (typeof(T).Equals(typeof(ProjectBase)))
                return GetValidateProjectResult(obj as ProjectBase);
            else
                return ValidateForPorject.GetValidateModelResult(obj);
        }


        public static List<ValidateForPorjectResult> GetValidateProjectResult(ProjectBase obj)
        {
            var ret = new List<ValidateForPorjectResult>();
            Type type = typeof(ProjectBase);
            var tableNameDescription = "";
            var tableNameDes = type.GetCustomAttributes(typeof(TableNameAtribute), true);
            if (tableNameDes.Length > 0)
                tableNameDescription = ((TableNameAtribute)tableNameDes[0]).Name;
            foreach (var f in type.GetProperties())
            {
                var attr = f.GetCustomAttributes(typeof(RequiredColumnAttribute), true);
                if (attr.Length > 0)
                {
                    var colattr = (RequiredColumnAttribute)attr[0];
                    if (colattr.ProjectTypeId == 0 || colattr.ProjectTypeId == obj.AppraisalTypeId)
                    {
                        var col_value = f.GetValue(obj);
                        var col_type = f.PropertyType;
                        if (col_value == null || ValidateForPorject.IsNullByType(col_type, col_value))
                        {
                            ret.Add(new ValidateForPorjectResult() { Category = colattr.CategoryName, FieldName = colattr.Name });
                        }
                    }
                }
            }
            return ret;
        }



        public async Task<TodoCountDtoForSql> GetProjectTodoCount(long? userId, int? isComplete)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@userid", userId ?? (object)DBNull.Value));
            parameters.Add(new SqlParameter("@hasComplete", isComplete ?? (object)DBNull.Value));
            var output_p = new SqlParameter("@projectTodoCount", 0);
            var output_o = new SqlParameter("@oaTodoCount", 0);
            output_p.Direction = ParameterDirection.Output;
            output_o.Direction = ParameterDirection.Output;
            parameters.Add(output_p);
            parameters.Add(output_o);
            SqlParameter[] pars = parameters.ToArray();
            var datas = await _dbContextProvider.GetDbContext().Set<ProjectTodoListDtoNew>().FromSql("exec spGetTodoTasksCount @userid,@hasComplete,@projectTodoCount out,@oaTodoCount out", pars).ToListAsync();
            var totalCount_p = (Int64)pars[2].Value;
            var totalCount_o = (Int64)pars[3].Value;
            var retdata = new TodoCountDtoForSql();
            retdata.ProjectTodoCount = (int)totalCount_p;
            retdata.OACount = (int)totalCount_o;
            return retdata;
        }

    }

    public static class ValidateForPorject
    {

        public static bool ValidateModel<T>(T obj, out string msg)
        {
            Type type = typeof(T);
            foreach (var f in type.GetProperties())
            {
                var attr = f.GetCustomAttributes(typeof(RequiredColumnAttribute), true);
                if (attr.Length > 0)
                {
                    var colattr = (RequiredColumnAttribute)attr[0];
                    var col_value = f.GetValue(obj);
                    var col_type = f.PropertyType;
                    if (col_value == null || IsNullByType(col_type, col_value))
                    {
                        msg = string.Format("{0}不能为空", colattr.Name);
                        return false;
                    }
                }
            }
            msg = "";
            return true;
        }

        public static List<ValidateForPorjectResult> GetValidateModelResult<T>(T obj)
        {
            var ret = new List<ValidateForPorjectResult>();
            Type type = typeof(T);
            var tableNameDescription = "";
            var tableNameDes = type.GetCustomAttributes(typeof(TableNameAtribute), true);
            if (tableNameDes.Length > 0)
                tableNameDescription = ((TableNameAtribute)tableNameDes[0]).Name;
            foreach (var f in type.GetProperties())
            {
                var attr = f.GetCustomAttributes(typeof(RequiredColumnAttribute), true);
                if (attr.Length > 0)
                {
                    var colattr = (RequiredColumnAttribute)attr[0];
                    var col_value = f.GetValue(obj);
                    var col_type = f.PropertyType;
                    if (col_value == null || IsNullByType(col_type, col_value))
                    {
                        ret.Add(new ValidateForPorjectResult() { Category = tableNameDescription, FieldName = colattr.Name });
                    }
                }
            }
            return ret;
        }

        public static List<ValidateForPorjectResult> GetValidateModelResult(CreateOrUpdateProJectBudgetInput entity, int appAppraisalTypeId)
        {

            if (appAppraisalTypeId == 8)
            {
                var item = entity.MapTo<ProjectBudget>();
                return GetValidateModelResult(item);

            }
            else
            {
                return null;
            }

        }

        public static void ValidateProjectForCertainSubmite(ProjectBase obj, Guid projectId, IDbContextProvider<FRMSCoreDbContext> dbContextProvider)
        {
            try
            {
                Type type = typeof(ProjectBase);
                var tableName = "";
                var tableNameDescription = "";
                var table = type.GetCustomAttributes(typeof(TableAttribute), true);
                if (table.Length > 0)
                    tableName = ((TableAttribute)table[0]).Name;
                else
                    return;

                var tableNameDes = type.GetCustomAttributes(typeof(TableNameAtribute), true);
                if (tableNameDes.Length > 0)
                    tableNameDescription = ((TableNameAtribute)tableNameDes[0]).Name;
                var relationId = Guid.Empty;
                foreach (var f in type.GetProperties())
                {
                    if (string.Compare(f.Name, "Id", true) == 0)
                    { relationId = Guid.Parse(f.GetValue(obj).ToString()); break; }

                }
                var query = dbContextProvider.GetDbContext().ProjectSupplement.Where(r => r.ProjectBaseId == projectId && r.RelationId == relationId && r.TableKey == tableName && r.HasSupplement == false);
                foreach (var item in query)
                {
                    foreach (var f in type.GetProperties())
                    {
                        if (f.Name == item.ColumnKey)
                        {
                            var col_value = f.GetValue(obj);
                            var col_type = f.PropertyType;
                            if (col_value != null)
                            {
                                if (!IsNullByType(col_type, col_value))
                                {
                                    item.Value = col_value.ToString();
                                    item.HasSupplement = true;
                                }
                            }
                        }
                    }
                }


                foreach (var f in type.GetProperties())
                {

                    var attr = f.GetCustomAttributes(typeof(RequiredColumnAttribute), true);
                    if (attr.Length > 0)
                    {
                        var colattr = (RequiredColumnAttribute)attr[0];
                        if (colattr.ProjectTypeId == 0 || colattr.ProjectTypeId == obj.AppraisalTypeId)
                        {
                            var col_value = f.GetValue(obj);
                            var col_type = f.PropertyType;
                            if (col_value == null || IsNullByType(col_type, col_value))
                            {
                                var exit_model = dbContextProvider.GetDbContext().ProjectSupplement.Where(r => r.ProjectBaseId == projectId && r.RelationId == relationId && r.TableKey == tableName && r.ColumnKey == f.Name);
                                if (exit_model.Count() == 0)
                                {
                                    var entity = new ProjectSupplement() { Id = Guid.NewGuid(), RelationId = relationId, ColumnKey = f.Name, ColumnName = colattr.Name, HasSupplement = false, ProjectBaseId = projectId, TableKey = tableName, TableName = tableNameDescription };
                                    dbContextProvider.GetDbContext().ProjectSupplement.Add(entity);
                                }
                                else
                                {
                                    foreach (var exit_item in exit_model)
                                    {
                                        exit_item.Value = "";
                                        exit_item.HasSupplement = false;
                                    }
                                }

                            }
                        }
                    }
                }
                //dbContextProvider.GetDbContext().SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static void ValidateModelForCertainSubmite<T>(T obj, Guid projectId, IDbContextProvider<FRMSCoreDbContext> dbContextProvider)
        {
            try
            {
                Type type = typeof(T);
                var tableName = "";
                var tableNameDescription = "";
                var table = type.GetCustomAttributes(typeof(TableAttribute), true);
                if (table.Length > 0)
                    tableName = ((TableAttribute)table[0]).Name;
                else
                    return;

                var tableNameDes = type.GetCustomAttributes(typeof(TableNameAtribute), true);
                if (tableNameDes.Length > 0)
                    tableNameDescription = ((TableNameAtribute)tableNameDes[0]).Name;
                var relationId = Guid.Empty;
                foreach (var f in type.GetProperties())
                {
                    if (string.Compare(f.Name, "Id", true) == 0)
                    { relationId = Guid.Parse(f.GetValue(obj).ToString()); break; }

                }
                var query = dbContextProvider.GetDbContext().ProjectSupplement.Where(r => r.ProjectBaseId == projectId && r.RelationId == relationId && r.TableKey == tableName && r.HasSupplement == false);
                foreach (var item in query)
                {
                    foreach (var f in type.GetProperties())
                    {
                        if (f.Name == item.ColumnKey)
                        {
                            var col_value = f.GetValue(obj);
                            var col_type = f.PropertyType;
                            if (col_value != null)
                            {
                                if (!IsNullByType(col_type, col_value))
                                {
                                    item.Value = col_value.ToString();
                                    item.HasSupplement = true;
                                }
                            }
                        }
                    }
                }


                foreach (var f in type.GetProperties())
                {

                    var attr = f.GetCustomAttributes(typeof(RequiredColumnAttribute), true);
                    if (attr.Length > 0)
                    {
                        var colattr = (RequiredColumnAttribute)attr[0];
                        var col_value = f.GetValue(obj);
                        var col_type = f.PropertyType;
                        if (col_value == null || IsNullByType(col_type, col_value))
                        {
                            var exit_model = dbContextProvider.GetDbContext().ProjectSupplement.Where(r => r.ProjectBaseId == projectId && r.RelationId == relationId && r.TableKey == tableName && r.ColumnKey == f.Name);
                            if (exit_model.Count() == 0)
                            {
                                var entity = new ProjectSupplement() { Id = Guid.NewGuid(), RelationId = relationId, ColumnKey = f.Name, ColumnName = colattr.Name, HasSupplement = false, ProjectBaseId = projectId, TableKey = tableName, TableName = tableNameDescription };
                                dbContextProvider.GetDbContext().ProjectSupplement.Add(entity);
                            }
                            else
                            {
                                foreach (var exit_item in exit_model)
                                {
                                    exit_item.Value = "";
                                    exit_item.HasSupplement = false;
                                }
                            }

                        }
                    }
                }
                //dbContextProvider.GetDbContext().SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static void DeleteProjectSupplement<T>(T obj, Guid projectId, IDbContextProvider<FRMSCoreDbContext> dbContextProvider)
        {
            Type type = typeof(T);
            var tableName = "";
            var table = type.GetCustomAttributes(typeof(TableAttribute), true);
            if (table.Length > 0)
                tableName = ((TableAttribute)table[0]).Name;
            else
                return;
            var relationId = Guid.Empty;
            foreach (var f in type.GetProperties())
            {
                if (string.Compare(f.Name, "Id", true) == 0)
                { relationId = Guid.Parse(f.GetValue(obj).ToString()); break; }

            }
            var query = dbContextProvider.GetDbContext().ProjectSupplement.Where(r => r.ProjectBaseId == projectId && r.RelationId == relationId && r.TableKey == tableName);
            foreach (var deletemodel in query)
            {
                dbContextProvider.GetDbContext().ProjectSupplement.Remove(deletemodel);
            }
        }


        public static bool IsNullByType(Type targetType, object targetvalue)
        {
            if (targetType.IsValueType)
            {
                if (targetType == typeof(Int32))
                {
                    if (Int32.Parse(targetvalue.ToString()) == 0)
                        return true;
                    else
                        return false;
                }
                if (targetType == typeof(Int64))
                {
                    if (Int64.Parse(targetvalue.ToString()) == 0)
                        return true;
                    else
                        return false;
                }
                if (targetType == typeof(Decimal))
                {
                    if (Decimal.Parse(targetvalue.ToString()) == 0)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            else
            {
                if (targetType == typeof(String))
                {
                    if (string.IsNullOrWhiteSpace(targetvalue.ToString()))
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (targetvalue == null)
                        return true;
                    else
                        return false;
                }

            }
        }





    }

}
