using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.SecurityAdapter.Shared
{
    [Serializable]
    [DataContract]
    public class ApplicationCommand
    {
        [DataMember]
        public string Command
        {
            get;
            set;
        }

        [DataMember]
        public int Level
        {
            get;
            set;
        }
    }
}
