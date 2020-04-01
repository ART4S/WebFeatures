﻿using System;
using AutoMapper;
using WebFeatures.Application.Infrastructure.Mappings;
using WebFeatures.Domian.Entities;

namespace WebFeatures.Application.Features.Products.GetProduct
{
    public class ProductInfoDto : IHasMappings
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public Guid? PictureId { get; set; }
        public Guid ManufacturerId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid BrandId { get; set; }

        public void ApplyMappings(Profile profile)
        {
            profile.CreateMap<Product, ProductInfoDto>(MemberList.Destination);
        }
    }
}