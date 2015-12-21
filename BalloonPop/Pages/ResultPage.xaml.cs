using BalloonPop.Data;
using BalloonPop.Data.DataModels;
using BalloonPop.Extensions;
using BalloonPop.ViewModels;
using BalloonPop.ViewModels.Scores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BalloonPop.Pages
{
    public sealed partial class ResultPage : Page
    {
        public ResultPage()
        {
            this.InitializeComponent();
            this.ResultLocalDb = new ResultLocalDb();
            this.ScoresVM = new AllScoresViewModel();
            this.DataContext = ScoresVM;
        }

        public AllScoresViewModel ScoresVM { get; set; }

        public ResultLocalDb ResultLocalDb { get; set; }

        private async void LoadScores(PlayerScore playerScore)
        {
            var scores = await this.ResultLocalDb.GetPlayerScoreAsync();

            var scoresVM = new List<PlayerScoreViewModel>();
            scores.ForEach(x => scoresVM.Add(new PlayerScoreViewModel(x.UserName, x.Score)));

            this.ScoresVM.Scores = scoresVM.OrderByDescending(x => x.Score).Take(10);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            PlayerScore playerScore = e.Parameter as PlayerScore;

            if (playerScore != null)
            {
                try
                {
                    await this.ResultLocalDb.InsertPlayerScoreAsync(playerScore);
                }
                catch (Exception)
                {
                }
            }
            
            this.LoadScores(playerScore);
        }

        private void SwitchToMenu(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Menu));
        }
    }
}
