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
    public class Privilege
    {
        [DataMember]
        public string Applicacion
        {
            get;
            set;
        }

        [DataMember]
        public string Command
        {
            get;
            set;
        }

        [DataMember]
        public bool IsEnable
        {
            get;
            set;
        }
    }
}
