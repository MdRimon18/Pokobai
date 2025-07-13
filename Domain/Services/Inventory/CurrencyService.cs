using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;


namespace Domain.Services.Inventory
{
    public class CurrencyService
    {
        private readonly IDbConnection _db;


        public CurrencyService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Currency>> Get(long? CurrencyId, string? CurrencyKey, int? LanguageId, string? CurrencyName, string? CurrencyCode, string? CurrencyShortName, string? Symbol, decimal? ExchangeRate, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CurrencyId", CurrencyId);
                parameters.Add("@CurrencyKey", CurrencyKey);
                
                parameters.Add("@LanguageId", LanguageId);
                parameters.Add("@CurrencyName", CurrencyName);
                parameters.Add("@CurrencyCode", CurrencyCode);
                parameters.Add("@CurrencyShortName", CurrencyShortName);
                parameters.Add("@Symbol", Symbol);
                parameters.Add("@ExchangeRate", ExchangeRate);
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);

                return await _db.QueryAsync<Currency>("Currency_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<Currency>();
            }
        }

        public async Task<Currency> GetById(long CurrencyId)

        {
            var Currencies = await (Get(CurrencyId, null, null, null, null, null, null, null, 1, 1));
            return Currencies.FirstOrDefault();
        }

        public async Task<Currency> GetByKey(string CurrencyKey)

        {
            var Currencies = await (Get(null, CurrencyKey, null, null, null, null, null, null, 1, 1));
            return Currencies.FirstOrDefault();
        }


        public async Task<long> Save(Currency currency)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CurrencyId", dbType: DbType.Int64, direction: ParameterDirection.Output);
              
                parameters.Add("@LanguageId", currency.LanguageId);
                parameters.Add("@CurrencyName", currency.CurrencyName);
                parameters.Add("@CurrencyCode", currency.CurrencyCode);
                parameters.Add("@CurrencyShortName", currency.CurrencyShortName);
                parameters.Add("@Symbol", currency.Symbol);
                parameters.Add("@ExchangeRate", currency.ExchangeRate);
                parameters.Add("@entryDateTime", currency.EntryDateTime);
                parameters.Add("@entryBy", currency.EntryBy);
                await _db.ExecuteAsync("Currency_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@CurrencyId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(Currency currency)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CurrencyId", currency.CurrencyId);
           
            parameters.Add("@LanguageId", currency.LanguageId);
            parameters.Add("@CurrencyName", currency.CurrencyName);
            parameters.Add("@CurrencyCode", currency.CurrencyCode);
            parameters.Add("@CurrencyShortName", currency.CurrencyShortName );
            parameters.Add("@Symbol", currency.Symbol);
            parameters.Add("@ExchangeRate", currency.ExchangeRate);
            parameters.Add("@lastModifyDate", currency.LastModifyDate);
            parameters.Add("@lastModifyBy", currency.LastModifyBy);
            parameters.Add("@deletedDate", currency.DeletedDate);
            parameters.Add("@DeletedBy", currency.DeletedBy);
            parameters.Add("@Status", currency.Status);
            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _db.ExecuteAsync("Currency_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }


        public async Task<bool> Delete(long CurrencyId)
        {
            var currency = await (Get(CurrencyId, null, null, null, null, null, null, null, 1, 1));
            var deleteObj = currency.FirstOrDefault();
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

