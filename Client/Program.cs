using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

class Client
{
    static async Task Main()
    {
        try
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 8080);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await clientSocket.ConnectAsync(ep);

            string message = "Hello, Server!";
            await clientSocket.SendAsync(Encoding.UTF8.GetBytes(message), SocketFlags.None);

            byte[] buffer = new byte[1024];
            int received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"At {DateTime.Now:HH:mm} from {((IPEndPoint)clientSocket.RemoteEndPoint!)?.Address} received: {receivedMessage}");

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}