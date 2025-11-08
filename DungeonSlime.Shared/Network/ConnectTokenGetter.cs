using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class GetConnectTokenResult
{
    public ulong ClientId { get; set; }
    public byte[] Token { get; set; }
}

public class ConnectTokenGetter
{
    private readonly HttpClient _httpClient;
    private readonly string _tokenEndpoint;

    public ConnectTokenGetter()
    {
        _tokenEndpoint = "http://" + EnvSettings.LoadTokenApiURL() + "/api/token";
        _httpClient = new HttpClient();
    }

    public async Task<GetConnectTokenResult> GetConnectTokenAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_tokenEndpoint);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            ulong clientId = root.GetProperty("clientId").GetUInt64();
            string base64Token = root.GetProperty("token").GetString() ?? string.Empty;

            byte[] token = Convert.FromBase64String(base64Token);

            return new GetConnectTokenResult { ClientId = clientId, Token = token };
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
