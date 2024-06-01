using E_Commerse_Website.Data;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.Services.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerse_Website.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IProductRepo _repo;
        public CustomerController(IProductRepo repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getSliderProductList()
        {
            try
            {
                var p_list = await _repo.GetAllAsync();
                var top3product = p_list.Take(2).Select(p => new
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
    }
}
