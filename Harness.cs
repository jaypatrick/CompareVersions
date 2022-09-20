namespace CompareVersions;

internal static class Harness
{
    internal static void Run(IList<string> args)
    {
        Console.WriteLine("Hello, World!");
        string version1 = args.Count > 0 ? args[0] : string.Empty;
        string version2 = args.Count > 0 ? args[1] : string.Empty;

        string inputVersion = "Input version string in <xx>.<xx>.<xx>.<xx> format";

        Console.Write("Welcome to CompareVersions. Would you like random version strings? y/n ");
        string createRandomVersion = Console.ReadLine();

        if (createRandomVersion == "y" || createRandomVersion == "Y")
        {
            Console.Write("Would you like to use equal version strings? y/n ");
            string useEqual = Console.ReadLine();

            if (useEqual == "y" || useEqual == "Y")
            {
                version1 = version2 = createRandomVersionString();
            }
            else
            {
                version1 = createRandomVersionString();
                version2 = createRandomVersionString();
            }

            Span<char> destination1, destination2 = new();

            // TODO: Work on the TryFormat methods
            // TODO: Work on TryParse methods
        }
        else
        {
            Console.WriteLine(inputVersion);
            version1 = Console.ReadLine();

            Console.WriteLine(inputVersion);
            version2 = Console.ReadLine();
        }

        Console.WriteLine($"The version strings are {version1} and {version2}");

        var result = new ComparisonOperations().CompareVersions(version1, version2);
        writeToConsole_1();

        void writeToConsole_1()
        {
            string isLessThan = "is less than";
            string isGreaterThan = "is greater than";
            string isEqualTo = "is equal to";
            string theResultWas = "The result was";

            if (result < 0) Console.WriteLine($"{version1} {isLessThan} {version2}: {theResultWas} {result}");
            if (result == 0) Console.WriteLine($"{version1} {isEqualTo} {version2}: {theResultWas} {result}");
            if (result > 0) Console.WriteLine($"{version1} {isGreaterThan} {version2}: {theResultWas} {result}");
        }

        Console.WriteLine("Have another go? y/n");
        string again = Console.ReadLine();

        if (again != "N" || again != "n")
        {
            Console.WriteLine("Firing it up...");
            Run(args);
        }

        Console.ReadLine();
    }

    /// <summary>
    /// Creates the random version string.
    /// </summary>
    /// <returns>A <see cref="String"/>value of the version.</returns>
    private static string createRandomVersionString()
    {
        int floor = 0;
        int ceiling = 99;
        char dot = '.';
        StringBuilder versionString = new StringBuilder();

        for (int i = 1; i <= 4; i++)
        {

            var quad = RandomNumberGenerator.GetInt32(floor, ceiling);

            versionString.Append(quad);
            versionString.Append(dot);
        }

        versionString.Length--;
        return versionString.ToString();
    }
}

