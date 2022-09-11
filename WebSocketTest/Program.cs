using System.Net.WebSockets;

namespace WebSocketTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ClientWebSocket webSocket = new ClientWebSocket();

            CancellationTokenSource source = new CancellationTokenSource();

            await webSocket.ConnectAsync(new Uri("wss://localhost:44394/ws"), source.Token);

            var data = System.Text.Encoding.UTF8.GetBytes("Hello World!");

            await webSocket.SendAsync(data, WebSocketMessageType.Text, true, source.Token);

            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            var receiveValue = System.Text.Encoding.UTF8.GetString(buffer);

            Console.WriteLine(receiveValue);
        }

        private static async Task Echo(WebSocket webSocket)
        {
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
}