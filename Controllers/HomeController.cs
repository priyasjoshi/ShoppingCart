using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using Facebook;

namespace ShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        ShoppingCartEntities _db = new ShoppingCartEntities();
        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1", Value = "1", Selected = true });
            items.Add(new SelectListItem { Text = "2", Value = "2" });
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "4", Value = "4" });
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            ViewBag.Quantity = items;
            return View(_db.ProductTables.ToList());
        }
    }
}