using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public void Find_OfCategoryByKeys_EmptyResult()
        {
            var result = Repo.Find<Category>(123);
            Assert.IsNull(result);
        }

        [Test]
        public void Find_OfProductByKeys_EmptyResult()
        {
            var result = Repo.Find<Product>(123);
            Assert.IsNull(result);
        }

        [Test]
        public void Find_OfSpringProductByKeys_EmptyResult()
        {
            var result = Repo.Find<SpringProduct>(123);
            Assert.IsNull(result);
        }

        [Test]
        public void Find_OfUnitOfPriceByKeys_EmptyResult()
        {
            var result = Repo.Find<UnitOfPrice>(123);
            Assert.IsNull(result);
        }

        [Test]
        public void Find_OfProductPriceByKeys_EmptyResult()
        {
            var result = Repo.Find<ProductPrice>(123);
            Assert.IsNull(result);
        }

        [Test]
        public async void FindAsync_OfCategoryByKeys_EmptyResult()
        {
            var result = await Repo.FindAsync<Category>(123);
            Assert.IsNull(result);
        }
    }
}