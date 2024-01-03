
class Program
{
    static void Main()
    {
        int[] input = { 2, 2 };
        int desiredWhiteBalls = 1;
        int desiredBlackBalls = 2;
        string balls = TranslateToBalls(input);
        Console.WriteLine(balls);
        List<string> whiteSegments = GenerateSegments(balls, 'W', desiredWhiteBalls);
        List<string> blackSegments = GenerateSegments(balls, 'B', desiredBlackBalls);

        PrintSegments("White segments:", whiteSegments);
        PrintSegments("Black segments:", blackSegments);

        List<Tuple<string, string, int, int>> combinations = GenerateCombinations(whiteSegments, blackSegments);
        HashSet<string> uniqueCombinations = new HashSet<string>();

        Console.WriteLine("Valid Combinations - Static programming:");
        foreach (var combination in combinations)
        {
            if (combination.Item1 == balls)
            {
                string x = combination.Item2 + ", " + combination.Item3 + ", " + combination.Item4;
                Console.WriteLine(x);
                uniqueCombinations.Add(x);
            }
        }

        Console.WriteLine(uniqueCombinations.Count);

        Console.WriteLine("\nValid Combinations - Dynamic programming:");

        List<Tuple<string, string, int, int>> combinationsDP = GenerateCombinationsDP(whiteSegments, blackSegments);
        HashSet<string> uniqueCombinationsDP = new HashSet<string>();

        foreach (var combination in combinationsDP)
        {
            if (combination.Item1 == balls)
            {
                string y = combination.Item2 + ", " + combination.Item3 + ", " + combination.Item4;
                Console.WriteLine(y);
                uniqueCombinationsDP.Add(y);
            }
        }

        Console.WriteLine(uniqueCombinationsDP.Count);
    }

    static string TranslateToBalls(int[] input)
    {
        string balls = "";
        for (int i = 0; i < input.Length; i += 2)
        {
            balls += new string('W', input[i]);
            if (i + 1 < input.Length)
            {
                balls += new string('B', input[i + 1]);
            }
        }
        return balls;
    }

    static List<string> GenerateSegments(string balls, char color, int desiredNumberOfBalls)
    {
        List<string> segments = new List<string>();

        for(int i = 0; i < balls.Length; i++)
        {
            for(int j = i; j < balls.Length; j++)
            {
                int length = j - i + 1;
                var txt = balls.Substring(i, length);

                if (CountOccurrences(txt, color, desiredNumberOfBalls))
                {
                    segments.Add(txt);
                }
            }
        }

        return segments;
    }

    static bool CountOccurrences(string input, char targetChar, int desiredCount)
    {
        return input.Count(c => c == targetChar) == desiredCount;
    }

    static void PrintSegments(string title, List<string> segments)
    {
        Console.WriteLine(title);
        foreach (var segment in segments)
        {
            Console.WriteLine(segment);
        }
    }

    // Static programming
    // Time complexity is O(2^(whites + blacks)), Space complexity is O(whites + blacks) + stack
    static List<Tuple<string, string, int, int>> GenerateCombinations(List<string> whites, List<string> blacks)
    {
        List<Tuple<string, string, int, int>> combinations = new List<Tuple<string, string, int, int>>();
        GenerateCombinationsHelper("", "", 0, 0, 0, whites, blacks, combinations);

        return combinations;
    }

    static void GenerateCombinationsHelper(string currentCombination, string currentCombinationTranslated, int index, int whiteSegments, int blackSegments, List<string> whites, List<string> blacks, List<Tuple<string, string, int, int>> combinations)
    {
        if (index == whites.Count + blacks.Count)
        {
            combinations.Add(new Tuple<string, string, int, int>(currentCombination, currentCombinationTranslated, whiteSegments, blackSegments));
            return;
        }

        if (index < whites.Count)
        {
            GenerateCombinationsHelper(currentCombination + whites[index], currentCombinationTranslated + ", " + whites[index], index + 1, whiteSegments + 1, blackSegments, whites, blacks, combinations);
        }
        if (index < blacks.Count)
        {
            GenerateCombinationsHelper(currentCombination + blacks[index], currentCombinationTranslated + ", " + blacks[index], index + 1, whiteSegments, blackSegments + 1, whites, blacks, combinations);
        }

        // Cover all possibilities
        GenerateCombinationsHelper(currentCombination, currentCombinationTranslated, index + 1, whiteSegments, blackSegments, whites, blacks, combinations);
    }

    // Dynamic programming
    // Time complexity is O(whites * blacks), Space complexity is O(whites * blacks) + stack
    static List<Tuple<string, string, int, int>> GenerateCombinationsDP(List<string> whites, List<string> blacks)
    {
        Dictionary<Tuple<string, string, int, int>, bool> memo = new Dictionary<Tuple<string, string, int, int>, bool>();
        List<Tuple<string, string, int, int>> combinations = new List<Tuple<string, string, int, int>>();
        GenerateCombinationsHelperDP("", "", 0, 0, 0, whites, blacks, combinations, memo);

        return combinations;
    }

    static void GenerateCombinationsHelperDP(string currentCombination, string currentCombinationTranslated, int index, int whiteSegments, int blackSegments, List<string> whites, List<string> blacks, List<Tuple<string, string, int, int>> combinations, Dictionary<Tuple<string, string, int, int>, bool> memo)
    {
        if (index == whites.Count + blacks.Count)
        {
            combinations.Add(new Tuple<string, string, int, int>(currentCombination, currentCombinationTranslated, whiteSegments, blackSegments));
            return;
        }

        // Check if the state has been memoized
        var currentState = new Tuple<string, string, int, int>(currentCombination, currentCombinationTranslated, whiteSegments, blackSegments);
        if (memo.ContainsKey(currentState))
        {
            return;
        }

        if (index < whites.Count)
        {
            GenerateCombinationsHelperDP(currentCombination + whites[index], currentCombinationTranslated + ", " + whites[index], index + 1, whiteSegments + 1, blackSegments, whites, blacks, combinations, memo);
        }
        if (index < blacks.Count)
        {
            GenerateCombinationsHelperDP(currentCombination + blacks[index], currentCombinationTranslated + ", " + blacks[index], index + 1, whiteSegments, blackSegments + 1, whites, blacks, combinations, memo);
        }

        // Cover all possibilities
        GenerateCombinationsHelperDP(currentCombination, currentCombinationTranslated, index + 1, whiteSegments, blackSegments, whites, blacks, combinations, memo);

        // Memoize the current state
        memo[currentState] = true;
    }
}