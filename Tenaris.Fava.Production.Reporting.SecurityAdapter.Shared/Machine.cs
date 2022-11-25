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
    public class Machine
    {
        [DataMember]
        public string Code
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }

        [DataMember]
        public string SortOrder
        {
            get;
            set;
        }
    }
}
