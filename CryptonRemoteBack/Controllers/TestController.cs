using System.Net.WebSockets;
using CryptonRemoteBack.Model;
using Microsoft.AspNetCore.Mvc;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController() { }

        [HttpGet("/api/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                byte[] buffer = new byte[1024 * 4];
                var task = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!task.IsCompleted)
                {
                    await FarmsHelpers.GetStats(webSocket, new List<(int, int, string)>() { (0, 0, "192.168.0.244") });
                    Thread.Sleep(1000);
                }
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                           "ended",
                                           CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }

            //if (HttpContext.WebSockets.IsWebSocketRequest)
            //{
            //    using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            //    while (!webSocket.CloseStatus.HasValue)
            //    {
            //        byte[] message = Encoding.UTF8.GetBytes(DateTime.Now.ToString()); //temp
            //        await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length),
            //                                  WebSocketMessageType.Text,
            //                                  true,
            //                                  CancellationToken.None);
            //        Thread.Sleep(1000);
            //    }
            //}
            //else
            //{
            //    HttpContext.Response.StatusCode = 400;
            //}
        }
    }
}
