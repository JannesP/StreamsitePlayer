using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StreamsitePlayer.Networking
{
    public class BufferedSocket
    {
        private Socket socket;

        public Socket Socket
        {
            get;
            set;
        }

        public byte[] ReceiveBuffer
        {
            get;
            set;
        }

        public byte[] SendBuffer
        {
            get;
            set;
        }

        public bool IsShutdown { get; private set; }

        public BufferedSocket(Socket socket, byte[] buffer)
        {
            this.Socket = socket;
            this.ReceiveBuffer = buffer;
        }

        private void Shutdown()
        {
            IsShutdown = true;
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
            }
        }

        public void Close()
        {
            if (socket != null)
            {
                if (!IsShutdown)
                {
                    Shutdown();
                }
                socket.Close();
            }
        }
    }
}
