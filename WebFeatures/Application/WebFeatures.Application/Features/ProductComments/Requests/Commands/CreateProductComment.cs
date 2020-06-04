﻿using AutoMapper;
using FluentValidation;
using System;
using WebFeatures.Application.Infrastructure.Mappings;
using WebFeatures.Application.Infrastructure.Requests;
using WebFeatures.Application.Interfaces.DataAccess;
using WebFeatures.Domian.Entities;

namespace WebFeatures.Application.Features.ProductComments.Requests.Commands
{
    /// <summary>
    /// Создать комментарий к товару
    /// </summary>
    public class CreateProductComment : ICommand<Guid>, IHasMappings
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Индентификатор родительского комментария
        /// </summary>
        public Guid? ParentCommentId { get; set; }

        public void ApplyMappings(Profile profile)
        {
            profile.CreateMap<CreateProductComment, ProductComment>(MemberList.Source);
        }

        public class Validator : AbstractValidator<CreateProductComment>
        {
            public Validator(IWriteDbContext db)
            {
                RuleFor(x => x.ProductId)
                    .MustAsync(async (x, t) => await db.Products.ExistsAsync(x));

                RuleFor(x => x.Body)
                    .NotEmpty();

                RuleFor(x => x.ParentCommentId)
                    .MustAsync(async (x, t) => await db.ProductComments.ExistsAsync(x.Value))
                    .When(x => x.ParentCommentId.HasValue);
            }
        }
    }
}