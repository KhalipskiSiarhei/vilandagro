using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        public void AddCategory_DataValid_CategoryCreated()
        {
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate);
            Assert.IsTrue(categoryToCreate.Id == 0);
            Repo.SaveChanges();
            Assert.IsTrue(categoryToCreate.Id > 0);
            var category = Repo.Where<Category>(x => x.Name == "Name").Single();
            Assert.IsTrue(category.Id == categoryToCreate.Id);
            Assert.IsTrue(category.Description == categoryToCreate.Description);
            Assert.IsTrue(category.Image == categoryToCreate.Image);
            Assert.IsTrue(category.Name == categoryToCreate.Name);
        }

        [Test]
        public void AddSomeNewData_DataIsValid_DataCreated()
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
            Repo.SaveChanges();

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

            // Get products via polymorphic query
            var products = Repo.GetAll<Product>().ToList();
            Assert.IsTrue(products.Count == 2);
            var dbProduct = products.SingleOrDefault(p => p.Id == product.Id);
            var dbSpringProduct = (SpringProduct)products.SingleOrDefault(p => p.Id == springProduct.Id);
            Assert.IsNotNull(dbProduct);
            Assert.IsNotNull(dbSpringProduct);
        }

        [TestCase(null, "Description", "Image")]
        [TestCase("TooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongNameTooLongName", "Description", "Image")]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void AddNewCategory_DataIsInvalid_ExceptionThrow(string name, string description, string image)
        {
            var categoryToCreate = new Category()
            {
                Description = description,
                Image = image,
                Name = name,
            };

            Repo.Add(categoryToCreate);
            Repo.SaveChanges();
        }
    }
}