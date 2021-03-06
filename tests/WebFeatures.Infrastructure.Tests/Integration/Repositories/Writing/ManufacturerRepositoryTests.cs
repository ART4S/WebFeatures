﻿using Dapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFeatures.Domian.Entities;
using WebFeatures.Infrastructure.DataAccess.Executors;
using WebFeatures.Infrastructure.DataAccess.Mappings;
using WebFeatures.Infrastructure.DataAccess.Mappings.Profiles;
using WebFeatures.Infrastructure.DataAccess.Repositories.Writing;
using WebFeatures.Infrastructure.Tests.Common.Base;
using WebFeatures.Infrastructure.Tests.Common.Stubs.Entities;
using Xunit;

namespace WebFeatures.Infrastructure.Tests.Integration.Repositories.Writing
{
    public class ManufacturerRepositoryTests : IntegrationTestBase
    {
        private ManufacturerWriteRepository CreateDefaultRepository()
        {
            var profile = new EntityProfile();

            profile.TryRegisterMap(typeof(ManufacturerMap));

            return new ManufacturerWriteRepository(Database.Connection, new DapperDbExecutor(), profile);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsNonEmptyCollection()
        {
            // Arrange
            ManufacturerWriteRepository sut = CreateDefaultRepository();

            // Act
            IEnumerable<Manufacturer> manufacturers = await sut.GetAllAsync();

            // Assert
            manufacturers.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetByIdAsync_WhenManufacturerExists_ReturnsManufacturer()
        {
            // Arrange
            ManufacturerWriteRepository sut = CreateDefaultRepository();

            Guid manufacturerId = new Guid("278a79e9-5889-4953-a7c9-448c1e185600");

            Guid cityId = new Guid("b27a7a05-d61f-4559-9b04-5fd282a694d3");

            // Act
            Manufacturer manufacturer = await sut.GetAsync(manufacturerId);

            // Assert
            manufacturer.Should().NotBeNull();
            manufacturer.Id.Should().Be(manufacturerId);
            manufacturer.StreetAddress.Should().NotBeNull();
            manufacturer.StreetAddress.CityId.Should().Be(cityId);
            manufacturer.StreetAddress.StreetName.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenManufacturerDoesntExist_ReturnsNull()
        {
            // Arrange
            ManufacturerWriteRepository sut = CreateDefaultRepository();

            // Act
            Manufacturer manufacturer = await sut.GetAsync(Guid.NewGuid());

            // Assert
            manufacturer.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_CreatesOneManufacturer()
        {
            // Arrange
            ManufacturerWriteRepository sut = CreateDefaultRepository();

            Manufacturer manufacturer = new ManufacturerStub();

            Task<int> GetManufacturersCount() => Database.Connection.ExecuteScalarAsync<int>(
                sql: @"SELECT COUNT(*) FROM public.manufacturers WHERE id = @Id AND streetaddress_cityid = @CityId",
                param: new
                {
                    manufacturer.Id,
                    manufacturer.StreetAddress.CityId
                });

            // Act
            int manufacturersCountBefore = await GetManufacturersCount();

            await sut.CreateAsync(manufacturer);

            int manufacturersCountAfter = await GetManufacturersCount();

            // Assert
            manufacturersCountBefore.Should().Be(0);
            manufacturersCountAfter.Should().Be(1);
        }
    }
}