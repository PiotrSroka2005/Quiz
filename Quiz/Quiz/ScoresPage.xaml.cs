using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quiz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ScoresPage : ContentPage
    {
        public ScoresPage()
        {
            InitializeComponent();
            LoadScores();
        }

        private async void LoadScores()
        {
            var scores = await App.Database.GetResultsAsync();
            scoresListView.ItemsSource = scores
                .OrderByDescending(score => score.Score)
                .ThenBy(score => score.TotalTime);
        }
    }
}