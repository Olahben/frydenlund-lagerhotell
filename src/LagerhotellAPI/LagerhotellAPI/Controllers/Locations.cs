using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("locations")]
public class LocationsController : ControllerBase
{
    private readonly LocationService _locationService;
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(LocationService locationService, ILogger<LocationsController> logger)
    {
        _locationService = locationService;
        _logger = logger;
    }

    [HttpPost]
    [Route("add")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddLocation([FromBody] AddLocationRequest request)
    {
        try
        {
            await _locationService.Add(request.Location);
            return Ok("Location was added");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error in Locations controller", ex);
            return Conflict("Error in adding location, location probably already exists");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AddLocation");
            return StatusCode(500);
        }
    }

    [HttpDelete("{name}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteLocation([FromRoute] string name)
    {
        try
        {
            await _locationService.Delete(name);
            return Ok("Location was deleted");
        }
        catch (InvalidOperationException ex)
        {
            return Conflict();
        } catch (Exception e)
        {
            _logger.LogError(e, "Error in DeleteLocation");
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = "Administrator")]
    [Route("modify")]
    public async Task<IActionResult> ModifyLocation([FromBody] ModifyLocationRequest request)
    {
        try
        {
            await _locationService.Modify(request.Location, request.OldLocationName);
            return Ok($"Location with the name {request.OldLocationName} was successfully changed to {request.Location.Name}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error in locations controller ModifyLocation, location does probably not exist. Error: {ex}");
            return Conflict();
        } catch (Exception e)
        {
            _logger.LogError(e, "Error in ModifyLocation");
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLocations([FromQuery] bool includeNonActive, int? skip, int? take)
    {
        try
        {
            List<Location> locations = await _locationService.GetAll(includeNonActive, skip, take);
            return Ok(new GetAllLocationsResponse(locations));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllLocations");
            return StatusCode(500, ex.Message);
        }
    }
}
