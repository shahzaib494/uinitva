using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unitiva.Service.Contracts.Dashborad
{
    public interface IDashborad
    {
        public Task<dynamic> GetDashboard();
        public Task<dynamic> GetFacultyCounts();
        public Task<dynamic> GetFormByStatus(string status);
    }
}
