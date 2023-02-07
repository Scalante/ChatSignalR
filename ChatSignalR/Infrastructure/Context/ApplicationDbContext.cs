using ChatSignalR.Core.Entities;
using static ChatSignalR.Infrastructure.Context.ApplicationDbContext;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalR.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<SalaChat> SalasChat { get; set; }

    }
}
