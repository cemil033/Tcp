using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace TCPClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }
        TcpClient tcpClient;
        private void FileDrop_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filename = Path.GetFileName(files[0]);
                FileNameLabel.Content = filename;

                FileInfo fi = new FileInfo(files[0]);
                var n = fi.Name + "." + fi.Length;
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.109"), 63291));
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.WriteLine(n);
                sw.Flush();
                Thread.Sleep(3000);
                MessageBox.Show("File Transferred....");
                TcpClient client1 = new TcpClient();
                client1.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.109"), 63291));
                Stream s = client1.GetStream();
                var b1 = File.ReadAllBytes(files[0]);
                s.Write(b1, 0, b1.Length);
                client.Close();
                MessageBox.Show("File Transferred 2....");
            }

        }
    }
}
