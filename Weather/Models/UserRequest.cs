using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather.Data;
using Weather.PublicClasses;

namespace Weather.Models
{
    /// <summary>
    /// Represent user's requests model.
    /// </summary>
    public class UserRequest
    {
        private  WeatherContext db = new WeatherContext();
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IP { get; set; }
        public string Coordinates { get; set; }
        public WeatherInformation WeatherInformation { get; set; }
        public DateTime Time { get; set; }
        public bool Status { get; set; }

        public User User { get; set; }

        public UserRequest()
        {

        }
        public UserRequest(string ip, double lat, double lng, string token = null)
        {
            this.IP = ip;

            this.Coordinates = string.Format("{0},{1}",lat,lng);

            this.UserId = User.GetUserByToken(token).Id;

            this.Time = DateTime.Now.ToUniversalTime();

            this.WeatherInformation = Tools.GetWeather(lat, lng);

            this.Status = this.IsValid();
        }

        /// <summary>
        /// Check user allows to make a request to the weather api.
        /// </summary>
        /// <returns>bool</returns>
        private bool IsValid()
        {
            DateTime dateTimeNow = DateTime.UtcNow;
            DateTime dateTimeBefore = DateTime.UtcNow.AddHours(-1);

            var count = db.UserRequests
            .Where(row => row.IP == this.IP)
            .Where(row => row.Time >= dateTimeBefore && row.Time <= dateTimeNow)
            .Count();

            User user = db.Users.Find(this.UserId);
            if(user.Token == null && count == 0)
            {
                return true;
            }

            if(user.Token != null && count < 4)
            {
                return true;
            }

            return false;
        }

    }
}