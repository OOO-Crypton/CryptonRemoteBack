namespace CryptonRemoteBack.Model
{
    public class VideocardView
    {
        public string FullName { get; set; } = string.Empty;
        public int CCDType { get; set; }
        public string CardManufacturer { get; set; } = string.Empty;
        public string CCDModel { get; set; } = string.Empty;
        public string GPUFrequency { get; set; } = string.Empty;
        public string MemoryFrequency { get; set; } = string.Empty;
        public MonitoringView MonitoringView { get; set; } = null!;

    }

    public class MonitoringView
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public double CurrentHashrate { get; set; }
        public double GPUTemperature { get; set; }
        public double FanRPM { get; set; }
        public double EnergyConsumption { get; set; }
    }

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
