namespace BalloonPop.ViewModels.Scores
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    
    using BalloonPop.Extensions;

    public class AllScoresViewModel : ViewModelBase
    {
        private ObservableCollection<PlayerScoreViewModel> scores;

        public AllScoresViewModel()
        {
            this.scores = new ObservableCollection<PlayerScoreViewModel>();
        }

        public IEnumerable<PlayerScoreViewModel> Scores
        {
            get
            {
                if (this.scores == null)
                {
                    this.scores = new ObservableCollection<PlayerScoreViewModel>();
                }

                return this.scores;
            }
            set
            {
                if (this.scores == null)
                {
                    this.scores = new ObservableCollection<PlayerScoreViewModel>();
                }

                this.scores.Clear();
                value.ForEach(this.scores.Add);
            }
        }
    }
}
