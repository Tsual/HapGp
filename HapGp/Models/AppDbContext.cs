using Microsoft.EntityFrameworkCore;

namespace HapGp.Models
{
    /*
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            UserRecordModel
    */
    public class AppDbContext:DbContext
    {
        public DbSet<BasicApplicationModel> BasicApplicationModels { get; set; }
        public DbSet<StorageModel> M_StorageModels { get; set; }
        public DbSet<TestModel> M_TestModels { get; set; }
        public DbSet<UserModel> M_UserModels { get; set; }
        public DbSet<AppConfigModel> M_AppConfigModels { get; set; }
        public DbSet<UserRecordModel> M_UserRecordModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasicApplicationModel>();
            modelBuilder.Entity<StorageModel>();
            modelBuilder.Entity<TestModel>();
            modelBuilder.Entity<UserModel>();
            modelBuilder.Entity<AppConfigModel>();
            modelBuilder.Entity<UserRecordModel>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=db0.db");
        }
       


    }
}
