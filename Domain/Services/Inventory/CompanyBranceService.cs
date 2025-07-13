using Dapper;
using System.Data;
using Domain.Entity.Settings;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Data;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class CompanyBranceService
    {
        private readonly IDbConnection _db;


        public CompanyBranceService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<CompanyBranch>> Get(long? BranchId, string BranchKey, string CompanyId, string? SearchValue, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BranchId", BranchId);
                parameters.Add("@BranchKey", BranchKey);
                parameters.Add("@CompanyId", CompanyId);            
                parameters.Add("@SearchValue", SearchValue);                
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<CompanyBranch>("CompanyBranch_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<CompanyBranch>();
            }
        }

        public async Task<CompanyBranch> GetById(long BranchId)

        {
            var companyBrance = await (Get(BranchId, null, null, null, 1, 1));
            return companyBrance.FirstOrDefault();
        }

        public async Task<CompanyBranch> GetByKey(string BranchKey)

        {
            var companyBrance = await (Get(null, BranchKey,null, null, 1, 1));
            return companyBrance.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(CompanyBranch companyBrance)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BranchId", companyBrance.BranchId);
                parameters.Add("@BranchKey", companyBrance.BranchKey);
                parameters.Add("@CompanyId", companyBrance.CompanyId);
                parameters.Add("@BranchName", companyBrance.BranchName);
                parameters.Add("@MobileNo", companyBrance.MobileNo);
                parameters.Add("@Email", companyBrance.Email);
                parameters.Add("@StateName", companyBrance.StateName);
                parameters.Add("@Address", companyBrance.Address);
                parameters.Add("@BrnchManagerName", companyBrance.BrnchManagerName);
                parameters.Add("@ManagerMobile", companyBrance.ManagerMobile);
                parameters.Add("@BranchImgLink", companyBrance.BranchImgLink);               
                parameters.Add("@EntryDateTime", companyBrance.EntryDateTime);
                parameters.Add("@EntryBy", companyBrance.EntryBy);
                parameters.Add("@LastModifyDate", companyBrance.LastModifyDate);
                parameters.Add("@LastModifyBy", companyBrance.LastModifyBy);
                parameters.Add("@DeletedDate", companyBrance.DeletedDate);
                parameters.Add("@DeletedBy", companyBrance.DeletedBy);
                parameters.Add("@Status", companyBrance.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("CompanyBranch_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long BranchId)
        {
            var companyBrance = await (Get(BranchId, null, null, null, 1, 1));
            var deleteObj = companyBrance.FirstOrDefault();
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
