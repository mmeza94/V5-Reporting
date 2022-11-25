using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tenaris.Library.Acquisition.Common;
using Tamsa.OplScada.AcquisitionClient;

namespace SimpleScadaCenexion
{
    /// <summary>
    /// definicion del forma TAGGeneralizada ONLINE ARRAYINT
    /// </summary>
    public class TagDefinicion
    {
        /// <summary>
        /// get a set, impelmentacion del 
        /// </summary>
        public ITag<short[]> TAGPLC { get; set; }

        /// <summary>
        /// get a set, nombre del tag
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// get a set, longritud del arrgle que corresponde al tag
        /// </summary>
        public int longitudTag { get; set; }

        /// <summary>
        /// posiccion en el arraglo donde se va a escribir el bloqueo
        /// </summary>
        public int posicionEscritura { get; set; }

        /// <summary>
        /// get a set, arrar Short de Bloqueo
        /// </summary>
        public short[] valorBloqueo { get; set; }

        /// <summary>
        /// get a set, arrar Short de Desbloqueo
        /// </summary>
        public short[] valorDesBloqueo { get; set; }

        /// <summary>
        /// cantidad de intento para la conexion con el SCADA
        /// </summary>
        public int maxIntConexionScada { get; set; }
    }
}
