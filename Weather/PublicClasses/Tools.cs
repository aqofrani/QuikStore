using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Weather.Data;

namespace Weather.PublicClasses
{
    public static class Tools
    {
        const string weatherApiUrl = "https://fcc-weather-api.glitch.me/api/current";


        /// <summary>
        /// Get IP HttpRequestMessage object
        /// </summary>
        /// <param name="request"></param>
        /// <returns>string/null</returns>
        public static string GetClientIp(HttpRequestMessage request)
        {

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Call API and make WeatherInformation object.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns>WeatherInformation</returns>
        public static WeatherInformation GetWeather(double lat, double lng)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string response = httpClient.GetStringAsync(new Uri(string.Format("{0}?lat={1}&lon={2}", weatherApiUrl, lat, lng))).Result;
            JObject responseJsonObject = JObject.Parse(response);
            return new WeatherInformation(responseJsonObject);
        }

        /// <summary>
        /// Reterns query parameter for given key.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns>string/null</returns>
        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;

            return match.Value;
        }
    }
}