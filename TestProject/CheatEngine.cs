using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    public class CheatEngine
    {
        public static string ChangeCurrentWordWithHarderOneIfPossible(string currentWord, string wordGuessedSoFar,
            string currentInput, string[] allWords, List<int> misplacedCharsSuggestionsSoFar,
            Func<string, string, MisplacedCharInfoHolder, char[], int> evaluateResult
            //string input, string wordToGuess, MisplacedCharInfoHolder misplacedChars,char[] currentlyGuessedChars
        )
        {
            //We can't change the word with one with different length
            var changeableWords = allWords.AsQueryable()
                //.Except<string>(new[] { currentWord })
                .Except(allWords.Where(w => w.Length != currentWord.Length));

            var stillChangeableWordsAfterSomeCharsGuessed =
                changeableWords.Where(w => MatchesGuessedPattern(w, wordGuessedSoFar));

            //TODO:
            We need to check also for words which may have guessed (if We didnnot change the word)

            //TODO:Naming
            var stillChangeableWordsAfterSuggestionsGiven = stillChangeableWordsAfterSomeCharsGuessed.Where(w =>
                //The characters we told user about that exists in a word
                misplacedCharsSuggestionsSoFar.All(
                    //still appear in next word
                    index => currentWord[index] == w[index])
            );

    
            var worstWordsToChange = stillChangeableWordsAfterSuggestionsGiven.ToList()
                .OrderBy(
                    w => evaluateResult(currentInput, w, new MisplacedCharInfoHolder(null),
                        wordGuessedSoFar.ToCharArray())

                ).ToList();


            return worstWordsToChange.First();
        }
        //Checks if the characters already guessed by the user still appear in s wordToChangeWith
        private static bool MatchesGuessedPattern(string wordToChangeWith, string wordGuessedSoFar)
        {
            for (int i = 0; i < wordGuessedSoFar.Length; i++)
            {
                var current = wordGuessedSoFar[i];
                if (current != '_' && current != wordToChangeWith[i]) //TODO:use const
                    return false;
            }

            return true;
        }
    }
}