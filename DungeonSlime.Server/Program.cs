using System;
using System.IO;
using CommandLine;

namespace DungeonSlime.Server
{
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

            using var game = new GameServer();
            game.Run();
        }
    }
}
