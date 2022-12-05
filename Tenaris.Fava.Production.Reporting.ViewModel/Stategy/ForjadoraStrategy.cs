﻿using Castle.Core;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public class ForjadoraStrategy : GeneralMachine, IActions
    {
        private ObservableCollection<ReportProductionHistory> productionReportHistories;
        private GeneralPiece SelectedBundle;
        public GeneralMachine GeneralMachine { get => this; }
        public IReportingProcess reportingProcess { get; set; }

        public ForjadoraStrategy()
        {
            reportingProcess = new RPGeneral(this);
        }

        public bool Report(GeneralPiece currentDGRow)
        {
            var ReportPRoduction = GetCurrentGroupItemToReport(currentDGRow);

            if (!reportingProcess.CanReport(currentDGRow, ReportPRoduction))
                return false;

            if (!reportingProcess.IsReportConfirmationAccepted(currentDGRow))
                return false;

            ReportProductionDto currentReportProductionDTO = reportingProcess.BuildReport()
                                                                             .ValidateReportStructure()
                                                                             .PrepareDtoForProductionReport();

            var response = Adapter.ReportProduction(WhoIsLogged, currentReportProductionDTO, currentReportProductionDTO.SelectedSendType,
                true, reportingProcess.dgRejectionReportDetails);


            reportingProcess.ShowITMessage(response);



            reportingProcess.CheckReportProductionForNextOperation(response);


            return false;
        }

        public ObservableCollection<GeneralPiece> Search(int Orden, int Colada, int Atado)
        {
            try
            {
                var generalPieces = ProductionReport.GetProductionGeneral(Orden, Colada, Atado);
                if (generalPieces == null)
                    return new ObservableCollection<GeneralPiece>();

                CurrentGeneralPieces = ProductionReport.ClassifyBySendStatus(generalPieces).ToList();


                if (Configurations.Instance.Machine == "Forjadora 0")
                {
                    CurrentGeneralPieces.ForEach(piece => GeneralPieceProcessor(piece));

                }


                return new ObservableCollection<GeneralPiece>(CurrentGeneralPieces);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ReportProductionDto GetCurrentGroupItemToReport(GeneralPiece currentDGRow)
        {
            ReportProductionDto reportDto = currentDGRow.BuildReportProductionDTO();
            reportDto.Operacion = GetOperation(currentDGRow, reportDto);
            reportDto.Secuencia = GetSequenceForDifferentExtreme(currentDGRow, reportDto);
            return reportDto;
        }


        public string GetOperation(GeneralPiece GeneralPiece, ReportProductionDto reportProductDto)
        {
            string operation = Configurations.Instance.Operacion;
            if (GeneralPiece.Extremo.ToString() != String.Empty)
            {
                //Hard Code por reporte de las dos operaciones en mismo colgante 
                if (reportProductDto.DescripcionMaquina.ToUpper().Contains("FORJADORA"))
                    operation = "Forjado " + GeneralPiece.Extremo;
                else if (reportProductDto.DescripcionMaquina.Contains("Roscadora"))
                    operation = "Mecanizado " + GeneralPiece.Extremo;

            }
            return operation;
        }


        public int GetSequenceForDifferentExtreme(GeneralPiece GeneralPiece, ReportProductionDto reportProductDto)
        {
            if (GeneralPiece.Extremo.ToString() != string.Empty)
            {
                if (GeneralPiece.Extremo.EndsWith("2"))
                    return reportProductDto.Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia) + 1;
                else
                    return reportProductDto.Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia);
            }
            else
                return reportProductDto.Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia);
        }

        public ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle)
        {
            this.SelectedBundle = SelectedBundle;
            if (SelectedBundle == null)
                return productionReportHistories = new ObservableCollection<ReportProductionHistory>();

            productionReportHistories = ProductionReportingBusiness.GetReportProductionHistoryByParamsTest(
                 new Dictionary<string, object>
             {
                                { "@Order", SelectedBundle.OrderNumber },
                                { "@GroupItemNumber", SelectedBundle.GroupItemNumber },
                                { "@HeatNumber", SelectedBundle.HeatNumber },
                                { "@idHistory", 0 },
                                { "@SendStatus", 0 },
                                { "@MachineSequence", 0 }
             });
            return productionReportHistories;
        }


        private void GeneralPieceProcessor(GeneralPiece currentDGRow)
        {
            var num1 = 0;
            var num2 = 0;

            ObservableCollection<ReportProductionHistory> collection = dgReporteProduccion_SelectionChanged(currentDGRow);

            foreach (ReportProductionHistory productionHistory in collection)
            {
                if (productionHistory.MachineOperation.Contains(SelectedBundle.Extremo))
                {
                    num1 += productionHistory.GoodCount;
                    num2 += productionHistory.ScrapCount;
                }

            }

            currentDGRow.BuenasReportadas = num1;
            currentDGRow.MalasReportadas = num2;
            currentDGRow.TotalReportado = (num1 + num2);
            currentDGRow.PendientesPorReportar = currentDGRow.LoadedCount - (num1 + num2);
            if (currentDGRow.PendientesPorReportar != 0)
            {
                currentDGRow.GoodCount = currentDGRow.GoodCount - num1;
                currentDGRow.ScrapCount = currentDGRow.ScrapCount - num2;
                currentDGRow.LoadedCount = currentDGRow.GoodCount + currentDGRow.ScrapCount;
            }
            else
            {
                currentDGRow.GoodCount = 0;
                currentDGRow.ScrapCount = 0;
                currentDGRow.LoadedCount = 0;
            }


            currentDGRow.Cargados = currentDGRow.BuenasReportadas + currentDGRow.MalasReportadas;


        }





    }
}
