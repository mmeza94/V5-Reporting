using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.SecurityAdapter.Shared;
using Tenaris.Manager.Security;

namespace Tenaris.Fava.Production.Reporting.SecurityAdapter
{
    public class SecurityAd : ISecurityAdapter
    {
        public SecurityAd()
        {
            Security.Notify += new EventHandler<Manager.Security.ChangedUserEventArgs>(this.OnNotify);
        }
        public event EventHandler<Shared.ChangedUserEventArgs> Notify;

        public bool CheckDynamicPassword(string password, int level)
        {
            return Security.CheckDynamicPassword(password, level);
        }

        public Shared.SecurityUser CheckUser(string user, string password)
        {
            Manager.Security.SecurityUser response = Security.CheckUser(user, password);
            Shared.SecurityUser SU = new Shared.SecurityUser()
            {
                UserID = response.UserID,
                Name = response.Name,
                Role = response.Role,
                IsAuthenticate = response.IsAuthenticate,
                Token = response.Token,
                Mail = response.Mail,
                FailOver = response.FailOver,
                ContryCode = response.ContryCode,
                ObjectSid = response.ObjectSid,
                BadPwdCount = response.BadPwdCount,
                WhenCreate = response.WhenCreate,
                AccountExpires = response.AccountExpires,
                DisplayName = response.DisplayName,
                LastLogon = response.LastLogon,
                LockoutTime = response.LockoutTime,
                PwdLastSet = response.PwdLastSet,
                LogonCount = response.LogonCount,
                BadPwdTime = response.BadPwdTime,
                TelefonoNumber = response.TelefonoNumber,
                LastLogonTimeStamp = response.LastLogonTimeStamp,
                WhenChanged = response.WhenChanged,
                Location = response.Location
            };

            return SU;
        }

        public Shared.SecurityUser CheckUser()
        {

            Manager.Security.SecurityUser response = Security.CheckUser();
            Shared.SecurityUser SU = new Shared.SecurityUser()
            {
                UserID = response.UserID,
                Name = response.Name,
                Role = response.Role,
                IsAuthenticate = response.IsAuthenticate,
                Token = response.Token,
                Mail = response.Mail,
                FailOver = response.FailOver,
                ContryCode = response.ContryCode,
                ObjectSid = response.ObjectSid,
                BadPwdCount = response.BadPwdCount,
                WhenCreate = response.WhenCreate,
                AccountExpires = response.AccountExpires,
                DisplayName = response.DisplayName,
                LastLogon = response.LastLogon,
                LockoutTime = response.LockoutTime,
                PwdLastSet = response.PwdLastSet,
                LogonCount = response.LogonCount,
                BadPwdTime = response.BadPwdTime,
                TelefonoNumber = response.TelefonoNumber,
                LastLogonTimeStamp = response.LastLogonTimeStamp,
                WhenChanged = response.WhenChanged,
                Location = response.Location
            };

            return SU;
        }

        public void ConnectionMonitor_StateChanged(object sender, StateChangeEventArgs e)
        {
            
        }

        public int CreateGroup(string Area, string Zone, string Name, string Code, string Description, bool Active)
        {
            return Security.CreateGroup(Area, Zone, Name, Code, Description, Active);
        }

        public void Dispose()
        {
            Security.Dispose();
        }

        public bool DynamicPassword(int level)
        {
            return DynamicPassword(level);
        }

        public List<Shared.Application> GetAllApplication()
        {
            List<Shared.Application> list = new List<Shared.Application>();
            List<Manager.Security.Application> security = Security.GetAllApplication();
            if (security != null)
            {
                foreach (var item in security)
                {
                    list.Add(new Shared.Application()
                    {
                        Code = item.Code,
                        Description = item.Description,
                        ExeName = item.ExeName,
                        IsManager = item.IsManager,
                        Name = item.Name
                    });
                }
            }
            return list;
        }

        public List<Shared.Area> GetAllArea()
        {
            List<Shared.Area> list = new List<Shared.Area>();
            List<Manager.Security.Area> security = Security.GetAllArea();
            if (security != null)
            {
                foreach (var item in security)
                {
                    list.Add(new Shared.Area()
                    {
                        Code = item.Code, Description = item.Description, Name = item.Name
                    });
                }
            }
            return list;
        }

        public List<Shared.Group> GetAllGroup()
        {
            List<Shared.Group> list = new List<Shared.Group>();
            List<Manager.Security.Group> security = Security.GetAllGroup();
            if (security != null)
            {
                foreach (var item in security)
                {
                    list.Add(new Shared.Group()
                    {
                        Code = item.Code,
                        Description = item.Description,
                        Name = item.Name,
                        Zone = item.Zone
                    });
                }
            }
            return list;
        }

        public List<Shared.Machine> GetAllMachine()
        {
            throw new NotImplementedException();
        }

        public List<Shared.SecurityUser> GetAllUser()
        {
            throw new NotImplementedException();
        }

        public List<Shared.Workstation> GetAllWorkstation()
        {
            throw new NotImplementedException();
        }

        public List<Shared.Zone> GetAllZone()
        {
            throw new NotImplementedException();
        }

        public List<Shared.Application> GetApplication(string User)
        {
            throw new NotImplementedException();
        }

        public List<Shared.ApplicationCommand> GetCommandForApplication(string application)
        {
            throw new NotImplementedException();
        }

