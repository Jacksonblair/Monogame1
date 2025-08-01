using System;
using CommandLine;

namespace DungeonSlime
{
    public class ProgramStartOptions
    {
        [Option("mode", Default = "client", HelpText = "Mode to run: server or client.")]
        public string Mode { get; set; }

        [Option("host", Default = "localhost", HelpText = "Server hostname for client mode.")]
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
                .Default.ParseArguments<ProgramStartOptions>(args)
                .WithParsed(opts =>
                {
                    Console.WriteLine(
                        $"Mode: {opts.Mode}, Host: {opts.Host}, Port: {opts.Port}, Debug: {opts.Debug}"
                    );

                    using var game = new Game1(opts);
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
