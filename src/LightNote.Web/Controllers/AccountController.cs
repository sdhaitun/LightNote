using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LightNote.Service;
using Castle.ActiveRecord;

namespace LightNote.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Home/

        public ActionResult LogOn()
        {
            return View();
        }

    }
}
