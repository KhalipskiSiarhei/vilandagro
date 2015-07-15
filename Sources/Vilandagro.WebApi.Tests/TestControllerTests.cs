using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NUnit.Framework;

namespace Vilandagro.WebApi.Tests
{
    [TestFixture]
    public class TestControllerTests : WebApiControllerTestFixtureBase
    {
        [Test]
        public async void Get()
        {
            // Act
            var responce = await Get("/api/test");

            // Asserts
            responce.EnsureSuccessStatusCode();
            var result = responce.Content.ReadAsAsync<string[]>().Result;
            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result.Contains("value11"));
            Assert.IsTrue(result.Contains("value22"));
        }

        [Test]
        public async void Exception_ShouldBeHandledAsInternalServerErrorWithoutContent()
        {
            var responce = await Get("/api/test/exception");

            Assert.IsTrue(responce.StatusCode == HttpStatusCode.InternalServerError);
            Assert.IsNull(responce.Content.ReadAsAsync<string>().Result);
        }

        [Test]
        public async void BusinessException_ShouldBeHandledAsBadRequestWithErrorMessage()
        {
            // Act
            var responce = await Get("/api/test/businessexception");

            // Asserts
            Assert.IsTrue(responce.StatusCode == HttpStatusCode.BadRequest);
            var httpError = responce.Content.ReadAsAsync<HttpError>().Result;
            Assert.IsTrue(httpError.Count == 1);
            Assert.IsTrue(httpError.Message == "Business exception");
            Assert.IsNull(httpError.ExceptionMessage);
            Assert.IsNull(httpError.ExceptionType);
            Assert.IsNull(httpError.StackTrace);
            Assert.IsNull(httpError.ModelState);
            Assert.IsNull(httpError.InnerException);
            Assert.IsNull(httpError.MessageDetail);
        }

        [Test]
        public async void NotFoundException_ShouldBeHandledAsBadRequestWithErrorMessage()
        {
            // Act
            var responce = await Get("/api/test/notfoundexception");

            // Asserts
            Assert.IsTrue(responce.StatusCode == HttpStatusCode.NotFound);
            var httpError = responce.Content.ReadAsAsync<HttpError>().Result;
            Assert.IsTrue(httpError.Count == 1);
            Assert.IsTrue(httpError.Message == "Not found exception");
            Assert.IsNull(httpError.ExceptionMessage);
            Assert.IsNull(httpError.ExceptionType);
            Assert.IsNull(httpError.StackTrace);
            Assert.IsNull(httpError.ModelState);
            Assert.IsNull(httpError.InnerException);
            Assert.IsNull(httpError.MessageDetail);
        }

        [Test]
        public async void InvalidModelState_ShouldBeHandledAsBadRequestWithErrorMessage()
        {
            // Act
            var responce = await Get("/api/test/modelStateException");

            // Asserts
            Assert.IsTrue(responce.StatusCode == HttpStatusCode.BadRequest);
            var httpError = responce.Content.ReadAsAsync<HttpError>().Result;
            Assert.IsTrue(httpError.Count == 2);
            Assert.IsTrue(httpError.Message == "The request is invalid.");
            Assert.IsTrue(httpError["ModelState"].ToString().Contains("field1"));
            Assert.IsTrue(httpError["ModelState"].ToString().Contains("Error message1"));
            Assert.IsTrue(httpError["ModelState"].ToString().Contains("field2"));
            Assert.IsTrue(httpError["ModelState"].ToString().Contains("Error message2"));
            Assert.IsNull(httpError.ModelState); //???
            Assert.IsNull(httpError.ExceptionMessage);
            Assert.IsNull(httpError.ExceptionType);
            Assert.IsNull(httpError.StackTrace);
            Assert.IsNull(httpError.InnerException);
            Assert.IsNull(httpError.MessageDetail);
        }
    }
}
