using E_Commerse_Website.Data;
using E_Commerse_Website.Models;
using E_Commerse_Website.Services.IRepo;
using Microsoft.EntityFrameworkCore;

namespace E_Commerse_Website.Services.Implimentation
{
    public interface ISliderRepo:IRepository<Slider>
    {
        Task Update(Slider obj);
        Task SoftDelete(int id);
    }
    public class SliderRepo : Repository<Slider>, ISliderRepo
    {
        private readonly myContext _context;
        public SliderRepo(myContext db) : base(db)
        {
            _context = db;
        }

        public async Task SoftDelete(int id)
        {
            var data = await _context.tbl_Slider.Where(s => s.slider_id == id).FirstOrDefaultAsync();
            data.slider_deleted = true;
        }

        public async Task Update(Slider obj)
        {
            var data = await _context.tbl_Slider.Where(s => s.slider_id == obj.slider_id).FirstOrDefaultAsync();
            data.slider_name = obj.slider_name;
            data.slider_description = obj.slider_description;
            data.update_time = obj.update_time;
            data.is_active = obj.is_active;
        }
    }
    

    public interface ISectionRepo:IRepository<Section>
    {
        Task Update(Section obj);
        Task SoftDelete(int id);
    }
    public class SectionRepo : Repository<Section>, ISectionRepo
    {
        private readonly myContext _context;
        public SectionRepo(myContext db) : base(db)
        {
            _context = db;
        }

        public async Task SoftDelete(int id)
        {
            var data = await _context.tbl_Section.Where(s => s.section_id == id).FirstOrDefaultAsync();
            data.section_deleted = true;
        }

        public async Task Update(Section obj)
        {
            var data = await _context.tbl_Section.Where(s => s.section_id == obj.section_id).FirstOrDefaultAsync();
            data.section_name = obj.section_name;
            data.section_description = obj.section_name;
            data.sort_order = obj.sort_order;
            data.update_time = obj.update_time;
            data.is_active = obj.is_active;
        }
    }


    public interface ISliderProductRepo : IRepository<SliderProduct>
    {
        Task Update(SliderProduct obj);
        Task SoftDelete(int id);
    }
    public class SliderProductRepo : Repository<SliderProduct>, ISliderProductRepo
    {
        private readonly myContext _context;
        public SliderProductRepo(myContext db) : base(db)
        {
            _context = db;
        }

        public async Task SoftDelete(int id)
        {
            var data = await _context.tbl_SliderProduct.Where(s => s.slider_product_id == id).FirstOrDefaultAsync();
            data.slider_product_deleted = true;
        }

        public async Task Update(SliderProduct obj)
        {
            var data = await _context.tbl_SliderProduct.Where(s => s.slider_product_id == obj.slider_product_id).FirstOrDefaultAsync();
           
            data.sort_order = obj.sort_order;
            data.is_visible = obj.is_visible;
            data.prod_id = obj.prod_id;
            if (data.slider_id != null)
            {
                data.slider_id = obj.slider_id;
            }
            if (data.product_image != null)
            {
                data.product_image = obj.product_image;
            }
        }
    }

    
    public interface ISectionProductRepo:IRepository<SectionProduct>
    {
        Task Update(SectionProduct obj);
        Task SoftDelete(int id);
    }
    public class SectionProductRepo : Repository<SectionProduct>, ISectionProductRepo
    {
        private readonly myContext _context;
        public SectionProductRepo(myContext db) : base(db)
        {
            _context = db;
        }

        public async Task SoftDelete(int id)
        {
            var data = await _context.tbl_SectionProduct.Where(s => s.section_product_id == id).FirstOrDefaultAsync();
            data.section_product_deleted = true;
        }

        public async Task Update(SectionProduct obj)
        {
            var data = await _context.tbl_SectionProduct.Where(s => s.section_product_id == obj.section_product_id).FirstOrDefaultAsync();
           
            data.sort_order = obj.sort_order;
            data.is_visible = obj.is_visible;
            data.prod_id = obj.prod_id;
            if (data.section_id != null)
            {
                data.section_id = obj.section_id;
            }
        }
    }


}
