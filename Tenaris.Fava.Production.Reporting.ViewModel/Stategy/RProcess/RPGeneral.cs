using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess
{
    public class RPGeneral : IReportingProcess
    {
        private ReportProductionDto ReportProductionDto { get; set; }

        private ReportConfirmationViewModel reportConfirmation { get; set; }
        private GeneralMachine GeneralMachine { get; set; }

        public  RPGeneral(GeneralMachine generalMachine)
        {
            this.GeneralMachine = generalMachine;
        }

        public bool CanReport(GeneralPiece currentDGRow)
        {
            bool response = true;
            var generalPieceDto = currentDGRow;
            ReportProductionDto = generalPieceDto.BuildReportProductionDTO();
            var maquina = Configurations.Instance.Machine;

            if (!GeneralMachine.Login())
                return response;

            if (!GeneralMachine.IsSended(ReportProductionDto))
                return response;

            if (!GeneralMachine.IsReportSequenceValidated(GeneralMachine.currentGeneralPieces, generalPieceDto))
                return response;

            return response;
        }

        public bool IsReportConfirmationAccepted(GeneralPiece currentDGRow)
        {
            reportConfirmation = new ReportConfirmationViewModel(
                                    currentDGRow, ReportProductionDto, GeneralMachine.GetFirstPieceLoadedNumberForIT(currentDGRow), true, GeneralMachine.WhoIsLogged);
            GeneralMachine.Request.Raise(new Notification() { Content = reportConfirmation });

            return reportConfirmation.Result;
        }

        public void BuildReport()
        {
            //currentProductionReportDto = reportConfirmation.CurrentReportProduction;
            //currentProductionReportDto.IdUDT = reportConfirmation.Atado;
            //currentProductionReportDto.Orden = reportConfirmation.Orden;
            //currentProductionReportDto.Colada = reportConfirmation.Colada;
            //string SelectedSendType = reportConfirmation.SelectedSendType;
            //ObservableCollection<RejectionReportDetail> dgRejectionReportDetails = reportConfirmation.RejectionReportDetails;
            //int lbITLoadHelper = reportConfirmation.ITLoadHelper;
            //int tbTotalLoaded = reportConfirmation.CargadasTotal;
            //int tbPreviousLoaded = reportConfirmation.CargadasAnterior;
            //string _User = whoIsLogged;

            //tbScrapCountL2 = reportConfirmation.MalasActual;
            //tbReworkedCountL2 = reportConfirmation.ReprocesosActual;
            //tbLoadedCountL2 = reportConfirmation.CargadasActual;
            //tbGoodCountL2 = reportConfirmation.BuenasActual;
        }


        public void PrepareDtoForProductionReport()
        {
            throw new NotImplementedException();
        }

        public void ValidateReportStructure()
        {
            throw new NotImplementedException();
        }

        
    }
}
