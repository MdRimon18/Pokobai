using Dapper;
using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity.Settings;
using System.Data;
 



namespace Domain.Data.Repository
{
    public class RoleService
    {
        private readonly IDbConnection _db;


        public RoleService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<Role>> Get(long? RoleId, string? Rolekey, string? RoleName, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@RoleId", RoleId);
                parameters.Add("@Rolekey", Rolekey);
                parameters.Add("@RoleName", RoleName);
                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<Role>("Role_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<Role>();
            }
        }

        public async Task<Role> GetById(long RoleId)

        {
            var role = await (Get(RoleId, null, null, 1, 1));
            return role.FirstOrDefault();
        }

        public async Task<Role> GetByKey(string Rolekey)

        {
            var role = await (Get(null, Rolekey, null, 1, 1));
            return role.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(Role role)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@RoleId", role.RoleId);
                parameters.Add("@Rolekey", role.Rolekey);
                parameters.Add("@RoleName", role.RoleName);
                parameters.Add("@EntryDateTime", role.EntryDateTime);
                parameters.Add("@EntryBy", role.EntryBy);
                parameters.Add("@LastModifyDate", role.LastModifyDate);
                parameters.Add("@LastModifyBy", role.LastModifyBy);
                parameters.Add("@DeletedDate", role.DeletedDate);
                parameters.Add("@DeletedBy", role.DeletedBy);
                parameters.Add("@Status", role.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("Role_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long RoleId)
        {
            var role = await (Get(RoleId, null, null, 1, 1));
            var deleteObj = role.FirstOrDefault();
            long DeletedSatatus = 0;
            if (deleteObj != null)
            {
                deleteObj.DeletedDate =DateTime.UtcNow;
                deleteObj.DeletedBy = UserInfo.UserId;
                deleteObj.Status = "Deleted";
                DeletedSatatus = await SaveOrUpdate(deleteObj);
            }

            return DeletedSatatus > 0;
        }
    }
}

