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
        public void Delete()
        {
            // Arrange
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate);
            Repo.SaveChanges();
            var category = Repo.Where<Category>(x => x.Name == "Name").Single();
            Assert.IsNotNull(category);

            // Act
            Repo.Delete(category);
            Repo.SaveChanges();

            // Asserts
            category = Repo.Where<Category>(x => x.Name == "Name").SingleOrDefault();
            Assert.IsNull(category);
        }

        [Test]
        public void Delete_TrtyToDeleteAlreadyDeletedEntity_InvalidOperationExceptionThrew()
        {
            // Arrange
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate);
            Repo.SaveChanges();
            var category = Repo.Where<Category>(x => x.Name == "Name").Single();
            Assert.IsNotNull(category);

            // Act
            Repo.Delete(category);
            Repo.SaveChanges();

            // Asserts
            Assert.Throws<InvalidOperationException>(() => Repo.Delete(category));
        }

        [Test]
        public void DeleteEntityById_EntityDoesNotExist_FalseReturned()
        {
            Assert.IsFalse(Repo.DeleteById<Category>(123));
        }

        [Test]
        public void DeleteEntityById_EntityExists_EntityRemovedAndTrueReturned()
        {
            // Arrange
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };

            Repo.Add(categoryToCreate);
            Repo.SaveChanges();
            var category = Repo.Where<Category>(x => x.Name == "Name").Single();
            Assert.IsNotNull(category);

            // Act
            Assert.IsTrue(Repo.DeleteById<Category>(category.Id));
            Repo.SaveChanges();

            // Asserts
            Assert.IsNull(Repo.Find<Category>(category.Id));
        }
    }
}