namespace BalloonPop.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;

    using SQLite.Net;
    using SQLite.Net.Async;
    using SQLite.Net.Platform.WinRT;

    using BalloonPop.Data.DataModels;


    public class ResultLocalDb
    {
        public ResultLocalDb()
        {
            this.InitDb();
        }

        public SQLiteAsyncConnection GetDbConnectionAsync()
        {
            var dbFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");

            var connectionFactory =
                new Func<SQLiteConnectionWithLock>(
                    () =>
                    new SQLiteConnectionWithLock(
                        new SQLitePlatformWinRT(),
                        new SQLiteConnectionString(dbFilePath, storeDateTimeAsTicks: false)));

            var asyncConnection = new SQLiteAsyncConnection(connectionFactory);

            return asyncConnection;
        }

        public async void InitDb()
        {
            var connection = this.GetDbConnectionAsync();
            await connection.CreateTableAsync<PlayerScore>();
        }

        public async Task<int> InsertPlayerScoreAsync(PlayerScore playerScore)
        {
            var connection = this.GetDbConnectionAsync();
            var result = await connection.InsertAsync(playerScore);
            return result;
        }

        public async Task<IEnumerable<PlayerScore>> GetPlayerScoreAsync()
        {
            var connection = this.GetDbConnectionAsync();
            var result = await connection.Table<PlayerScore>().ToListAsync();
            return result;
        }
    }
}
