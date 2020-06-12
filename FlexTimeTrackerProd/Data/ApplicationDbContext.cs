using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlexTimeTrackerProd.Models;

namespace FlexTimeTrackerProd.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FlexTimeTrackerProd.Models.AddEntry> AddEntry { get; set; }
        public DbSet<FlexTimeTrackerProd.Models.UseEntry> UseEntry { get; set; }
        public DbSet<FlexTimeTrackerProd.Models.UserMinutes> UserMinutes { get; set; }
        public DbSet<FlexTimeTrackerProd.Models.EventLog> EventLog { get; set; }
    }
}
