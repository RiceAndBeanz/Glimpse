namespace WebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class location : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "Location_Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Vendors", "Location_Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "Location_Lng");
            DropColumn("dbo.Vendors", "Location_Lat");
        }
    }
}
