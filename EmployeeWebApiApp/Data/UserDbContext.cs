using EmployeeWebApiApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWebApiApp.Data
{
    public class UserDbContext : DbContext
    {
        internal IEnumerable<object> userModels;

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<UserModel> userModel { get; set; }
        public DbSet<EmployeeModel> employeeModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeModel>().ToTable("Employees");
            modelBuilder.Entity<UserModel>().ToTable("User");

        }
    }
}
