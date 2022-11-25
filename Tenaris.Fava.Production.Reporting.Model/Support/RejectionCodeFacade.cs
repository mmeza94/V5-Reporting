using System;
using System.Collections;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class RejectionCodeFacade
    {
        public IList GetRejectionCodeByIdMachine(int idMachine)
        {
            try
            {
                Machine machine = (Machine)new CommonMachineRepository().Get(typeof(Machine), idMachine);
                return new ProductionRejectionCodeRepository().GetRejectionCodeByMachine(machine);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IList GetRejectionCodeByMachineDescription(string machineDescription)
        {
            try
            {
                Machine machine = new CommonMachineRepository().GetMachineByDescription(machineDescription);
                return new ProductionRejectionCodeRepository().GetRejectionCodeByMachine(machine);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RejectionCode GetRejectionCodeByCode(string code)
        {
            try
            {

                return new ProductionRejectionCodeRepository().GetRejectionCodeByCode(code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RejectionCode GetRejectionCodeByDescription(string description)
        {
            try
            {

                return new ProductionRejectionCodeRepository().GetRejectionCodeByDescription(description);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
