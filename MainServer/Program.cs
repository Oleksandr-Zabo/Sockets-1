using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace MainServer;

class Program
{
    static void Main()
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        IPAddress ip = IPAddress.Parse("192.168.0.10");
        IPEndPoint ep = new IPEndPoint(ip, 80);
        s.Bind(ep);
        s.Listen(10);

        try
        {
            while (true)
            {
                Socket ns = s.Accept();
                Console.WriteLine(ns.RemoteEndPoint?.ToString());
                ns.Send(System.Text.Encoding.Unicode.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture)));
                ns.Shutdown(SocketShutdown.Both);
                ns.Close();
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Error server: {ex.Message}");
        }
    }
}