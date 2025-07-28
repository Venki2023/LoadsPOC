using LOADS.Interfaces;
using LOADS.Models;
using Meijer.Application.Framework.Core;

namespace LOADS.Adapters
{
    public class OBLConfigurationAdapter : WebApiAdapterBase, IOBLConfigurationAdapter
    {
        #region Properties
        protected override string BaseAddress => _baseAddress;
        private readonly string _baseAddress;
        private ILogger<OBLConfigurationAdapter> _loggerAdapter { get; set; }
        #endregion

        #region ConstantValues
        public const string SearchOBLConfigurationURI = "OBLConfiguration/Search";
        public const string SearchScheduleConfigurationURI = "ScheduleConfiguration/Search";
        

        #endregion 

        #region Constructor
        public OBLConfigurationAdapter(IHttpClientFactory clientFactory, IConfiguration configService, ILogger<OBLConfigurationAdapter> loggerAdapter) : base(clientFactory)
        {
            _loggerAdapter = loggerAdapter;
            _baseAddress = configService.GetValue<string>("LogisticsCommon:OBLConfigAPIBaseAddress") ?? string.Empty;
            ApiManagerToken = configService.GetValue<string>("LogisticsCommon:APIManagerKey");
        }
        #endregion

        #region Methods
        /// <summary>
        /// GetOBLAppConfiguration
        /// </summary>
        /// <param>FilterCriteriaOBLConfig</param>
        /// <returns>List<OBLAppConfigModel></returns>
        public async Task<List<OBLAppConfigModel>> GetOBLAppConfiguration(FilterCriteriaOBLConfig FilterCriteria)
        {
            List<OBLAppConfigModel> lstOBLConfig = new List<OBLAppConfigModel>();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    lstOBLConfig = await PostRequestAsync<FilterCriteriaOBLConfig, List<OBLAppConfigModel>>(SearchOBLConfigurationURI, FilterCriteria);
                }
                lstOBLConfig = lstOBLConfig.Where(x => x.IsActive == Convert.ToBoolean("true")).ToList();
            }
            catch (Exception Ex)
            {
                _loggerAdapter.LogError(Ex, "Exception thrown in method OBLConfigurationAdapter :: GetOBLAppConfiguration " + Ex.Message);
            }
            return lstOBLConfig;
        }

        public async Task<List<OBLAppConfigModel>> GetAllOBLAppConfigurations()
        {
            List<OBLAppConfigModel> lstOBLConfig = new List<OBLAppConfigModel>();
            FilterCriteriaOBLConfig FilterCriteria = new FilterCriteriaOBLConfig();
            FilterCriteria.PartitionKey = string.Empty;
            FilterCriteria.Rowkey = string.Empty;
            FilterCriteria.AppId = string.Empty;
            lstOBLConfig = await GetOBLAppConfiguration(FilterCriteria);
            return lstOBLConfig;    
        }

        public async Task<List<ScheduleConfigModel>> GetScheduleConfigurations(FilterCriteriaOBLConfig FilterCriteria)
        {
            List<ScheduleConfigModel> lstOBLConfig = new List<ScheduleConfigModel>();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    lstOBLConfig = await PostRequestAsync<FilterCriteriaOBLConfig, List<ScheduleConfigModel>>(SearchScheduleConfigurationURI, FilterCriteria);
                }
                lstOBLConfig = lstOBLConfig.Where(x => x.IsActive == Convert.ToBoolean("true")).ToList();
            }
            catch (Exception Ex)
            {
                _loggerAdapter.LogError(Ex, "Exception thrown in method OBLConfigurationAdapter :: GetOBLAppConfiguration " + Ex.Message);
            }
            return lstOBLConfig;
        }

        #endregion
    }
}
