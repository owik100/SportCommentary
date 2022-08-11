using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportCommentaryDataAccess.EFModels;

namespace SportCommentary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        
        }

        public DbSet<Commentary> Commentary { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<SingleComment> SingleComment { get; set; }
        public DbSet<SportType> SportType { get; set; }
    }
}