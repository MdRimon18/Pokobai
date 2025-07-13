using Dapper;
using Domain.DbContex;
using Domain.Entity;
using Domain.Entity.Inventory;
using Domain.Services.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Inventory
{
    public class ProductVariantService
    {
        private readonly IDbConnection _db;

        public ProductVariantService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();
        }

        public async Task<IEnumerable<ProductVariant>> Get(
            long? productVariantId,
            long? productId,
            string? skuNumber,
            string? color,
            string? size,
            bool? isPrimary,
            string? status,
            int? pageNumber,
            int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ProductVariantId", productVariantId);
                parameters.Add("@ProductId", productId);
                parameters.Add("@SkuNumber", skuNumber);
                parameters.Add("@Color", color);
                parameters.Add("@Size", size);
                parameters.Add("@IsPrimary", isPrimary);
                parameters.Add("@Status", status);
                parameters.Add("@PageNumber", pageNumber ?? 1);
                parameters.Add("@PageSize", pageSize ?? 10);

                return await _db.QueryAsync<ProductVariant>("ProductVariants_Get_SP", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error occurred: {ex.Message}");
                return Enumerable.Empty<ProductVariant>();
            }
        }

        public async Task<ProductVariant> GetById(long productVariantId)
        {
            var productVariants = await Get(productVariantId, null, null, null, null, null, null, 1, 1);
            return productVariants.FirstOrDefault();
        }

        public async Task<long> SaveOrUpdate(ProductVariant productVariant)
        {
            try
            {
                if (productVariant.ProductVariantId > 0)
                {
                    EntityHelper.SetUpdateAuditFields(productVariant);
                }
                else
                {
                    EntityHelper.SetCreateAuditFields(productVariant);
                }

                var parameters = new DynamicParameters();

                parameters.Add("@ProductVariantId", productVariant.ProductVariantId);
                parameters.Add("@ProductId", productVariant.ProductId);
                parameters.Add("@SkuNumber", productVariant.SkuNumber);
                parameters.Add("@Color", productVariant.Color);
                parameters.Add("@Size", productVariant.Size);
                parameters.Add("@Weight", productVariant.Weight);
                parameters.Add("@Height", productVariant.Height);
                parameters.Add("@Width", productVariant.Width);
                parameters.Add("@Length", productVariant.Length);
                parameters.Add("@StockQuantity", productVariant.StockQuantity);
                parameters.Add("@BodyPartName", productVariant.BodyPartName);
                parameters.Add("@ImageUrl", productVariant.ImageUrl);
                parameters.Add("@IsPrimary", productVariant.IsPrimary);
                parameters.Add("@Position", productVariant.Position);
                parameters.Add("@EntryDateTime", productVariant.EntryDateTime);
                ParameterHelper.AddAuditParameters(productVariant, parameters);

                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _db.ExecuteAsync("ProductVariants_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error occurred: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long productVariantId)
        {
            var productVariants = await Get(productVariantId, null, null, null, null, null, null, 1, 1);
            var deleteObj = productVariants.FirstOrDefault();
            long deletedStatus = 0;

            if (deleteObj != null)
            {
                EntityHelper.SetDeleteAuditFields(deleteObj);
                deletedStatus = await SaveOrUpdate(deleteObj);
            }

            return deletedStatus > 0;
        }
    }

}
