using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.UnitOfWork;
using E_Commerse_Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
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
                    var preObj = await _unit.Category.GetAsync(o => o.category_id == category.category_id);
                    var copy = new Category
                    {
                        category_id = preObj.category_id,
                        category_name = preObj.category_name,
                        category_deleted = preObj.category_deleted
                    };
                    await _unit.Category.Update(category);
                    await EditHistory(copy, category, "Category Updated");
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

                    // ----------------- Code For Save History
                    if (category.Product != null)
                    {
                        var obj = new Category
                        {
                            category_id = category.category_id,
                            category_name = category.category_name,
                            category_deleted = false,
                            Product = new List<Product>(),
                        };
                        foreach (var p in category.Product)
                        {
                            obj.Product.Add(new Product
                            {
                                product_id = p.product_id,
                                product_name = p.product_name,
                                product_price = p.product_price,
                                product_deleted = p.product_deleted,
                                product_description = p.product_description,
                                product_image = p.product_image,
                                adm_id = p.adm_id,
                                cat_id = p.cat_id
                            });
                        }
                        await AddHistory(obj, "Category Added Along With " +category.Product.Count+ " new Products");
                    }
                    else
                    {
                        await AddHistory(category, "Category Added");
                    }
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
                var x = data.Product.Where(p => !p.product_deleted).ToList();
                data.Product = x;
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
                    var cat = await _unit.Category.GetAsync(o => o.category_id == id);
                    await _unit.Category.SoftDelete(id);
                    await DeleteHistory(cat,"Category Deleted");
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
                    var products = data.Product.Where(p => !p.product_deleted).ToList();
                    foreach (var product in products)
                    {
                        await _unit.Product.SoftDelete(product.product_id);

                    }
                    await _unit.Category.SoftDelete(id);

                    //------------------------------- code to save history
                    var productIds = products.Select(p => p.product_id).ToList();
                    string objJson = JsonConvert.SerializeObject(productIds);
                    Claim aId = User.FindFirst(ClaimTypes.NameIdentifier);
                    int adminId = int.Parse(aId.Value);
                    var adminName = await _unit.Admin.GetAsync(o => o.admin_id == adminId);
                    var AH = new AdminHistory
                    {
                        AH_time = DateTime.Now,
                        AH_title = "Category Deleted Along With All It's Products By " + adminName.admin_name,
                        AH_description = $"Deleted. Category And Product Under This Category. Category: {data.category_name}. Product Ids {objJson}",
                        AH_deleted = false,
                        admin_id = adminId
                    };
                    await _unit.AdminHistory.AddAsync(AH);

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
                var newCategory = await _unit.Category.GetAsync(o => o.category_id==newId);
                if (previousId != 0 && newId != 0 && previousId != newId)
                {
                    var products = data.Product.Where(p=>!p.product_deleted).ToList();
                    foreach (var product in products)
                    {
                        product.cat_id = newId;
                    }

                    //------------------------------- code to save history
                    var productIds = products.Select(p => p.product_id).ToList();
                    string objJson = JsonConvert.SerializeObject(productIds);
                    Claim aId = User.FindFirst(ClaimTypes.NameIdentifier);
                    int adminId = int.Parse(aId.Value);
                    var adminName = await _unit.Admin.GetAsync(o => o.admin_id == adminId);
                    var AH = new AdminHistory
                    {
                        AH_time = DateTime.Now,
                        AH_title = "Products Recategorised By " + adminName.admin_name,
                        AH_description = $"Recategorised. All The Products Recategorised. Previous Category: {data.category_name}. New Category: {newCategory.category_name}. Product Ids {objJson}",
                        AH_deleted = false,
                        admin_id = adminId
                    };
                    await _unit.AdminHistory.AddAsync(AH);
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

        [HttpPost]
        public async Task<IActionResult> GetRowsByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("No IDs provided");
            }
            //  logic to retrieve rows from the database based on the IDs
            //  demonstration purposes, let's assume you have a service to handle this
            try
            {
                // Constructing an expression to filter by IDs
                // Call your GetAsyncRange method passing the filter expression
                var rows = await _unit.Product.GetAsyncRange(x=>ids.Contains(x.product_id)); // Or we can use the below 2 line insted of this line  
                //Expression<Func<Product, bool>> filterByIds = x => ids.Contains(x.product_id);
                //var rows = await _unit.Product.GetAsyncRange(filterByIds);
                var p_list = rows.Select(p => new
                {
                    product_name = p.product_name,
                    product_price = p.product_price,
                    product_description = p.product_description,
                    product_image = p.product_image,
                });
                return Json(p_list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                    var preProduct = await _unit.Product.GetAsync(p => p.product_id == pvm.product.product_id);
                    var copy = new Product
                    {
                        product_id = preProduct.product_id,
                        product_name = preProduct.product_name,
                        product_description = preProduct.product_description,
                        product_price = preProduct.product_price,
                        product_image = preProduct.product_image,
                        product_deleted = preProduct.product_deleted,
                        cat_id = preProduct.cat_id,
                        adm_id = preProduct.adm_id,
                    };
                    await _unit.Product.Update(pvm.product);
                    await EditHistory(copy, preProduct, "Product Updated");
                    await _unit.SaveAsync();
                    return Json(true);
                }
                else
                {
                    var admin = User.FindFirst(ClaimTypes.NameIdentifier);
                    pvm.product.adm_id = int.Parse(admin.Value);
                    await _unit.Product.AddAsync(pvm.product);
                    await _unit.SaveAsync();
                    await AddHistory(pvm.product, "Product Added");
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
                var prod = await _unit.Product.GetAsync(p => p.product_id == id);
                await _unit.Product.SoftDelete(id);
                
                await DeleteHistory(prod, "Product Deleted");
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
