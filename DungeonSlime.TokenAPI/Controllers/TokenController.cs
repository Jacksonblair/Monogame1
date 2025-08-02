using System.Net;
using Microsoft.AspNetCore.Mvc;
using NetcodeIO.NET;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    EnvSettings envSettings;
    TokenFactory tokenFactory;

    public TokenController()
    {
        this.envSettings = new EnvSettings();

        tokenFactory = new TokenFactory(
            envSettings.ProtocolId, // must be the same protocol ID as passed to both client and server constructors
            envSettings.PrivateKey // byte[32], must be the same as the private key passed to the Server constructor
        );
    }

    [HttpGet]
    public IActionResult GetToken()
    {
        /**
            addressList,		// IPEndPoint[] list of addresses the client can connect to. Must have at least one and no more than 32.
            expirySeconds,		// in how many seconds will the token expire
            serverTimeout,		// how long it takes until a connection attempt times out and the client tries the next server.
            sequenceNumber,		// ulong token sequence number used to uniquely identify a connect token.
            clientID,		    // ulong ID used to uniquely identify this client
            userData		    // byte[], up to 256 bytes of arbitrary user data (available to the server as RemoteClient.UserData)
        */

        var endpoints = new IPEndPoint[]
        {
            new IPEndPoint(IPAddress.Parse(envSettings.ServerHost), envSettings.ServerPort) // Replace with real game server IP
        };

        int expirySeconds = 30;
        int serverTimeout = 10;
        ulong sequenceNumber = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // Simple unique-ish token
        ulong clientId = (ulong)new Random().NextInt64(); // You may want to replace this with something meaningful
        byte[] userData = new byte[0]; // Optional user data

        byte[] token = tokenFactory.GenerateConnectToken(
            endpoints,
            expirySeconds,
            serverTimeout,
            sequenceNumber,
            clientId,
            userData
        );

        return Ok(Convert.ToBase64String(token));
    }
}
