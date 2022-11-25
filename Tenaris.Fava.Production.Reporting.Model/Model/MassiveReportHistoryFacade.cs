using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class MassiveReportHistoryFacade
    {

        public IList GetMassiveReportHistoryByParams(int? orderNumber, int? groupItemNumber, int? heatNumber,
             Enumerations.ProductionReportSendStatus? sendStatus, int? machineSequence, int? reportSequence)
        {
            return new ProductionMassiveReportHistoryRepository().GetMassiveReportHistoryByParams
                (orderNumber, groupItemNumber, heatNumber, sendStatus, machineSequence, reportSequence);
        }



        public TpsMassiveReportHistory GetLastMachineProductionReport(ReportProductionDto reportProductionDto)
        {
            return new ProductionMassiveReportHistoryRepository().GetLastMachineReportProduction(reportProductionDto);
        }
        public void SaveTpsMassiveReportHistory(MassiveLoad massiveLoadRow)
        {

            try
            {
                var tpsMassiveReportHistory = new TpsMassiveReportHistory
                {
                    GoodCount = massiveLoadRow.GoodCount,
                    GroupItemNumber = massiveLoadRow.GroupItemNumber,
                    HeatNumber = massiveLoadRow.HeatNumber,
                    //IdMachine = (new CommonMachineRepository().GetMachineByDescription(reportProductionDto.DescripcionMaquina)).Id,
                    IdOrder = massiveLoadRow.IdOrder,
                    InsDateTime = DateTime.Now,
                    InsertedBy = "extcvm",
                    TotalQuantity = massiveLoadRow.TotalQuantity,
                    LotNumberHtr = massiveLoadRow.LotNumberHTR,
                    ReworkedCount = 0,
                    ScrapCount = massiveLoadRow.ScrapCount,
                    SendStatus = Enumerations.ProductionReportSendStatus.Completo,
                    MachineSequence = massiveLoadRow.MachineSequence,
                    MachineOperation = massiveLoadRow.Operation,
                    MachineOption = massiveLoadRow.Option,
                    Destiny = massiveLoadRow.Destiny,
                    RejectionCode = massiveLoadRow.RejectionCode,
                    ReportSequence = massiveLoadRow.ReportSequence,
                    Observation = ""
                };
                new ProductionMassiveReportHistoryRepository().Save(tpsMassiveReportHistory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int GetPreviousSequenceByOperation(string operation)
        {
            return new ProductionMassiveReportHistoryRepository().GetPreviousSequenceByOperation(operation);
        }
        public int GetLastMachineGoodPieces(ReportProductionDto reportProductionDto)
        {
            return new ProductionMassiveReportHistoryRepository().GetLastMachineGoodPieces(reportProductionDto);
        }
        public int GetLastMachineGoodPieces(int order, int heat, int groupItem, string description, string extreme)
        {
            return new ProductionMassiveReportHistoryRepository().GetLastMachineGoodPieces(order, heat, groupItem, description, extreme);
        }
    }
}
