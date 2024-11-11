using RIK_Test.Shared.DTOs;

namespace RIK_Test.Shared.DTOs;
public class EventDTO {
	public int Id { get; set; }

	public string? Name { get; set; }

	public string? Description { get; set; }

	public string? Place { get; set; }

	public DateTime? Date { get; set; }

	public List<ParticipantDTO> Participants { get; set; } = new();
}

