using Gherkin;
using NUnit.Framework;
using RestSharp;
using RestSharp_Fundamentals.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_Fundamentals.StepDefinitions
{
    [Binding, Scope(Tag = "Authentication")]
    public class AuthenticationSteps
    {
        private readonly RestActions _restActions;
        private string _token;

        public AuthenticationSteps(RestActions restActions) {
            _restActions = restActions;
        }

        [Then(@"the response should contain an authentication token")]
        public void ThenTheResponseShouldContainAnAuthenticationToken()
        {
            _token = _restActions.RestResponse.Content;
            Assert.IsFalse(string.IsNullOrEmpty(_token));
        }

    }
}
