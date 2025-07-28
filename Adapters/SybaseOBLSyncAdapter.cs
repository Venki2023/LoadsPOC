using LOADS.Interfaces;
using LOADS.Models;
using Meijer.Application.Framework.Core;

namespace LOADS.Adapters
{
    public class SybaseOBLSyncAdapter : WebApiAdapterBase,ISybaseOBLSyncAdapter
    {
        #region Properties
        private readonly string? _BaseAddress;
        private readonly string? _ApiManagerToken;
        private ILogger<SybaseOBLSyncAdapter> _loggerAdapter { get; set; }
        #endregion

        #region ConstantValues
        public const string GetMileageURI = "Loads/GetMileageQty";
        #endregion

        #region Constructor
        /// <summary>
        /// SybaseOBLSyncAdapter
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="configService"></param>
        /// <param name="loggerAdapter"></param>
        public SybaseOBLSyncAdapter(IHttpClientFactory clientFactory, IConfiguration configService, ILogger<SybaseOBLSyncAdapter> loggerAdapter) : base(clientFactory)
        {
            _loggerAdapter = loggerAdapter;
            _BaseAddress = configService.GetValue<string>("LogisticsCommon:OBLSyncAPIBaseAddress");
            _ApiManagerToken = configService.GetValue<string>("LogisticsCommon:APIManagerKey");
            this.TimeoutSeconds = 300;
        }
        protected override string? BaseAddress => _BaseAddress;
        protected override string? ApiManagerToken => _ApiManagerToken;
        #endregion

        /// <summary>
        /// GetMileageQty
        /// </summary>
        /// <param name="OrigUnitId"></param>
        /// <param name="DestUnitId"></param>
        /// <returns></returns>
        public async Task<List<MileageModel>> GetMileageQty(string OrigUnitId, string DestUnitId)
        {
            List<MileageModel> mileage = new List<MileageModel>();
            var URI = GetMileageURI + "/" + OrigUnitId + "/" + DestUnitId;
            if(!Program.IsUnitTestRunning)
            {
                mileage = await this.GetRequestAsync<List<MileageModel>>(URI);
            }
            return mileage;
        }
    }
}
