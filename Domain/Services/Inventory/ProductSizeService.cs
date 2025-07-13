using Dapper;
using System.Data;
using Domain.Entity.Settings;
using System.Data;
using Domain.DbContex;
using Domain.Services.Shared;
using Domain.Entity;

namespace Domain.Services.Inventory
{
  
    
        public class ProductSizeService
        {
            private readonly IDbConnection _db;


            public ProductSizeService(DbConnectionDapper db)
            {
                _db = db.GetDbConnection();

            }
            public async Task<IEnumerable<ProductSze>> Get(long? ProductSizeId, string? ProductSizeKey, long? LanguageId,string? ProductSizeName, int? PageNumber, int? PageSize)
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@ProductSizeId", ProductSizeId);
                    parameters.Add("@ProductSizeKey", ProductSizeKey);
                    parameters.Add("@ProductSizeName", ProductSizeName);
                    parameters.Add("@LanguageId", LanguageId);
                    parameters.Add("@PageNumber", PageNumber);
                    parameters.Add("@PageSize", PageSize);

                    return await _db.QueryAsync<ProductSze>("PrdcuctSize_GetSP", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {

                    return Enumerable.Empty<ProductSze>();
                }
            }

            public async Task<ProductSze> GetById(long ProductSizeId)

            {
                var _ProductSize = await (Get(ProductSizeId, null, null, null, 1, 1));
                return _ProductSize.FirstOrDefault();
            }

            public async Task<ProductSze> GetByKey(string ProductSizeKey)

            {
                var _ProductSize = await (Get(null, ProductSizeKey, null, null, 1, 1));
                return _ProductSize.FirstOrDefault();
            }


            public async Task<long> SaveOrUpdate(ProductSze _ProductSize)
            {
                try
                {
                if (_ProductSize.ProductSizeId > 0)
                {
                    EntityHelper.SetUpdateAuditFields(_ProductSize);
                }
                else
                {
                    EntityHelper.SetCreateAuditFields(_ProductSize);
                }
                var parameters = new DynamicParameters();

                    parameters.Add("@ProductSizeId", _ProductSize.ProductSizeId);
                    parameters.Add("@ProductSizeKey", _ProductSize.ProductSizeKey);
                    parameters.Add("@LanguageId", _ProductSize.LanguageId);
                    parameters.Add("@ProductSizeName", _ProductSize.ProductSizeName);
                   ParameterHelper.AddAuditParameters(_ProductSize, parameters);

                //parameters.Add("@EntryDateTime", _ProductSize.EntryDateTime);
                //    parameters.Add("@EntryBy", _ProductSize.EntryBy);
                //    parameters.Add("@LastModifyDate", _ProductSize.LastModifyDate);
                //    parameters.Add("@LastModifyBy", _ProductSize.LastModifyBy);
                //    parameters.Add("@DeletedDate", _ProductSize.DeletedDate);
                //    parameters.Add("@DeletedBy", _ProductSize.DeletedBy);
                //    parameters.Add("@Status", _ProductSize.Status);
                    parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await _db.ExecuteAsync("Product_Size_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                    return (long)parameters.Get<int>("@SuccessOrFailId");
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log the error)
                    Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                    return 0;
                }
            }

            public async Task<bool> Delete(long ProductSizeId)
            {
                var _ProductSize = await (Get(ProductSizeId, null, null, null, 1, 1));
                var deleteObj = _ProductSize.FirstOrDefault();
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
