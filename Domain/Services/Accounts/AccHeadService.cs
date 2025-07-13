using Dapper;
using Domain.DbContex;
using Domain.Entity.Accounts;
using System.Data;
 
namespace Domain.Services.Accounts
{
    public class AccHeadService
    {

        private readonly IDbConnection _db;


        public AccHeadService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<AccHead>> Get(long? AccHeadId, string? AccHeadKey, string? AccHeadName, string AccType, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@AccHeadId", AccHeadId);
                parameters.Add("@AccHeadKey", AccHeadKey);
                parameters.Add("@AccHeadName", AccHeadName);
                parameters.Add("@AccType", AccType);
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);

                return await _db.QueryAsync<AccHead>("Acc_Head_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<AccHead>();
            }
        }

        public async Task<AccHead> GetById(long AccHeadId)

        {
            var accHead = await Get(AccHeadId, null, null, null, 1, 1);
            return accHead.FirstOrDefault();
        }

        public async Task<AccHead> GetByKey(string AccHeadKey)

        {
            var accHead = await Get(null, AccHeadKey, null, null, 1, 1);
            return accHead.FirstOrDefault();
        }


        public async Task<long> Save(AccHead accHead)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@AccHeadId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("@AccHeadName", accHead.AccHeadName);
                parameters.Add("@AccType", accHead.AccType);
                parameters.Add("@AccHeadKey", accHead.AccHeadKey);
                parameters.Add("@entryDateTime", accHead.EntryDateTime);
                parameters.Add("@entryBy", accHead.EntryBy);
                await _db.ExecuteAsync("Acc_Head_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@AccHeadId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(AccHead accHead)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AccHeadId", accHead.AccHeadId);

            parameters.Add("@AccHeadName", accHead.AccHeadName);
            parameters.Add("@AccType", accHead.AccType);
            parameters.Add("@lastModifyDate", accHead.LastModifyDate);
            parameters.Add("@lastModifyBy", accHead.LastModifyBy);
            parameters.Add("@deletedDate", accHead.DeletedDate);
            parameters.Add("@DeletedBy", accHead.DeletedBy);
            parameters.Add("@Status", accHead.Status);
            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _db.ExecuteAsync("Acc_head_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }


        public async Task<bool> Delete(long AccHeadId)
        {
            var accHead = await Get(AccHeadId, null, null, null, 1, 1);
            var deleteObj = accHead.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate =DateTime.UtcNow;
                deleteObj.Status = "Deleted";
                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
    }
}

