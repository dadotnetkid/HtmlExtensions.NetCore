using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HtmlExtension.Core.Web.Models
{
    public class NorthwindDb:DbContext
    {

        public NorthwindDb(DbContextOptions<NorthwindDb> options):base(options)
        {
            
        }
        public DbSet<Customers> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>().ToTable("Customers");
            base.OnModelCreating(modelBuilder);
        }
    }
}
