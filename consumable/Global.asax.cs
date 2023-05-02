using Toyota.Common.Credential;
using Toyota.Common.Web.Platform;

namespace consumable
{
    public class MvcApplication : WebApplication
    {
        public MvcApplication()
        {
            ApplicationSettings.Instance.Name = "Online LIV";
            ApplicationSettings.Instance.Alias = "LIV";
            ApplicationSettings.Instance.OwnerName = "Toyota Motor Manufacturing Indonesia";
            ApplicationSettings.Instance.OwnerAlias = "TMMIN";
            ApplicationSettings.Instance.OwnerEmail = "tdk@toyota.co.id";
            ApplicationSettings.Instance.Menu.Enabled = true;
            ApplicationSettings.Instance.Runtime.HomeController = "Home"; //uncomment this to setting default page

            ApplicationSettings.Instance.Security.EnableAuthentication = true; //edited by putri
            ApplicationSettings.Instance.Security.EnableSingleSignOn = false; //edited by putri (bypass edit false)
            ApplicationSettings.Instance.Logging.Enabled = false; //edited by putri
            ApplicationSettings.Instance.Security.SimulateAuthenticatedSession = true; //(bypass edit true)
            ApplicationSettings.Instance.Security.IgnoreAuthorization = true;

            //new
            ApplicationSettings.Instance.DefaultDbSc = "SecurityCenter";
            ApplicationSettings.Instance.Menu.SecurityCenter = false;
            ApplicationSettings.Instance.Security.EnableTracking = false;
            ApplicationSettings.Instance.Security.Encrypt = false;


            ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser = new User()
            {
                Username = "0111437",
                Password = "12345",
                FirstName = "Harun",
                LastName = "Barbasy",
                Id = "00111437",
                RegistrationNumber = "09708699"
            };

            //ApplicationSettings.Instance.Security.SimulatedAuthenticatedUser = new User()
            //{
            //    Username = "250208.ASMI",
            //    Password = "12345",
            //    FirstName = "",
            //    LastName = "",
            //    Id = "00111437",
            //    RegistrationNumber = "V250208"
            //};
        }

        protected override void Startup()
        {
            //ProviderRegistry.Instance.Register<IUserProvider>(typeof(UserProvider), DatabaseManager.Instance, "SecurityCenter");
            //new TDK
            ProviderRegistry.Instance.Register<IUserProvider>(typeof(DbUserProvider), DatabaseManager.Instance, "SecurityCenter");
            ProviderRegistry.Instance.Register<ISingleSignOnProvider>(typeof(SingleSignOnProvider), ProviderRegistry.Instance.Get<IUserProvider>(), DatabaseManager.Instance, "SSO");


            //ProviderRegistry.Instance.Register<UILayout>(typeof(BootstrapLayout));
            //BootstrapLayout layout = (BootstrapLayout)ApplicationSettings.Instance.UI.GetLayout();          
        }
    }
}