global using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using LagerhotellAPI.Models.DomainModels.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("assets")]
public class AssetsController : ControllerBase
{
    private readonly AssetService _assetService;
    private readonly ImageAssetValidator _imageAssetValidator = new();

    public AssetsController(AssetService assetService)
    {
        _assetService = assetService;
    }

    // Add validation of requests here

    [HttpPost]
    [Route("add")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddAsset([FromBody] AddImageAssetRequest request)
    {
        try
        {
            _imageAssetValidator.ValidateAndThrow(request.Asset);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex);
        }
        try
        {
            string assetId = await _assetService.AddAsset(request.Asset);
            return Ok(new AddAssetResponse() { AssetId = assetId });
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"{ex}");
            return Conflict("Asset already exists");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{assetId}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAsset([FromRoute] string assetId)
    {
        try
        {
            await _assetService.DeleteAsset(assetId);
            return Ok("Asset was deleted");
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

    [HttpPut]
    [Route("modify")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ModifyAsset([FromBody] ModifyAssetRequest request)
    {
        try
        {
            _imageAssetValidator.ValidateAndThrow(request.Asset);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }

        try
        {
            await _assetService.ModifyAsset(request.AssetId, request.Asset);
            return Ok("Asset was modified");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("{assetId}")]
    public async Task<IActionResult> GetAsset([FromRoute] string assetId)
    {
        ImageAsset? asset = await _assetService.GetAsset(assetId);
        if (asset == null)
        {
            return NotFound();
        }
        return Ok(asset);
    }

    [HttpGet("{skip}/{take}/{warehouseHotelId}")]
    public async Task<IActionResult> GetAllAssets(int? skip, int? take, string? warehouseHotelId)
    {
        try
        {
            List<ImageAsset> assets = await _assetService.GetAssets(skip, take, warehouseHotelId);
            return Ok(new GetAllAssetsResponse() { Assets = assets });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex}");
            return StatusCode(500);
        }
    }
}
