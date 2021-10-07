using SAP.Middleware.Connector;

namespace consumable.Logic
{
    public class SAPConfigChange : IDestinationConfiguration
    {
        private string SAPUser;
        private string SAPPassword;

        public SAPConfigChange(string _SAPUser, string _SAPPassword)
        {
            this.SAPUser = _SAPUser;
            this.SAPPassword = _SAPPassword;
        }

        #region IDestinationConfiguration Members

        public bool ChangeEventsSupported()
        {
            return false;
        }

        //public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {
            if (destinationName == Common.AppSetting.SAP_NAME)
            {
                RfcConfigParameters _RfcConfigParameters = new RfcConfigParameters();
                _RfcConfigParameters.Add(RfcConfigParameters.User, this.SAPUser);
                _RfcConfigParameters.Add(RfcConfigParameters.Password, this.SAPPassword);
                _RfcConfigParameters.Add(RfcConfigParameters.Client, Common.AppSetting.SAP_CLIENT);
                _RfcConfigParameters.Add(RfcConfigParameters.Language, Common.AppSetting.SAP_LANG);
                _RfcConfigParameters.Add(RfcConfigParameters.AppServerHost, Common.AppSetting.SAP_ASHOST);
                _RfcConfigParameters.Add(RfcConfigParameters.SystemNumber, Common.AppSetting.SAP_SYSN);
                _RfcConfigParameters.Add(RfcConfigParameters.PeakConnectionsLimit, Common.AppSetting.SAP_MAX_POOL_SIZE);
                _RfcConfigParameters.Add(RfcConfigParameters.IdleTimeout, Common.AppSetting.SAP_IDLE_TIMEOUT);
                return _RfcConfigParameters;
            }
            else
                return null;
        }

        RfcDestinationManager.ConfigurationChangeHandler changeHandler;
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged
        {
            add
            {
                changeHandler = value;
            }
            remove
            {
                //do nothing
            }
        }
        public void removeDestination()
        {
            changeHandler("SAPNCO", new RfcConfigurationEventArgs(RfcConfigParameters.EventType.DELETED));
        }
        #endregion
    }
}
