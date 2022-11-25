using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class NormalizedLoadHistoryFacade
    {
        public List<NormalizedLoadHistoryDto> GetNormalizedLoadHistoryItems()
        {
            DataTable result = null;
            var items = new List<NormalizedLoadHistoryDto>();
            try
            {
                result = new NormalizedLoadHistoryRepository().
                    GetLastHistory();
                foreach (DataRow row in result.Rows)
                {
                    items.Add(
                            new NormalizedLoadHistoryDto
                            {
                                OrderNumber = int.Parse(row["OrderNumber"].ToString()),
                                HeatNumber = int.Parse(row["HeatNumber"].ToString()),
                                GroupItemNumber = int.Parse(row["GroupItemNumber"].ToString()),
                                InsDateTime = DateTime.Parse(row["InsDateTime"].ToString()),
                                Identificador = int.Parse(row["Identificador"].ToString())
                            }
                        );
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return items;
        }
    }
}

