using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FactoriesCreator.CustomerServiceReference;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ServiceModel;
using GetSqlStringCompletedEventArgs = FactoriesCreator.CustomerServiceReference.GetSqlStringCompletedEventArgs;

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
            Resultado = e.Result;
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

        public string Resultado
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

        private string _resultado;

        #endregion

        #region Metodos publicos 
        // Metodos publicos 

        private void TestConnection()
        {
            Proxy.GetSqlStringCompleted += Proxy_GetSqlStringCompleted;
            Proxy.GetSqlStringAsync();
        }

        private void EjecutarQuery()
        {
            Proxy.SelectCompleted += Proxy_SelectCompleted; 
            Proxy.SelectAsync();
        }
       

        public void InicializarComandos() 
        { 
            // Asocia el metodo al comando 
            ComandoEjecutarTemporal = new RelayCommand(TestConnection);
            ComandoQuery = new RelayCommand(EjecutarQuery);
        }

        #endregion

        #region Comandos 
        // Comandos 

        /// <summary>
        /// Comando para ejecutarQuery
        /// </summary>
        public RelayCommand ComandoEjecutarTemporal { get; set; }
        public RelayCommand ComandoQuery { get; set; }

        #endregion
    }
}
