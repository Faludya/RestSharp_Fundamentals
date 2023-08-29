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
            _restActions.ExecutePostRequest(path);
        }

        [StepDefinition(@"the response status code should be ""([^""]*)""")]
        public void ThenTheResponseStatusCodeShouldBe(int statusCode)
        {
            _restActions.AssertResponseCode((HttpStatusCode)statusCode);
        }
    }
}
