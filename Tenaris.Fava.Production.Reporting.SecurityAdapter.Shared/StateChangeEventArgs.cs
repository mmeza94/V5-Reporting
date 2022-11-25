using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.SecurityAdapter.Shared
{
    [Serializable]
    public class StateChangeEventArgs : EventArgs
    {
        private object _connection;

        private bool _isConnected;

        public bool IsConnected => _isConnected;

        public bool IsDisconnected => !IsConnected;

        public object Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        public StateChangeEventArgs()
        {
        }

        public StateChangeEventArgs(bool isConnected, object connection)
        {
            _isConnected = isConnected;
            Connection = connection;
        }
    }
}
