using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows.Controls;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy;
using Tenaris.Fava.Production.Reporting.ViewModel.Support;

namespace Tenaris.Fava.Production.Reporting.Model.Stategy
{
    public class GranalladoraStrategy : GeneralMachine, IActions
    {
        #region Properties

        public GeneralMachine GeneralMachine { get => this; }
        #endregion

        #region Implements methods
        public ObservableCollection<GeneralPiece> Search(int Orden, int Colada, int Atado)
        {
            try
            {
                var generalPieces = ProductionReport.GetProductionGeneral(Orden, Colada, Atado);
                if (generalPieces == null)
                    return null;
                currentGeneralPieces = ProductionReport.ClassifyBySendStatus(generalPieces).ToList();

                return new ObservableCollection<GeneralPiece>(currentGeneralPieces);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public bool Report(GeneralPiece currentDGRow)
        {
            bool response = true;
            var generalPieceDto = currentDGRow;
            var reportProductionDto = GetCurrentGroupItemToReport(currentDGRow);

            if (!Login())
                return response;
            //if (reportProductionDto == null)
            //    return response;
            if (!IsSended(reportProductionDto))
                return response;

            if (!IsReportSequenceValidated(currentGeneralPieces, generalPieceDto))
                return response;
          
            return false;
        }

        
        #endregion

    }
}
