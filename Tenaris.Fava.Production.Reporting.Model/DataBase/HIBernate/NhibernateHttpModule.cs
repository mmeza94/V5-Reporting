using NHibernate;
using NHMA = NHibernate.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Model;

namespace Tenaris.Fava.Production.Reporting.Model.NhAccess
{
    public class NhibernateHttpModule
    {
        // this is only used if not running in HttpModule mode
        protected static ISessionFactory m_factory;

        // this is only used if not running in HttpModule mode
        private static ISession m_session;

        //private NHibernateManager mgr;

        private static readonly string KEY_NHIBERNATE_FACTORY = "NHibernateSessionFactoryRP";
        private static readonly string KEY_NHIBERNATE_SESSION = "NHibernateSessionRP";
        //CARGA EL CONNECTION DEL APP.CONFIG

    
        
        private static readonly string KEY_CONNECTION_STRING = Configurations.Instance.ConnectionString;


        public void Init(HttpApplication context)
        {
            // log4net.Config.XmlConfigurator.Configure();
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        public void Dispose()
        {
            if (m_session != null)
            {
                m_session.Close();
                m_session.Dispose();
            }

            if (m_factory != null)
            {
                m_factory.Close();
            }
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            context.Items[KEY_NHIBERNATE_SESSION] = CreateSession();
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            ISession session = context.Items[KEY_NHIBERNATE_SESSION] as ISession;
            if (session != null)
            {
                try
                {
                    session.Flush();
                    session.Close();
                }
                catch { }
            }

            context.Items[KEY_NHIBERNATE_SESSION] = null;
        }

        private void mimethod()
        {



            // cfg.AddAssembly(typeof(Address).AssemblyQualifiedName);
        }
        #region Método Respaldo


        /// <summary>
        /// Crea la "fábrica" de sesiones. Serializa la información del archivo de configuración.
        /// Sección de Nhibernate
        /// </summary>
        /// <returns>ISessionFactory</returns>
        //protected static ISessionFactory CreateSessionFactory1()
        //{
        //    try
        //    {
        //        NHibernate.Cfg.Configuration config;
        //        ISessionFactory factory;
        //        config = new NHibernate.Cfg.Configuration();
        //        NHMA.HbmSerializer serializer = new NHMA.HbmSerializer();
        //        serializer.Validate = true; // optional

        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            serializer.Serialize(
        //                stream, typeof(Equipo).Assembly
        //                 //System.Reflection.Assembly.GetExecutingAssembly()
        //                );
        //            stream.Position = 0;
        //            config.AddInputStream(stream);
        //            stream.Close();
        //        }
        //        //config.SetProperty("hibernate.connection.connection_string", DecryptConnString(config.Properties["hibernate.connection.connection_string"].ToString()));
        //        if (config == null)
        //        {
        //            throw new InvalidOperationException("NHibernate configuration is null.");
        //        }
        //        //Nuvo Código

        //        //ConfigurationManager.ConnectionStrings[KEY_CONNECTION_STRING].ConnectionString;
        //        config.Properties["connection.connection_string"] =
        //            ConfigurationManager.ConnectionStrings[KEY_CONNECTION_STRING].ConnectionString;
        //        factory = config.BuildSessionFactory();



        //        if (factory == null)
        //        {
        //            throw new InvalidOperationException("Call to Configuration.BuildSessionFactory() returned null.");
        //        }
        //        else
        //        {
        //            return factory;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// Crea la "fábrica" de sesiones. Serializa la información del archivo de configuración.
        /// Sección de Nhibernate
        /// </summary>
        /// <returns>ISessionFactory</returns>
        protected static ISessionFactory CreateSessionFactory()
        {
            try
            {
                //NHibernate.Cfg.Configuration config;
                ISessionFactory factory;
                NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();

                //Configuration config = new Configuration();
                config.Properties.Add(NHibernate.Cfg.Environment.ConnectionProvider,
                typeof(NHibernate.Connection.DriverConnectionProvider)
                .AssemblyQualifiedName);
                config.Properties.Add(NHibernate.Cfg.Environment.Dialect,
                //typeof(NHibernate.Dialect.Oracle9iDialect)
                typeof(NHibernate.Dialect.MsSql2008Dialect)
                .AssemblyQualifiedName);
                config.Properties.Add(NHibernate.Cfg.Environment.ConnectionDriver,
                //typeof(NHibernate.Driver.OracleClientDriver)
                typeof(NHibernate.Driver.SqlClientDriver)
                .AssemblyQualifiedName);

                config.Properties.Add(NHibernate.Cfg.Environment.
                      ProxyFactoryFactoryClass, typeof
                      (NHibernate.ByteCode.LinFu.ProxyFactoryFactory)
                      .AssemblyQualifiedName);
                //config.Properties.Add(NHibernate.Cfg.Environment.ShowSql,"true");

                //Nuvo Código

                config.Properties.Add(NHibernate.Cfg.Environment.ConnectionString,
                     KEY_CONNECTION_STRING);

                NHMA.HbmSerializer serializer = new NHMA.HbmSerializer();
                serializer.Validate = true; // optional

                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(
                        stream, typeof(ReportProductionHistory).Assembly
                        //System.Reflection.Assembly.GetExecutingAssembly()
                        );
                    stream.Position = 0;
                    config.AddInputStream(stream);
                    stream.Close();
                }


                factory = config.BuildSessionFactory();



                if (factory == null)
                {
                    throw new InvalidOperationException("Call to Configuration.BuildSessionFactory() returned null.");
                }
                else
                {
                    return factory;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Factory actual
        /// </summary>
        public static ISessionFactory CurrentFactory
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    // running without an HttpContext (non-web mode)
                    // the nhibernate session is a singleton in the app domain
                    if (m_factory != null)
                    {
                        return m_factory;
                    }
                    else
                    {
                        //NHibernateManager mgr = new NHibernateManager();
                        m_factory = CreateSessionFactory();
                        //m_factory = mgr.Session.SessionFactory;
                        return m_factory;
                    }
                }
                else
                {
                    // running inside of an HttpContext (web mode)
                    // the nhibernate session is a singleton to the http request
                    HttpContext currentContext = HttpContext.Current;

                    ISessionFactory factory = currentContext.Application[KEY_NHIBERNATE_FACTORY] as ISessionFactory;

                    if (factory == null)
                    {
                        // NHibernateManager mgr = new NHibernateManager();
                        factory = CreateSessionFactory();
                        //factory = mgr.Session.SessionFactory;
                        currentContext.Application[KEY_NHIBERNATE_FACTORY] = factory;
                    }

                    return factory;
                }
            }
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        /// <summary>
        /// Crea la sesión a través del CurrentFactory
        /// </summary>
        /// <returns>ISession</returns>
        public static ISession CreateSession()
        {
            ISessionFactory factory;
            ISession session;

            factory = NhibernateHttpModule.CurrentFactory;

            if (factory == null)
            {
                throw new InvalidOperationException("Call to Configuration.BuildSessionFactory() returned null.");
            }

            session = factory.OpenSession();

            if (session == null)
            {
                throw new InvalidOperationException("Call to factory.OpenSession() returned null.");
            }

            return session;
        }

        /// <summary>
        /// Sesión actual
        /// </summary>
        public static ISession CurrentSession
        {
            get
            {

                if (HttpContext.Current == null)
                {
                    // running without an HttpContext (non-web mode)
                    // the nhibernate session is a singleton in the app domain
                    if (m_session != null)
                    {
                        return m_session;
                    }
                    else
                    {
                        // NHibernateManager mgr = new NHibernateManager();
                        m_session = CreateSession();
                        // m_session = mgr.Session;

                        return m_session;
                    }
                }
                else
                {
                    // running inside of an HttpContext (web mode)
                    // the nhibernate session is a singleton to the http request
                    HttpContext currentContext = HttpContext.Current;

                    ISession session = currentContext.Items[KEY_NHIBERNATE_SESSION] as ISession;

                    if (session == null)
                    {
                        //NHibernateManager mgr = new NHibernateManager();
                        session = CreateSession();
                        //session = mgr.Session;
                        currentContext.Items[KEY_NHIBERNATE_SESSION] = session;
                    }

                    return session;
                }
            }
        }
    }
}
