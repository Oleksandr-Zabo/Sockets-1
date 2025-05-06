using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MainServer;

class AsyncServer
{
    static async Task Main()
    {
        try
        {
            IPAddress ip = IPAddress.Any;
            IPEndPoint ep = new IPEndPoint(ip, 8080);

            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ep);
            serverSocket.Listen(5);

            Console.WriteLine("Server is waiting for connections...");

            while (true)
            {
                try
                {
                    Socket clientSocket = await serverSocket.AcceptAsync();
                    _ = HandleClientAsync(clientSocket);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Error accepting connection: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server error: {ex.Message}");
        }
    }

    static async Task HandleClientAsync(Socket clientSocket)
    {
        try
        {
            byte[] buffer = new byte[1024];
            int received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine($"At {DateTime.Now:HH:mm} from {((IPEndPoint)clientSocket.RemoteEndPoint!)?.Address} received: {receivedMessage}");

            string response = "Hello, Client!";
            await clientSocket.SendAsync(Encoding.UTF8.GetBytes(response), SocketFlags.None);
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Client handling error: {ex.Message}");
        }
        finally
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}