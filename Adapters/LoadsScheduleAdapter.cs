using LOADS.Interfaces;
using LOADS.Models;
using LOADS.Pages;
using Meijer.Application.Framework.Core;

namespace LOADS.Adapters
{
    public class LoadsScheduleAdapter : WebApiAdapterBase,ILoadsScheduleAdapter
    {

        #region  EndPoints 
        private const string SEARCH_SCHEDULE_SUMMARY = "LoadScheduleSummary/Search";
        private const string CREATE_SCHEDULE_SUMMARY = "LoadsScheduleSummary";
        private const string UPDATE_SCHEDULE_SUMMARY = "LoadsScheduleSummary";
        private const string DELETE_SCHEDULE_SUMMARY = "LoadsScheduleSummary";
        
        private const string SEARCH_SCHEDULE_DETAILS = "LoadScheduleDetails/Search";
        private const string UPSERT_SCHEDULE_DETAILS = "LoadScheduleDetails/Upsert";
        private const string CREATE_SCHEDULE_DETAILS = "LoadsScheduleDetails";
        private const string DELETE_SCHEDULE_DETAILS = "LoadScheduleDetails";
        
        private const string SEARCH_SCHEDULE_FREQUENCY = "LoadsFrequencySchedule/Search";
        private const string UPSERT_SCHEDULE_FREQUENCY = "LoadsFrequencySchedule/Upsert";
        private const string DELETE_SCHEDULE_FREQUENCY = "LoadsFrequencySchedule";

        private const string SEARCH_SCHEDULE_CARRIER = "LoadsCarrierUnit/Search";
        private const string UPSERT_SCHEDULE_CARRIER = "LoadsCarrierUnit/Upsert";
        private const string DELETE_SCHEDULE_CARRIER = "LoadsCarrierUnit";


        #endregion

