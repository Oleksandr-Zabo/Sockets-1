using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

class Client
{
    static void Main()
    {
        try
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 8080);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ep);

            Console.Write("Enter 'time' or 'date': ");
            string request = Console.ReadLine()?.Trim().ToLower() ?? "invalid";
            clientSocket.Send(Encoding.UTF8.GetBytes(request));

            byte[] buffer = new byte[1024];
            int received = clientSocket.Receive(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, received);

            Console.WriteLine($"Server response: {response}");

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
    }
}