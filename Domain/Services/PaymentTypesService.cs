using Dapper;
using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity.Settings;
using System.Data;



namespace Domain.Services
{
    public class PaymentTypesService
    {
        private readonly IDbConnection _db;


        public PaymentTypesService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<PaymentTypes>> Get(long? PaymentTypeId, string? PaymentTypeKey, string PaymentTypesName, long? LanguageId, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PaymentTypeId", PaymentTypeId);
                parameters.Add("@PaymentTypeKey", PaymentTypeKey);
                parameters.Add("@PaymentTypesName", PaymentTypesName);
                parameters.Add("@LanguageId", LanguageId);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<PaymentTypes>("PaymentTypes_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<PaymentTypes>();
            }
        }

        public async Task<PaymentTypes> GetById(long PaymentTypeId)

        {
            var paymentTypes = await (Get(PaymentTypeId, null, null, null, 1, 1));
            return paymentTypes.FirstOrDefault();
        }

        public async Task<PaymentTypes> GetByKey(string PaymentTypeKey)

        {
            var paymentTypes = await (Get(null, PaymentTypeKey, null, null, 1, 1));
            return paymentTypes.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(PaymentTypes paymentTypes)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PaymentTypeId", paymentTypes.PaymentTypeId);
                parameters.Add("@PaymentTypeKey", paymentTypes.PaymentTypeKey);
                parameters.Add("@LanguageId", paymentTypes.LanguageId);
                parameters.Add("@PaymentTypesName", paymentTypes.PaymentTypesName);
                parameters.Add("@EntryDateTime", paymentTypes.EntryDateTime);
                parameters.Add("@EntryBy", paymentTypes.EntryBy);
                parameters.Add("@LastModifyDate", paymentTypes.LastModifyDate);
                parameters.Add("@LastModifyBy", paymentTypes.LastModifyBy);
                parameters.Add("@DeletedDate", paymentTypes.DeletedDate);
                parameters.Add("@DeletedBy", paymentTypes.DeletedBy);
                parameters.Add("@Status", paymentTypes.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("PaymentTypes_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long PaymentTypeId)
        {
            var paymentTypes = await (Get(PaymentTypeId, null, null, null, 1, 1));
            var deleteObj = paymentTypes.FirstOrDefault();
            long DeletedSatatus = 0;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate =DateTime.UtcNow;
                deleteObj.DeletedBy = UserInfo.UserId;
                deleteObj.Status = "Deleted";
                DeletedSatatus = await SaveOrUpdate(deleteObj);
            }

            return DeletedSatatus > 0;
        }
    }
}
