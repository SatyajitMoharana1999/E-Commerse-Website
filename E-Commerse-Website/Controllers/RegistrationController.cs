using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.DTO;
using E_Commerse_Website.Services.EmailServices;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace E_Commerse_Website.Controllers
{
	public class RegistrationController : Controller
	{
		private readonly myContext _context;
		private readonly IEmailService _emailService;
        private readonly ILogger<RegistrationController> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RegistrationController(myContext context, IEmailService emailService, ILogger<RegistrationController> logger, IServiceProvider serviceProvider)
		{
			_context = context;
			_emailService = emailService;
			_logger = logger;
			_serviceProvider = serviceProvider;

        }

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(CustomerViewModel model)
		{
			try
			{
                if (ModelState.IsValid)
				{
					var customerExists = await _context.tbl_customer.Where(u => u.customer_email == model.customer_email).FirstOrDefaultAsync();
					if (customerExists != null)
					{
						string message = "Thes <b>Email</b> Address Is Already <b>Exist</b>. Use Another Email Or You Can Login Using This Email.";
                        return Json(new { success = false, message });
                    }
					var customer = new Customer
                    {
                        customer_name = model.customer_name,
                        customer_email = model.customer_email,
                        customer_password = model.customer_password, // Hash this password!
                        customer_gender = model.customer_gender,
                        customer_country = model.customer_country,
                        customer_city = model.customer_city,
                        customer_address = model.customer_address
                    };

                    await _context.AddAsync(customer);
                    await _context.SaveChangesAsync();

                    TempData["cid"] = customer.customer_id;

                    // Generate OTP and send email without waiting
                    GenerateAndSendOTPAsyncFireAndForget(customer.customer_id, customer.customer_email);

                    //return RedirectToAction("VerifyEmailIdentity", new { email = customer.customer_email,id=customer.customer_id });
                    return Json(new { success = true, id = customer.customer_id, email = customer.customer_email, message = "Successfully Registered" });
                }
                return Json(new { success = false, message = "Registration Failed" });
            }
			catch (Exception)
			{
                return Json(new { success = false, message = "Registration Failed" });
                throw;
			}
			
        }
		public async Task<IActionResult> VerifyOTP(string email, int id,bool forgetpassword)
		{
            ViewBag.Email = email;
            ViewBag.CustomerId = id;
            ViewBag.forgetpassword = forgetpassword;
            return View();
		}

		[HttpPost]
		public async Task<IActionResult> ReEnterEmail(int id,string email)
		{
			if(id==0 && string.IsNullOrEmpty(email))
			{
				return Json(false);
			}
			var customer = await _context.tbl_customer.Where(u => u.customer_id == id).FirstOrDefaultAsync();
			customer.customer_email = email;
			await _context.SaveChangesAsync();
			GenerateAndSendOTPAsyncFireAndForget(id, email);

            return Json(true);
		}

        private async Task GenerateAndSendOTP(myContext context, IEmailService emailService, int customerId, string email)
        {
            _logger.LogInformation("GenerateAndSendOTP started for customerId: {customerId}, email: {email}", customerId, email);

            try
            {
                var otp = new OTP_Customer
                {
                    CustomerId = customerId,
                    Code = GenerateOTP(),
                    CreatedAt = DateTime.Now,
                };

                await context.tbl_OTP_Customer.AddAsync(otp);
                await context.SaveChangesAsync();

                _logger.LogInformation("OTP generated and saved for customerId: {customerId}", customerId);

                var message = $"Your OTP is {otp.Code}. It is valid for 10 minutes.";
                var dtObj = new EmailDTO()
                {
                    To = email,
                    Subject = "From Eshop",
                    Body = $"<h1>{message}</h1>"
                };

                _logger.LogInformation("Sending email to: {email}", email);
                emailService.SendEmailAsyncFireAndForget(dtObj);
                _logger.LogInformation("Email sent successfully to: {email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating OTP and sending email for customerId: {customerId}, email: {email}", customerId, email);
            }
        }


        private void GenerateAndSendOTPAsyncFireAndForget(int customerId, string email)
        {
            Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var scopedContext = scope.ServiceProvider.GetRequiredService<myContext>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    await GenerateAndSendOTP(scopedContext, emailService, customerId, email);
                }
            });
        }


        private string GenerateOTP()
		{
			var random = new Random();
			return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
		}


		[HttpPost]
		public async Task<IActionResult> VerifyOTP(string email,string code,bool forgetpassword)
		{
			if (ModelState.IsValid)
			{
				
				var customers = await _context.tbl_customer.Where(u => u.customer_email == email).OrderByDescending(o=>o.customer_id)
					.ToListAsync();
				if (customers.Count == 0)
				{
					return Json(new {success=false,message="This Email Is Not Found"});
				}
				//if (customers.Count > 1)
				//{
				//	return RedirectToAction("DupulicateEmail", "Registration");
				//}
				var target_customer = customers[0];
				if(target_customer == null)
				{
                    return Json(new { success = false, message = "No Such Account Found. Register again" });
                }
				var otp = await _context.tbl_OTP_Customer
					.Where(o => o.CustomerId == target_customer.customer_id && o.Code == code)
					.OrderByDescending(u => u.CreatedAt)
					.FirstOrDefaultAsync();
				if(otp!=null && otp.CreatedAt.AddMinutes(10) < DateTime.Now)
				{
                    return Json(new {success=false,message="OTP Expired. Request Again" });
				}
				if (otp != null && otp.CreatedAt.AddMinutes(10) > DateTime.Now)
				{
					if (!forgetpassword)
					{
                        target_customer.isVerified = true;
                        await _context.SaveChangesAsync();

                        //code for direct login agter registration
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, target_customer.customer_id.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, target_customer.customer_name));
                        claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                        ClaimsIdentity CI = new ClaimsIdentity(claims, "CustomerCookie");
                        ClaimsPrincipal CP = new ClaimsPrincipal(CI);
                        var AP = new AuthenticationProperties
                        {

                        };
                        await HttpContext.SignInAsync("CustomerCookie", CP, AP);
						//--------------- end direct login ------------------
                    }
                   

                    return Json(new { success = true, message = "OTP verified successfully" });
                }

				//ModelState.AddModelError(string.Empty, "Invalid OTP or OTP has expired.");
			}

			return Json(new { success = false,message = "Enter OTP again"});
		}

		[HttpPost]
		public async Task<IActionResult> OTPRequest(string email,int id)
		{
			try
			{
                GenerateAndSendOTPAsyncFireAndForget(id, email);
                return Json(true);
            }
			catch (Exception)
			{
				return NotFound();
				throw;
			}
			
        }

		[HttpPost]
		public async Task<IActionResult> Login(string email,string password)
		{
			if (ModelState.IsValid)
			{
				var customer = await _context.tbl_customer.Where(u => u.customer_email == email).ToListAsync();
				if (customer.Count == 0)
				{
					return Json(new { success = false, message = "Wrong Email Id Or Password" });
				}
				if(customer.Count > 1)
				{
                    //return Json(new { success = false, redirectTo = Url.Action("MultipleEmail", "Registration") });
                    return Json(new { success = false, message = "Wrong Email Id Or Password" });
                }
				var target_customer = customer[0];
				if(customer != null && target_customer.customer_password == password)
				{
					
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, target_customer.customer_id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, target_customer.customer_name));
                    claims.Add(new Claim(ClaimTypes.Role, "Customer"));
                    ClaimsIdentity CI = new ClaimsIdentity(claims, "CustomerCookie");
                    ClaimsPrincipal CP = new ClaimsPrincipal(CI);
                    var AP = new AuthenticationProperties
                    {

                    };
                    await HttpContext.SignInAsync("CustomerCookie", CP, AP);
                    return Json(new { success = true, message = "Login Success" });
                }
			}
			return Json(new { success = false, message = "Wrong Email Id Or Password" });
		}
		

		public async Task<IActionResult> ForgetPassword(string f_email)
		{
			try
			{
				var customer = await _context.tbl_customer.Where(u => u.customer_email == f_email).FirstOrDefaultAsync();
				if (customer == null)
				{
					return Json(new { success = false, message = "This Email Address Is Not Found. You Should Register" });
				}
				int id=Convert.ToInt32(customer.customer_id);
				GenerateAndSendOTPAsyncFireAndForget(id, f_email);
                return Json(new { success = true,message="An OTP Sent To Your Email",email=$"{customer.customer_email}",id=customer.customer_id});
			}
			catch (Exception)
			{
                return Json(new { success = false, message = "Something Went Wrong" });
                throw;
			}
		}


		public IActionResult ResetPassword(string email,int id)
		{
			ViewBag.email = email;
			ViewBag.id = id;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(string password,int id,string email)
		{
			try
			{
				var customer = await _context.tbl_customer.Where(u => u.customer_id == id && u.customer_email == email).FirstOrDefaultAsync();
				if (customer == null)
				{
					return Json(new { success = false, message = "Something Went Wrong" });
				}
				customer.customer_password = password;
				await _context.SaveChangesAsync();
				return Json(new { success = true, message = "Password Changed Successfully. Now Login To Your Account Using Your Email Address And New Password" });
            }
			catch (Exception)
			{
                return Json(new { success = false, message = "Something Went Wrong" });
                throw;
			}
		}

		public async Task<IActionResult> CustomerLogout()
		{
			try
			{
                await HttpContext.SignOutAsync("CustomerCookie");
                return Json(true);
            }
			catch (Exception)
			{
				return Json(false);
				throw;
			}
			
		}

        public async Task<IActionResult> MultipleEmail()
		{
			return View();
		}
    }
}