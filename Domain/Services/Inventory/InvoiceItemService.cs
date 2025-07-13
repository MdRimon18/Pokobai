using Dapper;
using System.Data;
using Domain.Entity.Settings;

using System.Data;
using System.Drawing;
using Domain.DbContex;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Inventory
{


	public class InvoiceItemService
	{
		private readonly IDbConnection _db;
		private readonly ApplicationDbContext _context;

		public InvoiceItemService(DbConnectionDapper db, ApplicationDbContext context)
		{
			_db = db.GetDbConnection();
			_context = context;
		}
		public async Task<IEnumerable<InvoiceItems>> Get(long? InvoiceItemId, long? InvoiceId,int? PageNumber, int? PageSize)
		{
			try
			{
				var parameters = new DynamicParameters();

				parameters.Add("@InvoiceItemId", InvoiceItemId);
				parameters.Add("@InvoiceId", InvoiceId);
				parameters.Add("@PageNumber", PageNumber);
				parameters.Add("@PageSize", PageSize);

				return await _db.QueryAsync<InvoiceItems>("Invoice_items_Get_SP", parameters, commandType: CommandType.StoredProcedure);

			}
			catch (Exception ex)
			{

				return Enumerable.Empty<InvoiceItems>();
			}
		}

		public async Task<InvoiceItems> GetById(long InvoiceItemId)

		{
			var _invoiceItems = await (Get(InvoiceItemId, null, 1, 1));
			return _invoiceItems.FirstOrDefault();
		}




		public async Task<long> SaveOrUpdate(InvoiceItems _invoiceItems)
		{
			try
			{
				var parameters = new DynamicParameters();

				parameters.Add("@InvoiceItemId", _invoiceItems.InvoiceItemId);
				parameters.Add("@InvoiceId", _invoiceItems.InvoiceId);
				parameters.Add("@ProductId", _invoiceItems.ProductId);
				parameters.Add("@Quantity", _invoiceItems.Quantity);
				parameters.Add("@BuyingPrice", _invoiceItems.BuyingPrice);
				parameters.Add("@SellingPrice", _invoiceItems.SellingPrice);
				parameters.Add("@TotalPrice", _invoiceItems.TotalPrice);
				parameters.Add("@VatPercentg", _invoiceItems.VatPercentg);
				parameters.Add("@VatAmount", _invoiceItems.VatAmount);
				parameters.Add("@DiscountPercentg", _invoiceItems.DiscountPercentg);
				parameters.Add("@DiscountAmount", _invoiceItems.DiscountAmount);
				parameters.Add("@ExpirationDate", _invoiceItems.ExpirationDate);
				parameters.Add("@PromoOrCuppnAppliedId", _invoiceItems.PromoOrCuppnAppliedId);
				parameters.Add("@ProductImage", _invoiceItems.ProductImage);
				parameters.Add("@CategoryName", _invoiceItems.CategoryName);
				parameters.Add("@SubCtgName", _invoiceItems.SubCtgName);
				parameters.Add("@Unit", _invoiceItems.Unit);
                parameters.Add("@ProductVariantId", _invoiceItems.ProductVariantId);

                parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
				await _db.ExecuteAsync("InvoiceItemsInsertOrUpdateInvoice", parameters, commandType: CommandType.StoredProcedure);

				return (long)parameters.Get<int>("@SuccessOrFailId");
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log the error)
				Console.WriteLine($"An error occurred while adding order: {ex.Message}");
				return 0;
			}
		}

		public async Task<bool> Delete(long InvoiceItemId)
		{
			try
			{
				// Begin transaction
				using var transaction = await _context.Database.BeginTransactionAsync();

				// Execute raw DELETE query
				var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
					"DELETE FROM InvoiceItems WHERE Id = {0}",
					InvoiceItemId
				);

				// Commit transaction
				await transaction.CommitAsync();

				// Return true if at least one row was deleted
				return rowsAffected > 0;
			}
			catch (Exception ex)
			{

				return false;
			}
		}
	}
}
