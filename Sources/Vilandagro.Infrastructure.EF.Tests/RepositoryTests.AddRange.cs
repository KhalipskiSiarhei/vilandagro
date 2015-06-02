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
        public void AddRange_DataValid_DataAdded()
        {
            var categoryToCreate1 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            var categoryToCreate2 = new Category()
            {
                Description = "CategoryDescription1",
                Image = "Image1",
                Name = "Name1",
            };

            Repo.AddRange(new[] { categoryToCreate1, categoryToCreate2 });
            Repo.SaveChanges();
            Assert.IsTrue(categoryToCreate1.Id > 0);
            Assert.IsTrue(categoryToCreate2.Id > 0);

            var category1 = Repo.Where<Category>(x => x.Name == categoryToCreate1.Name).Single();
            Assert.IsTrue(category1.Id == categoryToCreate1.Id);
            Assert.IsTrue(category1.Description == categoryToCreate1.Description);
            Assert.IsTrue(category1.Image == categoryToCreate1.Image);
            Assert.IsTrue(category1.Name == categoryToCreate1.Name);

            var category2 = Repo.Where<Category>(x => x.Name == categoryToCreate2.Name).Single();
            Assert.IsTrue(category2.Id == categoryToCreate2.Id);
            Assert.IsTrue(category2.Description == categoryToCreate2.Description);
            Assert.IsTrue(category2.Image == categoryToCreate2.Image);
            Assert.IsTrue(category2.Name == categoryToCreate2.Name);
        }

        [Test]
        public void AddRange_DataInvalid_DataWasNotAdded()
        {
            var categoryToCreate1 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            var categoryToCreate2 = new Category()
            {
                Description = "CategoryDescription1",
                Image = "Image1",
                Name = null,
            };

            Repo.AddRange(new[] { categoryToCreate1, categoryToCreate2 });
            Assert.Throws<DbEntityValidationException>(() => Repo.SaveChanges());
            Assert.IsTrue(categoryToCreate1.Id == 0);
            Assert.IsTrue(categoryToCreate2.Id == 0);
        }
    }
}