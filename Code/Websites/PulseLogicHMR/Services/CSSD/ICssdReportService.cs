using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services
{
    public interface ICssdReportService
    {
        Task<IList<IntegratedCssdReportDto>> GetIntegratedCssdReport(DateTime FromDate, DateTime ToDate);
    }
}
