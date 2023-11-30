using CryptonRemoteBack.Model.Models;
using CryptonRemoteBack.Model.Views;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace CryptonRemoteBack.Model
{
    public static class FarmsHelpers
    {
        /// <summary>
        /// Запуск
        /// </summary>
        /// <param name="ip">Адрес фермы</param>
        /// <param name="minerName">Название майнера</param>
        /// <param name="poolAddress">Адрес пула</param>
        /// <param name="walletName">Название кошелька</param>
        /// <param name="additionalParams">Дополнительные параметры командной строки</param>
        /// <returns>true, если запуск успешен</returns>
        public static async Task<bool> StartFarm(string ip,
                                                 string minerName,
                                                 string poolAddress,
                                                 string walletName,
                                                 string additionalParams)
        {
            StartMinerParams parameters = new(walletName, poolAddress, additionalParams);
            string route = $"http://{ip}:8080/miner/{minerName}/start";
            string? res = await RequestHelpers.PostMessageAsync(parameters, route);
            if (res != null)
            {
                try
                {
                    MinerReturn? result = JsonConvert.DeserializeObject<MinerReturn>(res);
                    return result != null && string.IsNullOrWhiteSpace(result.stderr);
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Перезапуск
        /// </summary>
        /// <param name="ip">Адрес фермы</param>
        /// <param name="minerName">Название майнера</param>
        /// <returns>true, если перезапуск успешен</returns>
        public static async Task<bool> RestartFarm(string ip,
                                                   string minerName)
        {
            string route = $"http://{ip}:8080/miner/{minerName}/restart";
            string? res = await RequestHelpers.GetMessageAsync(route);
            if (res != null)
            {
                try
                {
                    MinerReturn? result = JsonConvert.DeserializeObject<MinerReturn>(res);
                    return result != null && string.IsNullOrWhiteSpace(result.stderr);
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Остановка
        /// </summary>
        /// <param name="ip">Адрес фермы</param>
        /// <param name="minerName">Название майнера</param>
        /// <returns></returns>
        public static async Task<bool> StopFarm(string ip,
                                                string minerName)
        {
            string route = $"http://{ip}:8080/miner/{minerName}/stop";
            string? res = await RequestHelpers.GetMessageAsync(route);
            if (res != null)
            {
                try
                {
                    MinerReturn? result = JsonConvert.DeserializeObject<MinerReturn>(res);
                    return result != null && string.IsNullOrWhiteSpace(result.stderr);
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Получение архива данных мониторинга
        /// </summary>
        /// <param name="ip">Адрес фермы</param>
        /// <returns>Данные мониторинга</returns>
        public static async Task<List<MinerMonitoringRecord>?> GetMonitorings(string ip)
        {
            string route = $"http://{ip}:8080/monitoring/list";
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


        /// <summary>
        /// Получение текущих значений мониторинга
        /// </summary>
        /// <param name="webSocket">Веб-сокет, на который прееправляются данные</param>
        /// <param name="farms">Данные о фермах</param>
        public async static Task GetStats(WebSocket webSocket, List<(int farmId, int fsId, string farmIp)> farms)
        {
            List<FarmStatView> stats = new();
            foreach ((int farmId, int fsId, string farmIp) in farms)
            {
                if (string.IsNullOrWhiteSpace(farmIp)) continue;
                FarmStatView? stat;
                try
                {
                    using TcpClient tcpClient = new();
                    await tcpClient.ConnectAsync(farmIp, 44444);
                    using NetworkStream stream = tcpClient.GetStream();
                    byte[] result = new byte[1024 * 10];

                    await stream.ReadAsync(result);
                    tcpClient.Close();
                    string resultString = Encoding.UTF8.GetString(result);
                    List<VideocardView>? monitoring = JsonConvert
                        .DeserializeObject<List<VideocardView>>(resultString);
                    stat = new(farmId, fsId, monitoring, "OK");
                }
                catch (Exception ex)
                {
                    stat = new(farmId, fsId, null, $"Failed to connect: {ex.Message}");
                }
                stats.Add(stat);
            }

            if (stats.Any())
            {
                byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(stats));
                await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length),
                                          WebSocketMessageType.Text,
                                          true,
                                          CancellationToken.None);
            }

            Thread.Sleep(5000);
        }


        /// <summary>
        /// Разгон
        /// </summary>
        /// <param name="ip">Адрес фермы</param>
        /// <param name="input">Параметры разгона</param>
        /// <returns>true, если успешно</returns>
        public static async Task<bool> Overclocking(string ip,
                                                   FarmOverclockingModel input)
        {
            string route = $"http://{ip}:8080/overclocking/settings";
            string? res = await RequestHelpers.PostMessageAsync(input, route);
            if (res != null)
            {
                try
                {
                    MinerReturn? result = JsonConvert.DeserializeObject<MinerReturn>(res);
                    return result != null && string.IsNullOrWhiteSpace(result.stderr);
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }
    }
}
