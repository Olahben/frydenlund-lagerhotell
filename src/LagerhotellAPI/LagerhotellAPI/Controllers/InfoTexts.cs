global using LagerhotellAPI.Models.ValueTypes;
global using LagerhotellAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LagerhotellAPI.Controllers;

[ApiController]
[Route("info-texts")]
public class InfoTextsController : ControllerBase
{
    private readonly InfoTextService _infoTextService;

    public InfoTextsController(InfoTextService infoTextService)
    {
        _infoTextService = infoTextService;
    }

    [HttpGet("${skip}/{take}")]
    public async Task<IActionResult> GetInfoTexts([FromRoute] int? skip, [FromRoute] int? take)
    {
        List<InfoText> infoTexts = await _infoTextService.GetInfoTexts(skip, take);
        return Ok(new GetInfoTexts() { InfoTexts = infoTexts });
    }

    [HttpGet]
    [Route("storage-unit")]
    public async Task<IActionResult> GetInfoTextStorageUnit([FromRoute] StorageUnitSizesGroup? sizeGroup)
    {
        InfoText? infoText = await _infoTextService.GetInfoTextStorageUnit(sizeGroup);
        if (infoText == null)
        {
            return NotFound();
        }
        return Ok(infoText);
    }

    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateInfoText([FromBody] CreateInfoTextRequest request)
    {
        try
        {
            string infoTextId = await _infoTextService.CreateInfoText(request.InfoText);
            return Ok(new CreateInfoTextResponse() { InfoTextId = infoTextId });
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"{ex}");
            return Conflict("InfoText already exists");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{infoTextId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteInfoText([FromRoute] string infoTextId)
    {
        try
        {
            await _infoTextService.DeleteInfoText(infoTextId);
            return Ok("InfoText was deleted");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut]
    [Route("modify")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ModifyInfoText([FromBody] ModifyInfoText request)
    {
        try
        {
            await _infoTextService.ModifyInfoText(request.InfoTextId, request.InfoText);
            return Ok("InfoText was modified");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }
}
