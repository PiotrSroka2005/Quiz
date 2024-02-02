using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    public class Database
    {
        private SQLiteAsyncConnection database;

        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<UserResult>().Wait();
        }

        public Task<List<UserResult>> GetResultsAsync()
        {
            return database.Table<UserResult>().ToListAsync();
        }
        public Task<UserResult> GetUserName(string userName)
        {
            return database.Table<UserResult>().Where(i => i.UserName == userName).FirstOrDefaultAsync();
        }

        public Task<int> SaveResultAsync(UserResult result)
        {
            return database.InsertAsync(result);
        }
    }
}
