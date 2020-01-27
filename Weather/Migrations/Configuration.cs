namespace Weather.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Weather.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Weather.Data.WeatherContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Weather.Data.WeatherContext context)
        {

            context.Users.AddOrUpdate(
              p => p.Id,
              new User { Name = "Anonymous", Phone=null, Zipcode=null }
           
            );

        }
    }
}
