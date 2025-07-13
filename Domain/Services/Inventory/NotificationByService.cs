using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class NotificationByService
    {
        private readonly IDbConnection _db;


        public NotificationByService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<NotificationBy>> Get(long? NotificationById)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@NotificationById", NotificationById);
                return await _db.QueryAsync<NotificationBy>("[stt].[NofificationBy_Get_SP]", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<NotificationBy>();
            }
        }

        public async Task<NotificationBy> GetById(long NotificationById)

        {
            var notificationById = await (Get(NotificationById));
            return notificationById.FirstOrDefault();
        }

        //public async Task<Colors> GetByKey(string ColorKey)

        //{
        //    var colors = await (Get(null, ColorKey, null, null, 1, 1));
        //    return colors.FirstOrDefault();
        //}


        //public async Task<long> SaveOrUpdate(Colors colors)
        //{
        //    try
        //    {
        //        var parameters = new DynamicParameters();

        //        parameters.Add("@ColorId", colors.ColorId);
        //        parameters.Add("@ColorKey", colors.ColorKey);
        //        parameters.Add("@LanguageId", colors.LanguageId);
        //        parameters.Add("@ColorIdName", colors.ColorIdName);
        //        parameters.Add("@EntryDateTime", colors.EntryDateTime);
        //        parameters.Add("@EntryBy", colors.EntryBy);
        //        parameters.Add("@LastModifyDate", colors.LastModifyDate);
        //        parameters.Add("@LastModifyBy", colors.LastModifyBy);
        //        parameters.Add("@DeletedDate", colors.DeletedDate);
        //        parameters.Add("@DeletedBy", colors.DeletedBy);
        //        parameters.Add("@Status", colors.Status);
        //        parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //        await _db.ExecuteAsync("Color_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

        //        return (long)parameters.Get<int>("@SuccessOrFailId");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception (e.g., log the error)
        //        Console.WriteLine($"An error occurred while adding order: {ex.Message}");
        //        return 0;
        //    }
        //}

        //public async Task<bool> Delete(long ColorId)
        //{
        //    var colors = await (Get(ColorId, null, null, null, 1, 1));
        //    var deleteObj = colors.FirstOrDefault();
        //    long DeletedSatatus = 0;
        //    if (deleteObj != null)
        //    {
        //        deleteObj.DeletedDate = DateTimeHelper.CurrentDateTime();
        //        deleteObj.Status = "Deleted";
        //        DeletedSatatus = await SaveOrUpdate(deleteObj);
        //    }

        //    return DeletedSatatus > 0;
        //}
    }

}

