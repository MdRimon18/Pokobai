using Dapper;
using Domain.DbContex;
using Domain.Entity.Settings;
using System.Data;
 

namespace Domain.Services.Inventory
{
    public class BrandService
    {
        private readonly IDbConnection _db;


        public BrandService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Brands>> Get(long? BrandId, string? BrandKey, string BrandName, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@BrandId", BrandId);
                parameters.Add("@BrandKey", BrandKey);
                parameters.Add("@BrandName", BrandName);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);
                return await _db.QueryAsync<Brands>("Brand_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<Brands>();
            }
        }
        public async Task<Brands> GetById(long BrandId)
        {
            var brands = await (Get(BrandId, null, null, 1, 1));
            return brands.FirstOrDefault();
        }
        public async Task<Brands> GetByKey(string BrandKey)
        {
            var brands = await (Get(null, BrandKey, null, 1, 1));
            return brands.FirstOrDefault();
        }
        public async Task<long> SaveOrUpdate(Brands brands)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BrandId", brands.BrandId);
                parameters.Add("@BrandKey", brands.BrandKey);
                parameters.Add("@BrandName", brands.BrandName);
                parameters.Add("@BrandDetails", brands.BrandDetails);
                parameters.Add("@EntryDateTime", brands.EntryDateTime);
                parameters.Add("@EntryBy", brands.EntryBy);
                parameters.Add("@LastModifyDate", brands.LastModifyDate);
                parameters.Add("@LastModifyBy", brands.LastModifyBy);
                parameters.Add("@DeletedDate", brands.DeletedDate);
                parameters.Add("@DeletedBy", brands.DeletedBy);
                parameters.Add("@Status", brands.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("Brand_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long BrandId)
        {
            var brands = await (Get(BrandId, null, null, 1, 1));
            var deleteObj = brands.FirstOrDefault();
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
