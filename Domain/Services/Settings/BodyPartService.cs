
using Dapper;
using Domain.DbContex;
using Domain.Entity.Settings;
using System.Data;
using System.Data.Common;


namespace Domain.Services 
{
    public class BodyPartService
    {
        private readonly IDbConnection _db;


        public BodyPartService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        // Method to get all BodyParts
        public async Task<IEnumerable<BodyPart>> GetBodyPartsAsync()
        {
            var sql = "SELECT BodyPartName FROM BodyParts"; // Adjust table name as needed
            return await _db.QueryAsync<BodyPart>(sql);
        }

    }
}
