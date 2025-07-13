using Dapper;
using Domain.DbContex;
using Domain.Entity.Settings;
using System.Data;

namespace Domain.Services.Inventory
{
    public class WarehouseService
    {
        private readonly IDbConnection _db;


        public WarehouseService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Warehouse>> Get(long? WarehouseId, string? WarehouseKey, string? WarehouseName, long? LocationId, string?ManagerName,string? PhoneNumber,string? Email,int? Capacity,string? OperatingHours, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@WarehouseId", WarehouseId);
                parameters.Add("@WarehouseKey", WarehouseKey);
                parameters.Add("@WarehouseName", WarehouseName);
                parameters.Add("@LocationId", LocationId);
                parameters.Add("@ManagerName", ManagerName);
                parameters.Add("@PhoneNumber", PhoneNumber);
                parameters.Add("@Email", Email);
                parameters.Add("@Capacity", Capacity);
                parameters.Add("@OperatingHours", OperatingHours);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<Warehouse>("Warehouse_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<Warehouse>();
            }
        }

        public async Task<Warehouse> GetById(long WarehouseId)

        {
            var warehouse = await (Get(WarehouseId,null, null, null, null, null, null, null, null, 1, 1));
            return warehouse.FirstOrDefault();
        }

        public async Task<Warehouse> GetByKey(string WarehouseKey)

        {
            var warehouse = await (Get(null, WarehouseKey, null, null, null, null, null, null, null, 1, 1));
            return warehouse.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(Warehouse warehouse)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@WarehouseId", warehouse.WarehouseId);
                parameters.Add("@WarehouseKey", warehouse.WarehouseKey);
                parameters.Add("@WarehouseName", warehouse.WarehouseName);
                parameters.Add("@LocationId", warehouse.LocationId);
                parameters.Add("@ManagerName", warehouse.ManagerName);
                parameters.Add("@PhoneNumber", warehouse.PhoneNumber);
                parameters.Add("@Email", warehouse.Email);
                parameters.Add("@Capacity", warehouse.Capacity);
                parameters.Add("@OperatingHours", warehouse.OperatingHours);
                parameters.Add("@EntryDateTime", warehouse.EntryDateTime);
                parameters.Add("@EntryBy", warehouse.EntryBy);
                parameters.Add("@LastModifyDate", warehouse.LastModifyDate);
                parameters.Add("@LastModifyBy", warehouse.LastModifyBy);
                parameters.Add("@DeletedDate", warehouse.DeletedDate);
                parameters.Add("@DeletedBy", warehouse.DeletedBy);
                parameters.Add("@Status", warehouse.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("Warehouse_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long WarehouseId)
        {
            var warehouse = await (Get(WarehouseId, null, null, null, null, null, null, null, null, 1, 1));
            var deleteObj = warehouse.FirstOrDefault();
            long DeletedSatatus = 0;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate = DateTime.UtcNow;
                deleteObj.Status = "Deleted";
                DeletedSatatus = await SaveOrUpdate(deleteObj);
            }

            return DeletedSatatus > 0;
        }
    }
}

