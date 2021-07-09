using System.Collections.Generic;

namespace WordGame.BL.Interfaces
{
   public interface IFileLogic
   {
      IList<string> ReadWordsFromFile(string filePath);
      void WriteWordsToFile(string filePath, IList<string> words);
   }
}
