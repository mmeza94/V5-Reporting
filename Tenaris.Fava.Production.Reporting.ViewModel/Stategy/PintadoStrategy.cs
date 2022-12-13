using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.Business;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;
using Tenaris.Fava.Production.Reporting.ViewModel.Stategy.RProcess;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public class PintadoStrategy : GeneralMachine, IActions
    {
        #region Properties
        public Dictionary<string, object> Filters { get; set; }
        public Dictionary<string, object> OutPuts { get; set; }
        public GeneralMachine GeneralMachine => this;
        public IReportingProcess reportingProcess { get; set; }
        #endregion

        #region Constructor
        public PintadoStrategy()
        {
            reportingProcess = new RPGeneral(this);
            Filters = Filter;
            OutPuts = OutPut;
        }
        #endregion

        #region Methods

        public IActions Search()
        {
            ObservableCollection<BoxReport> dt = ProductionReportingBusiness.GetBoxesForPainting(Filters);
            ObservableCollection<BoxLoad> dtBaseDeDatos = ProductionReportingBusiness.GetLoadPainting(Filters);


            AddValues("CajasParaPintado", dt).AddValues("CargadasParaPintado", dtBaseDeDatos);

            return this;
        }

        public ObservableCollection<ReportProductionHistory> dgReporteProduccion_SelectionChanged(GeneralPiece SelectedBundle)
        {
            throw new NotImplementedException();
        }

        public bool Report(GeneralPiece currentDGRow)
        {
            throw new NotImplementedException();
        }

        
        #endregion
    }
}
