﻿using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using WebFeatures.Application.Interfaces.DataContext;
using WebFeatures.Domian.Entities;
using WebFeatures.Requests;

namespace WebFeatures.Application.Features.Products.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProduct, ProductInfoDto>
    {
        private readonly IReadContext _db;
        private readonly IMapper _mapper;

        public GetProductHandler(IReadContext db, IMapper mapper)
            => (_db, _mapper) = (db, mapper);

        public async Task<ProductInfoDto> HandleAsync(GetProduct request, CancellationToken cancellationToken)
        {
            Product product = await _db.GetByIdAsync<Product>(request.Id);
            return _mapper.Map<ProductInfoDto>(product);
        }
    }
}
