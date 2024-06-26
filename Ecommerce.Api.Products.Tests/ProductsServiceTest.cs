using AutoMapper;
using Ecommerce.Api.Products.Db;
using Ecommerce.Api.Products.Profiles;
using Ecommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Ecommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async void GetProductsReturnAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(nameof(GetProductsReturnAllProducts))
            .Options;
            var dbContext = new ProductsDbContext(options);
            Createproducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg=>cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var ProductsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await ProductsProvider.GetProductsAsync();
            Assert.True(product.IsSuccess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async void GetProductsReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(nameof(GetProductsReturnsProductUsingValidId))
            .Options;
            var dbContext = new ProductsDbContext(options);
            Createproducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var ProductsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await ProductsProvider.GetProductAsync(1);
            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async void GetProductsReturnsProductUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(nameof(GetProductsReturnsProductUsingValidId))
            .Options;
            var dbContext = new ProductsDbContext(options);
            Createproducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var ProductsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await ProductsProvider.GetProductAsync(-1);
            Assert.False(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void Createproducts(ProductsDbContext dbContext)
        {
            for (int i=1; i<=10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i+10,
                    Price = (decimal) (i*3.14)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
