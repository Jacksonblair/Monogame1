using System;
using CommandLine;

namespace DungeonSlime
{
    public class ClientStartOptions { }

    public static class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load("../.env");

            Parser
                .Default.ParseArguments<ClientStartOptions>(args)
                .WithParsed(opts =>
                {
                    // Console.WriteLine(
                    //     $"Mode: {opts.Mode}, Host: {opts.Host}, Port: {opts.Port}, Debug: {opts.Debug}"
                    // );

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
