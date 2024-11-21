using KmlAPI.Models;
using KmlAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/placemarks")]
public class PlacemarkController : ControllerBase
{
    private readonly IPlacemarkService _service;

    public PlacemarkController(IPlacemarkService service)
    {
        _service = service;
    }

    [HttpPost("export")]
    public IActionResult ExportFilteredKML([FromBody] FilterModel filters)
    {
        var filtered = _service.FilterPlacemarks(filters);

        if (!filtered.Any())
            return BadRequest("No results found for the provided filters.");

        var file = _service.ExportFilteredKML(filtered);

        return File(file, "application/vnd.google-earth.kml+xml", "filtered.kml");
    }

    [HttpGet]
    public IActionResult GetFilteredPlacemarks([FromQuery] FilterModel filters)
    {
        var filtered = _service.FilterPlacemarks(filters);

        if (!filtered.Any())
            return BadRequest("No results found for the provided filters.");

        return Ok(filtered);
    }

    [HttpGet("filters")]
    public IActionResult GetAvailableFilters()
    {
        var clientes = _service.GetUniqueValues("CLIENTE");
        var situacoes = _service.GetUniqueValues("SITUAÇÃO");
        var bairros = _service.GetUniqueValues("BAIRRO");

        return Ok(new { Clientes = clientes, Situacoes = situacoes, Bairros = bairros });
    }
}