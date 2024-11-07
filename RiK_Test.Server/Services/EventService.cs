using RiK_Test.Server.Repositories;
using RIK_Test.Shared.Models;
using System.Data;

namespace RiK_Test.Server.Services;
public class EventService {

    private readonly IEventRepository _eventRepository;
    private readonly IParticipantRepository _participantRepository;

    public EventService(IEventRepository eventRepository, IParticipantRepository participantRepository) {
        _eventRepository = eventRepository;
        _participantRepository = participantRepository;
    }


    public async Task<List<Event>> GetAllEventsAsync() {
        return await _eventRepository.GetEventsAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int id) {
        return await _eventRepository.GetByIdAsync(id);
    }

    public async Task<Event> AddEventAsync(Event evt) {
        await _eventRepository.AddEventAsync(evt);
        var addedEvent = await _eventRepository.GetByIdAsync(evt.Id);
        if (addedEvent == null) throw new KeyNotFoundException($"Did not find event with {evt.Id} after adding"); 
        return addedEvent;
    }

    public async Task<Event?> UpdateEventAsync(int id, Event evt) {
        if (id != evt.Id) throw new ArgumentException("Event ID mismatch");

        await _eventRepository.UpdateAsync(evt);
        return await _eventRepository.GetByIdAsync(id);
    }

    public async Task<List<Event>> DeleteEventAsync(int id) {
        await _eventRepository.DeleteAsync(id);

        return await GetAllEventsAsync();
    }

    public async Task<Event?> RegisterParticipantToEvent(int eventId, int participantId) {
        var evt = await _eventRepository.GetByIdAsync(eventId);
        if (evt == null) throw new KeyNotFoundException("Event not found");

        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null) throw new KeyNotFoundException("Participant not found");

        if (evt.Participants.Any(p => p.Id == participantId)) throw new DuplicateNameException($"Participant {participant.Name} is already added to event {evt.Name}.");

        evt.Participants.Add(participant);
        await _eventRepository.UpdateAsync(evt);
        
        return evt;
    }

    public async Task<Event?> RemoveParticipantFromEvent(int eventId, int participantId) {
        var evt = await _eventRepository.GetByIdAsync(eventId);
        if (evt == null) throw new KeyNotFoundException("Event not found");

        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null) throw new KeyNotFoundException("Participant not found");

        if (!evt.Participants.Any(p => p.Id == participantId)) throw new KeyNotFoundException($"Participant {participant.Name} not found in event {evt.Name}.");

        evt.Participants.Remove(participant);
        await _eventRepository.UpdateAsync(evt);

        return evt;
    }
}
