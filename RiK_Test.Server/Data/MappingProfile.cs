using AutoMapper;
using RIK_Test.Shared.Models;
using RIK_Test.Shared.DTOs;

namespace RiK_Test.Server.Data;
public class MappingProfile : Profile {

	public MappingProfile() {
		CreateMap<Event, EventDTO>();
		CreateMap<EventDTO, Event>();

		CreateMap<Participant, ParticipantDTO>();	
		CreateMap<ParticipantDTO, Participant>();
        CreateMap<CreateParticipantDTO, Participant>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Events, opt => opt.Ignore());
		CreateMap<Participant, CreateParticipantDTO>();
    }
}
