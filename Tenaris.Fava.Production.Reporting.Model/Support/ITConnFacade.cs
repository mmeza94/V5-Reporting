using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenaris.Fava.Production.Reporting.Model.Adapter;

namespace Tenaris.Fava.Production.Reporting.Model.Support
{
    public class ITConnFacade
    {
        private DataTable GetAvailableItems(string op, int sequence)
        {
            ITServiceAdapter itConn = new ITServiceAdapter();
            string resultMessage = "";

            var numeroOperacionsiguiente = sequence + 1;
            var option = ConfigurationManager.AppSettings.Get("Option_" + (numeroOperacionsiguiente).ToString());
            var operation = ConfigurationManager.AppSettings.Get("Operation_" + (numeroOperacionsiguiente).ToString());
            var availableItems = itConn.GetAvailableStock(op, operation, option, "", "", ref resultMessage);
            return availableItems;
        }

        public DataTable GetAvailableStock(string op, int sequence)
        {
            ITServiceAdapter itConn = new ITServiceAdapter();
            string resultMessage = "";

            var option = ConfigurationManager.AppSettings.Get("Option_" + (sequence).ToString());
            var operation = ConfigurationManager.AppSettings.Get("Operation_" + (sequence).ToString());
            var availableItems = itConn.GetAvailableStock(op, operation, option, "", "", ref resultMessage);
            return availableItems;
        }

        public bool IsGroupItemAvailableForNextOperation(int groupItemNumber, int heatNumber, int order, int sequence)
        {
            bool result = false;
            try
            {
                DataTable availableItems = GetAvailableItems(order.ToString(), sequence);
                var items = from c in availableItems.AsEnumerable()
                            select new
                            {
                                GroupItemNumber = c.Field<string>("IdUdt"),
                                HeatNumber = c.Field<string>("Colada"),
                                OrderNumber = c.Field<string>("Order")
                            };
                var ocurrences = items.Where(c => c.GroupItemNumber == groupItemNumber.ToString()
                    && c.HeatNumber == heatNumber.ToString()).Count();
                result = ocurrences > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
