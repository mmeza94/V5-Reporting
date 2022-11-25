using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.SecurityAdapter.Shared
{
    public class SecurityUser
    {
       

        [DataMember]
        public string UserID;

        [DataMember]
        public string Name;

        [DataMember]
        public string Role;

        [DataMember]
        public bool IsAuthenticate;

        [DataMember]
        public Guid Token;

        [DataMember]
        public string Mail;

        [DataMember]
        public string FailOver;

        [DataMember]
        public string ContryCode;

        [DataMember]
        public string ObjectSid;

        [DataMember]
        public int BadPwdCount;

        [DataMember]
        public string WhenCreate;

        [DataMember]
        public DateTime AccountExpires;

        [DataMember]
        public string DisplayName;

        [DataMember]
        public DateTime LastLogon;

        [DataMember]
        public DateTime LockoutTime;

        [DataMember]
        public DateTime PwdLastSet;

        [DataMember]
        public int LogonCount;

        [DataMember]
        public DateTime BadPwdTime;

        [DataMember]
        public string TelefonoNumber;

        [DataMember]
        public DateTime LastLogonTimeStamp;

        [DataMember]
        public string WhenChanged;

      

        [DataMember]
        public string Location;

        public SecurityUser()
        {
        }

    }
}
