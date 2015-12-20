namespace BalloonPop.ViewModels.Scores
{
    public class PlayerScoreViewModel
    {
        public PlayerScoreViewModel(string username, int score)
        {
            this.Username = username;
            this.Score = score;
        }

        public string Username { get; set; }

        public int Score { get; set; }

        public string ScoreAsText { get { return string.Format("{0} - {1}", this.Username, this.Score); } }
    }
}
