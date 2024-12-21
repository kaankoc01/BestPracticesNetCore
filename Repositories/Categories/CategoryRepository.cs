using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Categories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        public IQueryable<Category> GetCategoryWithProducts()
        {
            return context.Categories.Include(c => c.Products).AsQueryable();
        }

        public Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
