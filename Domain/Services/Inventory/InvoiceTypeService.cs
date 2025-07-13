using Dapper;
 
using System.Data;
using Domain.Entity.Settings;
using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity;
using Domain.Services.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Domain.Services.Inventory
{
    public class InvoiceTypeService
    {
        private readonly IDbConnection _db;


        public InvoiceTypeService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<InvoiceType>> Get(long? InvoiceTypeId, string? InvoiceTypeKey, int? LanguageId, string? InvoiceTypeName, int? pagenumber, int? pageSize)
        {
            try
            {
                
                var parameters = new DynamicParameters();

                parameters.Add("@InvoiceTypeId", InvoiceTypeId);
                parameters.Add("@InvoiceTypeKey", InvoiceTypeKey);

                parameters.Add("@LanguageId", LanguageId);
                parameters.Add("@InvoiceTypeName", InvoiceTypeName);
               
                parameters.Add("@page_number", pagenumber);
                parameters.Add("@page_size", pageSize);

                return await _db.QueryAsync<InvoiceType>("invoice_type_Get_SP", parameters, commandType: CommandType.StoredProcedure);

            }


            catch (Exception ex)
            {

                return Enumerable.Empty<InvoiceType>();
            }
        }
        public async Task<IEnumerable<InvoiceType>> GetInvoiceTypesAsync(int page = 1, int pageSize = 10,
         string search = "", string sortColumn = "InvoiceTypeId",
         string sortDirection = "desc")
        {
            string searchFilter = string.IsNullOrEmpty(search)
                ? ""
                : $"AND (LOWER(InvoiceTypeName) LIKE @Search)";

            string validSortColumn = sortColumn switch
            {
                "InvoiceTypeName" => "InvoiceTypeName",
                "EntryDateTime" => "EntryDateTime",
                "LastModifyDate" => "LastModifyDate",
                "Status" => "Status",
                _ => "InvoiceTypeId" // Default sorting column
            };

            string query = $@"
            WITH InvoiceData AS (
            SELECT 
            InvoiceTypeId,
            InvoiceTypeKey,
            LanguageId,
            InvoiceTypeName,
            EntryDateTime,
            EntryBy,
            LastModifyDate,
            LastModifyBy,
            DeletedDate,
            DeletedBy,
            Status,
            COUNT(*) OVER() AS TotalCount 
        FROM [stt].[InvoiceTypes]
        WHERE Status='Active'  {searchFilter}
        )
        SELECT * FROM InvoiceData
        ORDER BY {validSortColumn} {sortDirection}
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            var parameters = new DynamicParameters();
            parameters.Add("Search", $"%{search.ToLower()}%");
            parameters.Add("Offset", (page - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            // Use _db for querying
            var invoiceTypes = await _db.QueryAsync<InvoiceType>(query, parameters);

            return invoiceTypes.ToList();
        }

        public async Task<InvoiceType> GetById(long InvoiceTypeId)

        {
            var invoiceType = await (Get(InvoiceTypeId, null, null, null, 1, 1));
            return invoiceType.FirstOrDefault();
        }

        public async Task<InvoiceType> GetByKey(string InvoiceTypeKey)

        {
            var invoiceType = await (Get(null, InvoiceTypeKey, null, null, 1, 1));
            return invoiceType.FirstOrDefault();
        }


        public async Task<long> Save(InvoiceType invoiceType)
        {
            try
            {
                EntityHelper.SetCreateAuditFields(invoiceType);
                var parameters = new DynamicParameters();

                parameters.Add("@InvoiceTypeId", dbType: DbType.Int64, direction: ParameterDirection.Output);

                parameters.Add("@LanguageId", invoiceType.LanguageId);
                parameters.Add("@InvoiceTypeName", invoiceType.InvoiceTypeName);
                ParameterHelper.AddAuditParameters(invoiceType, parameters);

                await _db.ExecuteAsync("invoice_type_Insert_SP", parameters, commandType: CommandType.StoredProcedure);



                return parameters.Get<long>("@InvoiceTypeId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }


        public async Task<bool> Update(InvoiceType invoiceType)
        {
            EntityHelper.SetUpdateAuditFields(invoiceType);
            var parameters = new DynamicParameters();
            parameters.Add("@InvoiceTypeId", invoiceType.InvoiceTypeId);

            parameters.Add("@LanguageId", invoiceType.LanguageId);
            parameters.Add("@InvoiceTypeName", invoiceType.InvoiceTypeName);

            ParameterHelper.AddAuditParameters(invoiceType, parameters);

            parameters.Add("@success", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await _db.ExecuteAsync("invoice_type_Update_SP",
                  parameters, commandType: CommandType.StoredProcedure);

            int success = parameters.Get<int>("@success");
            return success > 0;
        }


        public async Task<bool> Delete(long InvoiceTypeId)
        {
            var invoiceType = await (Get(InvoiceTypeId, null, null, null, 1, 1));
            var deleteObj = invoiceType.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                EntityHelper.SetDeleteAuditFields(deleteObj);
                
                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
        public async Task<bool> DeleteByKey(Guid key)
        {
            var invoiceType = await (Get(null, key.ToString(), null, null, 1, 1));
            var deleteObj = invoiceType.FirstOrDefault();
            bool isDeleted = false;
            if (deleteObj != null)
            {
                EntityHelper.SetDeleteAuditFields(deleteObj);

                isDeleted = await Update(deleteObj);
            }

            return isDeleted;
        }
        //Example of Pagination sorting searching using efficient and performanceable code 
        public async Task<IEnumerable<User>> GetUsersAsync(int page = 1, int pageSize = 10,
            string search = "", string sortColumn = "Id", string sortDirection = "desc",
            int roleId = 0)
            {
             string searchFilter = string.IsNullOrEmpty(search) ? "" : $"AND (LOWER(U.[FirstName]+' '+ U.[LastName]) LIKE @Search OR LOWER(Email) LIKE @Search OR LOWER(Phone) LIKE @Search)";

              string validSortColumn = sortColumn switch
             {
                "UserFullName" => "UserFullName",
                "Email" => "Email",
                "Phone" => "Phone",
                "TotalReceivedOrder" => "TotalReceivedOrder",
                "TotalDeliveryOrder" => "TotalDeliveryOrder",
                "CreatedDate" => "CreatedDate",
                _ => "Id" // Default to Id if an invalid column is provided
            };

            string query = $@"
          WITH UserData AS (
            SELECT 
            U.*,
            U.[FirstName]+' '+ U.[LastName] AS UserFullName,
            (SELECT COUNT(1) FROM [dbo].[Orders] WHERE CollectionDriverId = U.Id) AS TotalReceivedOrder,
            (SELECT COUNT(1) FROM [dbo].[Orders] WHERE DeliveryDriverId = U.Id) AS TotalDeliveryOrder,
            COUNT(*) OVER() AS TotalCount 
            FROM [Users] U
            WHERE U.[Status] = 0 AND U.RoleId = @RoleId {searchFilter}
        )
            SELECT * FROM UserData
            ORDER BY {validSortColumn} {sortDirection}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            var parameters = new DynamicParameters();
            parameters.Add("Search", $"%{search.ToLower()}%");
            parameters.Add("Offset", (page - 1) * pageSize);
            parameters.Add("PageSize", pageSize);
            parameters.Add("RoleId", roleId);

            // Use _db instead of creating a new SqlConnection
            var users = await _db.QueryAsync<User>(query, parameters);

            return users.ToList();
        }

    }
}

