using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.Data_Access
{
    public class StoredProcedures
    {

        #region Test
        //Testeado para Grana, Enderezadora y CND---probar forja 2 y hornos
        public static string GetProductionGeneralTest = Configurations.Instance.GetProductionGeneral;
        public const string GetRejectionCodeByMachineDescriptionTestV5 = "[Production].[GetRejectionCodeByMachineDescriptionTestV5]";
        //SelectionChanged
        public const string GetReportProductionHistoryByParamsTest = "[Production].[GetReportProductionHistoryByParamsTest]";
        public const string GetPreviousCountersByMachineTest = "[Production].[GetPreviousCountersByMachineTest]";
        public const string InsReportProductionHistoryTestV5 = "[Production].[InsReportProductionHistoryTestV5]";
        public const string InsRejectionReportDetailTestV5 = "[Production].[InsRejectionReportDetailTestV5]";
        public const string LoginUser = "[Common].[LoginUserTestV5]";





        //Testeado para Forja 0 y 1
        public const string GetProductionGeneralV2Test = "[dbo].[GetProductionGeneralV2Test]";
        //Testeado para Forja 0 y 1
        public const string GetProductionGeneralTestForjasyHornos = "[dbo].[GetProductionGeneralTestForjasyHornos]";



        #endregion

        #region Sin refactorizar
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
        #endregion

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
