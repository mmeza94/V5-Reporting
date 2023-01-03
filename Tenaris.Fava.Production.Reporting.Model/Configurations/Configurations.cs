using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Tenaris.Fava.Production.Reporting.Model.Model
{
    [XmlRoot("configsection")]
    public class Configurations
    {

        #region Singleton
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

        #region Childrens

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

        [XmlElement("GetProductionGeneral")]
        public string GetProductionGeneral { get; set; }

        [XmlElement("StrategyWork")]
        public string StrategyWork { get; set; }

        public string PathStrategy { get => "Tenaris.Fava.Production.Reporting.ViewModel"; }

        #endregion

        #region Constructors

        private Configurations()
        {

        }

        #endregion

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
            GetProductionGeneral = configurations.GetProductionGeneral;
<<<<<<< HEAD

            StrategyWork = configurations.StrategyWork == null ?null: $"Tenaris.Fava.Production.Reporting.ViewModel.Stategy.{configurations.StrategyWork}";
=======
            StrategyWork = configurations.StrategyWork == null ? null : $"Tenaris.Fava.Production.Reporting.ViewModel.Stategy.{configurations.StrategyWork}";
>>>>>>> 8f78c3a2d292ba2bcde027cff6ed7c0ddb52aad3
        }

        public void GetConfigutation()
        {
            string urlConfiguration = ConfigurationManager.AppSettings["URLConfigurations"].ToString().ToUpper();
            string config = ConfigurationManager.AppSettings["Machine"].ToString().ToUpper();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(urlConfiguration);

            XElement machinConfig = XElement.Parse(xmldoc.InnerXml)
                .Elements("configsection")
                .ToList()
                .FirstOrDefault(x => x.Attribute("name")
                .Value.Equals(config));

            StringReader reader = new StringReader(machinConfig.ToString());
            XmlSerializer xml = new XmlSerializer(typeof(Configurations));
            Configurations configurations = (Configurations)xml.Deserialize(reader);
            Configurations.Instance.SetConfigMachine(configurations);

        }

    }

}
