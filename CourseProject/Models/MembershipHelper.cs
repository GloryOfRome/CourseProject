using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class MembershipHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>
            (new RoleStore<IdentityRole>(db));

        static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
            (new UserStore<ApplicationUser>(db));

        public static List<string> GetAllRoles()
        {
            var result = roleManager.Roles.Select(r => r.Name).ToList();
            return result;
        }

        public static bool CheckIfInRole(string userId, string role)
        {
            var result = userManager.IsInRole(userId, role);
            return result;
        }

        public static bool AddUserToRole(string userId, string role)
        {
            if (CheckIfInRole(userId, role))
                return false;
            else
            {
                userManager.AddToRole(userId, role);
                return true;
            }
        }

        public static void AddRole(string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole { Name = roleName });
            }
        }

        public static List<string> GetAllRolesOfUser(string userId)
        {
            return userManager.GetRoles(userId).ToList();
        }

    }
}