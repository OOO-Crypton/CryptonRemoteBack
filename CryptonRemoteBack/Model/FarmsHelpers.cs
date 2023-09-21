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

        public static async Task<List<MinerMonitoringRecord>?> GetMonitorings(string ip)
        {
            string route = $"http://{ip}:5000/monitoring/list";
            string? res = await RequestHelpers.GetMessageAsync(route);
            if (res != null)
            {
                try
                {
                    List<MinerMonitoringRecord>? result = JsonConvert.DeserializeObject<List<MinerMonitoringRecord>>(res);
                    return result;
                }
                catch
                {
                    return null;
                }
            }
            else return null;
        }
    }
}
