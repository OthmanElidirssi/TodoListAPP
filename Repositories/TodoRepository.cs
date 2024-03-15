using Microsoft.EntityFrameworkCore;
using TodoListAPP.Models;

namespace TodoListAPP.Repositories
{
    public class TodoRepository
    {
        private readonly TodoAppContext _context;

        public TodoRepository(TodoAppContext context)
        {
            _context = context;
        }

        public async Task<List<Todo>> GetAllTodosAsync(int userId)
        {
            List<Todo> todos = await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
            return todos ;
        }


        public async Task<bool> AddTodoAsync(Todo todo, int userId)
        {
            todo.UserId = userId;
            todo.CreatedAt = DateTime.Now;
            try
            {
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Dictionary<string, (int Completed, int NonCompleted)>> GetTodoStatisticsAsync(int userId)
        {
            var distinctCategories = await _context.Categories.Select(c => c.CategoryName).ToListAsync();
            var userTodoStatistics = await _context.Todos
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.Category.CategoryName)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    CompletedCount = g.Count(t => t.CompletedAt != null),
                    NonCompletedCount = g.Count(t => t.CompletedAt == null)
                })
                .ToDictionaryAsync(x => x.CategoryName, x => (x.CompletedCount, x.NonCompletedCount));
            var todoStatistics = distinctCategories.ToDictionary(
                category => category,
                _ => (Completed: 0, NonCompleted: 0)
            );
            foreach (var kvp in userTodoStatistics)
            {
                todoStatistics[kvp.Key] = kvp.Value;
            }

            return todoStatistics;
        }


        public async Task<List<Todo>> GetTodosByCategoryAsync(int userId, string categoryName)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId && t.Category.CategoryName == categoryName)
                .ToListAsync();
        }
        public async Task<List<Todo>> GetCompletedTodosAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId && t.CompletedAt != null)
                .ToListAsync();
        }

        public async Task<List<Todo>> GetNonCompletedTodosAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId && t.CompletedAt == null)
                .ToListAsync();
        }
        public async Task<List<Todo>> GetAllTodosAsync(int userId, string? categoryFilter = null, bool? completedFilter = null, string? sortByPriority = null)
        {
            IQueryable<Todo> query = _context.Todos
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                query = query.Where(t => t.Category.CategoryName == categoryFilter);
            }

            if (completedFilter.HasValue)
            {
                query = query.Where(t => (t.CompletedAt != null) == completedFilter.Value);
            }

            if (!string.IsNullOrEmpty(sortByPriority))
            {
                query = query.OrderBy(t => t.Priority);
            }

            return await query.ToListAsync();
        }


        public async Task<List<string>> GetDistinctCategoriesAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.UserId == userId)
                .Select(t => t.Category.CategoryName)
                .Distinct()
                .ToListAsync();
        }

        public async Task ToggleTaskStateAsync(int taskId)
        {
            var task = await _context.Todos.FindAsync(taskId);
            if (task != null)
            {
                if (task.CompletedAt != null)
                {
                    task.CompletedAt = null; 
                }
                else
                {
                    task.CompletedAt = DateTime.Now; 
                }
                await _context.SaveChangesAsync();
            }
        }



    }
}
