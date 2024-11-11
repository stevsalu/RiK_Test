using RIK_Test.Shared.Models;

namespace RIK_Test.Shared.DTOs;

public class ParticipantDTO {
	public int Id { get; set; }

	public string? Name { get; set; }

	public string? LastName { get; set; }

	public string? Code { get; set; }

	public string? Description { get; set; }

	public Participant.ParticipantType Type { get; set; }

	public string NameToString() {
		if (Type == Participant.ParticipantType.Private) return Name + " " + LastName;
		return Name;
	}

}