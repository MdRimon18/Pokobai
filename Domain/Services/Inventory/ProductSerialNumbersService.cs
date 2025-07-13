using Dapper;
using System.Data;
using Domain.Entity.Settings;
using System.Data;
using Domain.DbContex;
using Domain.Entity;
using Domain.Services.Shared;

namespace Domain.Services.Inventory
{
    public class ProductSerialNumbersService
    {
        private readonly IDbConnection _db;


        public ProductSerialNumbersService(DbConnectionDapper db)
        {
            _db = db.GetDbConnection();

        }
        public async Task<IEnumerable<ProductSerialNumbers>> Get(long? ProdSerialNmbrId, string? ProdSerialNmbrKey, long? ProductId, string? SerialNumber, long? Rate, DateTime? Date, long? TagSupplierId, string SupplierName, string SupplierOrgName, string? Remarks, string? SerialStatus, int? PageNumber, int? PageSize)
        {
            try
            {
                var parameters = new DynamicParameters();

                parameters.Add("@ProdSerialNmbrId", ProdSerialNmbrId);
                parameters.Add("@ProdSerialNmbrKey", ProdSerialNmbrKey);
                parameters.Add("@ProductId", ProductId);
                parameters.Add("@SerialNumber", SerialNumber);
                parameters.Add("@Rate", Rate);
                parameters.Add("@Date", Date);
                parameters.Add("@TagSupplierId", TagSupplierId);
                parameters.Add("@SupplierName", SupplierName);
                parameters.Add("@SupplierOrgName", SupplierOrgName);
                parameters.Add("@Remarks", Remarks);
                parameters.Add("@SerialStatus", SerialStatus);

                parameters.Add("@PageNumber", PageNumber);
                parameters.Add("@PageSize", PageSize);

                return await _db.QueryAsync<ProductSerialNumbers>("ProductSerialNumbers_getSp", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                return Enumerable.Empty<ProductSerialNumbers>();
            }
        }

        public async Task<ProductSerialNumbers> GetById(long ProdSerialNmbrId)

        {
            var _productSerialNumbers = await (Get(ProdSerialNmbrId, null, null,null,null, null, null, null, null, null, null, 1, 1));
            return _productSerialNumbers.FirstOrDefault();
        }
        public async Task<List<ProductSerialNumbers>> GetByProductId(long productId,int pageNumber,int pageSize)

        {
            var product_list = await (Get(null, null, productId, null, null, null, null, null, null, null, null, pageNumber, pageSize));
            return product_list.ToList();
        }
        public async Task<ProductSerialNumbers> GetByKey(string ProdSerialNmbrKey)

        {
            var _productSerialNumbers = await (Get(null, ProdSerialNmbrKey, null,null,null, null, null, null, null, null, null, 1, 1));
            return _productSerialNumbers.FirstOrDefault();
        }


        public async Task<long> SaveOrUpdate(ProductSerialNumbers _productSerialNumbers)
        {
            try
            {
                if (_productSerialNumbers.ProdSerialNmbrId > 0)
                {
                    EntityHelper.SetUpdateAuditFields(_productSerialNumbers);
                }
                else
                {
                    EntityHelper.SetCreateAuditFields(_productSerialNumbers);
                }

                var parameters = new DynamicParameters();

                parameters.Add("@ProdSerialNmbrId", _productSerialNumbers.ProdSerialNmbrId);
                parameters.Add("@ProdSerialNmbrKey", _productSerialNumbers.ProdSerialNmbrKey);
                parameters.Add("@ProductId", _productSerialNumbers.ProductId);    
                parameters.Add("@Rate", _productSerialNumbers.Rate);
                parameters.Add("@Date", _productSerialNumbers.Date);
                parameters.Add("@SerialNumber", _productSerialNumbers.SerialNumber);
                parameters.Add("@TagSupplierId", _productSerialNumbers.TagSupplierId);
                parameters.Add("@SupplierName", _productSerialNumbers.SupplierName);
                parameters.Add("@SupplierOrgName", _productSerialNumbers.SupplierOrgName);
                parameters.Add("@Remarks", _productSerialNumbers.Remarks);
                parameters.Add("@SerialStatus", _productSerialNumbers.SerialStatus);

                ParameterHelper.AddAuditParameters(_productSerialNumbers, parameters);
                //parameters.Add("@EntryDateTime", _productSerialNumbers.EntryDateTime);
                //parameters.Add("@EntryBy", _productSerialNumbers.EntryBy);
                //parameters.Add("@LastModifyDate", _productSerialNumbers.LastModifyDate);
                //parameters.Add("@LastModifyBy", _productSerialNumbers.LastModifyBy);
                //parameters.Add("@DeletedDate", _productSerialNumbers.DeletedDate);
                //parameters.Add("@DeletedBy", _productSerialNumbers.DeletedBy);
                //parameters.Add("@Status", _productSerialNumbers.Status);
                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _db.ExecuteAsync("ProductSerialNumbers_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

                return (long)parameters.Get<int>("@SuccessOrFailId");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                Console.WriteLine($"An error occurred while adding order: {ex.Message}");
                return 0;
            }
        }

        

    }
}
