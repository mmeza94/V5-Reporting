using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    public class ProductionBox
    {
        public ProductionBox()
        {

        }

        public ProductionBox(string id, int parentOrderNumber, int orderNumber, string machineId, string operationId, string type, int processSequence, int maxPieces, int loadedPieces, int missingPieces)
        {
            Id = id;
            ParentOrderNumber = parentOrderNumber;
            OrderNumber = orderNumber;
            MachineId = machineId;
            OperationId = operationId;
            Type = type;
            ProcessSequence = processSequence;
            MaxPieces = maxPieces;
            LoadedPieces = loadedPieces;
            MissingPieces = missingPieces;
        }

        public ProductionBox(int idN2, string id, int parentOrderNumber, int orderNumber, string machineId, string operationId, string type, int processSequence, int maxPieces, int loadedPieces, int missingPieces)
            : this(id, parentOrderNumber, orderNumber, machineId, operationId, type, processSequence, maxPieces, loadedPieces, missingPieces)
        {
            IdN2 = idN2;
        }

        public int IdN2 { get; set; }

        public string Id { get; set; }

        public int ParentOrderNumber { get; set; }

        public int OrderNumber { get; set; }

        public string MachineId { get; set; }

        public string OperationId { get; set; }

        public string Type { get; set; }

        public int ProcessSequence { get; set; }

        public int MaxPieces { get; set; }

        public int LoadedPieces { get; set; }

        public int MissingPieces { get; set; }
    }
}
