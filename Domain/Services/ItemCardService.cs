using Dapper;
using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity;
using Domain.Entity.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ItemCardService
    {
        private readonly IDbConnection _db;


        public ItemCardService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<ItemCart>> GetItemCartAsync(
         int? CartId = null,
         string? BrowserId=null,
         long? CustomerId = null,
         long? ProductId = null,
         string? Sku = null)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CartId", CartId);
                parameters.Add("@BrowserId", BrowserId);
                parameters.Add("@CustomerId", CustomerId);
                parameters.Add("@ProductId", ProductId);
                parameters.Add("@Sku", Sku);

                // Execute stored procedure and fetch data
                return await _db.QueryAsync<ItemCart>(
                    "ItemCart_Get_SP",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Enumerable.Empty<ItemCart>();
            }
        }
        public async Task<long> SaveOrUpdateItemCart(ItemCart itemCart)
        {
            try
            {
                
                //if (itemCart.CartId > 0)
                //{
                //    EntityHelper.SetUpdateAuditFields(itemCart);
                //}
                //else
                //{
                   // EntityHelper.SetCreateAuditFields(itemCart);
                //}

                var parameters = new DynamicParameters();

                // Map the ItemCart properties to the stored procedure parameters
                parameters.Add("@CartId", itemCart.CartId);
                parameters.Add("@ProductId", itemCart.ProductId);
                parameters.Add("@Sku", itemCart.Sku);
                parameters.Add("@BrowserId",itemCart.BrowserId);
                parameters.Add("@CustomerId", UserInfo.UserId);
                parameters.Add("@Quantity", itemCart.Quantity);
                parameters.Add("@Price", itemCart.Price);
                parameters.Add("@Discount", itemCart.Discount);
                parameters.Add("@Vat", itemCart.Vat);
                if (itemCart.CartId > 0)
                {
                    parameters.Add("@LastModifyDate", DateTime.UtcNow);
                   
                }
                else
                {
                    parameters.Add("@CreatedAt", DateTime.UtcNow);
                   
                   
                }
               
                //parameters.Add("@CreatedAt", itemCart.CreatedAt);
                //parameters.Add("@LastModifyDate", itemCart.LastModifyDate);

                // Add an output parameter for SuccessOrFailId
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Execute the stored procedure
                await _db.ExecuteAsync("ItemCart_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                // Retrieve the output parameter value
                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                throw;
                
            }
        }
        //give corresponding parameters
        public async Task<bool> DeleteItemCart(int? cartId = null,string? browserId = null,int? customerId = null, int? productId = null, string sku = null)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CartId", cartId);
                parameters.Add("@BrowserId", browserId);
                parameters.Add("@CustomerId", customerId);
                parameters.Add("@ProductId", productId);
                parameters.Add("@Sku", sku);
                parameters.Add("@IsSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);

              
                await _db.ExecuteAsync("ItemCart_Delete_SP", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<bool>("@IsSuccess");
            }
            catch (Exception ex)
            {
               
                return false; 
            }
        }


    }
}
