using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Vilandagro.Core;
using Vilandagro.Core.Entities;
using Vilandagro.Core.Exceptions;

namespace Vilandagro.WebApi.Controllers
{
    public class TestController : ApiController
    {
        private readonly IRepository _repository;

        public TestController(IRepository repository)
        {
            _repository = repository;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<string[]> Get()
        {
            return await Task.FromResult(new [] { "value11", "value22" });
        }

        [HttpGet]
        [Route("api/test/exception")]
        public void Exception()
        {
            throw new Exception();
        }

        [HttpGet]
        [Route("api/test/businessException")]
        public void BusinessException()
        {
            throw new BusinessException("Business exception");
        }

        [HttpGet]
        [Route("api/test/notFoundException")]
        public void NotFoundException()
        {
            throw new NotFoundException("Not found exception");
        }

        [HttpGet]
        [Route("api/test/modelStateException")]
        public void ModeStateException()
        {
            ModelState.AddModelError("field1", "Error message1");
            ModelState.AddModelError("field2", "Error message2");
            throw new ModelStateException(ModelState);
        }

        [HttpPost]
        [Route("api/test/addNewCategory")]
        public async Task<int> AddNewCategory()
        {
            var newCategory = new Category() { Description = "Description", Image = "Image", Name = "Name" };
            _repository.Add(newCategory);
            await _repository.SaveChangesAsync();

            return newCategory.Id;
        }

        [HttpGet]
        [Route("api/test/categories")]
        public async Task<List<Category>> GetCategories()
        {
            return await _repository.GetAll<Category>().ToListAsync();
        }
    }
}