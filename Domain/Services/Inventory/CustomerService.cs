using Dapper;
using System.Data;
using Domain.Entity.Settings;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class CustomerService
    {
        private readonly IDbConnection _db;


        public CustomerService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Customers>> Get(long? CustomerId, string CustomerKey,
     string CustomerName,
     string MobileNo, string Email, string Occupation, string StateName,
     int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CustomerId", CustomerId);
                parameters.Add("@CustomerKey", CustomerKey);
                parameters.Add("@CustomerName", CustomerName);
                parameters.Add("@MobileNo", MobileNo);
                parameters.Add("@Email", Email);
                parameters.Add("@StateName", StateName);
                parameters.Add("@Occupation", Occupation);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);
              

                return await _db.QueryAsync<Customers>("Customers_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Customers>();
            }
        }
        public async Task<Customers> GetById(long CustomerId)

        {
            var customers = await (Get(CustomerId, null, null, null, null, null,null, 1, 1));
            return customers.FirstOrDefault();
        }

        public async Task<Customers> GetByKey(string CustomerKey)

        {
            var customers = await (Get(null, CustomerKey, null, null, null, null,null, 1, 1));
            return customers.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(Customers customers)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CustomerId", customers.CustomerId);
                parameters.Add("@CustomerKey", customers.key);
                parameters.Add("@BranchId", customers.BranchId);
                parameters.Add("@CustomerName", customers.CustomerName);
                parameters.Add("@MobileNo", customers.MobileNo);
                parameters.Add("@Email", customers.Email);
                
                parameters.Add("@CountryId", customers.CountryId);
                parameters.Add("@StateName", customers.StateName);
                parameters.Add("@CustAddrssOne", customers.CustAddrssOne);
                parameters.Add("@CustAddrssTwo", customers.CustAddrssTwo);
                parameters.Add("@Occupation", customers.Occupation);
                parameters.Add("@OfficeName", customers.OfficeName);
                parameters.Add("@CustImgLink", customers.CustImgLink);


                parameters.Add("@EntryDateTime", customers.EntryDateTime);
                parameters.Add("@EntryBy", customers.EntryBy);
                parameters.Add("@LastModifyDate", customers.LastModifyDate);
                parameters.Add("@LastModifyBy", customers.LastModifyBy);
                parameters.Add("@DeletedDate", customers.DeletedDate);
                parameters.Add("@DeletedBy", customers.DeletedBy);
                parameters.Add("@Status", customers.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("Customers_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long CustomerId)
        {
            var customers = await (Get(CustomerId, null, null, null, null, null,null, 1, 1));
            var deleteObj = customers.FirstOrDefault();
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

