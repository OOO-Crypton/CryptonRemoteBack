using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class FarmMonitoringView
    {
        public FarmView Farm { get; set; }
        public List<MinerMonitoringRecord>? Monitorings { get; set; }

        public FarmMonitoringView(Farm farm, List<MinerMonitoringRecord>? monitorings)
        {
            Farm = new(farm);
            Monitorings = monitorings != null ? new(monitorings) : null;
        }
    }
    public class FarmStatView
    {
        public int Farm { get; set; }
        public MinerMonitoringRecord? Stat { get; set; }

        public FarmStatView(int farmId, MinerMonitoringRecord? stat)
        {
            Farm = farmId;
            Stat = stat ?? null;
        }
    }
}
