using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using TestApp.Dtos;

namespace TestApp.Services;

public class JsonExternalApiService
{
    private readonly ExternalApiService _externalApiService;

    public JsonExternalApiService(ExternalApiService externalApiService)
    {
        _externalApiService = externalApiService;
    }
    
    public async Task<string> GetCurrencyByCodeJson(string countryCode)
    {
        var xml = await _externalApiService.GetCurrencyByCodeXml(countryCode);
        var json = ConvertXmlToJson<CurrencyInfo>(xml);
        return json;
    }

    public async Task<string> GetCountriesJson()
    {
        var xml = await _externalApiService.GetCountriesXml();
        var json = ConvertXmlToJson<CountriesResponse>(xml);
        return json;
    }
    
    private string ConvertXmlToJson<T>(string xml)
    {
        try
        {
            XDocument xmlDoc = XDocument.Parse(xml);

            if (typeof(T) == typeof(CurrencyInfo))
            {
                // Извлечение данных для валюты
                var isoCode = xmlDoc.Descendants("{http://www.oorsprong.org/websamples.countryinfo}sISOCode").FirstOrDefault()?.Value;
                var name = xmlDoc.Descendants("{http://www.oorsprong.org/websamples.countryinfo}sName").FirstOrDefault()?.Value;

                var result = new CurrencyInfo
                {
                    IsoCode = isoCode,
                    Name = name
                };

                return JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            else if (typeof(T) == typeof(CountriesResponse))
            {
                var countries = xmlDoc.Descendants("{http://www.oorsprong.org/websamples.countryinfo}tCountryCodeAndName")
                    .Select(country => new CountryInfo
                    {
                        IsoCode = country.Element("{http://www.oorsprong.org/websamples.countryinfo}sISOCode")?.Value,
                        Name = country.Element("{http://www.oorsprong.org/websamples.countryinfo}sName")?.Value
                    })
                    .ToList();

                var result = new CountriesResponse
                {
                    Countries = countries
                };

                return JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }

            throw new InvalidOperationException("Unsupported type for XML conversion.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting XML to JSON: {ex.Message}");
            throw;
        }
    }
}
