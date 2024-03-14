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
    public class recievePipe
    {
        NamedPipeServerStream pipeServer;
        StreamString ss;

        public recievePipe()
        {
            pipeServer =
                new NamedPipeServerStream("recieveTorPipe", PipeDirection.InOut, 5);
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

    }

}
