using MySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySite.Controllers
{
    

    public class HomeController : Controller
    {
        MySiteContext context = new MySiteContext();

        public ActionResult Index()
        {
            ViewBag.ListCategory = context.Categories.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}