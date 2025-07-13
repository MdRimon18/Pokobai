using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;
using Domain.CommonServices;
namespace Domain.Services.Inventory
{
    
        public class ShippingByService
        {
            private readonly IDbConnection _db;


            public ShippingByService(DbConnectionDapper db)
            {
                _db = db.GetDbConnection();

            }
            public async Task<IEnumerable<ShippingBy>> Get(long? ShippingById, string? ShippingByKey, int? LanguageId, string? ShippingByName, int? pagenumber, int? pageSize)
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@ShippingById", ShippingById);
                    parameters.Add("@ShippingByKey", ShippingByKey);

                    parameters.Add("@LanguageId", LanguageId);
                    parameters.Add("@ShippingByName", ShippingByName);

                    parameters.Add("@page_number", pagenumber);
                    parameters.Add("@page_size", pageSize);
                 

                    return await _db.QueryAsync<ShippingBy>("shipping_By_type_Get_SP", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {

                    return Enumerable.Empty<ShippingBy>();
                }
            }

            public async Task<ShippingBy> GetById(long ShippingById)

            {
                var shippingBy = await (Get(ShippingById, null, null, null, 1, 1));
                return shippingBy.FirstOrDefault();
            }

            public async Task<ShippingBy> GetByKey(string ShippingByKey)

            {
                var shippingBy = await (Get(null, ShippingByKey, null, null, 1, 1));
                return shippingBy.FirstOrDefault();
            }


            public async Task<long> Save(ShippingBy shippingBy)
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@ShippingById", dbType: DbType.Int64, direction: ParameterDirection.Output);

                    parameters.Add("@LanguageId", shippingBy.LanguageId);
                    parameters.Add("@ShippingByName", shippingBy.ShippingByName);
                    parameters.Add("@entryDateTime", shippingBy.EntryDateTime);
                    parameters.Add("@entryBy", shippingBy.EntryBy);
                    await _db.ExecuteAsync("Shipping_By_type_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                    return parameters.Get<long>("@ShippingById");
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log the error)
                    Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                    return 0;
                }
            }


            public async Task<bool> Update(ShippingBy shippingBy)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ShippingById", shippingBy.ShippingById);

                parameters.Add("@LanguageId", shippingBy.LanguageId);
                parameters.Add("@ShippingByName", shippingBy.ShippingByName);

                parameters.Add("@lastModifyDate", shippingBy.LastModifyDate);
                parameters.Add("@lastModifyBy", shippingBy.LastModifyBy);
                parameters.Add("@deletedDate", shippingBy.DeletedDate);
                parameters.Add("@DeletedBy", shippingBy.DeletedBy);
                parameters.Add("@Status", shippingBy.Status);
                parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("Shipping_By_Update_SP",
                      parameters, commandType: CommandType.StoredProcedure);

                int success = parameters.Get<int>("@success");
                return success > 0;
            }


            public async Task<bool> Delete(long ShippingById)
            {
                var shippingBy = await (Get(ShippingById, null, null, null, 1, 1));
                var deleteObj = shippingBy.FirstOrDefault();
                bool isDeleted = false;
                if (deleteObj != null)
                {
                    deleteObj.DeletedBy = UserInfo.UserId;
                    deleteObj.DeletedDate = DateTime.UtcNow;
                    deleteObj.Status = "Deleted";
                    isDeleted = await Update(deleteObj);
                }

                return isDeleted;
            }
        }
    }

