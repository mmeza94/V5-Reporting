using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Support

{
    [Serializable]
    public class ManagerConfiguration : ConfigurationSection, IXmlSerializable
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
       
        public static ManagerConfiguration Settings
        {
            get
            {
                return (ManagerConfiguration)ConfigurationManager.GetSection("ManagerConfiguration");
            }
            set
            {
                ConfigurationManager.RefreshSection("ManagerConfiguration");
            }
        }

        /// <summary>
        /// 
        /// </summary>
     

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("applicationCommand")]
        public string ApplicationCommand
        {
            get
            {
                return base["applicationCommand"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("adapterDependencies")]
        public string AdapterDependencies
        {
            get
            {
                return base["adapterDependencies"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("commandAdapterAssemblyFullName")]
        public string CommandAdapterAssemblyFullName
        {
            get
            {
                return base["commandAdapterAssemblyFullName"].ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("commandAdapterClassName")]
        public string CommandAdapterClassName
        {
            get
            {
                return base["commandAdapterClassName"].ToString();
            }
        }

        #region IXmlSerializable members
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            DeserializeElement(reader, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        { }
        #endregion

    }
}
