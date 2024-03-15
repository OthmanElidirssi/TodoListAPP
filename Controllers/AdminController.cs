using Microsoft.AspNetCore.Mvc;
using TodoListAPP.Models;
using TodoListAPP.Repositories;
using TodoListAPP.Filters.Action;
namespace TodoListAPP.Controllers
{

    [AuthenticateSession]
    [AdminRole]
    public class AdminController : Controller
    {

        TodoAppContext _context = new TodoAppContext();
        UserRepository userRepo;
        CategoryRepository cateRepo;
        public AdminController()
        {

            userRepo = new UserRepository(_context);
            cateRepo = new CategoryRepository(_context);

        }


        [HttpGet]
        public IActionResult Users()
        {
            List<User> users = userRepo.GetAllUsersExceptAdminAsync().Result;
            return View(users);

        }

        public IActionResult Categories()
        {

            List<Category> categories=cateRepo.GetAllCategoriesAsync().Result;
            return View(categories);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            cateRepo.RemoveCategoryAsync(categoryId).Wait();
            return RedirectToAction("Categories");

        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddCategory(Category category)
        {

            if (!ModelState.IsValid)
            {
                List<string> errorMessages = ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage)
                                            .ToList();
                TempData["errorMessages"] = errorMessages;
                return View();
            }

            TempData["successMessages"] = new String[] { "Category Created Successfully" };

            cateRepo.CreateCategoryAsync(category).Wait();
            return View();
        }


        [HttpPost]
        public IActionResult ToggleUserStatus(int userId)
        {
            bool result=userRepo.ToggleUserStatusAsync(userId).Result;
            return RedirectToAction("Users");
        }
    }

}
