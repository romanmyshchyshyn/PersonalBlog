using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalBlog.DataAccess.Models;
using System;
using System.Linq;

namespace PersonalBlog.DataAccess
{
    public class PersonalBlogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Rate> Rates { get; set; }

        public PersonalBlogContext(DbContextOptions<PersonalBlogContext> options) : 
            base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Post)
                .WithOne(p => p.Article)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasAlternateKey(p => p.Title);

            modelBuilder.Entity<Role>()
                .HasAlternateKey(r => r.Name);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Rates)
                .OnDelete(DeleteBehavior.Cascade);

            var doubleArrayToStringConverter = new ValueConverter<double[], string>(
                v => string.Join(";", v),
                v => v.Split(';').Select(val => double.Parse(val)).ToArray());

            modelBuilder.Entity<User>()
                .Property(u => u.Weights)
                .HasConversion(doubleArrayToStringConverter);

            modelBuilder.Entity<Post>()
                .Property(u => u.Features)
                .HasConversion(doubleArrayToStringConverter);
        }
    }
}
