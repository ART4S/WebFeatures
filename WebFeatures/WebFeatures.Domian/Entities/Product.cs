﻿using System;
using WebFeatures.Domian.Common;
using WebFeatures.Domian.Exceptions;

namespace WebFeatures.Domian.Entities
{
    public class Product : BaseEntity, IUpdatable
    {
        public string Name { get; set; }
        public decimal? Price { get; private set; }
        public string Description { get; set; }

        public Guid? PictureId { get; set; }
        public Picture Picture { get; set; }

        public Guid ManufacturerId { get; }
        public Manufacturer Manufacturer { get; }

        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid BrandId { get; }
        public Brand Brand { get; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Product(string name, string description, Guid manufacturerId, Guid brandId)
        {
            Name = name;
            Description = description;
            ManufacturerId = manufacturerId;
            BrandId = brandId;
        }

        private Product() { } // For EF

        public void SetPrice(decimal price)
        {
            if (price <= 0)
                throw new DomianValidationException("Price shouldn't be less than or equals 0");

            Price = price;
        }

        public void RemovePrice()
        {
            Price = null;
        }
    }
}
