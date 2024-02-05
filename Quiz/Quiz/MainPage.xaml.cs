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
        private int currentQuestionIndex = 0;
        private int currentScore = 0;
        private List<int> questions = new List<int>();
        private Stopwatch stopwatch = new Stopwatch();
        private List<double> times = new List<double>();

        public MainPage()
        {
            InitializeComponent();
            GenerateQuestions();
        }

        private void GenerateQuestions()
        {
            Random random = new Random();
            questions = Enumerable.Range(0, 5).Select(_ => random.Next(1, 1001)).ToList();
        }

        private void StartQuiz(object sender, EventArgs e)
        {
            currentScore = 0;
            currentQuestionIndex = 0;
            times.Clear();
            startGameView.IsVisible = false;
            finishGameView.IsVisible = false;
            gameView.IsVisible = true;
            ShowNextQuestion();
        }

        private void FinishButton(object sender, EventArgs e)
        {
            finishGameView.IsVisible = false;
            startGameView.IsVisible = true;
        }

        private void ShowNextQuestion()
        {
            submitAnswerButton.IsEnabled = true;
            if (currentQuestionIndex < questions.Count)
            {
                questionLabel.IsVisible = true;
                questionLabel.Text = $"Podwojona wartość {questions[currentQuestionIndex]} to:";
                answerEntry.IsVisible = true;
                submitAnswerButton.IsVisible = true;
                feedbackLabel.IsVisible = false;
                feedbackFrame.IsVisible = false;
                stopwatch.Restart();
            }
            else
            {
                FinishQuiz();
            }
        }

        private async void SubmitAnswer(object sender, EventArgs e)
        {
            submitAnswerButton.IsEnabled = false;
            stopwatch.Stop();
            int correctAnswer = questions[currentQuestionIndex] * 2;
            if (int.TryParse(answerEntry.Text, out int userAnswer) && userAnswer == correctAnswer)
            {
                currentScore++;
                feedbackLabel.Text = "Poprawna odpowiedź!";
                feedbackFrame.BorderColor = Color.Green;
                feedbackLabel.TextColor = Color.Green;
            }
            else
            {
                feedbackLabel.Text = $"Niestety, to jest zła odpowiedź. Poprawna odpowiedź to: {correctAnswer}";
                feedbackFrame.BorderColor = Color.Red;
                feedbackLabel.TextColor = Color.Red;
            }
            feedbackLabel.IsVisible = true;
            feedbackFrame.IsVisible = true;

            times.Add(stopwatch.Elapsed.TotalSeconds);
            currentQuestionIndex++;
            answerEntry.Text = string.Empty; // Clear answer entry

            await Task.Delay(2500); // Short delay before next question
            ShowNextQuestion();
        }

        private void FinishQuiz()
        {
            double totalTime = times.Sum();
            SaveResult(userNameEntry.Text, totalTime, currentScore);
            DisplayFinalResults(totalTime);
        }

        private async void ViewScoresClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoresPage());
        }

        private void DisplayFinalResults(double totalTime)
        {
            finishGameView.IsVisible = true;
            gameView.IsVisible = false;
            questionLabel.IsVisible = false;
            answerEntry.IsVisible = false;
            submitAnswerButton.IsVisible = false;
            resultPoints.Text = currentScore.ToString();
            resultTime.Text = totalTime.ToString("F2");
        }

        private void SaveResult(string userName, double totalTime, int score)
        {
            App.Database.SaveResultAsync(new UserResult(userName, totalTime, score));
        }

    }

}
