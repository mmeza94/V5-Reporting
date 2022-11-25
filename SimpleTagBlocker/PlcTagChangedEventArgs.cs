using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScadaCenexion
{
    /// <summary>
    /// clase custom EverntArgs
    /// </summary>
    public class PlcTagChangedEventArgs : EventArgs
    {
        /// <summary>
        /// constructor de PlcTagChangedEventArgs 
        /// </summary>
        /// <param name="parametroValor"></param>
        /// <param name="parametroNombreTag"></param>
        public PlcTagChangedEventArgs(string parametroValor, string parametroNombreTag)
        {
            valor = parametroValor;
            NombreTag = parametroNombreTag;
        }

        /// <summary>
        /// constructor de PlcTagChangedEventArgs 
        /// </summary>
        /// <param name="parametroValor"></param>
        /// <param name="parametroNombreTag"></param>
        /// <param name="parametroIdtracking"></param>
        public PlcTagChangedEventArgs(string parametroValor, string parametroNombreTag, string parametroIdtracking)
        {
            valor = parametroValor;
            NombreTag = parametroNombreTag;
            idTracking = parametroIdtracking;
        }

        /// <summary>
        /// get a set, el valor completo leido del tag
        /// </summary>
        public string valor { get; set; }

        /// <summary>
        /// get a set, para el nombre del tag
        /// </summary>
        public string NombreTag { set; get; }

        /// <summary>
        /// get a set, para el identificador del tracking, tomado del SCADA
        /// </summary>
        public string idTracking { set; get; }
    }
}
