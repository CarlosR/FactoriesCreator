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
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace FactoriesCreator.ViewModels
{
    public class FactoryCreatorModel : BaseINPC
    {
        #region Miembros privados
        // Miembros privados 
        #endregion

        #region Constructor 
        // Constructor
        #endregion

        #region Propiedades 
        // Propiedades 

        string QueryString
        {
            get { return _queryString; }
            set
            {
                if (_queryString != value)
                {
                    _queryString = value;
                    RaisePropertyChanged("QueryString");
                }
            }
        }

        private string _queryString;

        #endregion

        #region Metodos publicos 

        // Metodos publicos 

        #endregion

        #region Comandos 

        // Comandos 

        #endregion
    }
}
