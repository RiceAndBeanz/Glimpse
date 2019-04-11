namespace WebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promotionimagemodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotionImages",
                c => new
                    {
                        PromotionImageId = c.Int(nullable: false, identity: true),
                        Image = c.Binary(),
                        Promotion_PromotionId = c.Int(),
                    })
                .PrimaryKey(t => t.PromotionImageId)
                .ForeignKey("dbo.Promotions", t => t.Promotion_PromotionId)
                .Index(t => t.Promotion_PromotionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionImages", "Promotion_PromotionId", "dbo.Promotions");
            DropIndex("dbo.PromotionImages", new[] { "Promotion_PromotionId" });
            DropTable("dbo.PromotionImages");
        }
    }
}
