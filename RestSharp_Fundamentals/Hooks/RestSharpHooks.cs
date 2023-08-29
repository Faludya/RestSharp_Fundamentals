using BoDi;
using Newtonsoft.Json.Bson;
using RestSharp;
using RestSharp_Fundamentals.Helpers;
using RestSharp_Fundamentals.TestData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharp_Fundamentals.Hooks
{
    [Binding, Scope(Tag = "API")]
    public class RestSharpHooks
    {
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();
        private readonly IObjectContainer _objectContainer;

        private RestActions _restActions;
        private SharedData _sharedData;

        public RestSharpHooks(IObjectContainer objectContainer, SharedData sharedData)
        {
            _objectContainer = objectContainer;
            _sharedData = sharedData;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _log.Info("Selected url: " + BaseConfig.BaseUrl);

            RestActions restActions = new RestActions(BaseConfig.BaseUrl);
            _objectContainer.RegisterInstanceAs<RestActions>(restActions);
        }
    }
}
