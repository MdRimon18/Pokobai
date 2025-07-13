using Dapper;
using Microsoft.CodeAnalysis;
using System.Data;
using Domain.Entity.Settings;
using System.Data;
using Domain.CommonServices;
using Domain.DbContex;


namespace Domain.Services.Inventory
{
    public class LanguageService
    {
        private readonly IDbConnection _db;


        public LanguageService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Language>> Get(long? LanguageId, string? LanguageKey, string? LanguageName, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@LanguageId", LanguageId);
                parameters.Add("@LanguageKey", LanguageKey);
                parameters.Add("@LanguageName", LanguageName);
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);

                return await _db.QueryAsync<Language>("Language_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<Language>();
            }
        }

        public async Task<Language> GetById(long LanguageId)

        {
            var languages = await (Get(LanguageId, null, null, 1, 1));
            return languages.FirstOrDefault();
        }

        public async Task<Language> GetByKey(string LanguageKey)

        {
            var languages = await (Get(null, LanguageKey, null, 1, 1));
            return languages.FirstOrDefault();
        }


        public async Task<int> Save(Language language)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@LanguageId", dbType: DbType.Int32, direction: ParameterDirection.Output);
               
                parameters.Add("@LanguageName", language.LanguageName);
                parameters.Add("@entryDateTime", language.EntryDateTime);
                parameters.Add("@entryBy", language.EntryBy);
                await _db.ExecuteAsync("Language_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<int>("@LanguageId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(Language language)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LanguageId", language.LanguageId);
            
            parameters.Add("@LanguageName", language.LanguageName);
            parameters.Add("@lastModifyDate", language.LastModifyDate);
            parameters.Add("@lastModifyBy", language.LastModifyBy);
            parameters.Add("@deletedDate", language.DeletedDate);
            parameters.Add("@DeletedBy", language.DeletedBy);
            parameters.Add("@Status", language.Status);
            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _db.ExecuteAsync("Language_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }


        public async Task<bool> Delete(long LanguageId)
        {
            var Language = await (Get(LanguageId, null, null, 1, 1));
            var deleteObj = Language.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate = DateTime.UtcNow;
                deleteObj.DeletedBy= UserInfo.UserId;
                deleteObj.Status = "Deleted";
                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
    }
}

