using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.Membership;

namespace Riode.WebUI.Model.DataContexts
{
    // public class RiodeDbContext:DbContext eger biz membership yaziriqsa bu zaman bu clas toremelidir IdentityDbContextden;
    public class RiodeDbContext : IdentityDbContext<RiodeUser,RiodeRole,int,RiodeUserClaim,RiodeUserRole,RiodeUserLogin,RiodeRoleClaim,RiodeUserToken>
    {
        //public RiodeDbContext()
        //    :base()
        //{

        //} 
        public RiodeDbContext(DbContextOptions options)
           : base(options)
        {

        }
        //Appsettings vermisiq lazim deyil bize uje;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Data Source =.\SQLEXPRESS; Initial Catalog = RiodeWithGitshop; User Id = sa; Password = query;");
        //    }
        //}
        public DbSet<ContactPost> ContactPosts { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<OneCategory> OneCategories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSizeColorItem> ProductSizeColorCollection { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SpecificationCategoryItem> SpecificationCategoryCollection{ get; set; }
        public DbSet<SpecificationValue> SpecificationValues{ get; set; }
        public DbSet<Subscrice> Subscrices { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        



        // Database olan membershiplerin ADi qabaginda ASP isdemirikse gorsensin ozmuz duz qos eliyib burda yaziriq
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RiodeUser>(e=> {
                           // adi   //ADI qabagindaki
                e.ToTable("Users","Membership");

            });

            builder.Entity<RiodeRole>(e => {
                // adi   //ADI qabagindaki
                e.ToTable("Roles", "Membership");

            });

            builder.Entity<RiodeUserRole>(e => {
                // adi   //ADI qabagindaki
                e.ToTable("UserRoles", "Membership");

            });

            builder.Entity<RiodeUserClaim>(e => {
                // adi   //ADI qabagindaki
                e.ToTable("UserClaims", "Membership");

            });

            builder.Entity<RiodeRoleClaim>(e => {
                // adi   //ADI qabagindaki
                e.ToTable("RoleClaims", "Membership");

            });
            builder.Entity<RiodeUserToken>(e => {
                // adi   //ADI qabagindaki
                e.ToTable("UserTokens", "Membership");

            });
            builder.Entity<RiodeUserLogin>(e => {
                // adi   //ADI qabagindaki
                e.ToTable("UserLogins", "Membership");

            });
        }

    }
}   
