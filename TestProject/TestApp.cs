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

                var misplacedCharsSuggestionsSoFar = new List<int>();
                var misplacedCharsSuggestions = new MisplacedCharInfoHolder(index => misplacedCharsSuggestionsSoFar.Add(index));
                var cheatEngine = new CheatEngine();

                while (currentlyGuessed != currentWord)
                {
                    var hint = $"Current word consists of {currentWord.Length} letters";
                    var misplacedCharsHint = string.Empty;
                    foreach (var suggestion in misplacedCharsSuggestions.MisplacedCharInfo)
                    {
                        var charIndex = suggestion;
                        var charAtIndex = currentWord[charIndex];
                        misplacedCharsHint += $"Character \"{charAtIndex} should be somewhere";
                        misplacedCharsHint += Environment.NewLine;
                    }

                    if (misplacedCharsHint != string.Empty)
                        hint += Environment.NewLine + misplacedCharsHint;

                    _output(hint);

                    _output("Please input your guess:");
                    var input = _input();

                    currentWord = cheatEngine.ChangeCurrentWordWithHarderOneIfPossible(currentWord, currentlyGuessed, input, _wordsToGuess,
                        misplacedCharsSuggestionsSoFar, UpdateCurrentlyGuessed);

                    currentlyGuessed = EvaluateResult(input, currentWord, currentlyGuessed, misplacedCharsSuggestions).Item1;
                    _output(currentlyGuessed);
                }

                _output("Nice job. Moving to the next word");

                var currentWordList = _wordsToGuess.ToList();
                currentWordList.Remove(currentWord);
                _wordsToGuess = currentWordList.ToArray();
            }
        }

        private Tuple<string, int> EvaluateResult(string input, string wordToGuess, string currentlyGuessed, MisplacedCharInfoHolder misplacedChars)
        {
            if (input.Length != wordToGuess.Length)
            {
                _output("Inserted word length doesn't even match the length requested");
                return new Tuple<string, int>(currentlyGuessed, 0);
            }

            var currentlyGuessedChars = currentlyGuessed.ToCharArray();

            var charactersGuessed = UpdateCurrentlyGuessed(input, wordToGuess, misplacedChars, currentlyGuessedChars);

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


            return new Tuple<string, int>(new string(currentlyGuessedChars), charactersGuessed);
        }

        private static int UpdateCurrentlyGuessed(string input, string wordToGuess, MisplacedCharInfoHolder misplacedChars,
            char[] currentlyGuessedChars)
        {
            var changedChars = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == wordToGuess[i] && input[i] != currentlyGuessedChars[i])
                {
                    currentlyGuessedChars[i] = input[i];
                    changedChars++;
                    //TODO:Should refactor: take this task outside of the method
                    misplacedChars.RemoveIfContains(i);
                }
            }

            return changedChars;
        }

        private void UpdateMisplacedCharSuggestions(MisplacedCharInfoHolder misplacedChars, string input, char[] charsLeftToGuess)
        {
            for (var i = 0; i < charsLeftToGuess.Length; i++)
            {
                var inputChar = charsLeftToGuess[i];
                if (input.Any(c => c == inputChar))
                    misplacedChars.AddIfNotAlreadyExists(i);
            }
        }


        private string GenerateEmptyResult(int lenght)
        {
            return new string(NotGuessedChar, lenght);
        }
    }
}