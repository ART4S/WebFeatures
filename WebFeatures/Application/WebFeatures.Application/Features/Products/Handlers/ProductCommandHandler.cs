﻿using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebFeatures.Application.Features.Products.Events;
using WebFeatures.Application.Features.Products.Requests.Commands;
using WebFeatures.Application.Infrastructure.Events;
using WebFeatures.Application.Infrastructure.Requests;
using WebFeatures.Application.Infrastructure.Results;
using WebFeatures.Application.Interfaces.DataAccess;
using WebFeatures.Domian.Entities;

namespace WebFeatures.Application.Features.Products.Handlers
{
    internal class ProductCommandHandler :
        IRequestHandler<CreateProduct, Guid>,
        IRequestHandler<UpdateProduct, Empty>
    {
        private readonly IWriteDbContext _db;
        private readonly IEventStorage _events;
        private readonly IMapper _mapper;

        public ProductCommandHandler(IWriteDbContext db, IEventStorage events, IMapper mapper)
        {
            _db = db;
            _events = events;
            _mapper = mapper;
        }

        public async Task<Guid> HandleAsync(CreateProduct request, CancellationToken cancellationToken)
        {
            Product product = _mapper.Map<Product>(request);

            await _db.Products.CreateAsync(product);

            _events.Add(new ProductCreated(product.Id));

            return product.Id;
        }

        public async Task<Empty> HandleAsync(UpdateProduct request, CancellationToken cancellationToken)
        {
            Product product = await _db.Products.GetAsync(request.Id);

            _mapper.Map(request, product);

            await _db.Products.UpdateAsync(product);

            return Empty.Value;
        }
    }
}