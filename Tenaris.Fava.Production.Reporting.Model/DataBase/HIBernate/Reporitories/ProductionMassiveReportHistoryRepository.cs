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
    public class ProductionMassiveReportHistoryRepository:BaseDataAccess
    {
        public IList GetMassiveReportHistoryByParams(int? orderNumber, int? groupItemNumber, int? heatNumber,
             Enumerations.ProductionReportSendStatus? sendStatus, int? machineSequence, int? reportSequence)
        {
            IList items = null;
            try
            {
                var criteria = m_session.CreateCriteria<TpsMassiveReportHistory>();
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
                if (reportSequence.HasValue)
                    criteria.Add(Restrictions.Eq("ReportSequence", reportSequence));
                items = criteria.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }

        public TpsMassiveReportHistory GetLastMachineReportProduction(ReportProductionDto reportProductionDto)
        {
            TpsMassiveReportHistory reportProductionHistory = null;
            try
            {
                var criteria = m_session.CreateCriteria<TpsMassiveReportHistory>();
                criteria.Add(Restrictions.Eq("IdOrder", reportProductionDto.Orden));
                criteria.Add(Restrictions.Eq("HeatNumber", reportProductionDto.Colada));
                criteria.Add(Restrictions.Eq("GroupItemNumber", reportProductionDto.IdUDT));
                criteria.Add(Restrictions.Eq("MachineSequence", reportProductionDto.Secuencia--));
                criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                    Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Completo
                }));
                criteria.SetMaxResults(1);
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List<TpsMassiveReportHistory>();

                reportProductionHistory = (items.Count == 0) ? null : items.First<TpsMassiveReportHistory>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportProductionHistory;
        }

        public IList GetMassiveReportHistoryOrderByInsDateTime(int orderNumber, int heatNumber, int groupItemNumber,
            bool descOrder)
        {
            IList items = null;
            try
            {
                var criteria = m_session.CreateCriteria<TpsMassiveReportHistory>();
                criteria.Add(Restrictions.Eq("IdOrder", orderNumber));
                criteria.Add(Restrictions.Eq("HeatNumber", heatNumber));
                criteria.Add(Restrictions.Eq("GroupItemNumber", groupItemNumber));
                if (descOrder)
                    criteria.AddOrder(Order.Desc("InsDateTime"));
                else
                    criteria.AddOrder(Order.Asc("InsDateTime"));
                items = criteria.List();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return items;
        }

        public int GetLastMachineGoodPieces(ReportProductionDto reportProductionDto)
        {
            int goodPieces = 0;
            try
            {
                var criteria = m_session.CreateCriteria<TpsMassiveReportHistory>();
                criteria.Add(Restrictions.Eq("IdOrder", reportProductionDto.Orden));
                criteria.Add(Restrictions.Eq("HeatNumber", reportProductionDto.Colada));
                criteria.Add(Restrictions.Eq("GroupItemNumber", reportProductionDto.IdUDT));
                criteria.Add(Restrictions.Eq("MachineSequence", reportProductionDto.Secuencia--));
                criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                    Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Completo
                }));
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List();
                foreach (TpsMassiveReportHistory item in items)
                {
                    goodPieces += item.GoodCount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return goodPieces;
        }
       
        public int GetPreviousSequence(string description)
        {
            var machine = new CommonMachineRepository().GetMachineByDescription(description);
            var criteria = m_session.CreateCriteria<TpsMassiveReportHistory>();
            criteria.Add(Restrictions.Eq("IdMachine", machine.Id));
            criteria.AddOrder(Order.Desc("InsDateTime"));
            criteria.SetMaxResults(1);

            var items = criteria.List<TpsMassiveReportHistory>();
            var reportProductionHistory = (items.Count == 0) ? null : items.First();
            return (reportProductionHistory != null) ? reportProductionHistory.MachineSequence - 1 :
                0;
        }
        public int GetPreviousSequenceByOperation(string operation)
        {
            var criteria = m_session.CreateCriteria<TpsMassiveReportHistory>();
            criteria.Add(Restrictions.Eq("MachineOperation", operation.Trim()));
            criteria.AddOrder(Order.Desc("InsDateTime"));
            criteria.SetMaxResults(1);

            var items = criteria.List<TpsMassiveReportHistory>();
            var reportProductionHistory = (items.Count == 0) ? null : items.First();
            return (reportProductionHistory != null) ? reportProductionHistory.MachineSequence - 1 :
                0;
        }
        public int GetLastMachineGoodPieces(int order, int heat, int groupItem, string description, string extreme)
        {
            int goodPieces = 0;
            try
            {

                //Hard Code por REporte de dos operaciones desde una sola máquina
                //description = (description == "Forjadora") ? "Forjado" : description;
                if (description.Contains("Forjadora"))
                    description = "Forjado";
                else if (description == "Roscadora")
                    description = "Mecanizado";




                var criteria = m_session.CreateCriteria<TpsMassiveReportHistory>();

                criteria.Add(Restrictions.Eq("IdOrder", order));
                criteria.Add(Restrictions.Eq("HeatNumber", heat));
                criteria.Add(Restrictions.Eq("GroupItemNumber", groupItem));
                criteria.Add(Restrictions.Eq("MachineSequence",
                    (extreme != string.Empty) ? GetPreviousSequenceByOperation(description + " " + extreme) :
                    GetPreviousSequence(description)));
                //criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                //    Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Complete
                //}));
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List();
                foreach (TpsMassiveReportHistory item in items)
                {
                    goodPieces += item.GoodCount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return goodPieces;
        }

    }
}
