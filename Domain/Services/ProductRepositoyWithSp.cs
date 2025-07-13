
using Dapper;

 
using System.Data;
using System.Data.Common;

namespace Domain.Data.Repository
{
    //public class ProductRepositoyWithSp
    //{
    //    private readonly IDbConnection _dbConnection;

    //    public ProductRepositoyWithSp(DbConnection dbConnectionFactory)
    //    {
    //        _dbConnection = dbConnectionFactory.GetConnection();
       
    //    public async Task<IEnumerable<Products>> GetProducts()
    //    {
    //        return await _dbConnection.QueryAsync<Products>("GetProducts", commandType: CommandType.StoredProcedure);
    //    }

    //    private async Task<Products> GetProductById(int id)
    //    {
    //        return await _dbConnection.QueryFirstOrDefaultAsync<Products>("GetProductById", new { Id = id }, commandType: CommandType.StoredProcedure);
    //    }

    //    public async Task<int> AddProduct(Products product)
    //    {
    //        var parameters = new DynamicParameters();
    //        parameters.Add("@Name", product.Name);
    //        parameters.Add("@Price", product.Price);
    //        parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

    //        await _dbConnection.ExecuteAsync("AddProduct", parameters, commandType: CommandType.StoredProcedure);

    //        return parameters.Get<int>("@Id");
    //    }

    //    public async Task<bool> UpdateProduct(Products product)
    //    {
    //        var parameters = new DynamicParameters();
    //        parameters.Add("@Id", product.Id);
    //        parameters.Add("@Name", product.Name);
    //        parameters.Add("@Price", product.Price);

    //        int affectedRows = await _dbConnection.ExecuteAsync("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
    //        return affectedRows > 0;
    //    }

    //    public async Task<bool> DeleteProduct(int id)
    //    {
    //        int affectedRows = await _dbConnection.ExecuteAsync("DeleteProduct", new { Id = id }, commandType: CommandType.StoredProcedure);
    //        return affectedRows > 0;
    //    }
    //}
}




//public async Task<Tasks> GetTaskById(int taskId)
//{
//    var parameters = new
//    {
//        TaskId = taskId
//    };

//    return await _dbConnection.QueryFirstOrDefaultAsync<Tasks>(
//        "GetTaskById",  
//        parameters,
//        commandType: CommandType.StoredProcedure
//    );
//}
