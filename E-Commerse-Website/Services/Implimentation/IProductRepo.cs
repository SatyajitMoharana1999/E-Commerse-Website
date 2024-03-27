using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.IRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerse_Website.Services.Implimentation
{
    public interface IProductRepo : IRepository<Product>
    {
        Task Update(Product obj);
        Task SoftDelete(int id);
    }

    public class ProductRepo : Repository<Product>, IProductRepo
    {
        private readonly myContext _context;
        public ProductRepo(myContext db) : base(db)
        {
            _context = db;
        }

        public async Task Update(Product obj)
        {
            var data = await _context.tbl_product.FirstOrDefaultAsync(x => x.product_id == obj.product_id);
            if (obj.product_image != null)
            {
                data.product_image = obj.product_image;
            }
            data.product_name = obj.product_name;
            data.product_price = obj.product_price;
            data.product_description = obj.product_description;
            data.cat_id = obj.cat_id;
        }
        public async Task SoftDelete(int id)
        {
            var data = await _context.tbl_product.Where(o => o.product_id == id).FirstOrDefaultAsync();
            data.product_deleted = true;
        }
    }
}
