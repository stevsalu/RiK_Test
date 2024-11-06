using RIK_Test.Shared.Models;

namespace RiK_Test.Server.Repositories;

public interface IEventRepository {
    Task<List<Event>> GetEventsAsync();
    Task<Event?> GetByIdAsync(int id);
    Task AddEventAsync(Event evt);
    Task UpdateAsync(Event evt);
    Task DeleteAsync(int id);
}
