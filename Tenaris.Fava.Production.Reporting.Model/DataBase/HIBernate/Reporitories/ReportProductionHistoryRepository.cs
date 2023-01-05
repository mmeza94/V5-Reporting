using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Library.Log;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class ReportProductionHistoryRepository:BaseDataAccess
    {
        public ReportProductionHistory GetReportProductionHistoryByIdHistory(int idHistory)
        {
            ReportProductionHistory reportPorductionHistory = null;

            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
                criteria.Add(Restrictions.Eq("IdHistory", idHistory));
                criteria.SetMaxResults(1);

                var items = criteria.List<ReportProductionHistory>();
                reportPorductionHistory = (items.Count == 0) ? null : items.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportPorductionHistory;
        }

        public ReportProductionHistory GetReportProductionHistoryByIdHistory(int idHistory, int orderNumber, int heatNumber, int groupItemNumber)
        {
            ReportProductionHistory reportPorductionHistory = null;

            try
            {
                Trace.Message("GetReportProductionHistoryByIdHistory({0})", idHistory);
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
                criteria.Add(Restrictions.Eq("IdHistory", idHistory));
                criteria.Add(Restrictions.Eq("IdOrder", orderNumber));
                criteria.Add(Restrictions.Eq("HeatNumber", heatNumber));
                criteria.Add(Restrictions.Eq("GroupItemNumber", groupItemNumber));

                criteria.SetMaxResults(1); /// select top 1
                  

                var items = criteria.List<ReportProductionHistory>();
                reportPorductionHistory = (items.Count == 0) ? null : items.First();

            }
            catch (Exception ex)
            {
                Trace.Exception(ex);
            }
            return reportPorductionHistory;
        }
        public ReportProductionHistoryV1 GetReportProductionHistoryByIdHistoryV1(int idHistory, int orderNumber, int heatNumber, int groupItemNumber)
        {
            ReportProductionHistoryV1 reportPorductionHistory = null;

            try
            {
                Trace.Message("GetReportProductionHistoryByIdHistory({0})", idHistory);
                var criteria = m_session.CreateCriteria<ReportProductionHistoryV1>();
                criteria.Add(Restrictions.Eq("IdHistory", idHistory));
                criteria.Add(Restrictions.Eq("IdOrder", orderNumber));
                criteria.Add(Restrictions.Eq("HeatNumber", heatNumber));
                criteria.Add(Restrictions.Eq("GroupItemNumber", groupItemNumber));

                criteria.SetMaxResults(1); /// select top 1


                var items = criteria.List<ReportProductionHistoryV1>();
                reportPorductionHistory = (items.Count == 0) ? null : items.First();

            }
            catch (Exception ex)
            {
                Trace.Exception(ex);
            }
            return reportPorductionHistory;
        }


        public ObservableCollection<ReportProductionHistory> GetReportProductionHistoryByParams(
            int? orderNumber, 
            int? groupItemNumber, 
            int? heatNumber,
            int? idHistory, 
            Enumerations.ProductionReportSendStatus? sendStatus,
            int? machineSequence)
        {
            IList items = null;
            ObservableCollection < ReportProductionHistory > reportPorductionHistory = new ObservableCollection<ReportProductionHistory>();
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
                if (orderNumber.HasValue)
                    criteria.Add(Restrictions.Eq("IdOrder", orderNumber));
                if (groupItemNumber.HasValue)
                    criteria.Add(Restrictions.Eq("GroupItemNumber", groupItemNumber));
                if (heatNumber.HasValue)
                    criteria.Add(Restrictions.Eq("HeatNumber", heatNumber));
                if (idHistory.HasValue)
                    criteria.Add(Restrictions.Eq("IdHistory", idHistory));
                if (sendStatus.HasValue)
                    criteria.Add(Restrictions.Eq("SendStatus", sendStatus));
                if (machineSequence.HasValue)
                    criteria.Add(Restrictions.Eq("MachineSequence", machineSequence));
                items = criteria.List();

                foreach (var item in items)
                {
                    reportPorductionHistory.Add((ReportProductionHistory)item);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportPorductionHistory;
        }

        public ObservableCollection<ReportProductionHistoryV1> GetReportProductionHistoryByParamsV1(int? orderNumber, int? groupItemNumber, int? heatNumber,
            int? idHistory, Enumerations.ProductionReportSendStatus? sendStatus, int? machineSequence)
        {
            IList items = null;
            ObservableCollection<ReportProductionHistoryV1> reportPorductionHistory = new ObservableCollection<ReportProductionHistoryV1>();
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistoryV1>();
                if (orderNumber.HasValue)
                    criteria.Add(Restrictions.Eq("IdOrder", orderNumber));
                if (groupItemNumber.HasValue)
                    criteria.Add(Restrictions.Eq("GroupItemNumber", groupItemNumber));
                if (heatNumber.HasValue)
                    criteria.Add(Restrictions.Eq("HeatNumber", heatNumber));
                if (idHistory.HasValue)
                    criteria.Add(Restrictions.Eq("IdHistory", idHistory));
                if (sendStatus.HasValue)
                    criteria.Add(Restrictions.Eq("SendStatus", sendStatus));
                if (machineSequence.HasValue)
                    criteria.Add(Restrictions.Eq("MachineSequence", machineSequence));
                items = criteria.List();

                foreach (var item in items)
                {
                    reportPorductionHistory.Add((ReportProductionHistoryV1)item);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportPorductionHistory;
        }

        public ReportProductionHistory GetLastMachineReportProduction(ReportProductionDto reportProductionDto)
        {
            ReportProductionHistory reportProductionHistory;
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
                criteria.Add(Restrictions.Eq("IdOrder", reportProductionDto.Orden));
                criteria.Add(Restrictions.Eq("HeatNumber", reportProductionDto.Colada));
                criteria.Add(Restrictions.Eq("GroupItemNumber", reportProductionDto.IdUDT));
                criteria.Add(Restrictions.Eq("MachineSequence", reportProductionDto.Secuencia--));
                criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Completo
                }));
                criteria.SetMaxResults(1);
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List<ReportProductionHistory>();

                reportProductionHistory = (items.Count == 0) ? null : items.First<ReportProductionHistory>();
            }
            catch (Exception ex)
            {
                Trace.Exception(ex);
                Trace.Message("GetLastMachineReportProduction.....");
                reportProductionHistory = new ReportProductionHistory();
            }
            return reportProductionHistory;
        }

        public IList GetReportProductionHistoryOrderByInsDateTime(int orderNumber, int heatNumber, int groupItemNumber,
            bool descOrder)
        {
            IList items = null;
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
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
                Trace.Exception(ex);
            }
            return items;
        }



        public int GetLastMachineGoodPieces(ReportProductionDto reportProductionDto)
        {
            int goodPieces = 0;
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
                criteria.Add(Restrictions.Eq("IdOrder", reportProductionDto.Orden));
                criteria.Add(Restrictions.Eq("HeatNumber", reportProductionDto.Colada));
                criteria.Add(Restrictions.Eq("GroupItemNumber", reportProductionDto.IdUDT));
                criteria.Add(Restrictions.Eq("MachineSequence", reportProductionDto.Secuencia--));
                criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                    Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Completo
                }));
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List();
                foreach (ReportProductionHistory item in items)
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
        public int GetLastMachineGoodPieces(GeneralPiece generalPieceDto, int sequence)
        {
            int goodPieces = 0;
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
                criteria.Add(Restrictions.Eq("IdOrder", generalPieceDto.OrderNumber));
                criteria.Add(Restrictions.Eq("HeatNumber", generalPieceDto.HeatNumber));
                criteria.Add(Restrictions.Eq("GroupItemNumber", generalPieceDto.GroupItemNumber));
                criteria.Add(Restrictions.Eq("MachineSequence", sequence--));
                //criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                //    Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Complete
                //}));
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List();
                foreach (ReportProductionHistory item in items)
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
            var criteria = m_session.CreateCriteria<ReportProductionHistory>();
            if (machine == null)
                machine = new Machine();
            Trace.Message("ID {0} -NAME {1}  -CODE {2}", machine.Id, machine.Name, machine.Code);
            criteria.Add(Restrictions.Eq("IdMachine", machine.Id));
            criteria.AddOrder(Order.Desc("InsDateTime"));
            criteria.SetMaxResults(1);
            var items = criteria.List<ReportProductionHistory>();

            var reportProductionHistory = (items.Count == 0) ? null : items.First();
            return (reportProductionHistory != null) ? reportProductionHistory.MachineSequence - 1 :
                0;
        }
        public int GetPreviousSequenceByOperation(string operation)
        {
            int result = 0;
            if (Configurations.Instance.VersionApplication.Equals("V1"))
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistoryV1>();
                criteria.Add(Restrictions.Eq("MachineOperation", operation.Trim()));
                criteria.AddOrder(Order.Desc("InsDateTime"));
                criteria.SetMaxResults(1);
                var items = criteria.List<ReportProductionHistoryV1>();

                var reportProductionHistory = (items.Count == 0) ? null : items.First();//criteria.List<ReportProductionHistory>()[0];
                result = (reportProductionHistory != null) ? reportProductionHistory.MachineSequence - 1 :
                    0;
            }
            else
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();
                criteria.Add(Restrictions.Eq("MachineOperation", operation.Trim()));
                criteria.AddOrder(Order.Desc("InsDateTime"));
                criteria.SetMaxResults(1);
                var items = criteria.List<ReportProductionHistory>();

                var reportProductionHistory = (items.Count == 0) ? null : items.First();//criteria.List<ReportProductionHistory>()[0];
                result = (reportProductionHistory != null) ? reportProductionHistory.MachineSequence - 1 :
                    0;
            }
            return result;
        }
        public int GetLastMachineGoodPieces(int order, int heat, int groupItem, string description, string extreme)
        {
            int goodPieces = 0;
            try
            {
                if (Configurations.Instance.VersionApplication.Equals("V1"))
                {
                    //Hard Code por REporte de dos operaciones desde una sola máquina
                    //description = (description == "Forjadora") ? "Forjado" : description;
                    if (description.Contains("Forjadora"))
                        description = "Forjado";
                    else if (description == "Roscadora" || description == "Roscadora 4")
                        description = "Mecanizado";
                    //cambiar aqui
                    var criteria = m_session.CreateCriteria<ReportProductionHistoryV1>();

                    criteria.Add(Restrictions.Eq("IdOrder", order));
                    criteria.Add(Restrictions.Eq("HeatNumber", heat));
                    criteria.Add(Restrictions.Eq("GroupItemNumber", groupItem));
                    criteria.Add(Restrictions.Eq("MachineSequence",
                        (extreme != string.Empty) ? GetPreviousSequenceByOperation(description + " " + extreme) : GetPreviousSequence(description)));
                    //criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                    //    Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Complete
                    //}));
                    criteria.AddOrder(Order.Desc("InsDateTime"));
                    var items = criteria.List();
                    foreach (ReportProductionHistoryV1 item in items)
                    {
                        goodPieces += item.GoodCount;
                    }
                }
                else
                {

                    //Hard Code por REporte de dos operaciones desde una sola máquina
                    //description = (description == "Forjadora") ? "Forjado" : description;
                    if (description.Contains("Forjadora"))
                        description = "Forjado";
                    else if (description == "Roscadora" || description == "Roscadora 4")
                        description = "Mecanizado";
                    //cambiar aqui
                    var criteria = m_session.CreateCriteria<ReportProductionHistory>();

                    criteria.Add(Restrictions.Eq("IdOrder", order));
                    criteria.Add(Restrictions.Eq("HeatNumber", heat));
                    criteria.Add(Restrictions.Eq("GroupItemNumber", groupItem));
                    criteria.Add(Restrictions.Eq("MachineSequence",
                        (extreme != string.Empty) ? GetPreviousSequenceByOperation(description + " " + extreme) : GetPreviousSequence(description)));
                    //criteria.Add(Restrictions.In("SendStatus", new List<Enumerations.ProductionReportSendStatus>(){
                    //    Enumerations.ProductionReportSendStatus.Final, Enumerations.ProductionReportSendStatus.Complete
                    //}));
                    criteria.AddOrder(Order.Desc("InsDateTime"));
                    var items = criteria.List();
                    foreach (ReportProductionHistory item in items)
                    {
                        goodPieces += item.GoodCount;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return goodPieces;
        }

        public DataTable GetProductionReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            DataTable result = new DataTable();

            var connStr = m_session.Connection.ConnectionString;
            if (zone == Enumerations.Zone.Forja)
                connStr = ConfigurationManager.ConnectionStrings[""].ConnectionString;
            else if (zone == Enumerations.Zone.Meca)
                connStr = ConfigurationManager.ConnectionStrings[""].ConnectionString;

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetProductionReport", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicial", fechaIni);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFin);
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public DataTable GetGranallaProductionReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            DataTable result = new DataTable();
            var connStr = (zone == Enumerations.Zone.CND) ? m_session.Connection.ConnectionString :
                ConfigurationManager.ConnectionStrings[FORJA_CONNSTR_NAME].ConnectionString;
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetGranallaProductionReport", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicial", fechaIni);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFin);
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }



        public DataTable GetProductionOnProcess(Enumerations.Zone zone)
        {
            DataTable result = new DataTable();
            var connStr = m_session.Connection.ConnectionString;
            if (zone == Enumerations.Zone.Forja)
                connStr = ConfigurationManager.ConnectionStrings[""].ConnectionString;
            else if (zone == Enumerations.Zone.Meca)
                connStr = ConfigurationManager.ConnectionStrings[""].ConnectionString;


            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetProductionOnProcess", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public DataTable GetGranallaProductionOnProcess(Enumerations.Zone zone)
        {
            DataTable result = new DataTable();
            var connStr = (zone == Enumerations.Zone.CND) ? m_session.Connection.ConnectionString :
               ConfigurationManager.ConnectionStrings[FORJA_CONNSTR_NAME].ConnectionString;

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetGranallaProductionOnProcess", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public DataTable GetHeatersProductionReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            DataTable result = new DataTable();
            var connStr = (zone == Enumerations.Zone.CND) ? m_session.Connection.ConnectionString :
                ConfigurationManager.ConnectionStrings[FORJA_CONNSTR_NAME].ConnectionString;
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetHeatersProductionReport", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicial", fechaIni);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFin);
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public DataTable GetHeatersProductionOnProcess(Enumerations.Zone zone)
        {
            DataTable result = new DataTable();
            var connStr = (zone == Enumerations.Zone.CND) ? m_session.Connection.ConnectionString :
               ConfigurationManager.ConnectionStrings[FORJA_CONNSTR_NAME].ConnectionString;

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetHeatersProductionOnProcess", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public ReportProductionHistory GetLastReportOnRevenido()
        {
            ReportProductionHistory reportProductionHistory = null;
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();

                criteria.Add(Restrictions.Eq("MachineSequence", 7));//Secuencia de revenido
                criteria.SetMaxResults(1);
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List<ReportProductionHistory>();
                reportProductionHistory = (items.Count == 0) ? null : items.First<ReportProductionHistory>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportProductionHistory;
        }
        public ReportProductionHistory GetLastReportOnHeaters(int sequence)
        {
            ReportProductionHistory reportProductionHistory = null;
            try
            {
                var criteria = m_session.CreateCriteria<ReportProductionHistory>();

                criteria.Add(Restrictions.Eq("MachineSequence", sequence));//Secuencia de revenido
                criteria.SetMaxResults(1);
                criteria.AddOrder(Order.Desc("InsDateTime"));
                var items = criteria.List<ReportProductionHistory>();
                reportProductionHistory = (items.Count == 0) ? null : items.First<ReportProductionHistory>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportProductionHistory;
        }

        public void SaveReportProductionHistory(ReportProductionHistory reportProductionHistory)
        {
            Save(reportProductionHistory);
            m_session.Flush();
            m_session.Evict(reportProductionHistory);
        }

        public void SaveReportProductionHistory(ReportProductionHistoryV1 reportProductionHistory)
        {
            Save(reportProductionHistory);
            m_session.Flush();
            m_session.Evict(reportProductionHistory);
        }
    }
}
