namespace WebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fluent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vendors", "Email", c => c.String(maxLength: 20));
            CreateIndex("dbo.Users", "Email", unique: true);
            CreateIndex("dbo.Vendors", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Vendors", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Vendors", "Email", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
        }
    }
}
