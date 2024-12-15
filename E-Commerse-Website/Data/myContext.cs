using E_Commerse_Website.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace E_Commerse_Website.Data
{
    public class myContext:DbContext
    {
        public myContext(DbContextOptions<myContext> options) : base(options) { }
        public DbSet<Admin> tbl_admin { get; set; }
        public DbSet<Customer> tbl_customer { get; set; }
        public DbSet<Product> tbl_product{ get; set; }
        public DbSet<Category> tbl_category{ get; set; }
        public DbSet<Cart> tbl_cart{ get; set; }
        public DbSet<Feedback> tbl_feedback{ get; set; }
        public DbSet<Faqs> tbl_faqs{ get; set; }
        public DbSet<AdminHistory> tbl_adminHistory { get; set; }
		public DbSet<OTP_Customer> tbl_OTP_Customer { get; set; }
		public DbSet<Slider> tbl_Slider { get; set; }
		public DbSet<SliderProduct> tbl_SliderProduct { get; set; }
		public DbSet<Section> tbl_Section { get; set; }
		public DbSet<SectionProduct> tbl_SectionProduct { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Product).HasForeignKey(p=>p.cat_id);
            modelBuilder.Entity<Product>().HasOne(p => p.Admin).WithMany(c => c.Product).HasForeignKey(p=>p.adm_id);
            modelBuilder.Entity<AdminHistory>().HasOne(p => p.Admin).WithMany(c => c.History).HasForeignKey(p=>p.admin_id);
        }
    }
}
