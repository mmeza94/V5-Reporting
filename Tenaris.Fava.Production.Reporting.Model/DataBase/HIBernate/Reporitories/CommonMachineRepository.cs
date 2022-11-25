using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class CommonMachineRepository : BaseDataAccess
    {
        public Machine GetMachineByDescription(string description)
        {
            var machine = new Machine();

            try
            {
                var criteria = m_session.CreateCriteria<Machine>();
                criteria.Add(Restrictions.Eq("Description", description));
                criteria.Add(Restrictions.Eq("Active", true));
                machine = (Machine)criteria.UniqueResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return machine;
        }
    }
}
