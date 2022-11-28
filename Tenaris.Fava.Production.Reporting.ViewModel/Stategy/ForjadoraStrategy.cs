using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using Tenaris.Fava.Production.Reporting.Model.Interfaces;
using Tenaris.Fava.Production.Reporting.Model.Model;
using Tenaris.Fava.Production.Reporting.ViewModel.Interfaces;

namespace Tenaris.Fava.Production.Reporting.ViewModel.Stategy
{
    public class ForjadoraStrategy : GeneralMachine, IActions
    {

        public GeneralMachine GeneralMachine { get => this; }
        public IReportingProcess reportingProcess { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Report(GeneralPiece currentDGRow)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<GeneralPiece> Search(int Orden, int Colada, int Atado)
        {

            throw new NotImplementedException();
        }

        public  ReportProductionDto GetCurrentGroupItemToReport(GeneralPiece currentDGRow)
        {
            ReportProductionDto reportDto = currentDGRow.BuildReportProductionDTO();
            reportDto.Operacion = GetOperation(currentDGRow, reportDto);
            reportDto.Secuencia = GetSequenceForDifferentExtreme(currentDGRow, reportDto);
            return reportDto;
        }


        public string GetOperation(GeneralPiece GeneralPiece, ReportProductionDto reportProductDto)
        {
            string operation = Configurations.Instance.Operacion;
            if (GeneralPiece.Extremo.ToString() != String.Empty)
            {
                //Hard Code por reporte de las dos operaciones en mismo colgante 
                if (reportProductDto.DescripcionMaquina.ToUpper().Contains("FORJADORA"))
                    operation = "Forjado " + GeneralPiece.Extremo;
                else if (reportProductDto.DescripcionMaquina.Contains("Roscadora"))
                    operation = "Mecanizado " + GeneralPiece.Extremo;

            }
            return operation;
        }


        public int GetSequenceForDifferentExtreme(GeneralPiece GeneralPiece, ReportProductionDto reportProductDto)
        {
            if (GeneralPiece.Extremo.ToString() != string.Empty)
            {
                if (GeneralPiece.Extremo.EndsWith("2"))
                    return reportProductDto.Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia) + 1;
                else
                    return reportProductDto.Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia);
            }
            else
                return reportProductDto.Secuencia = Convert.ToInt32(Configurations.Instance.Secuencia);
        }

      
    }
}
