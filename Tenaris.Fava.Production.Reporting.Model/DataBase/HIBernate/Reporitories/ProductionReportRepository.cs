using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class ProductionReportRepository:BaseDataAccess
    {
        public DataTable GetProductionByWorkship(DateTime fechaIni, DateTime fechaFin,
            Enumerations.Zone zone, Int16? workshift, Int16? sequence)
        {
            DataTable tblResult = new DataTable();
            var connStr = m_session.Connection.ConnectionString;
            if (zone == Enumerations.Zone.Forja || zone == Enumerations.Zone.Hornos ||
                zone == Enumerations.Zone.Granalla2)
                connStr = Configurations.Instance.ConnectionString;
            else if (zone == Enumerations.Zone.Meca)
                connStr = Configurations.Instance.ConnectionString;

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetProductionReportByWorkshift", connection);
                    if (zone == Enumerations.Zone.Hornos)
                        command.CommandText = "Production.GetHeatersProductionReportByWorkshift";
                    if (zone == Enumerations.Zone.Granalla2)
                        command.CommandText = "Production.GetGranallaProductionReportByWorkshift";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicial", fechaIni);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFin);
                    if (workshift.HasValue)
                        command.Parameters.AddWithValue("@Workshift", workshift);
                    if (sequence.HasValue)
                        command.Parameters.AddWithValue("@MachineSequence", sequence);
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(tblResult);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return tblResult;
        }

        public DataTable GetRejectionDetailByDateRange(DateTime fechaIni, DateTime fechaFin,
            Int32? idReportProductionHistory, Enumerations.Zone zone)
        {
            DataTable tblResult = new DataTable();
            var connStr = m_session.Connection.ConnectionString;
            if (zone == Enumerations.Zone.Forja || zone == Enumerations.Zone.Hornos ||
                zone == Enumerations.Zone.Granalla2)
                connStr = Configurations.Instance.ConnectionString;
            else if (zone == Enumerations.Zone.Meca)
                connStr = Configurations.Instance.ConnectionString;
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetRejectionDetailByDateRange", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicial", fechaIni.Date);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFin.Date);
                    if (idReportProductionHistory.HasValue)
                        command.Parameters.AddWithValue("@IdReportProductionHistory", idReportProductionHistory);

                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(tblResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tblResult;
        }


        public Enumerations.ForgeMode GetCurrentForgeMode(int groupItemNumber)
        {
            DataTable tblResult = new DataTable();
            var connStr = m_session.Connection.ConnectionString;
            connStr = Configurations.Instance.ConnectionString;
            Enumerations.ForgeMode forgeMode = Enumerations.ForgeMode.BothEnds;
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Tracking.GetForgeMode", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GroupItemNumber", groupItemNumber);
                    forgeMode = (bool)command.ExecuteScalar() ? Enumerations.ForgeMode.BothEnds : Enumerations.ForgeMode.OneEnd;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return forgeMode;
        }
    }
}
