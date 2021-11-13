using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringParsing
{
    public class StringParser : IStringParser
    {
        private string charactersToRemove = "_4";
        private int maxLength = 15; 

        public IEnumerable<string> ParseStrings(IEnumerable<string> input)
        {
            if(input == null)
            {
                throw new ArgumentException("Input list of strings cannot be null");
            }

            var output = new List<string>();
            string currentString;

            foreach(var str in input)
            {
                //skips the input string if it's null or empty
                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }

                currentString = RemoveForbiddenCharacters(str);
                currentString = RemoveConsecutiveCharacters(currentString);
                currentString = TruncateStringToMaxLength(currentString);
                currentString = ReplaceDollarWithPound(currentString);

                //don't add processed string if the result is empty
                if (string.IsNullOrWhiteSpace(currentString))
                {
                    continue;
                }

                output.Add(currentString);
            }

            if(output.Count == 0)
            {
                throw new ArgumentException("Input provided does not have any output after processing");
            }

            return output;
        }

        private string RemoveForbiddenCharacters(string input)
        {
            foreach(var character in charactersToRemove)
            {
                input = input.Replace(character.ToString(), string.Empty);
            }
            return input;
        }

        private string RemoveConsecutiveCharacters(string input)
        {
            char justAddedChar = new char();
            var builder = new StringBuilder();

            foreach(var character in input)
            {
                if (character != justAddedChar)
                {
                    builder.Append(character);
                }

                justAddedChar = character;
            }
            return new string(builder.ToString());
        }

        private string TruncateStringToMaxLength(string input)
        {
            if(input.Length > maxLength)
            {
                return input.Substring(0, maxLength);
            }
            return input;
        }

        private string ReplaceDollarWithPound(string input)
        {
            return input.Replace('$','£');
        }
    }
}
