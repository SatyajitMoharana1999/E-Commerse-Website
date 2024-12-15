using E_Commerse_Website.Data;
using E_Commerse_Website.Services.Implimentation;

namespace E_Commerse_Website.Services.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAdminRepo Admin { get; }
        ICustomerRepo Customer { get; }
        ICategoryRepo Category { get; }
        IProductRepo Product { get; }
        IAdminHistoryRepo AdminHistory { get; }
        ISliderRepo SliderRepo { get; }
        ISectionRepo SectionRepo { get; }
        ISliderProductRepo SliderProductRepo { get; }
        ISectionProductRepo SectionProductRepo { get; }

        Task SaveAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly myContext dbContext;
        public IAdminRepo Admin { get; private set; }
        public ICustomerRepo Customer { get; private set; }
        public ICategoryRepo Category { get; private set; }
        public IProductRepo Product { get; private set; }
        public IAdminHistoryRepo AdminHistory { get; private set; }
        public ISliderRepo SliderRepo { get; private set; }
        public ISectionRepo SectionRepo { get; private set; }
        public ISliderProductRepo SliderProductRepo { get; private set; }
        public ISectionProductRepo SectionProductRepo { get; private set; }

        public UnitOfWork(myContext dbContext)
        {
            this.dbContext = dbContext;
            Admin = new AdminRepo(this.dbContext);
            Customer = new CustomerRepo(this.dbContext);
            Category = new CatyegoryRepo(this.dbContext);
            Product = new ProductRepo(this.dbContext);
            AdminHistory = new AdminHistoryRepo(this.dbContext);
            SliderRepo = new SliderRepo(this.dbContext);
            SectionRepo = new SectionRepo(this.dbContext);
            SliderProductRepo = new SliderProductRepo(this.dbContext);
            SectionProductRepo = new SectionProductRepo(this.dbContext);
        }
        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
