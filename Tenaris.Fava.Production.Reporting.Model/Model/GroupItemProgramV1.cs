using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHMA = NHibernate.Mapping.Attributes;


namespace Tenaris.Fava.Production.Reporting.Model.DTO
{
    [NHMA.Class(Table = "GroupItemProgram", Schema = "Manufacturing")]
    public class GroupItemProgramV1
    {

        [NHMA.Id(0, Name = "Id", TypeType = typeof(Int32), Column = "idGroupItemProgram", Access = "property")]
        [NHMA.Generator(1, Class = "native")]
        public virtual int Id { get; set; }

        [NHMA.Property(Name = "IdGroupItemHistory", Access = "property", Column = "idGroupItemHistory")]
        public virtual int IdGroupItemHistory { get; set; }

        [NHMA.Property(Name = "IdHeatProgram", Access = "property", Column = "idHeatProgram")]
        public virtual int IdHeatProgram { get; set; }

        [NHMA.Property(Name = "IdBatch", Access = "property", Column = "idBatch")]
        public virtual int IdBatch { get; set; }

        [NHMA.Property(Name = "IdStatus", Access = "property", Column = "idStatus")]
        public virtual int IdStatus { get; set; }

        [NHMA.Property(Name = "Sequence", Access = "property", Column = "Sequence")]
        public virtual int Sequence { get; set; }

        [NHMA.Property(Name = "ElaborationState", Access = "property", Column = "ElaborationState")]
        public virtual string ElaborationState { get; set; }

        [NHMA.Property(Name = "ProgrammedPieces", Access = "property", Column = "ProgrammedPieces")]
        public virtual int ProgrammedPieces { get; set; }

        [NHMA.Property(Name = "LoadedPieces", Access = "property", Column = "LoadedPieces")]
        public virtual int LoadedPieces { get; set; }
        [NHMA.Property(Name = "IsGroupItemSent", Access = "property", Column = "IsGroupItemSent")]
        public virtual int IsGroupItemSent { get; set; }
    }
}
