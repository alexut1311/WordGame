using System.Collections.Generic;

namespace WordGame.BL.Interfaces
{
   public interface IDictionaryLogic
   {
      IList<string> GetWordsFromDictionary(string filePath);
      void AddWordsFromDictionary(string filePath, string newWords);
      IList<string> GenerateWordPermutations(string word, string dictionaryFilePath, bool isCustomDictionary);
   }
}
