﻿using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.View.Dialog;
using Tenaris.Fava.Production.Reporting.View.Properties;
using Tenaris.Fava.Production.Reporting.ViewModel.Reflection;

namespace Tenaris.Fava.Production.Reporting.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {

        public App()
        {
            InitializeCultures();
        }

        public void InitializeCultures()
        {
            if (!string.IsNullOrEmpty(Settings.Default.Culture))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.Culture);
            }
            if (!string.IsNullOrEmpty(Settings.Default.UICulture))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.UICulture);
            }
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        public void ApplicationStartup(Object sender, StartupEventArgs e)
        {
            Configurations.Instance.GetConfigutation();
            try
            {
                ReflectionStrategy.LoaderReflection();
                if (Configurations.Instance.Machine == "Pintado")
                {
                    StartupUri = new Uri("/Tenaris.Fava.Production.Reporting.View;component/PaintingReportView.xaml", UriKind.Relative);
                    return;
                }
                StartupUri = new Uri("/Tenaris.Fava.Production.Reporting.View;component/ProductionReport.xaml", UriKind.Relative);
            }
            catch (Exception)
            {

                MessageBox.Show("Estrategia no cargada");
                Environment.Exit(0);

            }

        }

    }
}
