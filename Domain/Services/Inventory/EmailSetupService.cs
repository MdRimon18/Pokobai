using Dapper;
 
using System.Data;
using Domain.Entity.Settings;
using Domain.CommonServices;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class EmailSetupService
    {
        private readonly IDbConnection _db;


        public EmailSetupService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<EmailSetup>> Get(int? EmailSetupId, string? EmailSetupkey, string FromEmail, string FromName, string UserName, string Password, string BaseUrl, long? PortNumber, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@EmailSetupId", EmailSetupId);
                parameters.Add("@EmailSetupkey", EmailSetupkey);
                parameters.Add("@FromEmail", FromEmail);
                parameters.Add("@FromName", FromName);
                parameters.Add("@UserName", UserName);
                parameters.Add("@Password", Password);
                parameters.Add("@BaseUrl", BaseUrl);
              

                parameters.Add("@PortNumber", PortNumber);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<EmailSetup>("EmailSetup_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<EmailSetup>();
            }
        }

        public async Task<EmailSetup> GetById(int EmailSetupId)

        {
            var emailSetup = await (Get(EmailSetupId, null, null, null,null,null, null,null, 1, 1));
            return emailSetup.FirstOrDefault();
        }

        public async Task<EmailSetup> GetByKey(string EmailSetupkey)

        {
            var emailSetup = await (Get(null, EmailSetupkey, null, null, null,null, null, null, 1, 1));
            return emailSetup.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(EmailSetup emailSetup)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@EmailSetupId", emailSetup.EmailSetupId);
                parameters.Add("@EmailSetupkey",emailSetup.EmailSetupkey);
                parameters.Add("@BranchId", emailSetup.BranchId);
                parameters.Add("@FromEmail", emailSetup.FromEmail);
                parameters.Add("@FromName", emailSetup.FromName);
                parameters.Add("@UserName", emailSetup.UserName);
                parameters.Add("@Password", emailSetup.Password);
                parameters.Add("@BaseUrl", emailSetup.BaseUrl);
                parameters.Add("@ApiKey", emailSetup.ApiKey);
                parameters.Add("@PortNumber", emailSetup.PortNumber);
                parameters.Add("@IsDefault", emailSetup.IsDefault);
                parameters.Add("@EntryDateTime", emailSetup.EntryDateTime);
                parameters.Add("@EntryBy", emailSetup.EntryBy);
                parameters.Add("@LastModifyDate", emailSetup.LastModifyDate);
                parameters.Add("@LastModifyBy", emailSetup.LastModifyBy);
                parameters.Add("@DeletedDate", emailSetup.DeletedDate);
                parameters.Add("@DeletedBy", emailSetup.DeletedBy);
                parameters.Add("@Status", emailSetup.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("EmailSetup_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(int EmailSetupId)
        {
            var emailSetup = await (Get(EmailSetupId,null,null,null,null,null,null,null, 1, 1));
            var deleteObj = emailSetup.FirstOrDefault();
            long DeletedSatatus = 0;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate = DateTime.UtcNow;
                deleteObj.DeletedBy = UserInfo.UserId;
                deleteObj.Status = "Deleted";
                DeletedSatatus = await SaveOrUpdate(deleteObj);
            }

            return DeletedSatatus > 0;
        }
    }
}
