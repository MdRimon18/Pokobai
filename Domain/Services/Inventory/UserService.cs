using Dapper;
using Domain.DbContex;
using Domain.Entity;
using Domain.Entity.Settings;
using Domain.Services.Shared;
using System.Data;

namespace Domain.Services.Inventory
{
    public class UserService
    {
        private readonly IDbConnection _db;

        public UserService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();
        }

        public async Task<IEnumerable<User>> Get(long? userId = null, string? email = null, string? name = null,
            string? phoneNo = null, string? password = null, long? roleId = null, 
            int? pageNumber = null, int? pageSize = null)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@UserId", userId);
                parameters.Add("@Email", email);
                parameters.Add("@Name", name);
                parameters.Add("@PhoneNo", phoneNo);
                parameters.Add("@Password", password);
                parameters.Add("@RoleId", roleId);
               
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@PageSize", pageSize);

                return await _db.QueryAsync<User>("Users_Get_SP", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get: {ex.Message}");
                return Enumerable.Empty<User>();
            }
        }

        public async Task<User?> GetById(long userId)
        {
            var users = await Get(userId: userId, pageNumber: 1, pageSize: 1);
            return users.FirstOrDefault();
        }

        public async Task<User?> GetByEmail(string email)
        {
            var users = await Get(email: email, pageNumber: 1, pageSize: 1);
            return users.FirstOrDefault();
        }
        public async Task<User?> GetByPhone(string phone)
        {
            var users = await Get(phoneNo: phone, pageNumber: 1, pageSize: 1);
            return users.FirstOrDefault();
        }
        public async Task<long> SaveOrUpdate(User user)
        {
            try
            {
                if (user.UserId > 0)
                    EntityHelper.SetUpdateAuditFields(user);
                else
                    EntityHelper.SetCreateAuditFields(user);

                var parameters = new DynamicParameters();

                parameters.Add("@UserId", user.UserId);
                parameters.Add("@Name", user.Name);
                parameters.Add("@PhoneNo", user.PhoneNo);
                parameters.Add("@Email", user.Email);
                parameters.Add("@Password", user.Password);
                parameters.Add("@RoleId", user.RoleId);
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@BranchId", user.BranchId);
                parameters.Add("@CountryId", user.CountryId);
                parameters.Add("@CountryCode", user.CountryCode);
                parameters.Add("@MembershipId", user.MembershipId);
                parameters.Add("@IsAbleToLogin", user.IsAbleToLogin);
                parameters.Add("@ImgLink", user.ImgLink);

                ParameterHelper.AddAuditParameters(user, parameters);

                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _db.ExecuteAsync("User_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving user: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> Delete(long userId)
        {
            var user = await GetById(userId);
            if (user != null)
            {
                user.DeletedDate = DateTime.UtcNow;
                user.Status = "Deleted";
                var result = await SaveOrUpdate(user);
                return result > 0;
            }
            return false;
        }
    }
}
