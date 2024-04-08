using E_Commerse_Website.Data;
using E_Commerse_Website.Services.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerse_Website.Controllers
{
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
    }
}
