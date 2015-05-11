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
        public void DeleteRange_DataRemoved()
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

            var category1 = Repo.Where<Category>(x => x.Name == categoryToCreate1.Name).Single();
            var category2 = Repo.Where<Category>(x => x.Name == categoryToCreate2.Name).Single();
            Repo.DeleteRange(new[] { category1, category2 });
            Repo.SaveChanges();
            Assert.IsNull(Repo.Where<Category>(x => x.Name == categoryToCreate1.Name).SingleOrDefault());
            Assert.IsNull(Repo.Where<Category>(x => x.Name == categoryToCreate2.Name).SingleOrDefault());
        }

        [Test]
        public void DeleteRange_DeleteOneItemWhichIsIncorrect_InvalidOperationExceptionThrew()
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

            Repo.Add(categoryToCreate1);
            Repo.SaveChanges();

            var category1 = Repo.Where<Category>(x => x.Name == categoryToCreate1.Name).Single();
            Assert.Throws<InvalidOperationException>(() => Repo.DeleteRange(new[] { category1, categoryToCreate2 }));
            category1 = Repo.Where<Category>(x => x.Name == categoryToCreate1.Name).Single();
            Assert.IsNotNull(category1);
        }
    }
}