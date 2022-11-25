using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class SampleCuttingFacade
    {
        public DataTable GetCuttingNumbers(int op, int heatNumber, int groupItemNumber)
        {
            DataTable result = new DataTable();
            try
            {
                result = new SampleCuttingRepository().GetCuttingNumbers(
                    op, heatNumber, groupItemNumber, Enumerations.Zone.Hornos);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
