using Logic;

namespace LogicTests;

public class PasswordGeneratorTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(4)]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidateMinLength_ThrowsArgumentOutOfRangeException_WhenMinLengthIsLessThanOrEqualToFive(int minLength)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            PasswordGenerator.ValidateMinLength(minLength));
        
        Assert.Equal(nameof(minLength), exception.ParamName);
        Assert.Contains("must be greater than", exception.Message);
    }

    [Theory]
    [InlineData(1001)]
    [InlineData(2000)]
    [InlineData(int.MaxValue)]
    public void ValidateMinLength_ThrowsArgumentOutOfRangeException_WhenMinLengthIsGreaterThanThousand(int minLength)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            PasswordGenerator.ValidateMinLength(minLength));
        
        Assert.Equal(nameof(minLength), exception.ParamName);
        Assert.Contains("must be less than or equal to", exception.Message);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ValidateMinLength_DoesNotThrow_WhenMinLengthIsValid(int minLength)
    {
        // Act & Assert (should not throw)
        PasswordGenerator.ValidateMinLength(minLength);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(50)]
    public void SelectRandomWords_ReturnsWordsList_WithTotalLengthMeetingOrExceedingMinLength(int minLength)
    {
        // Act
        var words = PasswordGenerator.SelectRandomWords(minLength);
        
        // Assert
        Assert.NotEmpty(words);
        var totalLength = words.Sum(w => w.Length);
        Assert.True(totalLength >= minLength);
    }

    [Fact]
    public void SelectRandomWords_ReturnsUniqueWords_NoWordUsedTwice()
    {
        // Arrange
        const int minLength = 50;
        
        // Act
        var words = PasswordGenerator.SelectRandomWords(minLength);
        
        // Assert
        Assert.Equal(words.Count, words.Distinct().Count());
    }

    [Fact]
    public void ConvertToCamelCase_ReturnsEmptyString_WhenGivenEmptyList()
    {
        // Arrange
        var words = new List<string>();
        
        // Act
        var result = PasswordGenerator.ConvertToCamelCase(words);
        
        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ConvertToCamelCase_ReturnsSingleLowercaseWord_WhenGivenSingleWord()
    {
        // Arrange
        var words = new List<string> { "HELLO" };
        
        // Act
        var result = PasswordGenerator.ConvertToCamelCase(words);
        
        // Assert
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ConvertToCamelCase_ReturnsProperCamelCase_WhenGivenMultipleWords()
    {
        // Arrange
        var words = new List<string> { "hello", "WORLD", "test", "CASE" };
        
        // Act
        var result = PasswordGenerator.ConvertToCamelCase(words);
        
        // Assert
        Assert.Equal("helloWorldTestCase", result);
    }

    [Theory]
    [InlineData("hello", "h3ll0")]
    [InlineData("HELLO", "H3LL0")]
    [InlineData("test", "73$7")]
    [InlineData("Aesop", "@3$0p")]
    [InlineData("Items", "!73m$")]
    public void ReplaceSpecialCharacters_ReplacesCharactersCorrectly(string input, string expected)
    {
        // Act
        var result = PasswordGenerator.ReplaceSpecialCharacters(input);
        
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReplaceSpecialCharacters_DoesNotChangeOtherCharacters()
    {
        // Arrange
        const string input = "xyz123";
        
        // Act
        var result = PasswordGenerator.ReplaceSpecialCharacters(input);
        
        // Assert
        Assert.Equal("xyz123", result);
    }

    [Theory]
    [InlineData(6, false)]
    [InlineData(10, false)]
    [InlineData(20, false)]
    public void GeneratePassword_ReturnsPasswordMeetingMinLength_WithoutSpecialCharacters(int minLength, bool replaceSpecial)
    {
        // Act
        var password = PasswordGenerator.GeneratePassword(minLength, replaceSpecial);
        
        // Assert
        Assert.NotNull(password);
        Assert.True(password.Length >= minLength);
        // Should not contain special characters when replaceSpecial is false
        Assert.DoesNotContain('@', password);
        Assert.DoesNotContain('$', password);
        Assert.DoesNotContain('0', password);
        Assert.DoesNotContain('!', password);
        Assert.DoesNotContain('3', password);
        Assert.DoesNotContain('7', password);
    }

    [Theory]
    [InlineData(6, true)]
    [InlineData(10, true)]
    [InlineData(20, true)]
    public void GeneratePassword_ReturnsPasswordMeetingMinLength_WithSpecialCharacters(int minLength, bool replaceSpecial)
    {
        // Act
        var password = PasswordGenerator.GeneratePassword(minLength, replaceSpecial);
        
        // Assert
        Assert.NotNull(password);
        Assert.True(password.Length >= minLength);
        // Should not contain original characters that should be replaced
        Assert.DoesNotContain('a', password);
        Assert.DoesNotContain('A', password);
        Assert.DoesNotContain('s', password);
        Assert.DoesNotContain('S', password);
        Assert.DoesNotContain('o', password);
        Assert.DoesNotContain('O', password);
        Assert.DoesNotContain('i', password);
        Assert.DoesNotContain('I', password);
        Assert.DoesNotContain('e', password);
        Assert.DoesNotContain('E', password);
        Assert.DoesNotContain('t', password);
        Assert.DoesNotContain('T', password);
    }

    [Fact]
    public void GeneratePassword_ProducesDifferentResults_OnMultipleCalls()
    {
        // Arrange
        const int minLength = 15;
        const bool replaceSpecial = false;
        
        // Act
        var password1 = PasswordGenerator.GeneratePassword(minLength, replaceSpecial);
        var password2 = PasswordGenerator.GeneratePassword(minLength, replaceSpecial);
        var password3 = PasswordGenerator.GeneratePassword(minLength, replaceSpecial);
        
        // Assert
        // It's extremely unlikely that all three passwords would be identical
        Assert.False(password1 == password2 && password2 == password3, 
            "All three passwords should not be identical");
    }

    [Theory]
    [InlineData(5)]
    [InlineData(1001)]
    public void GeneratePassword_ThrowsArgumentOutOfRangeException_WhenMinLengthIsInvalid(int minLength)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            PasswordGenerator.GeneratePassword(minLength, false));
    }

    [Fact]
    public void GeneratePassword_ProducesCamelCaseFormat()
    {
        // Arrange
        const int minLength = 15;
        const bool replaceSpecial = false;
        
        // Act
        var password = PasswordGenerator.GeneratePassword(minLength, replaceSpecial);
        
        // Assert
        Assert.True(char.IsLower(password[0]), "First character should be lowercase");
        
        // Check that there are some uppercase letters (indicating word boundaries in camelCase)
        Assert.True(password.Any(char.IsUpper), "Password should contain some uppercase letters");
    }

    #region ValidatePasswordCount Tests
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void ValidatePasswordCount_ThrowsArgumentOutOfRangeException_WhenCountIsLessThanOne(int count)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            PasswordGenerator.ValidatePasswordCount(count));
        
        Assert.Equal(nameof(count), exception.ParamName);
        Assert.Contains("must be greater than or equal to", exception.Message);
    }

    [Theory]
    [InlineData(101)]
    [InlineData(200)]
    [InlineData(int.MaxValue)]
    public void ValidatePasswordCount_ThrowsArgumentOutOfRangeException_WhenCountIsGreaterThanHundred(int count)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            PasswordGenerator.ValidatePasswordCount(count));
        
        Assert.Equal(nameof(count), exception.ParamName);
        Assert.Contains("must be less than or equal to", exception.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(50)]
    [InlineData(100)]
    public void ValidatePasswordCount_DoesNotThrow_WhenCountIsValid(int count)
    {
        // Act & Assert (should not throw)
        PasswordGenerator.ValidatePasswordCount(count);
    }

    #endregion

    #region GeneratePasswords Tests

    [Theory]
    [InlineData(1, 10, false)]
    [InlineData(5, 15, false)]
    [InlineData(10, 20, true)]
    [InlineData(50, 8, false)]
    [InlineData(100, 12, true)]
    public void GeneratePasswords_ReturnsCorrectNumberOfPasswords(int count, int minLength, bool replaceSpecial)
    {
        // Act
        var passwords = PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecial);
        
        // Assert
        Assert.Equal(count, passwords.Count);
    }

    [Theory]
    [InlineData(5, 10, false)]
    [InlineData(10, 15, true)]
    public void GeneratePasswords_AllPasswordsMeetMinimumLength(int count, int minLength, bool replaceSpecial)
    {
        // Act
        var passwords = PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecial);
        
        // Assert
        Assert.All(passwords, password => 
            Assert.True(password.Length >= minLength, 
                $"Password '{password}' length {password.Length} should be >= {minLength}"));
    }

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(10, 15, true)]
    public void GeneratePasswords_AllPasswordsHaveSpecialCharactersReplaced_WhenReplaceSpecialIsTrue(int count, int minLength, bool replaceSpecial)
    {
        // Act
        var passwords = PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecial);
        
        // Assert
        Assert.All(passwords, password =>
        {
            // Should not contain original characters that should be replaced
            Assert.DoesNotContain('a', password);
            Assert.DoesNotContain('A', password);
            Assert.DoesNotContain('s', password);
            Assert.DoesNotContain('S', password);
            Assert.DoesNotContain('o', password);
            Assert.DoesNotContain('O', password);
            Assert.DoesNotContain('i', password);
            Assert.DoesNotContain('I', password);
            Assert.DoesNotContain('e', password);
            Assert.DoesNotContain('E', password);
            Assert.DoesNotContain('t', password);
            Assert.DoesNotContain('T', password);
        });
    }

    [Theory]
    [InlineData(5, 10, false)]
    [InlineData(10, 15, false)]
    public void GeneratePasswords_AllPasswordsInCamelCaseFormat(int count, int minLength, bool replaceSpecial)
    {
        // Act
        var passwords = PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecial);
        
        // Assert
        Assert.All(passwords, password =>
        {
            Assert.True(char.IsLower(password[0]), 
                $"First character of password '{password}' should be lowercase");
            Assert.True(password.Any(char.IsUpper), 
                $"Password '{password}' should contain some uppercase letters");
        });
    }

    [Fact]
    public void GeneratePasswords_ProducesDifferentPasswords()
    {
        // Arrange
        const int count = 10;
        const int minLength = 15;
        const bool replaceSpecial = false;
        
        // Act
        var passwords = PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecial);
        var uniquePasswords = passwords.Distinct().ToList();
        
        // Assert
        // It's extremely unlikely that all passwords would be identical with random generation
        Assert.True(uniquePasswords.Count > 1, 
            "Should generate multiple different passwords");
    }

    [Theory]
    [InlineData(0, 10, false)]
    [InlineData(-1, 10, false)]
    [InlineData(101, 10, false)]
    public void GeneratePasswords_ThrowsArgumentOutOfRangeException_WhenCountIsInvalid(int count, int minLength, bool replaceSpecial)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecial));
    }

    [Theory]
    [InlineData(5, 5, false)]
    [InlineData(5, 1001, false)]
    public void GeneratePasswords_ThrowsArgumentOutOfRangeException_WhenMinLengthIsInvalid(int count, int minLength, bool replaceSpecial)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecial));
    }

    #endregion
}
