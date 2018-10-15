using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InokerWebApp.Models;
using InokerWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace InokerWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly InokerwebappdbContext db;
        private Cart cart;
        private CartService cService;
        public CartController(InokerwebappdbContext _db, CartService _cService)
        {
            cService = _cService;
            db = _db;
            cart = cService.ReadCart();
        }

        public IActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            foreach (OrderItem item in cart.Items)
            {
                item.AvailableDimensions = db.Products
                    .Where(p => p.ModelId == item.ModelId && p.Stock > 0)
                    .Select(p => p.Width + "x" + p.Height).ToArray();
            }
            return View(cart);
        }
        public IActionResult AddModel(int modelId, string returnUrl = "")
        {
            Model m1 = db.Models.Find(modelId);
            if (m1 != null)
            {
                cart.AddModel(modelId);
                cService.SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public IActionResult AddProduct(Product product, string returnUrl)
        {
            Product p1 = db.Products.Find(product);
            if (p1 != null)
            {
                cart.AddProduct(product, 1);
                cService.SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public IActionResult RemoveItem(int productId, string returnUrl)
        {
            Product p1 = db.Products.Find(productId);

            if (p1 != null)
            {
                cart.RemoveItem(p1);
                cService.SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public IActionResult ChangeItem(int productId, int quantity, string
       returnUrl)
        {
            Product product = db.Products.Find(productId);

            if (product != null)
            {
                cart.ChangeItem(product, quantity);
                cService.SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}