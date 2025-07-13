using Dapper;
using System.Data;
using Domain.Entity.Settings;
using System.Data;
using Domain.DbContex;

namespace Domain.Services.Inventory
{
	public class LocationService
	{
		private readonly IDbConnection _db;


		public LocationService(DbConnectionDapper db)
		{
			_db = db.GetDbConnection();

		}
		public async Task<IEnumerable<Locations>> Get(long? LocationId, string? LocationKey, string? LocationName,int? PageNumber, int? PageSize)
		{
			try
			{
				var parameters = new DynamicParameters();

				parameters.Add("@LocationId", LocationId);
				parameters.Add("@LocationKey", LocationKey);
				parameters.Add("@LocationName", LocationName);
				parameters.Add("@PageNumber", PageNumber);
				parameters.Add("@PageSize", PageSize);

				return await _db.QueryAsync<Locations>("Locations_Get_SP", parameters, commandType: CommandType.StoredProcedure);

			}
			catch (Exception ex)
			{

				return Enumerable.Empty<Locations>();
			}
		}

		public async Task<Locations> GetById(long LocationId)

		{
			var _locations= await (Get(LocationId, null,null, 1, 1));
			return _locations.FirstOrDefault();
		}

		public async Task<Locations> GetByKey(string LocationKey)

		{
			var _locations = await (Get(null, LocationKey, null, 1, 1));
			return _locations.FirstOrDefault();
		}


		public async Task<long> SaveOrUpdate(Locations _locations)
		{
			try
			{
				var parameters = new DynamicParameters();

				parameters.Add("@LocationId", _locations.LocationId);
				parameters.Add("@LocationKey", _locations.LocationKey);
				parameters.Add("@LocationName", _locations.LocationName);
				parameters.Add("@EntryDateTime", _locations.EntryDateTime);
				parameters.Add("@EntryBy", _locations.EntryBy);
				parameters.Add("@LastModifyDate", _locations.LastModifyDate);
				parameters.Add("@LastModifyBy", _locations.LastModifyBy);
				parameters.Add("@DeletedDate", _locations.DeletedDate);
				parameters.Add("@DeletedBy", _locations.DeletedBy);
				parameters.Add("@Status", _locations.Status);
				parameters.Add("@SuccessOrFailId", dbType: DbType.Int32, direction: ParameterDirection.Output);
				await _db.ExecuteAsync("Locations_InsertOrUpdate_SP", parameters, commandType: CommandType.StoredProcedure);

				return (long)parameters.Get<int>("@SuccessOrFailId");
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log the error)
				Console.WriteLine($"An error occurred while adding order: {ex.Message}");
				return 0;
			}
		}

		public async Task<bool> Delete(long LocationId)
		{
			var _locations = await (Get(LocationId,null, null, 1, 1));
			var deleteObj = _locations.FirstOrDefault();
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
