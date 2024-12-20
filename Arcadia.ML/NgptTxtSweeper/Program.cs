using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NgptTxtSweeper;

class Program
{
    static void Main(string[] args)
    {
        // Ensure the correct number of arguments are provided
        if (args.Length < 3 || args.Length > 4)
        {
            Console.WriteLine("Usage: Program <inputFile> <outputFile> <searchString> [flag]");
            return;
        }

        string inputFile = args[0];
        string outputFile = args[1];
        string searchString = args[2];
        bool appendSearchString = args.Length == 4 && args[3].ToLower() == "true"; // If arg4 is "true", we append the searchString

        // Check if input file exists
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Error: Input file '{inputFile}' not found.");
            return;
        }

        try
        {
            // Check and delete output file if exists
            try
            {
                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                    Console.WriteLine($"File '{outputFile}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"File '{outputFile}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                return;
            }

            // Read all lines from the input file
            var lines = File.ReadAllLines(inputFile);

            // Process the lines
            var processedLines = lines
                .Where(line => Regex.IsMatch(line, $"^{Regex.Escape(searchString)}")) // Match lines starting with search string
                .Select(line => ProcessLine(line, searchString, appendSearchString)) // Apply rules to each line
                .Where(line => !string.IsNullOrWhiteSpace(line)) // Remove blank lines
                .Where(line => line != null) // Explicitly filter out nulls
                .Cast<string>() // Ensure we are working with IEnumerable<string> (non-nullable)
                .ToList();

            // Write the processed lines to the output file
            File.WriteAllLines(outputFile, processedLines);

            Console.WriteLine($"Processing complete. Output saved to '{outputFile}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    // Process the line according to the rules specified
    static string? ProcessLine(string line, string searchString, bool appendSearchString)
    {
        // Rule 0: Strip the searchString (including any space after it) at the start of the line
        if (line.StartsWith(searchString))
        {
            line = line.Substring(searchString.Length).TrimStart(); // Remove the searchString and any space after it
        }

        // Rule (a): Any line starting with 'Paraphrase: ' must have that section removed
        if (line.StartsWith("Paraphrase:"))
        {
            line = line.Substring("Paraphrase:".Length).TrimStart(); // Remove any space after it
        }

        // Rule (b): Any line starting with a word followed by ':' character, remove up to and including the ':'
        line = Regex.Replace(line, @"^\S+: ", "");

        // Rule (c): Any line that contains a word longer than 12 characters must be removed
        if (line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Any(word => word.Length > 12))
        {
            return null; // Remove the line by returning null
        }

        // Rule (d): Any line that has no alphabetic characters must be removed
        if (!line.Any(c => Char.IsLetter(c)))
        {
            return null;
        }

        // Rule (e): Any line that ends with ":" should be removed
        if (line.EndsWith(":"))
        {
            return null;
        }

        // Rule (f): Any line with more than two consecutive non-alpha characters should be removed
        if (Regex.IsMatch(line, @"[^a-zA-Z]{3,}"))
        {
            return null;
        }

        // Rule (g): Any line that starts with a non-alphabetic character should be removed
        if (line.Length > 0 && !Char.IsLetter(line[0]))
        {
            return null;
        }

        // Rule (h): Any line starting with 'Paraphrase' (with variation) should be removed
        if (line.StartsWith("Paraphrase"))
        {
            return null; // Remove the line
        }

        // Rule (i): Any line that is a single word must be removed
        if (line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
        {
            return null; // Remove the line by returning null
        }

        // If the flag is true, append \t{searchString} to the line
        if (appendSearchString)
        {
            line = $"{line}\t{searchString}";
        }

        // Return the processed line if it's not null or empty
        return line;
    }
}
