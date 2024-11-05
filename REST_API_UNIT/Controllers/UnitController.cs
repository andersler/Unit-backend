using Microsoft.AspNetCore.Mvc;
using REST_API_UNIT.Models;
using REST_API_UNIT.Services;
using REST_API_UNIT.Utils;
using System;
using static REST_API_UNIT.DTO.UnitDTO;

namespace REST_API_UNIT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;
        private readonly ILogger<UnitController> _logger;

        public UnitController(IUnitService unitService, ILogger<UnitController> logger)
        {
            _unitService = unitService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> DoCreateUnit([FromBody] DTOUnitNewRequest request)
        {
            try
            {
                if (Utils.InputValidationUtils.IsValid(request.Name) && Utils.InputValidationUtils.IsValid(request.Type))
                {
                    DTOUnitGetSingleResponse response = new();
                    var unit = new Unit
                    {
                        Name = request.Name,
                        IsActive = request.IsActive,
                        Type = request.Type,
                    };

                    var res = await _unitService.AddUnitAsync(unit);

                    response.unit = res;
                    response.Message = $"Successfully added unit: {res.Name}";

                    _logger.LogInformation("Adding unit {UnitId} at {Time}", res.Id, DateTime.Now);
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Failed to add unit at {Time}", DateTime.Now);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var Message = "Internal server error.";
                return StatusCode(StatusCodes.Status500InternalServerError, Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DoGetAllUnits()
        {
            DTOUnitGetResponse response = new();
            try
            {
                response.units = await _unitService.GetAllUnitsAsync();
                response.Message = "Successfully retrieved all units.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Message = "Internal server error.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DoGetUnitById(string id)
        {
            DTOUnitGetSingleResponse response = new();
            try
            {
                response.unit = await _unitService.GetUnitByIdAsync(id);
                if (response.unit != null)
                {
                    response.Message = $"Successfully retrieved unit with id: {response.unit.Id}.";
                    return Ok(response);
                }

                return NotFound($"Did not find id: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Message = "Internal server error.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DoUpdateUnit(string id, [FromBody] DTOUnitNewRequest request)
        {
            try
            {
                DTOUnitGetSingleResponse response = new();

                var existingUnit = await _unitService.GetUnitByIdAsync(id) != null ? await _unitService.GetUnitByIdAsync(id): null;
                if (existingUnit != null)
                {
                    if (InputValidationUtils.IsValid(request.Name) && InputValidationUtils.IsValid(request.Type))
                    {
                        existingUnit.Name = request.Name;
                        existingUnit.IsActive = request.IsActive;
                        existingUnit.Type = request.Type;

                        var res = await _unitService.UpdateUnitAsync(id, existingUnit);

                        response.Message = $"Successfully updated unit";
                        response.unit = res;

                        _logger.LogInformation("Updating unit {UnitId} at {Time}", id, DateTime.Now);
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return NotFound($"Did not find id: {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DoDeleteUnit(string id)
        {
            try
            {
                DTOUnitGetSingleResponse response = new();
                response.unit = await _unitService.GetUnitByIdAsync(id);
                response.Message = $"Successfully deleted unit";

                var res = await _unitService.DeleteUnitAsync(id);
                if (!res)
                {
                    _logger.LogInformation("Deleting unit {UnitId} at {Time}", id, DateTime.Now);
                    return Ok(response);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}

