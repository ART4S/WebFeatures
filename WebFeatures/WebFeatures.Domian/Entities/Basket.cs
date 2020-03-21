﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebFeatures.Domian.Common;

namespace WebFeatures.Domian.Entities
{
    public class Basket : BaseEntity
    {
        public Guid UserId { get; }
        public User User { get; }

        public IReadOnlyCollection<BasketItem> BasketItems => _basketItems.AsReadOnly();
        private readonly List<BasketItem> _basketItems = new List<BasketItem>();

        public Basket(User user)
        {
            UserId = user.Id;
            User = user;
        }

        private Basket() { } // For EF

        public void AddProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            BasketItem existingItem = _basketItems.SingleOrDefault(x => x.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.AddQuantity(1);
                return;
            }

            _basketItems.Add(new BasketItem(this, product));
        }

        public decimal GetTotalPrice()
        {
            decimal total = 0;

            foreach (BasketItem item in _basketItems)
            {
                if (item.Product.Price == null)
                    throw new Exception($"Basket: {Id}. One or more products don't have a price");

                total += item.Product.Price.Value * item.Quantity;
            }

            return total;
        }
    }
}
