using E_Commerse_Website.Data;
using E_Commerse_Website.Services.Implimentation;

namespace E_Commerse_Website.Services.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICustomerRepo Customer { get; }
        ICategoryRepo Category { get; }
        IProductRepo Product { get; }
        IAdminHistoryRepo AdminHistory { get; }
        Task SaveAsync();

    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly myContext dbContext;
        public ICustomerRepo Customer { get; private set; }
        public ICategoryRepo Category { get; private set; }
        public IProductRepo Product { get; private set; }
        public IAdminHistoryRepo AdminHistory { get; private set; }

        public UnitOfWork(myContext dbContext)
        {
            this.dbContext = dbContext;
            Customer = new CustomerRepo(this.dbContext);
            Category = new CatyegoryRepo(this.dbContext);
            Product = new ProductRepo(this.dbContext);
            AdminHistory = new AdminHistoryRepo(this.dbContext);
        }
        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
