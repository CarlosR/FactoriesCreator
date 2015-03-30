using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Browser;
using FactoriesCreator.CustomerServiceReference;
using FactoriesCreator.Models;
using GalaSoft.MvvmLight.Command;
using Telerik.Data;

namespace FactoriesCreator.ViewModels
{
    public class FactoryCreatorModel : BaseINPC
    {
        #region Miembros privados
        // Miembros privados 

        private CustomerServiceClient Proxy;

        #endregion

        #region Constructor 
        // Constructor

        public FactoryCreatorModel()
        {
            Proxy = new CustomerServiceClient();
            ElementosDeClase = new List<FactoryModel>();
            PropNombreClase = "";
            InicializarComandos();
        }

        void Proxy_GetSqlStringCompleted(object sender, GetSqlStringCompletedEventArgs e)
        {
        }

        void Proxy_SelectCompleted(object sender, SelectCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Resultado = new SLDataTable(e.Result);
                LlenarPropiedades();
            }
            else
            {
                MessageBox.Show("El query es incorecto");
            }
           
        }

        private void LlenarPropiedades()
        {
            ElementosDeClase = new List<FactoryModel>();
            foreach (var result in Resultado)
            {
                PropertyInfo[] propiedadesConsulta = result.GetType().GetProperties();

                foreach (var propConsulta in propiedadesConsulta)
                {
                    var propertyType = propConsulta.PropertyType;

                    if (propertyType.IsGenericType &&
                        propertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }

                    ElementosDeClase.Add(new FactoryModel()
                    {
                        PropertyName = propConsulta.Name,
                        PropertyType = propertyType.Name
                    });

                }
                return;
            }
        }

        #endregion

        #region Propiedades 
        // Propiedades 

        public string PropNombreClase
        {
            get { return _propNombreClase; }
            set
            {
                if (_propNombreClase != value)
                {
                    _propNombreClase = value;
                    RaisePropertyChanged("PropNombreClase");
                }
            }
        }

        private string _propNombreClase;

        public SLDataTable Resultado 
        {
            get { return _resultado; }
            set
            {
                if (_resultado != value)
                {
                    _resultado = value;
                    RaisePropertyChanged("Resultado");
                }
            }
        }

        private SLDataTable _resultado;

        public List<FactoryModel> ElementosDeClase
        {
            get { return _elementosDeClase; }
            set
            {
                if (_elementosDeClase != value)
                {
                    _elementosDeClase = value;
                    RaisePropertyChanged("ElementosDeClase");
                }
            }
        }

        private List<FactoryModel> _elementosDeClase;

        public string PropCodigoGenerado
        {
            get { return _propCodigoGenerado; }
            set
            {
                if (_propCodigoGenerado != value)
                {
                    _propCodigoGenerado = value;
                    RaisePropertyChanged("PropCodigoGenerado");
                }
            }
        }

        private string _propCodigoGenerado;


        public string PropClaseGenerada
        {
            get { return _propClaseGenerada; }
            set
            {
                if (_propClaseGenerada != value)
                {
                    _propClaseGenerada = value;
                    RaisePropertyChanged("PropClaseGenerada");
                }
            }
        }

        private string _propClaseGenerada;

        #endregion

        #region Metodos publicos 
        // Metodos publicos 

        private void TestConnection()
        {
            Proxy.GetSqlStringCompleted += Proxy_GetSqlStringCompleted;
            Proxy.GetSqlStringAsync();
        }

        private void EjecutarQuery(string query)
        {
            if (query.Length > 5)
            {
                string verificaSelect = query.Substring(0, 6).ToLower();

                if (verificaSelect == "select")
                {
                    Proxy.SelectCompleted += Proxy_SelectCompleted;
                    Proxy.SelectAsync(query);
                }
                else
                {
                    MessageBox.Show("Solo puede realizar querys de select", "Error de Consulta", MessageBoxButton.OK);
                }
            }
            
        }

