using System;
using System.Net.Http;
using System.Threading.Tasks;

public class ConnectTokenGetter
{
    private readonly HttpClient _httpClient;
    private readonly string _tokenEndpoint;

    public ConnectTokenGetter()
    {
        _tokenEndpoint = "http://" + EnvSettings.LoadTokenApiURL() + "/api/token";
        _httpClient = new HttpClient();
    }

    public async Task<byte[]> GetConnectTokenAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_tokenEndpoint);
        response.EnsureSuccessStatusCode();

        string base64Token = await response.Content.ReadAsStringAsync();

        try
        {
            return Convert.FromBase64String(base64Token);
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException(
                "Received an invalid base64 token from the server.",
                ex
            );
        }
    }
}
