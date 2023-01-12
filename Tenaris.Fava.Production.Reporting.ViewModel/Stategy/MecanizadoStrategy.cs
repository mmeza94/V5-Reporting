using Castle.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public class MecanizadoStrategy: GeneralMachine, IActions
    {
        private ObservableCollection<ReportProductionHistory> productionReportHistories;
        private GeneralPiece SelectedBundle;
        private IFormatterPiece formatterPiece;
        public GeneralMachine GeneralMachine { get => this; }
        public IReportingProcess reportingProcess { get; set; }
        public Dictionary<string, object> Filters { get; set; }
        public Dictionary<string, object> OutPuts { get; set; }

        public MecanizadoStrategy()
        {
            //reportingProcess = new RPMecanizadoExtremo1(this);
            Filters = Filter;
            OutPuts = OutPut;
            formatterPiece = new ProcessorPieces.ProcessorByForjas();

        }


        public IActions Search()
        {
            try
            {
                CurrentGeneralPieces = ProductionReportingBusiness.GetProductionGeneral(Filters);

                CurrentGeneralPieces = CurrentGeneralPieces.FormatterPieces(formatterPiece).Where(x => (x.SendStatus == Enumerations.ProductionReportSendStatus.Parcial) || (x.SendStatus == Enumerations.ProductionReportSendStatus.Completo)).ToList(); ;

                AddValues("Search", CurrentGeneralPieces.ToObservableCollection());
                return this;

            }
            catch (Exception)
            {
                throw;
            }
        }



        public IActions Report()
        {
            try
            {
                GeneralPiece currentDGRow = (GeneralPiece)Filters["Selected_Bundle"];

                reportingProcess = SelectReportingProcess(currentDGRow);


                var ReportPRoduction = GetCurrentGroupItemToReport(currentDGRow);

                if (!reportingProcess.CanReport(currentDGRow, ReportPRoduction))
                    return this;

                if (!reportingProcess.IsReportConfirmationAccepted(currentDGRow))
                    return this;



                //ValidateExtreme(ReportPRoduction, currentDGRow);



                ReportProductionDto currentReportProductionDTO = reportingProcess.BuildReport()
                                                                                 .ValidateReportStructure()
                                                                                 .PrepareDtoForProductionReport();

                var response = Adapter.ReportProduction(WhoIsLogged, currentReportProductionDTO, currentReportProductionDTO.SelectedSendType,
                    true, reportingProcess.dgRejectionReportDetails);


                reportingProcess.ShowITMessage(response);



                reportingProcess.CheckReportProductionForNextOperation(response);
            }
            catch (Exception)
            {

            }
            return this;
        }

        private IReportingProcess SelectReportingProcess(GeneralPiece currentPiece)
        {
            IReportingProcess SelectedReportingProcess = null;

            if (currentPiece.Extremo.Contains("1"))
            {
                SelectedReportingProcess = new RPMecanizadoExtremo1(this);
            }
            else
            {
                SelectedReportingProcess = new RPCajas(this);

            }

            return SelectedReportingProcess;
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
                return new ObservableCollection<ReportProductionHistory>();

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

        

        private ReportProductionDto ValidateExtreme(ReportProductionDto rp, GeneralPiece currentDGRow)
        {


            if (currentDGRow.Extremo.Contains("2"))
            {
                rp.Operacion = "Forjadora Extremo 2";
                rp.Secuencia = 5;
            }
            return rp;
        }
    }
}
