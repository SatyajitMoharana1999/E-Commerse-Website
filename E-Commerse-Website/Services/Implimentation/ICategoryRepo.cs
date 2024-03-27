using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.IRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerse_Website.Services.Implimentation
{
    public interface ICategoryRepo : IRepository<Category>
    { 
        Task Update(Category obj);
        Task SoftDelete(int id);
    }

    public class CatyegoryRepo : Repository<Category>, ICategoryRepo
    {
        private readonly myContext _context;
        public CatyegoryRepo(myContext db) : base(db)
        {
            _context = db;
        }

        public async Task Update(Category obj)
        {
            var data = await _context.tbl_category.Where(o => o.category_id == obj.category_id).FirstOrDefaultAsync();
            data.category_name = obj.category_name;
        }

        public async Task SoftDelete(int id)
        {
            var data = await _context.tbl_category.Where(o => o.category_id == id).FirstOrDefaultAsync();
            data.category_deleted = true;
        }
    }
}
