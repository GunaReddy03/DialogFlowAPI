using DialogFlowAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using static DialogFlowAPI.Models.Logins;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DialogFlowAPI.DbContext
{
    public class DialogFlowDbContext: IdentityDbContext<ApplicationUser>
    {
        public DialogFlowDbContext(DbContextOptions<DialogFlowDbContext> opt) : base(opt)
        {
          
        }
        public DbSet<TempClass> TempData { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<AgentUserModel> AgentUser { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<AgentUserModel>()
            //.HasKey(au => new { au.UserId, au.AgentID });

            //// Configure relationships if needed
            //builder.Entity<AgentUserModel>()
            //    .HasOne(au => au.ApplicationUser)
            //    .WithMany()
            //    .HasForeignKey(au => au.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);

        }

    }
}
