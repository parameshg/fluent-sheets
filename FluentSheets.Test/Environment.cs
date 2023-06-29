namespace FluentSheets.Test
{
    public static class DotnetEnvironment
    {
        public static void Load()
        {
            if (!File.Exists(".env"))
                return;

            foreach (var line in File.ReadAllLines(".env"))
            {
                var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}