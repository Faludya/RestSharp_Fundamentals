using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Runtime.CompilerServices;

namespace RestSharp_Fundamentals.Helpers
{
    public class RestActions
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RestClient RestClient { get; }
        public RestRequest RestRequest { get; set; }
        public RestResponse RestResponse { get; set; }
        public RestClientOptions RestClientOptions { get; }


        public RestActions(string baseUrl)
        {
            RestClientOptions = new RestClientOptions(baseUrl);
            RestClient = new RestClient(RestClientOptions);
        }

        //TODO: check if I need to remove the ="" 
        public RestActions ExecutePostRequest(string resource, string jsonString = "", ICollection<KeyValuePair<string, string>> headers = null)
        {
            RestRequest = new RestRequest(resource, Method.Post);
            if(headers!= null)
            {
                RestRequest.AddHeaders(headers); //TODO: what do the headers do? what addition do they bring
            }

            RestRequest.AddStringBody(jsonString, DataFormat.Json);
            RestResponse = RestClient.ExecutePostAsync(RestRequest).GetAwaiter().GetResult();
            Log.Debug(GetLoggableRequest());

            return this;
        }

        public RestActions ExecuteGetRequest(string resource, string jsonString="", ICollection<KeyValuePair<string, string>> headers = null)
        {
            RestRequest = new RestRequest(resource, Method.Get);
            if (headers != null)
            {
                RestRequest.AddHeaders(headers); //TODO: what do the headers do? what addition do they bring
            }

            RestRequest.AddStringBody(jsonString, DataFormat.Json);
            RestResponse = RestClient.ExecuteGetAsync(RestRequest).GetAwaiter().GetResult();
            Log.Debug(GetLoggableRequest());

            return this;
        }

        public RestActions AssertResponseCode(HttpStatusCode statusCode = HttpStatusCode.OK) //TODO: why is it like this? Because it is most common?
        {
            if (RestResponse == null)
            {
                Log.Error("RestResponse object is null!");
                throw new Exception("RestResponse object is null!");
            }

            if (!RestResponse.StatusCode.Equals(statusCode))
            {
                Log.Error(GetLoggableResponse());
                //TODO: Can't you just create a string with those parameter? Does it need to be interpolated? What does interpolation means?
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 2);
                defaultInterpolatedStringHandler.AppendLiteral("Unexpected response status code! Expected= ");
                defaultInterpolatedStringHandler.AppendFormatted(statusCode);
                defaultInterpolatedStringHandler.AppendLiteral(" -> Actual= ");
                defaultInterpolatedStringHandler.AppendFormatted(RestResponse.StatusCode);
                throw new AssertionException(defaultInterpolatedStringHandler.ToStringAndClear());
            }

            Log.Debug(GetLoggableResponse());
            return this;
        }

        #region Private loggers and verifiers
        public string GetLoggableRequest(RestRequest restRequest = null)
        {
            Log.Debug("Request logging...");
            if (restRequest != null)
            {
                RestRequest = restRequest;
            }

            if (RestRequest == null)
            {
                Log.Error("Provided request is null! Did you execute a request first?");
                throw new Exception("Provided request is null! Did you execute a request first?");
            }

            return JsonConvert.SerializeObject(new
            {
                method = RestRequest.Method.ToString(),
                uri = RestClient.BuildUri(RestRequest),
                requestParameters = RestRequest.Parameters.Select((Parameter parameter) => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                defaultParameters = RestClient.DefaultParameters.Select((Parameter parameter) => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                })
            }, Formatting.Indented);
        }

        public string GetLoggableResponse(RestResponse restResponse = null)
        {
            Log.Debug("Response logging...");
            if (restResponse != null)
            {
                RestResponse = restResponse;
            }

            if (RestResponse == null)
            {
                throw new Exception("Current response is null! Did you execute a request first?");
            }

            object content = ((!IsValidJson(RestResponse.Content)) ? ((object)RestResponse.Content) : ((object)JObject.Parse(RestResponse.Content ?? string.Empty)));
            return JsonConvert.SerializeObject(new
            {
                responseUri = RestResponse.ResponseUri,
                statusCode = RestResponse.StatusCode,
                errorMessage = RestResponse.ErrorMessage,
                headers = RestResponse.Headers,
                content = content
            }, Formatting.Indented);
        }

        private static bool IsValidJson(string strInput)
        {
            try
            {
                JObject.Parse(strInput.Trim());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
