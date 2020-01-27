using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Weather.PublicClasses;
using Weather.Models;
using Weather.Data;

namespace Weather.Controllers
{
    public class PublicController : ApiController
    {
        private WeatherContext db = new WeatherContext();

        /// <summary>
        /// Get weather information based on provided lat and lng
        /// </summary>
        /// <returns>JSON</returns>
        [Route("api/public/weather")]
        [HttpGet()]
        public HttpResponseMessage GetWeather()
        {
            APIResponse response;

            string token = null;

            //Get token from request header.
            if(Request.Headers.Contains("token"))
            {
                token = (string)Request.Headers.GetValues("token").First();
            }

            string ip = null;

            //Get user IP address from request
            ip = Tools.GetClientIp(Request);

            double lat = (Tools.GetQueryString(Request, "lat") != null) ? double.Parse(Tools.GetQueryString(Request, "lat")) : 0;

            double lng = (Tools.GetQueryString(Request, "lng") != null ) ? double.Parse(Tools.GetQueryString(Request, "lng")) : 0;

            //Check for valid lat and lng
            if(lat == 0 || lng == 0)
            {
                response = new APIResponse(HttpStatusCode.NoContent, "Please provide a valid coordinate.", null);

                return Request.CreateResponse(HttpStatusCode.OK, response, JsonMediaTypeFormatter.DefaultMediaType);
            }

            //Create and store user request
            UserRequest userRequest = new UserRequest(ip, lat, lng, token);
            
            db.UserRequests.Add(userRequest);
  
            db.SaveChanges();

            //Return error if user dose not allow to make request
            if(userRequest.Status == false)
            {
                response = new APIResponse(HttpStatusCode.Forbidden, "You reached out to your limitation. Please try again an hour later.", null);

                return Request.CreateResponse(HttpStatusCode.OK, response, JsonMediaTypeFormatter.DefaultMediaType);
            }

            response = new APIResponse(HttpStatusCode.OK, "Weather information", userRequest.WeatherInformation);

            return Request.CreateResponse(HttpStatusCode.OK, response, JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}
