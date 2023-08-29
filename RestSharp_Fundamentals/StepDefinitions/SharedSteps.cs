using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp_Fundamentals.Helpers;
using RestSharp_Fundamentals.TestData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_Fundamentals.StepDefinitions
{
    [Binding, Scope(Tag = "API")]
    public class SharedSteps
    {
        private RestActions _restActions;
        private SharedData _sharedData;
        public SharedSteps(RestActions restActions, SharedData sharedData) 
        {
            _restActions = restActions;
            _sharedData = sharedData;
        }

        [Given(@"I setup the payload with the (application json|default)")]
        public void GivenISetupThePayloadWithTheApplicationJson(string type)
        {
            switch (type) 
            {
                //TODO: finish setting up the payload with necessary information
                //need to learn how to add the headers (pair values)
                case "application json":
                    _sharedData.payload = JsonConvert.SerializeObject(BaseConfig.AuthHeader);
                    break;

                default:
                    break;
            }
        }


        [When(@"I make a POST request to the ""([^""]*)"" endpoint")]
        public void WhenIMakeAPOSTRequestToTheEndpoint(string path)
        {
            //TODO: understand what the other parameter does and how to initialize it?
            _restActions.ExecutePostRequest(path, _sharedData.payload);
        }

        [When(@"I make a GET request to the ""([^""]*)"" endpoint")]
        public void WhenIMakeAGETRequestToTheEndpoint(string path)
        {
            _restActions.ExecuteGetRequest(path);
        }

        [StepDefinition(@"the response status code should be ""([^""]*)""")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _restActions.AssertResponseCode((HttpStatusCode)statusCode);
        }

        [Then(@"the response message should be ""([^""]*)""")]
        public void ThenTheResponseMessageShouldBe(string responseMessage)
        {
            StringAssert.AreEqualIgnoringCase(responseMessage, _restActions.RestResponse.Content);
        }
    }
}
