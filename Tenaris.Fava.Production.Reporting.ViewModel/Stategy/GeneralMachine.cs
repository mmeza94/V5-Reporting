﻿using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Tenaris.Fava.Production.Reporting.Model.Adapter;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public abstract class GeneralMachine
    {
        #region Properties
        public string WhoIsLogged { get; set; }
        public ITServiceAdapter Adapter { get; set; }
        public IList<GeneralPiece> CurrentGeneralPieces { get; set; }
        public InteractionRequest<Notification> Request, IndBoxReportConfirmationRequest,
            ShowErrorMessageRequest, ShowMessageRequest, ShowQuestionRequests;
        public Dictionary<string, object> Filter, OutPut;
        #endregion

        #region Constructor
        public GeneralMachine()
        {
            Adapter = new ITServiceAdapter();
            Filter = new Dictionary<string, object>();
            OutPut = new Dictionary<string, object>();
        }
        #endregion

        #region Methods

        public GeneralMachine AddFilter(string key, object value)
        {
            if (!Filter.ContainsKey(key))
                Filter.Add(key, value);

            Filter[key] = value;

            return this;
        }

        public GeneralMachine AddValues(string key, object value)
        {
            if (!OutPut.ContainsKey(key))
                OutPut.Add(key, value);
            OutPut[key] = value;
            return this;
        }
        public bool Login()
        {

            if (!(ConfigurationManager.AppSettings["UserByPass"] == ""))
            {
                WhoIsLogged = ConfigurationManager.AppSettings["UserByPass"];
                return true;
            }

            WhoIsLogged = ProductionReportingBusiness.GetCurrentUser();

            if (string.IsNullOrEmpty(WhoIsLogged))
            {
                ShowErrorMessageRequest.Raise(new Notification() { Content = new ShowError("Error", string.Format("No se pudo iniciar sesión en el sistema. Operación cancelada")) });
                return false;
            }

            return true;

        }

        public bool IsSended(ReportProductionDto reportDto)
        {
            if (reportDto == null)
                return false;

            bool bypass = ConfigurationManager.AppSettings["Bypass"] == "true";
            if (!(reportDto.Enviado == Enumerations.AxlrBit.No || bypass))
            {
                ShowError showError = new ShowError("Error", string.Format("Este reporte ya ha sido enviado. Operación cancelada"));
                ShowErrorMessageRequest.Raise(new Notification() { Content = showError });
                return false;
            }
            return true;
        }

        public GeneralMachine SetRequest(InteractionRequest<Notification> request)
        {
            this.Request = request;
            return this;
        }

        public GeneralMachine SetIndBoxReportConfirmationRequest(InteractionRequest<Notification> IndBoxReportConfirmationRequest)
        {
            this.IndBoxReportConfirmationRequest = IndBoxReportConfirmationRequest;
            return this;
        }

        public GeneralMachine SetShowErrorMessageRequest(InteractionRequest<Notification> showErrorMessageRequest)
        {
            this.ShowErrorMessageRequest = showErrorMessageRequest;
            return this;
        }

        public GeneralMachine SetShowMessageRequest(InteractionRequest<Notification> showMessageRequest)
        {
            this.ShowMessageRequest = showMessageRequest;
            return this;
        }

        public GeneralMachine SetShowQuestionRequests(InteractionRequest<Notification> ShowQuestionRequests)
        {
            this.ShowQuestionRequests = ShowQuestionRequests;
            return this;
        }

        public bool IsReportSequenceValidated(IList<GeneralPiece> generalPieces, GeneralPiece currentGeneralPice)
        {
            if (!ValidationRules.ValidateReportSequence(generalPieces, currentGeneralPice))
            {
                ShowError showError = new ShowError("Error", string.Format("Debe reportar el parcial anterior. Operación cancelada"));
                ShowErrorMessageRequest.Raise(new Notification() { Content = showError });
                throw new System.Exception();
            }
            return true;
        }

        public int GetFirstPieceLoadedNumberForIT(GeneralPiece GeneralPiece)
        {
            int FirstReportedLoadedCount = CurrentGeneralPieces.Where(c => (c.GroupItemNumber == GeneralPiece.GroupItemNumber)
                                           && (c.ReportSequence == 1) && (c.Extremo == GeneralPiece.Extremo)).First().LoadedCount;

            return FirstReportedLoadedCount;
        }

        #endregion
    }
}
