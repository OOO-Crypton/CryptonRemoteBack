namespace CryptonRemoteBack.Model.Views
{
    public class PredictionView
    {
        public DateTime Date { get; set; }
        public double Prediction { get; set; }

        public PredictionView(DateTime date, double prediction)
        {
            Date = date;
            Prediction = prediction;
        }
    }
}
