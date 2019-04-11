namespace WebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promotionclick : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromotionClicks",
                c => new
                    {
                        PromotionClickId = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        PromotionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PromotionClickId)
                .ForeignKey("dbo.Promotions", t => t.PromotionId, cascadeDelete: true)
                .Index(t => t.PromotionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionClicks", "PromotionId", "dbo.Promotions");
            DropIndex("dbo.PromotionClicks", new[] { "PromotionId" });
            DropTable("dbo.PromotionClicks");
        }
    }
}
