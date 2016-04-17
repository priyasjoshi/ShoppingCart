using System.Web.Mvc;
using ShoppingCart.Models;
namespace ShoppingCart.Controllers
{
    public class FacebookController : Controller
    {
        [HttpPost]
        public JsonResult FacebookLogin(FacebookLoginButton model)
        {
            Session["uid"] = model.uid;
            Session["accessToken"] = model.accessToken;

            return Json(new { success = true });
        }

    }
}