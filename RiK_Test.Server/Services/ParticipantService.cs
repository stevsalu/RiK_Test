using RiK_Test.Server.Repositories;
using RIK_Test.Shared.Models;

namespace RiK_Test.Server.Services;
public class ParticipantService {

    private readonly IParticipantRepository _participantRepository;

    public ParticipantService(IParticipantRepository participantRepository) {
        _participantRepository = participantRepository;
    }


    public async Task<List<Participant>> GetAllParticipantsAsync() {
        return await _participantRepository.GetAllAsync();
    }

    public async Task<Participant?> GetParticipantByIdAsync(int id) {
        return await _participantRepository.GetByIdAsync(id);
    }

    public async Task<Participant> AddParticipantAsync(Participant participant) {
        await _participantRepository.AddAsync(participant);

        return await _participantRepository.GetByIdAsync(participant.Id);
    }

    public async Task<Participant?> UpdateParticipantAsync(int id, Participant participant) {
        if (id != participant.Id) throw new ArgumentException("Participant ID mismatch");

        await _participantRepository.UpdateAsync(participant);
        return await _participantRepository.GetByIdAsync(id);
    }

    public async Task<List<Participant>> DeleteParticipantAsync(int id) {
        await _participantRepository.DeleteAsync(id);

        return await GetAllParticipantsAsync();
    }

}

