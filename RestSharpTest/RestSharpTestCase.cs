using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using EmployeePayrollRestSharp;
using Newtonsoft.Json.Linq;

namespace RestSharpTest
{
    
    //UC1 

    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }
        private IRestResponse getEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/employees", Method.GET);

            //act
            IRestResponse response = client.Execute(request);

            return response;

        }

        [TestMethod]
        public void OncallingGetAPI_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            Assert.AreEqual( 14, dataResponse.Count);

           
        }

       
    }
}
