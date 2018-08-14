using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    public class TestApp
    {
        private readonly Action<string> _output;
        private readonly Func<string> _input;
        private const char NotGuessedChar = '_';
        private string[] _wordsToGuess = new[]
        {
            "army", "panoramic", "start", "cagey", "null", "vest", "channel", "battle", "circle", "excellent",
            "secretive", "car"
        };


        public TestApp(Action<string> output, Func<string> input)
        {
            _output = output;
            _input = input;
            _output("Welcome to TestApp game");
        }

        public void Start()
        {

            while (true)
            {
                var currentWord = _wordsToGuess.FirstOrDefault();
                if (currentWord == null)
                {
                    _output("You've guessed all the words! Bye!");
                    break;
                }

                var currentlyGuessed = GenerateEmptyResult(currentWord.Length);
                var misplacedCharsSuggestions = new List<int>();

                while (currentlyGuessed != currentWord)
                {
                    var hint = $"Current word consists of {currentWord.Length} letters";
                    var misplacedCharsHint = string.Empty;
                    foreach (var suggestion in misplacedCharsSuggestions)
                    {
                        var charIndex = suggestion;
                        var charAtIndex = currentWord[charIndex];
                        misplacedCharsHint += $"Character \"{charAtIndex} should be somewhere";
                        misplacedCharsHint += Environment.NewLine;
                    }

                    if (misplacedCharsHint != string.Empty)
                        hint += Environment.NewLine + misplacedCharsHint;

                    _output(hint);

                    var input = _input();

                    currentlyGuessed = EvaluateResult(input, currentWord, currentlyGuessed, misplacedCharsSuggestions);
                    _output(currentlyGuessed);
                }

                _output("Nice job. Moving to the next word");

                //Remove first wort in the array
                var currentWordList = _wordsToGuess.ToList();
                currentWordList.RemoveAt(0);
                _wordsToGuess = currentWordList.ToArray();
            }
        }

        private string EvaluateResult(string input, string wordToGuess, string currentlyGuessed, List<int> misplacedChars)
        {
            if (input.Length != wordToGuess.Length)
            {
                _output("Inserted word length doesn't even match the length requested");
                return currentlyGuessed;
            }

            var currentlyGuessedChars = currentlyGuessed.ToCharArray();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == wordToGuess[i] && input[i] != currentlyGuessedChars[i])
                {
                    currentlyGuessedChars[i] = input[i];
                    if (misplacedChars.Contains(i))
                        misplacedChars.Remove(i);
                }
            }

            //To add misplaced character suggestions

            //Create not guessed chars mask 
            var notGuessedCharsMask = wordToGuess.ToCharArray();

            for (int i = 0; i < currentlyGuessedChars.Length; i++)
            {
                if (currentlyGuessedChars[i] != NotGuessedChar)
                {
                    notGuessedCharsMask[i] = NotGuessedChar;
                }
            }


            UpdateMisplacedCharSuggestions(misplacedChars, input, notGuessedCharsMask);


            return new string(currentlyGuessedChars);
        }

        private void UpdateMisplacedCharSuggestions(List<int> misplacedChars, string input, char[] charsLeftToGuess)
        {
            for (var i = 0; i < charsLeftToGuess.Length; i++)
            {
                var inputChar = charsLeftToGuess[i];
                if (input.Any(c => c == inputChar))
                    if (misplacedChars.All(ind => ind != i))
                        misplacedChars.Add(i);
            }
        }


        private string GenerateEmptyResult(int lenght)
        {
            return new string(NotGuessedChar, lenght);
        }
    }
}