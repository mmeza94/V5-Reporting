using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Support;
using Tenaris.Fava.Production.Reporting.ViewModel.Dialog;
using Tenaris.Library.UI.Framework.ViewModel;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Support
{
    public class ValidationRules
    {

 


        public static bool CheckForValidIntegers(List<int> textBoxes, bool validateEmptyStrings)
        {
            bool result = true;
            foreach (int tb in textBoxes)
            {
                if (IsValidInteger(tb.ToString()))
                    result = true;
                else
                {                    
                    result = false;
                }                                    
            }
            return result;
        }

        private static bool IsValidInteger(string strNumber)
        {
            Regex objNotIntPattern = new Regex("[^0-9-]");
            Regex objIntPattern = new Regex("^-{0,1}\\d+$");

            var regexResult = !objNotIntPattern.IsMatch(strNumber) &&
                objIntPattern.IsMatch(strNumber);

            bool parseResult = true;
            try
            {
                if (regexResult)
                    int.Parse(strNumber);
            }
            catch (Exception ex)
            {
                if (ex != null)
                    parseResult = false;
            }

            return (regexResult && parseResult);
        }


        public static bool ValidateReportSequence(IList<GeneralPiece> generalPieces, GeneralPiece currentGeneralPice)
        {
            bool result = true;
            var filteredGeneralPieces = generalPieces.Where(x => x.Extremo == currentGeneralPice.Extremo).ToList();

            if (filteredGeneralPieces.Count > 0)
            {
                if (currentGeneralPice.ReportSequence > 1)
                    result = (filteredGeneralPieces[currentGeneralPice.ReportSequence - 2].Sended == Enumerations.AxlrBit.Si) ? true : false;
            }
            return result;
        }

        public static bool ValidateCountersForProductionReport(
            int tbGoodCount, int tbScrapCount,
            int tbReworkedCount, int tbLoadedCount, int ep,
            Enumerations.ProductionReportSendStatus sendStatus)
        {
            bool result = true;
            try
            {
                List<int> textBoxes = new List<int>(){tbGoodCount,tbLoadedCount,
                    tbReworkedCount,tbScrapCount};
                textBoxes.AddRange(textBoxes);
                //bool validIntegers = CheckForValidIntegers(textBoxes, ep, true);
                //if (validIntegers)
                //{
                //    int goodCount = tbGoodCount;
                //    int scrapCount = tbScrapCount;
                //    int reworkedCount = tbReworkedCount;
                //    int loadedCount = tbLoadedCount;
                //    if ((goodCount + scrapCount != loadedCount))
                //    {
                //        ep.SetError(tbLoadedCount, "La cantidad cargada no coincide con el total piezas registradas");
                //        result = false;
                //    }
                //    else
                //        ep.SetError(tbLoadedCount, "");


                //}
                //else
                result = true; //validIntegers


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }


        //Primera sobrecarga
        public static bool ValidateRejectionReasons(int scrapCount, IList rejectionReportDetails)
        {
            
            bool result = true;

            try
            {

                if (rejectionReportDetails != null)
                {
                    int rejections = 0;
                    foreach (RejectionReportDetail rrd in rejectionReportDetails)
                        rejections += rrd.ScrapCount;

                    if (scrapCount != rejections)
                    {
                        //ep.SetError(dgv, "La cantidad de Detalles de Rechazos no coincide con la cantidad de Piezas Malas");
                        result = false;
                    }

                    //ep.SetError(dgv, "");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //segunda sobrecarga
        public static bool ValidateRejectionReasons(int scrapCount, IList rejectionReportDetails,
          int ep, ObservableCollection<RejectionReportDetail> dgv, InteractionRequest<Notification> showErrorWindowRequest)
        {
            bool result = true;

            try
            {
                int rejections = 0;
                if (rejectionReportDetails != null)
                {


                    foreach (RejectionReportDetail rrd in rejectionReportDetails)
                        rejections += rrd.ScrapCount;

                    if (scrapCount != rejections)
                    {
                        ShowError showError = new ShowError("Error", "La cantidad de Detalles de Rechazos no coincide con la cantidad de Piezas Malas");
                        showErrorWindowRequest.Raise(new Notification() { Content = showError });
                        result = false;
                    }
                    //else
                    //ep.SetError(dgv, "");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Función que valida si la tarjeta de línea a reportar en revenido es la correcta de
        /// de acuerdo al orden
        /// </summary>
        /// <param name="generalPieceDto"></param>
        /// <returns></returns>
        public static bool ValidateOnHeatersReportOrder(GeneralPiece generalPieceDto)
        {
            IList<NormalizedLoadHistoryDto> items = null;
            try
            {
                items = new NormalizedLoadHistoryFacade().GetNormalizedLoadHistoryItems();
                var lastReport = new ReportProductionHistoryFacade().GetLastReportOnRevenido();
                //Obtengo el primer registro de carga en normalizado de acuerdo al ultimo reportado en revenido
                var item = items.Where(x => (x.GroupItemNumber == lastReport.GroupItemNumber) &&
                    (x.HeatNumber == lastReport.HeatNumber) &&
                    (x.OrderNumber == lastReport.IdOrder)).OrderBy(x => x.InsDateTime).First();

                //Obtengo la tarjeta de línea que debe reportarse de acuerdo a la siguiete
                var nextItem = items.Where(x => (x.GroupItemNumber != item.GroupItemNumber) &&
                    (x.Identificador > item.Identificador)).
                    OrderBy(x => x.InsDateTime).First();

                return (nextItem.GroupItemNumber == generalPieceDto.GroupItemNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}
