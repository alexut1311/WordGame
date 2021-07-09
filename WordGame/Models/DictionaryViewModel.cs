using System.Collections.Generic;

namespace WordGame.Models
{
   public class DictionaryViewModel
   {
      public IList<string> CurrentWords { get; set; }
      public string NewWords { get; set; }
   }
}
