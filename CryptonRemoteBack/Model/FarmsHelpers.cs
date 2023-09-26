using CryptonRemoteBack.Model.Views;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

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


        public async static Task GetStats(WebSocket webSocket, List<(int farmId, string farmIp)> farms)
        {
            byte[] mainBuf = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket
                .ReceiveAsync(new ArraySegment<byte>(mainBuf),
                              CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                List<FarmStatView> stats = new();
                //foreach ((int farmId, string farmIp) in farms)
                //{
                //    string route = $"wss://{farmIp}:44444";
                //    FarmStatView? stat;
                //    try
                //    {
                //        var buffer = new byte[1024 * 4];
                //        ClientWebSocket clientWebSocket = new();
                //        await clientWebSocket.ConnectAsync(new Uri(route), CancellationToken.None);
                //        _ = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);

                //        string json = Encoding.UTF8.GetString(buffer);
                //        await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                //                                         null,
                //                                         CancellationToken.None);

                //        MinerMonitoringRecord? monitoring = JsonConvert.DeserializeObject<MinerMonitoringRecord>(json);
                //        stat = new(farmId, monitoring);
                //    }
                //    catch
                //    {
                //        stat = new(farmId, null);
                //    }
                //    stats.Add(stat);
                //}

                //byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(stats));
                byte[] message = Encoding.UTF8.GetBytes(DateTime.Now.ToString());
                await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length),
                                          WebSocketMessageType.Text,
                                          true,
                                          CancellationToken.None);
                mainBuf = new byte[1024 * 4];
                result = await webSocket
                    .ReceiveAsync(new ArraySegment<byte>(mainBuf), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value,
                                       result.CloseStatusDescription,
                                       CancellationToken.None);
        }
    }
}
