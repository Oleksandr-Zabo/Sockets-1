using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        IPEndPoint ep = new IPEndPoint(ip, 8080);

        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(ep);

        string message = "Hello, Server!";
        clientSocket.Send(Encoding.UTF8.GetBytes(message));

        byte[] buffer = new byte[1024];
        int received = clientSocket.Receive(buffer);
        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, received);

        Console.WriteLine($"At {DateTime.Now:HH:mm} from {((IPEndPoint)clientSocket.RemoteEndPoint).Address} received: {receivedMessage}");

        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}