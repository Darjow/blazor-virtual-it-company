using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

public class MockConfiguration : IConfiguration
{
    private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

    public MockConfiguration()
    {
        _values["JwtSecretKey"] = "WatEenMoeilijkeSecretKeyIsDitVindJeNietBlaBlaBla";
    }



    public string this[string key]
    {
        get
        {
            if (_values.ContainsKey(key))
            {
                return _values[key];
            }
            return null;
        }
        set => _values[key] = value;
    }
    public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();
    public IChangeToken GetReloadToken() => throw new NotImplementedException();
    public IConfigurationSection GetSection(string key) => throw new NotImplementedException();
}
