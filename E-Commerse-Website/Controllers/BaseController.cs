using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.Services.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace E_Commerse_Website.Controllers
{
    public class BaseController : Controller
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _env;
        public BaseController(myContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Admin")]
        public string ImageSave(IFormFile imageData, string folderName)
        {
            try
            {
                string wwwrootpath = _env.WebRootPath;
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(imageData.FileName);
                string directory = Path.Combine(wwwrootpath, folderName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string filepath = Path.Combine(directory, filename);
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    imageData.CopyTo(fs);
                }
                return filename;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        
        public void RequireCall(string activeClass)
        {
            if (User.Identity.IsAuthenticated)
            {
                Claim? aid = User.FindFirst(ClaimTypes.NameIdentifier);
                var a_role = User.FindFirst(ClaimTypes.Role)?.Value;
                var a_img = User.FindFirst(ClaimTypes.UserData)?.Value;
                string? aidval = aid?.Value;
                string? name = _context.tbl_admin.Where(o => o.admin_id.ToString() == aidval).Select(p => p.admin_name).FirstOrDefault();
                string fn = name.Trim();
                string[] parts = fn.Split(' ');
                string firstName = parts.Length > 1 ? parts[0] : fn;
                ViewData["AdminName"] = firstName;
                ViewData["Role"] = a_role;
                ViewData["AdminImage"] = a_img;
            }

            ViewData["Admin_Details_Page"] = "";
            ViewData["CustomerList"] = "";
            ViewData["index"] = "";
            ViewData["Upsert"] = "";
            ViewData["CategoryList"] = "";
            ViewData["ProductList"] = "";
            ViewData["ActivityHistory"] = "";
            ViewData[activeClass] = "active";
        }


        //__------------------------------------------------------- //  History Methods  // ---------------------------------------------------__
        [Authorize]
        public async Task AddHistory<T>(T obj, string title)
        {
            string className = typeof(T).Name;
            string actionDescription = "Added";
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string newJson = JsonConvert.SerializeObject(obj,settings);

            Claim aId = User.FindFirst(ClaimTypes.NameIdentifier);
            int adminId = int.Parse(aId.Value);
            var adminName = await _context.tbl_admin.FirstOrDefaultAsync(o => o.admin_id == adminId);
            title = title + " by " + adminName.admin_name;

            var AH = new AdminHistory
            {
                AH_time = DateTime.Now,
                AH_title = title,
                AH_description = $"{actionDescription}. New {className}: {newJson}",
                AH_deleted = false,
                admin_id = adminId
            };

            await _context.tbl_adminHistory.AddAsync(AH);
            await _context.SaveChangesAsync();
        }

        [Authorize]
        public async Task EditHistory<T>(T pre_obj, T new_obj, string title)
        {
            string className = typeof(T).Name;
            string actionDescription = "Updated";
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string preJson = JsonConvert.SerializeObject(pre_obj, settings);
            string newJson = JsonConvert.SerializeObject(new_obj, settings);

            Claim aId = User.FindFirst(ClaimTypes.NameIdentifier);
            int adminId = int.Parse(aId.Value);
            var adminName = await _context.tbl_admin.FirstOrDefaultAsync(o => o.admin_id == adminId);
            title = title + " by " + adminName.admin_name;

            var AH = new AdminHistory
            {
                AH_time = DateTime.Now,
                AH_title = title,
                AH_description = $"{actionDescription}. Previous {className}: {preJson}. New {className}: {newJson}",
                AH_deleted = false,
                admin_id = adminId
            };

            await _context.tbl_adminHistory.AddAsync(AH);
            //await _unit.SaveAsync();
        }

        [Authorize]
        public async Task DeleteHistory<T>(T obj, string title)
        {
            string className = typeof(T).Name;
            string actionDescription = "Deleted";
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string objJson = JsonConvert.SerializeObject(obj, settings);

            Claim aId = User.FindFirst(ClaimTypes.NameIdentifier);
            int adminId = int.Parse(aId.Value);
            var adminName = await _context.tbl_admin.FirstOrDefaultAsync(o => o.admin_id == adminId);
            title = title + " by " + adminName.admin_name;

            var AH = new AdminHistory
            {
                AH_time = DateTime.Now,
                AH_title = title,
                AH_description = $"{actionDescription}. {className}: {objJson}",
                AH_deleted = false,
                admin_id = adminId
            };

            await _context.tbl_adminHistory.AddAsync(AH);
        }
    }
}
