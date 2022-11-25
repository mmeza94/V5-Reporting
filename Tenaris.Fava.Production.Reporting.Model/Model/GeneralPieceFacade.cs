using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class GeneralPieceFacade
    {
        public void OrderByInsDateTime(IList<GeneralPiece> generalPieces)
        {
            try
            {
                generalPieces.OrderByDescending(x => x.InsDateTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
