using InokerWebApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InokerWebApp.Extensions
{
    public static class SessionExtension
    {
        public static void SerializeCart(this ISession session, string key, Cart cart)
        {
            session.SetString(key, JsonConvert.SerializeObject(cart));
        }
        public static Cart DeserializeCart(this ISession session, string key)
        {
            string jsonString = session.GetString(key);
            if (jsonString != null)
            {
                return JsonConvert.DeserializeObject<Cart>(jsonString);
            }
            else
            {
                return null;
            }

        }
    }
}
