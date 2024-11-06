using Microsoft.EntityFrameworkCore;
using RIK_Test.Shared.Models;

namespace RiK_Test.Server.Data;
public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Event> EventsTable { get; set; }

    public DbSet<Participant> ParticipantsTable { get; set; }
}