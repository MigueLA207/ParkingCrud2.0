using CrudPark.Application.DTOs;
using CrudPark.Application.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace CrudPark.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperatorsController : ControllerBase
    {
        
        private readonly IOperatorService _operatorService;

        public OperatorsController(IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetOperators()
        {

            var operators = await _operatorService.GetAllOperatorsAsync();
            return Ok(operators);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> CreateOperatorAsync([FromBody] CreateOperatorDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newOperatorDto = await _operatorService.CreateOperatorAsync(createDto);
            return CreatedAtAction(nameof(GetOperators), new { id = newOperatorDto.OperatorId }, newOperatorDto);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOperatorById(int id)
        {
            var operatorDto = await _operatorService.GetOperatorByIdAsync(id);

            if (operatorDto == null)
            {
                return NotFound();
            }

            return Ok(operatorDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOperator(int id, [FromBody] UpdateOperatorDto updateDto)
        {
            var updatedOperator = await _operatorService.UpdateOperatorAsync(id, updateDto);

            if (updatedOperator == null)
            {
                return NotFound($"Operator with ID {id} not found.");
            }

            return Ok(updatedOperator);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperator(int id)
        {
            var success = await _operatorService.DeleteOperatorAsync(id);

            if (!success)
            {
                return NotFound($"Operator with ID {id} not found.");
            }

            return NoContent(); 
        }
        
        
        
        
    }
}