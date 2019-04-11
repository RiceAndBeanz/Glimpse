namespace WebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class charlimitemailv2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Vendors", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Vendors", "Email", c => c.String(maxLength: 100));
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 100));
            CreateIndex("dbo.Vendors", "Email", unique: true);
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Vendors", new[] { "Email" });
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 20));
            AlterColumn("dbo.Vendors", "Email", c => c.String(maxLength: 20));
            CreateIndex("dbo.Users", "Email", unique: true);
            CreateIndex("dbo.Vendors", "Email", unique: true);
        }
    }
}
