using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class NormalizedLoadHistoryRepository : BaseDataAccess
    {
        public DataTable GetLastHistory()
        {
            DataTable result = new DataTable();
            var connStr = m_session.Connection.ConnectionString;

            try
            {
                using (var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    var command = new SqlCommand("Production.[GetNormalizedLoadHistory]", connection);
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
    }
}
