namespace WebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagerework : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotions", "PromotionImageURL", c => c.String());
            AddColumn("dbo.PromotionImages", "ImageURL", c => c.String());
            DropColumn("dbo.Promotions", "PromotionImage");
            DropColumn("dbo.PromotionImages", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromotionImages", "Image", c => c.Binary());
            AddColumn("dbo.Promotions", "PromotionImage", c => c.Binary());
            DropColumn("dbo.PromotionImages", "ImageURL");
            DropColumn("dbo.Promotions", "PromotionImageURL");
        }
    }
}
