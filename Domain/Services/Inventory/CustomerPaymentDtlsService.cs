using Dapper;
 
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class CustomerPaymentDtlsService
    {
       
            private readonly IDbConnection _db;


            public CustomerPaymentDtlsService(DbConnectionDapper db)
            {
                _db = db.GetDbConnection();

            }
            public async Task<IEnumerable<CustomerPaymentDtls>> Get(long? CustomerPaymentId, string? CustomerPaymentKey, long? BranchId, long? InvoiceId, long? CustomerId, int? PageNumber, int? PageSize)
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CustomerPaymentId", CustomerPaymentId);
                    parameters.Add("@CustomerPaymentKey", CustomerPaymentKey);
                    parameters.Add("@BranchId", BranchId);
                    parameters.Add("@InvoiceId", InvoiceId); 
                    parameters.Add("@CustomerId", CustomerId);
                    parameters.Add("@PageNumber", PageNumber);
                    parameters.Add("@PageSize", PageSize);

                    return await _db.QueryAsync<CustomerPaymentDtls>("CustomerPaymentDtls_Get_SP", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {

                    return Enumerable.Empty<CustomerPaymentDtls>();
                }
            }

            public async Task<CustomerPaymentDtls> GetById(long CustomerPaymentId)

            {
                var customerPaymentDtls = await (Get(CustomerPaymentId, null, null,null,null, 1, 1));
                return customerPaymentDtls.FirstOrDefault();
            }

            public async Task<CustomerPaymentDtls> GetByKey(string CustomerPaymentKey)

            {
                var customerPaymentDtls = await (Get(null, CustomerPaymentKey, null,null, null, 1, 1));
                return customerPaymentDtls.FirstOrDefault();
            }


            public async Task<long> SaveOrUpdate(CustomerPaymentDtls customerPaymentDtls)
            {
                try
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@CustomerPaymentId", customerPaymentDtls.CustomerPaymentId);
                    parameters.Add("@CustomerPaymentKey", customerPaymentDtls.CustomerPaymentKey);
                    parameters.Add("@BranchId", customerPaymentDtls.BranchId);
                    parameters.Add("@InvoiceId", customerPaymentDtls.InvoiceId);
                    parameters.Add("@CustomerId", customerPaymentDtls.CustomerId);
                    parameters.Add("@PaymentDate", customerPaymentDtls.PaymentDate);
                    parameters.Add("@PaidAmount", customerPaymentDtls.PaidAmount);
                    parameters.Add("@EntryDateTime", customerPaymentDtls.EntryDateTime);
                    parameters.Add("@EntryBy", customerPaymentDtls.EntryBy);
                    parameters.Add("@LastModifyDate", customerPaymentDtls.LastModifyDate);
                    parameters.Add("@LastModifyBy", customerPaymentDtls.LastModifyBy);
                    parameters.Add("@DeletedDate", customerPaymentDtls.DeletedDate);
                    parameters.Add("@DeletedBy", customerPaymentDtls.DeletedBy);
                    parameters.Add("@Status", customerPaymentDtls.Status);
                    parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await _db.ExecuteAsync("CustomerPaymentDtlsr_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                    return (long)parameters.Get<int>("@SuccessOrFailId");
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log the error)
                    Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                    return 0;
                }
            }

            public async Task<bool> Delete(long CustomerPaymentId)
            {
                var customerPaymentDtls = await (Get(CustomerPaymentId, null, null,null, null, 1, 1));
                var deleteObj = customerPaymentDtls.FirstOrDefault();
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
