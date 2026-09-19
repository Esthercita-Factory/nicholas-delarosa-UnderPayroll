using UnderPayroll.Web.ViewModels.Home;

namespace UnderPayroll.Web.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetSummaryAsync();
}
