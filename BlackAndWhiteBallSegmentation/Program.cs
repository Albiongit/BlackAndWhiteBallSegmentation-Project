
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

        Console.WriteLine("Valid Combinations:");
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
}