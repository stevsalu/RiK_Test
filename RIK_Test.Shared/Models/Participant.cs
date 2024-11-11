using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIK_Test.Shared.Models;
public class Participant {

    public enum ParticipantType {
        Unkown = 0,
        Private = 1,
        Business = 2
    }
   
    public int Id { get; set; }

    public ParticipantType? Type { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public int? ParticipantCount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Description { get; set; }

    public List<Event> Events { get; set; } = new();

}

