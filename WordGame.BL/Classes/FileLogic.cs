using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordGame.BL.Interfaces;

namespace WordGame.BL.Classes
{
   public class FileLogic : IFileLogic
   {
      public IList<string> ReadWordsFromFile(string filePath)
      {
         try
         {
            return File.ReadAllLines(filePath).Select(x => x.ToLower()).ToList();
         }
         catch (FileNotFoundException)
         {
            Console.WriteLine($"File at path {filePath} not found.");
            return new List<string>();
         }
         catch (Exception e)
         {
            Console.WriteLine($"Exception while trying to read file at path {filePath}, error message: {e.Message}");
            return new List<string>();
         }
      }

      public void WriteWordsToFile(string filePath, IList<string> words)
      {
         if (!File.Exists(filePath))
         {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(filePath))
            {
               WriteToFile(sw, words);
            }
         }
         else
         {
            // Append to the already existing file
            using (StreamWriter sw = File.AppendText(filePath))
            {
               WriteToFile(sw, words);
            }
         }
      }
      private void WriteToFile(StreamWriter sw, IList<string> words)
      {
         foreach (string word in words)
         {
            if (!string.IsNullOrWhiteSpace(word))
            {
               sw.WriteLine(word);
            }
         }
      }
   }
}
