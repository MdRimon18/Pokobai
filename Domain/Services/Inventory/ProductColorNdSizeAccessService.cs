using Dapper;
using System.Data;
using Domain.Entity.Settings;
using System.Data;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class ProductColorNdSizeAccessService
    {
        private readonly IDbConnection _db;


        public ProductColorNdSizeAccessService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<ProductColorNdSizeAccess>> Get(long? ProdcsId, string? ProdcsKey, long? ProductId, long? ColorId, long? SizeId ,int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ProdcsId", ProdcsId);
                parameters.Add("@ProdcsKey", ProdcsKey);
                parameters.Add("@ProductId", ProductId);
                parameters.Add("@ColorId", ColorId);
                parameters.Add("@SizeId", SizeId);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<ProductColorNdSizeAccess>("ProdtClrNSizAcs_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<ProductColorNdSizeAccess>();
            }
        }

        public async Task<ProductColorNdSizeAccess> GetById(long? ProdcsId)

        {
            var productColorNdSizeAccess = await (Get(ProdcsId,null, null, null, null, 1, 1));
            return productColorNdSizeAccess.FirstOrDefault();
        }

        public async Task<ProductColorNdSizeAccess> GetByKey(string ProdcsKey)

        {
            var productColorNdSizeAccess = await (Get(null, ProdcsKey, null,null, null, 1, 1));
            return productColorNdSizeAccess.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(ProductColorNdSizeAccess productColorNdSizeAccess)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ProdcsId", productColorNdSizeAccess.ProdcsId);
                parameters.Add("@ProdcsKey", productColorNdSizeAccess.ProdcsKey);
                parameters.Add("@ProductId", productColorNdSizeAccess.ProductId);
                parameters.Add("@ColorId", productColorNdSizeAccess.ColorId);
                parameters.Add("@SizeId", productColorNdSizeAccess.SizeId);
                parameters.Add("@Weight", productColorNdSizeAccess.Weight);
                parameters.Add("@UnitId", productColorNdSizeAccess.UnitId);
                parameters.Add("@Qnty", productColorNdSizeAccess.Qnty);
                parameters.Add("@EntryDateTime", productColorNdSizeAccess.EntryDateTime);
                parameters.Add("@EntryBy", productColorNdSizeAccess.EntryBy);
                parameters.Add("@LastModifyDate", productColorNdSizeAccess.LastModifyDate);
                parameters.Add("@LastModifyBy", productColorNdSizeAccess.LastModifyBy);
                parameters.Add("@DeletedDate", productColorNdSizeAccess.DeletedDate);
                parameters.Add("@DeletedBy", productColorNdSizeAccess.DeletedBy);
                parameters.Add("@Status", productColorNdSizeAccess.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("ProdtClrNSizAcs_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long? ProdcsId)
        {
            var productColorNdSizeAccess = await (Get(ProdcsId, null, null,null, null, 1, 1));
            var deleteObj = productColorNdSizeAccess.FirstOrDefault();
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

