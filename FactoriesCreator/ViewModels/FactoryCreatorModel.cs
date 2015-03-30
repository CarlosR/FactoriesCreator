using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.Windows;
using FactoriesCreator.CustomerServiceReference;
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
            InicializarComandos();
        }

        void Proxy_GetSqlStringCompleted(object sender, GetSqlStringCompletedEventArgs e)
        {
            PropQueryString = e.Result;
        }

        void Proxy_SelectCompleted(object sender, SelectCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Resultado = new SLDataTable(e.Result); 
            }
            else
            {
                MessageBox.Show("El query es incorecto");
            }
           
        }

        #endregion

        #region Propiedades 
        // Propiedades 

        public string PropQueryString
        {
            get { return _queryString; }
            set
            {
                if (_queryString != value)
                {
                    _queryString = value;
                    RaisePropertyChanged("PropQueryString");
                }
            }
        }

        private string _queryString;

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
       

        public void InicializarComandos() 
        { 
            // Asocia el metodo al comando 
            ComandoEjecutarTemporal = new RelayCommand(TestConnection);
            ComandoQuery = new RelayCommand<string> (EjecutarQuery);
        }

        #endregion

        #region Comandos 
        // Comandos 

        /// <summary>
        /// Comando para ejecutarQuery
        /// </summary>
        public RelayCommand ComandoEjecutarTemporal { get; set; }
        public RelayCommand<string> ComandoQuery { get; set; }

        #endregion
    }
}
