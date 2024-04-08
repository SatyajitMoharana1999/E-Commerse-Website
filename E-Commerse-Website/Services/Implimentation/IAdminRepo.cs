using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.IRepo;

namespace E_Commerse_Website.Services.Implimentation
{
    public interface IAdminRepo:IRepository<Admin>
    {
    }
    public class AdminRepo : Repository<Admin>, IAdminRepo
    {
        public AdminRepo(myContext db) : base(db)
        {

        }
    }
}
