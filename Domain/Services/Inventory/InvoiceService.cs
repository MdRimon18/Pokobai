using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;
using Domain.CommonServices;


namespace Domain.Services.Inventory
{
	public class InvoiceService
	{
		private readonly IDbConnection _db;


		public InvoiceService(DbConnectionDapper db)
		{
			_db = db.GetDbConnection();

		}
		public async Task<IEnumerable<Invoice>> Get(long? InvoiceId, string? InvoiceKey,long? BranchId, 
			string? InvoiceNumber, int? CustomerID, long? InvoiceTypeId,string? SalesByName,			
			long? OrderStatusId, string VoucherCode,int? PageNumber, int? PageSize)
		{
			try
			{
				var parameters = new DynamicParameters();
				parameters.Add("@InvoiceId", InvoiceId);
				parameters.Add("@InvoiceKey", InvoiceKey);
                parameters.Add("@CompanyId", CompanyInfo.CompanyId);
                parameters.Add("@BranchId", BranchId);
				parameters.Add("@InvoiceNumber", InvoiceNumber);
				parameters.Add("@CustomerID", CustomerID);
				parameters.Add("@InvoiceTypeId", InvoiceTypeId);
				parameters.Add("@SalesByName", SalesByName);
				parameters.Add("@OrderStatusId", OrderStatusId);
				parameters.Add("@VoucherCode", VoucherCode);
				parameters.Add("@PageNumber", PageNumber);
				parameters.Add("@PageSize", PageSize);

				return await _db.QueryAsync<Invoice>("Invoices_GetSp", parameters, commandType: CommandType.StoredProcedure);

			}
			catch (Exception ex)
			{

				return Enumerable.Empty<Invoice>();
			}
		}

		public async Task<Invoice> GetById(long InvoiceId)

		{
			var invoice = await (Get(InvoiceId,null, null, null, null, null, null, null,  null, 1, 1));
			return invoice.FirstOrDefault();
		}

		public async Task<Invoice> GetByKey(string InvoiceKey)

		{
			var invoice = await (Get(null,
				InvoiceKey, null, null, null, null, null, null, null, 1, 1));
			return invoice.FirstOrDefault();
		}


		public async Task<long> SaveOrUpdate(Invoice invoice)
		{
			try  
			{
				var parameters = new DynamicParameters();

				parameters.Add("@InvoiceId", invoice.InvoiceId);
                parameters.Add("@CompanyId", CompanyInfo.CompanyId);
                parameters.Add("@BranchId", invoice.BranchId);
				parameters.Add("@InvoiceNumber", invoice.InvoiceNumber);
				parameters.Add("@CustomerID", invoice.CustomerID);
				parameters.Add("@InvoiceDateTime", invoice.InvoiceDateTime);
				parameters.Add("@InvoiceTypeId", invoice.InvoiceTypeId);
				parameters.Add("@NotificationById", invoice.NotificationById);
				parameters.Add("@SalesByName", invoice.SalesByName);
				parameters.Add("@Notes", invoice.Notes);
				parameters.Add("@PaymentTypeId", invoice.PaymentTypeId);
				parameters.Add("@PaymentReference", invoice.PaymentReference);
				parameters.Add("@ShippingMethodId", invoice.ShippingMethodId);
				parameters.Add("@CurrencyId", invoice.CurrencyId);
				parameters.Add("@OrderStatusId", invoice.OrderStatusId);
				parameters.Add("@TotalQnty", invoice.TotalQnty);
				parameters.Add("@TotalAmount", invoice.TotalAmount);
				parameters.Add("@TotalVat", invoice.TotalVat);
				parameters.Add("@TotalDiscount", invoice.TotalDiscount);
				parameters.Add("@TotalAddiDiscount", invoice.TotalAddiDiscount);
				parameters.Add("@TotalPayable", invoice.TotalPayable);
				parameters.Add("@RecieveAmount", invoice.RecieveAmount);
				parameters.Add("@DueAmount", invoice.DueAmount);
				parameters.Add("@DuePaymentDate", invoice.DuePaymentDate);
				parameters.Add("@PromoOrCupponId", invoice.PromoOrCupponId);
				parameters.Add("@PolicyId", invoice.PolicyId);
				parameters.Add("@DeliveryDate", invoice.DeliveryDate);
				parameters.Add("@PriorityLevelId", invoice.PriorityLevelId);
				parameters.Add("@GiftOrder", invoice.GiftOrder);
				parameters.Add("@VoucherCode", invoice.VoucherCode);
				parameters.Add("@ShipmentTrackingNumber", invoice.ShipmentTrackingNumber);
				parameters.Add("@ExchangeRate", invoice.ExchangeRate);
                parameters.Add("@DeliveryAddressId", invoice.DeliveryAddressId);
                parameters.Add("@ExpirationDate", invoice.ExpirationDate);
				parameters.Add("@EntryDateTime", invoice.EntryDateTime);
				parameters.Add("@EntryBy", invoice.EntryBy);
				parameters.Add("@LastModifyDate", invoice.LastModifyDate);
				parameters.Add("@LastModifyBy", invoice.LastModifyBy);
				parameters.Add("@DeletedDate", invoice.DeletedDate);
				parameters.Add("@DeletedBy", invoice.DeletedBy);
				parameters.Add("@Status", invoice.Status);
				parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
				await _db.ExecuteAsync("Invoices_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

				return (long)parameters.Get<int>("@SuccessOrFailId");
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log the error)
				Console.WriteLine($"An error occurred while adding order: {ex.Message}");
				return 0;
			}
		}

		public async Task<bool> Delete(long InvoiceId)
		{
			var invoice = await (Get(InvoiceId,
				null, null, null, null, null, null, null, null, 1, 1));
			var deleteObj = invoice.FirstOrDefault();
			long DeletedSatatus = 0;
			if (deleteObj != null)
			{
				deleteObj.DeletedDate = DateTime.UtcNow;
				deleteObj.Status = "Deleted";
				DeletedSatatus = await SaveOrUpdate(deleteObj);
			}

			return DeletedSatatus > 0;
		}
	}
}
