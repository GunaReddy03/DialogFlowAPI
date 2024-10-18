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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

    }
}
