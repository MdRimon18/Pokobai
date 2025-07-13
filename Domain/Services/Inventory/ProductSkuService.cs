using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;
namespace Domain.Services.Inventory
{
    public class ProductSkuService
    {
      
            private readonly IDbConnection _db;


            public ProductSkuService(DbConnectionDapper db)
            {
                _db = db.GetDbConnection();

            }
            public async Task<IEnumerable<ProductSKU>> Get(long? ProdSkuId, string? ProdSkuKey, string Sku, long? ColorId,long? SizeId, int? PageNumber, int? PageSize)
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@ProdSkuId", ProdSkuId);
                    parameters.Add("@ProdSkuKey", ProdSkuKey);
                    parameters.Add("@Sku", Sku);
                    parameters.Add("@ColorId", ColorId);
                    parameters.Add("@SizeId", SizeId);
                    parameters.Add("@PageNumber", PageNumber);
                    parameters.Add("@PageSize", PageSize);

                    return await _db.QueryAsync<ProductSKU>("ProductSKUs_Get_SP", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {

                    return Enumerable.Empty<ProductSKU>();
                }
            }

            public async Task<ProductSKU> GetById(long ProdSkuId)

            {
                var productSKU = await (Get(ProdSkuId, null,null,null, null, 1, 1));
                return productSKU.FirstOrDefault();
            }

            public async Task<ProductSKU> GetByKey(string ProdSkuKey)

            {
                var productSKU = await (Get(null,ProdSkuKey, null, null, null, 1, 1));
                return productSKU.FirstOrDefault();
            }


            public async Task<long> SaveOrUpdate(ProductSKU productSKU)
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ProdSkuId", productSKU.ProdSkuId);
                    parameters.Add("@ProdSkuKey", productSKU.ProdSkuKey);
                    parameters.Add("@ProductId", productSKU.ProductId);
                    parameters.Add("@Sku", productSKU.Sku);
                    parameters.Add("@ColorId", productSKU.ColorId);
                    parameters.Add("@SizeId", productSKU.SizeId);
                    parameters.Add("@Weight", productSKU.Weight);
                    parameters.Add("@UnitId", productSKU.UnitId);
                    parameters.Add("@StockQnty", productSKU.StockQnty);
                    parameters.Add("@WarehouseId", productSKU.WarehouseId);
                    parameters.Add("@SupplierId", productSKU.SupplierId);
                    parameters.Add("@BuyingPrice", productSKU.BuyingPrice);
                    parameters.Add("@CurrencyId", productSKU.CurrencyId);
                    parameters.Add("@SellingPrice", productSKU.SellingPrice);
                    parameters.Add("@LeadTimeDays", productSKU.LeadTimeDays);
                    parameters.Add("@RackNumber", productSKU.RackNumber);
                    parameters.Add("@BatchNumber", productSKU.BatchNumber);
                    parameters.Add("@EntryDateTime", productSKU.EntryDateTime);
                    parameters.Add("@EntryBy", productSKU.EntryBy);
                    parameters.Add("@LastModifyDate", productSKU.LastModifyDate);
                    parameters.Add("@LastModifyBy", productSKU.LastModifyBy);
                    parameters.Add("@DeletedDate", productSKU.DeletedDate);
                    parameters.Add("@DeletedBy", productSKU.DeletedBy);
                    parameters.Add("@Status", productSKU.Status);
                    parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await _db.ExecuteAsync("ProductSKUs_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                    return (long)parameters.Get<int>("@SuccessOrFailId");
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log the error)
                    Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                    return 0;
                }
            }

            public async Task<bool> Delete(long ProdSkuId)
            {
                var productSKU = await (Get(ProdSkuId, null, null,null,null, 1, 1));
                var deleteObj = productSKU.FirstOrDefault();
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
