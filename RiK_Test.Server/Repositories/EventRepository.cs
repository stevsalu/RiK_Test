using Microsoft.EntityFrameworkCore;
using RiK_Test.Server.Data;
using RIK_Test.Shared.Models;
using System.Collections;

namespace RiK_Test.Server.Repositories;

public class EventRepository : IEventRepository {
    private readonly AppDbContext _ctx;

    public EventRepository(AppDbContext context) {
        _ctx = context;
    }

    public async Task<List<Event>> GetEventsAsync() {
        return await _ctx.EventsTable.ToListAsync();
    }

    public async Task AddEventAsync(Event evt) {
        _ctx.EventsTable.Add(evt);

        await _ctx.SaveChangesAsync();
    }

    public async Task<Event?> GetByIdAsync(int id) {
        return await _ctx.EventsTable.FindAsync(id);
    }

    public async Task UpdateAsync(Event evt) {
        _ctx.Entry(evt).State = EntityState.Modified;
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var evt = await _ctx.EventsTable.FindAsync(id);
        if (evt != null) {
            _ctx.EventsTable.Remove(evt);
            await _ctx.SaveChangesAsync();
        }
    }
}
