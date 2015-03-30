using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            Resultado = e.Result;
            Temporal = new SLDataTable(e.Result);
            var shit = Resultado.Select(x => new { Value = x.Keys }).ToList();

            //Temporal = new ObservableCollection<string>(shit);


            //foreach (var blabla in Resultado)
            //{
            //    foreach (var llave in blabla.Keys)
            //    {

            //        foreach (var valores in blabla.Values)
            //        {
            //            Temporal.Add(new { Columna = llave, Lsad = llave });

            //        }

            //    }

                
            //}
            
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

        public SLDataTable Temporal
        {
            get { return _temporal; }
            set
            {
                if (_temporal != value)
                {
                    _temporal = value;
                    RaisePropertyChanged("Temporal");
                }
            }
        }

        private SLDataTable _temporal;

        public ObservableCollection<Dictionary<string, object>> Resultado
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

        private ObservableCollection<Dictionary<string, object>> _resultado;

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
