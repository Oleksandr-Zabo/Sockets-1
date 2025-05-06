using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MainServer;

class MainServer
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
            Socket clientSocket = serverSocket.Accept();
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(clientSocket);
        }
    }

    static void HandleClient(object obj)
    {
        Socket clientSocket = (Socket)obj;
        byte[] buffer = new byte[1024];

        int received = clientSocket.Receive(buffer);
        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, received);
        Console.WriteLine($"At {DateTime.Now:HH:mm} from {((IPEndPoint)clientSocket.RemoteEndPoint!)?.Address} received: {receivedMessage}");

        string response = "Hello, Client!";
        clientSocket.Send(Encoding.UTF8.GetBytes(response));

        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}