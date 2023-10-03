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
        public int ActiveFlightSheetId { get; set; }
        public MinerMonitoringRecordNoCard? Stat { get; set; }
        public string Message { get; set; }

        public FarmStatView(int farmId, int activeFSId, MinerMonitoringRecordNoCard? stat, string message)
        {
            Farm = farmId;
            ActiveFlightSheetId = activeFSId;
            Stat = stat;
            Message = message;
        }
    }
}
