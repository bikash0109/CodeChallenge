using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var compensation = new Compensation()
            {
                EmployeeId = "BR8376",
                EffectiveDate = Convert.ToDateTime("01/01/2021"),
                Salary = 10
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.EmployeeId);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
        }

        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedEffectiveDate = Convert.ToDateTime("2011-01-01");
            var expectedSalary = 10;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedEffectiveDate, compensation.EffectiveDate);
            Assert.AreEqual(expectedSalary, compensation.Salary);
        }

        [TestMethod]
        public void UpdateCompensation_Returns_Ok()
        {
            // Arrange
            var compensation = new Compensation()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                EffectiveDate = Convert.ToDateTime("2021-10-10"),
                Salary = 2
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/compensation/{compensation.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
            var newCompensation = putResponse.DeserializeContent<Compensation>();

            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
        }

        [TestMethod]
        public void UpdateCompensation_Returns_NotFound()
        {
            // Arrange
            var compensation = new Compensation()
            {
                EmployeeId = "BR8376",
                EffectiveDate = Convert.ToDateTime("01/01/2021"),
                Salary = 20
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/compensation/{compensation.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
