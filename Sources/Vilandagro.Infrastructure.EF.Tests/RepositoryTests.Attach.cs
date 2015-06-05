using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        public void GetEntityId()
        {
            Assert.IsTrue(Repo.GetEntityId(new Category()) == 0);
            Assert.IsTrue(Repo.GetEntityId(new Category() { Id = 10 }) == 10);
            Assert.IsTrue(Repo.GetEntityId(new object()) == -1);
        }

        [Test]
        public void Attach_NewEntityIsAttachedToRepo_EntityIsCreated()
        {
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Attach(categoryToCreate);
            Repo.SaveChanges();

            Assert.IsTrue(categoryToCreate.Id > 0);
            var category = Repo.GetAll<Category>(c => c.Id == categoryToCreate.Id).Single();
            Assert.IsTrue(category.Id == categoryToCreate.Id);
        }

        [Test]
        public void Attach_ExistedEntityIsAttachedToRepoButNotExistedInFact_ExceptionThrew()
        {
            var categoryToCreate = new Category()
            {
                Id = 123,
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Attach(categoryToCreate);
            Assert.Throws<DbUpdateConcurrencyException>(() => Repo.SaveChanges());
        }
    }
}