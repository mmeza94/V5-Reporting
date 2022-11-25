using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;

namespace Tenaris.Fava.Production.Reporting.Model.Business
{
    public class PaintingReportBusiness
    {
        private static PaintingReportBusiness instance;
        private static readonly object locker = new object();
        public static PaintingReportBusiness Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new PaintingReportBusiness();
                        }
                    }
                }
                return instance;
            }
        }




    }
}
