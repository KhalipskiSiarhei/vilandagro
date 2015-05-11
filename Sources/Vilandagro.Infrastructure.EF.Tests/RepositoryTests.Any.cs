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
        public void Any_OfCategoryByKeys_EmptyResult()
        {
            var result = Repo.Any<Category>(x => x.Id == 123);
            Assert.IsFalse(result);
        }

        [Test]
        public void Any_OfProductByKeys_EmptyResult()
        {
            var result = Repo.Any<Product>(x => x.Id == 123);
            Assert.IsFalse(result);
        }

        [Test]
        public void Any_OfSpringProductByKeys_EmptyResult()
        {
            var result = Repo.Any<SpringProduct>(x => x.Id == 123);
            Assert.IsFalse(result);
        }

        [Test]
        public void Any_OfUnitOfPriceByKeys_EmptyResult()
        {
            var result = Repo.Any<UnitOfPrice>(x => x.Id == 123);
            Assert.IsFalse(result);
        }

        [Test]
        public void Any_OfProductPriceByKeys_EmptyResult()
        {
            var result = Repo.Any<ProductPrice>(x => x.Id == 123);
            Assert.IsFalse(result);
        }
    }
}
