using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;

using Tenaris.Library.Acquisition.Common;
using Tamsa.OplScada.AcquisitionClient;
using Tenaris.Library.System.Factory;
using Tenaris.Library.Log;

namespace SimpleScadaCenexion
{

    public delegate void CahngerTrakingEventHandler(object send, PlcTagChangedEventArgs eventArgas);

    public class ScadaAdquisidor
    {
        private static IAcquisitionSession scada;
        private bool isScadaConnected;
        private TagDefinicion Tag;

        public event CahngerTrakingEventHandler ChangerTracking;
        //public event EventHandler<PlcTagChangedEventArgs> ChangedTracking;

        /// <summary>
        /// constructor, toma la co nfiguracion del TAGDEFINICION del appconfig
        /// </summary>
        public ScadaAdquisidor()
        {
            Trace.Debug("ScadaAdquisidor.ScadaAdquisidor() sin parametros");
            try
            {
                //escribirLog("Cargando Configuracion Tag Bloqueo");
                Tag = new TagDefinicion();
                Tag.TagName = ConfigurationManager.AppSettings["TagName"].ToString();
                Tag.longitudTag = Convert.ToInt32(ConfigurationManager.AppSettings["LongitudTag"].ToString());
                Tag.posicionEscritura = Convert.ToInt32(ConfigurationManager.AppSettings["PosisionEscritura"].ToString());
                short bloqueoValorTEM = (short)Convert.ToUInt16(ConfigurationManager.AppSettings["ValorBloqueo"].ToString());
                short desbloqueoValorTEM = (short)Convert.ToUInt16(ConfigurationManager.AppSettings["ValorDesBloqueo"].ToString());
                Tag.maxIntConexionScada = Convert.ToInt32(ConfigurationManager.AppSettings["MaxIntConexionScada"].ToString());

                Tag.valorBloqueo = new short[Tag.longitudTag];
                Tag.valorBloqueo[Tag.posicionEscritura] = bloqueoValorTEM;

                Tag.valorDesBloqueo = new short[Tag.longitudTag];
                Tag.valorDesBloqueo[Tag.posicionEscritura] = desbloqueoValorTEM;
            }
            catch (Exception ex)
            {
                Trace.Error("=> error en constructor ScadaAdquisidor() sin parametros, mensaje de error:{0}",ex.Message);
                Trace.Exception(ex);
            }
        }

        /// <summary>
        /// constructor, toma la configuracion del TagDefinicion del parameto ingresado
        /// </summary>
        /// <param name="paramTagDefinicions"></param>
        public ScadaAdquisidor(TagDefinicion paramTagDefinicions)
        {
            Trace.Debug("ScadaAdquisidor.ScadaAdquisidor()");
            try
            {
                //escribirLog("Cargando Configuracion Tag Bloqueo");
                Tag = paramTagDefinicions;
            }
            catch (Exception ex)
            {
                Trace.Error("=> error en constructor ScadaAdquisidor(), mensaje de error:{0}", ex.Message);
                Trace.Exception(ex);
            }
        }

        /// <summary>
        /// inicia la configuracion del OPL SACADA
        /// </summary>
        public void inicializar()
        {
            Trace.Debug("ScadaAdquisidor.inicializar()");
            try
            {
                Trace.Message("Iniciando la la conexion al OPLSCADA");
                //escribirLog("Iniciando la la conexion al OPLSCADA");
                IFactory<IAcquisitionSession> factory = FactoryProvider.Instance.CreateFactory<IAcquisitionSession>("AcquisitionConfiguration");
                scada = factory.Create();
                scada.StateChanged += new StateChangedHandler(scada_StateChanged);
                scada.Open();
            }
            catch (Exception ex)
            {
                Trace.Error("Error inicializar(), mensaje de error:{0}",ex.Message);
                Trace.Exception(ex);
            }
        }

        /// <summary>
        /// Agraga el TAG y la suscripcion al evento vaulerChanger
        /// </summary>
        public void activar()
        {
            Trace.Debug("ScadaAdquisidor.activar()");
            Trace.Message("  ==>ACTIVACION & SUSCRIPCION TAG:{0}", Tag.TagName);
            //escribirLog("Abriendo conexion a OPLSCADA");
            try
            {
                int countint = 0;
                scada.Open();
                while (!isScadaConnected && countint < Tag.maxIntConexionScada)
                {
                    Thread.Sleep(200);
                    countint++;
                }
                Tag.TAGPLC = scada.Tags.Add<short[]>(Tag.TagName, null);
                Tag.TAGPLC.Subscribe();
                Tag.TAGPLC.ValueChanged += new ValueChangedEventHandler<short[]>(valueChanged);
                if (Tag.TAGPLC.IsSubscribed)
                {
                    leerTag();
                }
                Trace.Message(string.Format("  ==>TAG:{0} IS SUBSCRIBED:{1}", Tag.TagName, Tag.TAGPLC.IsSubscribed));
                //escribirLog(string.Format("TAG:{0} Suscrito:{1}", Tag.TagName, Tag.TAGPLC.IsSubscribed));
            }
            catch (Exception ex)
            {
                Trace.Error("error activar(), mensaje de error:{0}",ex.Message);
                Trace.Exception(ex);
            }
        }

