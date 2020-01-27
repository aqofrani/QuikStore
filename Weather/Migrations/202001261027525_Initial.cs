namespace Weather.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        IP = c.String(),
                        Coordinates = c.String(),
                        WeatherInformation_Style = c.String(),
                        WeatherInformation_Description = c.String(),
                        WeatherInformation_CityName = c.String(),
                        WeatherInformation_Temperature = c.Double(nullable: false),
                        WeatherInformation_Pressure = c.Double(nullable: false),
                        WeatherInformation_Humidity = c.Double(nullable: false),
                        WeatherInformation_MinimumTemperature = c.Double(nullable: false),
                        WeatherInformation_MaximumTemperature = c.Double(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        Zipcode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRequests", "UserId", "dbo.Users");
            DropIndex("dbo.UserRequests", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserRequests");
        }
    }
}
