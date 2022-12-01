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
                        
                        result = false;
                    }

                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        
    
       
    }


}
