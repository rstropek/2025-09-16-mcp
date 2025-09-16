using Logic;

Console.WriteLine("=== Password Generator Demo ===");
Console.WriteLine();

// Example 1: Basic password generation without special characters
Console.WriteLine("1. Basic password (min length: 15, no special characters):");
var password1 = PasswordGenerator.GeneratePassword(15, false);
Console.WriteLine($"   Generated: {password1} (length: {password1.Length})");
Console.WriteLine();

// Example 2: Password with special character replacement
Console.WriteLine("2. Password with special characters (min length: 20):");
var password2 = PasswordGenerator.GeneratePassword(20, true);
Console.WriteLine($"   Generated: {password2} (length: {password2.Length})");
Console.WriteLine();

// Example 3: Longer password
Console.WriteLine("3. Longer password (min length: 50, with special characters):");
var password3 = PasswordGenerator.GeneratePassword(50, true);
Console.WriteLine($"   Generated: {password3} (length: {password3.Length})");
Console.WriteLine();

// Example 4: Demonstrate error handling
Console.WriteLine("4. Error handling demonstration:");
try
{
    PasswordGenerator.GeneratePassword(3, false);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"   ✓ Correctly caught exception: {ex.Message}");
}

try
{
    PasswordGenerator.GeneratePassword(1001, false);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"   ✓ Correctly caught exception: {ex.Message}");
}
Console.WriteLine();

Console.WriteLine("=== Demo Complete ===");
