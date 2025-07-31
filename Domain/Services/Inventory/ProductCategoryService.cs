using Dapper;
using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity;
using Domain.Entity.Settings;
using System.Data;

namespace Domain.Services.Inventory
{
    public class ProductCategoryService
    {
        private readonly IDbConnection _db;


        public ProductCategoryService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<ProductCategories>> Get(long? CompanyId,long? ProdCtgId, string? ProdCtgKey, long? BranchId, string? ProdCtgName, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ProdCtgId", ProdCtgId);
                parameters.Add("@ProdCtgKey", ProdCtgKey);
                parameters.Add("@CompanyId", CompanyId);
                parameters.Add("@BranchId", BranchId);
                parameters.Add("@ProdCtgName", ProdCtgName);
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);

                return await _db.QueryAsync<ProductCategories>("Product_Ctg_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<ProductCategories>();
            }
        }
        public async Task<List<ProductCategories>> FetchModelList(long CompanyId)
        {
            var list = await Get(
                CompanyId,
                null,
                null,
                null,
                null,
                GlobalPageConfig.PageNumber,
                GlobalPageConfig.PageSize
            );

            return list.ToList(); // Convert and return as List<Unit>
        }
        public async Task<ProductCategories> GetById(long companyId,long ProdCtgId)

        {
            var productCategories = await (Get(companyId, ProdCtgId, null, null, null, 1, 1));
            return productCategories.FirstOrDefault();
        }

        public async Task<ProductCategories> GetByKey(long companyId,string ProdCtgKey)

        {
            var productCategories = await (Get(companyId, null, ProdCtgKey, null, null, 1, 1));
            return productCategories.FirstOrDefault();
        }


        public async Task<long> Save(ProductCategories productCategories)
        {
            try
            {
                EntityHelper.SetCreateAuditFields(productCategories);

               var parameters = new DynamicParameters();

                parameters.Add("@ProdCtgId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("@CompanyId",productCategories.CompanyId);
                parameters.Add("@branchId", productCategories.BranchId);
                parameters.Add("@ProdCtgName", productCategories.ProdCtgName);
                parameters.Add("@ImageUrl", productCategories.ImageUrl);
                parameters.Add("@entryDateTime", productCategories.EntryDateTime);
                parameters.Add("@entryBy", productCategories.EntryBy);
                await _db.ExecuteAsync("Product_Ctg_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@ProdCtgId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(ProductCategories productCategories)
        {
            EntityHelper.SetUpdateAuditFields(productCategories);
            var parameters = new DynamicParameters();
            parameters.Add("@ProdCtgId", productCategories.ProdCtgId);
            parameters.Add("@CompanyId", productCategories.CompanyId);
            parameters.Add("@branchId", productCategories.BranchId);
            parameters.Add("@ProdCtgName", productCategories.ProdCtgName);
            parameters.Add("@ImageUrl", productCategories.ImageUrl);
            parameters.Add("@Status", productCategories.Status);
            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _db.ExecuteAsync("Product_Ctg_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }


        public async Task<bool> Delete(long companyId,long ProdCtgId)
        {
            var productCategories = await (Get(companyId, ProdCtgId, null, null, null, 1, 1));
            var deleteObj = productCategories.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate = DateTime.UtcNow;
                deleteObj.Status = "Deleted";
                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
    }
}
