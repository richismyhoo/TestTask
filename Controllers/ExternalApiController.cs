using Microsoft.AspNetCore.Mvc;
using TestApp.Services;

namespace TestApp.Controllers;

[ApiController]
[Route("externalApi")]
public class ExternalApiController : ControllerBase
{
    private readonly ExternalApiService _externalApiService;
    private readonly JsonExternalApiService _jsonExternalApiService;

    public ExternalApiController(ExternalApiService externalApiService, JsonExternalApiService jsonExternalApiService)
    {
        _externalApiService = externalApiService;
        _jsonExternalApiService = jsonExternalApiService;
    }

    [HttpGet("getCountriesXml")]
    public async Task<ActionResult> GetCountriesByIso()
    {
        try
        {
            var countries = await _externalApiService.GetCountriesXml();
            return Ok(countries);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("getCurrencyByIsoXml/{isoCode}")]
    public async Task<ActionResult> GetCurrencyByIso(string isoCode)
    {
        try
        {
            var currency = await _externalApiService.GetCurrencyByCodeXml(isoCode);
            return Ok(currency);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("getCountriesJson")]
    public async Task<ActionResult> GetCountriesJson()
    {
        try
        {
            var countries = await _jsonExternalApiService.GetCountriesJson();
            return Ok(countries);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("getCurrencyByIsoJson/{isoCode}")]
    public async Task<ActionResult> GetCurrencyByIsoJson(string isoCode)
    {
        try
        {
            var currency = await _jsonExternalApiService.GetCurrencyByCodeJson(isoCode);
            return Ok(currency);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}