        private void FormatearPropiedad(PropertyInfo property, object result)
        {

            if (property.PropertyType.Name == "String")
            {
                PropCodigoGenerado += "\"" + property.GetValue(result, null) + "\",";
            }


            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {

                if (property.PropertyType.GetGenericArguments()[0].Name == "Decimal")
                {
                    PropCodigoGenerado += property.GetValue(result, null) + "M,";
                }

                if (property.PropertyType.GetGenericArguments()[0].Name == "DateTime")
                {
                    PropCodigoGenerado += "Convert.ToDateTime(\"" + property.GetValue(result, null) + "\"),";
                }

                if (property.PropertyType.GetGenericArguments()[0].Name == "Int32")
                {
                    PropCodigoGenerado += property.GetValue(result, null);
                }
            }

        }

        private void GenerarCodigo()
        {
            if (ElementosDeClase.Count > 0)
            {
                GenerarClase();
            }

            if (ElementosDeClase.Count == 1)
            {
                if (PropNombreClase != String.Empty)
                {
                    PropCodigoGenerado = "var "+PropNombreClase.ToLower()+" = new " + PropNombreClase + ">\n{";
                }
                else
                {
                    PropCodigoGenerado = "var miClase = new MiClase\n{";
                }
                
                foreach (var result in Resultado)
                {
                    PropertyInfo[] propertyInfos = result.GetType().GetProperties();
                    foreach (var property in propertyInfos)
                    {
                        PropCodigoGenerado += "\n   " + property.Name + "= ";
                        FormatearPropiedad(property,result);

                    }      
                    
                }
                PropCodigoGenerado += "\n};";
            }

            if (ElementosDeClase.Count > 1)
            {
                if (PropNombreClase != String.Empty)
                {
                    PropCodigoGenerado = "var miLista = new List<" + PropNombreClase + ">\n{";
                }
                else
                {
                    PropCodigoGenerado = "var miLista = new List<MiClase>\n{";
                }
                
                foreach (var result in Resultado)
                {
                    if (PropNombreClase != String.Empty)
                    {
                        PropCodigoGenerado += "\n   new " + PropNombreClase + "\n   {";
                    }
                    else
                    {
                        PropCodigoGenerado += "\n   new MiClase()\n   {";
                    }
                    

                    PropertyInfo[] propertyInfos = result.GetType().GetProperties();
                    foreach (var property in propertyInfos)
                    {

                        PropCodigoGenerado += "\n       " + property.Name + "= ";
                        FormatearPropiedad(property, result);
                    }

                    PropCodigoGenerado += "\n   },";

                }
                PropCodigoGenerado = PropCodigoGenerado.Remove(PropCodigoGenerado.Length - 1);
                PropCodigoGenerado += "\n};";

            }
        }

        private void GenerarClase()
        {
            if (PropNombreClase != String.Empty)
            {
                PropClaseGenerada = "public class " + PropNombreClase + "\n{";
            }
            else
            {
                PropClaseGenerada = "public class MiClase\n{";
            }
            
            foreach (var elemento in ElementosDeClase)
            {
                PropClaseGenerada += "\n   public "+elemento.PropertyType+" "+elemento.PropertyName 
                    + " { get; set; }";
            }

            PropClaseGenerada += "\n}";
        }

        private void CopiarAClipboard()
        {
            
            if (PropCodigoGenerado != null && PropClaseGenerada != null)
            {
                string codigoCompleto = PropClaseGenerada + "\n \n" + PropCodigoGenerado;

                Clipboard.SetText(codigoCompleto);

            }
        }
       

        public void InicializarComandos() 
        { 
            // Asocia el metodo al comando 
            ComandoEjecutarTemporal = new RelayCommand(TestConnection);
            ComandoQuery = new RelayCommand<string> (EjecutarQuery);
            ComandoGenerarCodigo = new RelayCommand(GenerarCodigo);
            ComandoCopiarAClipboard = new RelayCommand(CopiarAClipboard);
        }

        #endregion

        #region Comandos 
        // Comandos 

        /// <summary>
        /// Comando para ejecutarQuery
        /// </summary>
        public RelayCommand ComandoEjecutarTemporal { get; set; }
        public RelayCommand<string> ComandoQuery { get; set; }
        public RelayCommand ComandoGenerarCodigo { get; set; }
        public RelayCommand ComandoCopiarAClipboard { get; set; }

        #endregion
    }
}
