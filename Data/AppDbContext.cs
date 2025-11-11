using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

      public DbSet<Category> Categories { get; set; }
      public DbSet<Expense> Expenses { get; set; }

      protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // Seed roles
    builder.Entity<IdentityRole>().HasData(
        new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
        new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
    );

    // Relationships
    builder.Entity<Category>()
        .HasOne(c => c.User)
        .WithMany(u => u.Categories)
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    builder.Entity<Expense>()
        .HasOne(e => e.User)
        .WithMany(u => u.Expenses)
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Restrict); // prevent cascade loop

    builder.Entity<Expense>()
        .HasOne(e => e.Category)
        .WithMany(c => c.Expenses)
        .HasForeignKey(e => e.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);
}

    }
}