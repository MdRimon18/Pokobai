using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class ReviewService
    {
        private readonly IDbConnection _db;


        public ReviewService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<CustomerReview>> Get(long? ReviewId, string? ReviewKey, string ReviewEmail, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ReviewId", ReviewId);
                parameters.Add("@ReviewKey", ReviewKey);
                parameters.Add("@ReviewEmail", ReviewEmail);              
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<CustomerReview>("CustomerReview_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<CustomerReview>();
            }
        }

        public async Task<CustomerReview> GetById(long ReviewId)

        {
            var customerReview = await (Get(ReviewId, null, null, 1, 1));
            return customerReview.FirstOrDefault();
        }

        public async Task<CustomerReview> GetByKey(string ReviewKey)

        {
            var customerReview = await (Get(null, ReviewKey, null, 1, 1));
            return customerReview.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(CustomerReview customerReview)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ReviewId", customerReview.ReviewId);
                parameters.Add("@ReviewKey", customerReview.ReviewKey);
                parameters.Add("@ReviewName", customerReview.ReviewName);
                parameters.Add("@ReviewEmail", customerReview.ReviewEmail);
                parameters.Add("@StarCount", customerReview.StarCount);
                parameters.Add("@ReviewDetails", customerReview.ReviewDetails);
                parameters.Add("@EntryDateTime", customerReview.EntryDateTime);
                parameters.Add("@EntryBy", customerReview.EntryBy);
                parameters.Add("@LastModifyDate", customerReview.LastModifyDate);
                parameters.Add("@LastModifyBy", customerReview.LastModifyBy);
                parameters.Add("@DeletedDate", customerReview.DeletedDate);
                parameters.Add("@DeletedBy", customerReview.DeletedBy);
                parameters.Add("@Status", customerReview.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("CustomerReview_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long ReviewId)
        {
            var customerReview = await (Get(ReviewId, null,null, 1, 1));
            var deleteObj = customerReview.FirstOrDefault();
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
