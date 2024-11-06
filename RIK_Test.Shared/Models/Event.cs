﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIK_Test.Shared.Models;
    public class Event : IValidatableObject {

    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Place {  get; set; }

    public DateTime? Date { get; set; }

    public List<Participant> Participants { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (string.IsNullOrWhiteSpace(Name)) yield return new ValidationResult("The name field is required");
    }
}
