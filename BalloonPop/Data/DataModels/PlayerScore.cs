namespace BalloonPop.Data.DataModels
{
    using SQLite.Net.Attributes;

    public class PlayerScore
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string UserName { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Score: {1}", this.UserName, this.Score);
        }

    }
}