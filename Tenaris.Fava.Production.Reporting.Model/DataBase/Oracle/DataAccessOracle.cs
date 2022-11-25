using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using Tenaris.Fava.Production.Reporting.Model.DTO;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Tenaris.Fava.Production.Reporting.Model.Data_Access
{
    public class DataAccessOracle
    {

        public static string GetBoxesByWOMotherSP = "vbo_admin.tps_wo_parent_child_pkg.get_boxes_by_wo_mother_sp";
        private static String GetConnectionString()
        {
            String sConn;

            sConn = ConfigurationManager.AppSettings["TheOracleConnectionString"].ToString();

            return sConn;
        }

        /// <summary>
        /// CONSULTA EL LISTADO DE CAJAS DISPONIBLES POR ESTACION CONFIGURADA Y SU OP MADRE 
        /// </summary>
        /// <param name="numeroOPMother"></param>
        /// <returns></returns>
        /// 

        // // COMENTADO PORQUE NO SE USA
        //public static List<BoxProductionIT> GetBoxesByWOMotherStacion(int numeroOPMother)
        //{
        //    OracleDataAdapter daOracleCommand;
        //    OracleCommand cmdSQL = null;
        //    List<BoxProductionIT> ListadoBoxProducction = new List<BoxProductionIT>();
        //    BoxProductionIT BoxProducction;
        //    //OracleDataReader data;
        //    //DataTable tabla;

        //    try
        //    {
        //        daOracleCommand = new OracleDataAdapter();
        //        daOracleCommand.SelectCommand = new OracleCommand();
        //        daOracleCommand.SelectCommand.Connection = new OracleConnection(GetConnectionString());

        //        cmdSQL = daOracleCommand.SelectCommand;
        //        cmdSQL.CommandText = CommandOracle.GetBoxesByWOMother;
        //        cmdSQL.CommandType = System.Data.CommandType.StoredProcedure;

        //        OracleParameter TheParams = new OracleParameter("p_wo_mother", OracleType.Number);
        //        TheParams.Value = numeroOPMother;
        //        TheParams.Direction = ParameterDirection.Input;
        //        cmdSQL.Parameters.Add(TheParams);

        //        TheParams = new OracleParameter("p_process_operation", OracleType.NVarChar);
        //        TheParams.Value = ConfigurationManager.AppSettings["Operation_10"].ToString();
        //        TheParams.Direction = ParameterDirection.Input;
        //        cmdSQL.Parameters.Add(TheParams);

        //        TheParams = new OracleParameter("p_process_option", OracleType.NVarChar);
        //        TheParams.Value = ConfigurationManager.AppSettings["Option_10"].ToString();
        //        TheParams.Direction = ParameterDirection.Input;
        //        cmdSQL.Parameters.Add(TheParams);

        //        TheParams = new OracleParameter("p_rsout", OracleType.Cursor);
        //        TheParams.Direction = ParameterDirection.Output;
        //        cmdSQL.Parameters.Add(TheParams);


        //        cmdSQL.Connection.Open();
        //        cmdSQL.ExecuteNonQuery();

        //        IDataReader readerTable = (OracleDataReader)cmdSQL.Parameters[3].Value;
        //        if (readerTable.FieldCount > 0)
        //        {

        //            while (readerTable.Read())
        //            {
        //                BoxProducction = new BoxProductionIT();

        //                BoxProducction.OrderNumberMoter = Convert.ToInt32(readerTable["WO_MOTHER"].ToString());
        //                BoxProducction.OrderNumberChildren = Convert.ToInt32(readerTable["WO_DAUGHTER"].ToString());
        //                BoxProducction.IdBox = readerTable["BOX_ID"].ToString();
        //                BoxProducction.TotalPieces = Convert.ToInt32(readerTable["TOTAL_PIECES"].ToString());
        //                BoxProducction.ReportPieces = Convert.ToInt32(readerTable["REPORTED_PIECES"].ToString());
        //                BoxProducction.idOperacionIt = readerTable["PROCESS_OPERATION"].ToString();
        //                BoxProducction.idOpcionIT = readerTable["PROCESS_OPTION"].ToString();
        //                BoxProducction.Status = readerTable["STATUS_BOX"].ToString();

        //                ListadoBoxProducction.Add(BoxProducction);
        //            }
        //        }
        //        cmdSQL.Connection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Trace.Exception(ex, true);
        //        //string TheError = ex.Message.ToString();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (cmdSQL.Connection.State.ToString() == "Open")
        //        {
        //            cmdSQL.Connection.Close();
        //        }
        //    }
        //    return ListadoBoxProducction;
        //}

        /// <summary>
        /// CONSUALTA TODAS LAS CAJAS DISPONIBLES PARA UNA DETERMINADA OP MADRE
        /// </summary>
        /// <param name="numeroOPMother"></param>
        /// <returns></returns>
        public static List<BoxProductionIT> GetBoxesByWOMother(int numeroOPMother)
        {
            OracleCommand val = null;
            List<BoxProductionIT> list = new List<BoxProductionIT>();
            try
            {
                OracleDataAdapter val2 = new OracleDataAdapter();
                val2.SelectCommand = new OracleCommand();
                val2.SelectCommand.Connection = new OracleConnection(GetConnectionString());
                val = val2.SelectCommand;
                val.CommandText = GetBoxesByWOMotherSP;
                val.CommandType = CommandType.StoredProcedure;
                OracleParameter val3 = new OracleParameter("p_wo_mother", OracleDbType.Int16);
                //OracleParameter val4 = new OracleParameter("d", ) ;
                val3.Value = numeroOPMother;
                val3.Direction = ParameterDirection.Input;
                val.Parameters.Add(val3);
                val3 = new OracleParameter("p_process_operation", OracleDbType.NVarchar2);
                val3.Value = "";
                val3.Direction = ParameterDirection.Input;
                val.Parameters.Add(val3);
                val3 = new OracleParameter("p_process_option", OracleDbType.NVarchar2);
                val3.Value = "";
                val3.Direction = ParameterDirection.Input;
                val.Parameters.Add(val3);
                val3 = new OracleParameter("p_rsout", OracleDbType.RefCursor);
                val3.Direction = ParameterDirection.Output;
                val.Parameters.Add(val3);
                val.Connection.Open();
                val.ExecuteNonQuery();
                OracleDataReader dataReader = val.ExecuteReader();
                if (dataReader.FieldCount > 0)
                {
                    while (dataReader.Read())
                    {
                        BoxProductionIT boxProducctionIT = new BoxProductionIT();
                        boxProducctionIT.OrderNumberMoter = Convert.ToInt32(dataReader["WO_MOTHER"].ToString());
                        boxProducctionIT.OrderNumberChildren = Convert.ToInt32(dataReader["WO_DAUGHTER"].ToString());
                        boxProducctionIT.IdBox = dataReader["BOX_ID"].ToString();
                        boxProducctionIT.TotalPieces = Convert.ToInt32(dataReader["TOTAL_PIECES"].ToString());
                        boxProducctionIT.ReportPieces = Convert.ToInt32(dataReader["REPORTED_PIECES"].ToString());
                        boxProducctionIT.idOperacionIt = dataReader["PROCESS_OPERATION"].ToString();
                        boxProducctionIT.idOpcionIT = dataReader["PROCESS_OPTION"].ToString();
                        boxProducctionIT.Status = dataReader["STATUS_BOX"].ToString();
                        list.Add(boxProducctionIT);
                    }
                }
                System.Console.ReadLine();
                ((DbConnection)(object)val.Connection).Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (((DbConnection)(object)val.Connection).State.ToString() == "Open")
                {
                    ((DbConnection)(object)val.Connection).Close();
                }
            }

            return list;
        }


        public static class CommandOracle
        {
            public static string GetBoxesByWOMother = ConfigurationManager.AppSettings["GetBoxesByWOMother"].ToString();
            public static string TheOracleConnectionString = ConfigurationManager.AppSettings["TheOracleConnectionString"].ToString();
        }


    }
}
