using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.Builder
{
    public interface IBuilder
    {
        
        IBuilder OrderByDate();
        IBuilder GetReportSequence();
        IBuilder GetLoadedCountByBundle();
        IBuilder SetSendStatus();


    }
}
