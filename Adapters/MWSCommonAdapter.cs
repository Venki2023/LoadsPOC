using LOADS.Models;
using Meijer.Application.Framework.Core;
using LOADS.Interfaces;
using DevExpress.Office.Utils;



namespace LOADS.Adapters
{
    public class MWSCommonAdapter : WebApiAdapterBase, IMWSCommonAdapter
    {
        #region EndPoints
        private const string FACILITY_GET_ALL_FACILITIES_END_POINT = "{0}/Facilities";
        private const string GET_UNIT_DETAILS = "{0}/Units";
        #endregion


        #region Private Members
        private readonly string _BaseAddress;
        private readonly string _ApiManagerToken;
        private ILogger<MWSCommonAdapter> _logger { get; set; }
        #endregion

        public MWSCommonAdapter(IHttpClientFactory clientFactory, IConfiguration configService, ILogger<MWSCommonAdapter> logger) : base(clientFactory)
        {
            this._BaseAddress = configService.GetValue<string>("LogisticsCommon:MWSCommonAPIBaseAddressV2");
            this._ApiManagerToken = configService.GetValue<string>("LogisticsCommon:APIManagerKey");
            _logger = logger;

        }
        protected override string BaseAddress => _BaseAddress;
        protected override string ApiManagerToken => _ApiManagerToken;

        public async Task<List<CampusUnitModel>> GetAllFacilities()
        {
            List<CampusUnitModel> facilities = new List<CampusUnitModel>();

            int dfId = ConstantValues.DFIdForFacilities;
            string URI = string.Format(FACILITY_GET_ALL_FACILITIES_END_POINT, dfId);
            if (!Program.IsUnitTestRunning)
            {
                facilities = await GetRequestAsync<List<CampusUnitModel>>(URI);
            }

            return facilities;
        }


        public async Task<List<dynamic>> GetUnitDetails()
        {
                      
            List<dynamic> AllUnits = new List<dynamic>();
            try
            {
                var v_AllUnits = await this.GetRequestAsync<List<dynamic>>(GET_UNIT_DETAILS);
                foreach (var units in v_AllUnits)
                {
                    var unitCloseDate = units.UnitCloseDate;
                    if (unitCloseDate == "NULL")
                    {

                        if (units.UnitId.ToString().Length < 3)
                        {
                            var v_UnitId = Convert.ToInt32(units.UnitId);
                          
                            AllUnits.Add(units);
                        }
                        else
                        {
                            AllUnits.Add(units);
                        }
                    }
                    else
                    {
                        if(unitCloseDate == null)
                        {

                        }
                        else
                        {
                            var closingDate = Convert.ToDateTime(unitCloseDate);
                            if (closingDate > DateTime.UtcNow)
                            {
                                if (units.UnitId.ToString().Length < 3)
                                {
                                    var v_UnitId = Convert.ToInt32(units.UnitId);
                                    AllUnits.Add(units);
                                }
                                else
                                {
                                    AllUnits.Add(units);
                                }
                            }
                        }
                           
                    }

                }
                return AllUnits;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return AllUnits;


        }
    }
}
