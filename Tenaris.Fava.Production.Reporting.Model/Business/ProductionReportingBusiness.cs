﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.Data_Access;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.Business
{
    public class ProductionReportingBusiness
    {


        #region Granalladora, Enderezadora y CND

        public static ObservableCollection<GeneralPiece> GetProductionGeneral(Dictionary<String, object> listParams)
        {

            return DataAccessSQL.Instance.GetProductionGeneral(listParams);
        }


        public static ObservableCollection<ReportProductionHistory> GetReportProductionHistory(Dictionary<String, object> listParams, string ConnectionString)
        {

            return DataAccessSQL.Instance.GetReportProductionHistory(listParams, ConnectionString);
        }



        public static ObservableCollection<int> GetPreviousCountersByMachineTest(Dictionary<string, object> listParams)
        {
            return DataAccessSQL.Instance.GetPreviousCountersByMachineTest(listParams);
        }




        public static ObservableCollection<ReportProductionHistory> GetReportProductionHistoryByParamsTest(Dictionary<string, object> listParams)
        {
            return DataAccessSQL.Instance.GetReportProductionHistoryByParamsTest(listParams);

        }



        public static int InsReportProductionHistoryTestV5(ReportProductionDto reportProductionDto, Enumerations.ProductionReportSendStatus sendStatus)
        {
            //todas las maquinas

            return DataAccessSQL.Instance.InsReportProductionHistoryTestV5(reportProductionDto, sendStatus);

        }
        public static int InsBundleReportProductionHistoryTestV5(ReportProductionDto reportProductionDto, Enumerations.ProductionReportSendStatus sendStatus)
        {
            //PINTADO, CAJAS Y MECANIZADO EXTREMO 1
            return DataAccessSQL.Instance.InsBundleReportProductionHistoryTestV5(reportProductionDto, sendStatus);

        }

        public static List<int> GetCuttingNumbers(Dictionary<String, object> listParams)
        {
            return DataAccessSQL.Instance.GetCuttingNumbers(listParams);
        }


        public static Enumerations.ForgeMode GetCurrentForgeMode(int GroupitemNumber)
        {
            return DataAccessSQL.Instance.GetCurrentForgeMode(GroupitemNumber);

        }

        public static int GetLastMachineGoodPieces(int groupItemNumber, int Sequence)
        {
            return DataAccessSQL.Instance.GetLastMachineGoodPieces(groupItemNumber, Sequence);

        }

      


        public static void InsRejectionReportDetailTestV5(RejectionReportDetail rejectionReportDetail, int IdReportProductionHistoryInserted)
        {
            DataAccessSQL.Instance.InsRejectionReportDetailTestV5(rejectionReportDetail, IdReportProductionHistoryInserted);

        }










        public static int GetPreviousSequenceByOperation(string operation)
        {

            return DataAccessSQL.Instance.GetPreviousSequenceByOperation(operation);
        }


        public static int GetPreviousSequence(string description)
        {

            return DataAccessSQL.Instance.GetPreviousSequence(description);
        }



        public static int IsBoxSelect(int numberOrderMotehr, string idBox)
        {

            return DataAccessSQL.Instance.IsBoxSelect(numberOrderMotehr, idBox);
        }


        public static string BoxSelect(int numberOrderMotehr)
        {

            return DataAccessSQL.Instance.BoxSelect(numberOrderMotehr);
        }

        public static int GetActiveBox()
        {

            return DataAccessSQL.Instance.GetActiveBox();
        }


        public static void UpdBoxReportada(string idBox)
        {

            DataAccessSQL.Instance.UpdBoxReportada(idBox);
        }


        public static ObservableCollection<BoxReport> GetBoxesForPainting(Dictionary<string, object> listParams)
        {
            return DataAccessSQL.Instance.GetBoxesForPainting(listParams);
        }
        public static int InsLoadPintado(PaintingReport reportProductionDto)
        {

            return DataAccessSQL.Instance.InsLoadPintado(reportProductionDto);
        }


        public static ObservableCollection<BoxLoad> GetLoadPainting(Dictionary<string, object> listparams)
        {

            return DataAccessSQL.Instance.GetLoadPainting(listparams);
        }

        public static OPChildrens GetNextOpChildrenActive(int OrdenNumberMother)
        {

            return DataAccessSQL.Instance.GetNextOpChildrenActive(OrdenNumberMother);
        }

      

        public static string GetCurrentUser()
        {
            return DataAccessSQL.Instance.GetCurrentUser();
        }


        public static bool LoginUser(string user, string Password)
        {
            return DataAccessSQL.Instance.LoginUser(user, Password);

        }

        #endregion
    }
}
