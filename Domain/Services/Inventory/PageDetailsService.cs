using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class PageDetailsService
    {
        private readonly IDbConnection _db;


        public PageDetailsService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<PageDetails>> Get(long? PageId, string? PageKey, string PageName, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PageId", PageId);
                parameters.Add("@PageKey", PageKey);
                parameters.Add("@PageName", PageName);              
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<PageDetails>("PageDetails_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<PageDetails>();
            }
        }

        public async Task<PageDetails> GetById(long PageId)

        {
            var pageDetails = await (Get(PageId,null, null, 1, 1));
            return pageDetails.FirstOrDefault();
        }

        public async Task<PageDetails> GetByKey(string PageKey)

        {
            var pageDetails = await (Get(null, PageKey, null, 1, 1));
            return pageDetails.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(PageDetails pageDetails)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@PageId", pageDetails.PageId);
                parameters.Add("@PageKey", pageDetails.PageKey);
                parameters.Add("@PageName", pageDetails.PageName);             
                parameters.Add("@Status", pageDetails.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("PageDetails_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long PageId)
        {
            var pageDetails = await (Get(PageId, null,null, 1, 1));
            var deleteObj = pageDetails.FirstOrDefault();
            long DeletedSatatus = 0;
            if (deleteObj != null)
            {
                deleteObj.Status = "Deleted";
                DeletedSatatus = await SaveOrUpdate(deleteObj);
            }

            return DeletedSatatus > 0;
        }
    }
}
