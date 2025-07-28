using LOADS.Interfaces;
using LOADS.Models;

namespace LOADS.Adapters
{
    public class LoadsService : ILoadsService
    {
        public LoadsScheduleSummaryModel? _summaryModel;

        public List<LoadsScheduleSummaryModel>? _lstscheduleSummaries;
        public void SetScheduleSummary(LoadsScheduleSummaryModel scheduleSummary)
        {
            this._summaryModel = scheduleSummary;
        }
        public LoadsScheduleSummaryModel GetScheduleSummary()
        {
            return this._summaryModel ?? new LoadsScheduleSummaryModel();
        }

        public List<LoadsScheduleSummaryModel> GetScheduleSummaries()
        {
            return this._lstscheduleSummaries ?? new List<LoadsScheduleSummaryModel>();
        }
        public void SetScheduleSummaries(List<LoadsScheduleSummaryModel> scheduleSummaries)
        {
            this._lstscheduleSummaries = scheduleSummaries;
        }
    }
}
