namespace CryptonRemoteBack.Model.Models
{
    public class FarmRegisterModel
    {
        public string LocalSystemID { get; set; } = string.Empty;
        public SystemInfo SystemInfo { get; set; }
        public string LocalSystemAddress { get; set; } = string.Empty;
    }

    public class SystemInfo
    {
        public string Motherboard { get; set; } = string.Empty;
        public string CPU { get; set; } = string.Empty;
        public string OSVersion { get; set; } = string.Empty;
    }
}
