using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.Enums
{
    public class Enumerations
    {
        public enum ProductionReportSendStatus
        {
            ForSend = 0,
            Parcial,
            Final,
            Completo

        }
        public enum AxlrBit
        {
            No = 0,
            Si
        }
        public enum Zone
        {
            CND,
            Forja,
            Hornos,
            Granalla2,
            Meca
        }
        public enum ForgeMode
        {
            OneEnd = 0,
            BothEnds
        }
    }
}
