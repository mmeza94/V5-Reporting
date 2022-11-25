using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class RejectionReportDetailRepository:BaseDataAccess
    {
        public DataTable GetRejectionDetailReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            DataTable result = new DataTable();
            var connStr = (zone == Enumerations.Zone.CND) ? m_session.Connection.ConnectionString :
              ConfigurationManager.ConnectionStrings[FORJA_CONNSTR_NAME].ConnectionString;
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.GetRejectionDetailReport", connection);
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
    }
}
