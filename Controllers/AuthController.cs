using Microsoft.AspNetCore.Mvc;
using TodoListAPP.Models;
using TodoListAPP.Models.dtos;
using TodoListAPP.Repositories;
using TodoListAPP.Filters.Action;

namespace TodoListAPP.Controllers
{
    public class AuthController : Controller
    {
        private readonly TodoAppContext _context=new TodoAppContext();
        UserRepository userRepo;


        public AuthController()
        {
            userRepo = new UserRepository(_context);

        }

        

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                List<string> errorMessages = ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage)
                                            .ToList();
                TempData["errorMessages"]=errorMessages;
                return RedirectToAction("Login");
            }

            var (user,result)=userRepo.AuthenticateAsync(login.UserName,login.Password).Result;
            switch (result)
            {
                case Enums.AuthenticationResult.Success:

                    //TempData["successMessages"] = new String[] { "User Autneticated Successfully" };
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserRole",user.Role.RoleName );


                    if (user.Role.RoleName.Equals("ROLE_ADMIN"))
                    {
                        return Redirect("~/Admin/Users");
                    }
                    else
                    {
                        return Redirect("~/User/Home");
                    }
                    break;

                case Enums.AuthenticationResult.UserNotActive:
                    TempData["errorMessages"] = new String[] { "This User is Not Active" };

                    break;

                case Enums.AuthenticationResult.UserNotFound:

                    TempData["errorMessages"] = new String[] { "UserName or Password is Incorrect" };

                    break;

                default:
                    break;
            }



            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
             return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
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

            bool created=userRepo.CreateUserAsync(user).Result;
            if (created)
            {
                TempData["successMessages"] = new String[] { "User Created Successfully" };
                return RedirectToAction("Login");
            }
            else
            {
                TempData["errorMessages"] = new String[] { "An Erro Occured While Creating the User" };
                return View();
            }

   
        }

       [HttpGet]
       [AuthenticateSession]
       public IActionResult Logout()
        {
            TempData["successMessages"] = new String[] { "User Logged out Successfully" };
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }


 
}
