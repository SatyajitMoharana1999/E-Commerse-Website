using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.IRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerse_Website.Services.Implimentation
{
    public interface IAdminHistoryRepo : IRepository<AdminHistory>
    {
        //Task Update(AdminHistory obj);
        Task SoftDelete(int id);
    }
    public class AdminHistoryRepo : Repository<AdminHistory>, IAdminHistoryRepo
    {
        private readonly myContext _context;
        public AdminHistoryRepo(myContext db) : base(db)
        {
            _context = db;
        }

        //public async Task Update(AdminHistory obj)
        //{
        //    var data = await _context.tbl_adminHistory.Where(o => o.AH_id == obj.AH_id).FirstOrDefaultAsync();
        //    data.AH_title = obj.AH_title;
        //}

        public async Task SoftDelete(int id)
        {
            var data = await _context.tbl_adminHistory.Where(o => o.AH_id == id).FirstOrDefaultAsync();
            data.AH_deleted = true;
        }
    }
}
