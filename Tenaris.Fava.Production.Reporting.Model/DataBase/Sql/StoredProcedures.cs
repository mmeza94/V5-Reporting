using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.Data_Access
{
    public class StoredProcedures
    {

        public const string GetProductionGeneral = "[DBO].[GetProductionGeneral]"; //V1B ReportProductionDB
        public const string GetProductionGeneralProgrammed = "[dbo].[GetProductionGeneralProgrammed]"; //V1B ReportProductionDB
        public const string GetProductionGeneralProgrammedTorno4 = "[dbo].[GetProductionGeneralProgrammedTorno4]"; //V1B ReportProductionDB
        public const string GetProductionGeneralProgrammedTorno2 = "[dbo].[GetProductionGeneralProgrammedTorno2]"; //V1B ReportProductionDB
        public const string GetProductionGeneralV2 = "[DBO].[GetProductionGeneralV2]"; //V1B ReportProductionDB
        public const string GetReportProductionHistory = "[ProductionFIN].[GetReportProductionHistory]"; //V1 ForjaReportProductionDB
        public const string GetPreviousSequence = "[ProductionFIN].[GetPreviousSequence]"; //V1 ForjaReportProductionDB
        public const string GetPreviousSequenceByOperation = "[ProductionFIN].[GetPreviousSequenceByOperation]"; //V1 ForjaReportProductionDB
        public const string GetNextOpChildrenActive = "[ProductionGuide].[GetNextOpChildrenActive]"; //V4 ReportProductionDBV4
        public const string UpdBoxReportada = "[ProductionGuide].[UpdBoxReportada]"; //V4 ReportProductionDBV4
        public const string IsBoxSelect = "[ProductionGuide].[IsBoxSelect]"; //V4 ReportProductionDBV4
        public const string GetBoxesForPainting = "[ProductionFIN].[GetBoxesForPainting]"; //V3 ReportProductionDBV3
        public const string InsLoadPintado = "[ProductionFIN].[InsLoadPintado]"; //V3 ReportProductionDBV3
        public const string GetLoadPainting = "[ProductionFIN].[GetLoadPainting]"; //V3 ReportProductionDBV3
        public const string GetCurrentUserByWorkstation = "[Security].[GetCurrentUserByWorkstation]";
        public const string GetActiveBox = "[ProductionGuide].[GetBoxActive]";


        //public static Dictionary<string, string> StoresDataBase { get; set; } =new Dictionary<string, string>();

        public StoredProcedures()
        {
            //StoresDataBase.Add(GetProductionGeneral, ConfigurationManager.AppSettings["V1B"]);
            //StoresDataBase.Add(GetReportProductionHistory, ConfigurationManager.AppSettings["V1"]);
            //StoresDataBase.Add(GetPreviousSequence, ConfigurationManager.AppSettings["V1"]);
            //StoresDataBase.Add(GetPreviousSequenceByOperation, ConfigurationManager.AppSettings["V1"]);
            //StoresDataBase.Add(GetNextOpChildrenActive, ConfigurationManager.AppSettings["V4"]);
            //StoresDataBase.Add(UpdBoxReportada, ConfigurationManager.AppSettings["V4"]);
            //StoresDataBase.Add(IsBoxSelect, ConfigurationManager.AppSettings["V4"]);
            //StoresDataBase.Add(GetBoxesForPainting, ConfigurationManager.AppSettings["V3"]);
            //StoresDataBase.Add(InsLoadPintado, ConfigurationManager.AppSettings["V3"]);
            //StoresDataBase.Add(GetLoadPainting, ConfigurationManager.AppSettings["V3"]);
        }

    }
}
