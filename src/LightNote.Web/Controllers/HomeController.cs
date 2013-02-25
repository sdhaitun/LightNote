using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LightNote.Service;
using Castle.ActiveRecord;

namespace LightNote.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private UserService userService = new UserService();

        public ActionResult Index()
        {
            var userList = userService.UserDao.GetAll();
            return View(userList);
        }

        public ActionResult Install()
        {
            ActiveRecordStarter.CreateSchema();
            return Content("Install Success!");
        }
    }
}