        /// <summary>
        /// agrega el tag solamente, sin suscripcion al evento vaulercahnger
        /// </summary>
        public void activarOnliBlock()
        {
            Trace.Debug("ScadaAdquisidor.activarOnliBlock()");
            Trace.Message("  ==>ACTIVACION & SUSCRIPCION TAG:{0}", Tag.TagName);
            //escribirLog("Abriendo conexion a OPLSCADA");
            try
            {
                int countint = 0;
                scada.Open();
                while (!isScadaConnected && countint < Tag.maxIntConexionScada)
                {
                    Thread.Sleep(200);
                    countint++;
                }
                Tag.TAGPLC = scada.Tags.Add<short[]>(Tag.TagName, null);
                //Tag.TAGPLC.Subscribe();
                leerTag();
                Trace.Message("  ==>TAG:{0}", Tag.TagName);
                //escribirLog(string.Format("Listo para escribir en TAG:{0}", Tag.TagName));
            }
            catch (Exception ex)
            {
                Trace.Error("error en activarOnliBlock() mensasje:{0}", ex.Message);
                Trace.Exception(ex);
            }
        }

        /// <summary>
        /// cansela la suscripcion del tag, y cierra la conexion con el OPLSACADA, uasa cuando se usa el metodo activar()
        /// </summary>
        public void desActivar()
        {
            Trace.Debug("ScadaAdquisidor.desActivar()");
            try
            {
                Trace.Message("  ==>CLOSE, UNSUBSCRIBED TAG:{0}", Tag.TagName);
                //escribirLog(string.Format("Cerrando suscripcion TAG {0}", Tag.TagName));
                Tag.TAGPLC.ValueChanged -= valueChanged;
                Tag.TAGPLC.Unsubscribe();

            }
            catch (Exception ex)
            {
                Trace.Error("error en metodo desActivar(), mensaje de error:{0}", ex.Message);
                Trace.Exception(ex);
            }
            desActivarOnliBlock();
        }

        /// <summary>
        /// Cierra solamente conexion el OPLSACADA, usar si se ha iniciado con activarOnliBlock()
        /// </summary>
        public void desActivarOnliBlock()
        {
            Trace.Debug("ScadaAdquisidor.desActivarOnliBlock()");
            try
            {
                Trace.Message("  ==>CLOSE CONEXION OPLSCADA");
                //escribirLog("Cerrando conexiones con OPLSCADA");
                scada.StateChanged -= scada_StateChanged;
                scada.Close();
            }
            catch (Exception ex)
            {
                Trace.Error("error en metodo desActivar(), mensaje de error:{0}", ex.Message);
                Trace.Exception(ex);
            }
        }
        
        /// <summary>
        /// manejador del enven vaulerChanger del OPLSCADA, valida y castea al evento TrakingChanged 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void valueChanged(ITag<short[]> sender, ValueChangedEventArgs<short[]> args)
        {
            Trace.Debug("ScadaAdquisidor.valueChanged()");
            try
            {
                string valorLeido = String.Join(",", Array.ConvertAll<short, string>(((short[])(args.Value.Value)), Convert.ToString));
                //los idTrackin estan dividiados en la posiciones 1 y 2 de los handshakeds, si tenemos doferente de sero alguina de esas posiciones consideramos evento
                short[] arrgValues = (short[])args.Value.Value;
                if (args != null && (arrgValues[1] != 0 || arrgValues[2] != 0))
                {
                    string idtracking = arrgValues[1].ToString() + arrgValues[2].ToString();
                    OncahngerTracking(new PlcTagChangedEventArgs(valorLeido, Tag.TagName, idtracking));
                    //ChangedTracking(sender, new PlcTagChangedEventArgs(valorLeido, Tag.TagName, idtracking));
                }
                //Trace.Message("Cambio TAG:{0}, Estado:{1}, Tiempo:{2} Valor:{3}", Tag.TagName, args.Value.Quality, args.Value.Timestamp.ToString(), valorLeido);
            }
            catch (Exception ex)
            {
                Trace.Error("error en metodo desActivar(), mensaje de error:{0}", ex.Message);
                Trace.Exception(ex);
            }
        }

