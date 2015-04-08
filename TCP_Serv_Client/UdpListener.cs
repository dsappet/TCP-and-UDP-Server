using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Net;
using System.Net.Sockets;


namespace TCP_Serv_Client
{
    public class UdpListener
    {
        private UdpClient client;
        private IPEndPoint sender;
        private UpdateInfoTxtDelegate UpdateInfoTxt;
        // public int port; // TODO add more dynamic capability for changing listener port. 
        bool stop;
        public UdpListener(UpdateInfoTxtDelegate d)
        {
            UpdateInfoTxt = d;
        }
        public void initUdp(int port)
        {
            try
            {
                client = new UdpClient(port); // listen on PORT 514 (syslog of Perle)
                sender = new IPEndPoint(IPAddress.Any, port);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Internal Exception occured while attempting to init UDP client and sender. :::: " + ex.ToString());
            }
            stop = false;
        }
        public void Listen()
        {
            Task t = new Task(ListenThread);
            t.Start(); 
        }
        private void ListenThread()
        {
            byte[] data = new byte[1024];
            string stringData;
            stop = false;
            while (!stop)
            {
                data = client.Receive(ref sender);
                stringData = Encoding.ASCII.GetString(data, 0, data.Length);
                UpdateInfoTxt(stringData);
            }
            
        }
    }
}
