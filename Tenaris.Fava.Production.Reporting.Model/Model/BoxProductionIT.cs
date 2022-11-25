using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class BoxProductionIT
    {
        public BoxProductionIT()
        {

        }

        public BoxProductionIT(int orderNumberChildrenPAR, int orderNumberMoterPAR, int totalPiecesPAR, int reportPiecesPAR, string statusPAR)
        {
            //Id = idPAR;
            OrderNumberChildren = orderNumberChildrenPAR;
            OrderNumberMoter = orderNumberMoterPAR;
            //MachineId =MachineIdPAR;
            //OperationId = OperationIdPAR;
            //this.Type = TypePAR;
            //ProcessSequence = ProcessSequencePAR;
            //MaxPieces = MaxPiecesPAR;
            TotalPieces = totalPiecesPAR;
            ReportPieces = reportPiecesPAR;
            Status = statusPAR;
        }

        public string IdBox { get; set; }
        public int OrderNumberMoter { get; set; }
        public int OrderNumberChildren { get; set; }
        //public string MachineId { get; set; }
        //public string OperationId { get; set; }
        //public string Type { get; set; }
        //public int ProcessSequence { get; set; }
        //public int MaxPieces { get; set; }
        public int TotalPieces { get; set; }
        public int ReportPieces { get; set; }
        public string idOpcionIT { get; set; }
        public string idOperacionIt { get; set; }
        public string Status { get; set; }
    }
}
