using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class ReportProductionHistoryFacade
    {
        public IList GetReportProductionHistoryByParams(int? orderNumber, int? groupItemNumber, int? heatNumber,
            int? idHistory, Enumerations.ProductionReportSendStatus? sendStatus, int? machineSequence)
        {
            return new ReportProductionHistoryRepository().GetReportProductionHistoryByParams
                (orderNumber, groupItemNumber, heatNumber, idHistory, sendStatus, machineSequence);
        }

        public ReportProductionHistory GetReportProductionHistoryByIdHistory(int idHistory)
        {
            return new ReportProductionHistoryRepository().GetReportProductionHistoryByIdHistory(idHistory);
        }

        public ReportProductionHistory GetReportProductionHistoryByIdHistory(int idHistory, int orderNumber,
            int heatNumber, int groupItemNumber)
        {
            return new ReportProductionHistoryRepository().GetReportProductionHistoryByIdHistory(idHistory, orderNumber,
                heatNumber, groupItemNumber);
        }
        public ReportProductionHistory GetLastMachineProductionReport(ReportProductionDto reportProductionDto)
        {
            return new ReportProductionHistoryRepository().GetLastMachineReportProduction(reportProductionDto);
        }

        public void SaveReportProductionHistory(ReportProductionDto reportProductionDto,
            Enumerations.ProductionReportSendStatus sendStatus, IList rejectionReportDetails)
        {

            try
            {
                var reportProductionHistory = new ReportProductionHistory
                {
                    GoodCount = reportProductionDto.CantidadBuenas,
                    GroupItemNumber = reportProductionDto.IdUDT,
                    HeatNumber = reportProductionDto.Colada,
                    IdHistory = reportProductionDto.IdHistory,
                    IdMachine = (new CommonMachineRepository().GetMachineByDescription(reportProductionDto.DescripcionMaquina)).Id,
                    IdOrder = reportProductionDto.Orden,
                    InsDateTime = DateTime.Now,
                    InsertedBy = reportProductionDto.IdUser,
                    TotalQuantity = reportProductionDto.CantidadTotal,
                    LotNumberHtr = reportProductionDto.Lote,
                    ReworkedCount = reportProductionDto.CantidadReprocesadas,
                    ScrapCount = reportProductionDto.CantidadMalas,
                    SendStatus = sendStatus,
                    MachineSequence = reportProductionDto.Secuencia,
                    MachineOperation = reportProductionDto.Operacion,
                    MachineOption = reportProductionDto.Opcion,
                    Observation = reportProductionDto.Observaciones,
                    GroupItemType = reportProductionDto.TipoUDT,
                    ChildOrder = reportProductionDto.Orden,
                    ChildGroupItemNumber = reportProductionDto.IdUDT,
                    ChildGroupItemType = reportProductionDto.TipoUDT

                    //LoadedCount = reportProductionDto.l

                };
                if (rejectionReportDetails.Count > 0)
                {
                    foreach (RejectionReportDetail rrd in rejectionReportDetails)
                        rrd.ReportProductionHistory = reportProductionHistory;
                    new ReportProductionHistoryRepository().Save(reportProductionHistory, rejectionReportDetails);
                }
                else
                    new ReportProductionHistoryRepository().Save(reportProductionHistory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveReportProductionHistoryForRevenido(ReportProductionDto reportProductionDto,
          Enumerations.ProductionReportSendStatus sendStatus, IList rejectionReportDetails)
        {

            try
            {
                var reportProductionHistory = new ReportProductionHistory
                {
                    GoodCount = reportProductionDto.CantidadBuenas,
                    GroupItemNumber = reportProductionDto.IdUDT,
                    HeatNumber = reportProductionDto.Colada,
                    IdHistory = reportProductionDto.IdHistory,
                    IdMachine = (new CommonMachineRepository().GetMachineByDescription(reportProductionDto.DescripcionMaquina)).Id,
                    IdOrder = reportProductionDto.Orden,
                    InsDateTime = DateTime.Now,
                    InsertedBy = reportProductionDto.IdUser,
                    TotalQuantity = reportProductionDto.CantidadTotal,
                    LotNumberHtr = reportProductionDto.Lote,
                    ReworkedCount = reportProductionDto.CantidadReprocesadas,
                    ScrapCount = reportProductionDto.CantidadMalas,
                    SendStatus = sendStatus,
                    MachineSequence = reportProductionDto.Secuencia,
                    MachineOperation = reportProductionDto.Operacion,
                    MachineOption = reportProductionDto.Opcion,
                    Observation = reportProductionDto.Observaciones,
                    GroupItemType = reportProductionDto.TipoUDT,
                    ChildOrder = reportProductionDto.Orden,
                    ChildGroupItemNumber = reportProductionDto.IdUDT,
                    ChildGroupItemType = reportProductionDto.TipoUDT

                    //LoadedCount = reportProductionDto.l

                };
                var reportRepository = new ReportProductionHistoryRepository();
                if (rejectionReportDetails.Count > 0)
                {

                    reportRepository.SaveReportProductionHistory(reportProductionHistory);
                    var lastReportProductionHistory = reportRepository.GetLastReportOnHeaters(reportProductionDto.Secuencia); //reportProductionHistory;
                    foreach (RejectionReportDetail rrd in rejectionReportDetails)
                        rrd.ReportProductionHistory = lastReportProductionHistory;
                    new ReportProductionHistoryRepository().Save(rejectionReportDetails);
                }
                else
                    new ReportProductionHistoryRepository().SaveReportProductionHistory(reportProductionHistory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetPreviousSequenceByOperation(string operation)
        {
            return new ReportProductionHistoryRepository().GetPreviousSequenceByOperation(operation);
        }
        //public int GetLastMachineGoodPieces(ReportProductionDto reportProductionDto)
        //{
        //    return new ReportProductionHistoryRepository().GetLastMachineGoodPieces(reportProductionDto);
        //}
        //public int GetLastMachineGoodPieces(GeneralPieceDto generalPieceDto, int sequence)
        //{
        //    return new ReportProductionHistoryRepository().GetLastMachineGoodPieces(generalPieceDto,sequence);
        //}
        public int GetLastMachineGoodPieces(int order, int heat, int groupItem, string description, string extreme)
        {
            return new ReportProductionHistoryRepository().GetLastMachineGoodPieces(order, heat, groupItem, description, extreme);
        }

        public DataTable GetProductionReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            return new ReportProductionHistoryRepository().GetProductionReport(fechaIni, fechaFin, zone);
        }
        public DataTable GetGranallaProductionReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            return new ReportProductionHistoryRepository().GetGranallaProductionReport(fechaIni, fechaFin, zone);
        }
        public DataTable GetProductionOnProcess(Enumerations.Zone zone)
        {
            return new ReportProductionHistoryRepository().GetProductionOnProcess(zone);
        }
        public DataTable GetGranallaProductionOnProcess(Enumerations.Zone zone)
        {
            return new ReportProductionHistoryRepository().GetGranallaProductionOnProcess(zone);
        }
        public DataTable GetHeatersProductionReport(DateTime fechaIni, DateTime fechaFin, Enumerations.Zone zone)
        {
            return new ReportProductionHistoryRepository().GetHeatersProductionReport(fechaIni, fechaFin, zone);
        }

        public DataTable GetHeatersProductionOnProcess(Enumerations.Zone zone)
        {
            return new ReportProductionHistoryRepository().GetHeatersProductionOnProcess(zone);
        }

        public ReportProductionHistory GetLastReportOnRevenido()
        {
            try
            {
                return new ReportProductionHistoryFacade().GetLastReportOnRevenido();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
