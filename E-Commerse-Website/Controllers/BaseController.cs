using E_Commerse_Website.Data;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.Services.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles = "Admin")]
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
            ViewData[activeClass] = "active";
        }


    }
}
