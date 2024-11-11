using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiK_Test.Server.Services;
using RIK_Test.Shared.DTOs;
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
    public async Task<ActionResult<IEnumerable<Participant>>> GetAllParticipants() {
        try {
            var participants = await _participantService.GetAllParticipantsAsync();
            return Ok(participants);
        }
        catch (Exception ex) {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Participant>>> GetParticipant(int id) {
        try {
            var participant = await _participantService.GetParticipantByIdAsync(id);
            if (participant == null) return NotFound();
            return Ok(participant);
        }
        catch (Exception ex) {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddParticipant(Participant participant) {
        try {
            var createdParticipant = await _participantService.AddParticipantAsync(participant);
            return CreatedAtAction(nameof(GetParticipant), new { id = createdParticipant.Id }, createdParticipant);
        }
        catch (Exception ex) {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParticipant(int id, CreateParticipantDTO participant) {
        try {
            var updatedParticipant = await _participantService.UpdateParticipantAsync(id, participant);
            if (updatedParticipant == null) return NotFound();
            return Ok(updatedParticipant);
        }
        catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception) {
            return StatusCode(500, "An error occurred while updating the event.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParticipant(int id) {
        try {
            return Ok(await _participantService.DeleteParticipantAsync(id));
        }
        catch (Exception) {
            return StatusCode(500, "An error occurred while deleting the event.");
        }
    }
}
