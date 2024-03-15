using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TodoListAPP.Filters.Action;
using TodoListAPP.Models;
using TodoListAPP.Repositories;


namespace TodoListAPP.Controllers
{

    [AuthenticateSession]
    [UserRole]
    public class UserController : Controller
    {

        TodoAppContext _context=new TodoAppContext();
        TodoRepository todoRepository;

        public UserController() { 

            todoRepository=new TodoRepository(_context);
        
        }
        public IActionResult Home()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName");
            var todoStatistics = todoRepository.GetTodoStatisticsAsync(int.Parse(userId)).Result;
            ViewData["UserId"] = userId;
            ViewData["UserName"] = userName;

            return View(todoStatistics);
        }

        public IActionResult Tasks(string? categoryFilter = null, bool? completedFilter = null, string? sortByPriority = null)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var categories = todoRepository.GetDistinctCategoriesAsync(int.Parse(userId)).Result;
            ViewData["Categories"] = categories;

            var todos = todoRepository.GetAllTodosAsync(int.Parse(userId), categoryFilter, completedFilter, sortByPriority).Result;

            return View(todos);
        }

        [HttpPost]
        public IActionResult ToggleTaskState(int taskId)
        {
            todoRepository.ToggleTaskStateAsync(taskId).Wait(); 
            return RedirectToAction("Tasks");
        }


        [HttpGet]
        public IActionResult AddTask()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var categories = _context.Categories.ToList();

            ViewData["Categories"] = categories;

            return View();
        }

        [HttpPost]
        public  IActionResult AddTask(Todo todo)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("UserId");
                bool result=todoRepository.AddTodoAsync(todo,int.Parse(userId)).Result;
                return RedirectToAction("Tasks");
            }
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = categories;
            return View(todo);
        }


    }
}
