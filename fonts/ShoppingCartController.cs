using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
namespace ShoppingCart.Controllers
{
    public class ShoppingCartController : Controller
    {
        ShoppingCartEntities _db = new ShoppingCartEntities();
        public ActionResult Index()
        {
            return View();
        }
        private int isExisting(int id) {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++) 
                if (cart[i].Product.ProductID == id) 
                    return i;
                return -1;
        }
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            int total_quantity = 0;
            List<Item> cart = new List<Item>();
            if (Session["cart"] == null)
            {
                cart.Add(new Item(_db.ProductTables.Find(id), 1));
            }
            else
            {
                cart = (List<Item>)Session["cart"];
                int index = isExisting(id);
                if (index == -1)
                    cart.Add(new Item(_db.ProductTables.Find(id), 1));
                else
                    cart[index].Quantity++;

            }
            Item[] items = cart.ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                total_quantity += Convert.ToInt32(items[i].Quantity.ToString());
            }
            //ViewBag.TotalQuantity = total_quantity;
            Session["cart"] = cart;
           
            return Json(total_quantity);
            
        }
        public ActionResult CartDetails() {
            if (Session["cart"] == null ){
                ViewBag.Message = "You have no items in your shopping Cart";
            }
            else {
                ViewBag.Message = "";
            }
            return View();
        }
        public ActionResult Checkout()
        {
            return RedirectToAction("LoginFB", "Account");
        }
        public ActionResult RemovefromCart(int id)
        {
            List<Item> cart = new List<Item>();
            cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.ProductID == id)
                {
                    cart.RemoveAt(i);
                }
            if (cart.Count < 1)
            {
                Session["cart"] = null;
            }
            else {
                Session["cart"] = cart;
            }
            return RedirectToAction("CartDetails", "ShoppingCart");
        }
        
    }
}