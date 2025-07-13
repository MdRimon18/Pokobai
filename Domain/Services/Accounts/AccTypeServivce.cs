using Dapper;
using Domain.DbContex;
using Domain.Entity.Settings;
using System.Data;
 

namespace Domain.Services.Accounts
{
    public class AccTypeServivce
    {
        private readonly IDbConnection _db;


        public AccTypeServivce(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<AccType>> Get(int? LanguageId)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@LanguageId", LanguageId);


                return await _db.QueryAsync<AccType>("AccountType_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<AccType>();
            }
        }
    }


}
