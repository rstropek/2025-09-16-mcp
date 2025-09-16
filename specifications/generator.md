# Password Generator

We write a password generator that builds passwords by concatenating random words (see [FrequentWords.cs](../Logic/FrequentWords.cs)). The generated password must be camelCase. One word must only be used once in a password.

The generator accepts a minimal length parameter. The generated password must be at least as long as the minimal length parameter. If the minimal length is less than or equal to five, the generator must throw an `ArgumentOutOfRangeException`. Throw an exception if the minimal length is greater than 1000 as well.

The generator accepts a boolean parameter `replaceSpecialCharacters`. If this parameter is true, the generator must do the following replacements:

* 'a' and 'A' are replaced by '@'
* 's' and 'S' are replaced by '$'
* 'o' and 'O' are replaced by '0'
* 'i' and 'I' are replaced by '!'
* 'e' and 'E' are replaced by '3'
* 't' and 'T' are replaced by '7'

The Password Generator must be in a static class and the generation method must be static as well. Both must be public. Please choose meaningful names for the class and the method.

The different processing steps must be implemented in separate `internal` static methods. Make them `InternalsVisibleTo` for testing.

Write xunit unit tests in the project [LogicTests](../LogicTests/).

