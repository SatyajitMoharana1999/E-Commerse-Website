using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.IRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerse_Website.Services.Implimentation
{
    public interface ICartRepo : IRepository<Cart>
    {
        Task Update(Cart obj);
        Task HardDelete(int id);
    }

    public  class CartRepo : Repository<Cart>, ICartRepo
    {
        private readonly myContext _context;
        public CartRepo(myContext db) : base(db)
        {
            _context = db;
        }

        public async Task Update(Cart obj)
        {
            var data = await _context.tbl_cart.FirstOrDefaultAsync(u => u.cart_id == obj.cart_id);
            data.cust_id = obj.cust_id;
            data.cart_status = obj.cart_status;
            data.prod_id = obj.prod_id;
            data.product_quantity = obj.product_quantity;
        }

        public async Task HardDelete(int id)
        {
            var data = await _context.tbl_cart.Where(o => o.cart_id == id).FirstOrDefaultAsync();
            _context.Remove(data);
        }
    }
}
