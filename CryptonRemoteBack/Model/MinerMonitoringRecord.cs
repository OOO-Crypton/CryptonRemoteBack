namespace CryptonRemoteBack.Model
{
    public class MinerMonitoringRecord
    {
        public string id { get; set; }
        public DateTime date { get; set; }
        public bool isActive { get; set; }
        public double currentHashrate { get; set; }
        public double gpuTemperature { get; set; }
        public double fanRPM { get; set; }
        public double energyConsumption { get; set; }
        public MinerMonitoringRecordVideocard videocard { get; set; }
    }

    public class MinerMonitoringRecordNoCard
    {
        public string id { get; set; }
        public DateTime date { get; set; }
        public bool isActive { get; set; }
        public double currentHashrate { get; set; }
        public double gpuTemperature { get; set; }
        public double fanRPM { get; set; }
        public double energyConsumption { get; set; }
    }
}
