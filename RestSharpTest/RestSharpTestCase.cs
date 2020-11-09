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


        //Get Employee List

        [TestMethod]
        public void OncallingGetAPI_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();

            //assert

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            Assert.AreEqual( 14, dataResponse.Count);

           
        }

        // UC2 Add employee json object 
        
        [TestMethod]
        public void GivenEmployee_Post_ShouldReturnAddedEmployee()
        {
            //arrange
            RestRequest request = new RestRequest("/employees", Method.POST);

            JObject jObjectBody = new JObject();
            jObjectBody.Add("Name", "Gayathri");
            jObjectBody.Add("Salary", "900000");

            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);

            //assert

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);


            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);

            Assert.AreEqual("Gayathri", dataResponse.Name);

            Assert.AreEqual("900000", dataResponse.Salary);


        }
        


    }
}
