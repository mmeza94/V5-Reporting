//using NHibernate.Criterion;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Tenaris.Fava.Production.Reporting.Model.Business;
//using Tenaris.Fava.Production.Reporting.Model.DTO;
//using Tenaris.Library.Framework;

//namespace Tenaris.Fava.Production.Reporting.Model.Builder
//{
//    public class GeneralPieceProcessor : IBuilder
//    {
//        IList<GeneralPiece> GeneralPieces;
//        IList<GeneralPiece> FilteredGeneralPieces;
//        int order = 0;
//        int heat = 0;
//        int groupItem = 0;
//        string description = "";
//        string extreme = "";


//        public IBuilder GetProductionGeneral(Dictionary<string, object> listParams)
//        {
//            GeneralPieces = ProductionReportingBusiness.GetProductionGeneral(listParams);
//            return this;
//        }

//        public IBuilder OrderByDate()
//        {
//            GeneralPieces.OrderByDescending(item => item.InsDateTime).ToList();
//            return this;
//        }

//        internal void ForEachTest()
//        {


//            GeneralPieces.ForEach(item =>
//            {
//                if (order != item.OrderNumber || heat != item.HeatNumber ||
//                    groupItem != item.GroupItemNumber || description != item.Description
//                    || extreme != item.Extremo)
//                {

//                    order = item.OrderNumber;
//                    heat = item.HeatNumber;
//                    groupItem = item.GroupItemNumber;
//                    description = item.Description;
//                    extreme = item.Extremo;

//                    //if (!GeneralPieces.Exists(x => (x.OrderNumber == order)
//                    //                                      && (x.HeatNumber == heat)
//                    //                                      && (x.GroupItemNumber == groupItem)
//                    //                                      && (x.Description == description)
//                    //                                      && x.Extremo == extreme))

//                    //    GeneralPieces.AddRange(GetSomePieces(order, heat, groupItem, orderedGeneralPieces, description, extreme));
//                }



//            }
//}

//        public IBuilder GetSomePieces()
//        {
//           // GeneralPieces.ForEach(item => test(item));
//            return this;
//        }

//        public bool IsAlreadyAdded(IList<GeneralPiece> generalPieces)
//        {

           

//           //if( order != generalPieces.OrderNumber || generalPieces != generalPieces.HeatNumber ||
//           //             groupItem != generalPieces.GroupItemNumber || description != generalPieces.Description
//           //             || extreme != generalPieces.Extremo



//            return true;
//        }
//    }
//}
