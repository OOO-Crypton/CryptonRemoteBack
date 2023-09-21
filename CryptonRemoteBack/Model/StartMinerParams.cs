namespace CryptonRemoteBack.Model
{
    internal class StartMinerParams
    {
        public string walletName { get; set; }
        public string poolAddress { get; set; }
        public string additionalParams { get; set; }

        public StartMinerParams(string walletName, string poolAddress, string additionalParams)
        {
            this.walletName = walletName;
            this.poolAddress = poolAddress;
            this.additionalParams = additionalParams;
        }
    }
}
