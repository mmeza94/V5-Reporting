using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Enums;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess.Reporitories
{
    public class ManufacturingGroupItemProgramRepository: BaseDataAccess
    {
        public int GetProgrammedPiecesByIdBatch(int idBatch)
        {
            int result = 0;
            try
            {
                var criteria = m_session.CreateCriteria<GroupItemProgram>();
                criteria.Add(Restrictions.Eq("IdBatch", idBatch));
                criteria.SetMaxResults(1);

                var items = criteria.List<GroupItemProgram>();
                var groupItemProgram = (items.Count == 0) ? null : items.First();
                result = (groupItemProgram != null) ? groupItemProgram.ProgrammedPieces : 0;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GetProgrammedPiecesByIdBatchV1(int idBatch)
        {
            int result = 0;
            try
            {
                var criteria = m_session.CreateCriteria<GroupItemProgramV1>();
                criteria.Add(Restrictions.Eq("IdBatch", idBatch));
                criteria.SetMaxResults(1);

                var items = criteria.List<GroupItemProgramV1>();
                var groupItemProgram = (items.Count == 0) ? null : items.First();
                result = (groupItemProgram != null) ? groupItemProgram.ProgrammedPieces : 0;
   

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
