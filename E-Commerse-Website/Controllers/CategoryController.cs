using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.UnitOfWork;
using E_Commerse_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace E_Commerse_Website.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork _unit;
        public CategoryController(myContext context, IWebHostEnvironment env, IUnitOfWork unit) : base(context, env)
        {
            _unit = unit;
        }

        //__--------------------------------------------- //  Category Operations  // -----------------------------------------__

        //--------------------------------------------- View Page :  Category List
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CategoryList()
        {
            RequireCall("CategoryList");
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetCategoryList()
        {
            try
            {
                var categoriesWithProducts = await _unit.Category.GetAllWithIncludeAsync(c => c.Product);
                var categoryList = categoriesWithProducts.Where(o => !o.category_deleted).Select(c => new
                {
                    category_id = c.category_id,
                    category_name = c.category_name,
                    product_count = c.ProductCount
                });

                return Json(categoryList.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        
        public async Task<IActionResult> AddCategory(Category category)
        {
            try
            {
                if (category.category_id != 0)
                {
                    await _unit.Category.Update(category);
                    await _unit.SaveAsync();
                    return Json(true);
                }
                else
                {
                    await _unit.Category.AddAsync(category);
                    if (category.Product != null)
                    {
                        var admin = User.FindFirst(ClaimTypes.NameIdentifier);
                        foreach (var p in category.Product)
                        {
                            p.adm_id = int.Parse(admin.Value);
                            p.product_image = ImageSave(imageData: p.product_img_data, "ProductImages"); //---------------------------------------------------------------------------------------------------------- marker
                            await _unit.Product.AddAsync(p);
                        }
                    }
                    await _unit.SaveAsync();
                    return Json(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                if (id != 0)
                {
                    var data = await _unit.Category.GetAsync(o => o.category_id == id);
                    if (data != null)
                    {
                        return Json(data);
                    }
                }
                return Json(null);
            }
            catch (Exception)
            {
                return Json(null);
                throw;
            }
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> CategoryAction(int id)
        {
            RequireCall("CategoryList");
            if (id != 0)
            {
                var data = await _unit.Category.GetWithIncludeAsync(c => c.category_id == id && !c.category_deleted, c => c.Product);
                return View(data);
            }
            return NotFound();
        }

        [HttpPost]
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (id != 0)
                {
                    await _unit.Category.SoftDelete(id);
                    await _unit.SaveAsync();
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> DeleteCategoryAndProducts(int id)
        {
            try
            {
                var data = await _unit.Category.GetWithIncludeAsync(c => c.category_id == id, c => c.Product);
                if (id != 0)
                {
                    foreach (var products in data.Product)
                    {
                        await _unit.Product.SoftDelete(products.product_id);
                    }
                    await _unit.Category.SoftDelete(id);
                    await _unit.SaveAsync();
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        [HttpPost]
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Recategorise(int previousId, int newId)
        {
            try
            {
                var data = await _unit.Category.GetWithIncludeAsync(c => c.category_id == previousId, c => c.Product);
                if (previousId != 0 && newId != 0 && previousId != newId)
                {
                    foreach (var products in data.Product)
                    {
                        products.cat_id = newId;
                    }

                    await _unit.SaveAsync();
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }


        //__--------------------------------------------- //  Product Operations  // -----------------------------------------__

        //---------------------------------------------- Page -  Product List

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ProductList()
        {
            RequireCall("ProductList");
            return View();
        }

        
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var data = await _unit.Product.GetAllWithIncludeAsync(p => p.Category);
                data = data.Where(p => !p.product_deleted).ToList();
                var productList = data.Select(p => new
                {
                    product_id = p.product_id,
                    product_name = p.product_name,
                    product_price = p.product_price,
                    product_image = p.product_image,
                    Category = new
                    {
                        category_id = p.Category.category_id,
                        category_name = p.Category.category_name,
                    }
                });
                return Json(productList);
            }
            catch (Exception)
            {
                return Json(new { error = "An error occurred while processing GetAllProducts." });
            }
        }

        //public async Task<IActionResult> GetAllProducts()   //----------------------  same as the above function but in return type - Content
        //{
        //    try
        //    {
        //        var data = await _unit.Product.GetAllWithIncludeAsync(p => p.Category);
        //        data = data.Where(p => !p.product_deleted).ToList();
        //        var options = new System.Text.Json.JsonSerializerOptions
        //        {
        //            ReferenceHandler = ReferenceHandler.Preserve,
        //        };
        //        var jsonData = System.Text.Json.JsonSerializer.Serialize(data, options);
        //        return Json(jsonData);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { error = "An error occurred while processing GetAllProducts." });
        //    }
        //}


        //public async Task<IActionResult> GetAllProducts()  //------------------------  same as the above function but in return type Ok Json 
        //{
        //    try
        //    {
        //        var data = await _unit.Product.GetAllWithIncludeAsync(p => p.Category);
        //        data = data.Where(p => !p.product_deleted).ToList();
        //        var serializerOptions = new JsonSerializerOptions
        //        {
        //            ReferenceHandler = ReferenceHandler.Preserve
        //        };

        //        var jsonData = System.Text.Json.JsonSerializer.Serialize(data, serializerOptions);
        //        return Ok(jsonData); // Use Ok() for successful responses with content
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500); // Return a generic error code (adjust as needed)
        //    }
        //}


        //---------------------------------------------    //  Upsert  (update & insert)
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ProductUpsert(int id)
        {
            RequireCall("ProductList");
            ProductVM pvm = new()
            {
                categoryList = (await _unit.Category.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.category_name,
                    Value = u.category_id.ToString()
                }),
                product = new Product()
            };
            if (id != 0)
            {
                pvm.product = await _unit.Product.GetAsync(u => u.product_id == id);
            }
            else
            {
                var c_id = Request.Query["cat_id"];
                if (!string.IsNullOrEmpty(c_id))
                {
                    pvm.product.cat_id = int.Parse(c_id);
                }
            }
            return View(pvm);
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpsert(ProductVM pvm, IFormFile imagefile)
        {
            try
            {
                if (imagefile != null)
                {
                    pvm.product.product_image = ImageSave(imagefile, "ProductImages");
                }
                if (pvm.product.product_id != 0)
                {
                    await _unit.Product.Update(pvm.product);
                    await _unit.SaveAsync();
                    return Json(true);
                }
                else
                {
                    await _unit.Product.AddAsync(pvm.product);
                    await _unit.SaveAsync();
                    return Json(true);
                }
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(int id)
        {
            try
            {
                await _unit.Product.SoftDelete(id);
                await _unit.SaveAsync();
                return Json(true);  
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }
        public async Task<IActionResult> ProductDetails(int id)
        {
            RequireCall("ProductList");
            var obj = await _unit.Product.GetWithIncludeAsync(u => u.product_id == id, c => c.Admin,c=>c.Category);
            return View(obj);
        }
    }
}
