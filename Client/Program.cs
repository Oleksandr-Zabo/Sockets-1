using System.Net;
using System.Net.Sockets;

namespace Client;

class Program
{
    static void Main()
    {
        IPAddress ip = IPAddress.Parse("192.168.0.10");
        IPEndPoint ep = new IPEndPoint(ip, 80);
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            s.Connect(ep);
            if (s.Connected)
            {
                string strSend = "GET / HTTP/1.1\r\nHost: 192.168.0.10\r\nConnection: close\r\n\r\n";
                s.Send(System.Text.Encoding.ASCII.GetBytes(strSend));
                byte[] buffer = new byte[1024];
                int l;
                do
                {
                    l = s.Receive(buffer);
                    Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer, 0, l));
                } while (l > 0);
            }
            else
            {
                Console.WriteLine("Not connected");
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Error client: {ex.Message}");
        }
        finally
        {
            if (s.Connected)
            {
                s.Shutdown(SocketShutdown.Both);
            }
            s.Close();
        }
    }
}