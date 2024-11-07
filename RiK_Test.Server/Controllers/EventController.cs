using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiK_Test.Server.Repositories;
using RiK_Test.Server.Services;
using RIK_Test.Shared.Models;
using System.Data;

namespace RiK_Test.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase {

    private readonly EventService _eventService;

    public EventController(EventService eventService) {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents() {
        try {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        catch (Exception ex) {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id) {
        try {
            var evt = await _eventService.GetEventByIdAsync(id);
            if (evt == null) return NotFound();
            return Ok(evt);
        }
        catch (Exception ex) {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddEvent(Event evt) {
        try {
            var createdEvent = await _eventService.AddEventAsync(evt);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }
        catch (Exception ex) {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("{eventId}/register/{participantId}")]
    public async Task<IActionResult> RegisterParticipantToEvent(int eventId, int participantId) {
        try {
            var updatedEvent = await _eventService.RegisterParticipantToEvent(eventId, participantId);
            return Ok(updatedEvent);
        }
        catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
        catch (DuplicateNameException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception) {
            return StatusCode(500, "An error occurred while registering the participant to the event.");
        }
    }

    [HttpPost("{eventId}/remove/{participantId}")]
    public async Task<IActionResult> RemoveParticipantFromEvent(int eventId, int participantId) {
        try {
            var updatedEvent = await _eventService.RemoveParticipantFromEvent(eventId, participantId);
            return Ok(updatedEvent);
        }
        catch (KeyNotFoundException ex) {
            return NotFound(ex.Message);
        }
        catch (Exception) {
            return StatusCode(500, "An error occurred while registering the participant to the event.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event evt) {
        try {
            var updatedEvent = await _eventService.UpdateEventAsync(id, evt);
            if (updatedEvent == null) return NotFound();
            return Ok(updatedEvent);
        }
        catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        }
        catch (Exception) {
            return StatusCode(500, "An error occurred while updating the event.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id) {
        try {
            return Ok(await _eventService.DeleteEventAsync(id));
        }
        catch (Exception) {
            return StatusCode(500, "An error occurred while deleting the event.");
        }
    }
}

