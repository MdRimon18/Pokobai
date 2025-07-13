using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Dapper;
using Domain.DbContex;
using Domain.Entity.Settings;
 
 
public class ProductRepository
{
    private readonly IDbConnection _dbConnection;

    public ProductRepository(DbConnectionDapper dbConnectionFactory)
    {
        _dbConnection = dbConnectionFactory.GetDbConnection();
    }

    public async Task<IEnumerable<Products>> GetProducts()
    {
        return await _dbConnection.QueryAsync<Products>("SELECT * FROM Products");
    }

    public async Task<Products> GetProductById(int id)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Products>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> AddProduct(Products product)
    {
        string sql = "INSERT INTO dbo.Products  (Name, Price) VALUES (@Name, @Price); SELECT SCOPE_IDENTITY();";
        return await _dbConnection.ExecuteScalarAsync<int>(sql, product);
    }

    public async Task<bool> UpdateProduct(Products product)
    {
        string sql = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
        int affectedRows = await _dbConnection.ExecuteAsync(sql, product);
        return affectedRows > 0;
    }
        
    public async Task<bool> DeleteProduct(int id)
    {
        string sql = "DELETE FROM Products WHERE Id = @Id";
        int affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}
