using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NUnit.Framework;
using Vilandagro.Core.Entities;
using Vilandagro.WebApi.Handlers;

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
        public async void GetGet_OneTransactionIsUsed()
        {
            // Act
            var responce1 = await Get("/api/test");
            var responce2 = await Get("/api/test");

            // Asserts
            responce1.EnsureSuccessStatusCode();
            var result = responce1.Content.ReadAsAsync<string[]>().Result;
            Assert.IsTrue(result.Length == 2);
            Assert.IsTrue(result.Contains("value11"));
            Assert.IsTrue(result.Contains("value22"));

            responce2.EnsureSuccessStatusCode();
            result = responce2.Content.ReadAsAsync<string[]>().Result;
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

        [Test]
        public async void AddNewDataSomeTimes()
        {
            var categories1Responce = await Get("/api/test/categories");
            var newCategory1Responce = await Post("/api/test/addNewCategory", new StringContent(string.Empty));
            var newCategory2Responce = await Post("/api/test/addNewCategory", new StringContent(string.Empty));
            var categories2Responce = await Get("/api/test/categories");

            categories1Responce.EnsureSuccessStatusCode();
            newCategory1Responce.EnsureSuccessStatusCode();
            newCategory2Responce.EnsureSuccessStatusCode();
            categories2Responce.EnsureSuccessStatusCode();
            var categories1 = categories1Responce.Content.ReadAsAsync<List<Category>>().Result;
            var newCategory1Id = newCategory1Responce.Content.ReadAsAsync<int>().Result;
            var newCategory2Id = newCategory2Responce.Content.ReadAsAsync<int>().Result;
            var categories2 = categories2Responce.Content.ReadAsAsync<List<Category>>().Result;
            CollectionAssert.IsEmpty(categories1);
            Assert.IsTrue(newCategory1Id > 0);
            Assert.IsTrue(newCategory2Id > 0);
            Assert.IsTrue(categories2.Count - categories1.Count == 2);
            Assert.IsTrue(categories2.Any(c => c.Id == newCategory1Id));
            Assert.IsTrue(categories2.Any(c => c.Id == newCategory2Id));
        }

        [TestCase("PerBatch")]
        [TestCase("PerRequest")]
        [TestCase("")]
        public async void Batch(string transactionPattern)
        {
            //Create a request to query for customers
            HttpRequestMessage categories1Request = new HttpRequestMessage(HttpMethod.Get,
                string.Concat(WebApiStarter.WebApiDefaultAddress, "/api/test/categories"));
            //Create a message to add a customer
            HttpRequestMessage addNewCategoryRequest = new HttpRequestMessage(HttpMethod.Post,
                string.Concat(WebApiStarter.WebApiDefaultAddress, "/api/test/addNewCategory"));
            //Create a request to query for customers
            HttpRequestMessage categories2Request = new HttpRequestMessage(HttpMethod.Get,
                string.Concat(WebApiStarter.WebApiDefaultAddress, "/api/test/categories"));

            HttpMessageContent categories1Content = new HttpMessageContent(categories1Request);
            HttpMessageContent addNewCategoryContent = new HttpMessageContent(addNewCategoryRequest);
            HttpMessageContent categories2Content = new HttpMessageContent(categories2Request);

            MultipartContent batchRequestContent = new MultipartContent("mixed", "batch_" + Guid.NewGuid());
            batchRequestContent.Add(categories1Content);
            batchRequestContent.Add(addNewCategoryContent);
            batchRequestContent.Add(categories2Content);
            if (!string.IsNullOrEmpty(transactionPattern))
            {
                batchRequestContent.Headers.Add(TransactionPerRequestMessageHandler.TransactionPattern,
                    new string[] { transactionPattern });
            }

            var batchResponse = await Post("/api/batch", batchRequestContent);
            MultipartMemoryStreamProvider batchResponseContent = await batchResponse.Content.ReadAsMultipartAsync();
            HttpResponseMessage categories1Rresponse = await batchResponseContent.Contents[0].ReadAsHttpResponseMessageAsync();
            HttpResponseMessage addNewCategoryRresponse = await batchResponseContent.Contents[1].ReadAsHttpResponseMessageAsync();
            HttpResponseMessage categories2Rresponse = await batchResponseContent.Contents[2].ReadAsHttpResponseMessageAsync();

            Assert.IsTrue(categories1Rresponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(addNewCategoryRresponse.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(categories2Rresponse.StatusCode == HttpStatusCode.OK);

            CollectionAssert.IsEmpty(categories1Rresponse.Content.ReadAsAsync<List<Category>>().Result);
            var newCategoryId = addNewCategoryRresponse.Content.ReadAsAsync<int>().Result;
            var categories = categories2Rresponse.Content.ReadAsAsync<List<Category>>().Result;
            CollectionAssert.IsNotEmpty(categories);
            Assert.IsTrue(categories.Any(c => c.Id == newCategoryId));
        }

        [Test]
        public async void Batch_SomeRequestsAreNotSuccess()
        {
            //Create a request to query for customers
            HttpRequestMessage categories1Request = new HttpRequestMessage(HttpMethod.Get,
                string.Concat(WebApiStarter.WebApiDefaultAddress, "/api/test1/categories"));
            //Create a message to add a customer
            HttpRequestMessage addNewCategoryRequest = new HttpRequestMessage(HttpMethod.Post,
                string.Concat(WebApiStarter.WebApiDefaultAddress, "/api/test2/addNewCategory"));
            //Create a request to query for customers
            HttpRequestMessage categories2Request = new HttpRequestMessage(HttpMethod.Get,
                string.Concat(WebApiStarter.WebApiDefaultAddress, "/api/test/categories"));

            HttpMessageContent categories1Content = new HttpMessageContent(categories1Request);
            HttpMessageContent addNewCategoryContent = new HttpMessageContent(addNewCategoryRequest);
            HttpMessageContent categories2Content = new HttpMessageContent(categories2Request);

            MultipartContent batchRequestContent = new MultipartContent("mixed", "batch_" + Guid.NewGuid());
            batchRequestContent.Add(categories1Content);
            batchRequestContent.Add(addNewCategoryContent);
            batchRequestContent.Add(categories2Content);

            var batchResponse = await Post("/api/batch", batchRequestContent);
            MultipartMemoryStreamProvider batchResponseContent = await batchResponse.Content.ReadAsMultipartAsync();
            HttpResponseMessage categories1Rresponse = await batchResponseContent.Contents[0].ReadAsHttpResponseMessageAsync();
            HttpResponseMessage addNewCategoryRresponse = await batchResponseContent.Contents[1].ReadAsHttpResponseMessageAsync();
            HttpResponseMessage categories2Rresponse = await batchResponseContent.Contents[2].ReadAsHttpResponseMessageAsync();

            Assert.IsTrue(categories1Rresponse.StatusCode == HttpStatusCode.NotFound);
            Assert.IsTrue(addNewCategoryRresponse.StatusCode == HttpStatusCode.NotFound);
            Assert.IsTrue(categories2Rresponse.StatusCode == HttpStatusCode.OK);

            CollectionAssert.IsEmpty(categories2Rresponse.Content.ReadAsAsync<List<Category>>().Result);
        }
    }
}