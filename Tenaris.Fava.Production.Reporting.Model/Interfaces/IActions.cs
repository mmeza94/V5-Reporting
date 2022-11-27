using System.Collections.ObjectModel;
using Tenaris.Fava.Production.Reporting.Model.DTO;

namespace Tenaris.Fava.Production.Reporting.Model.Interfaces
{
    public interface IActions
    {

        ObservableCollection<GeneralPiece> Search(int Orden, int Colada, int Atado);


    }
}
