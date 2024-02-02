using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Quiz
{
    public partial class MainPage : ContentPage
    {
        private int currentScore = 0;
        private Stopwatch stopwatch = new Stopwatch();
        private List<double> times = new List<double>();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void StartQuiz(object sender, EventArgs e)
        {
            currentScore = 0; // Reset current score
            times.Clear(); // Clear previous times

            for (int i = 0; i < 5; i++)
            {
                int number = new Random().Next(1, 1001); // Generate random number
                stopwatch.Restart(); // Restart stopwatch for each question
                string answer = await DisplayPromptAsync($"Pytanie {i + 1}", $"Podwojona wartość {number} to:"); // Ask question
                stopwatch.Stop(); // Stop the stopwatch

                if (int.TryParse(answer, out int result) && result == number * 2)
                {
                    // If answer is correct
                    currentScore++; // Increment score
                    times.Add(stopwatch.Elapsed.TotalSeconds); // Record time
                    await DisplayAlert("Poprawna odpowiedź", "Brawo! To jest poprawna odpowiedź.", "OK");
                }
                else
                {
                    // If answer is incorrect
                    times.Add(stopwatch.Elapsed.TotalSeconds); // Record time
                    await DisplayAlert("Zła odpowiedź", $"Niestety, to jest zła odpowiedź. Poprawna odpowiedź to: {number * 2}", "OK");
                }
            }

            double totalTime = times.Sum(); // Calculate total time
            SaveResult(userNameEntry.Text, totalTime, currentScore); // Save result to database
            await DisplayAlert("Koniec quizu", $"Twój wynik: {currentScore} punktów. Całkowity czas: {totalTime} sekund.", "OK"); // Show final score and time
        }

        private void SaveResult(string userName, double totalTime, int score)
        {
            App.Database.SaveResultAsync(new UserResult(userName, totalTime, score));
        }
    }

}
