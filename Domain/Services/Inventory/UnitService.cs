using Dapper;
using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity;
using Domain.Entity.Settings;
using Domain.Services.Shared;
using System.Data;


namespace Domain.Services.Inventory
{
    public class UnitService
    {
        private readonly IDbConnection _db;


        public UnitService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Unit>> Get(long? UnitId, string? UnitKey, long? BranchId, string? UnitName, int? pagenumber, int? pageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@UnitId", UnitId);
                parameters.Add("@UnitKey", UnitKey);
                parameters.Add("@BranchId", BranchId);
                parameters.Add("@UnitName", UnitName);
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);
                
                return await _db.QueryAsync<Unit>("Unit_Get_SP", parameters, commandType: CommandType.StoredProcedure);
                
            }
            catch (Exception ex)
            {
    
                return Enumerable.Empty<Unit>();
            }
        }

        public async Task<Unit> GetById(long UnitId)

        {
            var units = await (Get(UnitId,null,null, null, 1,1));
            return units.FirstOrDefault();
        }

        public async Task<Unit> GetByKey(string UnitKey)

        {
            var units = await (Get(null, UnitKey, null, null, 1, 1));
            return units.FirstOrDefault();
        }


        public async Task<long> Save(Unit unit)
        {
            try
            {
                EntityHelper.SetCreateAuditFields(unit);
                unit.BranchId = CompanyInfo.BranchId;
                var parameters = new DynamicParameters();

                parameters.Add("@UnitId", dbType: DbType.Int64, direction: ParameterDirection.Output);
                parameters.Add("@branchId", unit.BranchId);
                parameters.Add("@unitName", unit.UnitName);
                ParameterHelper.AddAuditParameters(unit, parameters);

                await _db.ExecuteAsync("Unit_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@UnitId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(Unit unit)
        {
       
            EntityHelper.SetUpdateAuditFields(unit);

            var parameters = new DynamicParameters();
            parameters.Add("@unitId", unit.UnitId);
            parameters.Add("@branchId", unit.BranchId);
            parameters.Add("@unitName", unit.UnitName);
            ParameterHelper.AddAuditParameters(unit, parameters);

            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);

           
            await _db.ExecuteAsync("Unit_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }

        
        public async Task<bool> Delete(long UnitId)
        {
            var unit = await (Get(UnitId, null, null, null, 1, 1));
            var deleteObj = unit.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                EntityHelper.SetDeleteAuditFields(deleteObj);
                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
    }
}
