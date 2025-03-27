namespace TestApp.Cache;

public class ExternalApiCache
{
    private string? _countriesXml { get; set; }
    private Dictionary<string, string> _currencies { get; set; }

    public ExternalApiCache()
    {
        _currencies = new Dictionary<string, string>();
    }
    public void SaveCountriesToCache(string countriesXml)
    {
        _countriesXml = countriesXml;
    }

    public string? GetCountriesFromCache()
    {
        return _countriesXml;
    }

    public void SaveCurrencyToCache(string countryCode, string currencyXml)
    {
        _currencies.Add(countryCode, currencyXml);
    }

    public string? GetCurrencyFromCache(string countryCode)
    {
        return _currencies.ContainsKey(countryCode) ? _currencies[countryCode] : null;
    } 
}