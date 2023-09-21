namespace CryptonRemoteBack.Model
{
    internal class StartMinerReturn
    {
        public int exitCode { get; set; }
        public string stdout { get; set; }
        public string stderr { get; set; }
    }
}
