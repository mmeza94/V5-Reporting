using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.SecurityAdapter.Shared
{
    public interface ISecurityAdapter
    {
        
        event EventHandler<ChangedUserEventArgs> Notify;

        void Initialize(ServicesType ServicesType, string culturename);

        void Initialize(ServicesType ServicesType);

        void Uninitialize();

        SecurityUser ValidateUser(string user, string password);

        SecurityUser GetCurrentUser();

        SecurityUser LogOff();

        List<Privilege> GetPrivilegies(string User, string Application);

        bool GetPrivilegies(string user, Privilege privilegies);

        List<Application> GetApplication(string User);

        List<Application> GetAllApplication();
        int InsertApplicationCommand(string ApplicationName, string Code, string ApplicationDescription, string ApplicationExeName, bool ApplicationIsManager, string[] ApplicationCommand);
        int CreateGroup(string Area, string Zone, string Name, string Code, string Description, bool Active);

        int UserGroupAddUsersToGroups(string User, string[] Group);

        int GroupPolicyAddGroupToAppCommand(string Group, string Application, string[] Command);

        List<SecurityUser> GetUserNameByEmail(string Email);        

        List<SecurityUser> GetUsersByName(string name);

        List<Group> GetAllGroup();

        List<Group> GetGroupForUser(string username);

        List<ApplicationCommand> GetCommandForApplication(string application);

        List<ApplicationCommand> GetCommandForGroup(string groupname);

        List<SecurityUser> GetUserForGroup(string groupname);

        List<Zone> GetAllZone();

        List<Zone> GetZoneForArea(string Area);

        List<Zone> GetZoneForUser(string User);

        List<Group> GetGroupForZone(string zonename);

        List<Privilege> GetGroupPolices(string groupname);

        List<SecurityUser> GetAllUser();

        List<Workstation> GetAllWorkstation();

        List<Machine> GetAllMachine();

        List<Machine> GetMachineForZone(string Zone);

        List<Workstation> GetWorkstationForZone(string Zone);

        List<Area> GetAllArea();

        int WorkstationCreateWorkstation(string Code, string Name, string Description);

        int ZoneCreateZone(string Area, string Code, string Name, string Description);

        int ZoneWorkstationAddWorkstation(string Zone, string[] Workstation);

        int ZoneDetailsAddMachine(string Zone, string[] Machine);

        int UserCreateUser(string Identification, string Password, string LastName, string FirstName, string Location, string Email, bool IsSecondaryProvider);

        void Login();

        void Login(out bool cancel);

        void Login(out bool cancel, LoginButton inactiveButtons);

        void Dispose();

        SecurityUser CheckUser(string user, string password);

        SecurityUser CheckUser();

        bool GetCustomAuthentication(string userName, string password, string applicationName, string commandName);        
        bool CheckDynamicPassword(string password, int level);
        bool DynamicPassword(int level);        

        void seraut_UserChanged(object sender, ChangedUserEventArgs e);

        void ConnectionMonitor_StateChanged(object sender, StateChangeEventArgs e);

        void MessageRecivedHandler(SecurityUser user);
    }
}
