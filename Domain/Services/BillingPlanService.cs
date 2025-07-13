using Dapper;
using Domain.DbContex;
using Domain.Entity;
using Domain.Entity.Settings;
using System.Data;

namespace Domain.Services
{
    public class BillingPlanService
    {
        private readonly IDbConnection _db;


        public BillingPlanService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<BillingPlans>> Get(long? BillingPlanId, string? BillingPlanKey,string? BillingPlanName, long? LanguageId,int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BillingPlanId", BillingPlanId);
                parameters.Add("@BillingPlanKey", BillingPlanKey);
                parameters.Add("@BillingPlanName", BillingPlanName);
                parameters.Add("@LanguageId", LanguageId);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<BillingPlans>("BillingPlan_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<BillingPlans>();
            }
        }

        public async Task<BillingPlans> GetById(long UnitId)

        {
            var units = await (Get(UnitId, null, null, null, 1, 1));
            return units.FirstOrDefault();
        }

        public async Task<BillingPlans> GetByKey(string UnitKey)

        {
            var units = await (Get(null, UnitKey, null, null, 1, 1));
            return units.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(BillingPlans billingPlan)
        {
            try
            {
                if (billingPlan.BillingPlanId > 0)
                {
                    EntityHelper.SetUpdateAuditFields(billingPlan);
                }
                else
                {
                    EntityHelper.SetCreateAuditFields(billingPlan);
                }

                var parameters = new DynamicParameters();

                parameters.Add("@BillingPlanId", billingPlan.BillingPlanId);
                parameters.Add("@BillingPlanKey", billingPlan.BillingPlanKey);
                parameters.Add("@LanguageId", billingPlan.LanguageId);
                parameters.Add("@BillingPlanName", billingPlan.BillingPlanName);
                parameters.Add("@EntryDateTime", billingPlan.EntryDateTime);
                parameters.Add("@EntryBy", billingPlan.EntryBy);
                parameters.Add("@LastModifyDate", billingPlan.LastModifyDate);
                parameters.Add("@LastModifyBy", billingPlan.LastModifyBy);
                parameters.Add("@DeletedDate", billingPlan.DeletedDate);
                parameters.Add("@DeletedBy", billingPlan.DeletedBy);
                parameters.Add("@Status", billingPlan.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                
                
                await _db.ExecuteAsync("BillingPlan_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);
 
                return   (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }
 
        public async Task<bool> Delete(long BillingPlanId)
        {
            var billingPlan = await (Get(BillingPlanId, null, null, null, 1, 1));
            var deleteObj = billingPlan.FirstOrDefault();
            long DeletedSatatus = 0;
            if (deleteObj != null)
            {
                EntityHelper.SetDeleteAuditFields(deleteObj);
                DeletedSatatus = await SaveOrUpdate(deleteObj); 
            }

            return DeletedSatatus > 0;
        }
    }
}
