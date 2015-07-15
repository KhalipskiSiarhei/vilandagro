using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF.Tests
{
    public partial class RepositoryTests
    {
        [Test]
        public async void AddSomeNewDataInAsync_DataIsValid_DataCreated()
        {
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            var productToCreate = new Product()
            {
                Category = categoryToCreate,
                Description = "ProductDescription",
                Image = "ProductImage",
                Name = "ProductName",
            };
            var springProductToCreate = new SpringProduct()
            {
                Category = categoryToCreate,
                Description = "SpringProductDescription",
                Image = "SpringProductImage",
                Name = "SpringProductName",
                Diametr = 10,
                Weight = 20,
            };
            var unitOfPriceToCreate = new UnitOfPrice()
            {
                Description = "UnitOfPriceDescription",
                Name = "UnitOfPriceName",
            };
            var productPriceToCreate = new ProductPrice()
            {
                Product = productToCreate,
                UnitOfPrice = unitOfPriceToCreate,
            };
            var springProductPriceToCreate = new ProductPrice()
            {
                Product = springProductToCreate,
                UnitOfPrice = unitOfPriceToCreate,
            };

            Repo.Add(categoryToCreate);
            Repo.Add(productToCreate);
            Repo.Add(springProductToCreate);
            Repo.Add(unitOfPriceToCreate);
            Repo.Add(productPriceToCreate);
            Repo.Add(springProductPriceToCreate);
            var affectedRowsCount = await Repo.SaveChangesAsync();

            Assert.IsTrue(affectedRowsCount > 0);
            var category = Repo.Where<Category>(x => x.Name == "Name").Single();
            var product = category.Products.Single(p => p.Name == productToCreate.Name);
            var springProduct = category.Products.Single(p => p.Name == springProductToCreate.Name);
            var unitOfPrice = Repo.Where<UnitOfPrice>(x => x.Name == unitOfPriceToCreate.Name);
            var productPrice = product.ProductPrices.Single();
            var springProductPrice = springProduct.ProductPrices.Single();

            Assert.IsNotNull(category);
            Assert.IsNotNull(product);
            Assert.IsNotNull(springProduct);
            Assert.IsNotNull(unitOfPrice);
            Assert.IsNotNull(productPrice);
            Assert.IsNotNull(springProductPrice);
        }

        [Test]
        public void SaveChanges_UpdateExistedData_DataUpdated()
        {
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            Repo.Add(categoryToCreate);
            Repo.SaveChanges();
            Assert.IsTrue(categoryToCreate.Version == 1);

            categoryToCreate.Description = "Hello world!!!";
            Assert.IsTrue(Repo.SaveChanges() > 0);
            Assert.IsTrue(categoryToCreate.Version == 2);

            categoryToCreate.Description = "2345";
            Repo.SaveChanges();
            Assert.IsTrue(categoryToCreate.Version == 3);

            var savedCategory = Repo.GetAll<Category>(c => c.Id == categoryToCreate.Id).Single();
            Assert.IsTrue(savedCategory.Description == "2345");
            Assert.IsTrue(savedCategory.Version == 3);
        }
    }
}