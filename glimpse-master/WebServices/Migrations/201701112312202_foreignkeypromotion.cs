namespace WebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foreignkeypromotion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PromotionImages", "Promotion_PromotionId", "dbo.Promotions");
            DropIndex("dbo.PromotionImages", new[] { "Promotion_PromotionId" });
            RenameColumn(table: "dbo.PromotionImages", name: "Promotion_PromotionId", newName: "PromotionId");
            AlterColumn("dbo.PromotionImages", "PromotionId", c => c.Int(nullable: false));
            CreateIndex("dbo.PromotionImages", "PromotionId");
            AddForeignKey("dbo.PromotionImages", "PromotionId", "dbo.Promotions", "PromotionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromotionImages", "PromotionId", "dbo.Promotions");
            DropIndex("dbo.PromotionImages", new[] { "PromotionId" });
            AlterColumn("dbo.PromotionImages", "PromotionId", c => c.Int());
            RenameColumn(table: "dbo.PromotionImages", name: "PromotionId", newName: "Promotion_PromotionId");
            CreateIndex("dbo.PromotionImages", "Promotion_PromotionId");
            AddForeignKey("dbo.PromotionImages", "Promotion_PromotionId", "dbo.Promotions", "PromotionId");
        }
    }
}
