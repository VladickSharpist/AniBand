using System.Linq;
using System.Threading.Tasks;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AniBand.Auth.Services.Extensions
{
    public static class UserManagerExtension
    {
        public static User GetById(
            this UserManager<User> manager,
            long id) 
                => manager.Users.FirstOrDefault(u => u.Id == id);
        
        public static async Task<User> GetByIdAsync(
            this UserManager<User> manager,
            long id) 
                => await manager.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}