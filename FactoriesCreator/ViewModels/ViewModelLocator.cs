
namespace FactoriesCreator.ViewModels
{
    public class ViewModelLocator
    {
        private FactoryCreatorModel _factoryCreatorModel;

        public FactoryCreatorModel FactoryCreatorModel
        {
            get
            {
                return _factoryCreatorModel ??
                       (_factoryCreatorModel = new FactoryCreatorModel());
            }
        }
    }
}
