using InokerWebApp.Extensions;
using InokerWebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InokerWebApp.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor accessor;
        public CartService(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;
        }
        public Cart ReadCart()
        {
            Cart cart;
            ISession session = accessor.HttpContext.Session;
            if (session.DeserializeCart("Cart") != null)
            {
                cart = session.DeserializeCart("Cart");
            }
            else
            {
                cart = new Cart();
            }
            return cart;
        }
        public void SaveCart(Cart cart)
        {
            accessor.HttpContext.Session.SerializeCart("Cart", cart);
        }
        public void DeleteCart()
        {
            accessor.HttpContext.Session.Clear();
        }
    }
}
