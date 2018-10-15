using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InokerWebApp.Models
{
    public class Cart
    {
            private List<OrderItem> itemCollection = new List<OrderItem>();

            public void AddModel(int modelId)
            {
                OrderItem it1 = itemCollection
                .SingleOrDefault(it => it.ModelId == modelId);
                if (it1 == null)
                {
                    it1 = new OrderItem
                    {
                        ModelId = modelId,
                    };
                    itemCollection.Add(it1);
                }
            }

            public void AddProduct(Product product, int quantity)
            {
                var sameModelItems = itemCollection // Orders with same model as product
                .Where(it => it.ModelId == product.ModelId);

                OrderItem it1;

                if (sameModelItems.Count() > 0)
                {
                    it1 = sameModelItems.SingleOrDefault(it => it.Product.ProductId == product.ProductId) ?? sameModelItems.First(); // Order with same product or just first
                    it1.Quantity += quantity;
                }
                else
                {
                    it1 = new OrderItem
                    {
                        ModelId = product.ModelId,
                        Product = product,
                        Quantity = quantity
                    };
                    itemCollection.Add(it1);
                }
            }

            public virtual void RemoveItem(Product product)
            {
                OrderItem st1 = itemCollection.SingleOrDefault(st =>
                    st.Product.ProductId == product.ProductId);
                itemCollection.Remove(st1);
            }

            public void ChangeItem(Product product, int quantity)
            {
                OrderItem st1 = itemCollection.SingleOrDefault(st =>
               st.Product.ProductId == product.ProductId);
                if (st1 != null)
                {
                    st1.Quantity = quantity;
                }
            }
            public virtual decimal CartValue()
            {
                decimal value = itemCollection.Sum(st => st.Product.Price * st.Quantity);
                return value;
            }
            public virtual void DeleteCart()
            {
                itemCollection.Clear();
            }
            public IEnumerable<OrderItem> Items
            {
                get
                {
                    return itemCollection;
                }
            }

    }
}
