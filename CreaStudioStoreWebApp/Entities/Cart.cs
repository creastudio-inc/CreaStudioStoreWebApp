using System;
using System.Collections.Generic;
using System.Linq;

namespace CreaStudioStoreWebApp.Entities
{
    public class Cart : AspnetRun.Core.Entities.Base.EntityBase
    {        
        public string UserName { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(Guid productId, int quantity = 1, string color = "Black", decimal unitPrice = 0)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                Items.Add(
                    new CartItem()
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        Color = color,
                        UnitPrice = unitPrice,
                        TotalPrice = quantity * unitPrice
                    });
            }
        }

        public void RemoveItem(Guid cartItemId)
        {
            var removedItem = Items.FirstOrDefault(x => x.Id == cartItemId);
            if (removedItem != null)
            {
                Items.Remove(removedItem);
            }
        }

        public void RemoveItemWithProduct(Guid productId)
        {
            var removedItem = Items.FirstOrDefault(x => x.ProductId == productId);
            if (removedItem != null)
            {
                Items.Remove(removedItem);
            }
        }

        public void ClearItems()
        {
            Items.Clear();       
        }
    }
}
