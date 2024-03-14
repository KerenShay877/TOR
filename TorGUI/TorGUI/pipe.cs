using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Pipes;
using System.IO;
using System.Threading;


namespace TorGUI
{
    public class pipe
    {
        NamedPipeServerStream pipeServer;
        StreamString ss ;

        public pipe()
        {
            pipeServer =
                new NamedPipeServerStream("torPipe", PipeDirection.InOut, 5);
            ss = new StreamString(pipeServer);
       

        }

        public bool connect()
        {

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            return pipeServer.IsConnected;
        }

        public bool isConnected()
        {
            return pipeServer.IsConnected;
        }

        public string receive()
        {
            if (!isConnected())
                return "";

            string res = ss.receiveFromEngine();

            Console.WriteLine("code from engine " + res);


            return res;
        }

        public void send(string move)
        {
            if (!isConnected())
                return;

            ss.sendToEngine(move);
        }

        public void close()
        {
            pipeServer.Close();
        }
    }


    
    public class StreamString
    {
        private Stream ioStream;
        private Encoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new ASCIIEncoding();
        }

        public string receiveFromEngine()
        {
          
            byte[] inBuffer = new byte[1024];
            ioStream.Read(inBuffer, 0, 1024);


            String MyString = Encoding.ASCII.GetString(inBuffer).TrimEnd((Char)0);
            return Encoding.ASCII.GetString(inBuffer).TrimEnd((Char)0);
        }

        public void sendToEngine(string outString)
        {
         
            byte[] t = Encoding.ASCII.GetBytes(outString);
            byte[] inBuffer = new byte[t.Length + 1];

            for (int i = 0; i < t.Length; i++ )
            {
                inBuffer[i] = t[i];
            }
            inBuffer[inBuffer.Length - 1] = 0;

            try
            {
                ioStream.Write(inBuffer, 0, inBuffer.Length);

                ioStream.Flush();
            }
            catch
            {

            }
        }
    }

}
