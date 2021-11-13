using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StringParsing.UnitTests
{
    public class StringParser_ParseStringsTests
    {
        [Theory]
        [InlineData("wasd_", "wasd")]
        [InlineData("wasd4", "wasd")]
        [InlineData("_wasd4", "wasd")]
        public void WhenPassedAStringWithForbiddenChars_DeletesChars(string inputstring, string expectedOutput)
        {
            StringParser Sut = new StringParser();

            var input = new List<string> { inputstring };

            var result = Sut.ParseStrings(input);

            result.First().Should().Be(expectedOutput);

        }

        [Theory]
        [InlineData("AAA","A")]
        [InlineData("AAAc91%cWwW", "Ac91%cWwW")]
        [InlineData("AaAaAaAaAaAa", "AaAaAaAaAaAa")]
        public void WhenPassedAStringWithDuplicateChars_RemovesDuplicates(string inputstring, string expectedOutput)
        {
            StringParser Sut = new StringParser();

            var input = new List<string> { inputstring };

            var result = Sut.ParseStrings(input);

            result.First().Should().Be(expectedOutput);

        }

        [Theory]
        [InlineData("123123123123123", "123123123123123")] // 15 characters exactly
        [InlineData("123123123123123123123", "123123123123123")] //21 characters
        [InlineData("4_4_4_111111123123123123123123123", "123123123123123")] //21 characters with forbidden and duplicate characters
        public void WhenPassedAStringLongerThan15Chars_TruncatesString(string inputstring, string expectedOutput)
        {
            StringParser Sut = new StringParser();

            var input = new List<string> { inputstring };

            var result = Sut.ParseStrings(input);

            result.First().Should().Be(expectedOutput);

        }

        [Theory]
        [InlineData("$", "£")]
        [InlineData("123$", "123£")]
        [InlineData("_$4$4$_", "£")]
        public void WhenPassedAStringWithDollarSign_ReplacesDollarWithPound(string inputstring, string expectedOutput)
        {
            StringParser Sut = new StringParser();

            var input = new List<string> { inputstring };

            var result = Sut.ParseStrings(input);

            result.First().Should().Be(expectedOutput);

        }

        [Fact]
        public void WhenPassedAStringWithMultipleViolations_ShouldReturnProcessedString()
        {
            StringParser Sut = new StringParser();

            var input = new List<string> { "AAAc91%cWwWkLq$1ci3_848v3d__K" };

            var result = Sut.ParseStrings(input);

            result.First().Should().Be("Ac91%cWwWkLq£1c");
        }


        [Theory]
        [InlineData("4_4_4")]
        [InlineData("_____")]
        public void WhenProcessedStringIsEmpty_ThrowsArgumentException(string inputstring)
        {
            StringParser Sut = new StringParser();
            var input = new List<string> { inputstring };

            Action act = () => Sut.ParseStrings(input);

            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("Input provided does not have any output after processing", exception.Message);
        }

        [Fact]
        public void WhenPassedNullList_ThrowsArgumentException()
        {
            StringParser Sut = new StringParser();

            Action act = () => Sut.ParseStrings(null);

            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("Input list of strings cannot be null", exception.Message);
        }

        [Fact]
        public void WhenNotAllStringsReturnAValue_ShouldOnlyReturnStringsWithValues()
        {
            StringParser Sut = new StringParser();

            var input = new List<string> { "AAAc91%cWwWkLq$1ci3_848v3d__K" , "44444", "____444__4", "$$$$$$" };

            var result = Sut.ParseStrings(input);
            
            result.Should().HaveCount(2);
            result.Should().Contain("Ac91%cWwWkLq£1c");
            result.Should().Contain("£");

        }
    }
}
