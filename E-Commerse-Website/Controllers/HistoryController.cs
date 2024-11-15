using E_Commerse_Website.Data;
using E_Commerse_Website.Services.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerse_Website.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie",Roles ="Admin")]
    public class HistoryController : BaseController
    {
        private readonly IUnitOfWork _unit;
        public HistoryController(IUnitOfWork unit,myContext context,IWebHostEnvironment env) : base(context,env)
        {
            _unit = unit;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ActivityHistory()
        {
            RequireCall("ActivityHistory");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetActivityHistory()
        {
            try
            {
                var AH = await _unit.AdminHistory.GetAllAsync();
                return Json(AH);
            }
            catch (Exception)
            {
                return RedirectToAction("SomethingWentWrong");
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> MyActivity()
        {
            try
            {
                var adm_id = User.FindFirst(ClaimTypes.NameIdentifier);
                if (adm_id.Value == null) { return RedirectToAction("EntityNotExist", "admin"); }
                var AH = await _unit.AdminHistory.GetAsyncRange(a => a.admin_id == int.Parse(adm_id.Value));
                AH = AH.OrderByDescending(o => o.AH_id).Take(20);
                var titles = AH.Select(h => new
                {
                    AH_id = h.AH_id,
                    AH_title = h.AH_title
                });
                return Json(titles);
            }
            catch (Exception)
            {
                return RedirectToAction("SomethingWentWrong");
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("History/ActivityDetails/{id}")]
        public async Task<IActionResult> ActivityDetails(int id)
        {
            try
            {
                RequireCall("ActivityHistory");
                var AHD = await _unit.AdminHistory.GetWithIncludeAsync(u => u.AH_id == id,u=>u.Admin);
                if (AHD == null)
                {
                    return NotFound();
                }
                return View(AHD);
            }
            catch (Exception)
            {
                return NotFound();
                throw;
            }
        }
    }
}
