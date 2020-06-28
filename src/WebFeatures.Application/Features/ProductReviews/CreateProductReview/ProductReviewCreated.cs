﻿using System;
using WebFeatures.Application.Interfaces.Events;

namespace WebFeatures.Application.Features.ProductReviews.CreateProductReview
{
    public class ProductReviewCreated : IEvent
    {
        public Guid Id { get; }

        public ProductReviewCreated(Guid id)
        {
            Id = id;
        }
    }
}