using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Tenaris.Fava.Production.Reporting.Model.Model
{
    [XmlRoot("configsection")]
    public class Configurations
    {



        #region "Singleton"

        // Solo permitimos una instacia unica del modelo
        // el acceso debe realizarse a traves de la propiedad Instance

        static Configurations classInstance = null;
        static object classLock = new object();

        public static Configurations Instance
        {
            get
            {
                lock (classLock)
                {
                    if (classInstance == null)
                    {
                        classInstance = new Configurations();
                    }
                }
                return classInstance;
            }
        }


        #endregion

        [XmlElement("MachineFiltre")]
        public string MachineFiltre { get; set; }

        [XmlElement("Machine")]
        public string Machine { get; set; }

        [XmlElement("Secuencia")]
        public string Secuencia { get; set; }

        [XmlElement("Operacion")]
        public string Operacion { get; set; }
        [XmlElement("Opcion")]
        public string Opcion { get; set; }

        [XmlElement("MaquinaInicialZona")]
        public string MaquinaInicialZona { get; set; }

        [XmlElement("Extremo")]
        public string Extremo { get; set; }

        [XmlElement("MaquinaInicialZonaFiltre")]
        public string MaquinaInicialZonaFiltre { get; set; }

        [XmlElement("MachineToReport")]
        public string MachineToReport { get; set; }

        [XmlElement("ExtremoFiltre")]
        public string ExtremoFiltre { get; set; }

        [XmlElement("isPintado")]
        public bool isPintado { get; set; }

        [XmlElement("ConnectionString")]

        public string ConnectionString { get; set; }

        [XmlElement("VersionApplication")]
        public string VersionApplication { get; set; }

        [XmlElement("N1CounterTag")]
        public string N1CounterTag { get; set; }

        [XmlElement("N2CounterTag")]
        public string N2CounterTag { get; set; }

        [XmlElement("FlagITNOReportBox")]
        public string FlagITNOReportBox { get; set; }

        [XmlElement("Workstation")]
        public string Workstation { get; set; }


        public Configurations()
        {

        }

        public Configurations(string machineFiltre, string machine, string secuencia, string operacion, string opcion, string maquinaInicialZona, string extremo, string maquinaInicialZonaFiltre, string machineToReport, string extremoFiltre, bool isPintado, string connectionString, string versionApplication, string n1CounterTag, string n2CounterTag, string flagITNOReportBox, string workstation)
        {
            MachineFiltre = machineFiltre;
            Machine = machine;
            Secuencia = secuencia;
            Operacion = operacion;
            Opcion = opcion;
            MaquinaInicialZona = maquinaInicialZona;
            Extremo = extremo;
            MaquinaInicialZonaFiltre = maquinaInicialZonaFiltre;
            MachineToReport = machineToReport;
            ExtremoFiltre = extremoFiltre;
            this.isPintado = isPintado;
            ConnectionString = connectionString;
            VersionApplication = versionApplication;
            N1CounterTag = n1CounterTag;
            N2CounterTag = n2CounterTag;
            FlagITNOReportBox = flagITNOReportBox;
            Workstation = workstation;
        }

        public void SetConfigMachine(Configurations configurations)
        {
            MachineFiltre = configurations.MachineFiltre;
            Machine = configurations.Machine;
            Secuencia = configurations.Secuencia;
            Operacion = configurations.Operacion;
            Opcion = configurations.Opcion;
            MaquinaInicialZona = configurations.MaquinaInicialZona;
            Extremo = configurations.Extremo;
            MaquinaInicialZonaFiltre = configurations.MaquinaInicialZonaFiltre;
            MachineToReport = configurations.MachineToReport;
            ExtremoFiltre = configurations.ExtremoFiltre;
            isPintado = configurations.isPintado;
            ConnectionString = configurations.ConnectionString;
            VersionApplication = configurations.VersionApplication;
            N1CounterTag = configurations.N1CounterTag;
            N2CounterTag = configurations.N2CounterTag;
            FlagITNOReportBox = configurations.FlagITNOReportBox;
            Workstation = configurations.Workstation;
        }

        public Configurations GetConfigMachine()
        {
            Configurations configurations = new Configurations();

            configurations.MachineFiltre = MachineFiltre;
            configurations.Machine = Machine;
            configurations.Secuencia = Secuencia;
            configurations.Operacion = Operacion;
            configurations.Opcion = Opcion;
            configurations.MaquinaInicialZona = MaquinaInicialZona;
            configurations.Extremo = Extremo;
            configurations.MaquinaInicialZonaFiltre = MaquinaInicialZonaFiltre;
            configurations.MachineToReport = MachineToReport;
            configurations.ExtremoFiltre = ExtremoFiltre;
            configurations.isPintado = isPintado;
            configurations.ConnectionString = ConnectionString;
            configurations.VersionApplication = VersionApplication;
            configurations.N1CounterTag = N1CounterTag;
            configurations.N2CounterTag = N2CounterTag;
            configurations.FlagITNOReportBox = FlagITNOReportBox;
            configurations.Workstation = Workstation;
            return configurations;
        }



        public void GetConfigutation()
        {
            string urlConfiguration = ConfigurationManager.AppSettings["URLConfigurations"].ToString().ToUpper();
            string config = ConfigurationManager.AppSettings["Machine"].ToString().ToUpper();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(urlConfiguration);
            //xmldoc.Load("../../../Tenaris.Fava.Production.Reporting.Model/Configurations/Configurations.Xml");

            XElement element = XElement.Parse(xmldoc.InnerXml);
            XElement machinConfig = element.Elements("configsection").ToList().FirstOrDefault(x => x.Attribute("name").Value.Equals(config));

            StringReader reader = new StringReader(machinConfig.ToString());
            XmlSerializer xml = new XmlSerializer(typeof(Configurations));
            Configurations configurations = (Configurations)xml.Deserialize(reader);

            Configurations.Instance.SetConfigMachine(configurations);
        }




    }


}
