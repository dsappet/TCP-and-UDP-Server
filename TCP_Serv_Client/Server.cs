using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Threading; // Dispatcher.CurrentDispatcher is held in this 
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
    public class TcpServer
    {
        static TcpListener listener;
        public int servPort;
        public IPAddress servIP;
        private bool runServ;
        private UpdateInfoTxtDelegate UpdateInfoTxt;
        private Window win;
        private TcpClient client; // this is the client that connected to our server

        // Constructor
        public TcpServer(UpdateInfoTxtDelegate d, Window w)
        {
            UpdateInfoTxt = d;
            win = w;
        }
        public void StartServ()
        {
            listener = new TcpListener(servIP, servPort);
            listener.Start();
            client = new TcpClient();
            //UpdateInfoTxt.Invoke("Listener Socket Started");
            Task server = new Task(TCPServService, TaskCreationOptions.LongRunning);
            server.Start();
        }

        private void TCPServService()
        {
            runServ = true;
            Stream stream; StreamReader sr; StreamWriter sw;
            string request = "";
            Socket socket;
            while (runServ)
            {
                
                if (listener.Pending()) // if there is a pending network connection (data available)
                {
                    socket = listener.AcceptSocket();
                    //win.Dispatcher.BeginInvoke(UpdateInfoTxt, "Pending Network Connection Found");
                    stream = new NetworkStream(socket);
                    stream.ReadTimeout = 10000;
                    stream.WriteTimeout = 10000;
                    sr = new StreamReader(stream);
                    sw = new StreamWriter(stream);
                    sw.AutoFlush = true;
                    try
                    {
                        int numBytes = socket.Available; // for debug
                        request = sr.ReadLine(); // This required a /n ($L) in VGSS land to terminate 
                        //int test = sr.Peek();
                        //request = sr.ReadToEnd();
                        //char[] bufr = new char[socket.Available+1];
                        //sr.Read(bufr, 0, socket.Available);
                        sw.WriteLine("ACCEPTED");
                        if (request != null) win.Dispatcher.BeginInvoke(UpdateInfoTxt, ("READ DATA: " + request));
                    }
                    catch (SocketException ex)
                    {
                        win.Dispatcher.BeginInvoke(UpdateInfoTxt, ex.Message);
                    }
                    catch (IOException ex)
                    {
                        win.Dispatcher.BeginInvoke(UpdateInfoTxt, ("NO Data Available from client, closing connection"));
                        win.Dispatcher.BeginInvoke(UpdateInfoTxt, ex.Message);                     
                    }
                    stream.Close(); 
                    socket.Close();
                                           
                }
                
                Thread.Sleep(5);
            }
            
        }
        private bool IsConnected(Socket s)
        {
            s.Blocking = false;
            s.Send(new byte[1], 0, 0);
            s.Blocking = true;
            if (s.Connected) return true; else return false;
        }
        public void StopServ()
        {
            runServ = false;
            listener.Stop();
        }
    }
}