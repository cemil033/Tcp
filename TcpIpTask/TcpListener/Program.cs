using System.Net;
using System.Net.Sockets;

namespace TCP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    string rd;
                    byte[] b1;
                    string v;
                    int m;
                    TcpListener list = new TcpListener(IPAddress.Parse("192.168.0.109"), 63291);
                    list.Start();
                    TcpClient client = list.AcceptTcpClient();
                    StreamReader sr = new StreamReader(client.GetStream());
                    rd = sr.ReadLine();
                    v = rd.Substring(rd.LastIndexOf('.') + 1);
                    m = int.Parse(v);
                    list.Stop();
                    client.Close();
                    list = new TcpListener(new IPEndPoint(IPAddress.Parse("192.168.0.109"), 63291));
                    list.Start();
                    TcpClient client1 = list.AcceptTcpClient();
                    Stream s = client1.GetStream();
                    b1 = new byte[m];
                    s.Read(b1, 0, b1.Length);
                    File.WriteAllBytes(@"C:\Users\User\Desktop" + "\\" + rd.Substring(0, rd.LastIndexOf('.')), b1);
                    list.Stop();
                    client1.Close();
                    Console.WriteLine("Complete Succssesful");
                }
            });
            Console.ReadKey();
        }
    }
}