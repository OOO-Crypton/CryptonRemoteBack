using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class FarmView
    {
        public int Id { get; set; }
        public string SystemInfo { get; set; } = string.Empty;
        public string LocalSystemAddress { get; set; } = string.Empty;
        public string LocalSystemID { get; set; } = string.Empty;
        public UserView User { get; set; } = null!;
        public FlightSheetView? ActiveFlightSheet { get; set; }

        public FarmView(Farm farm)
        {
            Id = farm.Id;
            SystemInfo = farm.SystemInfo;
            LocalSystemID = farm.LocalSystemID;
            LocalSystemAddress = farm.LocalSystemAddress;
            User = new UserView(farm.User);
            ActiveFlightSheet = farm.ActiveFlightSheet != null
                ? new FlightSheetView(farm.ActiveFlightSheet) : null;
        }
    }
}
