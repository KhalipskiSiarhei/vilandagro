using System;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF.Tests
{
    [TestFixture]
    public partial class RepositoryTests : TestFixtureBase
    {
        [TestCase(typeof(Category))]
        [TestCase(typeof(Product))]
        [TestCase(typeof(UnitOfPrice))]
        [TestCase(typeof(SpringProduct))]
        [TestCase(typeof(ProductPrice))]
        public void GetAll_DynamicType_EmptyResult(Type typeToGet)
        {
            var result = Repo.GetAll(typeToGet).ToListAsync().Result;
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAll_OfCategory_IsEmptyResult()
        {
            var result = Repo.GetAll<Category>();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAll_OfProduct_EmptyResult()
        {
            var result = Repo.GetAll<Product>();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAll_OfUnitOfPrice_EmptyResult()
        {
            var result = Repo.GetAll<UnitOfPrice>();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAll_OfSpringProduct_EmptyResult()
        {
            var result = Repo.GetAll<SpringProduct>();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAll_OfProductPrice_EmptyResult()
        {
            var result = Repo.GetAll<ProductPrice>();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAll_ByPredicate_EmptyReResult()
        {
            var result = Repo.GetAll<Category>(c => c.Id == 987654).ToList();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetAll_SomeDataAreLocaSomeAreInDb_GotDataFromDbOnly()
        {
            // Create and save first category
            var categoryToCreate1 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate1);
            Repo.SaveChanges();

            // Create first category, but not save it...
            var categoryToCreate2 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate2);

            var categories = Repo.GetAll<Category>().ToList();
            Assert.IsTrue(categories.Count == 1);
            Assert.IsTrue(categories[0].Id == categoryToCreate1.Id);

            Assert.IsTrue(_dbContext.Categories.Local.Count == 2);
            Assert.IsNotNull(_dbContext.Categories.Local.Single(c => c == categoryToCreate1));
            Assert.IsNotNull(_dbContext.Categories.Local.Single(c => c == categoryToCreate2));
        }

        [Test]
        public void GetAll_SomeDataAreLocaSomeAreInDbButWasRemoved_GotAppropriateData()
        {
            // Create and save first category
            var categoryToCreate1 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate1);
            Repo.SaveChanges();

            // Create and save second category
            var categoryToCreate2 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate2);
            Repo.SaveChanges();

            // Create third category, but not save it...
            var categoryToCreate3= new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            Repo.Add(categoryToCreate3);

            // Create forth category, but not save it...
            var categoryToCreate4 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            Repo.Add(categoryToCreate4);

            Repo.Delete(categoryToCreate1);
            Repo.Delete(categoryToCreate3);

            var categories = Repo.GetAll<Category>().ToList();
            Assert.IsTrue(categories.Count == 2);
            Assert.IsNotNull(categories.Single(c => c.Id == categoryToCreate1.Id));
            Assert.IsNotNull(categories.Single(c => c.Id == categoryToCreate2.Id));

            Assert.IsTrue(_dbContext.Categories.Local.Count == 2);
            Assert.IsNotNull(_dbContext.Categories.Local.Single(c => c.Id == categoryToCreate2.Id));
            Assert.IsNotNull(_dbContext.Categories.Local.Single(c => c.Id == categoryToCreate2.Id));
        }
    }
}