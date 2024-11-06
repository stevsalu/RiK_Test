using Microsoft.EntityFrameworkCore;
using RiK_Test.Server.Data;
using RIK_Test.Shared.Models;

namespace RiK_Test.Server.Repositories;
public class ParticipantRepository : IParticipantRepository {
    private readonly AppDbContext _ctx;

    public ParticipantRepository(AppDbContext context) {
        _ctx = context;
    }

    public async Task<List<Participant>> GetAllAsync() {
        return await _ctx.ParticipantsTable.ToListAsync();
    }

    public async Task<Participant?> GetByIdAsync(int id) {
        return await _ctx.ParticipantsTable.FindAsync(id);
    }

    public async Task AddAsync(Participant participant) {
        _ctx.ParticipantsTable.Add(participant);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Participant participant) {
        _ctx.Entry(participant).State = EntityState.Modified;
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var participant = await _ctx.ParticipantsTable.FindAsync(id);
        if (participant != null) {
            _ctx.ParticipantsTable.Remove(participant);
            await _ctx.SaveChangesAsync();
        }
    }
}
