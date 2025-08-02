using System;
using CommandLine;

namespace DungeonSlime.Server
{
    public class ServerStartOptions
    {
        [Option("host", Default = "127.0.0.1", HelpText = "Server hostname for client mode.")]
        public string Host { get; set; }

        [Option("port", Default = 12345, HelpText = "Port number.")]
        public int Port { get; set; }

        [Option("debug", Default = false, HelpText = "Enable debug output.")]
        public bool Debug { get; set; }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser
                .Default.ParseArguments<ServerStartOptions>(args)
                .WithParsed(opts =>
                {
                    Console.WriteLine($"Host: {opts.Host}, Port: {opts.Port}, Debug: {opts.Debug}");

                    using var game = new GameServer(opts);
                    game.Run();
                    // Start server or client based on opts.Mode
                })
                .WithNotParsed(errs =>
                {
                    // Handle errors
                });
        }
    }
}
