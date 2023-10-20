using CryptonRemoteBack.Domain;
using Newtonsoft.Json;

namespace CryptonRemoteBack.Model.Views
{
    public class FarmView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public SystemData? SystemInfo { get; set; }
        public string LocalSystemAddress { get; set; } = string.Empty;
        public string LocalSystemID { get; set; } = string.Empty;
        public string ContainerGUID { get; set; } = string.Empty;
        public UserView User { get; set; } = null!;
        public FlightSheetView? ActiveFlightSheet { get; set; }

        public FarmView(Farm farm)
        {
            Id = farm.Id;
            Name = farm.Name;
            SystemInfo = JsonConvert.DeserializeObject<SystemData>(farm.SystemInfo);
            LocalSystemID = farm.LocalSystemID;
            LocalSystemAddress = farm.LocalSystemAddress;
            ContainerGUID = farm.ContainerGUID;
            User = new UserView(farm.User);
            ActiveFlightSheet = farm.ActiveFlightSheet != null
                ? new FlightSheetView(farm.ActiveFlightSheet, true) : null;
        }
    }

    public class SystemData
    {
        public string Motherboard { get; set; } = string.Empty;
        public string CPU { get; set; } = string.Empty;
        public string OSVersion { get; set; } = string.Empty;
    }
}
