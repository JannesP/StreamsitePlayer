using StreamsitePlayer.Networking.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamsitePlayer.Networking
{
    public class TcpServer
    {
        public event OnNetworkControlEventHandler NetworkControl;
        public event OnNetworkRequestEventHandler NetworkRequest;

        protected virtual void OnNetworkControl(NetworkControlEventArgs e)
        {
            if (NetworkControl != null)
            {
                NetworkControl(this, e);
            }
        }

        protected virtual void OnNetworkRequest(NetworkRequestEventArgs e)
        {
            if (NetworkRequest != null)
            {
                NetworkRequest(this, e);
            }
        }

        // Thread signal. 
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        private const int MSG_MAX_LENGTH = 512;
        private static Encoding CHAR_ENCODING = Encoding.UTF8;

        private List<BufferedSocket> clients = new List<BufferedSocket>();

        private TcpListener tcpListener;
        private Thread listenThread;
        private bool listen = false;


        public TcpServer(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listen = true;
            listenThread = new Thread(() => Listen(tcpListener));
            listenThread.Start();
        }

        private void Listen(TcpListener tcpListener)
        {
            try
            {
                tcpListener.Start();
                while (listen)
                {
                    // Set the event to nonsignaled state.
                    tcpClientConnected.Reset();

                    // Start to listen for connections from a client.
                    Logger.Log("TcpListener", "Waiting for a connection...");

                    // Accept the connection.  
                    // BeginAcceptSocket() creates the accepted socket.
                    tcpListener.BeginAcceptSocket(new AsyncCallback(ServerAcceptTcpClient), tcpListener);

                    // Wait until a connection is made and processed before  
                    // continuing.
                    tcpClientConnected.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            finally
            {
                tcpListener.Stop();
                Logger.Log("TcpListener", "Server closed!");
            }
        }

        public void ServerAcceptTcpClient(IAsyncResult ar)
        {
            if (!listen) return;
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;
            try
            {
                Socket client = listener.EndAcceptSocket(ar);
                byte[] buffer = new byte[MSG_MAX_LENGTH];
                BufferedSocket bufSoc = new BufferedSocket(client, buffer);
                clients.Add(bufSoc);
                bufSoc.Socket.BeginReceive(bufSoc.ReceiveBuffer, 0, bufSoc.ReceiveBuffer.Length, SocketFlags.None, ClientReceived, bufSoc);
            }
            catch (ObjectDisposedException)
            {
                Logger.Log("TcpListener", "TcpListener stopped! (stream disposed)");
            }
            // Signal the calling thread to continue.
            tcpClientConnected.Set();

        }

        private void ClientReceived(IAsyncResult ar)
        {
            BufferedSocket bufSoc = (BufferedSocket)ar.AsyncState;
            int bytesReceived = bufSoc.Socket.EndReceive(ar);
            if (bytesReceived == 0) //client wants to disconnect
            {
                bufSoc.Close();
                clients.Remove(bufSoc);
            }
            else
            {
                byte[] buffer = bufSoc.ReceiveBuffer;
                NetworkEventType type = (NetworkEventType)buffer[0];
                int id = buffer[1];
                byte[] data = null;
                if (buffer.Length > 2)
                {
                    data = new byte[buffer.Length - 2];
                    Array.Copy(buffer, 2, data, 0, data.Length);
                }
                switch (type)
                {
                    case NetworkEventType.Control:
                        var controlEvent = new NetworkControlEventArgs(bufSoc, (NetworkControlEvent)id, data);
                        OnNetworkControl(controlEvent);
                        break;
                    case NetworkEventType.Request:
                        var requestEvent = new NetworkRequestEventArgs(bufSoc, (NetworkRequestEvent)id, data);
                        OnNetworkRequest(requestEvent);
                        break;
                }
            }
        }

        private void ClientSendCallback(IAsyncResult ar)
        {
            BufferedSocket bufSoc = (BufferedSocket)ar.AsyncState;
            int sendBytes = bufSoc.Socket.EndSend(ar);
            if (sendBytes < bufSoc.SendBuffer.Length)
            {
                Logger.Log("NETWORKING", "Failed to send all of the data to " + bufSoc.Socket.RemoteEndPoint.ToString() + " ... " + (bufSoc.SendBuffer.Length - sendBytes) + " bytes are missing!");
            }
        }

        public void BroadcastMessage(string message)
        {
            // Send the message to all clients
            byte[] bytes = CHAR_ENCODING.GetBytes(message);
            if (bytes.Length > MSG_MAX_LENGTH) throw new Exception("Can't send more data then we are allowed to!");
            foreach (BufferedSocket bufSoc in clients)
            {
                bufSoc.SendBuffer = bytes;
                bufSoc.Socket.BeginSend(bufSoc.SendBuffer, 0, bufSoc.SendBuffer.Length, SocketFlags.Broadcast, ClientSendCallback, bufSoc);
            }
        }

        public void Stop()
        {
            if (listen)
            {
                foreach (BufferedSocket bufSoc in clients) bufSoc.Close();
                clients.Clear();
                listen = false;
                tcpClientConnected.Set();
            }
        }

    }
}
