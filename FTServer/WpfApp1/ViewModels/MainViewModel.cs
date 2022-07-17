using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media.Imaging;

namespace WpfApp1.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        ObservableCollection<Image> images { get; set; } =new ObservableCollection<Image>();
        public TcpClient client;
        public IPAddress ip;
        public BinaryReader br;
        public IPEndPoint ep;
        public int port;
        public Socket hostSocket;
        public Thread thread;
        public void Load()
        {
            try
            {
                Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint hostIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.109"), 63291);                
                receiveSocket.Bind(hostIpEndPoint);
                receiveSocket.Listen(10);                
                hostSocket = receiveSocket.Accept();
                trreadimage();                
            }
            catch (Exception)
            {               
            }
            
        }
        public BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }
        private void trreadimage()
        {
            int dataSize;
            string imageName = "Image-" + System.DateTime.Now.Ticks + ".JPG";
            try
            {

                dataSize = 0;
                byte[] b = new byte[1024 * 10000];  
                dataSize = hostSocket.Receive(b);
                if (dataSize > 0)
                {
                    MemoryStream ms = new MemoryStream(b);
                    var img =(Bitmap)Image.FromStream(ms);
                    img.Save(imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    
                    Src = BitmapToBitmapSource(img);
                    ms.Close();
                }

            }
            catch (Exception)
            {
                
                
            }
            
        }
        ImageConverter _imageConverter = new ImageConverter();
        public  Bitmap GetImageFromByteArray(byte[] byteArray)
        {
            Bitmap bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {                
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
                                 (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }
        private BitmapSource src;
        public BitmapSource Src
        {
            get => src; set
            {
                src = value;                
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            Load();
        }
    }
}
