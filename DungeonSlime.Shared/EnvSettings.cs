using System;

public class EnvSettings
{
    public ulong ProtocolId { get; }
    public byte[] PrivateKey { get; }
    public string ServerHost { get; }
    public int ServerPort { get; }

    public EnvSettings()
    {
        ProtocolId = LoadProtocolId();
        PrivateKey = LoadPrivateKey();
        ServerHost = LoadServerHost();
        ServerPort = LoadServerPort();
    }

    private static ulong LoadProtocolId()
    {
        var str = Environment.GetEnvironmentVariable("PROTOCOL_ID");
        if (string.IsNullOrWhiteSpace(str) || !ulong.TryParse(str, out var val))
            throw new ArgumentException("Missing or invalid PROTOCOL_ID environment variable");
        return val;
    }

    private static byte[] LoadPrivateKey()
    {
        var base64 = Environment.GetEnvironmentVariable("PRIVATE_KEY");
        if (string.IsNullOrWhiteSpace(base64))
            throw new ArgumentException("Missing PRIVATE_KEY environment variable");
        try
        {
            return Convert.FromBase64String(base64);
        }
        catch (FormatException)
        {
            throw new ArgumentException("PRIVATE_KEY environment variable is not valid Base64");
        }
    }

    private static string LoadServerHost()
    {
        var host = Environment.GetEnvironmentVariable("SERVER_HOST");
        if (string.IsNullOrWhiteSpace(host))
            throw new ArgumentException("Missing SERVER_HOST environment variable");
        return host;
    }

    private static int LoadServerPort()
    {
        var str = Environment.GetEnvironmentVariable("SERVER_PORT");
        if (string.IsNullOrWhiteSpace(str) || !int.TryParse(str, out var val))
            throw new ArgumentException("Missing or invalid SERVER_PORT environment variable");
        return val;
    }
}
