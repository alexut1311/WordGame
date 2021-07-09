using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeCantSpell.Hunspell;
using WordGame.BL.Interfaces;

namespace WordGame.BL.Classes
{
   public class DictionaryLogic : IDictionaryLogic
   {
      private readonly IFileLogic _fileLogic;
      private static readonly IList<string> _singleLettersWords = new List<string> { "a", "i", "o" };

      public DictionaryLogic(IFileLogic fileLogic)
      {
         _fileLogic = fileLogic;
      }

      public IList<string> GetWordsFromDictionary(string filePath)
      {
         return _fileLogic.ReadWordsFromFile(filePath);
      }

      public void AddWordsFromDictionary(string filePath, string newWords)
      {
         string[] words = newWords.Split(' ');
         _fileLogic.WriteWordsToFile(filePath, words);
      }

      public IList<string> GenerateWordPermutations(string word, string dictionaryFilePath, bool isCustomDictionary)
      {
         word = word.ToLower();
         IDictionary<string, string> wordCharacters = SplitWordInCharacters(word);
         IList<string> result = WordPermutations(wordCharacters);
         IList<string> acceptedWords = CheckWords(result, dictionaryFilePath, isCustomDictionary);

         return acceptedWords;
      }

      private IDictionary<string, string> SplitWordInCharacters(string word)
      {
         IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();
         char[] charArr = word.ToCharArray();
         for (int i = 0; i < charArr.Length; i++)
         {
            keyValuePairs.Add(i.ToString(), charArr[i].ToString());
         }
         return keyValuePairs;
      }

      private IList<string> WordPermutations(IDictionary<string, string> characters)
      {
         IDictionary<string, string> combinations = new Dictionary<string, string>();
         IList<string> result = new List<string>();
         combinations = characters;
         foreach (KeyValuePair<string, string> combination in combinations)
         {
            if (_singleLettersWords.Contains(combination.Value))
            {
               result.Add(combination.Value);
            }
         }

         for (int i = 0; i < characters.Count; i++)
         {
            IDictionary<string, string> temporaryCombinations = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> character in characters)
            {
               foreach (KeyValuePair<string, string> combination in combinations)
               {
                  if (!combination.Key.Contains(character.Key))
                  {
                     temporaryCombinations.Add(character.Key + combination.Key, combination.Value + character.Value);
                  }
               }
            }

            Parallel.ForEach(temporaryCombinations, keyValuePaire =>
            {
               result.Add(keyValuePaire.Value);
            });

            combinations = temporaryCombinations;
         }

         return result.Distinct().ToList();
      }

      private IList<string> CheckWords(IList<string> words, string dictionaryFilePath, bool isCustomDictionary)
      {
         IList<string> result = new List<string>();

         if (isCustomDictionary)
         {
            IList<string> dictionaryWords = _fileLogic.ReadWordsFromFile(dictionaryFilePath);

            if (dictionaryWords.Count == 0)
            {
               return new List<string>();
            }

            Parallel.ForEach(words, word =>
            {
               if (dictionaryWords.Contains(word))
               {
                  result.Add(word);
               }
            });
         }
         else
         {
            Parallel.ForEach(words, word =>
            {
               if (CheckSpelling(word, dictionaryFilePath))
               {
                  result.Add(word);
               }
            });
         }

         return result;
      }

      private bool CheckSpelling(string content, string dictionaryFilePath)
      {
         try
         {
            WordList dictionary = WordList.CreateFromFiles(dictionaryFilePath);
            bool ok = dictionary.Check(content);
            return ok;
         }
         catch (FileNotFoundException)
         {
            Console.WriteLine($"File at path {dictionaryFilePath} not found.");
            return false;
         }
         catch (Exception e)
         {
            Console.WriteLine($"Exception while trying to read file at path {dictionaryFilePath}, error message: {e.Message}");
            return false;
         }
      }
   }
}
