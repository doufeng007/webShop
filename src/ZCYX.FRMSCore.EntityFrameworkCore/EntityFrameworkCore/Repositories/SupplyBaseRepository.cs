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
using Supply;
using ZCYX.FRMSCore.Application;
using Dapper;

namespace ZCYX.FRMSCore.EntityFrameworkCore.Repositories
{
    public class SupplyBaseRepository : FRMSCoreRepositoryBase<SupplyBase, Guid>, ISupplyBaseRepository
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;
        private IDynamicRepository _dynamicRepository;
        public SupplyBaseRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider, IDynamicRepository dynamicRepository) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _dynamicRepository = dynamicRepository;
        }
        public async override Task<SupplyBase> InsertAsync(SupplyBase entity)
        {
            var parameter1 = new SqlParameter("@id", entity.Id);
            var parameter2 = new SqlParameter("@Name", entity.Name);
            var parameter3 = new SqlParameter("@Version", entity.Version);
            var parameter4 = new SqlParameter("@Money", entity.Money);
            var parameter5 = new SqlParameter("@Type", entity.Type);
            var parameter6 = new SqlParameter("@UserId", string.IsNullOrWhiteSpace(entity.UserId) ? (object)DBNull.Value : entity.UserId);
            var parameter7 = new SqlParameter("@CreatorUserId", entity.CreatorUserId ?? (object)DBNull.Value);
            var parameter8 = new SqlParameter("@ProductDate", entity.ProductDate ?? (object)DBNull.Value);
            var parameter9 = new SqlParameter("@ExpiryDate", entity.ExpiryDate ?? (object)DBNull.Value);
            var parameter10 = new SqlParameter("@Status", entity.Status);
            var parameter11 = new SqlParameter("@Unit", entity.Unit);
            var parameter12 = new SqlParameter("@PutInDate", entity.PutInDate ?? (object)DBNull.Value);
            object[] pars = { parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, parameter9,parameter10,parameter11, parameter12 };
            var ret = _dbContextProvider.GetDbContext().Set<SupplyBase>().FromSql("EXEC [dbo].[spInsertSupplyMakeCode] @id,@Name,@Version ,@Money ,@Type ,@UserId,@CreatorUserId  ,@ProductDate ,@ExpiryDate,@Status,@Unit,@PutInDate", pars).FirstOrDefault();
            return ret;



        }








    }



}
