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
    public class SampleCuttingRepository:BaseDataAccess
    {
        public DataTable GetCuttingNumbers(int op, int heatNumber, int groupItemNumber, Enumerations.Zone zone)
        {
            DataTable result = new DataTable();
            var connStr = (zone == Enumerations.Zone.CND) ? m_session.Connection.ConnectionString :
                Configurations.Instance.ConnectionString;
            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Sample.GetCuttingNumbers", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OP", op);
                    command.Parameters.AddWithValue("@Colada", heatNumber);
                    command.Parameters.AddWithValue("@Atado", groupItemNumber);
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
