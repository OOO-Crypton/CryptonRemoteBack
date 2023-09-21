using Newtonsoft.Json;

namespace CryptonRemoteBack.Model
{
    public static class FarmsHelpers
    {
        public static async Task<bool> StartFarm(string ip,
                                                 string minerName,
                                                 string poolAddress,
                                                 string walletName,
                                                 string additionalParams)
        {
            StartMinerParams parameters = new(walletName, poolAddress, additionalParams);
            string route = $"http://{ip}:5000/miner/{minerName}/start";
            string? res = await RequestHelpers.PostMessageAsync(parameters, route);
            if (res != null)
            {
                StartMinerReturn? result = JsonConvert.DeserializeObject<StartMinerReturn>(res);
                return result != null && string.IsNullOrWhiteSpace(result.stderr);
            }
            else return false;
        }

        public static async Task<bool> RestartFarm(string ip,
                                                   string minerName)
        {
            string route = $"http://{ip}:5000/miner/{minerName}/restart";
            string? res = await RequestHelpers.GetMessageAsync(route);
            if (res != null)
            {
                StartMinerReturn? result = JsonConvert.DeserializeObject<StartMinerReturn>(res);
                return result != null && string.IsNullOrWhiteSpace(result.stderr);
            }
            else return false;
        }

        public static async Task<bool> StopFarm(string ip,
                                                string minerName)
        {
            string route = $"http://{ip}:5000/miner/{minerName}/stop";
            string? res = await RequestHelpers.GetMessageAsync(route);
            if (res != null)
            {
                StartMinerReturn? result = JsonConvert.DeserializeObject<StartMinerReturn>(res);
                return result != null && string.IsNullOrWhiteSpace(result.stderr);
            }
            else return false;
        }
    }

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

    internal class StartMinerReturn
    {
        public int exitCode { get; set; }
        public string stdout { get; set; }
        public string stderr { get; set; }
    }
}
