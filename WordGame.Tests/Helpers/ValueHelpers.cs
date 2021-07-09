using System;
using System.Collections.Generic;

namespace WordGame.Tests.Helpers
{
   internal class ValueHelpers
   {
      internal static IList<string> GetWords()
      {
         return new List<string>
         {
            "word",
            "another",
            "test"
         };
      }

      internal static string GetStringToSplit()
      {
         return "word for another test";
      }

      internal static IList<string> GetDictionaryWords()
      {
         return new List<string>
         {
            "et",
            "te",
            "eh",
            "he",
            "het",
            "eth",
            "the"
         };
      }
   }
}
