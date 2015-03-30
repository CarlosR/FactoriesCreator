using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Reflection;

namespace FactoriesCreator.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerService
    {
        [OperationContract]
        public string GetSqlString()
        {
            return "Data Source=(local); Initial Catalog=MVVMTestDataBase; Persist Security Info=True; Integrated Security = True;";
        }

        // Add more operations here and mark them with [OperationContract]

        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataAdapter da; // envia la consulta a la base de datos y recibe el resultado

        /// <summary>
        /// Ejecuta un SQL con un Insert, Update o Delete
        /// </summary>
        /// <param name="sqlQuery">Es el Query a Ejecutar</param>
        /// <returns>Returna True sí se ejecuta la instrucción SQL</returns>
        [OperationContract]
        public string Acciones(string sqlQuery)
        {
            string resultado = "true";

            try
            {
                cn = new SqlConnection(GetSqlString());
                cmd = new SqlCommand(sqlQuery, cn);

                cn.Open();
                cmd.ExecuteNonQuery();

                return resultado;
            }
            catch (Exception error)
            {
                resultado = error.Message;
                return resultado;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
        }

        /// <summary>
        /// Retorna una Colección de Datos de acuerdo a los resultados de la consulta
        /// </summary>
        /// <param name="sqlQuery">Es el Query a ejecutar</param>
        /// <returns>Retorna un DataTable que puede o no estar lleno</returns>
        [OperationContract]
        public IEnumerable<Dictionary<string, object>> Consulta(string sqlQuery)
        {
            IEnumerable<Dictionary<string, object>> lista;
            var resultado = new DataTable();

            try
            {
                cn = new SqlConnection(GetSqlString());
                cn.Open();
                da = new SqlDataAdapter(sqlQuery, cn);
                da.Fill(resultado); // LLena el Data Table con el resultado

                var columnas = resultado.Columns.Cast<DataColumn>();

                lista = resultado.AsEnumerable().Select(x => columnas.Select(c =>
                                 new { Column = c.ColumnName, Value = x[c] })
                             .ToDictionary(i => i.Column, i => i.Value != DBNull.Value ? i.Value : null));

                return lista;
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();// Cierra la conexion
                cn.Dispose(); // Libera el recurso de memoria 
            }

        }

        [OperationContract]
        public IEnumerable<Dictionary<string, object>> Select(string query)
        {
            IEnumerable<Dictionary<string, object>> resultado;

            string querySelect = query;

            try
            {
                resultado = Consulta(querySelect);
            }
            catch (Exception error)
            {
                return null;
            }

            return resultado;
        }
    }


    //public static class Helper
    //{
    //            // function that set the given object from the given data row
    //    public static void SetItemFromRow<T>(T item, DataRow row)
    //        where T : new()
    //    {
    //        // go through each column
    //        foreach (DataColumn c in row.Table.Columns)
    //        {
    //            // find the property for the column
    //            PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

    //            // if exists, set the value
    //            if (p != null && row[c] != DBNull.Value)
    //            {
    //                p.SetValue(item, row[c], null);
    //            }
    //        }
    //    }

    //    // function that creates an object from the given data row
    //    public static T CreateItemFromRow<T>(DataRow row)
    //        where T : new()
    //    {
    //        // create a new object
    //        T item = new T();

    //        // set the item
    //        SetItemFromRow<T>(item, row);

    //        // return 
    //        return item;
    //    }

    //    // function that creates a list of an object from the given data table
    //    public static List<T> CreateListFromTable<T>(DataTable tbl)
    //        where T : new()
    //    {
    //        // define return list
    //        var lst = new List<T>();

    //        // go through each row
    //        foreach (DataRow r in tbl.Rows)
    //        {
    //            // add to the list
    //            lst.Add(CreateItemFromRow<T>(r));
    //        }

    //        // return the list
    //        return lst;
    //    }
       
    //}
}
