﻿using Bogus;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using WebFeatures.Application.Features.Products.UpdateProduct;
using WebFeatures.Application.Interfaces.Files;
using WebFeatures.Application.Tests.Common.Base;
using WebFeatures.Domian.Entities.Products;
using Xunit;
using ValidationException = WebFeatures.Application.Exceptions.ValidationException;

namespace WebFeatures.Application.Tests.Integration.Features.Products.UpdateProduct
{
    public class UpdateProductCommandTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldUpdateProduct()
        {
            // Arrange
            var faker = new Faker();

            var request = new UpdateProductCommand()
            {
                Id = new Guid("f321a9fa-fc44-47e9-9739-bb4d57724f3e"),
                Name = faker.Lorem.Word(),
                Description = faker.Lorem.Sentences(),
                BrandId = new Guid("f612a3d0-573a-47e5-9f6b-a941f99fb26f"),
                CategoryId = new Guid("03e9c4b2-7587-4640-b376-2437414fd610"),
                ManufacturerId = new Guid("278a79e9-5889-4953-a7c9-448c1e185600"),
                Pictures = new IFile[0]
            };

            // Act
            await Mediator.SendAsync(request);

            Product product = await DbContext.Products.GetAsync(request.Id);

            // Assert
            product.Should().NotBeNull();
            product.Id.Should().Be(request.Id);
            product.Name.Should().Be(request.Name);
            product.MainPictureId.Should().BeNull();
            product.Description.Should().Be(request.Description);
            product.BrandId.Should().Be(request.BrandId);
            product.CategoryId.Should().Be(request.CategoryId);
            product.ManufacturerId.Should().Be(request.ManufacturerId);
        }

        [Fact]
        public void ShouldThrow_WhenInvalidProduct()
        {
            // Arrange
            var request = new UpdateProductCommand();

            // Act
            Func<Task> act = () => Mediator.SendAsync(request);

            // Assert
            act.Should().Throw<ValidationException>().And.Error.Should().NotBeNull();
        }
    }
}