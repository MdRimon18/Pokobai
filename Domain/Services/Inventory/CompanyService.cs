using Dapper;
 
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class CompanyService
    {
        private readonly IDbConnection _db;


        public CompanyService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Company>> Get(long? CompanyId, string CompanyKey, string CompanyName, string CompMobileNo, string? CompanyEmail, long? CountryId,string? SearchValue, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CompanyId", CompanyId);
                parameters.Add("@CompanyKey", CompanyKey);
                parameters.Add("@CompanyName", CompanyName);
                parameters.Add("@CompMobileNo", CompMobileNo);
                parameters.Add("@CompanyEmail", CompanyEmail);
                parameters.Add("@SearchValue", SearchValue);                
                parameters.Add("@CountryId", CountryId);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<Company>("Companies_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<Company>();
            }
        }

        public async Task<Company> GetById(long CompanyId)

        {
            var company = await (Get(CompanyId, null, null,null, null, null, null,1, 1));
            return company.FirstOrDefault();
        }

        public async Task<Company> GetByKey(string CompanyKey)

        {
            var company = await (Get(null, CompanyKey, null, null, null, null,null, 1, 1));
            return company.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(Company company)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CompanyId", company.CompanyId);
                parameters.Add("@CompanyKey", company.CompanyKey);
                parameters.Add("@LanguageId", company.LanguageId);
                parameters.Add("@OwnerOrUserId", company.OwnerOrUserId);
                parameters.Add("@CompanyName", company.CompanyName);
                parameters.Add("@BusinessTypeId", company.BusinessTypeId);
                parameters.Add("@CompMobileNo", company.CompMobileNo);
                parameters.Add("@CompanyEmail", company.CompanyEmail);
                parameters.Add("@CountryId", company.CountryId);
                parameters.Add("@StateName", company.StateName);
                parameters.Add("@CompAddrss", company.CompAddrss);
                parameters.Add("@CurrencyId", company.CurrencyId);
                parameters.Add("@BillingPlanId", company.BillingPlanId);
                parameters.Add("@WorkingDays", company.WorkingDays);
                parameters.Add("@StartToEndTime", company.StartToEndTime);
                parameters.Add("@CompanyLogoImgLink", company.CompanyLogoImgLink);
                parameters.Add("@IsHasBranch", company.IsHasBranch);
                parameters.Add("@EntryDateTime", company.EntryDateTime);
                parameters.Add("@EntryBy", company.EntryBy);
                parameters.Add("@LastModifyDate", company.LastModifyDate);
                parameters.Add("@LastModifyBy", company.LastModifyBy);
                parameters.Add("@DeletedDate", company.DeletedDate);
                parameters.Add("@DeletedBy", company.DeletedBy);
                parameters.Add("@Status", company.Status);
                parameters.Add("@MobileCode", company.MobileCode);
                parameters.Add("@TimeSpent", company.TimeSpent);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("Companies_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long CompanyId)
        {
            var company = await (Get(CompanyId, null, null, null, null, null,null, 1, 1));
            var deleteObj = company.FirstOrDefault();
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
