using System.Data.Entity;

namespace WebServices.Models
{
    public class GlimpseDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public GlimpseDbContext() : base("GlimpseDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public System.Data.Entity.DbSet<WebServices.Models.PromotionImage> PromotionImages { get; set; }

        public System.Data.Entity.DbSet<WebServices.Models.PromotionClick> PromotionClicks { get; set; }
    }
}