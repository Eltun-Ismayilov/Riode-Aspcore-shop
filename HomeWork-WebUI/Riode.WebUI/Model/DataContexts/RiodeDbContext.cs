using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.DataContexts
{
    public class RiodeDbContext:DbContext
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
        public DbSet<Contect> contects { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<OneCategory> OneCategories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
       
    }
}
