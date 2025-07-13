using Dapper;
using Domain.DbContex;
using System.Data;

using System.Threading.Tasks;
 

namespace Domain.Data.Repository
{
    public class TaskRepository
    {
        private readonly IDbConnection _dbConnection;

        public TaskRepository(DbConnectionDapper dbConnectionFactory)
        {
            _dbConnection = dbConnectionFactory.GetDbConnection();
        }

        public async Task<IEnumerable<Task>> GetTask()
        {
            return await _dbConnection.QueryAsync<Task>("select * from Tasks");
        }
        public async Task<Task> GetTaskById(long taskId)
        {
            string query = "SELECT * FROM Tasks WHERE Id = @TaskId";
            return await _dbConnection.QueryFirstOrDefaultAsync<Task>(query, new { TaskId = taskId });
        }
        public async Task<int> AddTask(Task task)
        {
            string query = "insert into Tasks (Title,IsComplete) values(@Title,@IsComplete)";
            return await _dbConnection.ExecuteScalarAsync<int>(query, task);
        }
        public async Task<bool> UpdateTask(Task task)
        {
            string sql = "UPDATE Tasks SET Title = @Title, IsComplete = @IsComplete WHERE Id = @Id";
            int affectedRows = await _dbConnection.ExecuteAsync(sql, task);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteTask(long id)
        {
            string sql = "DELETE FROM Tasks WHERE Id = @Id";
            int affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
        public async Task<IEnumerable<Task>> GetTaskSearchByTitle(string title)
        {
            string query = "SELECT * FROM Tasks WHERE Title = @title";
            return await _dbConnection.QueryAsync<Task>(query, new { title = title });
        }
    }
}
