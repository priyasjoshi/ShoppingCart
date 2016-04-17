using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using Facebook;
using ShoppingCart.Models;
using System.Dynamic;
using System.Configuration;
using System;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        private static string AppID = ConfigurationManager.AppSettings["FacebookAppId"].ToString();
        private static string AppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"].ToString();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginFB() {
            var returnUri = new UriBuilder(Request.Url)
            {
                Path = Url.Action("facebookAuthenticated", "Account")
            };
            var client = new FacebookClient();
            var fbLoginUrl = client.GetLoginUrl(new
            {
                client_id = AppID,
                redirect_uri = returnUri.Uri.AbsoluteUri,
                response_type = "code",
                scope = "email,user_friends"
            });
            return Redirect(fbLoginUrl.ToString());
            
        }
        public ActionResult facebookAuthenticated(String returnUrl)
        {

            var redirectUri = new UriBuilder(Request.Url)
            {
                Path = Url.Action("facebookAuthenticated", "Account")
            };
            var client = new FacebookClient();
            var oauthResult = client.ParseOAuthCallbackUrl(Request.Url);
            dynamic result = client.Get("/oauth/access_token", new
            {
                client_id = AppID,
                redirect_uri = redirectUri.Uri.AbsoluteUri,
                client_secret = AppSecret,
                code = oauthResult.Code,
            });
            var accessToken = result.access_token;
            Session["accessToken"] = accessToken;
            DateTime expires = DateTime.UtcNow.AddSeconds(Convert.ToDouble(result.expires));
            dynamic me = client.Get("/me",
                                    new
                                    {
                                        fields = "name,email,id",
                                        access_token = accessToken
                                    });
            dynamic friends = client.Get("/me/friends", new {
                                        fields = "name,id",
                                        access_token = accessToken
                                    });
            var data = friends["data"].ToString();
            List<InfoModel> info = new List<InfoModel>();
            info.Add(new InfoModel(Convert.ToInt64(me.id),me.name, me.email));
            Session["info"] = info;
            friends.friendsListing = JsonConvert.DeserializeObject<List<FacebookFriend>>(data);
            List<string> id = new List<string>();
            foreach (var items in friends.friendsListing) {
                id.Add(items.id);
            }
            Session["notification"] = id;
            return View("OrderDetails");
        }
        public ActionResult OrderDetails()
        {
            var model = new Order_Details();
            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrderwithHelper(Order_Details orderdetails)
        {
            decimal total = 0.0M;
            List<Item> cart = new List<Item>();
            List<InfoModel> info = new List<InfoModel>();
            UserInfo uf = new UserInfo();
            if (Session["cart"] != null)
            {
                cart = (List<Item>)Session["cart"];
                for (int j = 0; j < cart.Count; j++)
                {
                    float sub_total = (float)(cart[j].Product.ProductPrice * cart[j].Quantity);
                    total += total + (Decimal)sub_total;
                }
            }
            orderdetails.OrderTotal = total;
            if (Session["info"] != null)
            {
                info = (List<InfoModel>)Session["info"];
                for (int i = 0; i < info.Count; i++)
                {
                    orderdetails.UserID = info[i].Id;
                    uf.Name = info[i].Name;
                    uf.Email = info[i].Email;
                }
            }
            ShoppingCartEntities sc = new ShoppingCartEntities();
            sc.Insert_shoppingInfo(orderdetails.UserID, uf.Name,
            uf.Email, orderdetails.FirstName, orderdetails.LastName,
            orderdetails.Address, orderdetails.City, orderdetails.State, orderdetails.PostalCode,
            orderdetails.Country, orderdetails.Phone, orderdetails.OrderTotal);
            cart = (List<Item>)Session["cart"];
            for (int j = 0; j < cart.Count; j++)
            {
                float sub_total = (float)(cart[j].Product.ProductPrice * cart[j].Quantity);
                sc.Insert_shoppingCart(cart[j].Product.ProductID, cart[j].Quantity, sub_total);
            }
            List<string> notification = new List<string>();
            notification = (List<string>)Session["notification"];
                for (int i = 0; i < notification.Count; i++) {
                    SendNotification(notification[i]);
                }
            return View("OrderComplete");
        }
        public ActionResult SendNotification(string friendID) {
            var client = new FacebookClient();
            dynamic result = client.Get("oauth/access_token", new
            {
                client_id = AppID,
                client_secret = AppSecret,
                grant_type = "client_credentials"
            });
            client.AccessToken = result.access_token;
            dynamic nparams = new ExpandoObject();
            nparams.access_token = client.AccessToken;
            nparams.template = "testing 123.........";
            nparams.href = "http://localhost:65366/";
            var response = client.Post(" https://graph.facebook.com/" + friendID + "/notifications", nparams);
            return View();
        }
      
    }
}