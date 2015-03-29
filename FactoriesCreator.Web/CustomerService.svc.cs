using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace FactoriesCreator.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomerService
    {
        [OperationContract]
        public string GetSqlString()
        {
            //return "Mantenimiento";

            return "Data Source=ECRUZJ14-PC\\SRVCRUZ01;Initial Catalog=Siscohdyl;Persist Security Info=True;User ID=sa;Password=Mantenimiento";

        }

        // Add more operations here and mark them with [OperationContract]
    }
}
