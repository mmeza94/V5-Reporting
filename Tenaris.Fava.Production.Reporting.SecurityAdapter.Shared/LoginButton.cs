using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.SecurityAdapter.Shared
{
    [Serializable]
    [Flags]
    [DataContract]
    public enum LoginButton
    {
        None = 0x0,
        Login = 0x1,
        Cancel = 0x2,
        Logoff = 0x4
    }
}
