﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using  System.Linq;

namespace FactoriesCreator.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerService
    {
        [OperationContract]
        public string GetSqlString()
        {
            return "Data Source=ECRUZJ14-PC\\SRVCRUZ01;Initial Catalog=Siscohdyl;Persist Security Info=True;User ID=sa;Password=Mantenimiento";
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
        public List<string> Consulta(string sqlQuery)
        {
            List<string> lista;
            var resultado = new DataTable();

            try
            {
                cn = new SqlConnection(GetSqlString());
                cn.Open();
                da = new SqlDataAdapter(sqlQuery, cn);
                da.Fill(resultado); // LLena el Data Table con el resultado

                lista = resultado.AsEnumerable().Select(x => x.Field<string>("Descripcion")).ToList();

                //List<DataRow> list = resultado.AsEnumerable().ToList();
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

            return lista;
        }

        [OperationContract]
        public List<string> Select()
        {
            List<string> resultado = new List<string>();

            string querySelect = "select descripcion from Inv_DeptoUbicacion where idDepartamento = '5'";

            try
            {
                resultado = Consulta(querySelect);
            }

            catch (Exception Error)
            {
                throw Error;
            }

            return resultado;
        }
    }
}
