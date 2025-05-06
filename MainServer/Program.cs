using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MainServer;

class Server
{
    static void Main()
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
                Socket clientSocket = serverSocket.Accept();
                byte[] buffer = new byte[1024];
                int received = clientSocket.Receive(buffer);
                string request = Encoding.UTF8.GetString(buffer, 0, received).Trim();

                string response = request.ToLower() switch
                {
                    "time" => DateTime.Now.ToString("HH:mm:ss"),
                    "date" => DateTime.Now.ToString("yyyy-MM-dd"),
                    _ => "Invalid request"
                };

                clientSocket.Send(Encoding.UTF8.GetBytes(response));
                Console.WriteLine($"Sent: {response}");

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
        }
    }
}