        /// <summary>
        /// determina el estado de la conexion al OPLSCADA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void scada_StateChanged(IAcquisitionSession sender, StateChangedEventArgs args)
        {
            Trace.Debug("ScadaAdquisidor.scada_StateChanged()");
            switch (args.State)
            {
                case SessionState.Closed:
                    //escribirLog("Conexion a SCADA cerrada.");
                    Trace.Message("ScadaAdquisidor ## Conexion a SCADA cerrada.");
                    isScadaConnected = false;
                    break;
                case SessionState.Closing:
                    break;
                case SessionState.Open:
                    //escribirLog("Conexion a SCADA abierta.");
                    Trace.Message("ScadaAdquisidor ## Conexion a SCADA abierta.");
                    isScadaConnected = true;
                    break;
                case SessionState.Opening:
                    break;
                case SessionState.ServerNotRunning:
                    //escribirLog("No se encuentra la instancia de SCADA");
                    Trace.Message("ScadaAdquisidor ## No se encuentra la instancia de SCADA");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ler el valor del tag definido.
        /// </summary>
        private void leerTag()
        {
            Trace.Debug("ScadaAdquisidor.leerTag()");
            try
            {
                TagValue<short[]> valorLeido = Tag.TAGPLC.Read();
                string valueRead = String.Join(",", Array.ConvertAll<short, string>(((short[])(valorLeido.Value)), Convert.ToString));
                //escribirLog(string.Format("Se ha recuparado el VALO:{0} del TAG:{1}", valueRead, Tag.TagName));
                Trace.Message("ScadaAdquisidor ## READER IN TAG:{1} VALUE:{0}", valueRead, Tag.TagName);
            }
            catch (Exception ex)
            {
                Trace.Error("=> error en el metodo leerTag() mensaje de error {0}", ex.Message);
                Trace.Exception(ex);
            }
        }

        /// <summary>
        /// escribimos en el tag
        /// </summary>
        /// <param name="value"></param>
        private void escribirTag(short[] value)
        {
            Trace.Debug("ScadaAdquisidor.escribirTag()");
            //escribirLog(string.Format("Se va a escribir EL siguiente Valor:{0} al TAG:{1}", String.Join(",", Array.ConvertAll<short, string>(((short[])(value)), Convert.ToString)), Tag.TagName));
            Trace.Message("ScadaAdquisidor ## WRITE IN TAG:{1} VALUE:{0}", String.Join(",", Array.ConvertAll<short, string>(((short[])(value)), Convert.ToString)), Tag.TagName);
            if (Tag != null)
            {
                try
                {
                    Tag.TAGPLC.Write(value);
                }
                catch (Exception ex)
                {
                    Trace.Error("=> error en el metodo escribirTag() mensaje de error {0}", ex.Message);
                    Trace.Exception(ex);
                }
            }
        }

        /// <summary>
        /// Boquea el Tag con el valor configurado de bloqueo
        /// </summary>
        public void bloquear()
        {
            Trace.Debug("ScadaAdquisidor.bloquear()");
            escribirTag(Tag.valorBloqueo);
        }

        /// <summary>
        /// Bloquea el Tag con el valior configurado de desbloqueo
        /// </summary>
        public void desbloquear()
        {
            Trace.Debug("ScadaAdquisidor.desbloquear()");
            escribirTag(Tag.valorDesBloqueo);
        }

        /// <summary>
        /// resete el tag a un formato de handShaked, es para el uso de pruebas
        /// </summary>
        public void resetearHandshaked()
        {
            Trace.Debug("ScadaAdquisidor.resetearHandshaked()");
            short[] sort = {0,0,0,0,0,0,0,0,0,0};
            escribirTag(sort);
        }

        /// <summary>
        /// escribe en handShaked, para hacer pruebas
        /// </summary>
        /// <param name="value"></param>
        public void escribirHandshaked(string value)
        {
            Trace.Debug("ScadaAdquisidor.escribirHandshaked()");
            char[] arrayString = value.ToArray();
            string lowIdTracking = string.Format("ScadaAdquisidor  ## {0}{1}", arrayString[2], arrayString[3]);
            string higIdTracking = string.Format("ScadaAdquisidor  ## {0}{1}", arrayString[0], arrayString[1]);
            short[] sort = { 1, Convert.ToInt16(higIdTracking), Convert.ToInt16(lowIdTracking), 0, 0, 0, 0, 0, 0, 0 };
            escribirTag(sort);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventargs"></param>
        protected virtual void OncahngerTracking(PlcTagChangedEventArgs eventargs)
        {
            Trace.Debug("ScadaAdquisidor.OncahngerTracking()");
            if (ChangerTracking != null)
                ChangerTracking(this, eventargs);
        }
    }
}
