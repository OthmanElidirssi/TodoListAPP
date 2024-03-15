using Microsoft.EntityFrameworkCore;
using TodoListAPP.Models;
using TodoListAPP.Enums;

namespace TodoListAPP.Repositories
{

    public class UserRepository
    {
        private readonly TodoAppContext _context;

        public UserRepository(TodoAppContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<(User? user, AuthenticationResult result)> AuthenticateAsync(string usernameOrEmail, string password)
        {
            var user = await _context.Users
                                    .Include(u => u.Role)
                                    .FirstOrDefaultAsync(u => (u.UserName == usernameOrEmail || u.Email == usernameOrEmail) && u.Password == password);

            if (user== null)
            {
                return (null, AuthenticationResult.UserNotFound);
            }

            if(user.IsActive==false) {
                return (null, AuthenticationResult.UserNotActive) ;
            }

            return (user,AuthenticationResult.Success);
       
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
              
                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "ROLE_USER");
                if (userRole == null)
                { 
                    return false;
                }
                user.Role = userRole;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            { 
                return false;
            }
        }
   

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<User>> GetAllUsersExceptAdminAsync()
        {
            var users = await _context.Users
                .Where(u => u.Role == null || u.Role.RoleName != "ROLE_ADMIN")
                .ToListAsync();

            return users;
        }

        public async Task<bool> ToggleUserStatusAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false; 
            }
            user.IsActive = !user.IsActive;

            try
            {
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
