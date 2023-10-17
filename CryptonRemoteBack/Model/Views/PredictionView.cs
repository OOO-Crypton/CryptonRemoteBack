namespace CryptonRemoteBack.Model.Views
{
    public class PredictionView
    {
        public DateTime Date { get; set; }
        public double Prediction { get; set; }
        public string Request { get; set; }

        public PredictionView(DateTime date, double prediction, string request)
        {
            Date = date;
            Prediction = prediction;
            Request = request;
        }
    }
}
