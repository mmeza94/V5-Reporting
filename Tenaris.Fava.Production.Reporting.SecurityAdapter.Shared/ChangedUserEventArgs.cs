using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.SecurityAdapter.Shared
{    
    public class ChangedUserEventArgs : EventArgs
    {
        public string UserID
        {
            get;
            set;
        }

        public ChangedUserEventArgs(string userID)
        {
            UserID = userID;
        }
    }
}
