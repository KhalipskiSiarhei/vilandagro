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
        public void First_NoData_ExceptionThrow()
        {
            Assert.Throws<InvalidOperationException>(() => Repo.First<SpringProduct>(p => p.Id == 123));
        }

        [Test]
        public void First_DataExists_FirstItemReturned()
        {
            var categoryToCreate1 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            var categoryToCreate2 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate1);
            Repo.Add(categoryToCreate2);
            Repo.SaveChanges();

            var category = Repo.First<Category>(p => p.Id == categoryToCreate2.Id || p.Id == categoryToCreate1.Id);
            Assert.IsTrue(category.Id == categoryToCreate1.Id);
        }

        [Test]
        public void FirstAsync_NoData_ExceptionThrow()
        {
            Assert.Throws<InvalidOperationException>(async() => await Repo.FirstAsync<SpringProduct>(p => p.Id == 123));
        }

        [Test]
        public async void FirstAsync_DataExists_FirstItemReturned()
        {
            var categoryToCreate1 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            var categoryToCreate2 = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate1);
            Repo.Add(categoryToCreate2);
            Repo.SaveChanges();

            var category = await Repo.FirstAsync<Category>(p => p.Id == categoryToCreate2.Id || p.Id == categoryToCreate1.Id);
            Assert.IsTrue(category.Id == categoryToCreate1.Id);
        }
    }
}