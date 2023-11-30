namespace CryptonRemoteBack.Model.Models
{
    public class FarmRegisterModel
    {
        public string LocalSystemID { get; set; } = string.Empty;
        public SystemInfo SystemInfo { get; set; }
        public string LocalSystemAddress { get; set; } = string.Empty;
    }
}
