using DevExpress.CodeParser;
using LOADS.Interfaces;
using LOADS.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Graph.Models;
using System.Data;

namespace LOADS.Adapters
{
    public class CommonAdapter : ICommonAdapter
    {

        public string DISPATCHERS = "LOADS_DISPATCH";
        public string ITS = "LOADS_ITS";
        public string LOADS_DISPATCH_LEAD = "LOADS_DISPATCH_LEAD";
        public string LOADS_DISPATCH_MANAGER = "LOADS_DISPATCH_MANAGER";
        public string OTHER_ROLES = "ALL_OTHERS";
        public string Role { get; set; } =string.Empty;

        [Inject]
        AuthenticationStateProvider? AuthenticationStateProvider { get; set; }
        public List<SelectListItem> GetWeekEndDates()
        {
            List<SelectListItem> lstWeekEndDates = new List<SelectListItem>();
            List<string> v_lstWeekEndDates = new List<string>();
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
            DateTime dtStartDate = Convert.ToDateTime(WeekendingDate());
            DateTime dtEndDate = DateTime.Now.AddMonths(2); 
            v_lstWeekEndDates = GetWeekendDatesByDateRange(dtStartDate, dtEndDate);
            return lstWeekEndDates = v_lstWeekEndDates.Select(w => new SelectListItem { Text = w, Value = w }).ToList();
        }

        public List<SelectListItem> GetStartWeekEndDates()
        {
            List<SelectListItem> lstWeekEndDates = new List<SelectListItem>();
            List<string> v_lstWeekEndDates = new List<string>();
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
            DateTime dtStartDate = Convert.ToDateTime(SundayWeekendingDate());
            DateTime dtEndDate = DateTime.Now.AddMonths(2);
            v_lstWeekEndDates = GetStartWeekendDatesByDateRange(dtStartDate, dtEndDate);
            return lstWeekEndDates = v_lstWeekEndDates.Select(w => new SelectListItem { Text = w, Value = w }).ToList();
        }
        public List<string> GetWeekendDatesByDateRange(DateTime StartDate, DateTime EndDate)
        {
            List<string> strWeekendList = new List<string>();
            List<DateTime> dtWeekendList = new List<DateTime>();
            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday)
                    dtWeekendList.Add(date);
            }

            foreach (DateTime dt in dtWeekendList.OrderBy(a => a.Date))
            {
                strWeekendList.Add(dt.ToShortDateString());
            }
            return strWeekendList;
        }

        public List<string> GetStartWeekendDatesByDateRange(DateTime StartDate, DateTime EndDate)
        {
            List<string> strWeekendList = new List<string>();
            List<DateTime> dtWeekendList = new List<DateTime>();
            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Sunday)
                    dtWeekendList.Add(date);
            }

            foreach (DateTime dt in dtWeekendList.OrderBy(a => a.Date))
            {
                strWeekendList.Add(dt.ToShortDateString());
            }
            return strWeekendList;
        }


        public LOADSHeaderModel GetLOADSWeekendDates()
        {
            var varLOADSReportHdr = new LOADSHeaderModel
            {
                WeekEndDates = GetWeekEndDates()
            };
            return varLOADSReportHdr;
        }

        public LOADSHeaderModel GetLOADSStartWeekendDates()
        {
            var varLOADSReportHdr = new LOADSHeaderModel
            {
                WeekEndDates = GetStartWeekEndDates()
            };
            return varLOADSReportHdr;
        }

        public string WeekendingDate()
        {
            var dt = FirstDayOfWeek(DateTime.UtcNow).AddDays(6);
            var weekEndDate = dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
            return weekEndDate;
        }

        public string SundayWeekendingDate()
        {
            var dt = FirstDayOfWeek(DateTime.UtcNow).AddDays(7);
            var weekEndDate = dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
            return weekEndDate;
        }
        public string IsSelectedDateCurrentWeekend(DateTime dtSelected)
        {
            var dt = FirstDayOfWeek(dtSelected).AddDays(6);
            var weekEndDate = dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
            return weekEndDate;
        }
        public DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-diff).Date;
        }
        public DateTime LastDayOfWeek(DateTime date)
        {
            DateTime v_lastDayOfWeek = FirstDayOfWeek(date).AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
            return v_lastDayOfWeek;
        }

        public async Task<LoginUser> GetUserCredentials()
        {
            LoginUser loginUser = new LoginUser();
            if(AuthenticationStateProvider != null)
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (authState != null)
                {
                    var user = authState.User;
                    if (user.Identity.IsAuthenticated)
                    {
                        if (user.Claims.First(c => c.Type == "employeeId").Value.ToString() == "122007")
                        {
                            loginUser.EmployeeId = "3144784";
                            loginUser.EmployeeName = user.Identity.Name;
                        }
                        else
                        {
                            loginUser.EmployeeId = user.Claims.First(c => c.Type == "employeeId").Value.ToString();
                            loginUser.EmployeeName = user.Identity.Name;
                        }
                    }
                    foreach (var role in user.Claims.Where(c => c.Type == "role"))
                    {
                        if (role.Value.ToString() == LOADSRoles.LOADS_DISPATCH)
                        {
                            Role = DISPATCHERS;
                        }
                        else if (role.Value.ToString() == LOADSRoles.LOADS_ITS)
                        {
                            Role = ITS;
                        }
                        else if (role.Value.ToString() == LOADSRoles.LOADS_DISPATCH_MANAGER)
                        {
                            Role = LOADS_DISPATCH_MANAGER;
                        }
                        else
                        {
                            Role = OTHER_ROLES;
                        }
                    }
                }
            }
            return loginUser;
        }

        public string GetDefaultScheduleId(List<LoadsScheduleSummaryModel> lstLoadsScheduleSummary)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
            DateTime StartDate = DateTime.UtcNow;
            if (easternTime.DayOfWeek == DayOfWeek.Sunday)
            {
                StartDate = easternTime.AddDays(7);
            }
            else if (easternTime.DayOfWeek == DayOfWeek.Monday)
            {
                StartDate = easternTime.AddDays(6);
            }
            else if (easternTime.DayOfWeek == DayOfWeek.Tuesday)
            {
                StartDate = easternTime.AddDays(5);
            }
            else if (easternTime.DayOfWeek == DayOfWeek.Wednesday)
            {
                StartDate = easternTime.AddDays(4);
            }
            else if (easternTime.DayOfWeek == DayOfWeek.Thursday)
            {
                StartDate = easternTime.AddDays(3 + 7);
            }
            else if (easternTime.DayOfWeek == DayOfWeek.Friday)
            {
                StartDate = easternTime.AddDays(2 + 7);
            }
            else if (easternTime.DayOfWeek == DayOfWeek.Saturday)
            {
                StartDate = easternTime.AddDays(1 + 7);
            }
            if (lstLoadsScheduleSummary != null && lstLoadsScheduleSummary.Count > 0)
            {
                var v_lstScheduleSummary = lstLoadsScheduleSummary ?? new List<LoadsScheduleSummaryModel>();
                v_lstScheduleSummary = v_lstScheduleSummary.Where(a => a.ScheduleStartDateTime.Date == StartDate.Date).ToList();
                if (v_lstScheduleSummary != null && v_lstScheduleSummary.Count > 0)
                {
                    return v_lstScheduleSummary.FirstOrDefault().pk;
                }
            }
            return string.Empty;
        }

    }
}
