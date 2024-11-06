using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiK_Test.Server.Services;
using RIK_Test.Shared.Models;

namespace RiK_Test.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipantController : ControllerBase {

    private readonly ParticipantService _participantService;

    public ParticipantController(ParticipantService participantService) {
        _participantService = participantService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllParticipants() {
        try {
            var participants = await _participantService.GetAllParticipantsAsync();
            return Ok(participants);
        }
        catch (Exception ex) {
            return BadRequest(ex.ToString());
        }
    }
}
