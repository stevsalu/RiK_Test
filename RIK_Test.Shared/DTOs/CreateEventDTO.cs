using System.ComponentModel.DataAnnotations;

namespace RIK_Test.Shared.DTOs;

public class CreateEventDTO {

	[Required(ErrorMessage = "Event name is required")]
	public string Name { get; set; } = string.Empty;


    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; } = string.Empty;

	[Required(ErrorMessage = "Place is required")]
	public string Place { get; set; } = string.Empty;

	[Required(ErrorMessage = "Date is required")]
	[DataType(DataType.Date)]
	public DateTime Date { get; set; }
}
