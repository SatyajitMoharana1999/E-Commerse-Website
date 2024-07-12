using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.DTO;
using E_Commerse_Website.Services.EmailServices;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace E_Commerse_Website.Controllers
{
	public class RegistrationController : Controller
	{
		private readonly myContext _context;
		private readonly IEmailService _emailService;

		public RegistrationController(myContext context, IEmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(CustomerViewModel model)
		{
			if (ModelState.IsValid)
			{
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
                // Generate and send OTP
                await GenerateAndSendOTP(customer.customer_id, customer.customer_email);

				//return RedirectToAction("VerifyEmailIdentity", new { email = customer.customer_email,id=customer.customer_id });
				return Json(new {id=customer.customer_id,email=customer.customer_email});
			}
			return Json(false);
		}
		public async Task<IActionResult> VerifyOTP(string email, int id)
		{
            ViewBag.Email = email;
            ViewBag.CustomerId = id;
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
			await GenerateAndSendOTP(id, email);

            return Json(true);
		}
		private async Task GenerateAndSendOTP(int customerId, string email)
		{
			//// Delete OTPs older than 24 hours
			//var cutoffDate = DateTime.Now.AddHours(-24);
			//var oldOtps = _context.tbl_OTP_Customer.Where(o => o.CreatedAt < cutoffDate);
			//_context.tbl_OTP_Customer.RemoveRange(oldOtps);
			//await _context.SaveChangesAsync();

			//// Ensure only 3 OTPs per 24-hour period
			//var recentOtps = await _context.tbl_OTP_Customer
			//	.Where(o => o.CustomerId == customerId && o.CreatedAt >= cutoffDate)
			//	.OrderByDescending(o => o.CreatedAt)
			//	.ToListAsync();

			//if (recentOtps.Count >= 3)
			//{
			//	// Use the most recent OTP
			//	var recentOtp = recentOtps.First();
			//	await SendOTPEmail(email, recentOtp.Code);
			//	return;
			//}
			

			var otp = new OTP_Customer
			{
				CustomerId = customerId,
				Code = GenerateOTP(),
				CreatedAt = DateTime.Now,
			};

			_context.tbl_OTP_Customer.Add(otp);
			await _context.SaveChangesAsync();

			SendOTPEmail(email, otp.Code);
		}

		private string GenerateOTP()
		{
			var random = new Random();
			return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
		}

		private async Task SendOTPEmail(string email, string otp)
		{
			var message = $"Your OTP is {otp}. It is valid for 10 minutes.";
			var dtObj = new EmailDTO()
			{
				To=email,
				Subject="From Eshop",
				Body=$"<h1>{message}</h1>"
			};
			await _emailService.SendEmailAsync(dtObj);
		}


		[HttpPost]
		public async Task<IActionResult> VerifyOTP(string email,int id,string code)
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
				var target_customer = customers.Where(u => u.customer_id == id).FirstOrDefault();
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
                await GenerateAndSendOTP(id, email);
                return Json(true);
            }
			catch (Exception)
			{
				return NotFound();
				throw;
			}
			
        }

        public async Task<IActionResult> DupulicateEmail()
		{
			return View();
		}

    }
}