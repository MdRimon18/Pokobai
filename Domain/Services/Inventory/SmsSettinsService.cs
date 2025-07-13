 
using Dapper;
using System.Data;

using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class SmsSettinsService
    {
        private readonly IDbConnection _db;


        public SmsSettinsService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<SmsSettings>> Get(long? SmsSttngId, string? SmsSttngKey, string ?Title, string? SenderName, string? SmsCode, string? UserName, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@SmsSttngId", SmsSttngId);
                parameters.Add("@SmsSttngKey", SmsSttngKey);
                parameters.Add("@Title", Title);
                parameters.Add("@SenderName", SenderName);
                parameters.Add("@SmsCode", SmsCode);
                parameters.Add("@UserName", UserName);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<SmsSettings>("SmsSettings_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<SmsSettings>();
            }
        }

        public async Task<SmsSettings> GetById(long SmsSttngId)

        {
            var smsSettings = await (Get(SmsSttngId, null,null,null, null, null, 1, 1));
            return smsSettings.FirstOrDefault();
        }

        public async Task<SmsSettings> GetByKey(string SmsSttngKey)

        {
            var smsSettings = await (Get(null, SmsSttngKey,null,null, null, null, 1, 1));
            return smsSettings.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(SmsSettings smsSettings)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@SmsSttngId", smsSettings.SmsSttngId);
                parameters.Add("@SmsSttngKey", smsSettings.SmsSttngKey);
                parameters.Add("@BranchID", smsSettings.BranchId);
                parameters.Add("@SmsTypeId", smsSettings.SmsTypeId);
                parameters.Add("@Title", smsSettings.Title);
                parameters.Add("@SenderName", smsSettings.SenderName);
                parameters.Add("@SmsCode", smsSettings.SmsCode);
                parameters.Add("@ApiUrl", smsSettings.ApiUrl);
                parameters.Add("@UserName", smsSettings.UserName);
                parameters.Add("@Password", smsSettings.Password);
                parameters.Add("@PortNumber", smsSettings.PortNumber);
                parameters.Add("@IsDefault", smsSettings.IsDefault);
                parameters.Add("@Remarks", smsSettings.Remarks);
                parameters.Add("@EntryDateTime", smsSettings.EntryDateTime);
                parameters.Add("@EntryBy", smsSettings.EntryBy);
                parameters.Add("@LastModifyDate", smsSettings.LastModifyDate);
                parameters.Add("@LastModifyBy", smsSettings.LastModifyBy);
                parameters.Add("@DeletedDate", smsSettings.DeletedDate);
                parameters.Add("@DeletedBy", smsSettings.DeletedBy);
                parameters.Add("@Status", smsSettings.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("SmsSettings_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long SmsSttngId)
        {
            var smsSettings = await (Get(SmsSttngId,null,null, null, null, null, 1, 1));
            var deleteObj = smsSettings.FirstOrDefault();
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
