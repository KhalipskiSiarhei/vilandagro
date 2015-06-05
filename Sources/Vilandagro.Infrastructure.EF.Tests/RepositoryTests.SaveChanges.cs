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
        public void SaveChanges_UpdateExisteddata_DataUpdated()
        {
            var categoryToCreate = new Category()
            {
                Description = "CategoryDescription",
                Image = "Image",
                Name = "Name",
            };
            Repo.Add(categoryToCreate);
            Repo.SaveChanges();
            categoryToCreate.Description = "Hello world!!!";

            Repo.SaveChanges();

            var savedCategory = Repo.GetAll<Category>(c => c.Id == categoryToCreate.Id).Single();
            Assert.IsTrue(savedCategory.Description == "Hello world!!!");
            //Assert.IsTrue(savedCategory.Version == 1);
        }
    }
}