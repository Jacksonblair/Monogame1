using System;

public static class EnvSettings
{
    // public ulong ProtocolId { get; }
    // public byte[] PrivateKey { get; }
    // public string ServerHost { get; }
    // public int ServerPort { get; }
    // public string TokenApiHost { get; }
    // public int TokenApiPort { get; }
    // public string EnvironmentMode { get; }

    // public EnvSettings()
    // {
    //     ProtocolId = LoadProtocolId();
    //     PrivateKey = LoadPrivateKey();
    //     ServerHost = LoadServerHost();
    //     ServerPort = LoadServerPort();
    //     TokenApiHost = LoadTokenApiHost();
    //     TokenApiPort = LoadTokenApiPort();
    //     EnvironmentMode = LoadEnvironmentMode();
    // }

    public static string LoadEnvironmentMode()
    {
        var env = Environment.GetEnvironmentVariable("ENVIRONMENT");
        if (string.IsNullOrWhiteSpace(env))
            throw new ArgumentException("Missing ENVIRONMENT environment variable");
        return env;
    }

    public static int LoadTokenApiPort()
    {
        var str = Environment.GetEnvironmentVariable("TOKEN_API_PORT");
        if (string.IsNullOrWhiteSpace(str) || !int.TryParse(str, out var val))
            throw new ArgumentException("Missing or invalid TOKEN_API_PORT environment variable");
        return val;
    }

    public static string LoadTokenApiHost()
    {
        var host = Environment.GetEnvironmentVariable("TOKEN_API_HOST");
        if (string.IsNullOrWhiteSpace(host))
            throw new ArgumentException("Missing TOKEN_API_HOST environment variable");
        return host;
    }

    public static string LoadTokenApiURL()
    {
        return $"{EnvSettings.LoadTokenApiHost()}:{EnvSettings.LoadTokenApiPort()}";
    }

    public static ulong LoadProtocolId()
    {
        var str = Environment.GetEnvironmentVariable("PROTOCOL_ID");
        if (string.IsNullOrWhiteSpace(str) || !ulong.TryParse(str, out var val))
            throw new ArgumentException("Missing or invalid PROTOCOL_ID environment variable");
        return val;
    }

    public static byte[] LoadPrivateKey()
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

    public static string LoadServerHost()
    {
        var host = Environment.GetEnvironmentVariable("SERVER_HOST");
        if (string.IsNullOrWhiteSpace(host))
            throw new ArgumentException("Missing SERVER_HOST environment variable");
        return host;
    }

    public static int LoadServerPort()
    {
        var str = Environment.GetEnvironmentVariable("SERVER_PORT");
        if (string.IsNullOrWhiteSpace(str) || !int.TryParse(str, out var val))
            throw new ArgumentException("Missing or invalid SERVER_PORT environment variable");
        return val;
    }
}
