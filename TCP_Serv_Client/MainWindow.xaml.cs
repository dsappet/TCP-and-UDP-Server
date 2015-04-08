using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;

namespace TCP_Serv_Client
{
    // delegate
    public delegate void UpdateInfoTxtDelegate(string str);
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // My class and delegate defines
        TcpServer server;
        MyTCPClient client;
        UdpListener listener;
        public static event UpdateInfoTxtDelegate UpdateInfoTxt;
        
        // default stuff
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            // Create new instance of classes and delegates
            UpdateInfoTxt = new UpdateInfoTxtDelegate(UpdateInfoTxtMethod); // need to define the delegate before the class, it gets passed to the class
            server = new TcpServer(UpdateInfoTxt, Window1);
            client = new MyTCPClient(UpdateInfoTxt);
            listener = new UdpListener(UpdateInfoTxt);
            server.servIP = Dns.GetHostAddresses(Dns.GetHostName())[0];
            client.ip = Dns.GetHostAddresses(Dns.GetHostName())[0];
            this.ServIP_txt.Text = server.servIP.ToString();
            server.servPort = Convert.ToInt32(this.servPort_txt.Text);
            client.port = Convert.ToInt32(this.servPort_txt.Text);
            this.statusBox_txt.Text = "";
            this.filePath_txt.Text = Directory.GetCurrentDirectory() + @"\vgsslogs\" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-tt") + ".txt";
        }

        private void StartServ_Btn_Click(object sender, RoutedEventArgs e)
        {
            //IPAddress ip = new IPAddress(new byte[] {198,151,152,78});
            //server.servIP = (ip);
            server.servPort = Convert.ToInt32(this.servPort_txt.Text);
            server.StartServ();
            UpdateInfoTxtMethod("Server Started");
        }

        private void StopServ_Btn_Click(object sender, RoutedEventArgs e)
        {
            server.StopServ();
            UpdateInfoTxtMethod("Server Stopped");
        }

        public void UpdateInfoTxtMethod(string str)
        {
            if (!statusBox_txt.Dispatcher.CheckAccess())
            {
                statusBox_txt.Dispatcher.BeginInvoke(UpdateInfoTxt, str);
            }
            else
            {
                string data = DateTime.Now.ToLongTimeString() + " - " + str;
                this.statusBox_txt.Text += data + Environment.NewLine;
                FileWriter fw = new FileWriter(this.filePath_txt.Text, data);
            }

        }

        private void ClientSendStr_Btn_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = new IPAddress(new byte[] { 192,168,100,102 });
            client.port = Convert.ToInt32(this.servPort_txt.Text);
            client.ip = ip;
            client.SendTcpData(this.SendStr_Txt.Text + Environment.NewLine);
        }

        private void servPort_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                server.servPort = Convert.ToInt32(this.servPort_txt.Text);
                client.port = Convert.ToInt32(this.servPort_txt.Text);
            }
        }

        private void statusBox_txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.statusBox_txt.ScrollToEnd();
        }

        private void startUdpListener_Btn_Click(object sender, RoutedEventArgs e)
        {
            listener.initUdp(Convert.ToInt32(udpPort_txt.Text));
            listener.Listen();
        }

    }
}
