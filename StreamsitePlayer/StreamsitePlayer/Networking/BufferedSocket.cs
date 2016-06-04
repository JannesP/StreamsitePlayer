using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPlayer.Networking
{
    public class BufferedSocket
    {
        private bool isReadyToSend = true;
        private Queue<SendData> sendQueue = new Queue<SendData>();

        private class SendData
        {
            public byte[] Buffer { get; set; }
            public int Offset { get; set; }
            public int Size { get; set; }
            public SocketFlags Flags { get; set; }
            public AsyncCallback Callback { get; set; }
        }

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

        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags flags, AsyncCallback callback)
        {
            if (Socket != null)
            {
                return Socket.BeginReceive(buffer, offset, size, flags, callback, this);
            }
            else
            {
                return null;
            }
        }

        public int EndReceive(IAsyncResult asyncResult)
        {
            if (Socket != null)
            {
                try
                {
                    return Socket.EndReceive(asyncResult);
                }
                catch (ObjectDisposedException)
                {
                    return -1;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public void BeginSend(byte[] buffer, int offset, int size, SocketFlags flags, AsyncCallback callback)
        {
            BeginSend(buffer, offset, size, flags, callback, false);
        }

        private void BeginSend(byte[] buffer, int offset, int size, SocketFlags flags, AsyncCallback callback, bool queuedSend)
        {
            if (Socket != null)
            {
                if (isReadyToSend || queuedSend)
                {
                    isReadyToSend = false;
                    SendBuffer = buffer;
                    try
                    {
                        Socket.BeginSend(SendBuffer, offset, size, flags, callback, this);
                    }
                    catch (SocketException ex)
                    {
                        Logger.Log("BufferedSocket", ex.Message);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        Logger.Log("BufferedSocket", ex.Message);
                    }
                }
                else
                {
                    SendData sd = new SendData();
                    sd.Buffer = buffer;
                    sd.Offset = offset;
                    sd.Size = size;
                    sd.Flags = flags;
                    sd.Callback = callback;
                    sendQueue.Enqueue(sd);
                }
            }
        }

        private bool CheckForQueuedSend()
        {
            if (sendQueue.Count != 0)
            {
                SendData sd = sendQueue.Dequeue();
                BeginSend(sd.Buffer, sd.Offset, sd.Size, sd.Flags, sd.Callback, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int EndSend(IAsyncResult asyncResult)
        {
            if (Socket != null)
            {
                if (!CheckForQueuedSend())
                {
                    isReadyToSend = true;
                }
                return Socket.EndSend(asyncResult);
            }
            else
            {
                return 0;
            }
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
            if (Socket != null)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
        }

        public void Close()
        {
            if (Socket != null)
            {
                if (!IsShutdown)
                {
                    Shutdown();
                }
                Socket.Close();
            }
        }
    }
}
