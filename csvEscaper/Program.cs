const string quote = "\"";

if (args.Length == 0)
{
    Console.WriteLine("Usage: csvEscaper <input file> [<output file>]");
    return;
}

if (args.Length == 1)
{
    CsvEscape(args[0], DecorateWithDateTime(args[0]));
}
else
{
    CsvEscape(args[0], args[1]);
}

void CsvEscape(string inputFile, string outputFile)
{
    // We will check every line and if it starts not with a date,
    // we will consider it as a part of the previous line and will escape it with quotes.

    var lines = File.ReadAllLines(inputFile);
    var escapedLines = new List<string>();
    var previousLine = string.Empty;
    var outputLines = new List<string>();

    foreach (var line in lines)
    {
        if (StartsWithDate(line))
        {
            AddPreviousLine(escapedLines, previousLine, outputLines);
            previousLine = line;
        }
        else
        {
            escapedLines.Add(line);
        }
    }

    AddPreviousLine(escapedLines, previousLine, outputLines);

    File.WriteAllLines(outputFile, outputLines);
    Console.WriteLine($"Processed {lines.Length} lines. Output contains {outputLines.Count} lines");

    static void AddPreviousLine(List<string> escapedLines, string previousLine, List<string> outputLines)
    {
        if (previousLine == string.Empty)
            return;

        if (escapedLines.Count > 0)
        {
            outputLines.Add(ReplaceLastItemWithEscapedStrings(previousLine, escapedLines));
            escapedLines.Clear();
        }
        else
        {
            outputLines.Add(previousLine);
        }
    }
}

static string ReplaceLastItemWithEscapedStrings(string line, List<string> escapedLines)
{
    var lastItem = line[^(line.Length - line.LastIndexOf(',') - 1)..];

    return string.Concat(line.AsSpan(0, line.Length - lastItem.Length), quote, string.Join('\n', [lastItem, .. escapedLines]), quote);
}

bool StartsWithDate(string line) => line.Length >= 10 && DateTime.TryParse(line.AsSpan(0, 10), out _);

string DecorateWithDateTime(string fileName) =>
    $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now:yyyyMMddHHmmss}.{Path.GetExtension(fileName)}";