using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.Data_Access;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class ReportProductionHistoryFacade
    {
        public ObservableCollection<ReportProductionHistory> GetReportProductionHistoryByParams(int? orderNumber, int? groupItemNumber, int? heatNumber,
            int? idHistory, Enumerations.ProductionReportSendStatus? sendStatus, int? machineSequence)
        {
            return new ReportProductionHistoryRepository().GetReportProductionHistoryByParams
                (orderNumber, groupItemNumber, heatNumber, idHistory, sendStatus, machineSequence);
        }

       


  

        //Guardado del reporte REFACTORIZACION
        public void SaveReportProductionHistory(ReportProductionDto reportProductionDto,
            Enumerations.ProductionReportSendStatus sendStatus, IList rejectionReportDetails)
        {
            try
            {
                    int idReportProductionHistoryInserted = ProductionReportingBusiness.InsReportProductionHistoryTestV5(reportProductionDto, sendStatus);
                    if (rejectionReportDetails != null && rejectionReportDetails.Count > 0)
                        foreach (RejectionReportDetail rrd in rejectionReportDetails)
                            ProductionReportingBusiness.InsRejectionReportDetailTestV5(rrd, idReportProductionHistoryInserted);
                


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
                if (Configurations.Instance.VersionApplication != "V1" || Configurations.Instance.Machine.ToUpper() == "HORNO DE NORMALIZADO" || Configurations.Instance.Machine.ToUpper() == "HORNO DE REVENIDO")
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
                else
                {
                    var reportProductionHistory = new ReportProductionHistoryV1
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
                        //GroupItemType = reportProductionDto.TipoUDT,
                        //ChildOrder = reportProductionDto.Orden,
                        //ChildGroupItemNumber = reportProductionDto.IdUDT,
                        //ChildGroupItemType = reportProductionDto.TipoUDT

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
       
        public int GetLastMachineGoodPieces(int order, int heat, int groupItem, string description, string extreme)
        {
            return new ReportProductionHistoryRepository().GetLastMachineGoodPieces(order, heat, groupItem, description, extreme);
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
