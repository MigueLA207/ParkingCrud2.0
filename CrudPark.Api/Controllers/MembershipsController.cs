// Archivo: CrudPark.Api/Controllers/MembershipsController.cs
using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMembership([FromBody] CreateMembershipDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newMembershipDto = await _membershipService.CreateMembershipAsync(createDto);
                return CreatedAtAction(nameof(CreateMembership), new { id = newMembershipDto.MembershipId }, newMembershipDto);
            }
            catch (InvalidOperationException ex)
            {
                // Si el servicio lanzó nuestra excepción de negocio, devolvemos un 409 Conflict.
                // Es un código de estado que significa "La petición no se pudo completar
                // debido a un conflicto con el estado actual del recurso."
                return Conflict(new { message = ex.Message });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllMemberships()
        {
            var memberships = await _membershipService.GetAllMembershipsAsync();
            return Ok(memberships);
        }

        // GET /api/memberships/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembershipById(int id)
        {
            var membership = await _membershipService.GetMembershipByIdAsync(id);
            if (membership == null)
            {
                return NotFound();
            }
            return Ok(membership);
        }

        // PUT /api/memberships/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMembership(int id, [FromBody] UpdateMembershipDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedMembership = await _membershipService.UpdateMembershipAsync(id, updateDto);
                if (updatedMembership == null)
                {
                    return NotFound();
                }
                return Ok(updatedMembership);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE /api/memberships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            var success = await _membershipService.DeleteMembershipAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        
        
    }
}