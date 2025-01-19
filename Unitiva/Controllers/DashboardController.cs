using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unitiva.Service.Contracts.Dashborad;
using Unitiva.Service.Contracts.ILogin;

namespace Unitiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashborad _dashborad;
        private readonly IConfiguration _configuration;
        public DashboardController(IDashborad dashborad, IConfiguration configuration)
        {
            _dashborad = dashborad;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var res = await _dashborad.GetDashboard();
            return Ok(res);
        }

        [HttpGet("facultyCount")]
        public async Task<IActionResult> facultyCount()
        {
            var res = await _dashborad.GetFacultyCounts();
            return Ok(res);
        }


        [HttpGet("Status/{status}")]
        public async Task<IActionResult> getFormByStatus(string status)
        {
            var res = await _dashborad.GetFormByStatus(status);
            return Ok(res);
        }
    }
}
