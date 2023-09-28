﻿using CryptonRemoteBack.Model.Views;
using Newtonsoft.Json;
using System.Net.Sockets;
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
            while (!webSocket.CloseStatus.HasValue)
            {
                List<FarmStatView> stats = new();
                foreach ((int farmId, string farmIp) in farms)
                {
                    FarmStatView? stat;
                    try
                    {
                        using TcpClient tcpClient = new();
                        await tcpClient.ConnectAsync(farmIp, 44444);
                        using NetworkStream stream = tcpClient.GetStream();
                        byte[] result = new byte[2048];
                        await stream.ReadAsync(result);

                        MinerMonitoringRecordNoCard? monitoring = JsonConvert
                            .DeserializeObject<MinerMonitoringRecordNoCard>(Encoding.UTF8.GetString(result));
                        stat = new(farmId, monitoring);
                    }
                    catch
                    {
                        stat = new(farmId, null);
                    }
                    stats.Add(stat);
                }

                byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(stats));
                //byte[] message = Encoding.UTF8.GetBytes(DateTime.Now.ToString()); //temp
                await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length),
                                          WebSocketMessageType.Text,
                                          true,
                                          CancellationToken.None);
                Thread.Sleep(1000);
            }
            await webSocket.CloseAsync(webSocket.CloseStatus.Value,
                                       webSocket.CloseStatusDescription,
                                       CancellationToken.None);
        }
    }
}
