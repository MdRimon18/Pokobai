
using Dapper;
using System.Data;
using Domain.DbContex;
using Domain.Entity.Settings;

namespace Domain.Services.Inventory
{
    public class BusinessTypesService
    {
        private readonly IDbConnection _db;


        public BusinessTypesService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<BusinessType>> Get(long? BusinessTypeId, string? BusinessTypeKey, long? LanguageId, string? BusinessTypeName, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BusinessTypeId", BusinessTypeId);
                parameters.Add("@BusinessTypeKey", BusinessTypeKey);
                parameters.Add("@LanguageId", LanguageId);
                parameters.Add("@BusinessTypeName", BusinessTypeName);
                parameters.Add("@PageNumber", pagenumber);
                parameters.Add("@PageSize", pageSize);

                return await _db.QueryAsync<BusinessType>("BusinessTypes_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<BusinessType>();
            }
        }

        public async Task<BusinessType> GetById(long BusinessTypeId)

        {
            var businessTypes = await (Get(BusinessTypeId, null, null, null, 1, 1));
            return businessTypes.FirstOrDefault();
        }

        public async Task<BusinessType> GetByKey(string BusinessTypeKey)

        {
            var businessTypes = await (Get(null, BusinessTypeKey, null, null, 1, 1));
            return businessTypes.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(BusinessType businessType)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BusinessTypeId", businessType.BusinessTypeId);
                parameters.Add("@LanguageId", businessType.LanguageId);
                parameters.Add("@BusinessTypeKey", businessType.BusinessTypeKey);
                parameters.Add("@BusinessTypeName", businessType.BusinessTypeName);
                parameters.Add("@EntryDateTime", businessType.EntryDateTime);
                parameters.Add("@EntryBy", businessType.EntryBy);
                parameters.Add("@LastModifyDate", businessType.LastModifyDate);
                parameters.Add("@LastModifyBy", businessType.LastModifyBy);
                parameters.Add("@DeletedDate", businessType.DeletedDate);
                parameters.Add("@DeletedBy", businessType.DeletedBy);
                parameters.Add("@Status", businessType.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("BusinessTypes_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }





        public async Task<bool> Delete(long BusinessTypeId)
        {
            var businessType = await (Get(BusinessTypeId, null, null, null, 1, 1));
            var deleteObj = businessType.FirstOrDefault();
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
