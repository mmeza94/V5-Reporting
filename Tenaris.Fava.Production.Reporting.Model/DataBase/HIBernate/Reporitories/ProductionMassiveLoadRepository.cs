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
    public class ProductionMassiveLoadRepository:BaseDataAccess
    {
        public IList GetMassiveLoadRows(int? orderNumber, int? groupItemNumber, int? heatNumber,
            Enumerations.ProductionReportSendStatus? sendStatus, int? machineSequence)
        {
            IList items = null;
            try
            {
                var criteria = m_session.CreateCriteria<MassiveLoad>();
                if (orderNumber.HasValue)
                    criteria.Add(Restrictions.Eq("IdOrder", orderNumber));
                if (groupItemNumber.HasValue)
                    criteria.Add(Restrictions.Eq("GroupItemNumber", groupItemNumber));
                if (heatNumber.HasValue)
                    criteria.Add(Restrictions.Eq("HeatNumber", heatNumber));

                if (sendStatus.HasValue)
                    criteria.Add(Restrictions.Eq("SendStatus", sendStatus));
                if (machineSequence.HasValue)
                    criteria.Add(Restrictions.Eq("MachineSequence", machineSequence));
                items = criteria.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }

        public IList GetMassiveLoadRows(int? orderNumber, int? groupItemNumber,
            int? heatNumber, Enumerations.ProductionReportSendStatus? sendStatus)
        {
            IList items = null;
            try
            {
                var criteria = m_session.CreateCriteria<MassiveLoad>();

                if (orderNumber.HasValue)
                    criteria.Add(Restrictions.Eq("IdOrder", orderNumber));
                if (groupItemNumber.HasValue)
                    criteria.Add(Restrictions.Eq("GroupItemNumber", groupItemNumber));
                if (heatNumber.HasValue)
                    criteria.Add(Restrictions.Eq("HeatNumber", heatNumber));
                criteria.AddOrder(Order.Asc("IdOrder"));
                criteria.AddOrder(Order.Asc("HeatNumber"));
                criteria.AddOrder(Order.Asc("GroupItemNumber"));
                criteria.AddOrder(Order.Asc("MachineSequence"));
                criteria.AddOrder(Order.Asc("ReportSequence"));


                if (sendStatus.HasValue)
                    criteria.Add(Restrictions.Eq("SendStatus", sendStatus));

                items = criteria.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }

        public MassiveLoad GetFirstMassiveLoadRow(int? lastId)
        {
            try
            {
                var criteria = m_session.CreateCriteria<MassiveLoad>();
                criteria.AddOrder(Order.Asc("Id"));
                criteria.Add(Restrictions.IsNull("ErrorMessage"));
                criteria.AddOrder(Order.Asc("ReportSequence"));
                if (lastId.HasValue)
                    criteria.Add(Restrictions.Gt("Id", lastId));
                criteria.SetMaxResults(1);
                var items = criteria.List<MassiveLoad>();
                return (items.Count == 0) ? null : items.First<MassiveLoad>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
