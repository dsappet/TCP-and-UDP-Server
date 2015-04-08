using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
using System.Net.Sockets;

namespace TCP_Serv_Client
{
    public class MyTCPClient
    {
        public UpdateInfoTxtDelegate UpdateInfoTxt;
        public IPAddress ip;
        public int port;
        public byte[] dataByte;
        // Constructor
        public MyTCPClient(UpdateInfoTxtDelegate d)
        {
            UpdateInfoTxt = d;
        }
        public void SendTcpData(string sendStr)
        {
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            bool connected = client.Connected;
            NetworkStream clientStream = client.GetStream();
            //StreamWriter sw = new StreamWriter(clientStream);
            dataByte = ASCIIEncoding.ASCII.GetBytes(sendStr);
            clientStream.Write(dataByte,0,sendStr.Length);
            //sw.WriteLine(sendStr);
            clientStream.Close();
            client.Close();
        }
    }
}