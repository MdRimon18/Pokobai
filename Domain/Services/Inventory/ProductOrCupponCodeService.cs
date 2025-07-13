using Dapper;
using System.Data;
using Domain.Entity.Settings;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
	public class ProductOrCupponCodeService
	{
		private readonly IDbConnection _db;


		public ProductOrCupponCodeService(DbConnectionDapper db)
		{
			_db = db.GetDbConnection();

		}
		public async Task<IEnumerable<PromoOrCupponCode>> Get(long? PromoOrCuppnId, string? PromoOrCuppnKey, string? PromoOrCuppnName, string? Code,int? MaxUses, int? RemainingUses,string?
		Description, int? PageNumber, int? PageSize)
		{
			try
			{
				var parameters = new DynamicParameters();

				parameters.Add("@PromoOrCuppnId", PromoOrCuppnId);
				parameters.Add("@PromoOrCuppnKey", PromoOrCuppnKey);
				parameters.Add("@PromoOrCuppnName", PromoOrCuppnName);
				parameters.Add("@Code", Code);
				parameters.Add("@MaxUses", MaxUses);
				parameters.Add("@RemainingUses", RemainingUses);
				parameters.Add("@Description", Description);
				
				parameters.Add("@PageNumber", PageNumber);
				parameters.Add("@PageSize", PageSize);

				return await _db.QueryAsync<PromoOrCupponCode>("PromoORCupponCode_Get_SP", parameters, commandType: CommandType.StoredProcedure);

			}
			catch (Exception ex)
			{

				return Enumerable.Empty<PromoOrCupponCode>();
			}
		}

		public async Task<PromoOrCupponCode> GetById(long PromoOrCuppnId)

		{
			var _promoOrCupponCode = await (Get(PromoOrCuppnId,null,null,null,null, null, null, 1, 1));
			return _promoOrCupponCode.FirstOrDefault();
		}

		public async Task<PromoOrCupponCode> GetByKey(string PromoOrCuppnKey)

		{
			var _promoOrCupponCode = await (Get(null, PromoOrCuppnKey, null, null, null, null, null, 1, 1));
			return _promoOrCupponCode.FirstOrDefault();
		}


		public async Task<long> SaveOrUpdate(PromoOrCupponCode _promoOrCupponCode)
		{
			try
			{
				var parameters = new DynamicParameters();

				parameters.Add("@PromoOrCuppnId", _promoOrCupponCode.PromoOrCuppnId);
				parameters.Add("@PromoOrCuppnKey", _promoOrCupponCode.PromoOrCuppnKey);
				parameters.Add("@PromoOrCuppnName", _promoOrCupponCode.PromoOrCuppnName);
				parameters.Add("@Code", _promoOrCupponCode.Code);
				parameters.Add("@Discount", _promoOrCupponCode.Discount);
				parameters.Add("@ValidFrom", _promoOrCupponCode.ValidFrom);
				parameters.Add("@ValidTo", _promoOrCupponCode.ValidTo);
				parameters.Add("@MaxUses", _promoOrCupponCode.MaxUses);
				parameters.Add("@RemainingUses", _promoOrCupponCode.RemainingUses);
				parameters.Add("@Description", _promoOrCupponCode.Description);


				parameters.Add("@EntryDateTime", _promoOrCupponCode.EntryDateTime);
				parameters.Add("@EntryBy", _promoOrCupponCode.EntryBy);
				parameters.Add("@LastModifyDate", _promoOrCupponCode.LastModifyDate);
				parameters.Add("@LastModifyBy", _promoOrCupponCode.LastModifyBy);
				parameters.Add("@DeletedDate", _promoOrCupponCode.DeletedDate);
				parameters.Add("@DeletedBy", _promoOrCupponCode.DeletedBy);
				parameters.Add("@Status", _promoOrCupponCode.Status);
				parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
				await _db.ExecuteAsync("PromoOrCupponCode_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

				return (long)parameters.Get<int>("@SuccessOrFailId");
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log the error)
				Console.WriteLine($"An error occurred while adding order: {ex.Message}");
				return 0;
			}
		}

		public async Task<bool> Delete(long PromoOrCuppnId)
		{
			var _promoOrCupponCode = await (Get(PromoOrCuppnId, null, null, null, null, null, null, 1, 1));
			var deleteObj = _promoOrCupponCode.FirstOrDefault();
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
