using System;
using System.IO;
using CommandLine;

namespace DungeonSlime
{
    public class ClientStartOptions { }

    public static class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (env == "Development")
            {
                DotNetEnv.Env.Load(
                    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".env")
                );
            }
            ;

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
