using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;

namespace FactoriesCreator.Services
{
    /// <summary>
    /// Summary description for SqlService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataAdapter da; // envia la consulta a la base de datos y recibe el resultado

        [WebMethod]
        public string GetSqlString()
        {
            return "Data Source=ECRUZJ14-PC\\SRVCRUZ01;Initial Catalog=Siscohdyl;Persist Security Info=True;User ID=sa;Password=Mantenimiento";
        }

        /// <summary>
        /// Ejecuta un SQL con un Select, Insert, Update o Delete
        /// </summary>
        /// <param name="sqlQuery">Es el Query a Ejecutar</param>
        /// <returns>Returna True sí se ejecuta la instrucción SQL</returns>
        public string Acciones(string sqlQuery)
        {
            string resultado = "True";

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
        /// <returns>Retorna una lista que puede o no estar lleno</returns>
        public DataTable Consulta(string sqlQuery)
        {
            var resultado = new DataTable();

            try
            {
                cn = new SqlConnection(GetSqlString());
                cn.Open();
                da = new SqlDataAdapter(sqlQuery, cn);

                da.Fill(resultado); // LLena el Data Table con el resultado

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

            return resultado;
        }

    }
}