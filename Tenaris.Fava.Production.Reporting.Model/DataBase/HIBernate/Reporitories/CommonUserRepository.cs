using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class CommonUserRepository : BaseDataAccess
    {
        public User GetUserByUserName(string userName)
        {
            User user = null;
            try
            {
                var criteria = m_session.CreateCriteria<User>();
                criteria.Add(Restrictions.Eq("UserName", userName));
                criteria.SetMaxResults(1);
                var items = criteria.List<User>();
                if (items.Count > 0)
                    user = items.First();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;

        }

        public bool LoginUser(string userName, string password)
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("UserName", userName);
                ht.Add("Password", password);
                ht.Add("Active", Enumerations.AxlrBit.Si);
                User usuario = (User)Get(typeof(User), ht);
                return (usuario == null) ? false : true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
