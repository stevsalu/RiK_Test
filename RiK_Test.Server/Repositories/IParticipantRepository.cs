using RIK_Test.Shared.Models;

namespace RiK_Test.Server.Repositories;
public interface IParticipantRepository {
    Task<List<Participant>> GetAllAsync();
    Task<Participant?> GetByIdAsync(int id);
    Task<Participant?> GetByCodeAsync(string code);
    Task AddAsync(Participant participant);
    Task UpdateAsync(Participant participant);
    Task DeleteAsync(int id);
}
