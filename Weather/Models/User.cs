using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Weather.Data;

namespace Weather.Models
{
    /// <summary>
    /// Represent user model.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Zipcode { get; set; }
        public List<UserRequest> Requests { get; set; }

        private static WeatherContext db = new WeatherContext();
        public User()
        {
            //Generate new token for each new user model
            string Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Token = Token.Replace("=", "").Replace("+", "");
            this.Token = Token;
        }

        /// <summary>
        /// Finds user model by given token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>User</returns>
        public static User GetUserByToken(string token)
        {
            var user = db.Users
                        .Where(row => row.Token == token)
                        .FirstOrDefault<User>();
            return user;
        }

    }
}