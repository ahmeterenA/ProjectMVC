using Microsoft.EntityFrameworkCore;

namespace APP.Domain
{
    public class Db : DbContext
    {
        
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        
        
        public Db(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Group>().HasIndex(groupEntity => groupEntity.Title).IsUnique();

            modelBuilder.Entity<Role>().HasIndex(roleEntity => roleEntity.Name).IsUnique();
            
            modelBuilder.Entity<Project>().HasIndex(projectEntity => projectEntity.Name).IsUnique();

            modelBuilder.Entity<User>().HasIndex(userEntity => userEntity.UserName).IsUnique();
            
            modelBuilder.Entity<User>().HasIndex(userEntity => new { userEntity.FirstName, userEntity.LastName });

            
            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.User) 
                .WithMany(userEntity => userEntity.UserRoles) 
                .HasForeignKey(userRoleEntity => userRoleEntity.UserId) 
                                                                        
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.Role) 
                .WithMany(roleEntity => roleEntity.UserRoles) 
                .HasForeignKey(userRoleEntity => userRoleEntity.RoleId) 
                                                                        
                .OnDelete(DeleteBehavior.NoAction); 
            
            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.Group) 
                .WithMany(groupEntity => groupEntity.Users) 
                .HasForeignKey(userEntity => userEntity.GroupId) 
                                                                 
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskUser>()
                .HasOne(taskUserEntity => taskUserEntity.User)
                .WithMany(userEntity => userEntity.TaskUsers)
                .HasForeignKey(taskUserEntity => taskUserEntity.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskUser>()
                .HasOne(taskUserEntity => taskUserEntity.Task)
                .WithMany(taskEntity => taskEntity.TaskUsers)
                .HasForeignKey(taskUserEntity => taskUserEntity.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Task>()
                .HasOne(taskEntity => taskEntity.Project)
                .WithMany(projectEntity => projectEntity.Tasks)
                .HasForeignKey(taskEntity => taskEntity.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}