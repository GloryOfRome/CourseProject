using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseProject.Controllers
{
    public class AdminsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admins
        public ActionResult Index()
        {
            return View();
        }

        //  /Admins/AllRoles
        public ActionResult AllRoles()
        {
            var roles = MembershipHelper.GetAllRoles();
            return View(roles);
        }

        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(string Name)
        {
            MembershipHelper.AddRole(Name);
            return RedirectToAction("AllRoles");
        }

       

        [HttpGet]
        public ActionResult AddUserToRole()
        {
            //selectlist users
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "Email");
            //selectList roles
            ViewBag.role = new SelectList(db.Roles.ToList(), "Name", "Name");

            return View();
        }
        public ActionResult AddUserToRole(string userId, string role)
        {
            //selectlist users
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "Email");
            //selectList roles
            ViewBag.role = new SelectList(db.Roles.ToList(), "Name", "Name");
            //add this user to this role using membershipHelper
            MembershipHelper.AddUserToRole(userId, role);

            return RedirectToAction("AllRoles");
        }

        [HttpGet]
        public ActionResult GetAllRolesOfUser()
        {
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "Email");

            return View();
        }
        [HttpPost]
        public ActionResult GetAllRolesOfUser(string userId)
        {
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "Email");
            var res = MembershipHelper.GetAllRolesOfUser(userId);

            return View(res);
        }

    }
}