        #region Private Variables
        private ILogger<LoadsScheduleAdapter> _logger { get; set; }
        private readonly string? _BaseAddress;
        private readonly string? _ApiManagerToken;
        #endregion
        /// <summary>
        /// LoadsScheduleAdapter
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="configService"></param>
        /// <param name="logger"></param>
        public LoadsScheduleAdapter(IHttpClientFactory clientFactory, IConfiguration configService, ILogger<LoadsScheduleAdapter> logger) : base(clientFactory)
        {
         
            this._BaseAddress = configService.GetValue<string>("LogisticsCommon:LOADSAPIBaseAddress");
            this._ApiManagerToken = configService.GetValue<string>("LogisticsCommon:APIManagerKey");
            this.TimeoutSeconds = 300;
            _logger = logger;
        }
        protected override string? BaseAddress => _BaseAddress;
        protected override string? ApiManagerToken => _ApiManagerToken;
        #region  LOADS SUMMARY
        public async Task<ResponseModel> CreateLoadsScheduleSummary(List<LoadsScheduleSummaryModel> loadsScheduleSummary)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<List<LoadsScheduleSummaryModel>, ResponseModel>(CREATE_SCHEDULE_SUMMARY, loadsScheduleSummary);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule summary created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule summary: {ex.Message}";
                _logger.LogError(ex, "Error creating loads schedule summary");
            }
            return await Task.FromResult(response);

        }
        public async Task<ResponseModel> UpdateLoadsScheduleSummary(List<LoadsScheduleSummaryModel> loadsScheduleSummary)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PutRequestAsync<List<LoadsScheduleSummaryModel>, ResponseModel>(UPDATE_SCHEDULE_SUMMARY, loadsScheduleSummary);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule summary updated successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule summary: {ex.Message}";
                _logger.LogError(ex, "Error UpdateLoadsScheduleSummary");
            }
            return await Task.FromResult(response);
        }
        public async  Task<ResponseModel> DeleteLoadsScheduleSummary(string pk, string DocumentType, DateTime ScheduleStartDate, string ScheduleStatusCategory)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                string strScheduleStartDate = ScheduleStartDate.ToString("yyyy-MM-ddTHH:mm:ss");
                string DeleteUri = string.Format("LoadsScheduleSummary/{0}/{1}/{2}/{3}", pk, DocumentType,ScheduleStatusCategory,strScheduleStartDate);
                if (!Program.IsUnitTestRunning)
                {
                    var resposne = await this.DeleteRequestAsync(DeleteUri);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule summary created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule summary: {ex.Message}";
                _logger.LogError(ex, "Error DeleteLoadsScheduleSummary");
            }
            return response;
        }
        public async Task<List<LoadsScheduleSummaryModel>> SearchLoadsScheduleSummary(LoadsScheduleInputModel inputModel)
        {
            List<LoadsScheduleSummaryModel> lstScheduleSummary = new List<LoadsScheduleSummaryModel>();
            try
            {
                if(!Program.IsUnitTestRunning)
                {
                    lstScheduleSummary = await this.PostRequestAsync<LoadsScheduleInputModel, List<LoadsScheduleSummaryModel>>(SEARCH_SCHEDULE_SUMMARY, inputModel);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error SearchLoadsScheduleSummary");
            }
            return lstScheduleSummary;
        }
        #endregion

        #region LOADS DETAILS
        public async Task<ResponseModel> CreateLoadsScheduleDetails(List<LoadsScheduleDetailsModel> loadsScheduleDetails)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                this.TimeoutSeconds = 300;
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<List<LoadsScheduleDetailsModel>, ResponseModel>(CREATE_SCHEDULE_DETAILS, loadsScheduleDetails);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule summary created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule summary: {ex.Message}";
                _logger.LogError(ex, "Error creating loads schedule details");
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> UpsertLoadsScheduleDetails(List<LoadsScheduleDetailsModel> loadsScheduleDetails)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<List<LoadsScheduleDetailsModel>, ResponseModel>(UPSERT_SCHEDULE_DETAILS, loadsScheduleDetails);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule details created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule summary: {ex.Message}";
                _logger.LogError(ex, "Error UpsertLoadsScheduleDetails");
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> DeleteLoadsScheduleDetails(string pk, string DocumentType, DateTime ScheduleStartDate, string ScheduleStatusCategory)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Logic to create loads schedule summary
                string strScheduleStartDate = ScheduleStartDate.ToString("yyyy-MM-ddTHH:mm:ss");
                string DeleteUri = string.Format("LoadsScheduleDetails/{0}/{1}/{2}/{3}", pk, DocumentType,ScheduleStatusCategory, strScheduleStartDate);
                if (!Program.IsUnitTestRunning)
                {
                    var resposne = await this.DeleteRequestAsync(DeleteUri);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule summary created successfully.";
                _logger.LogError($"Error DeleteLoadsScheduleDetails");
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule summary: {ex.Message}";
            }
            return response;
        }
        public async Task<List<LoadsScheduleDetailsModel>> SearchLoadsScheduleDetails(LoadsScheduleInputModel inputModel)
        {
            List<LoadsScheduleDetailsModel> response = new List<LoadsScheduleDetailsModel>();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<LoadsScheduleInputModel, List<LoadsScheduleDetailsModel>>(SEARCH_SCHEDULE_DETAILS, inputModel);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error SearchLoadsScheduleDetails");
            }
            return response;
        }

        #endregion

        #region LOADS FREQUENCY SCHEDULE
         public async Task<ResponseModel> UpsertLoadsScheduleFrequency(List<LoadsFrequencyModel> loadsFrequencyModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<List<LoadsFrequencyModel>, ResponseModel>(UPSERT_SCHEDULE_FREQUENCY, loadsFrequencyModel);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule frequency created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule Frequency: {ex.Message}";
                _logger.LogError(ex, "Error UpsertLoadsScheduleFrequency");
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> DeleteLoadsScheduleFrequency(string pk, string DocumentType, int OriginationUnitId, int DestinationUnitId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Logic to create loads schedule summary
                string DeleteUri = string.Format("LoadsFrequencySchedule/{0}/{1}/{2}/{3}", pk, DocumentType, OriginationUnitId, DestinationUnitId);
                if (!Program.IsUnitTestRunning)
                {
                    var resposne = await this.DeleteRequestAsync(DeleteUri);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule Frequency deleted successfully.";
                _logger.LogError($"Error DeleteLoadsScheduleFrequency");
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule summary: {ex.Message}";
            }
            return response;
        }
        public async Task<List<LoadsFrequencyModel>> SearchLoadsScheduleFrequency(LoadsFrequencyInputModel inputModel)
        {
            List<LoadsFrequencyModel> response = new List<LoadsFrequencyModel>();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<LoadsFrequencyInputModel, List<LoadsFrequencyModel>>(SEARCH_SCHEDULE_FREQUENCY, inputModel);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error SearchLoadsScheduleFrequency");
            }
            return response;
        }

        #endregion

        #region LOADS  SCHEDULE CARRIER

        public async Task<ResponseModel> UpsertLoadsScheduleCarrier(List<LoadsCarrierUnitModel> loadsCarrierUnitModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<List<LoadsCarrierUnitModel>, ResponseModel>(UPSERT_SCHEDULE_CARRIER, loadsCarrierUnitModel);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule carrier created successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule carrier: {ex.Message}";
                _logger.LogError(ex, "Error UpsertLoadsScheduleCarrier");
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> DeleteLoadsScheduleCarrier(string pk, string DocumentType, int OriginationUnitId, int DestinationUnitId,string CarrierId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Logic to create loads schedule summary
                string DeleteUri = string.Format("DeleteLoadCarrierUnit/{0}/{1}/{2}/{3}/{4}", pk, DocumentType, CarrierId, OriginationUnitId, DestinationUnitId);
                if (!Program.IsUnitTestRunning)
                {
                    var resposne = await this.DeleteRequestAsync(DeleteUri);
                }
                response.IsSuccessful = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Loads schedule carrier deleted successfully.";
                _logger.LogError($"Error DeleteLoadsScheduleCarrier");
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.StatusCode = 500;
                response.ResponseMessage = $"Error creating loads schedule carrier: {ex.Message}";
            }
            return response;
        }
        public async Task<List<LoadsCarrierUnitModel>> SearchLoadsScheduleCarrier(LoadsCarrierUnitInputModel inputModel)
        {
            List<LoadsCarrierUnitModel> response = new List<LoadsCarrierUnitModel>();
            try
            {
                if (!Program.IsUnitTestRunning)
                {
                    response = await this.PostRequestAsync<LoadsCarrierUnitInputModel, List<LoadsCarrierUnitModel>>(SEARCH_SCHEDULE_CARRIER, inputModel);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error SearchLoadsScheduleCarrier");
            }
            return response;
        }

        #endregion
    }
}
