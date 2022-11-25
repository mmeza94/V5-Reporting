using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.Model.Support;

namespace Tenaris.Fava.Production.Reporting.ViewModel.FilterMachine
{
    public class ConditionsMachineFilter
    {

        public static ObservableCollection<GeneralPiece> filterPopulateGeneralProduction(ObservableCollection<GeneralPiece> generalPieces, IList<GeneralPiece> currentGeneraPieces, int Atado, string ConnectionString)
        {
            if (generalPieces != null)
            {

      
                    //currentGeneraPieces = new ProductionReport().ClassifyBySendStatus(generalPieces).Where(x => x.Description == Configurations.Instance.MachineFiltre).ToList();

                    //currentGeneraPieces = generalPieces.Where(x => x.Description == Configurations.Instance.MachineFiltre).ToList();


                switch (Configurations.Instance.Machine)
                {
                    case "Roscadora":

                        if (currentGeneraPieces != null)
                        {
                            currentGeneraPieces = currentGeneraPieces.Where(x => (x.SendStatus == Enumerations.ProductionReportSendStatus.Partial) ||
                                (x.SendStatus == Enumerations.ProductionReportSendStatus.Complete)).ToList();
                        }

                        break;
                    
                    case "Forjadora":

                        currentGeneraPieces = GetForgeCurrentGeneralPieces(currentGeneraPieces, Atado);



                        break;

                    default:
                        currentGeneraPieces = new ProductionReport().ClassifyBySendStatus(generalPieces,ConnectionString).Where(x => x.Description == ConfigurationManager.AppSettings["Machine"].ToString()).ToList();
                        break;
                }


         
            }
            //if (ConfigurationManager.AppSettings["Extremo"].ToString() != "")
            //    currentGeneraPieces = currentGeneraPieces.Where(x => x.Extremo == ConfigurationManager.AppSettings["Extremo"].ToString()).ToList();

            return new ObservableCollection<GeneralPiece>(currentGeneraPieces);
        }


        public static IList<GeneralPiece> GetForgeCurrentGeneralPieces(IList<GeneralPiece> currentPieces, int Atado)
        {

            var forgeMode = new ProductionReportFacade().GetCurrentForgeMode(Atado);
            if (forgeMode == Enumerations.ForgeMode.OneEnd)
            {
                currentPieces = currentPieces.Where(x => x.Extremo == "Extremo 1").ToList();
                foreach (var currentPiece in currentPieces)
                {
                    var productionReport = new ProductionReport();
                    currentPiece.Extremo = "Extremo 2";
                    //if(productionReport.IsForjadoraAndForgeModeIsOnline(currentPiece.GroupItemNumber)) 
                    currentPiece.LoadedCount = new ReportProductionHistoryFacade().
                     GetLastMachineGoodPieces(currentPiece.OrderNumber,
                       currentPiece.HeatNumber, currentPiece.GroupItemNumber,
                       currentPiece.Description, currentPiece.Extremo);
                }
            }

            return currentPieces;
        }


    }
}
