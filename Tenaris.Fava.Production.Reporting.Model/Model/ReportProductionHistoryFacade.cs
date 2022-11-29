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

        public ObservableCollection<ReportProductionHistoryV1> GetReportProductionHistoryByParamsV1(int? orderNumber, int? groupItemNumber, int? heatNumber,
            int? idHistory, Enumerations.ProductionReportSendStatus? sendStatus, int? machineSequence)
        {
            return new ReportProductionHistoryRepository().GetReportProductionHistoryByParamsV1
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
        public ReportProductionHistoryV1 GetReportProductionHistoryByIdHistoryV1(int idHistory, int orderNumber,
            int heatNumber, int groupItemNumber)
        {
            return new ReportProductionHistoryRepository().GetReportProductionHistoryByIdHistoryV1(idHistory, orderNumber,
                heatNumber, groupItemNumber);
        }
        public ReportProductionHistory GetLastMachineProductionReport(ReportProductionDto reportProductionDto)
        {
            return new ReportProductionHistoryRepository().GetLastMachineReportProduction(reportProductionDto);
        }

        private List<RejectionReportDetailV1> RejectionMapper(List<RejectionReportDetail> rejectionreport)
        {
            List<RejectionReportDetailV1> mapping = new List<RejectionReportDetailV1>();
            ReportProductionHistoryV1 a = new ReportProductionHistoryV1();
            foreach (var item in rejectionreport)
            {

                //if (true)
                //{

                //}
                //a.IdHistory = item.ReportProductionHistory.IdHistory;
                //a.Id = item.ReportProductionHistory.Id;
                //  a.IdOrder = item.ReportProductionHistory.IdOrder;
                //   a.HeatNumber = item.ReportProductionHistory.HeatNumber;
                //  a.GroupItemNumber = item.ReportProductionHistory.GroupItemNumber;
                //  a.SendStatus = item.ReportProductionHistory.SendStatus;
                //   a.TotalQuantity = item.ReportProductionHistory.TotalQuantity;
                //   a.GoodCount = item.ReportProductionHistory.GoodCount;
                //    a.ScrapCount = item.ReportProductionHistory.ScrapCount;
                //    a.ReworkedCount = item.ReportProductionHistory.ReworkedCount;
                //    a.IdMachine = item.ReportProductionHistory.IdMachine;
                //    a.LotNumberHtr = item.ReportProductionHistory.LotNumberHtr;
                //   a.InsDateTime = item.ReportProductionHistory.InsDateTime;
                //   a.InsertedBy = item.ReportProductionHistory.InsertedBy;
                //   a.UpdDateTime = item.ReportProductionHistory.UpdDateTime;
                //   a.UpdatedBy = item.ReportProductionHistory.UpdatedBy;
                //   a.MachineSequence = item.ReportProductionHistory.MachineSequence;
                //   a.MachineOption = item.ReportProductionHistory.MachineOption;
                //   a.MachineOperation = item.ReportProductionHistory.MachineOperation;
                //a.Observation = item.ReportProductionHistory.Observation;


                mapping.Add(new RejectionReportDetailV1
                {
                    Id = item.Id,
                    Extremo = item.Extremo,
                    Destino = item.Destino,
                    Active = item.Active,
                    InsDateTime = item.InsDateTime,
                    Observation = item.Observation,
                    RejectionCode = item.RejectionCode,
                    RejectionCodeDescription = item.RejectionCodeDescription,
                    ReportProductionHistory = new ReportProductionHistoryV1(),
                    ScrapCount = item.ScrapCount,
                    Trabajado = item.Trabajado,
                    UpdDateTime = item.UpdDateTime
                }); ;
            }

            return mapping;
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
                


                    //if (rejectionReportDetails != null && rejectionReportDetails.Count > 0)
                    //{
                    //    List<RejectionReportDetailV1> mappedlist = RejectionMapper(
                    //        new List<RejectionReportDetail>(rejectionReportDetails.OfType<RejectionReportDetail>()));
                    //    foreach (RejectionReportDetailV1 rrd in mappedlist)
                    //        rrd.ReportProductionHistory = reportProductionHistory;
                    //    new ReportProductionHistoryV1Repository().Save(reportProductionHistory, mappedlist);
                    //}
                    //else
                    //    new ReportProductionHistoryV1Repository().Save(reportProductionHistory);
                


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
