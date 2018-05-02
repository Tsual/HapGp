using Microsoft.EntityFrameworkCore;

namespace HapGp.Models
{
    public class AppDbContext:DbContext
    {
        public DbSet<BasicApplicationModel> BasicApplicationModels { get; set; }
        public DbSet<StorageModel> M_StorageModels { get; set; }
        public DbSet<TestModel> M_TestModels { get; set; }
        public DbSet<UserModel> M_UserModels { get; set; }
        public DbSet<AppConfigModel> M_AppConfigModels { get; set; }
        public DbSet<UserRecordModel> M_UserRecordModels { get; set; }
        public DbSet<ProjectModel> M_ProjectModels { get; set; }
        public DbSet<ProjectSelectModel> M_ProjectSelectModels { get; set; }
        public DbSet<SigninModel> M_SigninModel { get; set; }
        public DbSet<SchoolBusModel> M_SchoolBusModel { get; set; }
        public DbSet<LeaveModel> M_LeaveModle { get; set; }
        public DbSet<MissionModel> M_MissionModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasicApplicationModel>();
            modelBuilder.Entity<StorageModel>();
            modelBuilder.Entity<TestModel>();
            modelBuilder.Entity<UserModel>();
            modelBuilder.Entity<AppConfigModel>();
            modelBuilder.Entity<UserRecordModel>();
            modelBuilder.Entity<ProjectModel>();
            modelBuilder.Entity<ProjectSelectModel>();
            modelBuilder.Entity<SigninModel>();
            modelBuilder.Entity<SchoolBusModel>();
            modelBuilder.Entity<LeaveModel>();
            modelBuilder.Entity<MissionModel>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=db1.db");
        }
       


    }
}