        public List<Shared.ApplicationCommand> GetCommandForGroup(string groupname)
        {
            throw new NotImplementedException();
        }

        public Shared.SecurityUser GetCurrentUser()
        {
            Manager.Security.SecurityUser response = Security.GetCurrentUser();
            Shared.SecurityUser SU = new Shared.SecurityUser()
            {
                UserID = response.UserID,
                Name = response.Name,
                Role = response.Role,
                IsAuthenticate = response.IsAuthenticate,
                Token = response.Token,
                Mail = response.Mail,
                FailOver = response.FailOver,
                ContryCode = response.ContryCode,
                ObjectSid = response.ObjectSid,
                BadPwdCount = response.BadPwdCount,
                WhenCreate = response.WhenCreate,
                AccountExpires = response.AccountExpires,
                DisplayName = response.DisplayName,
                LastLogon = response.LastLogon,
                LockoutTime = response.LockoutTime,
                PwdLastSet = response.PwdLastSet,
                LogonCount = response.LogonCount,
                BadPwdTime = response.BadPwdTime,
                TelefonoNumber = response.TelefonoNumber,
                LastLogonTimeStamp = response.LastLogonTimeStamp,
                WhenChanged = response.WhenChanged,
                Location = response.Location
            };

            return SU;
        }

        public bool GetCustomAuthentication(string userName, string password, string applicationName, string commandName)
        {
            return Security.GetCustomAuthentication(userName, password, applicationName, commandName);
        }

        public List<Shared.Group> GetGroupForUser(string username)
        {
            throw new NotImplementedException();
        }

        public List<Shared.Group> GetGroupForZone(string zonename)
        {
            throw new NotImplementedException();
        }

        public List<Shared.Privilege> GetGroupPolices(string groupname)
        {
            throw new NotImplementedException();
        }

        public List<Shared.Machine> GetMachineForZone(string Zone)
        {
            throw new NotImplementedException();
        }

        public List<Shared.Privilege> GetPrivilegies(string User, string Application)
        {
            throw new NotImplementedException();
        }

        public bool GetPrivilegies(string user, Shared.Privilege privilegies)
        {
            throw new NotImplementedException();
        }

        public List<Shared.SecurityUser> GetUserForGroup(string groupname)
        {
            throw new NotImplementedException();
        }

        public List<Shared.SecurityUser> GetUserNameByEmail(string Email)
        {
            throw new NotImplementedException();
        }

        public List<Shared.SecurityUser> GetUsersByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Shared.Workstation> GetWorkstationForZone(string Zone)
        {
            throw new NotImplementedException();
        }

        public List<Shared.Zone> GetZoneForArea(string Area)
        {
            throw new NotImplementedException();
        }

        public List<Shared.Zone> GetZoneForUser(string User)
        {
            throw new NotImplementedException();
        }

        public int GroupPolicyAddGroupToAppCommand(string Group, string Application, string[] Command)
        {
            throw new NotImplementedException();
        }

        public void Initialize(Shared.ServicesType ServicesType, string culturename)
        {
            Security.Initialize((Manager.Security.ServicesType)ServicesType, culturename);
        }

        public void Initialize(Shared.ServicesType ServicesType)
        {
            Security.Initialize((Manager.Security.ServicesType)ServicesType);
        }

        public int InsertApplicationCommand(string ApplicationName, string Code, string ApplicationDescription, string ApplicationExeName, bool ApplicationIsManager, string[] ApplicationCommand)
        {
            throw new NotImplementedException();
        }

        public void Login()
        {
            Security.Login();
        }

        public void Login(out bool cancel)
        {
            Security.Login(out cancel);
        }

        public void Login(out bool cancel, Shared.LoginButton inactiveButtons)
        {
            Security.Login(out cancel, (Manager.Security.LoginButton)inactiveButtons);
        }

        public Shared.SecurityUser LogOff()
        {
            throw new NotImplementedException();
        }

        public void MessageRecivedHandler(Shared.SecurityUser user)
        {
            throw new NotImplementedException();
        }

        public void OnNotify(Object sender, Manager.Security.ChangedUserEventArgs e)
        {
            if (sender == null) return;
            Shared.ChangedUserEventArgs ee = new Shared.ChangedUserEventArgs(e.UserID);
            if(this.Notify != null)
            {
                this.Notify((object)this, ee);
            }
        }

        public void seraut_UserChanged(object sender, Shared.ChangedUserEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Uninitialize()
        {
            Security.Uninitialize();
        }

        public int UserCreateUser(string Identification, string Password, string LastName, string FirstName, string Location, string Email, bool IsSecondaryProvider)
        {
            throw new NotImplementedException();
        }

        public int UserGroupAddUsersToGroups(string User, string[] Group)
        {
            throw new NotImplementedException();
        }

        public Shared.SecurityUser ValidateUser(string user, string password)
        {
            throw new NotImplementedException();
        }

        public int WorkstationCreateWorkstation(string Code, string Name, string Description)
        {
            throw new NotImplementedException();
        }

        public int ZoneCreateZone(string Area, string Code, string Name, string Description)
        {
            throw new NotImplementedException();
        }

        public int ZoneDetailsAddMachine(string Zone, string[] Machine)
        {
            throw new NotImplementedException();
        }

        public int ZoneWorkstationAddWorkstation(string Zone, string[] Workstation)
        {
            throw new NotImplementedException();
        }
    }
}
