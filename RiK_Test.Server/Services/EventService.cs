using AutoMapper;
using RiK_Test.Server.Data;
using RiK_Test.Server.Repositories;
using RIK_Test.Shared.DTOs;
using RIK_Test.Shared.Models;
using SQLitePCL;
using System.Data;

namespace RiK_Test.Server.Services;
public class EventService {

    private readonly AppDbContext _context;
    private readonly IEventRepository _eventRepository;
    private readonly IParticipantRepository _participantRepository;
	private readonly IMapper _mapper;

	public EventService(AppDbContext context, IEventRepository eventRepository, IParticipantRepository participantRepository, IMapper mapper) {
        _context = context;
        _eventRepository = eventRepository;
        _participantRepository = participantRepository;
        _mapper = mapper;
    }


    public async Task<List<EventDTO>> GetEventsAsync(bool? onlyPast = null) {
        var events = await _eventRepository.GetEventsAsync();

        if (onlyPast == true) {
            events = events.Where(e => e.Date.HasValue && e.Date.Value < DateTime.Now).ToList();
        }
        else if (onlyPast == false) {
            events = events.Where(e => e.Date.HasValue && e.Date.Value > DateTime.Now).ToList();
        }

        return _mapper.Map<List<EventDTO>>(events);
    }

    public async Task<EventDTO?> GetEventByIdAsync(int id) {
        var evt = await _eventRepository.GetByIdAsync(id);
        return _mapper.Map<EventDTO>(evt);
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

    public async Task<List<EventDTO>> DeleteEventAsync(int id) {
        await _eventRepository.DeleteAsync(id);

        return await GetEventsAsync();
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

    public async Task<EventDTO?> RegisterOrAddParticipantToEvent(int eventId, CreateParticipantDTO participantDto) {
        var evt = await _eventRepository.GetByIdAsync(eventId);
        if (evt == null) throw new KeyNotFoundException("Event not found");

        var participant = await _participantRepository.GetByCodeAsync(participantDto.Code);
        using var transaction = await _context.Database.BeginTransactionAsync();

        try {
            if (participant == null) {
                participant = _mapper.Map<Participant>(participantDto);
                await _participantRepository.AddAsync(participant);
            }
            else {
                participant.Name = participantDto.Name ?? participant.Name;
                participant.LastName = participantDto.LastName ?? participant.LastName;
                participant.PaymentMethod = participantDto.PaymentMethod ?? participant.PaymentMethod;
                participant.Description = participantDto.Description ?? participant.Description;

                await _participantRepository.UpdateAsync(participant);
            }

            if (evt.Participants.Any(p => p.Id == participant.Id)) {
                throw new DuplicateNameException($"Participant {participant.Name} is already added to event {evt.Name}.");
            }

            evt.Participants.Add(participant);
            await _eventRepository.UpdateAsync(evt);

            await transaction.CommitAsync();
            
            return _mapper.Map<EventDTO>(evt);
        } catch (Exception) {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<EventDTO?> RemoveParticipantFromEvent(int eventId, int participantId) {
        var evt = await _eventRepository.GetByIdAsync(eventId);
        if (evt == null) throw new KeyNotFoundException("Event not found");

        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null) throw new KeyNotFoundException("Participant not found");

        if (!evt.Participants.Any(p => p.Id == participantId)) throw new KeyNotFoundException($"Participant {participant.Name} not found in event {evt.Name}.");

        evt.Participants.Remove(participant);
        await _eventRepository.UpdateAsync(evt);

        return _mapper.Map<EventDTO>(evt);
    }
}
