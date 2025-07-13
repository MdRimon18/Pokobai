using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.CommonServices;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
    public class CountryServiceV2
    {
        private readonly IDbConnection _db;


        public CountryServiceV2(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<CountryV2>> Get(long? CountryId, string? CountryKey, int? LanguageId, string? CountryName, string? CountryCode, string? CntryShortName, string? Capital, int? CurrencyId, decimal? CurrentArea, decimal? Population, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CountryId", CountryId);
                parameters.Add("@CountryKey", CountryKey);

                parameters.Add("@LanguageId", LanguageId);
                parameters.Add("@CountryName", CountryName);
                parameters.Add("@CountryCode", CountryCode);
                parameters.Add("@CntryShortName", CntryShortName);
                parameters.Add("@Capital", Capital);
                parameters.Add("@CurrencyId", CurrencyId);
                parameters.Add("@CurrentArea", CurrentArea);
                parameters.Add("@Population", Population);
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);

                return await _db.QueryAsync<CountryV2>("Country_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<CountryV2>();
            }
        }

        public async Task<CountryV2> GetById(long CountryId)

        {
            var country = await (Get(CountryId, null,null,null, null, null, null, null, null, null, 1, 1));
            return country.FirstOrDefault();
        }

        public async Task<CountryV2> GetByKey(string CountryKey)

        {
            var country = await (Get(null, CountryKey, null, null,null,null, null, null, null, null, 1, 1));
            return country.FirstOrDefault();
        }


        public async Task<long> Save(CountryV2 country)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CountryId", dbType: DbType.Int64, direction: ParameterDirection.Output);

                parameters.Add("@LanguageId", country.LanguageId);
                parameters.Add("@CountryName", country.CountryName);
                parameters.Add("@CountryCode", country.CountryCode);
                parameters.Add("@CntryShortName", country.CntryShortName);
                parameters.Add("@Capital", country.Capital);
                parameters.Add("@CurrencyId", country.CurrencyId);
                parameters.Add("@CurrentArea", country.CurrentArea);
                parameters.Add("@Population", country.Population);
                parameters.Add("@entryDateTime", country.EntryDateTime);
                parameters.Add("@entryBy", country.EntryBy);
                await _db.ExecuteAsync("Country_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@CountryId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(CountryV2 country)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CountryId", country.CountryId);

            parameters.Add("@LanguageId", country.LanguageId);
            parameters.Add("@CountryName", country.CountryName);
            parameters.Add("@CountryCode", country.CountryCode);
            parameters.Add("@CntryShortName ", country.CntryShortName);
            parameters.Add("@Capital", country.Capital);
            parameters.Add("@CurrencyId", country.CurrencyId);
            parameters.Add("@CurrentArea", country.CurrentArea);
            parameters.Add("@Population", country.Population);
            parameters.Add("@lastModifyDate", country.LastModifyDate);
            parameters.Add("@lastModifyBy", country.LastModifyBy);
            parameters.Add("@deletedDate", country.DeletedDate);
            parameters.Add("@DeletedBy", country.DeletedBy);
            parameters.Add("@Status", country.Status);
            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _db.ExecuteAsync("Country_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }


        public async Task<bool> Delete(long CountryId)
        {
            var country = await (Get(CountryId, null,null,null, null, null, null, null, null, null, 1, 1));
            var deleteObj = country.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                deleteObj.DeletedBy = UserInfo.UserId;
                deleteObj.DeletedDate = DateTime.UtcNow;
                deleteObj.Status = "Deleted";
                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
    }
}
