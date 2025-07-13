using Dapper;
using Domain.DbContex;
using Domain.Entity.Settings;
using System.Data;
 


namespace Domain.Services.Inventory
{
    public class SupplierService
    {
        private readonly IDbConnection _db;


        public SupplierService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Suppliers>> Get(long? SupplierId, string SupplierKey, string SupplrName, string MobileNo, string Email, string SuppOrgnznName, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@SupplierId", SupplierId);
                parameters.Add("@SupplierKey", SupplierKey);
                parameters.Add("@SupplrName", SupplrName);
                parameters.Add("@MobileNo", MobileNo);
                parameters.Add("@Email", Email);
                parameters.Add("@SuppOrgnznName", SuppOrgnznName);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<Suppliers>("Suppliers_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<Suppliers>();
            }
        }

        public async Task<Suppliers> GetById(long SupplierId)

        {
            var suppliers = await (Get(SupplierId, null,null,null, null, null, 1, 1));
            return suppliers.FirstOrDefault();
        }

        public async Task<Suppliers> GetByKey(string SupplierKey)

        {
            var suppliers = await (Get(null, SupplierKey,null,null, null, null, 1, 1));
            return suppliers.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(Suppliers suppliers)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@SupplierId", suppliers.SupplierId);
                parameters.Add("@SupplierKey", suppliers.key);
                parameters.Add("@BranchId", suppliers.BranchId);
                parameters.Add("@SupplrName", suppliers.SupplrName);
                parameters.Add("@MobileNo", suppliers.MobileNo);
                parameters.Add("@Email", suppliers.Email);
                parameters.Add("@SuppOrgnznName", suppliers.SuppOrgnznName);
                parameters.Add("@CountryId", suppliers.CountryId);
                parameters.Add("@StateName", suppliers.StateName);
                parameters.Add("@BusinessTypeId", suppliers.BusinessTypeId);
                parameters.Add("@SupplrAddrssOne", suppliers.SupplrAddrssOne);
                parameters.Add("@SupplrAddrssTwo", suppliers.SupplrAddrssTwo);
                parameters.Add("@DeliveryOffDay", suppliers.DeliveryOffDay);
                parameters.Add("@SupplrImgLink", suppliers.SupplrImgLink);


                parameters.Add("@EntryDateTime", suppliers.EntryDateTime);
                parameters.Add("@EntryBy", suppliers.EntryBy);
                parameters.Add("@LastModifyDate", suppliers.LastModifyDate);
                parameters.Add("@LastModifyBy", suppliers.LastModifyBy);
                parameters.Add("@DeletedDate", suppliers.DeletedDate);
                parameters.Add("@DeletedBy", suppliers.DeletedBy);
                parameters.Add("@Status", suppliers.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("Suppliers_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long SupplierId)
        {
            var suppliers = await (Get(SupplierId, null, null,null,null, null, 1, 1));
            var deleteObj = suppliers.FirstOrDefault();
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
