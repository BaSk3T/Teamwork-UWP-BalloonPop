using SQLite.Net.Attributes;

namespace BalloonPop.Data.DataModels
{
    public class PlayerScore
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string UserName { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return $"#{this.Id}; Name: {this.UserName}; Score: {this.Score}";
        }

    }
}
