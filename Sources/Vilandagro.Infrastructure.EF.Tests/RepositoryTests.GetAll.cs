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
        public void GetAll_OfCategory_EmptyResult()
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
    }
}