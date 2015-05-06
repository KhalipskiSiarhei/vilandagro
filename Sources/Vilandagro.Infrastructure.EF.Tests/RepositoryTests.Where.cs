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
        public void Where_OfCategory_EmptyResult()
        {
            var result = Repo.Where<Category>(c => c.Id == 123456).ToList();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Where_OfProduct_EmptyResult()
        {
            var result = Repo.Where<Product>(c => c.Id == 123456).ToList();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Where_OfSpringProduct_EmptyResult()
        {
            var result = Repo.Where<SpringProduct>(c => c.Id == 123456).ToList();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Where_fUnitOfPrice_EmptyResult()
        {
            var result = Repo.Where<UnitOfPrice>(c => c.Id == 123456).ToList();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void Where_OfProductPrice_EmptyResult()
        {
            var result = Repo.Where<ProductPrice>(c => c.Id == 123456).ToList();
            CollectionAssert.IsEmpty(result);
        }
    }
}