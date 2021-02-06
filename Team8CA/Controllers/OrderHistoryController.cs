﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Team8CA.DataAccess;
using Team8CA.Models;


namespace Team8CA.Controllers
{
    public class OrderHistoryController : Controller
    {


        private readonly AppDbContext db;
        private readonly ShoppingCart shoppingcart;


        public OrderHistoryController(AppDbContext db, ShoppingCart shoppingcart)
        {
            this.db = db;
            this.shoppingcart = shoppingcart;
        }

        public IActionResult Index()
        {
            string customerId = Request.Cookies["customerId"];
            List<Order> orders = db.Order.Where(o => o.CustomerId == customerId).ToList();

            ViewData["order"] = orders;

            ViewData["firstname"] = Request.Cookies["firstname"];
            string sessionid = Request.Cookies["sessionId"];
            ViewData["sessionId"] = sessionid;

            ViewData["customerid"] = customerId;
            List<ShoppingCartItem> shoppingcart = db.ShoppingCartItem.Where(x => x.ShoppingCartId == customerId).ToList();
            List<ShoppingCartItem> shoppingcartNull = db.ShoppingCartItem.Where(x => x.ShoppingCartId == "0").ToList();
            if (sessionid != null)
            {
                ViewData["cartcount"] = shoppingcart.Count;
            }
            else
            {
                ViewData["cartcount"] = shoppingcartNull.Count;
            }
            return View();
        }
        public IActionResult RecentOrder(int orderId)
        {
            Order order = db.Order.First(x => x.OrderId == orderId);
            order.OrderDetails = db.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            foreach (var o in order.OrderDetails)
            {
                o.ActivationCodes = db.ActivationCodes.Where(x => x.OrderId == orderId).ToList();
            }

            string customerId = Request.Cookies["customerId"];
            List<Order> orders = db.Order.Where(o => o.CustomerId == customerId).ToList();

            ViewData["order"] = orders;

            ViewData["firstname"] = Request.Cookies["firstname"];
            string sessionid = Request.Cookies["sessionId"];
            ViewData["sessionId"] = sessionid;
            ViewData["customerid"] = customerId;
            List<Review> cutsomerReviews = db.Reviews.Where(r => r.CustomerId == customerId).ToList();
            List<ShoppingCartItem> shoppingcart = db.ShoppingCartItem.Where(x => x.ShoppingCartId == customerId).ToList();
            List<ShoppingCartItem> shoppingcartNull = db.ShoppingCartItem.Where(x => x.ShoppingCartId == "0").ToList();
            if (sessionid != null)
            {
                ViewData["cartcount"] = shoppingcart.Count;
            }
            else
            {
                ViewData["cartcount"] = shoppingcartNull.Count;
            }

            List<Review> customerReviews = db.Reviews.Where(r => r.CustomerId == customerId).ToList();
            ViewData["customerReviews"] = customerReviews;

            return View(order);
        }
    }
}
