using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorGUI
{
    static class Global
    {
        public static pipe _enginePipe;
        public static recievePipe _engineRecievePipe;
        public static string _username;

        public static pipe enginePipe
        {
            get { return _enginePipe; }
            set { _enginePipe = value; }
        }

        public static recievePipe engineRecievePipe
        {
            get { return _engineRecievePipe; }
            set { _engineRecievePipe = value; }
        }

        public static string username
        {
            get { return _username; }
            set { _username = value; }
        }
    }
}
