namespace CryptonRemoteBack.Model.Models
{
    public class FarmOverclockingModel
    {
        public double СoreFrequency { get; set; }
        public double MemoryFrequency { get; set; }
        public double CoolerSpeed { get; set; }
        public double Consumption { get; set; }
    }

    public class SelectOverclockingModel
    {
        public int FarmId { get; set; }
        public int OverclockingParamsId { get; set; }
    }
}
