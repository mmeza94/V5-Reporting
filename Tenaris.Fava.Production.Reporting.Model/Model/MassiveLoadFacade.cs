using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class MassiveLoadFacade
    {

        public IList GetAllMassiveLoadRows()
        {
            try
            {
                return new ProductionMassiveLoadRepository().Get(typeof(MassiveLoad));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveMassiveLoadItem(MassiveLoad massiveLoad)
        {
            try
            {
                new ProductionMassiveLoadRepository().Save(massiveLoad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveMassiveLoadItems(IList massiveLoadItems)
        {
            try
            {
                new ProductionMassiveLoadRepository().Save(massiveLoadItems);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MassiveLoad GetFirstMassiveLoadRow(int? lastId)
        {
            try
            {
                return new ProductionMassiveLoadRepository().GetFirstMassiveLoadRow(lastId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList GetMassiveLoadRows(int? orderNumber, int? groupItemNumber,
           int? heatNumber, Enumerations.ProductionReportSendStatus? sendStatus)
        {
            try
            {
                return new ProductionMassiveLoadRepository().GetMassiveLoadRows(
                    orderNumber, groupItemNumber, heatNumber, sendStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
