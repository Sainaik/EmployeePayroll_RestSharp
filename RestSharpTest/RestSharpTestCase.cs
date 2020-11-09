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
    
    

    [TestClass]
    public class RestSharpTestCase
    {

        //UC1 
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

            Assert.AreEqual( 8, dataResponse.Count);

           
        }

        //UC2
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


        //UC3
        [TestMethod]
        public void givenMutipleEmployees_returnAddedEmployees()
        {
            List<Employee> employee = new List<Employee>();
            employee.Add(new Employee("Anil", "300000"));
            employee.Add(new Employee("Ram", "400000"));

            foreach (var emp in employee)
            {
                RestRequest request = new RestRequest("/employee", Method.POST);

                JObject jObject = new JObject();
                jObject.Add("name", emp.Name);
                jObject.Add("salary", emp.Salary);

                request.AddParameter("application/json", jObject, ParameterType.RequestBody);


                IRestResponse response = client.Execute(request);
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);

            }

            IRestResponse newResponse = getEmployeeList();

            Assert.AreEqual(newResponse.StatusCode, HttpStatusCode.OK);

            List<Employee> dataresponse = JsonConvert.DeserializeObject<List<Employee>>(newResponse.Content);

            Assert.AreEqual(10, dataresponse.Count);

            Console.WriteLine(newResponse.Content);


        }
        
        [TestMethod]
        public void UpdateEmployee_GivenID()
        {
            RestRequest request = new RestRequest("/employee/1", Method.PUT);
            JObject jObject = new JObject();

            jObject.Add("name", "Sai");
            jObject.Add("salary", "5000000");


            request.AddParameter("application/json", jObject, ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            Employee dataresponse = JsonConvert.DeserializeObject<Employee>(response.Content);

            Assert.AreEqual("Sai", dataresponse.Name);
            Assert.AreEqual("5000000", dataresponse.Salary);

        }

       [TestMethod]
        public void givenEmployeeId_deleteEmployee()
        {
            RestRequest request = new RestRequest("/employee/7", Method.DELETE);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            Console.WriteLine(response.Content);
        }



    }
}
