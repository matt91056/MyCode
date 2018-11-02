using Microsoft.EntityFrameworkCore;
using DemoWeb.Models;

namespace DemoWeb.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {     
           return  base.SaveChanges();
        }
        #region dbSet
        public virtual DbSet<BookModel> Book { get; set; }
        #endregion
    }
}