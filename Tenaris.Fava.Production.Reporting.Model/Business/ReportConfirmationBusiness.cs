using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;

namespace Tenaris.Fava.Production.Reporting.Model.Business
{
    public class ReportConfirmationBusiness
    {
        //Singleton
        private static ReportConfirmationBusiness instance;
        private static readonly object locker = new object();
        public static ReportConfirmationBusiness Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ReportConfirmationBusiness();
                        }
                    }
                }
                return instance;
            }
        }


        public List<RejectionReportDetail> removeRejection(RejectionReportDetail rejectionReportDetail)
        {
            throw new NotImplementedException();
        }
        public void report()
        {
        }
        public void Cancel()
        {
        }
        public bool unlockReport()
        {
            //Verificar con login, devolver true en caso
            //de login exitoso.
            return false;
        }

        
    
        public RejectionReportDetail addRejection(int cantidad, string destino, string razonDescarte,
            bool extremo1, bool extremo2, bool trabajado, string motivo)
        {
            //TO-DO: Validar los datos enviados y devolver objeto
            //Si los datos no pasan la validación devolver null

            throw new NotImplementedException();
        }
    }
}
