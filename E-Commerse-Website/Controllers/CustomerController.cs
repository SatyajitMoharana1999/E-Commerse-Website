using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerse_Website.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IProductRepo _repo;
        private readonly ICustomerRepo _cRepo;
        private readonly ICartRepo _cartRepo;
        private readonly myContext _context;
        public CustomerController(IProductRepo repo, ICustomerRepo cRepo,myContext context, ICartRepo cartRepo)
        {
            _repo = repo;
            _cRepo = cRepo;
            _context = context;
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> Index()
        {
            await RequiredCustomerCall();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getSliderProductList()
        {
            try
            {
                var p_list = await _repo.GetAsyncRange(u => !u.product_deleted);
                var top3product = p_list.OrderByDescending(p => p.product_id).Take(3).Select(p => new
                {
                    product_id = p.product_id,
                    product_name = p.product_name,
                    product_description = p.product_description,
                    product_price = p.product_price,
                    product_image=p.product_image
                });
                return Json(top3product);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> getSmallBannerProduct()
        {
            try
            {
                var p_list = await _repo.GetAsyncRange(u => u.cat_id == 3 && !u.product_deleted);
                var mostRecentProduct = p_list.OrderByDescending(p => p.product_id).FirstOrDefault();
                var pro = new
                {
                    product_id = mostRecentProduct.product_id,
                    product_name = mostRecentProduct.product_name,
                    product_description = mostRecentProduct.product_description,
                    product_price = mostRecentProduct.product_price,
                    product_image = mostRecentProduct.product_image
                };
                return Json(pro);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> getRandomProducts()
        {
            try
            {
                var products = await _repo.GetWithIncludeAsyncRange(p=>!p.product_deleted,p=>p.Category);
                var random = new Random();
                var productsByCategory = products
                    .GroupBy(p => p.cat_id)
                    .SelectMany(g => g.OrderBy(_ => random.Next()))
                    .ToList();
                var selectedProducts = productsByCategory.Take(8).ToList();
                var projectedProducts = selectedProducts.Select(p => new
                {
                    product_id = p.product_id,
                    product_name = p.product_name,
                    product_description = p.product_description,
                    product_price = p.product_price,
                    product_image = p.product_image,
                    cat_id = p.cat_id,
                    Category = p.Category.category_name
                }).ToList();
                return Json(projectedProducts);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<IActionResult> GetRecentlyAddedProducts()
        //{
        //    try
        //    {
        //        // Fetch all products that are not deleted
        //        var products = await _repo.GetWithIncludeAsyncRange(p => !p.product_deleted, p => p.Category);

        //        // Order products by their product_id in descending order to get the most recent products first
        //        var recentProducts = products
        //            .OrderByDescending(p => p.product_id)
        //            .Take(8) // Take the first 8 products
        //            .Select(product => new // Project the selected products into an anonymous type
        //            {
        //                product_id = product.product_id,
        //                product_name = product.product_name,
        //                product_description = product.product_description,
        //                product_price = product.product_price,
        //                product_image = product.product_image,
        //                cat_id = product.cat_id,
        //                Category = product.Category.category_name
        //            })
        //            .ToList(); // Convert the result to a list

        //        // Return the result as JSON
        //        return Json(recentProducts);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (optional)
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        public async Task<IActionResult> GetRecentlyAddedProducts()
        {
            try
            {
                var products = await _repo.GetWithIncludeAsyncRange(p => !p.product_deleted, p => p.Category);

                var productsByCategory = new List<Product>();

                // Group products by category
                var groupedProducts = products.GroupBy(p => p.cat_id);

                // Iterate through each group of products by category
                foreach (var group in groupedProducts)
                {
                    // Order products within each category by product_id in descending order
                    var orderedProducts = group.OrderByDescending(p => p.product_id).ToList();

                    // Skip the first product and take subsequent recent products until 8 products are added
                    var recentProducts = orderedProducts.Skip(1).Take(8 - productsByCategory.Count);

                    // Add the recent products to the list
                    productsByCategory.AddRange(recentProducts);

                    // If there are 8 products, break the loop
                    if (productsByCategory.Count >= 8)
                        break;
                }

                // Project the selected products into an anonymous type and return the result as JSON
                var result = productsByCategory.Select(p => new
                {
                    product_id = p.product_id,
                    product_name = p.product_name,
                    product_description = p.product_description,
                    product_price = p.product_price,
                    product_image = p.product_image,
                    cat_id = p.cat_id,
                    Category = p.Category.category_name
                }).ToList();

                return Json(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            try
            {
                var obj = await _repo.GetAsync(p => p.product_id == id);
                return View(obj);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated && User.IsInRole("Customer"))
                {
                    Claim cid = User.FindFirst(ClaimTypes.NameIdentifier);
                    var customer = await _cRepo.GetAsync(u => u.customer_id.ToString() == cid.Value);
                    if (id == 0 || id == null)
                    {
                        return Json(new { success = false, loginPopUp = false, message = "something went wrong" });
                    }
                    if (customer == null)
                    {
                        await HttpContext.SignOutAsync("CustomerCookie");
                        return Json(new { success = false, loginPopUp = true, message = "Your account is not valid please login again" });
                    }
                    var exist = await _context.tbl_cart.Where(u => u.cust_id.ToString()==cid.Value && u.prod_id == id && u.cart_status==0).FirstOrDefaultAsync();
                    if(exist != null)
                    {
                        return Json(new { success = true, loginPopUp = false, message = "This Product Is Already In Your Cart" });
                        //exist.product_quantity = exist.product_quantity + 1;
                    }
                    else
                    {
                        var obj = new Cart
                        {
                            cust_id = customer.customer_id,
                            prod_id = id,
                            product_quantity = 1,
                            cart_status = 0
                        };
                        await _context.tbl_cart.AddAsync(obj);
                    }
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message="Successfully Added in Your Cart" });
                }
                return Json(new { success = false,loginPopUp=true, message = "It seems that you did not log in yet. Please Login First" });
            }
            catch (Exception)
            {
                return Json(new { success = false, loginPopUp = false, message = "something went wrong" });
                throw;
                
            }
        }

        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                int id = 0;
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Customer"))
                    {
                        Claim c_id = User.FindFirst(ClaimTypes.NameIdentifier);
                        id = Convert.ToInt32(c_id.Value);
                    }
                }
                if (id == 0)
                {
                    return Json(null);
                }
                var list = await _cartRepo.GetWithIncludeAsyncRange(u => u.cust_id == id, u => u.prod);
                return Json(list);
            }
            catch (Exception)
            {
                return Json(null);
                throw;
            }
        }

        public async Task<IActionResult> RemoveCartItem(int id)
        {
            try
            {
                if(id==0 || id == null)
                {
                    return Json(false);
                }
                int customerId = 0;
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole("Customer"))
                    {
                        Claim c_id = User.FindFirst(ClaimTypes.NameIdentifier);
                        customerId = Convert.ToInt32(c_id.Value);
                    }
                }
                var cart = await _cartRepo.GetAsync(u => u.cart_id == id && u.cust_id == customerId && u.cart_status == 0);
                if (cart == null)
                {
                    return Json(false);
                }
                await _cartRepo.HardDelete(cart.cart_id);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }

        public async Task<IActionResult> ElectronicsPage()
        {
            await RequiredCustomerCall();
            return View();
        }

        public async Task<IActionResult> GetElectronics(int pageSize=30,int pageNumber=1)
        {
            try
            {
                var data = await _repo.GetAsyncRange(x => x.product_deleted);
                var pageinatedData = data.OrderByDescending(x => x.product_id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                int totalElement = pageinatedData.Count;
                int totalPages = (int)Math.Ceiling((totalElement / (double)pageSize));
                return Json(new { pageinatedData , totalPages });
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }


        public async Task RequiredCustomerCall()
        {
            // Debugging: Output authentication state to console
            Console.WriteLine($"User authenticated: {User.Identity.IsAuthenticated}");

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Customer"))
                {
                    Claim c_name = User.FindFirst(ClaimTypes.Name);
                    ViewData["CustomerName"] = c_name.Value;
                }
            }
        }

    }
}
