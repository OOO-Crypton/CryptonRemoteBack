using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public TestController() { }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                while (!webSocket.CloseStatus.HasValue)
                {
                    byte[] message = Encoding.UTF8.GetBytes(DateTime.Now.ToString()); //temp
                    await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length),
                                              WebSocketMessageType.Text,
                                              true,
                                              CancellationToken.None);
                    Thread.Sleep(1000);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}
