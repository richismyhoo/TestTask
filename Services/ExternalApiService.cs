using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using Newtonsoft.Json;
using TestApp.Cache;

namespace TestApp.Services;

public class ExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApiCache _cache;

    public ExternalApiService(HttpClient httpClient, ExternalApiCache cache)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://webservices.oorsprong.org/");
        _cache = cache;
    }

    public async Task<string> GetCountriesXml()
    {
        if (_cache.GetCountriesFromCache() != null)
            return _cache.GetCountriesFromCache();
        
        string request = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <ListOfCountryNamesByCode xmlns=""http://www.oorsprong.org/websamples.countryinfo"">
    </ListOfCountryNamesByCode>
  </soap:Body>
</soap:Envelope>";

        var content = new StringContent(request, Encoding.UTF8, "text/xml");

        HttpResponseMessage response = await _httpClient.PostAsync(
            "websamples.countryinfo/CountryInfoService.wso",
            content
            );
        
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"External API call error: {response.StatusCode}, details: {responseBody}");
        
        _cache.SaveCountriesToCache(responseBody);
        return responseBody;
    }

    public async Task<string> GetCurrencyByCodeXml(string countryCode)
    {
        if (_cache.GetCurrencyFromCache(countryCode) != null) 
            return _cache.GetCurrencyFromCache(countryCode);
        
        string request = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <CountryCurrency xmlns=""http://www.oorsprong.org/websamples.countryinfo"">
      <sCountryISOCode>{countryCode}</sCountryISOCode>
    </CountryCurrency>
  </soap:Body>
</soap:Envelope>";
        
        var content = new StringContent(request, Encoding.UTF8, "text/xml");
        
        HttpResponseMessage response = await _httpClient.PostAsync(
            "websamples.countryinfo/CountryInfoService.wso",
            content
        );
        
        response.EnsureSuccessStatusCode();
        
        string responseBody = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"External API call error: {response.StatusCode}, details: {responseBody}");

        _cache.SaveCurrencyToCache(countryCode, responseBody);
        
        return responseBody;
    }
}