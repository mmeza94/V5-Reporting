using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class GroupItemProgramFacade
    {

        public int GetProgrammedPieces(ReportProductionDto reportProductionDto)
        {
            int programmedPieces = 0;
            try
            {
                //if (reportProductionDto.Secuencia == 1)
                programmedPieces = new ManufacturingGroupItemProgramRepository().
                GetProgrammedPiecesByIdBatch(reportProductionDto.IdBatch);
                //else
                //{
                //    programmedPieces =
                //        new ReportProductionHistoryFacade().GetLastMachineGoodPieces(reportProductionDto);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return programmedPieces;
        }
        public int GetProgrammedPieces(int idBatch)
        {
            int programmedPieces = 0;
            try
            {
                //if (reportProductionDto.Secuencia == 1)
                programmedPieces = new ManufacturingGroupItemProgramRepository().
                GetProgrammedPiecesByIdBatch(idBatch);
                //else
                //{
                //    programmedPieces =
                //        new ReportProductionHistoryFacade().GetLastMachineGoodPieces(reportProductionDto);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return programmedPieces;
        }
    }
}
