using System.Text;

namespace Logic;

/// <summary>
/// Generates passwords by concatenating random words from a predefined list.
/// </summary>
public static class PasswordGenerator
{

    /// <summary>
    /// Generates a password by concatenating random words in camelCase format.
    /// </summary>
    /// <param name="minLength">The minimum length of the generated password. Must be between 6 and 1000.</param>
    /// <param name="replaceSpecialCharacters">If true, replaces certain characters with special symbols.</param>
    /// <returns>A password string meeting the specified criteria.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minLength is less than or equal to 5, or greater than 1000.</exception>
    public static string GeneratePassword(int minLength, bool replaceSpecialCharacters)
    {
        ValidateMinLength(minLength);
        
        var selectedWords = SelectRandomWords(minLength);
        var camelCasePassword = ConvertToCamelCase(selectedWords);
        
        if (replaceSpecialCharacters)
        {
            camelCasePassword = ReplaceSpecialCharacters(camelCasePassword);
        }
        
        return camelCasePassword;
    }

    /// <summary>
    /// Validates that the minimum length parameter is within acceptable bounds.
    /// </summary>
    /// <param name="minLength">The minimum length to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minLength is out of range.</exception>
    internal static void ValidateMinLength(int minLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(minLength, 5, nameof(minLength));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minLength, 1000, nameof(minLength));
    }

    /// <summary>
    /// Selects random words from the frequent words list without duplicates until the minimum length is reached.
    /// </summary>
    /// <param name="minLength">The minimum total length required.</param>
    /// <returns>A list of unique words that when combined meet or exceed the minimum length.</returns>
    internal static List<string> SelectRandomWords(int minLength)
    {
        var selectedWords = new List<string>();
        var usedIndices = new HashSet<int>();
        var currentLength = 0;
        
        while (currentLength < minLength)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Shared.Next(FrequentWordList.FrequentWords.Length);
            } 
            while (usedIndices.Contains(randomIndex));
            
            usedIndices.Add(randomIndex);
            var word = FrequentWordList.FrequentWords[randomIndex];
            selectedWords.Add(word);
            currentLength += word.Length;
        }
        
        return selectedWords;
    }

    /// <summary>
    /// Converts a list of words to camelCase format (first word lowercase, subsequent words capitalized).
    /// </summary>
    /// <param name="words">The words to convert to camelCase.</param>
    /// <returns>A camelCase string.</returns>
    internal static string ConvertToCamelCase(List<string> words)
    {
        if (words.Count == 0)
            return string.Empty;
        
        var result = new StringBuilder();
        
        // First word should be lowercase
        result.Append(words[0].ToLowerInvariant());
        
        // Subsequent words should have their first letter capitalized
        for (int i = 1; i < words.Count; i++)
        {
            var word = words[i];
            if (word.Length > 0)
            {
                result.Append(char.ToUpperInvariant(word[0]));
                if (word.Length > 1)
                {
                    result.Append(word.Substring(1).ToLowerInvariant());
                }
            }
        }
        
        return result.ToString();
    }

    /// <summary>
    /// Replaces specific characters with special symbols according to the specification.
    /// </summary>
    /// <param name="input">The input string to process.</param>
    /// <returns>The string with character replacements applied.</returns>
    internal static string ReplaceSpecialCharacters(string input)
    {
        var result = new StringBuilder(input);
        
        // Replace characters according to specification
        result.Replace('a', '@');
        result.Replace('A', '@');
        result.Replace('s', '$');
        result.Replace('S', '$');
        result.Replace('o', '0');
        result.Replace('O', '0');
        result.Replace('i', '!');
        result.Replace('I', '!');
        result.Replace('e', '3');
        result.Replace('E', '3');
        result.Replace('t', '7');
        result.Replace('T', '7');
        
        return result.ToString();
    }
}