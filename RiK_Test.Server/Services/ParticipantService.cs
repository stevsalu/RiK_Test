using AutoMapper;
using RiK_Test.Server.Repositories;
using RIK_Test.Shared.DTOs;
using RIK_Test.Shared.Models;

namespace RiK_Test.Server.Services;
public class ParticipantService {

    private readonly IParticipantRepository _participantRepository;
    private readonly IMapper _mapper;

    public ParticipantService(IParticipantRepository participantRepository, IMapper mapper) {
        _participantRepository = participantRepository;
        _mapper = mapper;
    }


    public async Task<List<Participant>> GetAllParticipantsAsync() {
        return await _participantRepository.GetAllAsync();
    }

    public async Task<Participant?> GetParticipantByIdAsync(int id) {
        return await _participantRepository.GetByIdAsync(id);
    }

    public async Task<Participant> AddParticipantAsync(Participant participant) {
        await _participantRepository.AddAsync(participant);
        var addedParticipant = await _participantRepository.GetByIdAsync(participant.Id);
        if (addedParticipant == null) throw new KeyNotFoundException($"Did not find participant with {participant.Id} after adding");
        return addedParticipant;
    }

    public async Task<CreateParticipantDTO?> UpdateParticipantAsync(int id, CreateParticipantDTO participantDto) {
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null) {
            throw new KeyNotFoundException("Participant not found.");
        }

        participant.Name = participantDto.Name ?? participant.Name;
        participant.LastName = participantDto.LastName ?? participant.LastName;
        participant.PaymentMethod = participantDto.PaymentMethod ?? participant.PaymentMethod;
        participant.Description = participantDto.Description ?? participant.Description;
        participant.Type = participantDto.Type ?? participant.Type;
        participant.Code = participantDto.Code ?? participant.Code;
        participant.ParticipantCount = participantDto.ParticipantCount ?? participant.ParticipantCount;

        await _participantRepository.UpdateAsync(participant);
        return _mapper.Map<CreateParticipantDTO>(participant);
    }

    public async Task<List<Participant>> DeleteParticipantAsync(int id) {
        await _participantRepository.DeleteAsync(id);

        return await GetAllParticipantsAsync();
    }

}

