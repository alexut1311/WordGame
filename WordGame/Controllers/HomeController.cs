using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using WordGame.BL.Interfaces;
using WordGame.Models;

namespace WordGame.Controllers
{
   public class HomeController : Controller
   {
      private readonly IDictionaryLogic _dictionaryLogic;
      private readonly string _dictionaryFilePath = @"Dictionaries\dictionary.txt";
      private readonly string _weCantSpellDictionaryFilePath = @"WeCantSpellDictionary\en_US.dic";
      public HomeController(IDictionaryLogic dictionaryLogic)
      {
         _dictionaryLogic = dictionaryLogic;
      }

      public IActionResult Index()
      {
         return View("Index");
      }

      [HttpGet]
      public IActionResult Dictionary()
      {
         IList<string> dictionaryWords = _dictionaryLogic.GetWordsFromDictionary(_dictionaryFilePath);
         DictionaryViewModel model = new DictionaryViewModel
         {
            CurrentWords = dictionaryWords
         };
         return View("Dictionary", model);
      }

      [HttpPost]
      public IActionResult Dictionary(DictionaryViewModel model)
      {
         _dictionaryLogic.AddWordsFromDictionary(_dictionaryFilePath, model.NewWords);
         return RedirectToAction("Dictionary", "Home");
      }

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

      [HttpPost]
      public IActionResult PlayGame([FromBody] PlayViewModel playViewModel)
      {
         IList<string> dictionaryWords = _dictionaryLogic.GenerateWordPermutations(playViewModel.InputWordValue,
            playViewModel.UseCustomDictionary ? _dictionaryFilePath : _weCantSpellDictionaryFilePath, playViewModel.UseCustomDictionary);
         return Json(dictionaryWords);
      }
   }
}
