using System.ComponentModel;

namespace FactoriesCreator.Clases_Comunes
{
    public class NotificationObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica Cambios efectuados a la propiedad
        /// </summary>
        /// <param name="propertyName">Propiedad que sufrio cambios</param>
        protected void RaisePropertyChanged(string propertyName) 
        { 
            var handler = PropertyChanged; 
            if (handler != null) 
            { 
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
