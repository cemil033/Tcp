using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FTServerr
{
    internal class Program
    {
        static Socket sendsocket;
        static void Main(string[] args)
        {
            
            try
            {
                sendsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ipendpoint = new IPEndPoint(IPAddress.Parse("192.168.0.109"), 63291);
                sendsocket.Connect(ipendpoint);
                if (sendsocket.Connected)
                {
                    while (true)
                    {                        
                        threadimage();
                        Thread.Sleep(10000);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);                
            }
            
            
        }
        static byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        static private void threadimage()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GetScreen().Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);   
                byte[] b = ms.ToArray();
                sendsocket.Send(b);
                ms.Close();
            }
            catch (Exception)
            {                
            }            
        }
        static private Bitmap GetScreen()
        {
            Bitmap bitmap = new Bitmap(1920,1080);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            return bitmap;
        }
    }
}
