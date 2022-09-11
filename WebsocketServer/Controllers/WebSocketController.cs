using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace WebSocketsSample.Controllers;

//[ApiController]
//[Route("[controller]")]
public class WebSocketController : ControllerBase
{
    [HttpGet("/ws")]
    public async Task<string> Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using Task<WebSocket> webSocket = HttpContext.WebSockets.AcceptWebSocketAsync();
            EchoAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }

        await Task.Delay(200);

        return "Websocket";
    }
    // </snippet>

    private static async void EchoAsync(Task<WebSocket> webSocket1)
    {
        var webSocket = await webSocket1;

        var buffer = new byte[1024 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}
