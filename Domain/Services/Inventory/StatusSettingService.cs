using Dapper;
using System.Data;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class StatusSettingService
    {
        private readonly IDbConnection _db;


        public StatusSettingService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<StatusSetting>> Get(long? StatusSettingId, string? StatusSettingKey, long? BranchId, string? StatusSettingName,string? PageName,int? Position, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@StatusSettingId", StatusSettingId);
                parameters.Add("@StatusSettingKey", StatusSettingKey);
                parameters.Add("@BranchId", BranchId);
                parameters.Add("@StatusSettingName", StatusSettingName);
                parameters.Add("@PageName", PageName);
                parameters.Add("@Position", Position);
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);

                return await _db.QueryAsync<StatusSetting>("Status_Setting_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<StatusSetting>();
            }
        }

        public async Task<StatusSetting> GetById(long StatusSettingId)

        {
            var statusSetting = await (Get(StatusSettingId, null,null,null, null, null, 1, 1));
            return statusSetting.FirstOrDefault();
        }

        public async Task<StatusSetting> GetByKey(string StatusSettingKey)

        {
            var statusSetting = await (Get(null, StatusSettingKey, null,null,null, null, 1, 1));
            return statusSetting.FirstOrDefault();
        }


        public async Task<long> Save(StatusSetting statusSetting)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@StatusSettingId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("@branchId", statusSetting.BranchId);
                parameters.Add("@StatusSettingName", statusSetting.StatusSettingName);
                parameters.Add("@PageName", statusSetting.PageName);
                parameters.Add("@Position", statusSetting.Position);
                parameters.Add("@entryDateTime", statusSetting.EntryDateTime);
                parameters.Add("@entryBy", statusSetting.EntryBy);
                await _db.ExecuteAsync("Status_Setting_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@StatusSettingId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(StatusSetting statusSetting)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@StatusSettingId", statusSetting.StatusSettingId    );
            parameters.Add("@branchId", statusSetting.BranchId);
            parameters.Add("@StatusSettingName", statusSetting.StatusSettingName);
            parameters.Add("@PageName", statusSetting.PageName);
            parameters.Add("@Position", statusSetting.Position);
            parameters.Add("@lastModifyDate", statusSetting.LastModifyDate);
            parameters.Add("@lastModifyBy", statusSetting.LastModifyBy);
            parameters.Add("@deletedDate", statusSetting.DeletedDate);
            parameters.Add("@DeletedBy", statusSetting.DeletedBy);
            parameters.Add("@Status", statusSetting.Status);
            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _db.ExecuteAsync("Status_Settings_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }


        public async Task<bool> Delete(long StatusSettingId)
        {
            var unit = await (Get(StatusSettingId, null, null, null,null,null, 1, 1));
            var deleteObj = unit.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate = DateTime.UtcNow;
                deleteObj.Status = "Deleted";
                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
    }
}
