using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Weather.Data;
using Weather.PublicClasses;
using EntityFrameworkPaginate;

namespace Weather.Controllers
{
    public class AdministratorController : ApiController
    {
        private WeatherContext db = new WeatherContext();

        [Route("api/administrator/requests")]
        [HttpGet()]
        public HttpResponseMessage GetRequests()
        {
            int page = 1;

            int.TryParse(Tools.GetQueryString(Request, "page"), out page);

            string name = Tools.GetQueryString(Request, "name");
            
            string ip = Tools.GetQueryString(Request, "ip");
            
            string time = Tools.GetQueryString(Request, "time");

            DateTime dateTime = new DateTime();

            if(time != null)
            {
                dateTime = DateTime.Parse(time);
            }

            string order = Tools.GetQueryString(Request, "order");

            var list = db.UserRequests
                .Where(row => (name != null) ? row.User.Name == name : true)
                .Where(row => (ip != null) ? row.IP == ip : true)
                .Where(row => (time != null) ? row.Time == dateTime : true)
                .Select(row => new { row.Time, row.IP, row.Coordinates, row.User.Name, row.WeatherInformation })
                .ToList()
                .OrderBy(row => (order == "name")? row.Name : null)
                .OrderBy(row => (order == "ip") ? row.IP : null)
                .OrderBy(row => (order == "time") ? row.Time : DateTime.Now);

            int total = list.Count();

            var pages = list.Skip((page - 1) * Pagination.ItemPerPage)
            .Take(Pagination.ItemPerPage);

            APIResponse response = new APIResponse(HttpStatusCode.NoContent, "Requests informations", pages, page, total);

            return Request.CreateResponse(HttpStatusCode.OK, response, JsonMediaTypeFormatter.DefaultMediaType);

        }

        [Route("api/administrator/requests/weather")]
        [HttpGet()]
        public HttpResponseMessage GetAllWeatherRequest()
        {
            int page = 1;
            
            int.TryParse( Tools.GetQueryString(Request, "page"),out page);

            var list = db.UserRequests
                .Select(row => new { row.Time, row.IP, row.Coordinates })
                .ToList();

            int total = list.Count();

            var pages = list.Skip((page - 1) * Pagination.ItemPerPage)
            .Take(Pagination.ItemPerPage);

            APIResponse response = new APIResponse(HttpStatusCode.NoContent, "Requests informations", pages, page, total);

            return Request.CreateResponse(HttpStatusCode.OK, response, JsonMediaTypeFormatter.DefaultMediaType);
        }


        [Route("api/administrator/requests/ip")]
        [HttpGet()]
        public HttpResponseMessage GetClientIPMovement()
        {
            int page = 1;

            int.TryParse(Tools.GetQueryString(Request, "page"), out page);

            var list = db.Users.OrderBy(user => user.Name)
            .Select(user => new
            {
                user.Name,
                IPs = user.Requests.GroupBy(request => request.IP)
                 .Select(group => new { IP = group.Key })
                .ToList()
            }).ToList();

            int total = list.Count();

            var pages = list.Skip((page - 1) * Pagination.ItemPerPage)
            .Take(Pagination.ItemPerPage);

            APIResponse response = new APIResponse(HttpStatusCode.NoContent, "Requests informations", pages, page, total);

            return Request.CreateResponse(HttpStatusCode.OK, response, JsonMediaTypeFormatter.DefaultMediaType);
        }

        [Route("api/administrator/requests/failed")]
        [HttpGet()]
        public HttpResponseMessage GetFailedRequest()
        {
            int page = 1;

            int.TryParse(Tools.GetQueryString(Request, "page"), out page);

            var list = db.UserRequests
                .Where(request => request.Status == false)
                .GroupBy(request => request.IP)
                .Select(group => new { IP = group.Key, Failed = group.Count() })
                .ToList();

            int total = list.Count();

            var pages = list.Skip((page - 1) * Pagination.ItemPerPage)
            .Take(Pagination.ItemPerPage);

            APIResponse response = new APIResponse(HttpStatusCode.NoContent, "Requests informations", pages, page, total);

            return Request.CreateResponse(HttpStatusCode.OK, response, JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}
