using Microsoft.EntityFrameworkCore;
using TodoListAPP.Models;

namespace TodoListAPP.Repositories
{
    public class CategoryRepository
    {
        private readonly TodoAppContext _context;

        public CategoryRepository(TodoAppContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            { 
                return false;
            }
        }
        public async Task<bool> RemoveCategoryAsync(int categoryId)
        {
            try
            {
                var category = new Category { CategoryId = categoryId };
                _context.Entry(category).State = EntityState.Deleted;
                await _context.SaveChangesAsync(); 
                return true;
            }
            catch (Exception)
            {
      
                return false;
            }
        }
    }


}
