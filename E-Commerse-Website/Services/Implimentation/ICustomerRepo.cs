using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.IRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerse_Website.Services.Implimentation
{
    public interface ICustomerRepo : IRepository<Customer>
    {
            
        Task Update(Customer obj);
    }

    public class CustomerRepo : Repository<Customer>, ICustomerRepo
    {
        private readonly myContext _context;
        public CustomerRepo(myContext db) : base(db)
        {
            _context = db;
        }
       
        public async Task Update(Customer obj)
        {
            var data = await _context.tbl_customer.FirstOrDefaultAsync(x => x.customer_id == obj.customer_id);
            if(obj.customer_image != null)
            {
               data.customer_image = obj.customer_image;
            }
            data.customer_name = obj.customer_name;
            data.customer_phone = obj.customer_phone;
            data.customer_email = obj.customer_email;
            data.customer_country = obj.customer_country;
            data.customer_city = obj.customer_city;
            data.customer_address=obj.customer_address;
            data.customer_password=obj.customer_password;
            data.customer_gender = obj.customer_gender;
            
            
        }
    }
}
