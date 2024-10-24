using Microsoft.EntityFrameworkCore;
using WebHookApp.Models;

namespace WebHookApp
{
    public class webHookDataContext:DbContext
    {

        public DbSet<WebHookUrl> Urls { get; set; }

        public DbSet<WebHookRequestModel> requests { get; set; }

        public webHookDataContext(DbContextOptions<webHookDataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
                
        }
    }
}
