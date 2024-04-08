using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.Services.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using E_Commerse_Website.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerse_Website.Controllers
{
    public class AdminController : BaseController
    {
        private readonly myContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ICustomerRepo _customerRepo;
        private readonly IProductRepo _productRepo;
        private readonly IUnitOfWork _unit;
        public AdminController(myContext context, IWebHostEnvironment env, ICustomerRepo customerRepo, IUnitOfWork unit) :base(context,env)
        {
            _context = context;
            _env = env;
            _customerRepo = customerRepo;
            _unit = unit;
        }

        //------------------------------------------------------------------------------------------ View Page  -  Index    ////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            RequireCall("index");
            return View();
        }

        //----------------------------------------------------------------------------------------- View Page  -  Login    ////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string adminEmail, string adminPassword, string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to another page, such as the home page
                return RedirectToAction("Index", "Home");
            }
            var row = _context.tbl_admin.FirstOrDefault(a => a.admin_email == adminEmail);
            if (row != null && row.admin_password == adminPassword)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, row.admin_id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, row.admin_name));
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim(ClaimTypes.UserData, row.admin_image));
                ClaimsIdentity CI = new ClaimsIdentity(claims, "myCookie");
                ClaimsPrincipal CP = new ClaimsPrincipal(CI);
                var AP = new AuthenticationProperties
                {

                };
                await HttpContext.SignInAsync("myCookie", CP, AP);
                return Redirect(string.IsNullOrEmpty(ReturnUrl) ? "/home/index" : ReturnUrl);
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return RedirectToAction("index");
        }

        //----------------------------------------------------------------------------------------- No View  -  Logout    ////
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Admin/login");
            }
            await HttpContext.SignOutAsync("myCookie");
            return Redirect("/home/index");
        }

        //--------------------------------------------------------------------------------------- View Page  -  Admin Details   ////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin_Details_Page()
        {
            RequireCall("Admin_Details_Page");
            //here i want to read the cookie value
            var userobjid = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userobjid != null)
            {
                string aid = userobjid.Value;
                Admin? obj = _context.tbl_admin.FirstOrDefault(o => o.admin_id.ToString() == aid);
                return View(obj);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Admin_Details_Page(Admin obj, IFormFile? img)
        {
            Admin? data = await _context.tbl_admin.FirstOrDefaultAsync(o => o.admin_id == obj.admin_id);
            if (data == null) { return NotFound(); }

            if (img != null && img.Length > 0)
            {
                string wwwrootpath = _env.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                string dir = Path.Combine(wwwrootpath, "AdminImages");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string filepath = Path.Combine(dir, fileName);
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    img.CopyTo(fs);
                }
                data.admin_image = fileName;

            }
            if (ModelState.IsValid)
            {
                var previousData = await _context.tbl_admin.FirstOrDefaultAsync(u => u.admin_id == obj.admin_id);
                data.admin_name = obj.admin_name;
                data.admin_email = obj.admin_email;
                data.admin_password = obj.admin_password;
                _context.tbl_admin.Update(data);
                await EditHistory(previousData,data, "Admin Updated");
                await _context.SaveChangesAsync();
                if(img != null && img.Length > 0)
                {
                    await UpdateImageClaims(data.admin_image);
                }
                return RedirectToAction("Admin_details_page");
            }
            return NotFound();
        }
        public async Task UpdateImageClaims(string newImageFileName)
        {
            List<Claim> claims = User.Claims.ToList();
            var imageClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
            if (!string.IsNullOrEmpty(newImageFileName))
            {
                claims.Remove(imageClaim);
                claims.Add(new Claim(ClaimTypes.UserData, newImageFileName));
                ClaimsIdentity CI = new ClaimsIdentity(claims, "myCookie");
                ClaimsPrincipal CP = new ClaimsPrincipal(CI);
                var AP = new AuthenticationProperties
                {

                };
                await HttpContext.SignInAsync("myCookie", CP, AP);
            }
        }
        //--------------------------------------------------//  Customer Operations  //----------------------------------------------------------
        //----------------------------------------------------- Customer list page
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CustomerList()
        {
            RequireCall("CustomerList");
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomerList()
        {
            try
            {
                var list = await _unit.Customer.GetAllAsync();
                return Json(list.ToList());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        //------------------------------------------------  Upsert(Update and Insert)  Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int id)
        {
            RequireCall("CustomerList");
            if (id != 0)
            {
                Customer c = await _unit.Customer.GetAsync(x => x.customer_id == id);
                if (c == null) { return RedirectToAction("EntityNotExist"); }
                return View(c);
            }
            return View(new Customer());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert([FromForm] Customer obj, [ValidateNever] IFormFile imagedata)
        {
            try
            {
                if (obj.customer_id == 0) { ModelState.Remove("customer_id"); }
                if (imagedata == null) { ModelState.Remove("imagedata"); }
                if (ModelState.IsValid)
                {
                    if (obj.customer_id != 0)
                    {
                        //update(edit) customer
                        var preObj = await _unit.Customer.GetAsync(o=>o.customer_id==obj.customer_id);
                        if (imagedata != null && imagedata.Length > 0)
                        {
                            obj.customer_image = ImageSave(imagedata, "CustomerImages");
                        }
                        await _unit.Customer.Update(obj);
                        await EditHistory(preObj, new_obj:obj,"Customer Updated");
                        await _unit.SaveAsync();
                        return Json(new { message = "Customer Updated successfully" });
                    }
                    else
                    {
                        //add customer
                        if (imagedata != null && imagedata.Length > 0)
                        {
                            obj.customer_image = ImageSave(imagedata,"CustomerImages");
                        }
                        await _unit.Customer.AddAsync(obj);
                        await _unit.SaveAsync();
                        await AddHistory(obj, "Customer Added");
                        return Json(new { message = "Customer Added successfully" });
                    }
                }
                return Json(new { message = "Something Went Wrong" });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        //------------------------------------------------------   Page : Customer Details
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            RequireCall("CustomerList");
            Customer c = await _unit.Customer.GetAsync(x => x.customer_id == id);
            return View(c);
        }

        public async Task<IActionResult> Demo()
        {
            RequireCall("CustomerList");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CustomerDelete(int id)
        {
            try
            {
                if (id == 0) { return Json(false); }
                var obj = await _customerRepo.GetAsync(x => x.customer_id == id);
                await _unit.Customer.RemoveAsync(obj);
                await DeleteHistory(obj, "Customer Deleted");
                await _unit.SaveAsync();
                return Json(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id == 0) { return Json(null); }
                var obj = await _unit.Customer.GetAsync(x => x.customer_id == id);
                return Json(obj);
            }
            catch (Exception)
            {
                return Json(null);
                throw;
            }
        }

        //------------------------------------------------------ View Page :  Customer DeleteRange  ( Select Delete )
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> CustomerDeleteRange()
        {
            RequireCall("CustomerList");
            string? idsJson = HttpContext.Request.Query["ids"];
            if (string.IsNullOrEmpty(idsJson))
            {
                return NotFound();
            }
            int[]? ids = JsonConvert.DeserializeObject<int[]>(idsJson);
            var obj = await _unit.Customer.GetAsyncRange(c => ids.Contains(c.customer_id));
            var c_list = obj.ToList();
            if (c_list.Count == 0 || c_list.Count==null)
            {
                return RedirectToAction("CustomerList");
            }

            // Your logic here using the array of IDs

            return View(obj.ToList());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> CustomerDeleteRange([FromBody] int[] ids)
        {
            try
            {
                if(ids==null || ids.Length == 0)
                {
                    return Json(false);
                }
                var obj = await _unit.Customer.GetAsyncRange(c => ids.Contains(c.customer_id));
                _customerRepo.RemoveRange(obj);
                await _unit.SaveAsync();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
            
        }
        //--------------------------------------------- Extra Methods

       

        

        public IActionResult SomethingWentWrong()
        {
            return View();
        }

        public IActionResult EntityNotExist()
        {
            return View();
        }
    }
}
