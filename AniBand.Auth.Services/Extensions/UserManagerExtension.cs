using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Domain;
using AniBand.Domain.Enums;
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

        public static IEnumerable<User> GetUsersByField(
            this UserManager<User> manager,
            Expression<Func<User, bool>> filter)
            => manager.Users.Where(filter);

        public static async Task ApproveUserAsync(
            this UserManager<User> manager,
            long id)
        {
            var user = await manager
                .GetByIdAsync(id);
            if (user == null)
            {
                throw new NullReferenceException("User doesnt exist or id is wrong");
            }

            user.Status = Status.Approved;
            await manager.AddClaimAsync(
                user, 
                new Claim(
                    CustomClaimTypes.Permission, 
                    "api.AniBand.User.Approved"));
            await manager.UpdateAsync(user);
        }
        
        public static async Task DeclineUserAsync(
            this UserManager<User> manager,
            long id,
            string message)
        {
            var user = await manager
                .GetByIdAsync(id);
            if (user == null)
            {
                throw new NullReferenceException("User doesnt exist or id is wrong");
            }

            user.Status = Status.Declined;
            user.DeclineMessage = message;
            await manager.UpdateAsync(user);
        }
    }
}