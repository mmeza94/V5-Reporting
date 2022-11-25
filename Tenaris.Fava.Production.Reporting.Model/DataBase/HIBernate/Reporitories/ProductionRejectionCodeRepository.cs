using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class ProductionRejectionCodeRepository : BaseDataAccess
    {

        public IList GetRejectionCodeByMachine(Machine machine)
        {
            if (Configurations.Instance.VersionApplication == "ReportProductionDBV4")
            {
                try
                {
                    return GetListByPropertyValue(typeof(RejectionCode), "Machine", machine);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    var ht = new Hashtable();
                    ht.Add("Machine", machine);
                    ht.Add("Active", Enumerations.AxlrBit.Si);
                    return GetList(typeof(RejectionCode), ht, "Description", true);
                    //return GetListByPropertyValue(typeof(RejectionCode), "Machine", machine);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

        }

        public RejectionCode GetRejectionCodeByCode(string code)
        {
            try
            {
                ICriteria criteria = m_session.CreateCriteria<RejectionCode>();
                criteria.Add(Restrictions.Eq("Code", code));
                criteria.SetMaxResults(1);

                var items = criteria.List<RejectionCode>();
                return (items.Count == 0) ? null : items.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RejectionCode GetRejectionCodeByDescription(string description)
        {
            try
            {
                ICriteria criteria = m_session.CreateCriteria<RejectionCode>();
                criteria.Add(Restrictions.Eq("Description", description));
                criteria.SetMaxResults(1);
                var items = criteria.List<RejectionCode>();
                return (items.Count == 0) ? null : items.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
