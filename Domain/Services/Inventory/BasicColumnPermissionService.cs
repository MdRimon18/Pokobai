using Dapper;
using Domain.DbContex;
using Domain.Entity.Settings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
 
using System.Data;
 

namespace Domain.Services.Inventory
{
    public class BasicColumnPermissionService
    {
        private readonly IDbConnection _db;


        public BasicColumnPermissionService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<ColumPermission>> Get(long? ColumnPermssinId, string? ColumnPermssinKey, long? BranchId, long? PageId, string ColumnName,
           int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ColumnPermssinId", ColumnPermssinId);
                parameters.Add("@ColumnPermssinKey", ColumnPermssinKey);
                parameters.Add("@BranchId", BranchId);
                parameters.Add("@PageId", PageId);
                parameters.Add("@ColumnName", ColumnName);              
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<ColumPermission>("ColumnPermission_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<ColumPermission>();
            }
        }

        public async Task<ColumPermission> GetById(long ColumnPermssinId)

        {
            var _columPermission = await (Get(ColumnPermssinId, null,null,null, null, 1, 1));
            return _columPermission.FirstOrDefault();
        }

        public async Task<ColumPermission> GetByKey(string ColumnPermssinKey)

        {
            var _columPermission = await (Get(null, ColumnPermssinKey,null, null, null, 1, 1));
            return _columPermission.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(ColumPermission _columPermission)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ColumnPermssinId", _columPermission.ColumnPermssinId);
                parameters.Add("@ColumnPermssinKey", _columPermission.ColumnPermssinKey);
                parameters.Add("@BranchId", _columPermission.BranchId);
                parameters.Add("@PageId", _columPermission.PageId);
                parameters.Add("@ColumnName", _columPermission.ColumnName);
                parameters.Add("@ColumnProprtyName", _columPermission.ColumnProprtyName);
                parameters.Add("@ColumnProprtyDataType", _columPermission.ColumnProprtyDataType);
                parameters.Add("@IsSearchable", _columPermission.IsSearchable);
                parameters.Add("@ColumnPosition", _columPermission.ColumnPosition);
                parameters.Add("@IsShow", _columPermission.IsShow);
                parameters.Add("@EntryDateTime", _columPermission.EntryDateTime);
                parameters.Add("@EntryBy", _columPermission.EntryBy);
                parameters.Add("@LastModifyDate", _columPermission.LastModifyDate);
                parameters.Add("@LastModifyBy", _columPermission.LastModifyBy);
                parameters.Add("@DeletedDate", _columPermission.DeletedDate);
                parameters.Add("@DeletedBy", _columPermission.DeletedBy);
                parameters.Add("@Status", _columPermission.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("ColumnPermission_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long ColumnPermssinId)
        {
            var _columPermission = await (Get(ColumnPermssinId,null, null, null, null, 1, 1));
            var deleteObj = _columPermission.FirstOrDefault();
            long DeletedSatatus = 0;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate =DateTime.UtcNow;
                deleteObj.Status = "Deleted";
                DeletedSatatus = await SaveOrUpdate(deleteObj);
            }

            return DeletedSatatus > 0;
        }
    }
}
